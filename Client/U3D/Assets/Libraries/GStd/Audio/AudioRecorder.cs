namespace GStd {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;

    public static class AudioRecorder
    {
        private static string deviceName = null;
        private static float tmpClipDuration = 0;
        private static int recordFrequency = 8000;

        public static void Start(bool isForceStart = true, int lengthSec=99, int frequency=8000)
        {
            if (Microphone.IsRecording(deviceName))
            {
                if (isForceStart)
                    Microphone.End(deviceName);
                else
                {
                    Debug.LogWarning("[Recorder]still recording. can not start, or you can set force start");
                    return;
                }
            }

            tmpClipDuration = Time.realtimeSinceStartup;
            audioClip = Microphone.Start(null, false, lengthSec, frequency);
            recordFrequency = frequency;

            Debug.Log("[Recorder]start record , clip = " + audioClip);
        }

        public static string EndAndSave(string persistentRelativeFilePath)
        {
            if (!Microphone.IsRecording(deviceName))
            {
                Debug.LogWarning("[Record]not recording, no need to end");
                return "";
            }
            else
            {
                Microphone.End(deviceName);

                if (audioClip == null)
                {
                    Debug.LogError("[Record]end record, but audio clip is null");
                    return "";
                }

                tmpClipDuration = Time.realtimeSinceStartup - tmpClipDuration;

                byte[] bytes = AudioCompress.CompressAudioClip(audioClip);

                string savePath = Application.persistentDataPath + "/" + persistentRelativeFilePath;
                File.WriteAllBytes(savePath, bytes);

                Debug.Log("[Recorder]end record, save to " + savePath + ", length/size=" + tmpClipDuration + "/" + bytes.Length/1024);

                return savePath;
            }
        }

        public static bool isRecording
        {
            get;
            private set;
        }

        public static AudioClip audioClip
        {
            get;
            private set;
        }

        // volume 0~1
        // gain 0~00
        public static void Play(string path, float volume=1.0f, float gain=1.0f)
        {
            if (isRecording)
            {
                Debug.LogWarning("still recording");
                return;
            }

            if (!File.Exists(path))
            {
                Debug.LogError(string.Format("play failed, file {0} not exist", path));
                return;
            }

            byte[] bytes = File.ReadAllBytes(path);
            try
            {
                var clip = AudioCompress.DecompressAudioClip(bytes, recordFrequency, 1, gain);
                if (clip != null)
                    AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
            }
            catch(System.Exception e)
            {
                Debug.LogError("[Record]play exception, message = " + e.Message);
            }
            

            Debug.Log("[Recorder]play record , path = " + path);
        }

    }

}

