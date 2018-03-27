namespace GStd
{
    using ns0;
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public sealed class AssetSimulator
    {
        private float float_0;
        private float float_1 = 1f;
        private int int_0 = -1;
        private int int_1 = -1;
        private int int_2 = -1;
        private const string string_0 = "SimulateAssetBundlesMode";
        private const string string_1 = "SimulateAssetBundleFailed";

        private float method_0()
        {
            if (this.float_0 < 0f)
            {
                this.method_9();
            }
            return this.float_0;
        }

        private float method_1()
        {
            if (this.float_1 < 0f)
            {
                this.method_9();
            }
            return this.float_1;
        }

        internal WaitLoadObject method_2()
        {
            return new Class25(null, UnityEngine.Random.Range(this.method_0(), this.method_1()));
        }

        internal WaitForSecondsRealtime method_3()
        {
            if (this.SimulateAssetBundleFailed && (UnityEngine.Random.Range(0, 100) <= 50))
            {
                return new WaitForSecondsRealtime(UnityEngine.Random.Range((float)1f, (float)5f));
            }
            return null;
        }

        internal UnityEngine.Object method_4(string string_2, string string_3, System.Type type_0)
        {
            string[] assetPathsFromAssetBundleAndAssetName = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(string_2, Path.GetFileNameWithoutExtension(string_3));
            string assetPath = null;
            if (!Path.HasExtension(string_3))
            {
                if (assetPathsFromAssetBundleAndAssetName.Length > 0)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(string_3);
                    foreach (string str8 in assetPathsFromAssetBundleAndAssetName)
                    {
                        if (Path.GetFileNameWithoutExtension(str8) == fileNameWithoutExtension)
                        {
                            assetPath = str8;
                            break;
                        }
                    }
                }
            }
            else
            {
                string str2 = Path.GetFileNameWithoutExtension(string_3);
                string extension = Path.GetExtension(string_3);
                foreach (string str4 in assetPathsFromAssetBundleAndAssetName)
                {
                    string str5 = Path.GetFileNameWithoutExtension(str4);
                    string str6 = Path.GetExtension(str4);
                    if ((str5 == str2) && (str6 == extension))
                    {
                        assetPath = str4;
                        break;
                    }
                }
            }
            if (assetPath == null)
            {
                return null;
            }
            UnityEngine.Object obj2 = AssetDatabase.LoadAssetAtPath(assetPath, type_0);
            if (obj2 == null)
            {
                return null;
            }
            return obj2;
        }

        internal WaitLoadObject method_5(string string_2, string string_3, System.Type type_0)
        {
            UnityEngine.Object obj2 = this.method_4(string_2, string_3, type_0);
            float num = UnityEngine.Random.Range(this.method_0(), this.method_1());
            if (obj2 == null)
            {
                return new Class25("Load object {0}:{1} failed.", new object[] { string_2, string_3 });
            }
            return new Class25(obj2, num);
        }

        internal WaitLoadObject method_6(string string_2, string string_3, System.Type type_0)
        {
            UnityEngine.Object obj2 = this.method_4(string_2, string_3, type_0);
            if (obj2 == null)
            {
                return new Class25("Load object {0}:{1} failed.", new object[] { string_2, string_3 });
            }
            return new Class25(obj2, 0f);
        }

        internal WaitLoadLevel method_7(string string_2, string string_3, LoadSceneMode loadSceneMode_0)
        {
            return new Class20(string_2, string_3, loadSceneMode_0, UnityEngine.Random.Range(this.method_0(), this.method_1()));
        }

        internal WaitLoadLevel method_8(string string_2, string string_3, LoadSceneMode loadSceneMode_0)
        {
            return new Class21(string_2, string_3, loadSceneMode_0);
        }

        private void method_9()
        {
            switch (this.SimulateLoadModeInEditor)
            {
                case 0:
                    this.int_1 = 0;
                    this.float_0 = 0f;
                    this.float_1 = 0f;
                    break;

                case 1:
                    this.int_1 = 1;
                    this.float_0 = 0f;
                    this.float_1 = 0f;
                    break;

                case 2:
                    this.int_1 = 1;
                    this.float_0 = 0f;
                    this.float_1 = 0.1f;
                    break;

                case 3:
                    this.int_1 = 1;
                    this.float_0 = 0f;
                    this.float_1 = 0.5f;
                    break;

                case 4:
                    this.int_1 = 1;
                    this.float_0 = 0.5f;
                    this.float_1 = 0.5f;
                    break;
            }
        }

        public bool SimulateAssetBundle
        {
            get
            {
                if (this.int_1 < 0)
                {
                    this.method_9();
                }
                return (this.int_1 > 0);
            }
        }

        public bool SimulateAssetBundleFailed
        {
            get
            {
                if (this.int_2 < 0)
                {
                    this.int_2 = !EditorPrefs.GetBool("SimulateAssetBundleFailed", false) ? 0 : 1;
                }
                return (this.int_2 > 0);
            }
            set
            {
                if ((this.int_2 > 0) != value)
                {
                    this.int_2 = !value ? 0 : 1;
                    EditorPrefs.SetBool("SimulateAssetBundleFailed", value);
                }
            }
        }

        public int SimulateLoadModeInEditor
        {
            get
            {
                if (this.int_0 == -1)
                {
                    this.int_0 = EditorPrefs.GetInt("SimulateAssetBundlesMode", 0);
                }
                return this.int_0;
            }
            set
            {
                if (value != this.int_0)
                {
                    this.int_0 = value;
                    EditorPrefs.SetInt("SimulateAssetBundlesMode", value);
                }
            }
        }
    }
}

