namespace GStd {
    using System;
    using UnityEngine;

    [Serializable]
    public class LanguageSetting
    {
        [SerializeField]
        private string name = "name";
        [SerializeField]
        private string csvFilePath = "csv file path";
    }

    [CreateAssetMenu(
     fileName = "I18NGenerate",
     menuName = "GStd/I18N/Generate")]
    public class I18NGenerate : ScriptableObject
    {
        [SerializeField]
        private string uiTextRegex = "[\u4e00-\u9fa5]";
        [SerializeField]
        private string scriptRegex = "(?<=\")([^\"]*)([\u4e00-\u9fa5]+)([^\"]*)(?=\")";
        //private string scriptRegex = "(?<=\")[^\"]*[\u4e00-\u9fa5]+[^\"]*(?=\")|(?<=\')[^\']*[\u4e00-\u9fa5]+[^\']*(?=\')";

        [SerializeField]
        private string uiPrefabRootPath = "Assets/Game/UIs";
        [SerializeField]
        private string scriptFileRootPath = "Assets/Game/Lua";
        [SerializeField]
        private string originCsvFilePath = "Assets/Game/I18N/translate_zh_chs.csv";
        //[SerializeField]
        //private string translatedCsvFilePath = "Assets/Game/I18N/zh_chs_translate_to_cht.csv";
        //[SerializeField]
        //private string finalCsvFilePath = "Assets/Game/I18N/translate_zh_cht.csv";
        //[SerializeField]
        //private string specialCsvFilePath = "Assets/Game/I18N/zh_cht_special_replace.csv";

        [SerializeField]
        private string refImageRegex = "/SpecialFonts+|/BigImages+";

        [SerializeField]
        private string refFontRegex = "/Fonts+|/TTF+";

        [SerializeField]
        private LanguageSetting[] languages;
    }
}


