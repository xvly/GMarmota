namespace GStd { 

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using SevenZip;
    using UnityEngine.Assertions;
    using System;

    public class AudioCompress{

        public enum AudioBandMode
        {
            Narrow,
            Wide
        }

        public static ICodec Codec = new MuLawCodec();

        private static List<byte> data = new List<byte>();
        private static List<short> tmp = new List<short>();

        public static byte[] CompressAudioClip(AudioClip clip, float gain = 1.0f)
        {
            data.Clear();

            short[] b = AudioClipToShorts(clip, gain);
            for (int i = 0; i < b.Length; i++)
            {
                //identify "quiet" samples, set them to exactly 1 so that a row of these "near-zero" samples becomes a row of exactly-one samples and achieves better Deflate compression
                if (b[i] <= 5 && b[i] >= -5 && b[i] != 0)
                    b[i] = 1;
            }

            byte[] mlaw = Codec.Encode(b);
            data.AddRange(mlaw);
            return zip(data.ToArray());
        }

        public static AudioClip DecompressAudioClip(byte[] data, int frequency = 8000, int channels = 1, float gain=1.0f)
        {
            byte[] d;
            d = unzip(data);

            short[] pcm = Codec.Decode(d);

            tmp.Clear();
            tmp.AddRange(pcm);

            //while (tmp.Count > 1 && Mathf.Abs(tmp[tmp.Count - 1]) <= 10)
            //{
            //    tmp.RemoveAt(tmp.Count - 1);
            //}

            //while (tmp.Count > 1 && Mathf.Abs(tmp[0]) <= 10)
            //{
            //    tmp.RemoveAt(0);
            //}

            return ShortsToAudioClip(tmp.ToArray(), channels, frequency, gain);

        }

        #region zip

        private static byte[] zip(byte[] data)
        {
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();

            using (var inputStream = new MemoryStream())
            using (var outStream = new MemoryStream())
            {
                // write data
                inputStream.Write(data, 0, data.Length);
                inputStream.Position = 0;

                // write properties
                encoder.WriteCoderProperties(outStream);
                outStream.Write(BitConverter.GetBytes(inputStream.Length), 0, 8); 

                // encode
                encoder.Code(inputStream, outStream, inputStream.Length, -1, null);

                // output
                outStream.Flush();
                return outStream.ToArray();
            }
        }

        private static byte[] unzip(byte[] data)
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();

            using (var inputStream = new MemoryStream())
            using (var outputStream = new MemoryStream())
            {
                // read data
                inputStream.Write(data, 0, data.Length);
                inputStream.Position = 0;

                // read prop
                byte[] properties = new byte[5];
                inputStream.Read(properties, 0, 5);

                // read size
                byte[] fileLengthBytes = new byte[8];
                inputStream.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                // decode
                decoder.SetDecoderProperties(properties);
                decoder.Code(inputStream, outputStream, inputStream.Length, fileLength, null);

                // output
                outputStream.Flush();
                return outputStream.ToArray();
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            //long TempPos = input.Position;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) break;
                output.Write(buffer, 0, read);
            }
            //input.Position = TempPos;// or you make Position = 0 to set it at the start 
        }
        #endregion

        #region convert
        private static byte[] AudioClipToBytes(AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            byte[] data = new byte[clip.samples * clip.channels];
            for (int i = 0; i < samples.Length; i++)
            {
                //convert to the -128 to +128 range
                float conv = samples[i] * 128.0f;
                int c = Mathf.RoundToInt(conv);
                c += 127;
                if (c < 0)
                    c = 0;
                if (c > 255)
                    c = 255;

                data[i] = (byte)c;
            }

            return data;
        }

        /// <summary>
		/// Convert an audio clip to a short array
		/// </summary>
		/// <param name="clip">The audio clip to convert</param>
		/// <returns>A short array</returns>
		private static short[] AudioClipToShorts(AudioClip clip, float gain = 1.0f)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            short[] data = new short[clip.samples * clip.channels];
            for (int i = 0; i < samples.Length; i++)
            {
                //convert to the -3267 to +3267 range
                float g = samples[i] * gain;
                if (Mathf.Abs(g) > 1.0f)
                {
                    if (g > 0)
                        g = 1.0f;
                    else
                        g = -1.0f;
                }
                float conv = g * 3267.0f;
                //int c = Mathf.RoundToInt( conv );

                data[i] = (short)conv;
            }

            return data;
        }

        /// <summary>
		/// Convert a byte array to an audio clip
		/// </summary>
		/// <param name="data">The byte array representing an audio clip</param>
		/// <param name="channels">How many channels in the audio data</param>
		/// <param name="frequency">The recording frequency of the audio data</param>
		/// <param name="gain">How much to boost the volume (1.0 = unchanged)</param>
		/// <returns>An AudioClip</returns>
		public static AudioClip BytesToAudioClip(byte[] data, int channels, int frequency, float gain)
        {
            float[] samples = new float[data.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                //convert to integer in -128 to +128 range
                int c = (int)data[i];
                c -= 127;
                samples[i] = ((float)c / 128.0f) * gain;
            }

            AudioClip clip = AudioClip.Create("clip", data.Length / channels, channels, frequency, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Convert a short array to an audio clip
        /// </summary>
        /// <param name="data">The short array representing an audio clip</param>
        /// <param name="channels">How many channels in the audio data</param>
        /// <param name="frequency">The recording frequency of the audio data</param>
        /// <param name="threedimensional">Whether the audio clip should be 3D</param>
        /// <param name="gain">How much to boost the volume (1.0 = unchanged)</param>
        /// <returns>An AudioClip</returns>
        public static AudioClip ShortsToAudioClip(short[] data, int channels, int frequency, float gain)
        {
            float[] samples = new float[data.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                //convert to float in the -1 to 1 range
                int c = (int)data[i];
                samples[i] = ((float)c / 3267.0f) * gain;
            }

            AudioClip clip = AudioClip.Create("clip", data.Length / channels, channels, frequency, false);
            clip.SetData(samples, 0);
            return clip;
        }

        #endregion
    }

}