using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolution.TestProject
{
    public class IfBracelessWonder
    {
        public int Main(int input)
        {
            if (input == 0) throw new ArgumentException();
            else
            {
                return input*2;
            }
        }
    }
}
