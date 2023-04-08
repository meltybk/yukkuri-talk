using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;

namespace YukkuriTalk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PitchValue.Text = PitchSlider.Value.ToString();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] wave = CreateWaveData();

            if (wave == null)
            {
                return;
            }

            // Synchronized play
            using (var ms = new MemoryStream(wave))
            using (var sp = new SoundPlayer(ms))
            {
                sp.PlaySync();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] wave = CreateWaveData();

            if (wave == null)
            {
                return;
            }

            var fileName = MainText.Text.Substring(0, Math.Min(MainText.Text.Length, 12)) + ".wav";

            if (SaveDirectory != null)
            {
                fileName = SaveDirectory + "\\" + fileName;
            }

            using (var stream = new FileStream(fileName, FileMode.Create))
            using (var file = new BinaryWriter(stream))
            {
                file.Write(wave);
                LogText.Text = "The wave file has been saved.\n" + stream.Name;
            }
        }

        private void Pitch_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PitchValue.Text = PitchSlider.Value.ToString();
        }

        private byte[] CreateWaveData()
        {
            String text = MainText.Text;
            int pitch = (int)PitchSlider.Value;

            IntPtr wavePtr = IntPtr.Zero;
            int size = 0;

            if (text.Length > 0)
            {
                wavePtr = AquesTalk.AquesTalk_Synthe(text, pitch, ref size);
            }
            if (wavePtr == IntPtr.Zero)
            {
                LogText.Text = "An unsupported text has been entered.";
                return null;
            }
            byte[] wave = new byte[size];
            Marshal.Copy(wavePtr, wave, 0, size);

            AquesTalk.AquesTalk_FreeWave(wavePtr);

            return wave;
        }

        private void Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Please select save directory.";
                dialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                SaveDirectory = dialog.FileName;

                LogText.Text = "New save directory selected.\n" + SaveDirectory;
            }
        }

        private string SaveDirectory = null;
    }
}
