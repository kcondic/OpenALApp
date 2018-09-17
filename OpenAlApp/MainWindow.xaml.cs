using System;
using System.IO;
using System.Windows;
using OpenAlApp.Helpers;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace OpenAlApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _context = new AudioContext();

            var soundFile = new WavLoader(File.Open("Samples/blip.wav", FileMode.Open));

            _buffer = AL.GenBuffer();
            AL.BufferData(_buffer, soundFile.Format, soundFile.Data, soundFile.Data.Length, soundFile.Rate);

            _source = AL.GenSource();
            AL.Source(_source, ALSourcei.Buffer, _buffer);
        }
        private readonly AudioContext _context;
        private readonly int _buffer;
        private readonly int _source;

        private void Play_Click(object sender, RoutedEventArgs e)
        {        
            AL.SourcePlay(_source);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AL.Source(_source, ALSourceb.Looping, true);
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            AL.Source(_source, ALSourceb.Looping, false);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            AL.SourcePause(_source);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            AL.SourceStop(_source);
        }

        private void Set_Source_Position_Click(object sender, RoutedEventArgs e)
        {
            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;
            if (!float.TryParse(SourcePositionX.Text.Replace('.',','), out x) || 
                !float.TryParse(SourcePositionY.Text.Replace('.', ','), out y) ||
                !float.TryParse(SourcePositionZ.Text.Replace('.', ','), out z))
                MessageBox.Show("Neispravan unos!");
            else
                AL.Source(_source, ALSource3f.Position, x, y, z);
        }

        private void Set_Source_Velocity_Click(object sender, RoutedEventArgs e)
        {
            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;
            if (!float.TryParse(SourceVelocityX.Text.Replace('.', ','), out x) ||
                !float.TryParse(SourceVelocityY.Text.Replace('.', ','), out y) ||
                !float.TryParse(SourceVelocityZ.Text.Replace('.', ','), out z))
                MessageBox.Show("Neispravan unos!");
            else
                AL.Source(_source, ALSource3f.Velocity, x, y, z);
        }

        private void Set_Listener_Position_Click(object sender, RoutedEventArgs e)
        {
            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;
            if (!float.TryParse(ListenerPositionX.Text.Replace('.', ','), out x) ||
                !float.TryParse(ListenerPositionY.Text.Replace('.', ','), out y) ||
                !float.TryParse(ListenerPositionZ.Text.Replace('.', ','), out z))
                MessageBox.Show("Neispravan unos!");
            else
                AL.Listener(ALListener3f.Position, x, y, z);
        }

        private void Set_Listener_Velocity_Click(object sender, RoutedEventArgs e)
        {
            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;
            if (!float.TryParse(ListenerVelocityX.Text.Replace('.', ','), out x) ||
                !float.TryParse(ListenerVelocityY.Text.Replace('.', ','), out y) ||
                !float.TryParse(ListenerVelocityZ.Text.Replace('.', ','), out z))
                MessageBox.Show("Neispravan unos!");
            else
                AL.Listener(ALListener3f.Velocity, x, y, z);
        }

        private void Set_Source_Gain_Click(object sender, RoutedEventArgs e)
        {
            var num = 0.0f;
            if (!float.TryParse(SourceGain.Text.Replace('.', ','), out num))
                MessageBox.Show("Neispravan unos!");
            else
                AL.Source(_source, ALSourcef.Gain, num);
        }

        private void Release_Resources(object sender, EventArgs e)
        {
            AL.SourceStop(_source);
            AL.DeleteSource(_source);
            AL.DeleteBuffer(_buffer);
            Alc.MakeContextCurrent(ContextHandle.Zero);
            _context.Dispose();
        }
    }
}