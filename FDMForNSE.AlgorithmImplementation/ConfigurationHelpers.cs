using System;
using System.Collections.Generic;
using System.Numerics;

namespace FDMForNSE.AlgorithmImplementation
{
    using ComplF = Func<double, Complex>;

    public enum ConfigVariant
    {
        OneSoliton,
        TwoEqualInwardSolitons,
        CatchUp,
        TwoDifferentInwardSolitons,
        ThreeSolitons
    }

    public struct ConfigVariantName
    {
        public ConfigVariant    ConfigVariant  { get; private set; }
        public string           Caption        { get; private set; }

        public ConfigVariantName(ConfigVariant configVariant, string caption)
            :this()
        {
            ConfigVariant   = configVariant;
            Caption         = caption;
        }

        public override string ToString()
        {
            return Caption;
        }

        public static object[] GetConfigVariantsRange()
        {
            return new object [] 
                { 
                    new ConfigVariantName(ConfigVariant.OneSoliton, "One soliton"),
                    new ConfigVariantName(ConfigVariant.TwoEqualInwardSolitons, "Two equal inward solitons"),
                    new ConfigVariantName(ConfigVariant.CatchUp, "Solitons catch-up"),
                    new ConfigVariantName(ConfigVariant.TwoDifferentInwardSolitons, "Two different inward solitons"),
                    new ConfigVariantName(ConfigVariant.ThreeSolitons, "Three solitons")
                };
        }
    }

    public class Configuration
    {
        public Interval         XInterval       { get; set; }
        public Interval         TInterval       { get; set; }
        public Net              Net             { get; set; }
        public InitConditions   InitConditions  { get; set; }
    }

    public class ConfigurationsStore
    {
        // Singleton.
        public static ConfigurationsStore Store { get; private set; }

        static ConfigurationsStore()
        {
            Store = new ConfigurationsStore();
        }

        // Instance members.
        private Dictionary<
            ConfigVariant, Configuration> configurations;

        private ConfigurationsStore()
        {
            configurations = setupConfigurations();
        }

        private Dictionary<
            ConfigVariant, Configuration> setupConfigurations()
        {
            var configs = new Dictionary<
                ConfigVariant, Configuration>();

            configs[ConfigVariant.OneSoliton]                   = setupOneSolitonConfig();
            configs[ConfigVariant.TwoEqualInwardSolitons]       = setupTwoEqualSolitonsConfig();
            configs[ConfigVariant.CatchUp]                      = setupCatchUpConfig();
            configs[ConfigVariant.TwoDifferentInwardSolitons]   = setupTwoDifferentSolitonsConfig();
            configs[ConfigVariant.ThreeSolitons]                = setupThreeSolitonsConfig();

            return configs;
        }

        private Configuration setupOneSolitonConfig()
        {
            return new Configuration
            {
                XInterval       = new Interval  { Start = -10.0, End = 10.0 },
                TInterval       = new Interval  { Start = 0.0,   End = 10.0 },
                Net             = new Net       { XStep = 0.1,   TStep = 0.0025 },
                InitConditions  = new InitConditions
                    {
                        PsiOfT0 = (double t) => { return 0.0; },
                        PsiOfTL = (double t) => { return 0.0; },
                        FiOfX = new ComplF((double x) =>
                            {
                                var ampl1 = 1.0;
                                var v1 = 2.0;

                                return SpecData.SQRT_2 *
                                    (ampl1 * Complex.Exp((x) * SpecData.I * -v1) /
                                    (Math.Cosh(ampl1 * (x - 5))));
                            })
                    }
            };
        }
        private Configuration setupTwoEqualSolitonsConfig()
        {
            return new Configuration
            {
                XInterval       = new Interval  { Start = -10.0, End = 10.0 },
                TInterval       = new Interval  { Start = 0.0,   End = 10.0 },
                Net             = new Net       { XStep = 0.1,   TStep = 0.0025 },
                InitConditions  = new InitConditions
                    {
                        PsiOfT0 = (double t) => { return 0.0; },
                        PsiOfTL = (double t) => { return 0.0; },
                        FiOfX = new ComplF((double x) =>
                            {
                                return SpecData.SQRT_2 *
                                    (Complex.Exp(-x * SpecData.I) / Math.Cosh(x - 5.0) +
                                     Complex.Exp(x * SpecData.I) / Math.Cosh(x + 5.0));
                            })
                    }
            };
        }
        private Configuration setupCatchUpConfig()
        {
            double ampl1 = 1.7, v1 = 2;
            double ampl2 = 0.8, v2 = 0.8;

            return new Configuration
            {
                XInterval       = new Interval  { Start = -20.0, End = 20.0 },
                TInterval       = new Interval  { Start = 0.0,   End = 5.0 },
                Net             = new Net       { XStep = 0.1,   TStep = 0.0025 },
                InitConditions  = new InitConditions
                {
                    PsiOfT0 = (double t) => { return 0.0; },
                    PsiOfTL = (double t) => { return 0.0; },
                    FiOfX = new ComplF((double x) =>
                    {
                        return SpecData.SQRT_2 *
                        (ampl1 * Complex.Exp((x) * SpecData.I * v1) / (Math.Cosh(ampl1 * (x + 5)))
                       + ampl2 * Complex.Exp((x) * SpecData.I * v2) / (Math.Cosh(ampl2 * (x))));
                    })
                }
            };
        }
        private Configuration setupTwoDifferentSolitonsConfig()
        {
            double ampl1 = 1.7, v1 = 2;
            double ampl2 = 0.8, v2 = 0.8;

            return new Configuration
            {
                XInterval = new Interval { Start = -20.0, End = 20.0 },
                TInterval = new Interval { Start = 0.0, End = 5.0 },
                Net = new Net { XStep = 0.1, TStep = 0.0025 },
                InitConditions = new InitConditions
                {
                    PsiOfT0 = (double t) => { return 0.0; },
                    PsiOfTL = (double t) => { return 0.0; },
                    FiOfX = new ComplF((double x) =>
                    {
                        return SpecData.SQRT_2 *
                        (ampl1 * Complex.Exp((x) * SpecData.I * v1) / (Math.Cosh(ampl1 * (x + 5)))
                       + ampl2 * Complex.Exp((-x) * SpecData.I * v2) / (Math.Cosh(ampl2 * (x - 2))));
                    })
                }
            };
        }
        private Configuration setupThreeSolitonsConfig()
        {
            double ampl1 = 1.0, v1 = 1.0;
            double ampl2 = 1.7, v2 = 2;
            double ampl3 = 0.8, v3 = 0.8;
            
            return new Configuration
            {
                XInterval       = new Interval  { Start = -20.0, End = 20.0 },
                TInterval       = new Interval  { Start = 0.0, End = 6.0 },
                Net             = new Net       { XStep = 0.1, TStep = 0.0025 },
                InitConditions  = new InitConditions 
                    {
                        PsiOfT0 = (double t) => { return 0.0; },
                        PsiOfTL = (double t) => { return 0.0; },
                        FiOfX   = new ComplF((double x) => 
                            {
                                return SpecData.SQRT_2 *
                                    (ampl1 * Complex.Exp((x) * SpecData.I * -v1) / (Math.Cosh(ampl1 * (x - 6)))
                                    + ampl2 * Complex.Exp((x) * SpecData.I * v2) / (Math.Cosh(ampl2 * (x + 6)))
                                    + ampl3 * Complex.Exp((x) * SpecData.I * v3) / (Math.Cosh(ampl3 * (x))));
                            })
                    }
            };
        }

        public Configuration this[ConfigVariant variant]
        {
            get
            {
                return configurations[variant];
            }
        }
    }
}
