using System;
using System.Numerics;

namespace SoundProject
{
 public class STFT
    {
        public int hopSize;
        public int winLength;
        private Complex[] win;

        public STFT(int winLength)
        {
            setWinLength(winLength);
            this.hopSize = (int)((double)winLength / 4.0);
        }

        public void setWinLength(int winLength)
        {
            this.winLength = winLength;

            win = new Complex[winLength];

            for(int sample = 0; sample < winLength; sample++)
            {
                win[sample] = new Complex(0.5 * (1.0 - Math.Cos(2.0*Math.PI*(double)sample/(double)winLength)), 0.0);
            }                      
        }

        public TFR forward(double[] input_signal)
        {
            int ns = input_signal.Length;
            int nw = winLength;
            
            int nf = (int)Math.Floor((double) (ns - nw) / (double)hopSize);

            Complex[][] fframe = new Complex[nf + 1][];
            for (int sample = 0; sample < fframe.Length; sample++)
            {
                fframe[sample] = new Complex[nw];
            }

            Complex factor = new Complex(2.0 / 3.0, 0.0);

            int hopInd = 0;
            for(int hop = 0; hop <= nf*hopSize; hop+=hopSize)
            {
                Complex[] buf = new Complex[nw];

                for (int sample = 0; sample < nw; sample++)
                {
                    buf[sample] = new Complex(input_signal[hop + sample], 0.0) * factor * win[sample];
                }

                fftw.FFT(buf, fftw.Direction.Forward);

                fframe[hopInd++] = buf;
            }

            return new TFR(fframe);
        }

        public double[] inverse(TFR tfr)
        {
            int nf = tfr.getTFRAsDoubleArray().Length - 1;
            int nw = tfr.getTFRAsDoubleArray()[0].Length;
            int ns = nf * hopSize + nw;
            double[] output = new double[ns];

            int hopInd = 0;
            Complex[] buf = new Complex[nw];

            try
            {
                for (int hop = 0; hop <= nf * hopSize; hop += hopSize)
                {
                    Array.Copy(tfr.getTFRAsDoubleArray()[hopInd++], buf, nw);
                    fftw.FFT(buf, fftw.Direction.Backward);

                    for (int sample = 0; sample < nw; sample++)
                        output[sample + hop] = output[sample + hop] + (buf[sample].Real / nw) * win[sample].Real;
                }

                return output;
            }
            catch (Exception)
            {
                return output;
            }
        }
    }
}