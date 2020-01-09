using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional
{
    public class TypeConformanceSpecimen
    {
        public TypeConformanceSpecimen(Type type)
        {
            Types = new[] { type };
        }

        public TypeConformanceSpecimen(IEnumerable<Type> types)
        {
            Types = types.ToArray();
        }

        public Type[] Types { get; }
        public int? KnownOffenders { get; private set; }
        public DateTime? Doomsday { get; private set; }
        public TimeSpan? WarnWithin { get; private set; }
        public string Message { get; private set; }

        public TypeConformanceSpecimen WithKnownOffenders(int knownOffenders)
        {
            KnownOffenders = knownOffenders;
            return this;
        }

        public TypeConformanceSpecimen ByDoomsday(DateTime doomsday)
        {
            Doomsday = doomsday;
            return this;
        }

        public TypeConformanceSpecimen WithWarningWithin(TimeSpan warnWithin)
        {
            if (Doomsday.HasValue == false)
            {
                throw new Exception("Doomsday must be set before using a warning.");
            }

            WarnWithin = warnWithin;
            return this;
        }

        public TypeConformanceSpecimen WithMessage(string message)
        {
            Message = message;
            return this;
        }
    }
}