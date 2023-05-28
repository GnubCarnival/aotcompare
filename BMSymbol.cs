// Decompiled with JetBrains decompiler
// Type: BMSymbol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

[Serializable]
public class BMSymbol
{
  private int mAdvance;
  private int mHeight;
  private bool mIsValid;
  private int mLength;
  private int mOffsetX;
  private int mOffsetY;
  private UIAtlas.Sprite mSprite;
  private Rect mUV;
  private int mWidth;
  public string sequence;
  public string spriteName;

  public void MarkAsDirty() => this.mIsValid = false;

  public bool Validate(UIAtlas atlas)
  {
    if (Object.op_Equality((Object) atlas, (Object) null))
      return false;
    if (!this.mIsValid)
    {
      if (string.IsNullOrEmpty(this.spriteName))
        return false;
      this.mSprite = Object.op_Equality((Object) atlas, (Object) null) ? (UIAtlas.Sprite) null : atlas.GetSprite(this.spriteName);
      if (this.mSprite != null)
      {
        Texture texture = atlas.texture;
        if (Object.op_Equality((Object) texture, (Object) null))
        {
          this.mSprite = (UIAtlas.Sprite) null;
        }
        else
        {
          Rect rect = this.mSprite.outer;
          this.mUV = rect;
          if (atlas.coordinates == UIAtlas.Coordinates.Pixels)
            this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
          else
            rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, true);
          this.mOffsetX = Mathf.RoundToInt(this.mSprite.paddingLeft * ((Rect) ref rect).width);
          this.mOffsetY = Mathf.RoundToInt(this.mSprite.paddingTop * ((Rect) ref rect).width);
          this.mWidth = Mathf.RoundToInt(((Rect) ref rect).width);
          this.mHeight = Mathf.RoundToInt(((Rect) ref rect).height);
          this.mAdvance = Mathf.RoundToInt(((Rect) ref rect).width + (this.mSprite.paddingRight + this.mSprite.paddingLeft) * ((Rect) ref rect).width);
          this.mIsValid = true;
        }
      }
    }
    return this.mSprite != null;
  }

  public int advance => this.mAdvance;

  public int height => this.mHeight;

  public int length
  {
    get
    {
      if (this.mLength == 0)
        this.mLength = this.sequence.Length;
      return this.mLength;
    }
  }

  public int offsetX => this.mOffsetX;

  public int offsetY => this.mOffsetY;

  public Rect uvRect => this.mUV;

  public int width => this.mWidth;
}
