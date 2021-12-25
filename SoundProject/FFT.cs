using System;
using System.Collections.Generic;
using System.Numerics;

namespace SoundProject
{
    internal class fftw
    {
        private const int minLength = 2;
        private const int maxLength = 16384;
        private const int minBits = 1;
        private const int maxBits = 14;

        public enum Direction
        {
            Forward = 1,

            Backward = -1
        };

        public static int Pow2(int power)
        {
            return power >= 0 && power <= 30 ? 1 << power : 0;
        }

        private static bool IsPowerOf2(int x)
        {
            return x > 0 ? (x & (x - 1)) == 0 : false;
        }

        private static int[] GetReversedBits(int numberOfBits)
        {
            if (numberOfBits < minBits || numberOfBits > maxBits)
            {
                throw new ArgumentOutOfRangeException();
            }

            int n = Pow2(numberOfBits);
            int[] rBits = new int[n];

            for (int i = 0; i < n; i++)
            {
                int oldBits = i;
                int newBits = 0;

                for (int j = 0; j < numberOfBits; j++)
                {
                    newBits = (newBits << 1) | (oldBits & 1);
                    oldBits = (oldBits >> 1);
                }

                rBits[i] = newBits;
            }

            return rBits;
        }

        private static void ReorderData(Complex[] data)
        {
            int len = data.Length;

            if (len < minLength || len > maxLength || !IsPowerOf2(len))
            {
                throw new ArgumentException("Incorrect data length.");
            }

            int[] rBits = GetReversedBits((int)Math.Log2(len));

            for (int i = 0; i < len; i++)
            {
                int s = rBits[i];

                if (s > i)
                {
                    Complex t = data[i];
                    data[i] = data[s];
                    data[s] = t;
                }
            }
        }

        private static Complex[] GetComplexRotation(int numberOfBits, Direction direction)
        {
            int n = 1 << (numberOfBits - 1);
            double uR = 1.0;
            double uI = 0.0;
            double angle = Math.PI / n * (int)direction;
            double wR = Math.Cos(angle);
            double wI = Math.Sin(angle);
            double t;
            Complex[] rotation = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                rotation[i] = new Complex(uR, uI);
                t = uR * wI + uI * wR;
                uR = uR * wR - uI * wI;
                uI = t;
            }

            return rotation;
        }

        public static void FFT(Complex[] data, Direction direction)
        {
            int n = data.Length;
            int m = (int)Math.Log2(n);

            // reorder data first
            ReorderData(data);

            // compute FFT
            int tn = 1, tm;

            for (int k = 1; k <= m; k++)
            {
                Complex[] rotation = GetComplexRotation(k, direction);

                tm = tn;
                tn <<= 1;

                for (int i = 0; i < tm; i++)
                {
                    var t = rotation[i];

                    for (int even = i; even < n; even += tn)
                    {
                        int odd = even + tm;
                        var ce = data[even];
                        var cot = data[odd] * t;

                        data[even] += cot;
                        data[odd] = ce - cot;
                    }
                }
            }

            if (direction == Direction.Forward)
            {
                for (int i = 0; i < n; i++)
                {
                    data[i] /= (double)n;
                }
            }
        }
    }
}
