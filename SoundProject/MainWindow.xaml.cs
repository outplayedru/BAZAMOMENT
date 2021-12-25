using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using ScottPlot;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using NAudio.Wave;
using System;
using System.Text;

namespace SoundProject.Dialogs
{

    public partial class MainWindow : Window
    {

        public string? AudioFileName;

        public class MyDataContext
        {
            public int Pt { get; set; }
            public int Qt { get; set; }
            public int Pp { get; set; }
            public int Qp { get; set; }
            public MyDataContext()
            {
                Pt = 1; Qt = 1; Pp = 1; Qp = 1;
            }
        }

        public static (double[] data, WaveFormat format) ReadWAV(string filePath)
        {
            using (WaveFileReader reader = new WaveFileReader(filePath))
            {
                ISampleProvider sampleProvider = reader.ToSampleProvider();
                var audio = new List<double>();
                var buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
                int samplesRead = 0;

                while((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    audio.AddRange(buffer.Take(samplesRead).Select(x => (double) x));
                }

                return (audio.ToArray(), reader.WaveFormat);
            }
        }

        public static void PlayWAV(WaveFormat format, double[] samples)
        {
            BufferedWaveProvider waveProvider = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(format.SampleRate, format.Channels));
            waveProvider.BufferLength = samples.Length * 4;
            byte[] frames = Array.ConvertAll(samples, x => (float) x).SelectMany(v => BitConverter.GetBytes(v)).ToArray();
            waveProvider.AddSamples(frames, 0, frames.Length);

            using (var output = new DirectSoundOut())
            {
                output.Init(waveProvider); // new WaveChannel32 new WaveFileReader(AudioFileName)
                output.Play();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MyDataContext();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "WAF files (*.wav)|*.wav|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                AudioFileName = openFileDialog.FileName;
            }
        }

        private void btnPlayStretched_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AudioFileName)) return;

            (double[] x, WaveFormat format) = ReadWAV(AudioFileName);

            Trace.WriteLine(format.SampleRate);

            PlotBefore.Plot.Title("Исходный сигнал");
            PlotBefore.Plot.Clear();
            PlotBefore.Plot.AddSignal(x, format.SampleRate, System.Drawing.Color.DarkBlue);
            PlotBefore.Refresh();

            int Pt = ((MyDataContext)DataContext).Pt;
            int Qt = ((MyDataContext)DataContext).Qt;
            int Pp = ((MyDataContext)DataContext).Pp;
            int Qp = ((MyDataContext)DataContext).Qp;

            PhaseVocoder pvoc = new PhaseVocoder(Pt, Qt, Pp, Qp, 128);  // winLength=128 - размер окна для оконного ПФ
            double[] y = pvoc.apply(x);
            // Raise volume
            double multiplier = 1d / y.Max();
            y = y.Select(x => x * multiplier).ToArray();

            PlayWAV(format, y);

            PlotAfter.Plot.Title("Преобразованный сигнал");
            PlotAfter.Plot.Clear();
            PlotAfter.Plot.AddSignal(y, format.SampleRate, System.Drawing.Color.DarkRed);
            PlotAfter.Refresh();

        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // только цифры
            e.Handled = regex.IsMatch(e.Text);
        }


    }
}






