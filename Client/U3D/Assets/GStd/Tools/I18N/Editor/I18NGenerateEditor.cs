namespace GStd.Editor {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEditor;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.IO;
    using System;
    using UnityEditorInternal;
    using CsvHelper;
    //using GStd;
    //using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
    using Microsoft.VisualBasic;

    [CustomEditor(typeof(I18NGenerate))]
    public class I18NGenerateEditor : Editor
    {
        private SerializedProperty uiTextRegex;
        private SerializedProperty scriptRegex;

        private SerializedProperty languages;
        private ReorderableList languageList;

        private SerializedProperty refFontRegex;
        private SerializedProperty refImageRegex;

        private bool isExpandTranslateStep = false;

        private void OnEnable()
        {
            this.uiTextRegex = this.serializedObject.FindProperty("uiTextRegex");
            this.scriptRegex = this.serializedObject.FindProperty("scriptRegex");

            this.refFontRegex = this.serializedObject.FindProperty("refFontRegex");
            this.refImageRegex = this.serializedObject.FindProperty("refImageRegex");

            this.languages = this.serializedObject.FindProperty("languages");
            this.languageList = new ReorderableList(this.serializedObject, this.languages);
            this.languageList.drawHeaderCallback = rect => GUI.Label(rect, "languages");
            this.languageList.elementHeight = this.languages.arraySize * EditorGUIUtility.singleLineHeight * 2;
            this.languageList.drawElementCallback = DrawLanguage;
        }

        private void DrawLanguage(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = this.languages.GetArrayElementAtIndex(index);

            var name = element.FindPropertyRelative("name");
            var csvFilePath = element.FindPropertyRelative("csvFilePath");

            Rect rt = new Rect(rect);
            rt.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rt, name);
            rt.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rt, csvFilePath);
        }
        
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.LabelField("Text");

            // csv path
            GUILayout.BeginHorizontal();
            GUILayout.Label("csv path", GUILayout.Width(50));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("originCsvFilePath"), new GUIContent(""));
            GUILayout.EndHorizontal();

            this.languages.isExpanded = EditorGUILayout.Foldout(this.languages.isExpanded, "languages");
            if (this.languages.isExpanded)
                this.languageList.DoLayoutList();

            // ui 
            GUILayout.BeginHorizontal();
            GUILayout.Label("ui", GUILayout.Width(50));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("uiPrefabRootPath"), new GUIContent(""));
            EditorGUILayout.PropertyField(this.uiTextRegex, new GUIContent(""));
            GUILayout.EndHorizontal();

            // script
            GUILayout.BeginHorizontal();
            GUILayout.Label("script", GUILayout.Width(50));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("scriptFileRootPath"), new GUIContent(""));
            EditorGUILayout.PropertyField(this.scriptRegex, new GUIContent(""));
            GUILayout.EndHorizontal();

            // 
            EditorGUILayout.PropertyField(this.refFontRegex);
            EditorGUILayout.PropertyField(this.refImageRegex);

            // generate button
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate origin csv file"))
                this.GenerateOriginCsvFile(false);
            if (GUILayout.Button("Regenerate origin csv file"))
                this.GenerateOriginCsvFile(true);

            // translate
            EditorGUILayout.Space();

            var curLanguage = I18NConfig.Instance.Language;
            int curLanguageIndex = 0;
            string[] languageNames = new string[this.languages.arraySize];
            for (int i=0; i<this.languages.arraySize; i++)
            {
                var stringValue = this.languages.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
                // record
                languageNames[i] = stringValue;

                // match
                if (stringValue == curLanguage)
                    curLanguageIndex = i;
            }
            var newLanguageIndex = EditorGUILayout.Popup(curLanguageIndex, languageNames);
            if (newLanguageIndex != curLanguageIndex)
            {
                I18NConfig config = AssetDatabase.LoadAssetAtPath<I18NConfig>("Assets/Game/I18N/I18NConfig.asset");
                if (config == null)
                {
                    Debug.LogError("can not load i18nconfig");
                    return;
                }

                config.Language = languageNames[newLanguageIndex];
                EditorUtility.SetDirty(config);

                I18NConfig.Instance.Language = languageNames[newLanguageIndex];
            }

            if (GUILayout.Button("translate"))
            {
                if (!ReplaceResource())
                {
                    Debug.LogError("translate stop, break by replace resource");
                    return;
                }
                if (!TranslatePrefabText())
                {
                    Debug.LogError("translate stop, break by translate prefab text");
                    return;
                }

                if (!GenerateLuaTable())
                {
                    Debug.LogError("translate stop, break by generate lua table");
                    return;
                }
            }

            this.isExpandTranslateStep = EditorGUILayout.Foldout(this.isExpandTranslateStep, "translate steps");
            if (this.isExpandTranslateStep)
            {
                if (GUILayout.Button("replace resource"))
                    ReplaceResource();

                if (GUILayout.Button("translate prefab"))
                    TranslatePrefabText();

                if (GUILayout.Button("generate lua table"))
                    GenerateLuaTable();
            }

            if (GUILayout.Button("generate filter lua table"))
            {
                if (!GenerateFilterLuaTable("config_chatfilter","translate_chatfilter.csv"))
                {
                    Debug.LogError("generate chat filter faild");
                }
                if (!GenerateFilterLuaTable("config_usernamefilter","translate_usernamefilter.csv"))
                {
                    Debug.LogError("generate name filter faild");
                }
            }

            //
            this.serializedObject.ApplyModifiedProperties();
        }
        
        bool GenerateUIKeys(ref List<string> keys)
        {
            var rootPath = this.serializedObject.FindProperty("uiPrefabRootPath").stringValue;
            if (rootPath == "" | rootPath.StartsWith("#"))
                return true;

            var strRegex = this.uiTextRegex.stringValue;
            var guids = AssetDatabase.FindAssets("t:prefab", new string[] { rootPath });
            using (ProgressIndicator progress = new ProgressIndicator("generate ui keys", guids.Length))
            {
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (progress.Show("generating ui:{0}", path))
                        return false;

                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    var texts = prefab.GetComponentsInChildren<Text>(true);

                    foreach (var text in texts)
                    {
                        if (text.text != "" && Regex.IsMatch(text.text, strRegex))
                        {
                            if (!keys.Contains(text.text))
                                keys.Add(text.text);
                        }
                    }

                    progress.Next();
                }
            }

            AssetDatabase.SaveAssets();

            return true;
        }

        bool GenerateScriptKeys(ref List<string> keys)//todo
        {
            var rootPath = this.serializedObject.FindProperty("scriptFileRootPath").stringValue;
            if (rootPath == "" | rootPath.StartsWith("#"))
                return true;

            var strRegex = this.scriptRegex.stringValue;//todo
            var files = Directory.GetFiles(
                rootPath, "*.lua", SearchOption.AllDirectories);

            using (ProgressIndicator progress = new ProgressIndicator("generate script keys", files.Length))
            {
                foreach (var file in files)
                {
                    if (file.Contains("i18n_translate.lua"))
                        continue;

                    if (progress.Show("generating script:{0}", file))
                        return false;

                    using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                    {
                        //var content = sr.ReadToEnd();
                        //var matches = Regex.Matches(content, strRegex);
                        //for (int i=0; i<matches.Count; i++)
                        //{
                        //    var match = matches[i];
                        //    if (match.Value != "" && !keys.Contains(match.Value))
                        //        keys.Add(match.Value);
                        //}

                        string line = sr.ReadLine();
                        while (line != null)
                        {
                            var matches = Regex.Matches(line, strRegex);
                            for(int i=0; i<matches.Count; i++)
                            {
                                var match = matches[i];

                                string fixMatch = match.Value;
                                fixMatch = fixMatch.Remove(0, 1);
                                fixMatch = fixMatch.Substring(0, fixMatch.Length - 1);
                                //Debug.Log(fixMatch);
                                fixMatch = fixMatch.Replace("\\r\\n", "\r\n");
                                fixMatch = fixMatch.Replace("\\n", "\n");
                                if (fixMatch != "" && !keys.Contains(fixMatch))
                                    keys.Add(fixMatch);
                            }
                            
                            line = sr.ReadLine();
                        }
                    }

                    progress.Next();
                }
            }

            return true;
        }

        class CsvData
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        void GenerateOriginCsvFile(bool isClear)//isClear == false
        {
            Debug.Log("start generate origin csv file...");
            var originCsvFilePath = this.serializedObject.FindProperty("originCsvFilePath").stringValue;

            // 检测原始文件是否已经存在，记录起来做增量判断
            List<string> oldKeys = null;
            if (!isClear)
            {
                using (StreamReader sr = new StreamReader(originCsvFilePath, Encoding.UTF8))
                {
                    if (sr != null)
                    {
                        var content = sr.ReadToEnd();
                        if (content != "") // 空也当做没有
                        {
                            using (CsvReader cr = new CsvReader(new StringReader(content)))
                            {
                                oldKeys = new List<string>();
                                //cr.ReadHeader();
                                while (cr.Read())
                                {
                                    oldKeys.Add(cr[0]);
                                }
                            }
                        }

                    }
                }
            }

            // 解析脚本和ui
            List<string> keys = new List<string>();
            if (!this.GenerateScriptKeys(ref keys))
                return;
            if (!this.GenerateUIKeys(ref keys))
                return;

            using (ProgressIndicator progress = new ProgressIndicator("genreate csv", 2))
            {
                if (oldKeys != null)
                {
                    // 筛选出无用key和增量k
                    progress.Show("remove old keys and add new keys");
                    for (int i = oldKeys.Count - 1; i >= 0; i--)
                    //foreach(var k in oldKeys)
                    {
                        var oldK = oldKeys[i];
                        bool isFind = false;
                        foreach (var newK in keys)
                        {
                            if (newK == oldK)
                            {
                                isFind = true;
                                keys.Remove(newK);
                                break;
                            }
                        }

                        // 已经用不到的key就删掉
                        if (!isFind && !oldK.StartsWith("record"))
                        {
                            oldKeys.Remove(oldK);
                        }
                    }
                    progress.Next();
                }

                if (keys.Count > 0)
                {
                    progress.Show("generate csv file");
                    using (CsvWriter cw = new CsvWriter(new StreamWriter(originCsvFilePath, false, Encoding.UTF8)))
                    {
                        cw.WriteHeader(typeof(CsvData));
                        if (oldKeys != null)
                        {
                            // 旧key
                            foreach (var key in oldKeys)
                            {
                                cw.WriteField(key);
                                cw.WriteField("");
                                cw.NextRecord();
                            }
                        }

                        // 新key
                        foreach (var key in keys)
                        {
                            cw.WriteField(key);
                            cw.WriteField("");
                            cw.NextRecord();
                        }

                        cw.WriteField("record" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                        cw.WriteField("");
                        cw.NextRecord();
                    }
                    progress.Next();
                }
            }
                
            AssetDatabase.Refresh(ImportAssetOptions.Default);

            Debug.Log("finished");
        }

        bool ReplaceResource()
        {
            var directories = Directory.GetDirectories("Assets/Game", @".*_I18N", SearchOption.AllDirectories);
            
            foreach (var dir in directories)
            {
                var targetFolder = dir.Replace("\\.", "\\").Replace("_I18N", "");
                var i18nFolder = dir + "\\" + I18NConfig.Instance.Language;
                Debug.Log("!! dir " + dir + "," + targetFolder);

                using (ProgressIndicator progress = new ProgressIndicator("...", directories.Length))
                {

                    var filePaths = Directory.GetFiles(i18nFolder, "*", SearchOption.AllDirectories);
                    foreach (var filePath in filePaths)
                    {
                        //var fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                        var targetFilePath = filePath.Replace("\\.", "\\").Replace("_I18N\\zh_cht", "");

                        if (progress.Show(filePath + " -> " + targetFilePath))
                            return false;

                        if (!File.Exists(targetFilePath))
                        {
                            Debug.LogError("replace faild = " + filePath + " -> " + targetFilePath);
                            continue;
                        }

                        File.Copy(filePath, targetFilePath, true);

                        progress.Next();
                    }
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("replace i18n resources finish.");

            return true;
        }

        bool TranslatePrefabText()
        {
            // fonts
            var strRefFontRegex = "/Fonts+|/TTF+";
            var guids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Game/UIs" });
            var strRegex = this.uiTextRegex.stringValue;
            using (ProgressIndicator progress = new ProgressIndicator("...", guids.Length))
            {
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    var texts = prefab.GetComponentsInChildren<Text>(true);

                    if (progress.Show("translating prefab " + path))
                        return false;

                    bool isAnyChange = false;
                    foreach (var text in texts)
                    {
                        // text regex
                        if (text.text != "" && Regex.IsMatch(text.text, strRegex))
                        {
                            var translateText = I18NConfig.Instance.TranslateText(text.text);
                            if (string.IsNullOrEmpty(translateText))
                            {
                                Debug.LogError("translate text is null or empty, origin text = " + text.text);
                            }
                            else
                            {
                                text.text = translateText;
                                isAnyChange = true;
                            }
                        }

                        var assetPath = AssetDatabase.GetAssetPath(
                            text.font.GetInstanceID());

                        // font reference
                        if (Regex.IsMatch(assetPath, strRefFontRegex))
                        {
                            var translateAssetPath = assetPath.Replace("Fonts", "Fonts_I18N/zh_cht");
                            translateAssetPath = translateAssetPath.Replace("TTF", "TTF_I18N/zh_cht");
                            var font = AssetDatabase.LoadAssetAtPath<Font>(translateAssetPath);
                            if (font == null)
                            {
                                Debug.LogWarning("not found " + translateAssetPath);
                            }
                            else
                            {
                                text.font = font;
                                isAnyChange = true;
                            }
                        }
                    }

                    if (isAnyChange)
                        EditorUtility.SetDirty(prefab);

                    progress.Next();
                }
            }

            // 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("translate i18n prefab finish.");

            return true;
        }

        bool GenerateLuaTable()
        {
            string outputPath = "Assets/Game/Lua/config/i18n_translate.lua";

            using (StreamReader sr = new StreamReader("Assets/Game/I18N/translate_zh_cht.csv", Encoding.UTF8))
            {
                if (sr != null)
                {
                    var content = sr.ReadToEnd();
                    if (content != "") // 空也当做没有
                    {
                        
                        using (CsvReader cr = new CsvReader(new StringReader(content)))
                        {
                            StringBuilder builder = new StringBuilder("_i18n_map = {\n");

                            while (cr.Read())
                            {
                                var key = cr[0].Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"").Replace("\r", "\\r");
                                var value = cr[1].Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"").Replace("\r","\\r"); ;
                                builder.AppendLine(string.Format("\t[\"{0}\"] = \"{1}\",", key, value));
                            }

                            builder.AppendLine("}");

                            using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
                            {
                                sw.Write(builder.ToString());
                                sw.Flush();
                            }
                        }
                    }
                }
            }

            return true;
        }

        bool GenerateFilterLuaTable(string lua_file, string csv_file)
        {
            string outputPath = string.Concat("Assets/Game/Lua/config/", lua_file, ".lua");

            using (StreamReader sr = new StreamReader(string.Concat("Assets/Game/I18N/", csv_file), Encoding.UTF8))
            {
                if (sr != null)
                {
                    var content = sr.ReadToEnd();
                    if (content != "") // 空也当做没有
                    {

                        using (CsvReader cr = new CsvReader(new StringReader(content)))
                        {
                            StringBuilder builder = new StringBuilder(string.Concat(lua_file, "_list = {\n"));

                            while (cr.Read())
                            {
                                var value = cr[0].Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"").Replace("\r", "\\r"); 
                                builder.AppendLine(string.Format("\t\"{0}\",",value));
                            }

                            builder.AppendLine("}");

                            using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
                            {
                                sw.Write(builder.ToString());
                                sw.Flush();
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}