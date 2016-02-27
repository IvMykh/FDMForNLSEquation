using System;
using System.Collections.Generic;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using FiZero = Func<double, double>;

    public class Program
    {
        public static void Main(string[] args)
        {
            //TestNlseSolverIterator(NlseSolver.DefaultSolver);
            PrintApproximateSolution(NlseSolver.DefaultSolver.GetApproximateSolution(20000));
        }

        public static void PrintApproximateSolution(IEnumerable<ApproximationPoint> solution)
        {
            Console.WriteLine("{0,20}", "Approximate solution");
            Console.WriteLine("{0,10}{1,10}", "x", "|U|");
            Console.WriteLine("------------------------------");

            foreach (var approxPoint in solution)
            {
                Console.WriteLine("{0,10:F3}{1,10:F3}", approxPoint.X, approxPoint.U.Magnitude);
            }
        }

        public static void TestNlseSolverIterator(NlseSolver nlseSolver)
        {
            var solutionsEnumerator = nlseSolver.SequenceOfApproximations().GetEnumerator();
            var shouldProceed       = solutionsEnumerator.MoveNext();

            var count = 1;
            for (int i = 0; i < count && shouldProceed; ++i)
            {
                if (i == count - 1)
                {
                    Console.WriteLine("***Solution {0}***", i + 1);
                    PrintApproximateSolution(solutionsEnumerator.Current);
                    Console.WriteLine("\n\n");
                }

                shouldProceed = solutionsEnumerator.MoveNext();
            }
        }
    }
}
