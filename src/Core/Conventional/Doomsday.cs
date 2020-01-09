using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional
{
    public static class Doomsday
    {
        public static TypeConformanceSpecimen WithKnownOffenders(this Type type, int offenders)
        {
            return new TypeConformanceSpecimen(type).WithKnownOffenders(offenders);
        }

        public static TypeConformanceSpecimen WithKnownOffenders(this IEnumerable<Type> types, int offenders)
        {
            return new TypeConformanceSpecimen(types).WithKnownOffenders(offenders);
        }

        public static TypeConformanceSpecimen ByDoomsday(this Type type, DateTime doomsday)
        {
            return new TypeConformanceSpecimen(type).ByDoomsday(doomsday);
        }

        public static TypeConformanceSpecimen ByDoomsday(this IEnumerable<Type> types, DateTime doomsday)
        {
            return new TypeConformanceSpecimen(types).ByDoomsday(doomsday);
        }

        public static void MustConformTo(this TypeConformanceSpecimen specimen, IConventionSpecification conventionSpecification)
        {
            if (ConventionConfiguration.DefaultFailureAssertionCallback == null)
            {
                throw new Exception("You must configure a default failure assertion when using Doomsday tests");
            }

            if (ConventionConfiguration.DefaultWarningAssertionCallback == null)
            {
                throw new Exception("You must configure a default warning assertion when using Doomsday tests");
            }

            var evaluatedResults = specimen.Types.Select(conventionSpecification.IsSatisfiedBy).ToList();

            if (evaluatedResults.All(x => x.IsSatisfied))
            {
                return;
            }

            var currentDate = ConventionConfiguration.DefaultCurrentDateResolver();
            var currentOffenders = evaluatedResults.Count(x => x.IsSatisfied == false);

            var result =
                evaluatedResults.Where(x => x.IsSatisfied == false).Aggregate(string.Empty, (s, x) =>
                    s +
                    x.SubjectName +
                    Environment.NewLine +
                    StringConstants.Underline +
                    Environment.NewLine +
                    x.Failures.Aggregate(string.Empty, (s1, s2) => s1 + s2 + Environment.NewLine) +
                    Environment.NewLine);

            if (specimen.Doomsday.HasValue && currentDate > specimen.Doomsday)
            {
                var message = "Doomsday is upon us! " + specimen.Message + Environment.NewLine + result;

                ConventionConfiguration.DefaultFailureAssertionCallback(message);
            }
            else if (currentOffenders > specimen.KnownOffenders)
            {
                var message = $"Expected {specimen.KnownOffenders} or less offenders but found {currentOffenders}: " + specimen.Message + Environment.NewLine + result;

                ConventionConfiguration.DefaultFailureAssertionCallback(message);
            }
            else if (specimen.WarnWithin.HasValue && currentDate.Add(specimen.WarnWithin.Value) > specimen.Doomsday)
            {
                var message = "Doomsday approaches! " + specimen.Message + Environment.NewLine + result;

                ConventionConfiguration.DefaultWarningAssertionCallback(message);
            }
        }
    }
}