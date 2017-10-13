using System;
using System.Linq;

namespace TestSolution.TestProjectSuccess
{
    public class SplitNamespaces
    {
        public DateTime[] ImInSystem { get; set; }

        public DateTime NeedLinq()
        {
            return ImInSystem.FirstOrDefault();
        }
    }
}