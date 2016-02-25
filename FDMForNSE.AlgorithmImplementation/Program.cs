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
            var xInterval       = new Interval { Start = 0.0, End = 10.0 };
            var tInterval       = new Interval { Start = 0.0, End = 10.0 };

            var net             = new Net { XStepsCount = 20, TStepsCount = 70 };

            var initCondFunc    = new InitConditions
                                    {
                                        FiOfX   = (double x) => { return Math.Exp(-x * x); },
                                        PsiOfT0 = (double t) => { return 0.0; },
                                        PsiOfTL = (double t) => { return 0.0; }
                                    };

            var nseSolver       = new NseSolver(xInterval, tInterval, initCondFunc, net);
            
            PrintApproximateSolution(nseSolver.GetApproximateSolution(1));
            PrintApproximateSolution(nseSolver.GetApproximateSolution(2));
            PrintApproximateSolution(nseSolver.GetApproximateSolution(3));

            PrintApproximateSolution(nseSolver.GetApproximateSolution(4));
            PrintApproximateSolution(nseSolver.GetApproximateSolution(5));
        }

        public static void PrintApproximateSolution(ApproximationPoint[] solution)
        {
            Console.WriteLine("{0,30}", "Approximate solution");
            Console.WriteLine("{0,10}{1,10}", "x", "|U|");
            Console.WriteLine("------------------------------");

            foreach (var approxPoint in solution)
            {
                Console.WriteLine("{0,10:F3}{1,10:F3}", approxPoint.X, approxPoint.U.Magnitude);
            }
        }
    }
}
