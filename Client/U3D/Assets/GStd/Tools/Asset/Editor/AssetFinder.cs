using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using GStd;

public class AssetFinderWindow: EditorWindow
{
    List<UnityEngine.Object> objs;
    Vector2 scrollPos = Vector2.zero;

    string floder = "Assets/Game";
    string assetType = "t:prefab";

    string assemblyName = "Assembly-CSharp";
    string componentTypeName = "";
    Sprite matchSprite = null;

    string assetBundleName = "";

    bool isFoldChild = false;
    string childName = "";
    string childAssemblyName = "";
    string childTypeName = "";
    Sprite childMatchImage;
    
    string nameRegex = "";
    LayerMask notequalLayer = 0;
    bool isNotequalLayer = false;
    LayerMask equalLayer = 0;
    bool isEuqalLayer = false;

    int fontSize = 20;
    string setAssetBundleName = "";

    GameObject replacePrefab;
    GameObject toAddPrefab;

    void OnGUI()
    {
        floder = EditorGUILayout.TextField("floder", floder);
        assetType = EditorGUILayout.TextField("asset type", assetType);

        nameRegex = EditorGUILayout.TextField("name regex", nameRegex);

        assemblyName = EditorGUILayout.TextField("assembly name", assemblyName);
        componentTypeName = EditorGUILayout.TextField("component type", componentTypeName);
        if (this.componentTypeName == "UnityEngine.UI.Image")
            this.matchSprite = EditorGUILayout.ObjectField("match sprite:", this.matchSprite, typeof(Sprite), false) as Sprite;
        else
            this.matchSprite = null;

        this.assetBundleName = EditorGUILayout.TextField("assetBundleName", this.assetBundleName);

        this.isFoldChild = EditorGUILayout.Foldout(this.isFoldChild, "child");
        if (this.isFoldChild)
        {
            childName = EditorGUILayout.TextField("obj name", childName);
            this.childAssemblyName = EditorGUILayout.TextField("Assembly", this.childAssemblyName);
            this.childTypeName = EditorGUILayout.TextField("Type", this.childTypeName);

            if (this.childTypeName == "UnityEngine.UI.Image")
            {
                this.childMatchImage = EditorGUILayout.ObjectField("match sprite:", this.childMatchImage, typeof(Sprite), false) as Sprite;
            }

            GUILayout.Space(5);
        }

        GUILayout.BeginHorizontal();
        isNotequalLayer = GUILayout.Toggle(isNotequalLayer, "notequal layer");
        if (isNotequalLayer)
            notequalLayer = 1 << EditorGUILayout.LayerField("", notequalLayer.ToInt());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        isEuqalLayer = GUILayout.Toggle(isEuqalLayer, "equal layer");
        if (isEuqalLayer)
            equalLayer = 1 << EditorGUILayout.LayerField("", equalLayer.ToInt());
        GUILayout.EndHorizontal();

        if (GUILayout.Button("find"))
        {
            objs = new List<UnityEngine.Object>();
            Find();
        }

        if (objs != null)
        {
            EditorGUILayout.Space();

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach(var obj in objs)
            {
                EditorGUILayout.ObjectField(obj, typeof(GameObject), false);
            }
            GUILayout.EndScrollView();

            // replace to notequal layer
            if (isNotequalLayer && this.childName != "" && GUILayout.Button("Fix child layer to notequal value"))
            {
                foreach(var obj in objs)
                {
                    var child = (obj as GameObject).transform.FindRecursively(this.childName);
                    if (child != null)
                        child.gameObject.layer = this.notequalLayer.ToInt();
                }
            }

            // replace child
            if (this.childName != "")
            {
                GUILayout.BeginHorizontal();
                replacePrefab = EditorGUILayout.ObjectField(replacePrefab, typeof(GameObject), true) as GameObject;
                if (GUILayout.Button("replace child"))
                {
                    if (replacePrefab == null)
                    {
                        //Debug.LogError("must set replace prefab");

                        List<UnityEngine.Object> tmpList = new List<UnityEngine.Object>();
                        foreach (var obj in objs)
                        {
                            var prefabInst = GameObject.Instantiate(obj) as GameObject;
                            var child = prefabInst.transform.FindRecursively(this.childName);
                            GameObject.DestroyImmediate(child.gameObject);
                            tmpList.Add(PrefabUtility.ReplacePrefab(prefabInst, obj));
                            GameObject.DestroyImmediate(prefabInst);
                        }
                        objs = tmpList;
                    }
                    else
                    {
                        List<UnityEngine.Object> tmpList = new List<UnityEngine.Object>();
                        foreach (var obj in objs)
                        {
                            var prefabInst = GameObject.Instantiate(obj) as GameObject;

                            var child = prefabInst.transform.FindRecursively(this.childName);
                            var siblingIndex = child.transform.GetSiblingIndex();
                            GameObject.DestroyImmediate(child.gameObject);
                            var replaceInst = GameObject.Instantiate(replacePrefab, prefabInst.transform, false) as GameObject;
                            replaceInst.transform.SetSiblingIndex(siblingIndex);
                            replaceInst.name = this.childName;
                            tmpList.Add(PrefabUtility.ReplacePrefab(prefabInst, obj));

                            GameObject.DestroyImmediate(prefabInst);
                        }
                        objs = tmpList;
                    }
                }
                GUILayout.EndHorizontal();
            }

            // add child
            if (this.assetType == "t:prefab")
            {
                GUILayout.BeginHorizontal();
                toAddPrefab = EditorGUILayout.ObjectField(toAddPrefab, typeof(GameObject), true) as GameObject;
                if (GUILayout.Button("add force"))
                {
                    if (toAddPrefab == null)
                    {
                        Debug.LogError("must set add prefab");
                    }
                    else
                    {
                        List<UnityEngine.Object> tmpList = new List<UnityEngine.Object>();
                        foreach (var obj in objs)
                        {
                            var prefabInst = GameObject.Instantiate(obj) as GameObject;

                            var toAddInst = GameObject.Instantiate(toAddPrefab, prefabInst.transform, false) as GameObject;
                            toAddInst.name = toAddPrefab.name; // remove (clone)
                            tmpList.Add(PrefabUtility.ReplacePrefab(prefabInst, obj));

                            GameObject.DestroyImmediate(prefabInst);
                        }
                        objs = tmpList;
                    }
                }

                if (GUILayout.Button("add if"))
                {
                    if (toAddPrefab == null)
                    {
                        Debug.LogError("must set add prefab");
                    }
                    else
                    {
                        List<UnityEngine.Object> tmpList = new List<UnityEngine.Object>();
                        foreach (var obj in objs)
                        {
                            if ((obj as GameObject).transform.Find(toAddPrefab.name))
                                continue;

                            var prefabInst = GameObject.Instantiate(obj) as GameObject;

                            var toAddInst = GameObject.Instantiate(toAddPrefab, prefabInst.transform, false) as GameObject;
                            toAddInst.name = toAddPrefab.name; // remove (clone)
                            tmpList.Add(PrefabUtility.ReplacePrefab(prefabInst, obj));

                            GameObject.DestroyImmediate(prefabInst);
                        }
                        objs = tmpList;
                    }
                }
                GUILayout.EndHorizontal();
            }

            // TMP:change fondsize
            GUILayout.BeginHorizontal();
            int.TryParse(GUILayout.TextArea(fontSize.ToString()), out fontSize);
            if (GUILayout.Button("font set"))
            {
                foreach(var obj in objs)
                {
                    var components = (obj as GameObject).GetComponentsInChildren<Image>(true);
                    foreach(var component in components)
                    {
                        if (component.sprite == matchSprite)
                        {
                            var texts = component.transform.GetComponentsInChildren<Text>();
                            foreach(var text in texts)
                            {
                                text.fontSize = fontSize;
                            }
                        }
                    }
                    EditorUtility.SetDirty(obj);
                }
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            this.setAssetBundleName = EditorGUILayout.TextArea(this.setAssetBundleName);
            if (GUILayout.Button("set ab name"))
            {
                foreach(var obj in objs)
                {
                    var ai = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj));
                    ai.assetBundleName = this.setAssetBundleName;
                    EditorUtility.SetDirty(ai);
                }
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("init all"))
            {
                foreach(var obj in objs)
                {
                    GameObject.Instantiate(obj);
                }
            }
           
        }
    }

    void Find()
    {
        objs = new List<UnityEngine.Object>();

        Type componentType = null;
        if (this.componentTypeName != "" && this.assemblyName != "")
            componentType = Assembly.Load(this.assemblyName).GetType(this.componentTypeName, true);

        Type childComponentType = null;
        if (this.childAssemblyName != "" && this.childTypeName != "")
            childComponentType = Assembly.Load(this.childAssemblyName).GetType(this.childTypeName, true);

        var guids = AssetDatabase.FindAssets(assetType, new string[] { floder });
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj == null)
                continue;

            bool isPass = true;
            if (nameRegex != "")
            {
                isPass = Regex.IsMatch(obj.name, nameRegex);
            }

            if (isPass && this.assetBundleName != "")
            {
                if (AssetImporter.GetAtPath(path).assetBundleName != this.assetBundleName)
                    isPass = false;
            }

            if (isPass && componentType != null)
            {
                var go = obj as GameObject;
                if (go == null)
                    continue;

                var components = go.GetComponentsInChildren(componentType, true);
                if (components == null || components.Length == 0)
                    isPass = false;

                if (isPass && this.matchSprite != null)
                {
                    bool isAnyMatch = false;
                    foreach (var component in components)
                    {
                        var img = component as Image;
                        if (img.sprite == matchSprite)
                        {
                            isAnyMatch = true;
                            break;
                        }
                    }

                    if (!isAnyMatch)
                        isPass = false;
                }
            }

            if (isPass && this.childName != "")
            {
                var go = obj as GameObject;
                if (go == null)
                    continue;

                var child = go.transform.FindRecursively(this.childName);
                if (child == null)
                    isPass = false;
                else
                {
                    if (isNotequalLayer)
                    {
                        if (child.gameObject.layer == this.notequalLayer.ToInt())
                            isPass = false;
                    }

                    if (isEuqalLayer)
                    {
                        if (child.gameObject.layer != this.notequalLayer.ToInt())
                            isPass = false;
                    }
                }

                if (isPass && childComponentType != null)
                {
                    var components = child.GetComponentsInChildren(childComponentType, true);
                    if (components == null || components.Length == 0)
                        isPass = false;

                    if (isPass && childComponentType == typeof(Image) && childMatchImage != null)
                    {
                        bool isAnyMatch = false;
                        foreach(var component in components)
                        {
                            var img = component as Image;
                            if (img.sprite == childMatchImage)
                            {
                                isAnyMatch = true;
                                break;
                            }
                        }

                        if (!isAnyMatch)
                            isPass = false;
                    }
                }
            }

            if (isPass && isNotequalLayer)
            {
                var go = obj as GameObject;
                if (go.layer == this.notequalLayer.ToInt())
                    isPass = false;
            }

            if (isPass && isEuqalLayer)
            {
                var go = obj as GameObject;
                if (go.layer != this.notequalLayer.ToInt())
                    isPass = false;
            }

            if (isPass)
                objs.Add(obj);
        }

    }

    [MenuItem("Assets/GStd/Finder",  false)]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<AssetFinderWindow>();
    }
}
