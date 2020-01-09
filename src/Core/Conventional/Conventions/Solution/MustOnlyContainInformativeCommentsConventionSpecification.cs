using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conventional.Extensions;

namespace Conventional.Conventions.Solution
{
    public class MustOnlyContainInformativeCommentsConventionSpecification : SolutionConventionSpecification
    {
        private readonly string[] _permittedCommentDelimiters;
        private readonly string[] _fileExemptions;
        private readonly string _fileSearchPattern;

        private const string UrlPattern = @"https?:\/\/([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?";
        private const string CommentPattern = @"(?!{0}|(?=\/\/ ReSharper))((?<urldudes>" + UrlPattern + @")|(?<cooldudes>\/\/\/)|(?<badguys>\/\/.+))";

        /// <param name="permittedCommentDelimiters">Eg "Note", "Todo". Case insensitive.</param>
        /// <param name="fileExemptions">Eg "AssembliInfo.cs". Case insensitive.</param>
        /// <param name="fileSearchPattern">Eg "*.cs"</param>
        public MustOnlyContainInformativeCommentsConventionSpecification(string[] permittedCommentDelimiters, string[] fileExemptions, string fileSearchPattern)
        {
            _permittedCommentDelimiters = permittedCommentDelimiters;
            _fileExemptions = fileExemptions;
            _fileSearchPattern = fileSearchPattern;
        }

        protected override string FailureMessage
        {
            get
            {
                return
                    "The solution must only contain comments with the following delimiters in the format [// Todo:]." + Environment.NewLine + 
                    "Permitted Delimiters: {0}".FormatWith(_permittedCommentDelimiters.Aggregate((s, s1) => s + "," + s1));
            }
        }

        public override ConventionResult IsSatisfiedBy(string solutionRoot)
        {
            var failures = new List<string>();

            foreach (var filePath in DirectoryEx.GetFilesExceptOutput(solutionRoot, _fileSearchPattern).Where(x => _fileExemptions.Any(x.EndsWith) == false))
            {
                var fileContents = File.ReadAllText(filePath);
                var matches = Regex.Matches(
                    fileContents,
                    CommentPattern.Replace("{0}", BuildPermittedComments(_permittedCommentDelimiters)),
                    RegexOptions.IgnoreCase)
                    .Cast<Match>()
                    .ToArray();

                if (matches.Any(x => x.Groups["badguys"].Success))
                {
                    var failureText =
                        matches
                            .Where(x => x.Groups["badguys"].Success)
                            .SelectMany(x => x.Groups["badguys"].Captures.Cast<Capture>())
                            .Aggregate(
                                filePath + Environment.NewLine,
                                (current, capture) => current + "Line " + DeriveLineNumber(fileContents, capture.Index) + ": " + capture.Value)
                            .Trim();

                    failures.Add(failureText);
                }
            }

            if (failures.Any())
            {
                return ConventionResult.NotSatisfied(
                    SolutionConventionResultIdentifier,
                    BuildFailureMessage(failures.Aggregate(string.Empty, (result, input) => result + Environment.NewLine + input).Trim()));
            }

            return ConventionResult.Satisfied(SolutionConventionResultIdentifier);
        }

        private static int DeriveLineNumber(string contents, int index)
        {
            return contents.Take(index + 1).Count(x => x == '\n');
        }

        private const string PermittedCommentFormat = @"(?=\/\/ {0}:)";
        private static string BuildPermittedComments(string[] definitions)
        {
            return definitions.Aggregate(string.Empty,
                (result, input) => result + "|" + PermittedCommentFormat.FormatWith(input)).TrimStart('|');
        }
    }
}