using System;

namespace TestSolution.TestProject
{
    public class ElseBracelessWonder
    {
        public int Main(int input)
        {
            if (input == 0)
            {
                throw new ArgumentException();
            }
            else return input * 2;
        }
    }
}