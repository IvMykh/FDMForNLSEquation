using System;
using System.Collections.Generic;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using F    = Func<double, double>;

    public struct InitConditions
    {
        public F FiOfX      { get; set; }
        public F PsiOfT0    { get; set; }
        public F PsiOfTL    { get; set; }
        
    }
    
    public struct Interval
    {
        public double Start { get; set; }
        public double End   { get; set; }
    }

    public struct Net
    {
        public int XStepsCount { get; set; }
        public int TStepsCount { get; set; }
    }

    public struct ApproximationPoint
	{
        public double   X { get; set; }
        public Complex  U { get; set; }
	}

    public class NseSolver
    {
        public Interval                     XInterval       { get; set; }
        public Interval                     TInterval       { get; set; }
        public Net                          Net             { get; set; }

        public InitConditions               InitConds       { get; set; }

        public NseSolver(Interval xInterval, Interval tInterval, InitConditions initConds, Net net)
        {
            this.XInterval      = xInterval;
            this.TInterval      = tInterval;
            this.InitConds      = initConds;
            this.Net            = net;
        }

        public ApproximationPoint[]         GetApproximateSolution(int timeMoment)
        {
            double xStep = (XInterval.End - XInterval.Start) / Net.XStepsCount;
            double tStep = (TInterval.End - TInterval.Start) / Net.TStepsCount;

            var initApproxPoints = new ApproximationPoint[Net.XStepsCount + 1];
            
            double x = XInterval.Start;

            for (int i = 0; i < initApproxPoints.Length; ++i)
            {
                initApproxPoints[i] = new ApproximationPoint 
                    { 
                        X = x, 
                        U = InitConds.FiOfX(x) 
                    };

                x += xStep;
            }

            var approxPointsPrev    = initApproxPoints; //new ApproximationPoint[Net.XStepsCount + 1];
            var approxPointsNext    = new ApproximationPoint[Net.XStepsCount + 1];
            
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

            double xStepSquare = xStep * xStep;
            
            for (int j = 0; j < timeMoment; ++j)
            {
                x = XInterval.Start;

                for (int i = 1; i < approxPointsNext.Length - 1; ++i)
                {
                    var Ui = approxPointsPrev[i].U;
                    var u = Complex.ImaginaryOne * tStep * (
                        (approxPointsPrev[i + 1].U - Ui - Ui + approxPointsPrev[i - 1].U) / xStepSquare +
                        Ui.Magnitude * Ui.Magnitude * Ui) +
                        Ui;

                    approxPointsNext[i] = new ApproximationPoint
                    {
                        X = x,
                        U = u
                    };

                    x += xStep;
                }

                approxPointsPrev = approxPointsNext;
                approxPointsNext = new ApproximationPoint[approxPointsNext.Length];
            }

            //var resultApproxPoints  = new ApproximationPoint[Net.XStepsCount + 1];
            //resultApproxPoints[0]   = new ApproximationPoint
            //    {
            //        X = XInterval.Start,
            //        U = InitConds.PsiOfT0(XInterval.Start)
            //    };

            //resultApproxPoints[resultApproxPoints.Length - 1] = new ApproximationPoint
            //    {
            //        X = XInterval.End,
            //        U = InitConds.PsiOfTL(XInterval.End)
            //    };

            //double xStepSquare = xStep * xStep;
            //x = XInterval.Start;

            //for (int i = 1; i < resultApproxPoints.Length; ++i)
            //{
            //    var Ui = initApproxPoints[i].U;
            //    var u = Complex.ImaginaryOne * tStep * (
            //        (initApproxPoints[i + 1].U - Ui - Ui + initApproxPoints[i - 1].U) / xStepSquare +
            //        Nu * Ui.Magnitude * Ui.Magnitude * Ui) +
            //        Ui;
                
            //    resultApproxPoints[i] = new ApproximationPoint
            //        {
            //            X = x,
            //            U = u
            //        };

            //    x += xStep;
            //}

            return approxPointsPrev;
        }
    }
}
