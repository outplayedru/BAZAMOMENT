using Resample;

namespace SoundProject
{
    public class PhaseVocoder
    {
        private readonly int Qt, Pt, Qp, Pp;
        private readonly STFT stft;
        private readonly Resampler resampler;


        /*
         * Pt/Qt = tempo change factor. A tempo change factor < 1 will lead to slower tempo. A tempo change factor > 1 will lead to faster tempo
         * Pp/Qp = pitch shift factor. A pitch shift factor < 1 will lead to reduced/lower pitch. A pitch shift factor > 1 will lead to increased/raised pitch.
         * 
         */

        public PhaseVocoder(int Pt, int Qt, int Pp, int Qp, int winLength)
        {
            this.Qt = Qt;
            this.Pt = Pt;
            this.Qp = Qp;
            this.Pp = Pp;
            this.stft = new STFT(winLength);
            this.resampler = new Resampler((uint)Qp, (uint)Pp);
        }

        public double[] apply(double[] input_signal)
        {
            TFR tfr = stft.forward(input_signal);
            tfr.applyTempoChange((double) Pt / (double) Qt * (double) Qp / (double) Pp); /* Tempo change step */
            double[] output_signal = stft.inverse(tfr);

            /* Pitch shift step. Apply function resample(Qp, Pp) on output_signal to change its sample rate by a factor Qp/Pp (multiplied by) */

            output_signal = resampler.Resample(output_signal);

            return output_signal;
        }
    }
}
