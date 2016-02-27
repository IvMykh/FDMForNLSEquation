using System;
using System.Collections.Generic;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using F         = Func<double, double>;
    using ComplF    = Func<double, Complex>;

    public class NlseSolver
    {
        // static members;
        private static NlseSolver defaultSolver;

        // TODO: deal with too great residual (perhaps there is a bug in computation scheme -> remake);
        static NlseSolver()
        {
            var xInterval       = new Interval  { Start = -5.0,  End = 25.0 };
            var tInterval       = new Interval  { Start = 0.0,  End = 10.0 };

            var net             = new Net       { XStep = 0.05, TStep = 0.00001 };
            var fiOfX           = new ComplF((double x) => 
                {
                    const double SQRT_2 = 1.4142135623730950488016887242097;
                    return SQRT_2 * 
                        (Complex.Exp((x - 5.0) / 2.0 * Complex.ImaginaryOne) / Math.Cosh(x - 5.0) +
                        Complex.Exp((x - 15.0) / 2.0 * Complex.ImaginaryOne) / Math.Cosh(x - 15.0)); 
                });

            var initCondFunc    = new InitConditions
                                    {
                                        FiOfX   = fiOfX,
                                        PsiOfT0 = (double t) => { return 0; },
                                        PsiOfTL = (double t) => { return 0; }
                                    };

            defaultSolver       = new NlseSolver(xInterval, tInterval, initCondFunc, net);
        }
        
        public static NlseSolver DefaultSolver
        {
            get { return defaultSolver; }
        }


        // instance members;
        public Interval                     XInterval       { get; set; }
        public Interval                     TInterval       { get; set; }
        public Net                          Net             { get; set; }

        public InitConditions               InitConds       { get; set; }

        public NlseSolver(Interval xInterval, Interval tInterval, InitConditions initConds, Net net)
        {
            this.XInterval      = xInterval;
            this.TInterval      = tInterval;
            this.InitConds      = initConds;
            this.Net            = net;
        }

        private ApproximationPoint[]                getInitApproximation()
        {
            var approxPointsCount = (int)((XInterval.End - XInterval.Start) / Net.XStep);

            var initApproxPoints = new ApproximationPoint[approxPointsCount + 1];
            double x = XInterval.Start;

            for (int i = 0; i < initApproxPoints.Length; ++i)
            {
                initApproxPoints[i] = new ApproximationPoint
                {
                    X = x,
                    U = InitConds.FiOfX(x)
                };

                x += Net.XStep;
            }

            #region //Start and end values.
            /*initApproxPoints[0] = new ApproximationPoint
            {
                X = XInterval.Start,
                U = InitConds.PsiOfT0(XInterval.Start)
            };
            
            initApproxPoints[initApproxPoints.Length - 1] = new ApproximationPoint
            {
                X = XInterval.End,
                U = InitConds.PsiOfTL(XInterval.End)
            };*/
            #endregion

            return initApproxPoints;
        }

        private ApproximationPoint[]                getNextApproximation(ApproximationPoint[] approxPointsPrev)
        {
            var approxPointsCount   = (int)((XInterval.End - XInterval.Start) / Net.XStep);

            var approxPointsNext    = new ApproximationPoint[approxPointsCount + 1];
            approxPointsNext[0]     = new ApproximationPoint
            {
                X = XInterval.Start,
                U = InitConds.PsiOfT0(XInterval.Start)
            };

            approxPointsNext[approxPointsNext.Length - 1] = new ApproximationPoint
            {
                X = XInterval.End,
                U = InitConds.PsiOfTL(XInterval.End)
            };

            double x            = XInterval.Start;
            double xStepSquare  = Net.XStep * Net.XStep;

            for (int i = 1; i < approxPointsNext.Length - 1; ++i)
            {
                var Ui = approxPointsPrev[i].U;

                var u = 2.0 * Complex.ImaginaryOne * Net.TStep * (
                    (approxPointsPrev[i + 1].U - Ui - Ui + approxPointsPrev[i - 1].U) / xStepSquare +
                    Ui.Magnitude * Ui.Magnitude * Ui) +
                    Ui;

                approxPointsNext[i] = new ApproximationPoint
                {
                    X = x + i * Net.XStep,
                    U = u
                };

                //x += Net.XStep;
            }

            return approxPointsNext;
        }

        public IEnumerable<ApproximationPoint>      GetApproximateSolution(int timeMoment)
        {
            ApproximationPoint[] approxPointsPrev = getInitApproximation();
            ApproximationPoint[] approxPointsNext = null;
            
            for (int j = 0; j < timeMoment; ++j)
            {
                approxPointsNext = getNextApproximation(approxPointsPrev);
                approxPointsPrev = approxPointsNext;
            }

            return approxPointsPrev;
        }

        public IEnumerable<IEnumerable<ApproximationPoint>> SequenceOfApproximations()
        {
            var approxPointsPrev = getInitApproximation();
            yield return approxPointsPrev;

            for (double tStep = TInterval.Start; tStep <= TInterval.End; tStep += Net.TStep)
            {
                var approxPointsNext = getNextApproximation(approxPointsPrev);
                yield return approxPointsNext;

                approxPointsPrev = approxPointsNext;
            }
        }
    }
}
