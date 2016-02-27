using System;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using F         = Func<double, double>;
    using ComplF    = Func<double, Complex>;

    public struct InitConditions
    {
        public ComplF   FiOfX      { get; set; }
        public F        PsiOfT0    { get; set; }
        public F        PsiOfTL    { get; set; }
        
    }
    
    public struct Interval
    {
        public double Start { get; set; }
        public double End   { get; set; }
    }

    public struct Net
    {
        public double XStep { get; set; }
        public double TStep { get; set; }
    }

    public struct ApproximationPoint
	{
        public double   X { get; set; }
        public Complex  U { get; set; }
	}
}