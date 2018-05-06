#if false

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GStd.Editor
{
	[CreateAssetMenu(fileName = "FontMaker", menuName = "GStd/FontMaker")]
	public sealed class FontMaker : ScriptableObject
	{
		[Serializable]
		private class Data  
		{
			[SerializeField]
			private string fontName;

			[SerializeField]
			private FontMaker.Glyph[] glyphs;

			public string GetName()
			{
				return this.fontName;
			}

			public FontMaker.Glyph[] GetGlyph()
			{
				return this.glyphs;
			}
		}

		[Serializable]
		private class Glyph
		{
			[SerializeField]
			private int code;

			[SerializeField]
			private Texture2D image;

			public int GetCode()
			{
				return this.code;
			}

			public Texture2D GetImage()
			{
				return this.image;
			}
		}

		[SerializeField]
		private string atlasName;

        [SerializeField]
        private int atlasPadding;

        [SerializeField]
        private int atlasSize;

        [SerializeField]
		private FontMaker.Data[] fonts;

		internal void Build(int padding=1, int size=4096)
		{
			EditorUtility.DisplayProgressBar("Make Font", "Process font images...", 0f);
			List<Texture2D> list = new List<Texture2D>();
			FontMaker.Data[] array = this.fonts;
			for (int i = 0; i < array.Length; i++)
			{
				FontMaker.Data data = array[i];
				FontMaker.Glyph[] array2 = data.GetGlyph();
				for (int j = 0; j < array2.Length; j++)
				{
					FontMaker.Glyph glyph = array2[j];
					string assetPath = AssetDatabase.GetAssetPath(glyph.GetImage());
					TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
					if (textureImporter != null)
					{
						bool flag = false;
						if (!textureImporter.isReadable)
						{
							textureImporter.isReadable = true;
							flag = true;
						}
						if (textureImporter.textureCompression != null)
						{
							textureImporter.textureCompression = 0;
							flag = true;
						}
						if (textureImporter.mipmapEnabled)
						{
							textureImporter.mipmapEnabled = false;
							flag = true;
						}
						if (textureImporter.npotScale != null)
						{
							textureImporter.npotScale = 0;
							flag = true;
						}
						if (flag)
						{
							textureImporter.SaveAndReimport();
						}
					}
					list.Add(glyph.GetImage());
				}
			}
			EditorUtility.DisplayProgressBar("Make Font", "Build font atlas...", 0.25f);
			Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, false);
			texture2D.name = "Font Atlas";
			Rect[] array3 = texture2D.PackTextures(list.ToArray(), padding, size, false);
			string assetPath2 = AssetDatabase.GetAssetPath(this);
			string directoryName = Path.GetDirectoryName(assetPath2);
			string text = this.atlasName;
			if (!text.EndsWith(".png") || !text.EndsWith(".PNG"))
			{
				text += ".png";
			}
			string text2 = Path.Combine(directoryName, text);
			byte[] array4 = texture2D.EncodeToPNG();
			FileStream fileStream = File.OpenWrite(text2);
			fileStream.Write(array4, 0, array4.Length);
			fileStream.Close();
			AssetDatabase.Refresh();
			TextureImporter textureImporter2 = AssetImporter.GetAtPath(text2) as TextureImporter;
			if (textureImporter2 != null)
			{
				bool flag2 = false;
				if (textureImporter2.isReadable)
				{
					textureImporter2.isReadable = false;
					flag2 = true;
				}
				if (!textureImporter2.alphaIsTransparency)
				{
					textureImporter2.alphaIsTransparency = true;
					flag2 = true;
				}
				if (flag2)
				{
					textureImporter2.SaveAndReimport();
				}
			}
			Texture mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(text2);
			EditorUtility.DisplayProgressBar("Make Font", "Build font material...", 0.35f);
			string path = this.atlasName + ".mat";
			string text3 = Path.Combine(directoryName, path);
			Material material = AssetDatabase.LoadAssetAtPath<Material>(text3);
			if (material == null)
			{
				Shader shader = Shader.Find("Transparent/Diffuse");
				material = new Material(shader);
				AssetDatabase.CreateAsset(material, text3);
			}
			material.mainTexture = mainTexture;
			EditorUtility.SetDirty(material);
			AssetDatabase.SaveAssets();
			EditorUtility.DisplayProgressBar("Make Font", "Build font settings...", 0.7f);
			int num = 0;
			FontMaker.Data [] array5 = this.fonts;
			for (int k = 0; k < array5.Length; k++)
			{
				FontMaker.Data data = array5[k];
				FontMaker.Glyph[] array6 = data.GetGlyph();
				if (array6.Length != 0)
				{
					string text4 = data.GetName();
					if (!text4.EndsWith(".fontsettings"))
					{
						text4 += ".fontsettings";
					}
					string text5 = Path.Combine(directoryName, text4);
					Font font = AssetDatabase.LoadAssetAtPath<Font>(text5);
					if (font == null)
					{
						font = new Font();
						AssetDatabase.CreateAsset(font, text5);
					}
					float num2 = 0f;
					CharacterInfo[] array7 = new CharacterInfo[array6.Length];
					for (int l = 0; l < array6.Length; l++)
					{
						FontMaker.Glyph glyph = array6[l];
						Texture2D texture2D2 = glyph.GetImage();

						Rect rect = array3[num++];
						if (num2 < (float)texture2D2.height)
						{
							num2 = (float)texture2D2.height;
						}
						CharacterInfo characterInfo = default(CharacterInfo);
						characterInfo.index = glyph.GetCode();
						float x = rect.x;
						float y = rect.y;
						float width = rect.width;
						float height = rect.height;
						characterInfo.uvBottomLeft = new Vector2(x, y);
						characterInfo.uvBottomRight = new Vector2(x + width, y);
						characterInfo.uvTopLeft = new Vector2(x, y + height);
						characterInfo.uvTopRight = new Vector2(x + width, y + height);
						characterInfo.minX = 0;
						characterInfo.minY = -(texture2D2.height);
						characterInfo.maxX = texture2D2.width;
						characterInfo.maxY = 0;
						characterInfo.advance = texture2D2.width;
						array7[l] = characterInfo;
					}
					font.characterInfo = array7;
					font.material = material;
					SerializedObject serializedObject = new SerializedObject(font);
					serializedObject.Update();
					SerializedProperty serializedProperty = serializedObject.FindProperty("m_LineSpacing");
					serializedProperty.floatValue = num2;
					serializedObject.ApplyModifiedPropertiesWithoutUndo();
					EditorUtility.SetDirty(font); 
				}
			}
			EditorUtility.DisplayProgressBar("Make Font", "Save and refresh assets.", 0.95f);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.ClearProgressBar();
		}
	}
}
#endif
