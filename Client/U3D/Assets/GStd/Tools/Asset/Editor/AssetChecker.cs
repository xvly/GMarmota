using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class AssetChecker: EditorWindow
{
    string atlasFloder = "Assets/Game/UIs/Views";
    TextureImporter[] errorTextures;
    Vector2 altasScrollPos;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("atlas");
        atlasFloder = GUILayout.TextField(atlasFloder);
        if (GUILayout.Button("check"))
        {
            this.CheckAtlas(atlasFloder);
        }
        GUILayout.EndHorizontal();
    }

    void CheckAtlas(string floder)
    {
        List<Texture> ret = new List<Texture>();

        var guids = AssetDatabase.FindAssets("t:texture", new string[] { floder });
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = TextureImporter.GetAtPath(path) as TextureImporter;

            var firstDir = Path.GetDirectoryName(path);
            firstDir = firstDir.Substring(firstDir.LastIndexOf("/")+1);

            TextureImporterSettings tis = new TextureImporterSettings();
            importer.ReadTextureSettings(tis);

            bool isAnyChange = false;

            if (importer.textureType != TextureImporterType.Sprite)
            { 
                importer.textureType = TextureImporterType.Sprite;
                Debug.Log("[textureType]" + path);

                isAnyChange = true;
            }

            if (importer.spritePackingTag != firstDir)
            {
                importer.spritePackingTag = firstDir;
                Debug.Log("[spritePackingTag]" + path + "," + firstDir);

                isAnyChange = true;
            }

            if (importer.spriteImportMode != SpriteImportMode.Single)
                Debug.Log("[spriteImportMode] must be single , " + path);

            if (tis.spriteMeshType != SpriteMeshType.FullRect)
            { 
                tis.spriteMeshType = SpriteMeshType.FullRect;
                Debug.Log("[spriteMeshType]" + path);
                importer.SetTextureSettings(tis);

                isAnyChange = true;
            }

            if (isAnyChange)
                importer.SaveAndReimport();
        }
    }

    [MenuItem("Assets/GStd/Check Atlas")]
    static void CheckAtlas()
    {
        GetWindow<AssetChecker>();
    }
}
