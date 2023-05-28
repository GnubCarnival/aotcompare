// Decompiled with JetBrains decompiler
// Type: SnapShotSavesOld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class SnapShotSavesOld
{
  private static int currentIndex;
  private static int[] dmg;
  private static Texture2D[] img;
  private static int index;
  private static bool inited;
  private static int maxIndex;

  public static void addIMG(Texture2D t, int d)
  {
    SnapShotSavesOld.init();
    SnapShotSavesOld.img[SnapShotSavesOld.index] = t;
    SnapShotSavesOld.dmg[SnapShotSavesOld.index] = d;
    SnapShotSavesOld.currentIndex = SnapShotSavesOld.index;
    ++SnapShotSavesOld.index;
    if (SnapShotSavesOld.index >= SnapShotSavesOld.img.Length)
      SnapShotSavesOld.index = 0;
    SnapShotSavesOld.maxIndex = Mathf.Max(SnapShotSavesOld.index, SnapShotSavesOld.maxIndex);
  }

  public static int getCurrentDMG() => SnapShotSavesOld.maxIndex == 0 ? 0 : SnapShotSavesOld.dmg[SnapShotSavesOld.currentIndex];

  public static Texture2D getCurrentIMG() => SnapShotSavesOld.maxIndex == 0 ? (Texture2D) null : SnapShotSavesOld.img[SnapShotSavesOld.currentIndex];

  public static int getCurrentIndex() => SnapShotSavesOld.currentIndex;

  public static int getLength() => SnapShotSavesOld.maxIndex;

  public static int getMaxIndex() => SnapShotSavesOld.maxIndex;

  public static Texture2D GetNextIMG()
  {
    ++SnapShotSavesOld.currentIndex;
    if (SnapShotSavesOld.currentIndex >= SnapShotSavesOld.maxIndex)
      SnapShotSavesOld.currentIndex = 0;
    return SnapShotSavesOld.getCurrentIMG();
  }

  public static Texture2D GetPrevIMG()
  {
    --SnapShotSavesOld.currentIndex;
    if (SnapShotSavesOld.currentIndex < 0)
      SnapShotSavesOld.currentIndex = SnapShotSavesOld.maxIndex - 1;
    return SnapShotSavesOld.getCurrentIMG();
  }

  public static void init()
  {
    if (SnapShotSavesOld.inited)
      return;
    SnapShotSavesOld.inited = true;
    SnapShotSavesOld.index = 0;
    SnapShotSavesOld.maxIndex = 0;
    SnapShotSavesOld.img = new Texture2D[99];
    SnapShotSavesOld.dmg = new int[99];
  }
}
