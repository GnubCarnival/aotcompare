// Decompiled with JetBrains decompiler
// Type: TextureScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Threading;
using UnityEngine;

public class TextureScaler
{
  public static IEnumerator Scale(Texture2D tex, int newWidth, int newHeight)
  {
    Color[] pixels = tex.GetPixels();
    Color[] newColors = new Color[newWidth * newHeight];
    Color[] newColors1 = newColors;
    int width = ((Texture) tex).width;
    int height = ((Texture) tex).height;
    int newWidth1 = newWidth;
    int newHeight1 = newHeight;
    TextureScaler.ThreadData parameter = new TextureScaler.ThreadData(pixels, newColors1, width, height, newWidth1, newHeight1);
    Thread thread = new Thread(new ParameterizedThreadStart(TextureScaler.BilinearScale));
    thread.Start((object) parameter);
    while (thread.IsAlive)
      yield return (object) new WaitForEndOfFrame();
    tex.Resize(newWidth, newHeight);
    tex.SetPixels(newColors);
    yield return (object) new WaitForEndOfFrame();
    tex.Apply();
  }

  public static void BilinearScale(object obj)
  {
    TextureScaler.ThreadData threadData = (TextureScaler.ThreadData) obj;
    float num1 = (float) (1.0 / ((double) threadData.NewWidth / (double) (threadData.TexWidth - 1)));
    float num2 = (float) (1.0 / ((double) threadData.NewHeight / (double) (threadData.TexHeight - 1)));
    int texWidth = threadData.TexWidth;
    int newWidth = threadData.NewWidth;
    for (int index1 = 0; index1 < threadData.NewHeight; ++index1)
    {
      int num3 = (int) Mathf.Floor((float) index1 * num2);
      int num4 = num3 * texWidth;
      int num5 = (num3 + 1) * texWidth;
      int num6 = index1 * newWidth;
      for (int index2 = 0; index2 < newWidth; ++index2)
      {
        int num7 = (int) Mathf.Floor((float) index2 * num1);
        float num8 = (float) index2 * num1 - (float) num7;
        Color[] texColors = threadData.TexColors;
        threadData.NewColors[num6 + index2] = TextureScaler.ColorLerpUnclamped(TextureScaler.ColorLerpUnclamped(texColors[num4 + num7], texColors[num4 + num7 + 1], num8), TextureScaler.ColorLerpUnclamped(texColors[num5 + num7], texColors[num5 + num7 + 1], num8), (float) index1 * num2 - (float) num3);
      }
    }
  }

  private static Color ColorLerpUnclamped(Color c1, Color c2, float value) => new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);

  public class ThreadData
  {
    public Color[] TexColors;
    public Color[] NewColors;
    public int TexWidth;
    public int TexHeight;
    public int NewWidth;
    public int NewHeight;

    public ThreadData(
      Color[] texColors,
      Color[] newColors,
      int texWidth,
      int texHeight,
      int newWidth,
      int newHeight)
    {
      this.TexColors = texColors;
      this.NewColors = newColors;
      this.TexWidth = texWidth;
      this.TexHeight = texHeight;
      this.NewWidth = newWidth;
      this.NewHeight = newHeight;
    }
  }
}
