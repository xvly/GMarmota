namespace GStd
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.IO;
    using CsvHelper;
    using System.Text.RegularExpressions;

    [CreateAssetMenu(
       fileName = "I18NConfig",
       menuName = "GStd/I18N/Config")]
    public class I18NConfig : ScriptableObject
    {
        private static I18NConfig instance;
        public static I18NConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = AssetManager.LoadObjectLocal("i18n/i18nconfig", "I18NConfig.asset", typeof(I18NConfig)) as I18NConfig;
                    Debug.Log("[I18NConfig]running " + instance.Language);
                    instance.LoadAsset();
                }
                return instance;
            }
        }

        void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            instance = this;
        }

        [SerializeField]
        private string language;
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                if (language != value)
                {
                    OnLanguageChanged(value);
                }
            }
        }

        // 词典
        private Dictionary<string, string> dictionary;

        // 判断是否需要翻译，简体中文是母语言，不需要翻译
        public bool IsNeedTranslate
        {
            get
            {
                return this.language != "zh_chs";
            }
        }

        // 翻译
        public string TranslateText(string key)
        {
            if (dictionary == null)
            {
                LoadAsset();
                Debug.LogWarning("I18NConfig must be inited before translate");
                return null;
            }

            if (!IsNeedTranslate)
                return key;

            string ret;
            if (!dictionary.TryGetValue(key, out ret))
            {
                return key;
            }

            return ret;
        }

        public string TranslateABPath(string path)
        {
            if (!IsNeedTranslate)
                return path;
            else
                return string.Format("{0}/{1}", language.ToString().ToLower(), path);
        }

        private LinkedList<Action> onLanguageChanged = new LinkedList<Action>();

        // 监听切换语言
        public LinkedListNode<Action> ListenLanguageChanged(Action action)
        {
            return onLanguageChanged.AddLast(action);
        }

        // 取消监听切换语言
        public void UnListenLanguageChanged(LinkedListNode<Action> node)
        {
            onLanguageChanged.Remove(node);
        }

        // 接收到切换语言事件
        public void OnLanguageChanged(string newLanguage)
        {
            language = newLanguage;

            // 加载语言文件
            LoadAsset();
        }

        private void LoadAsset()
        {
            dictionary = new Dictionary<string, string>();
            // origin language
            if (!IsNeedTranslate)
                return;

            //string ab = string.Format("{0}/translate", language.ToString().ToLower());
            //string asset = string.Format("translate_{0}", language.ToString());
            ////TextAsset textAsset = AssetManager.LoadObjectLocal(ab, asset, typeof(TextAsset)) as TextAsset;
            //if (textAsset == null)
            //{
            //    Debug.LogError("load language config failed, name = " + ab + "," + asset);
            //    return;
            //}

            //using (CsvReader cr = new CsvReader(new StringReader(textAsset.text)))
            //{
            //    while (cr.Read())
            //    {
            //        dictionary[cr[0]] = cr[1];
            //    }
            //}

            //AssetManager.UnloadAsseBundle(ab);
        }
    }
}