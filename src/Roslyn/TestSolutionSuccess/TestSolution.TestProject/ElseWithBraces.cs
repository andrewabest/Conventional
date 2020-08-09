using System;

namespace TestSolution.TestProjectSuccess
{
    public class ElseWithBraces
    {
        public int Main(int input)
        {
            if (input == 0)
            {
                throw new ArgumentException();
            }
            else
            {
                return input * 2;
            }
        }
    }
}