using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Binomial
            var p = 0.174;
            var n = 6;
            var x = 5;
            var bCdf = Binomial.CDF(p, n, x);
            var bPmf = Binomial.PMF(p, n, x);
            var bPmfLn = Binomial.PMFLn(p, n, x);

            var b = new Binomial(p, n);
            var _bCdf = b.CumulativeDistribution(x);
            var _bPmf = b.Probability(x);
            var _bPmfLn = b.ProbabilityLn(x);

            // Normal
            var mean = 1012.5;
            var stdDev = 24.8069;
            var x1 = 1000;
            var x2 = 1025;
            var nCdf = Normal.CDF(mean, stdDev, x2) - Normal.CDF(mean, stdDev, x1);
            var nPdf = Normal.PDF(mean, stdDev, x2) - Normal.PDF(mean, stdDev, x1);
            var nPdfLn = Normal.PDFLn(mean, stdDev, x2) - Normal.PDFLn(mean, stdDev, x1);

            var resN = new List<double>();

            for (int i = 950; i < 1075; i++)
            {
                //resN.Add(Normal.CDF(mean, stdDev, 1075) - Normal.CDF(mean, stdDev, i));
                resN.Add(Normal.CDF(mean, stdDev, i));
            }

            var _n = new Normal(mean, stdDev);
            var _nCdf = b.CumulativeDistribution(x2) - b.CumulativeDistribution(x1);
            var _nPdf = b.Probability(x2) - b.Probability(x1);
            var _nPdfLn = b.ProbabilityLn(x2) - b.ProbabilityLn(x1);
            var nSamples = new double[100];
            _n.Samples(nSamples);

            // Discrete Uniform
            var sqrtThree = Math.Sqrt(3);
            var _mean = 2.1;
            var _stdDev = 1.465;
            var lower =  (_mean - sqrtThree * _stdDev);
            var upper =  (_mean + sqrtThree * _stdDev);
            var u = new ContinuousUniform(lower, upper);
            var _p = u.Density(1.05);

            var h = new Histogram(nSamples, 5);

                     Regex _regexBack = new Regex(@"(?:(?<Lower>[\d]+)? *-? *(?:(?<Upper>[\d]+)))");
            var test0 = "66";

            var select = test0;

            if (_regexBack.IsMatch(select))
            {
                foreach (var match in _regexBack.Matches(select))
                {

                }
            }

            

            Console.ReadKey();
        }

        public static void PrintMessage()
        {
            var width = 11;
            var height = 11;

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Console.Write((
                        h == 0 ||
                        h == w ||
                        h == height - 1 ||
                        w == 0 ||
                        w == width - 1 ||
                        w == width - 1 - h) ?
                        "#" : " ");
                }

                Console.WriteLine();
            }
        }

        public static double Gauss(double z)
        {
            // input = z-value (-inf to +inf)
            // output = p under Standard Normal curve from -inf to z
            // e.g., if z = 0.0, function returns 0.5000
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable
            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        }

        public static double Student(double t, double df)
        {
            // for large integer df or double df
            // adapted from ACM algorithm 395
            // returns 2-tail p-value
            double n = df; // to sync with ACM parameter name
            double a, b, y;
            t = t * t;
            y = t / n;
            b = y + 1.0;
            if (y > 1.0E-6) y = Math.Log(b);
            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;
            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
              Math.Sqrt(y);
            return 2.0 * Gauss(-y); // ACM algorithm 209
        }

        static double gamma(double x)
        {
            double tmp = (x - 0.5) * Math.Log(x + 4.5) - (x + 4.5);
            double ser = 1.0 +
                            76.18009173 / (x + 0.0) - 86.50532033 / (x + 1.0) +
                            24.01409822 / (x + 2.0) - 1.231739516 / (x + 3.0) +
                            0.00120858003 / (x + 4.0) - 0.00000536382 / (x + 5.0);
            return Math.Exp(tmp + Math.Log(ser * Math.Sqrt(2 * Math.PI)));
        }


        public static double Gamma(double x)
        {
            //--- ïðîâåðêà
            if (x == 0)
            {
                //Print(__FUNCTION__ + ": the error variable");
                return (0);
            }
            //--- èíèöèàëèçàöèÿ ïåðåìåííûõ
            double p = 0.0;
            double pp = 0.0;
            double qq = 0.0;
            double z = 1.0;
            double sgngam = 1.0;
            int i = 0;
            //--- ïðîâåðêà
            if (x > 33.0)
                return (sgngam * GammaStirling(x));
            //---
            while (x >= 3)
            {
                x--;
                z = z * x;
            }
            //---
            while (x < 2)
            {
                if (x < 0.000000001)
                    return (z / ((1 + 0.5772156649015329 * x) * x));
                z /= x;
                x++;
            }
            //---
            if (x == 2)
                return (z);
            //---
            x -= 2.0;
            pp = 0.000160119522476751861407;
            pp = 0.00119135147006586384913 + x * pp;
            pp = 0.0104213797561761569935 + x * pp;
            pp = 0.0476367800457137231464 + x * pp;
            pp = 0.207448227648435975150 + x * pp;
            pp = 0.494214826801497100753 + x * pp;
            pp = 0.999999999999999996796 + x * pp;
            qq = -0.0000231581873324120129819;
            qq = 0.000539605580493303397842 + x * qq;
            qq = -0.00445641913851797240494 + x * qq;
            qq = 0.0118139785222060435552 + x * qq;
            qq = 0.0358236398605498653373 + x * qq;
            qq = -0.234591795718243348568 + x * qq;
            qq = 0.0714304917030273074085 + x * qq;
            qq = 1.00000000000000000320 + x * qq;
            //--- ïðîâåðêà
            if (qq == 0)
            {
                //Print(__FUNCTION__ + ": the error variable");
                return (0);
            }
            //--- âîçâðàò ðåçóëüòàòà
            return (z * pp / qq);
        }

        public static double GammaStirling(double x)
        {
            //--- ïðîâåðêà
            if (x == 0)
            {
                //Print(__FUNCTION__ + ": the error variable");
                return (0);
            }
            //--- èíèöèàëèçàöèÿ ïåðåìåííûõ
            double y = 0.0;
            double w = 0.0;
            double v = 0.0;
            double stir = 0.0;
            //---
            w = 1.0 / x;
            stir = 0.000787311395793093628397;
            stir = -0.000229549961613378126380 + w * stir;
            stir = -0.00268132617805781232825 + w * stir;
            stir = 0.00347222221605458667310 + w * stir;
            stir = 0.0833333333333482257126 + w * stir;
            w = 1 + w * stir;
            //---
            y = Math.Exp(x);
            if (x > 143.01608)
            {
                v = Math.Pow(x, 0.5 * x - 0.25);
                y = v * (v / y);
            }
            else
                y = Math.Pow(x, x - 0.5) / y;
            //--- âîçâðàò ðåçóëüòàòà
            return (2.50662827463100050242 * y * w);
        }
    }
}
