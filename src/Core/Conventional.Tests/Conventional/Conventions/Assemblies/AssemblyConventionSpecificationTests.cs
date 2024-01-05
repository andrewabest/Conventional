﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Assemblies
{
    public class AssemblyConventionSpecificationTests
    {
        private Assembly _testAssembly;

        [SetUp]
        public void Setup()
        {
            _testAssembly = Assembly.LoadFrom(KnownPaths.SolutionRoot +
                $"TestSolution{Path.DirectorySeparatorChar}TestSolution.TestProject{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}TestSolution.TestProject.dll");
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_FailsWhenAssemblyReferencesDllsFromBinDirectory()
        {
            var result = _testAssembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_FailsWhenAssemblyReferencesDllsFromReferenceAssembliesDirectory()
        {
            var result = _testAssembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_Success_FileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(".resx"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_Success_RegEx()
        {
            var matchResxFiles = new Regex(@"\.RESX$", RegexOptions.IgnoreCase);

            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(matchResxFiles))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_Regex()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(new Regex(@".\.txt")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_WithWildCardedFileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("*.sql"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_WithNonWildcardedFileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("sql"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_RegEx()
        {
            var matchSqlFiles = new Regex(@"\.SQL$", RegexOptions.IgnoreCase);

            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources(matchSqlFiles))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void
            MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_RegEx()
        {
            var matchNonEmbeddedRegEx = new Regex(".*NON_EMBEDDED.*", RegexOptions.IgnoreCase);

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources(matchNonEmbeddedRegEx));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_FileExtension_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(".svg"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_Regex_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(new Regex(@".+\.svg")));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(".png"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().EndWith("copy-not.png");
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_Regex()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(new Regex(@".*\.png")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().EndWith("copy-not.png");
        }

        private AssemblySpecimen[] TestProjects =>
            AllAssemblies.WithNamesMatching("*")
                .Where(specimen => specimen.ProjectFilePath.Contains("Tests"))
                .ToArray();

        // Note: In practice, this list of assemblies would be used to drive further convention tests (i.e. assembly.GetTypes())
        private static readonly List<Assembly> TestAssemblies = new List<Assembly>
        {
            typeof(DogFoodConventions).Assembly
        };

        [Test]
        public void MustBeIncludedInSetOfAssemblies_Success()
        {
            var result = TestProjects
                .MustConformTo(Convention.MustBeIncludedInSetOfAssemblies(TestAssemblies, "TestAssemblies"));

            // TODO: Use result.Should().AllSatisfy() once we've updated to fluentassertions 6.5.0+
            result.Select(x => x.IsSatisfied).Distinct().Single().Should().BeTrue();
        }

        [Test]
        public void MustBeIncludedInSetOfAssemblies_Failure()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var staleTestAssemblies = new List<Assembly>();

            var result = TestProjects
                .MustConformTo(Convention.MustBeIncludedInSetOfAssemblies(staleTestAssemblies, "TestAssemblies"));

            // TODO: Use result.Should().AllSatisfy() once we've updated to fluentassertions 6.5.0+
            result.Select(x => x.IsSatisfied).Distinct().Single().Should().BeFalse();
        }
    }
}