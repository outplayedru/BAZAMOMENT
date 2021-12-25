using System;

namespace Resample
{
    public class Resampler
    {
        private uint q, p;
        private double[,] filter;
        private uint flen;
        private uint nstages;
        private const double defaultAlpha = 5.0;
        private const uint defaultN = 10;

        private double[] state;
        private uint l_c = 0, k_c = 0;

        public Resampler(uint p, uint q)
        {
            InitKaiser(p, q, defaultN, defaultAlpha, 1.0);
        }

        private void Init(uint p, uint q, double[] f)
        {
            this.q = q;
            this.p = p;
            nstages = 1;
            uint i = q % p;

            while (i != 0)
            {
                nstages++;
                i = (i + q) % p;
            }

            flen = DivideCeiling((uint)f.Length, p);
            filter = new double[nstages, flen];

            i = 0;
            uint l = 0;

            for (l = 0; l < nstages; l++)
            {
                uint k = 0;
                for (uint j = i; j < f.Length; j += p)
                {
                    filter[l, k] = f[j];
                    k++;
                }
                i = (i + q) % p;
            }
  
            flipud(filter);


            state = new double[flen - 1];
        }

        private void InitKaiser(uint p, uint q, uint complexity, double alpha, double relative_cutoff)
        {
            uint maxpq = Math.Max(p, q);
            double standard_cutoff = 1.0 / maxpq;
            double[] f = FilterBuilder.KaiserFilter(relative_cutoff * standard_cutoff, (int)(2 * maxpq * complexity + 1), alpha);
            for (int i = 0; i < f.Length; i++)
                f[i] *= p;
            Init(p, q, f);
        }
        private void flipud(double[,] x)
        {
            int m = x.GetLength(0);
            int n = x.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    double temp = x[i, j];
                    x[i, j] = x[i, n - j - 1];
                    x[i, n - j - 1] = temp;
                }
            }
        }

        private double[] ZeroPad(double[] x, uint before, uint after)
        {
            double[] y = new double[x.Length + before + after];
            for (int i = 0; i < x.Length; i++)
            {
                y[i + before] = x[i];
            }
            return y;
        }

        private double[] ResampleLoop(double[] x, uint outputLength, ref uint k, ref uint l)
        {
            double[] y = new double[outputLength];
            for (int i = 0; i < outputLength; i++)
            {
                uint m = k / p;
                for (int j = 0; j < flen; j++)
                {
                    y[i] += x[m + j] * filter[l, j];
                }
                l++;
                l %= nstages;
                k = k + q;
            }
            return y;
        }

        public double[] Resample(double[] x)
        {
            uint l = 0;
            uint k = 0;
            uint n = (uint)Math.Floor((double)x.Length * p / q);
            x = ZeroPad(x, flen / 2, flen / 2);
            double[] y = ResampleLoop(x, n, ref k, ref l);
           
            return y;
        }

        private double[] AppendAndUpdateState(double[] x)
        {
            double[] y = new double[x.Length + flen - 1];
            for (int i = 0; i < flen - 1; i++)
            {
                y[i] = state[i];
            }
            for (int i = 0; i < x.Length; i++)
            {
                y[i + flen - 1] = x[i];
            }
            for (int i = 0; i < flen - 1; i++)
            {
                state[i] = y[x.Length + i];
            }
            return y;
        }

        public void ResetFilter()
        {
            state = new double[flen - 1];
            l_c = 0;
            k_c = 0;
        }

        private uint DivideCeiling(uint num, uint denom)
        {
            return (num + denom - 1) / denom;
        }

        public double[] ResampleContinuous(double[] x)
        {
            uint l = l_c; //l is the stage number in the filter
            uint k = k_c; //k is the start index into the upsampled signal
            uint inLength = (uint)x.Length;
            uint outLength = DivideCeiling(inLength * p - k, q); //Number of output samples depends on previous runs through k
            x = AppendAndUpdateState(x); //Add previous state to the beginning of the signal, and store current state
            double[] y = ResampleLoop(x, outLength, ref k, ref l);
            l_c = l; //Store for next call
            k_c = k - (inLength * p); //Index next time is the current index minus number of upsampled samples in this call
            return y;
        }
    }

    public static class FilterBuilder
    {
        public static double Sinc(double x)
        {
            if (x == 0.0)
                return 1.0;
            return Math.Sin(x) / x;
        }

        public static double[] LowpassSinc(double cutoff, int n)
        {
            double[] ret = new double[n];
            double delay = (double)(n - 1) / 2.0;
            for (int i = 0; i < n; i++)
            {
                double x = ((double)i - delay) * Math.PI * cutoff;
                ret[i] = Sinc(x);
            }
            return ret;
        }

        public static double[] HammingWindow(int n)
        {
            double[] win = new double[n];
            for (int i = 0; i < n; i++)
            {
                win[i] = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (n - 1));
            }
            return win;
        }

        public static double[] WindowedSinc(double cutoff, double[] window)
        {
            int length = window.Length;
            double[] f = new double[length];
            double[] h = LowpassSinc(cutoff, length);
            double sum = 0.0;
            for (int i = 0; i < length; i++)
            {
                f[i] = h[i] * window[i];
                sum += f[i];
            }
            sum = 1.0 / sum;
            for (int i = 0; i < length; i++)
            {
                f[i] *= sum;
            }
            return f;
        }

        public static double[] HammingFilter(double cutoff, int length)
        {
            return WindowedSinc(cutoff, HammingWindow(length));
        }

        public static double[] KaiserFilter(double cutoff, int length, double alpha)
        {
            return WindowedSinc(cutoff, KaiserWindow(length, alpha));
        }

        public static double MBessel0(double x)
        {
            x = x / 2.0;
            double fact = 1.0;
            double ret = 0;
            double pow = 1.0;
            for (uint i = 0; i < 20; i++)
            {
                ret += pow / (fact * fact);
                pow = pow * x * x;
                fact = fact * (i + 1);
            }
            return ret;
        }

        public static double[] KaiserWindow(int n, double alpha)
        {
            double beta = alpha * Math.PI;
            double[] win = new double[n];
            double denom = 1.0 / MBessel0(beta);
            double delay = (n - 1.0) / 2;
            for (int i = 0; i < n; i++)
            {
                double x = i - delay;
                double arg = 1.0 - Math.Pow((2.0 * x) / (n - 1.0), 2.0);
                win[i] = MBessel0(beta * Math.Sqrt(arg));
                win[i] *= denom;
            }
            return win;
        }
    }
}
