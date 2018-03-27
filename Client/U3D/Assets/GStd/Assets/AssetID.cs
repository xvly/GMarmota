namespace GStd
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEditor;
    using UnityEngine;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AssetID : IEquatable<AssetID>
    {
        public static readonly AssetID Empty;
        [SerializeField]
        private string bundleName;
        [SerializeField]
        private string assetName;
        [SerializeField]
        private string assetGUID;
        public AssetID(string bundleName, string assetName)
        {
            this.bundleName = bundleName;
            this.assetName = assetName;
            this.assetGUID = string.Empty;
        }

        static AssetID()
        {
            Empty = new AssetID(string.Empty, string.Empty);
        }

        public string BundleName
        {
            get
            {
                return this.bundleName;
            }
        }
        public string AssetName
        {
            get
            {
                return this.assetName;
            }
        }
        public string AssetGUID
        {
            get
            {
                return this.assetGUID;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return (string.IsNullOrEmpty(this.BundleName) || string.IsNullOrEmpty(this.AssetName));
            }
        }
        public static AssetID Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new AssetID();
            }
            int index = text.IndexOf(':');
            if (index < 0)
            {
                throw new FormatException("Can not parse AssetID.");
            }
            string bundleName = text.Substring(0, index);
            return new AssetID(bundleName, text.Substring(index + 1));
        }

        public T LoadObject<T>() where T: UnityEngine.Object
        {
            if (!this.IsEmpty)
            {
                string str = null;
                if (!string.IsNullOrEmpty(this.assetGUID))
                {
                    str = AssetDatabase.GUIDToAssetPath(this.assetGUID);
                }
                if (string.IsNullOrEmpty(str))
                {
                    string[] assetPathsFromAssetBundleAndAssetName = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(this.bundleName, Path.GetFileNameWithoutExtension(this.assetName));
                    if (!Path.HasExtension(this.assetName))
                    {
                        if (assetPathsFromAssetBundleAndAssetName.Length > 0)
                        {
                            str = assetPathsFromAssetBundleAndAssetName[0];
                        }
                    }
                    else
                    {
                        string extension = Path.GetExtension(this.assetName);
                        foreach (string str3 in assetPathsFromAssetBundleAndAssetName)
                        {
                            if (Path.GetExtension(str3) == extension)
                            {
                                str = str3;
                                break;
                            }
                        }
                    }
                }
                if (str != null)
                {
                    return AssetDatabase.LoadAssetAtPath<T>(str);
                }
            }
            return null;
        }

        public override string ToString()
        {
            return (this.BundleName + ": " + this.AssetName);
        }

        public bool Equals(AssetID other)
        {
            return ((this.BundleName == other.BundleName) && (this.AssetName == other.AssetName));
        }

        public override int GetHashCode()
        {
            int hashCode = this.bundleName.GetHashCode();
            return ((0x18d * hashCode) ^ this.assetName.GetHashCode());
        }
    }
}

