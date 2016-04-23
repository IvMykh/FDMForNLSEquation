using System;
using System.Collections.Generic;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using F         = Func<double, double>;
    using ComplF    = Func<double, Complex>;

    public class NlseSolver
    {
        // Static members.
        private static NlseSolver   defaultSolver;

        static NlseSolver()
        {
            var xInterval   = new Interval  { Start = -20.0,    End = 20.0 }; // originally [0, 20], than [-10, 10]
            var tInterval   = new Interval  { Start = 0.0,      End = 10.0 };

            var net         = new Net   //  { XStep = 0.1,     TStep = 0.00005 };
                                        //  { XStep = 0.05,     TStep = 0.00001 }; 
                                          { XStep = 0.1, TStep = 0.0025};

            //double alpha    = 1;
            //double U        = 500.0;
            //double omega    = 0;

            //double amplitude = 0.5;

            Complex i = Complex.ImaginaryOne;
            
            var fiOfX       = new ComplF((double x) => 
                {
                    //double sqrtAlpha = Math.Sqrt(alpha);
                    //double denominator = Math.Cosh(sqrtAlpha * (x - omega));

                    var ampl1 = 1.0;
                    var v1 = 1.0;

                    var ampl2 = 1.7;
                    var v2 = 2;

                    var ampl3 = 0.8;
                    var v3 = 0.8;

                    return
                        //SQRT_2 * sqrtAlpha * Complex.Exp(i * U * x / 2.0) /
                        //(denominator * denominator);

                        SpecData.SQRT_2 *
                        (//ampl1 * Complex.Exp((x) * i * -v1) / (Math.Cosh(ampl1 * (x - 5)))
                       + ampl2 * Complex.Exp((x) * i * v2)  / (Math.Cosh(ampl2 * (x + 5)))
                       + ampl3 * Complex.Exp((x) * i * v3)  / (Math.Cosh(ampl3 * (x))));
                });

            var initCondFunc    = new InitConditions
                                    {
                                        FiOfX   = fiOfX,
                                        PsiOfT0 = (double t) => { return 0.0; },
                                        PsiOfTL = (double t) => { return 0.0; }
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
            this.Net            = net;
            
            this.InitConds      = initConds;
        }

        public NlseSolver(Configuration configuration)
        {
            this.XInterval  = configuration.XInterval;
            this.TInterval  = configuration.TInterval;
            this.Net        = configuration.Net;
            this.InitConds  = configuration.InitConditions;
        }

        private ApproximationPoint[] getInitApproximation()
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
        private ApproximationPoint[] getAfterInitApproximation(ApproximationPoint[] initApproxPoints)
        {
            var afterInitApproxPoints = new ApproximationPoint[initApproxPoints.Length];
            afterInitApproxPoints[0] = new ApproximationPoint
            {
                X = XInterval.Start,
                U = InitConds.PsiOfT0(XInterval.Start)
            };

            afterInitApproxPoints[afterInitApproxPoints.Length - 1] = new ApproximationPoint
            {
                X = XInterval.End,
                U = InitConds.PsiOfTL(XInterval.End)
            };

            double xStepSquare = Net.XStep * Net.XStep;

            for (int i = 1; i < afterInitApproxPoints.Length - 1; ++i)
            {
                var Ui = initApproxPoints[i].U;

                var u = Complex.ImaginaryOne * Net.TStep * (
                    (initApproxPoints[i + 1].U - Ui - Ui + initApproxPoints[i - 1].U) / xStepSquare +
                    Ui.Magnitude * Ui.Magnitude * Ui) +
                    initApproxPoints[i].U;

                afterInitApproxPoints[i] = new ApproximationPoint
                {
                    X = XInterval.Start + i * Net.XStep,
                    U = u
                };
            }

            return afterInitApproxPoints;
        }
        private ApproximationPoint[] getNextApproximation(ApproximationPoint[] approxPointsJ, ApproximationPoint[] approxPointsJMinus1)
        {
            var approxPointsCount   = (int)((XInterval.End - XInterval.Start) / Net.XStep);

            var approxPointsJPlus1    = new ApproximationPoint[approxPointsCount + 1];
            approxPointsJPlus1[0]     = new ApproximationPoint
            {
                X = XInterval.Start,
                U = InitConds.PsiOfT0(XInterval.Start)
            };

            approxPointsJPlus1[approxPointsJPlus1.Length - 1] = new ApproximationPoint
            {
                X = XInterval.End,
                U = InitConds.PsiOfTL(XInterval.End)
            };

            double xStepSquare = Net.XStep * Net.XStep;

            for (int i = 1; i < approxPointsJPlus1.Length - 1; ++i)
            {
                var Ui = approxPointsJ[i].U;

                var u = 2.0 * Complex.ImaginaryOne * Net.TStep * (
                    (approxPointsJ[i + 1].U - Ui - Ui + approxPointsJ[i - 1].U) / xStepSquare +
                    Ui.Magnitude * Ui.Magnitude * Ui) +
                    approxPointsJMinus1[i].U;

                approxPointsJPlus1[i] = new ApproximationPoint
                {
                    X = XInterval.Start + i * Net.XStep,
                    U = u
                };
            }

            return approxPointsJPlus1;
        }

        public IEnumerable<ApproximationPoint> GetApproximateSolution(int timeMoment)
        {
            ApproximationPoint[] approxPointsJMinus1    = getInitApproximation();
            ApproximationPoint[] approxPointsJ          = getAfterInitApproximation(approxPointsJMinus1);
            ApproximationPoint[] approxPointsJPlus1     = null;

            switch (timeMoment)
            {
                case 0: return approxPointsJMinus1;
                case 1: return approxPointsJ;
                default:
                    {
                        for (int j = 2; j <= timeMoment; ++j)
                        {
                            approxPointsJPlus1  = getNextApproximation(approxPointsJ, approxPointsJMinus1);
                            approxPointsJMinus1 = approxPointsJ;
                            approxPointsJ       = approxPointsJPlus1;
                        }

                        return approxPointsJPlus1;
                    }
            }
        }

        public IEnumerable<IEnumerable<ApproximationPoint>> SequenceOfApproximations()
        {
            ApproximationPoint[] approxPointsJMinus1    = getInitApproximation();
            yield return approxPointsJMinus1;

            ApproximationPoint[] approxPointsJ          = getAfterInitApproximation(approxPointsJMinus1);
            yield return approxPointsJ;

            for (double tStep = TInterval.Start + 2 * Net.XStep; tStep <= TInterval.End; tStep += Net.TStep)
            {
                var approxPointsJPlus1 = getNextApproximation(approxPointsJ, approxPointsJMinus1);
                yield return approxPointsJPlus1;

                approxPointsJMinus1 = approxPointsJ;
                approxPointsJ = approxPointsJPlus1;
            }
        }
    }
}
