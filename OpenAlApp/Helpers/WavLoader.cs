using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace OpenAlApp.Helpers
{
    public class WavLoader
    {
        public WavLoader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var reader = new BinaryReader(stream))
            {
                // RIFF header
                var signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                reader.ReadInt32(); // Skip Riff chunk size in header

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                var formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                reader.ReadInt32(); // Skip format chunk size
                reader.ReadInt16(); // Skip audio format
                Channels = reader.ReadInt16();
                Rate = reader.ReadInt32();
                reader.ReadInt32(); // Skip byte rate
                reader.ReadInt16(); // Skip block align
                Bits = reader.ReadInt16();

                var dataSignature = new string(reader.ReadChars(4));
                if (dataSignature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                reader.ReadInt32(); // Skip data chunk size

                Data = reader.ReadBytes((int)reader.BaseStream.Length);
                Format = GetSoundFormat(Channels, Bits);
            }
        }
        public int Channels { get; set; }
        public int Bits { get; set; }
        public int Rate { get; set; }
        public byte[] Data { get; set; }
        public ALFormat Format { get; set; }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
