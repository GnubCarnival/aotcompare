// Decompiled with JetBrains decompiler
// Type: BMFont
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
  [HideInInspector]
  [SerializeField]
  private int mBase;
  private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
  [HideInInspector]
  [SerializeField]
  private int mHeight;
  [HideInInspector]
  [SerializeField]
  private List<BMGlyph> mSaved = new List<BMGlyph>();
  [HideInInspector]
  [SerializeField]
  private int mSize;
  [HideInInspector]
  [SerializeField]
  private string mSpriteName;
  [HideInInspector]
  [SerializeField]
  private int mWidth;

  public void Clear()
  {
    this.mDict.Clear();
    this.mSaved.Clear();
  }

  public BMGlyph GetGlyph(int index) => this.GetGlyph(index, false);

  public BMGlyph GetGlyph(int index, bool createIfMissing)
  {
    BMGlyph glyph = (BMGlyph) null;
    if (this.mDict.Count == 0)
    {
      int index1 = 0;
      for (int count = this.mSaved.Count; index1 < count; ++index1)
      {
        BMGlyph bmGlyph = this.mSaved[index1];
        this.mDict.Add(bmGlyph.index, bmGlyph);
      }
    }
    if (!this.mDict.TryGetValue(index, out glyph) & createIfMissing)
    {
      glyph = new BMGlyph() { index = index };
      this.mSaved.Add(glyph);
      this.mDict.Add(index, glyph);
    }
    return glyph;
  }

  public void Trim(int xMin, int yMin, int xMax, int yMax)
  {
    if (!this.isValid)
      return;
    int index = 0;
    for (int count = this.mSaved.Count; index < count; ++index)
      this.mSaved[index]?.Trim(xMin, yMin, xMax, yMax);
  }

  public int baseOffset
  {
    get => this.mBase;
    set => this.mBase = value;
  }

  public int charSize
  {
    get => this.mSize;
    set => this.mSize = value;
  }

  public int glyphCount => this.isValid ? this.mSaved.Count : 0;

  public bool isValid => this.mSaved.Count > 0;

  public string spriteName
  {
    get => this.mSpriteName;
    set => this.mSpriteName = value;
  }

  public int texHeight
  {
    get => this.mHeight;
    set => this.mHeight = value;
  }

  public int texWidth
  {
    get => this.mWidth;
    set => this.mWidth = value;
  }
}
