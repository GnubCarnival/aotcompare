// Decompiled with JetBrains decompiler
// Type: UIFont
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
  [HideInInspector]
  [SerializeField]
  private UIAtlas mAtlas;
  private static CharacterInfo mChar;
  private List<Color> mColors = new List<Color>();
  [HideInInspector]
  [SerializeField]
  private Font mDynamicFont;
  [HideInInspector]
  [SerializeField]
  private float mDynamicFontOffset;
  [SerializeField]
  [HideInInspector]
  private int mDynamicFontSize = 16;
  [SerializeField]
  [HideInInspector]
  private FontStyle mDynamicFontStyle;
  [SerializeField]
  [HideInInspector]
  private BMFont mFont = new BMFont();
  [SerializeField]
  [HideInInspector]
  private Material mMat;
  [SerializeField]
  [HideInInspector]
  private float mPixelSize = 1f;
  private int mPMA = -1;
  [HideInInspector]
  [SerializeField]
  private UIFont mReplacement;
  [HideInInspector]
  [SerializeField]
  private int mSpacingX;
  [SerializeField]
  [HideInInspector]
  private int mSpacingY;
  private UIAtlas.Sprite mSprite;
  private bool mSpriteSet;
  [SerializeField]
  [HideInInspector]
  private List<BMSymbol> mSymbols = new List<BMSymbol>();
  [HideInInspector]
  [SerializeField]
  private Rect mUVRect = new Rect(0.0f, 0.0f, 1f, 1f);

  public void AddSymbol(string sequence, string spriteName)
  {
    this.GetSymbol(sequence, true).spriteName = spriteName;
    this.MarkAsDirty();
  }

  private void Align(
    BetterList<Vector3> verts,
    int indexOffset,
    UIFont.Alignment alignment,
    int x,
    int lineWidth)
  {
    if (alignment == UIFont.Alignment.Left)
      return;
    int size = this.size;
    if (size <= 0)
      return;
    float num1;
    if (alignment == UIFont.Alignment.Right)
    {
      float num2 = (float) Mathf.RoundToInt((float) (lineWidth - x));
      if ((double) num2 < 0.0)
        num2 = 0.0f;
      num1 = num2 / (float) this.size;
    }
    else
    {
      float num3 = (float) Mathf.RoundToInt((float) (lineWidth - x) * 0.5f);
      if ((double) num3 < 0.0)
        num3 = 0.0f;
      num1 = num3 / (float) this.size;
      if ((lineWidth & 1) == 1)
        num1 += 0.5f / (float) size;
    }
    for (int index = indexOffset; index < verts.size; ++index)
    {
      Vector3 vector3 = verts.buffer[index];
      vector3.x += num1;
      verts.buffer[index] = vector3;
    }
  }

  public Vector2 CalculatePrintedSize(string text, bool encoding, UIFont.SymbolStyle symbolStyle)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
    Vector2 zero = Vector2.zero;
    bool isDynamic = this.isDynamic;
    if (isDynamic || this.mFont != null && this.mFont.isValid && !string.IsNullOrEmpty(text))
    {
      if (encoding)
        text = NGUITools.StripSymbols(text);
      if (isDynamic)
      {
        this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
        this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
        this.mDynamicFont.textureRebuildCallback = (Font.FontTextureRebuildCallback) null;
      }
      int length = text.Length;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int previousChar = 0;
      int size = this.size;
      int num4 = size + this.mSpacingY;
      bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
      for (int index1 = 0; index1 < length; ++index1)
      {
        char index2 = text[index1];
        if (index2 == '\n')
        {
          if (num2 > num1)
            num1 = num2;
          num2 = 0;
          num3 += num4;
          previousChar = 0;
        }
        else if (index2 < ' ')
          previousChar = 0;
        else if (!isDynamic)
        {
          BMSymbol bmSymbol = !flag ? (BMSymbol) null : this.MatchSymbol(text, index1, length);
          if (bmSymbol == null)
          {
            BMGlyph glyph = this.mFont.GetGlyph((int) index2);
            if (glyph != null)
            {
              num2 += this.mSpacingX + (previousChar == 0 ? glyph.advance : glyph.advance + glyph.GetKerning(previousChar));
              previousChar = (int) index2;
            }
          }
          else
          {
            num2 += this.mSpacingX + bmSymbol.width;
            index1 += bmSymbol.length - 1;
            previousChar = 0;
          }
        }
        else if (this.mDynamicFont.GetCharacterInfo(index2, ref UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
          num2 += this.mSpacingX + (int) UIFont.mChar.width;
      }
      float num5 = size <= 0 ? 1f : 1f / (float) size;
      zero.x = num5 * (num2 <= num1 ? (float) num1 : (float) num2);
      zero.y = num5 * (float) (num3 + num4);
    }
    return zero;
  }

  public static bool CheckIfRelated(UIFont a, UIFont b)
  {
    if (Object.op_Equality((Object) a, (Object) null) || Object.op_Equality((Object) b, (Object) null))
      return false;
    return a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0] || Object.op_Equality((Object) a, (Object) b) || a.References(b) || b.References(a);
  }

  private static void EndLine(ref StringBuilder s)
  {
    int index = s.Length - 1;
    if (index > 0 && s[index] == ' ')
      s[index] = '\n';
    else
      s.Append('\n');
  }

  public string GetEndOfLineThatFits(
    string text,
    float maxWidth,
    bool encoding,
    UIFont.SymbolStyle symbolStyle)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
    int num1 = Mathf.RoundToInt(maxWidth * (float) this.size);
    if (num1 < 1)
      return text;
    int length = text.Length;
    int num2 = num1;
    BMGlyph bmGlyph = (BMGlyph) null;
    int num3 = length;
    bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
    bool isDynamic = this.isDynamic;
    if (isDynamic)
    {
      this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
      this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
      this.mDynamicFont.textureRebuildCallback = (Font.FontTextureRebuildCallback) null;
    }
    while (num3 > 0 && num2 > 0)
    {
      char ch = text[--num3];
      BMSymbol bmSymbol = !flag ? (BMSymbol) null : this.MatchSymbol(text, num3, length);
      int mSpacingX = this.mSpacingX;
      if (!isDynamic)
      {
        if (bmSymbol != null)
        {
          mSpacingX += bmSymbol.advance;
        }
        else
        {
          BMGlyph glyph = this.mFont.GetGlyph((int) ch);
          if (glyph != null)
          {
            mSpacingX += glyph.advance + (bmGlyph != null ? bmGlyph.GetKerning((int) ch) : 0);
            bmGlyph = glyph;
          }
          else
          {
            bmGlyph = (BMGlyph) null;
            continue;
          }
        }
      }
      else if (this.mDynamicFont.GetCharacterInfo(ch, ref UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
        mSpacingX += (int) UIFont.mChar.width;
      num2 -= mSpacingX;
    }
    if (num2 < 0)
      ++num3;
    return text.Substring(num3, length - num3);
  }

  private BMSymbol GetSymbol(string sequence, bool createIfMissing)
  {
    int index = 0;
    for (int count = this.mSymbols.Count; index < count; ++index)
    {
      BMSymbol mSymbol = this.mSymbols[index];
      if (mSymbol.sequence == sequence)
        return mSymbol;
    }
    if (!createIfMissing)
      return (BMSymbol) null;
    BMSymbol symbol = new BMSymbol() { sequence = sequence };
    this.mSymbols.Add(symbol);
    return symbol;
  }

  public void MarkAsDirty()
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      this.mReplacement.MarkAsDirty();
    this.RecalculateDynamicOffset();
    this.mSprite = (UIAtlas.Sprite) null;
    UILabel[] active = NGUITools.FindActive<UILabel>();
    int index1 = 0;
    for (int length = active.Length; index1 < length; ++index1)
    {
      UILabel uiLabel = active[index1];
      if (((Behaviour) uiLabel).enabled && NGUITools.GetActive(((Component) uiLabel).gameObject) && UIFont.CheckIfRelated(this, uiLabel.font))
      {
        UIFont font = uiLabel.font;
        uiLabel.font = (UIFont) null;
        uiLabel.font = font;
      }
    }
    int index2 = 0;
    for (int count = this.mSymbols.Count; index2 < count; ++index2)
      this.symbols[index2].MarkAsDirty();
  }

  private BMSymbol MatchSymbol(string text, int offset, int textLength)
  {
    int count = this.mSymbols.Count;
    if (count != 0)
    {
      textLength -= offset;
      for (int index1 = 0; index1 < count; ++index1)
      {
        BMSymbol mSymbol = this.mSymbols[index1];
        int length = mSymbol.length;
        if (length != 0 && textLength >= length)
        {
          bool flag = true;
          for (int index2 = 0; index2 < length; ++index2)
          {
            if ((int) text[offset + index2] != (int) mSymbol.sequence[index2])
            {
              flag = false;
              break;
            }
          }
          if (flag && mSymbol.Validate(this.atlas))
            return mSymbol;
        }
      }
    }
    return (BMSymbol) null;
  }

  private void OnFontChanged() => this.MarkAsDirty();

  public void Print(
    string text,
    Color32 color,
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols,
    bool encoding,
    UIFont.SymbolStyle symbolStyle,
    UIFont.Alignment alignment,
    int lineWidth,
    bool premultiply)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
    {
      this.mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth, premultiply);
    }
    else
    {
      if (text == null)
        return;
      if (!this.isValid)
      {
        Debug.LogError((object) "Attempting to print using an invalid font!");
      }
      else
      {
        bool isDynamic = this.isDynamic;
        if (isDynamic)
        {
          this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
          this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
          this.mDynamicFont.textureRebuildCallback = (Font.FontTextureRebuildCallback) null;
        }
        this.mColors.Clear();
        this.mColors.Add(Color32.op_Implicit(color));
        int size1 = this.size;
        Vector2 vector2 = size1 <= 0 ? Vector2.one : new Vector2(1f / (float) size1, 1f / (float) size1);
        int size2 = verts.size;
        int num1 = 0;
        int x = 0;
        int num2 = 0;
        int previousChar = 0;
        int num3 = size1 + this.mSpacingY;
        Vector3 zero1 = Vector3.zero;
        Vector3 zero2 = Vector3.zero;
        Vector2 zero3 = Vector2.zero;
        Vector2 zero4 = Vector2.zero;
        Rect uvRect1 = this.uvRect;
        float num4 = ((Rect) ref uvRect1).width / (float) this.mFont.texWidth;
        float num5 = ((Rect) ref this.mUVRect).height / (float) this.mFont.texHeight;
        int length = text.Length;
        bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols && this.sprite != null;
        for (int index1 = 0; index1 < length; ++index1)
        {
          char index2 = text[index1];
          if (index2 == '\n')
          {
            if (x > num1)
              num1 = x;
            if (alignment != UIFont.Alignment.Left)
            {
              this.Align(verts, size2, alignment, x, lineWidth);
              size2 = verts.size;
            }
            x = 0;
            num2 += num3;
            previousChar = 0;
          }
          else if (index2 < ' ')
          {
            previousChar = 0;
          }
          else
          {
            if (encoding && index2 == '[')
            {
              int symbol = NGUITools.ParseSymbol(text, index1, this.mColors, premultiply);
              if (symbol > 0)
              {
                color = Color32.op_Implicit(this.mColors[this.mColors.Count - 1]);
                index1 += symbol - 1;
                continue;
              }
            }
            if (!isDynamic)
            {
              BMSymbol bmSymbol = !flag ? (BMSymbol) null : this.MatchSymbol(text, index1, length);
              if (bmSymbol == null)
              {
                BMGlyph glyph = this.mFont.GetGlyph((int) index2);
                if (glyph != null)
                {
                  if (previousChar != 0)
                    x += glyph.GetKerning(previousChar);
                  if (index2 == ' ')
                  {
                    x += this.mSpacingX + glyph.advance;
                    previousChar = (int) index2;
                    continue;
                  }
                  zero1.x = vector2.x * (float) (x + glyph.offsetX);
                  zero1.y = -vector2.y * (float) (num2 + glyph.offsetY);
                  zero2.x = zero1.x + vector2.x * (float) glyph.width;
                  zero2.y = zero1.y - vector2.y * (float) glyph.height;
                  zero3.x = ((Rect) ref this.mUVRect).xMin + num4 * (float) glyph.x;
                  zero3.y = ((Rect) ref this.mUVRect).yMax - num5 * (float) glyph.y;
                  zero4.x = zero3.x + num4 * (float) glyph.width;
                  zero4.y = zero3.y - num5 * (float) glyph.height;
                  x += this.mSpacingX + glyph.advance;
                  previousChar = (int) index2;
                  if (glyph.channel == 0 || glyph.channel == 15)
                  {
                    for (int index3 = 0; index3 < 4; ++index3)
                      cols.Add(color);
                  }
                  else
                  {
                    Color color1 = Color.op_Multiply(Color32.op_Implicit(color), 0.49f);
                    switch (glyph.channel)
                    {
                      case 1:
                        color1.b += 0.51f;
                        break;
                      case 2:
                        color1.g += 0.51f;
                        break;
                      case 4:
                        color1.r += 0.51f;
                        break;
                      case 8:
                        color1.a += 0.51f;
                        break;
                    }
                    for (int index4 = 0; index4 < 4; ++index4)
                      cols.Add(Color32.op_Implicit(color1));
                  }
                }
                else
                  continue;
              }
              else
              {
                zero1.x = vector2.x * (float) (x + bmSymbol.offsetX);
                zero1.y = -vector2.y * (float) (num2 + bmSymbol.offsetY);
                zero2.x = zero1.x + vector2.x * (float) bmSymbol.width;
                zero2.y = zero1.y - vector2.y * (float) bmSymbol.height;
                Rect uvRect2 = bmSymbol.uvRect;
                zero3.x = ((Rect) ref uvRect2).xMin;
                zero3.y = ((Rect) ref uvRect2).yMax;
                zero4.x = ((Rect) ref uvRect2).xMax;
                zero4.y = ((Rect) ref uvRect2).yMin;
                x += this.mSpacingX + bmSymbol.advance;
                index1 += bmSymbol.length - 1;
                previousChar = 0;
                if (symbolStyle == UIFont.SymbolStyle.Colored)
                {
                  for (int index5 = 0; index5 < 4; ++index5)
                    cols.Add(color);
                }
                else
                {
                  Color32 color32 = Color32.op_Implicit(Color.white);
                  color32.a = color.a;
                  for (int index6 = 0; index6 < 4; ++index6)
                    cols.Add(color32);
                }
              }
              verts.Add(new Vector3(zero2.x, zero1.y));
              verts.Add(new Vector3(zero2.x, zero2.y));
              verts.Add(new Vector3(zero1.x, zero2.y));
              verts.Add(new Vector3(zero1.x, zero1.y));
              uvs.Add(new Vector2(zero4.x, zero3.y));
              uvs.Add(new Vector2(zero4.x, zero4.y));
              uvs.Add(new Vector2(zero3.x, zero4.y));
              uvs.Add(new Vector2(zero3.x, zero3.y));
            }
            else if (this.mDynamicFont.GetCharacterInfo(index2, ref UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
            {
              zero1.x = vector2.x * ((float) x + ((Rect) ref UIFont.mChar.vert).xMin);
              zero1.y = (float) (-(double) vector2.y * ((double) num2 - (double) ((Rect) ref UIFont.mChar.vert).yMax + (double) this.mDynamicFontOffset));
              zero2.x = zero1.x + vector2.x * ((Rect) ref UIFont.mChar.vert).width;
              zero2.y = zero1.y - vector2.y * ((Rect) ref UIFont.mChar.vert).height;
              zero3.x = ((Rect) ref UIFont.mChar.uv).xMin;
              zero3.y = ((Rect) ref UIFont.mChar.uv).yMin;
              zero4.x = ((Rect) ref UIFont.mChar.uv).xMax;
              zero4.y = ((Rect) ref UIFont.mChar.uv).yMax;
              x += this.mSpacingX + (int) UIFont.mChar.width;
              for (int index7 = 0; index7 < 4; ++index7)
                cols.Add(color);
              if (UIFont.mChar.flipped)
              {
                uvs.Add(new Vector2(zero3.x, zero4.y));
                uvs.Add(new Vector2(zero3.x, zero3.y));
                uvs.Add(new Vector2(zero4.x, zero3.y));
                uvs.Add(new Vector2(zero4.x, zero4.y));
              }
              else
              {
                uvs.Add(new Vector2(zero4.x, zero3.y));
                uvs.Add(new Vector2(zero3.x, zero3.y));
                uvs.Add(new Vector2(zero3.x, zero4.y));
                uvs.Add(new Vector2(zero4.x, zero4.y));
              }
              verts.Add(new Vector3(zero2.x, zero1.y));
              verts.Add(new Vector3(zero1.x, zero1.y));
              verts.Add(new Vector3(zero1.x, zero2.y));
              verts.Add(new Vector3(zero2.x, zero2.y));
            }
          }
        }
        if (alignment == UIFont.Alignment.Left || size2 >= verts.size)
          return;
        this.Align(verts, size2, alignment, x, lineWidth);
        int size3 = verts.size;
      }
    }
  }

  public bool RecalculateDynamicOffset()
  {
    if (Object.op_Inequality((Object) this.mDynamicFont, (Object) null))
    {
      this.mDynamicFont.RequestCharactersInTexture("j", this.mDynamicFontSize, this.mDynamicFontStyle);
      CharacterInfo characterInfo;
      this.mDynamicFont.GetCharacterInfo('j', ref characterInfo, this.mDynamicFontSize, this.mDynamicFontStyle);
      float objB = (float) this.mDynamicFontSize + ((Rect) ref characterInfo.vert).yMax;
      if (!object.Equals((object) this.mDynamicFontOffset, (object) objB))
      {
        this.mDynamicFontOffset = objB;
        return true;
      }
    }
    return false;
  }

  private bool References(UIFont font)
  {
    if (Object.op_Equality((Object) font, (Object) null))
      return false;
    if (Object.op_Equality((Object) font, (Object) this))
      return true;
    return Object.op_Inequality((Object) this.mReplacement, (Object) null) && this.mReplacement.References(font);
  }

  public void RemoveSymbol(string sequence)
  {
    BMSymbol symbol = this.GetSymbol(sequence, false);
    if (symbol != null)
      this.symbols.Remove(symbol);
    this.MarkAsDirty();
  }

  public void RenameSymbol(string before, string after)
  {
    BMSymbol symbol = this.GetSymbol(before, false);
    if (symbol != null)
      symbol.sequence = after;
    this.MarkAsDirty();
  }

  private void Trim()
  {
    Texture texture = this.mAtlas.texture;
    if (!Object.op_Inequality((Object) texture, (Object) null) || this.mSprite == null)
      return;
    Rect pixels = NGUIMath.ConvertToPixels(this.mUVRect, ((Texture) this.texture).width, ((Texture) this.texture).height, true);
    Rect rect = this.mAtlas.coordinates != UIAtlas.Coordinates.TexCoords ? this.mSprite.outer : NGUIMath.ConvertToPixels(this.mSprite.outer, texture.width, texture.height, true);
    this.mFont.Trim(Mathf.RoundToInt(((Rect) ref rect).xMin - ((Rect) ref pixels).xMin), Mathf.RoundToInt(((Rect) ref rect).yMin - ((Rect) ref pixels).yMin), Mathf.RoundToInt(((Rect) ref rect).xMax - ((Rect) ref pixels).xMin), Mathf.RoundToInt(((Rect) ref rect).yMax - ((Rect) ref pixels).yMin));
  }

  public bool UsesSprite(string s)
  {
    if (!string.IsNullOrEmpty(s))
    {
      if (s.Equals(this.spriteName))
        return true;
      int index = 0;
      for (int count = this.symbols.Count; index < count; ++index)
      {
        BMSymbol symbol = this.symbols[index];
        if (s.Equals(symbol.spriteName))
          return true;
      }
    }
    return false;
  }

  public string WrapText(string text, float maxWidth, int maxLineCount) => this.WrapText(text, maxWidth, maxLineCount, false, UIFont.SymbolStyle.None);

  public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding) => this.WrapText(text, maxWidth, maxLineCount, encoding, UIFont.SymbolStyle.None);

  public string WrapText(
    string text,
    float maxWidth,
    int maxLineCount,
    bool encoding,
    UIFont.SymbolStyle symbolStyle)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.WrapText(text, maxWidth, maxLineCount, encoding, symbolStyle);
    int num1 = Mathf.RoundToInt(maxWidth * (float) this.size);
    if (num1 < 1)
      return text;
    StringBuilder s = new StringBuilder();
    int length = text.Length;
    int num2 = num1;
    int previousChar = 0;
    int num3 = 0;
    int num4 = 0;
    bool flag1 = true;
    bool flag2 = maxLineCount != 1;
    int num5 = 1;
    bool flag3 = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
    bool isDynamic = this.isDynamic;
    if (isDynamic)
    {
      this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
      this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
      this.mDynamicFont.textureRebuildCallback = (Font.FontTextureRebuildCallback) null;
    }
    for (; num4 < length; ++num4)
    {
      char index = text[num4];
      switch (index)
      {
        case '\n':
          if (flag2 && num5 != maxLineCount)
          {
            num2 = num1;
            if (num3 < num4)
              s.Append(text.Substring(num3, num4 - num3 + 1));
            else
              s.Append(index);
            flag1 = true;
            ++num5;
            num3 = num4 + 1;
            previousChar = 0;
            break;
          }
          goto label_43;
        case ' ':
          if (previousChar != 32 && num3 < num4)
          {
            s.Append(text.Substring(num3, num4 - num3 + 1));
            flag1 = false;
            num3 = num4 + 1;
            previousChar = (int) index;
            goto default;
          }
          else
            goto default;
        default:
          if (encoding && index == '[' && num4 + 2 < length)
          {
            if (text[num4 + 1] == '-' && text[num4 + 2] == ']')
            {
              num4 += 2;
              break;
            }
            if (num4 + 7 < length && text[num4 + 7] == ']' && NGUITools.EncodeColor(NGUITools.ParseColor(text, num4 + 1)) == text.Substring(num4 + 1, 6).ToUpper())
            {
              num4 += 7;
              break;
            }
          }
          BMSymbol bmSymbol = !flag3 ? (BMSymbol) null : this.MatchSymbol(text, num4, length);
          int mSpacingX = this.mSpacingX;
          if (!isDynamic)
          {
            if (bmSymbol != null)
            {
              mSpacingX += bmSymbol.advance;
            }
            else
            {
              BMGlyph glyph = bmSymbol != null ? (BMGlyph) null : this.mFont.GetGlyph((int) index);
              if (glyph != null)
                mSpacingX += previousChar == 0 ? glyph.advance : glyph.advance + glyph.GetKerning(previousChar);
              else
                break;
            }
          }
          else if (this.mDynamicFont.GetCharacterInfo(index, ref UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
            mSpacingX += Mathf.RoundToInt(UIFont.mChar.width);
          num2 -= mSpacingX;
          if (num2 < 0)
          {
            if (flag1 || !flag2 || num5 == maxLineCount)
            {
              s.Append(text.Substring(num3, Mathf.Max(0, num4 - num3)));
              if (!flag2 || num5 == maxLineCount)
              {
                num3 = num4;
                goto label_43;
              }
              else
              {
                UIFont.EndLine(ref s);
                flag1 = true;
                ++num5;
                if (index == ' ')
                {
                  num3 = num4 + 1;
                  num2 = num1;
                }
                else
                {
                  num3 = num4;
                  num2 = num1 - mSpacingX;
                }
                previousChar = 0;
              }
            }
            else
            {
              while (num3 < length && text[num3] == ' ')
                ++num3;
              flag1 = true;
              num2 = num1;
              num4 = num3 - 1;
              previousChar = 0;
              if (flag2 && num5 != maxLineCount)
              {
                ++num5;
                UIFont.EndLine(ref s);
                break;
              }
              goto label_43;
            }
          }
          else
            previousChar = (int) index;
          if (!isDynamic && bmSymbol != null)
          {
            num4 += bmSymbol.length - 1;
            previousChar = 0;
            break;
          }
          break;
      }
    }
label_43:
    if (num3 < num4)
      s.Append(text.Substring(num3, num4 - num3));
    return s.ToString();
  }

  public UIAtlas atlas
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.atlas : this.mAtlas;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.atlas = value;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.mAtlas, (Object) value))
          return;
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (Object.op_Inequality((Object) this.mAtlas, (Object) null))
            this.mMat = this.mAtlas.spriteMaterial;
          if (this.sprite != null)
            this.mUVRect = this.uvRect;
        }
        this.mPMA = -1;
        this.mAtlas = value;
        this.MarkAsDirty();
      }
    }
  }

  public BMFont bmFont => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.bmFont : this.mFont;

  public Font dynamicFont
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.dynamicFont : this.mDynamicFont;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.dynamicFont = value;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.mDynamicFont, (Object) value))
          return;
        if (Object.op_Inequality((Object) this.mDynamicFont, (Object) null))
          this.material = (Material) null;
        this.mDynamicFont = value;
        this.MarkAsDirty();
      }
    }
  }

  public int dynamicFontSize
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.dynamicFontSize : this.mDynamicFontSize;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.dynamicFontSize = value;
      }
      else
      {
        value = Mathf.Clamp(value, 4, 128);
        if (this.mDynamicFontSize == value)
          return;
        this.mDynamicFontSize = value;
        this.MarkAsDirty();
      }
    }
  }

  public FontStyle dynamicFontStyle
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.dynamicFontStyle : this.mDynamicFontStyle;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.dynamicFontStyle = value;
      }
      else
      {
        if (this.mDynamicFontStyle == value)
          return;
        this.mDynamicFontStyle = value;
        this.MarkAsDirty();
      }
    }
  }

  private Texture dynamicTexture
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.dynamicTexture;
      return this.isDynamic ? this.mDynamicFont.material.mainTexture : (Texture) null;
    }
  }

  public bool hasSymbols => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.hasSymbols : this.mSymbols.Count != 0;

  public int horizontalSpacing
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.horizontalSpacing : this.mSpacingX;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.horizontalSpacing = value;
      }
      else
      {
        if (this.mSpacingX == value)
          return;
        this.mSpacingX = value;
        this.MarkAsDirty();
      }
    }
  }

  public bool isDynamic => Object.op_Inequality((Object) this.mDynamicFont, (Object) null);

  public bool isValid => Object.op_Inequality((Object) this.mDynamicFont, (Object) null) || this.mFont.isValid;

  public Material material
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.material;
      if (Object.op_Inequality((Object) this.mAtlas, (Object) null))
        return this.mAtlas.spriteMaterial;
      if (Object.op_Inequality((Object) this.mMat, (Object) null))
      {
        if (Object.op_Inequality((Object) this.mDynamicFont, (Object) null) && Object.op_Inequality((Object) this.mMat, (Object) this.mDynamicFont.material))
          this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
        return this.mMat;
      }
      return Object.op_Inequality((Object) this.mDynamicFont, (Object) null) ? this.mDynamicFont.material : (Material) null;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.material = value;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.mMat, (Object) value))
          return;
        this.mPMA = -1;
        this.mMat = value;
        this.MarkAsDirty();
      }
    }
  }

  public float pixelSize
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.pixelSize;
      return Object.op_Inequality((Object) this.mAtlas, (Object) null) ? this.mAtlas.pixelSize : this.mPixelSize;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        this.mReplacement.pixelSize = value;
      else if (Object.op_Inequality((Object) this.mAtlas, (Object) null))
      {
        this.mAtlas.pixelSize = value;
      }
      else
      {
        float num = Mathf.Clamp(value, 0.25f, 4f);
        if ((double) this.mPixelSize == (double) num)
          return;
        this.mPixelSize = num;
        this.MarkAsDirty();
      }
    }
  }

  public bool premultipliedAlpha
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.premultipliedAlpha;
      if (Object.op_Inequality((Object) this.mAtlas, (Object) null))
        return this.mAtlas.premultipliedAlpha;
      if (this.mPMA == -1)
      {
        Material material = this.material;
        this.mPMA = Object.op_Equality((Object) material, (Object) null) || Object.op_Equality((Object) material.shader, (Object) null) || !((Object) material.shader).name.Contains("Premultiplied") ? 0 : 1;
      }
      return this.mPMA == 1;
    }
  }

  public UIFont replacement
  {
    get => this.mReplacement;
    set
    {
      UIFont uiFont = value;
      if (Object.op_Equality((Object) uiFont, (Object) this))
        uiFont = (UIFont) null;
      if (!Object.op_Inequality((Object) this.mReplacement, (Object) uiFont))
        return;
      if (Object.op_Inequality((Object) uiFont, (Object) null) && Object.op_Equality((Object) uiFont.replacement, (Object) this))
        uiFont.replacement = (UIFont) null;
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        this.MarkAsDirty();
      this.mReplacement = uiFont;
      this.MarkAsDirty();
    }
  }

  public int size
  {
    get
    {
      if (!Object.op_Equality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.size;
      return this.isDynamic ? this.mDynamicFontSize : this.mFont.charSize;
    }
  }

  public UIAtlas.Sprite sprite
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.sprite;
      if (!this.mSpriteSet)
        this.mSprite = (UIAtlas.Sprite) null;
      if (this.mSprite == null)
      {
        if (Object.op_Inequality((Object) this.mAtlas, (Object) null) && !string.IsNullOrEmpty(this.mFont.spriteName))
        {
          this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
          if (this.mSprite == null)
            this.mSprite = this.mAtlas.GetSprite(((Object) this).name);
          this.mSpriteSet = true;
          if (this.mSprite == null)
            this.mFont.spriteName = (string) null;
        }
        int index = 0;
        for (int count = this.mSymbols.Count; index < count; ++index)
          this.symbols[index].MarkAsDirty();
      }
      return this.mSprite;
    }
  }

  public string spriteName
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.spriteName : this.mFont.spriteName;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.spriteName = value;
      }
      else
      {
        if (!(this.mFont.spriteName != value))
          return;
        this.mFont.spriteName = value;
        this.MarkAsDirty();
      }
    }
  }

  public List<BMSymbol> symbols => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.symbols : this.mSymbols;

  public int texHeight
  {
    get
    {
      if (!Object.op_Equality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.texHeight;
      return this.mFont != null ? this.mFont.texHeight : 1;
    }
  }

  public Texture2D texture
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.texture;
      Material material = this.material;
      return !Object.op_Equality((Object) material, (Object) null) ? material.mainTexture as Texture2D : (Texture2D) null;
    }
  }

  public int texWidth
  {
    get
    {
      if (!Object.op_Equality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.texWidth;
      return this.mFont != null ? this.mFont.texWidth : 1;
    }
  }

  public Rect uvRect
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.uvRect;
      if (Object.op_Inequality((Object) this.mAtlas, (Object) null) && this.mSprite == null && this.sprite != null)
      {
        Texture texture = this.mAtlas.texture;
        if (Object.op_Inequality((Object) texture, (Object) null))
        {
          this.mUVRect = this.mSprite.outer;
          if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
            this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
          if (this.mSprite.hasPadding)
          {
            Rect mUvRect = this.mUVRect;
            ((Rect) ref this.mUVRect).xMin = ((Rect) ref mUvRect).xMin - this.mSprite.paddingLeft * ((Rect) ref mUvRect).width;
            ((Rect) ref this.mUVRect).yMin = ((Rect) ref mUvRect).yMin - this.mSprite.paddingBottom * ((Rect) ref mUvRect).height;
            ((Rect) ref this.mUVRect).xMax = ((Rect) ref mUvRect).xMax + this.mSprite.paddingRight * ((Rect) ref mUvRect).width;
            ((Rect) ref this.mUVRect).yMax = ((Rect) ref mUvRect).yMax + this.mSprite.paddingTop * ((Rect) ref mUvRect).height;
          }
          if (this.mSprite.hasPadding)
            this.Trim();
        }
      }
      return this.mUVRect;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.uvRect = value;
      }
      else
      {
        if (this.sprite != null || !Rect.op_Inequality(this.mUVRect, value))
          return;
        this.mUVRect = value;
        this.MarkAsDirty();
      }
    }
  }

  public int verticalSpacing
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.verticalSpacing : this.mSpacingY;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.verticalSpacing = value;
      }
      else
      {
        if (this.mSpacingY == value)
          return;
        this.mSpacingY = value;
        this.MarkAsDirty();
      }
    }
  }

  public enum Alignment
  {
    Left,
    Center,
    Right,
  }

  public enum SymbolStyle
  {
    None,
    Uncolored,
    Colored,
  }
}
