using System;

namespace TestSolution.TestProject
{
    using System.Linq;

    public class SplitNamespaces
    {
        public DateTime[] ImInSystem { get; set; }

        public DateTime NeedLinq()
        {
            return ImInSystem.FirstOrDefault();
        }
    }
}