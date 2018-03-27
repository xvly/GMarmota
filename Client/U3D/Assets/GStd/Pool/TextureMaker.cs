namespace GStd
{
    using System;
    using UnityEngine;

    public static class TextureMaker
    {
        public static Texture2D CornerBottomLeft(int size, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            int num = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i < num) && (j < num))
                    {
                        textured.SetPixel(i, j, fg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, bg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D CornerBottomRight(int size, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            int num = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i > num) && (j < num))
                    {
                        textured.SetPixel(i, j, fg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, bg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D CornerTopLeft(int size, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            int num = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i < num) && (j > num))
                    {
                        textured.SetPixel(i, j, fg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, bg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D CornerTopRight(int size, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            int num = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i > num) && (j > num))
                    {
                        textured.SetPixel(i, j, fg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, bg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D Cross(int size, int thickness, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((Mathf.Abs((int) (i - j)) > thickness) && (Mathf.Abs((int) ((i + j) - size)) > thickness))
                    {
                        textured.SetPixel(i, j, bg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, fg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D Dot(int size, Color fg, Color bg)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            int num = size / 2;
            int num2 = (size / 2) * (size / 2);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int num5 = i - num;
                    int num6 = j - num;
                    if (((num5 * num5) + (num6 * num6)) < num2)
                    {
                        textured.SetPixel(i, j, fg);
                    }
                    else
                    {
                        textured.SetPixel(i, j, bg);
                    }
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D Gray(float gray)
        {
            return Monochromatic(new Color(gray, gray, gray, 1f));
        }

        public static Texture2D Gray(int size, float gray)
        {
            return Monochromatic(size, new Color(gray, gray, gray, 1f));
        }

        public static Texture2D Monochromatic(Color color)
        {
            Texture2D textured = new Texture2D(1, 1, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            textured.SetPixel(0, 0, color);
            textured.Apply();
            return textured;
        }

        public static Texture2D Monochromatic(int size, Color color)
        {
            Texture2D textured = new Texture2D(size, size, TextureFormat.ARGB32, false) {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    textured.SetPixel(i, j, color);
                }
            }
            textured.Apply();
            return textured;
        }
    }
}

