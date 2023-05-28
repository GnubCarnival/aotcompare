// Decompiled with JetBrains decompiler
// Type: UILabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
  [HideInInspector]
  [SerializeField]
  private Color mEffectColor = Color.black;
  [HideInInspector]
  [SerializeField]
  private Vector2 mEffectDistance = Vector2.one;
  [HideInInspector]
  [SerializeField]
  private UILabel.Effect mEffectStyle;
  [HideInInspector]
  [SerializeField]
  private bool mEncoding = true;
  [HideInInspector]
  [SerializeField]
  private UIFont mFont;
  private int mLastCount;
  private UILabel.Effect mLastEffect;
  private bool mLastEncoding = true;
  private bool mLastPass;
  private Vector3 mLastScale = Vector3.one;
  private bool mLastShow;
  private string mLastText = string.Empty;
  private int mLastWidth;
  [HideInInspector]
  [SerializeField]
  private float mLineWidth;
  [HideInInspector]
  [SerializeField]
  private int mMaxLineCount;
  [SerializeField]
  [HideInInspector]
  private int mMaxLineWidth;
  [HideInInspector]
  [SerializeField]
  private bool mMultiline = true;
  [HideInInspector]
  [SerializeField]
  private bool mPassword;
  private bool mPremultiply;
  private string mProcessedText;
  private bool mShouldBeProcessed = true;
  [HideInInspector]
  [SerializeField]
  private bool mShowLastChar;
  [HideInInspector]
  [SerializeField]
  private bool mShrinkToFit;
  private Vector2 mSize = Vector2.zero;
  [SerializeField]
  [HideInInspector]
  private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;
  [SerializeField]
  [HideInInspector]
  private string mText = string.Empty;

  private void ApplyShadow(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols,
    int start,
    int end,
    float x,
    float y)
  {
    Color mEffectColor = this.mEffectColor;
    mEffectColor.a *= this.alpha * this.mPanel.alpha;
    Color32 color32 = Color32.op_Implicit(!this.font.premultipliedAlpha ? mEffectColor : NGUITools.ApplyPMA(mEffectColor));
    for (int index = start; index < end; ++index)
    {
      verts.Add(verts.buffer[index]);
      uvs.Add(uvs.buffer[index]);
      cols.Add(cols.buffer[index]);
      Vector3 vector3 = verts.buffer[index];
      vector3.x += x;
      vector3.y += y;
      verts.buffer[index] = vector3;
      cols.buffer[index] = color32;
    }
  }

  public override void MakePixelPerfect()
  {
    if (Object.op_Inequality((Object) this.mFont, (Object) null))
    {
      float pixelSize = this.font.pixelSize;
      Vector3 localScale = this.cachedTransform.localScale;
      localScale.x = (float) this.mFont.size * pixelSize;
      localScale.y = localScale.x;
      localScale.z = 1f;
      Vector3 localPosition = this.cachedTransform.localPosition;
      localPosition.x = (float) (Mathf.CeilToInt((float) ((double) localPosition.x / (double) pixelSize * 4.0)) >> 2);
      localPosition.y = (float) (Mathf.CeilToInt((float) ((double) localPosition.y / (double) pixelSize * 4.0)) >> 2);
      localPosition.z = (float) Mathf.RoundToInt(localPosition.z);
      localPosition.x *= pixelSize;
      localPosition.y *= pixelSize;
      this.cachedTransform.localPosition = localPosition;
      this.cachedTransform.localScale = localScale;
      if (!this.shrinkToFit)
        return;
      this.ProcessText();
    }
    else
      base.MakePixelPerfect();
  }

  public override void MarkAsChanged()
  {
    this.hasChanged = true;
    base.MarkAsChanged();
  }

  public override void OnFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    if (!Object.op_Inequality((Object) this.mFont, (Object) null))
      return;
    UIWidget.Pivot pivot = this.pivot;
    int size1 = verts.size;
    Color c = this.color;
    c.a *= this.mPanel.alpha;
    if (this.font.premultipliedAlpha)
      c = NGUITools.ApplyPMA(c);
    switch (pivot)
    {
      case UIWidget.Pivot.TopLeft:
      case UIWidget.Pivot.Left:
      case UIWidget.Pivot.BottomLeft:
        this.mFont.Print(this.processedText, Color32.op_Implicit(c), verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Left, 0, this.mPremultiply);
        break;
      case UIWidget.Pivot.TopRight:
      case UIWidget.Pivot.Right:
      case UIWidget.Pivot.BottomRight:
        this.mFont.Print(this.processedText, Color32.op_Implicit(c), verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Right, Mathf.RoundToInt(this.relativeSize.x * (float) this.mFont.size), this.mPremultiply);
        break;
      default:
        this.mFont.Print(this.processedText, Color32.op_Implicit(c), verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Center, Mathf.RoundToInt(this.relativeSize.x * (float) this.mFont.size), this.mPremultiply);
        break;
    }
    if (this.effectStyle == UILabel.Effect.None)
      return;
    int size2 = verts.size;
    float num = 1f / (float) this.mFont.size;
    float x = num * this.mEffectDistance.x;
    float y = num * this.mEffectDistance.y;
    this.ApplyShadow(verts, uvs, cols, size1, size2, x, -y);
    if (this.effectStyle != UILabel.Effect.Outline)
      return;
    int start1 = size2;
    int size3 = verts.size;
    this.ApplyShadow(verts, uvs, cols, start1, size3, -x, y);
    int start2 = size3;
    int size4 = verts.size;
    this.ApplyShadow(verts, uvs, cols, start2, size4, x, y);
    int start3 = size4;
    int size5 = verts.size;
    this.ApplyShadow(verts, uvs, cols, start3, size5, -x, -y);
  }

  protected override void OnStart()
  {
    if ((double) this.mLineWidth > 0.0)
    {
      this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
      this.mLineWidth = 0.0f;
    }
    if (!this.mMultiline)
    {
      this.mMaxLineCount = 1;
      this.mMultiline = true;
    }
    this.mPremultiply = Object.op_Inequality((Object) this.font, (Object) null) && Object.op_Inequality((Object) this.font.material, (Object) null) && ((Object) this.font.material.shader).name.Contains("Premultiplied");
  }

  private void ProcessText()
  {
    this.mChanged = true;
    this.hasChanged = false;
    this.mLastText = this.mText;
    float num1 = Mathf.Abs(this.cachedTransform.localScale.x);
    float num2 = (float) (this.mFont.size * this.mMaxLineCount);
    if ((double) num1 <= 0.0)
    {
      this.mSize.x = 1f;
      float size = (float) this.mFont.size;
      this.cachedTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
      this.mProcessedText = string.Empty;
    }
    else
    {
      do
      {
        if (this.mPassword)
        {
          this.mProcessedText = string.Empty;
          if (this.mShowLastChar)
          {
            int num3 = 0;
            for (int index = this.mText.Length - 1; num3 < index; ++num3)
              this.mProcessedText += "*";
            if (this.mText.Length > 0)
              this.mProcessedText += this.mText[this.mText.Length - 1].ToString();
          }
          else
          {
            int num4 = 0;
            for (int length = this.mText.Length; num4 < length; ++num4)
              this.mProcessedText += "*";
          }
          this.mProcessedText = this.mFont.WrapText(this.mProcessedText, (float) this.mMaxLineWidth / num1, this.mMaxLineCount, false, UIFont.SymbolStyle.None);
        }
        else
          this.mProcessedText = this.mMaxLineWidth <= 0 ? (this.mShrinkToFit || this.mMaxLineCount <= 0 ? this.mText : this.mFont.WrapText(this.mText, 100000f, this.mMaxLineCount, this.mEncoding, this.mSymbols)) : this.mFont.WrapText(this.mText, (float) this.mMaxLineWidth / num1, !this.mShrinkToFit ? this.mMaxLineCount : 0, this.mEncoding, this.mSymbols);
        this.mSize = string.IsNullOrEmpty(this.mProcessedText) ? Vector2.one : this.mFont.CalculatePrintedSize(this.mProcessedText, this.mEncoding, this.mSymbols);
        if (this.mShrinkToFit)
        {
          if (this.mMaxLineCount > 0 && (double) this.mSize.y * (double) num1 > (double) num2)
            num1 = Mathf.Round(num1 - 1f);
          else
            break;
        }
        else
          goto label_20;
      }
      while ((double) num1 > 1.0);
      if (this.mMaxLineWidth > 0)
      {
        float num5 = (float) this.mMaxLineWidth / num1;
        num1 = Mathf.Min((double) this.mSize.x * (double) num1 <= (double) num5 ? num1 : num5 / this.mSize.x * num1, num1);
      }
      num1 = Mathf.Round(num1);
      this.cachedTransform.localScale = new Vector3(num1, num1, 1f);
label_20:
      this.mSize.x = Mathf.Max(this.mSize.x, (double) num1 <= 0.0 ? 1f : (float) this.lineWidth / num1);
    }
    this.mSize.y = Mathf.Max(this.mSize.y, 1f);
  }

  public Color effectColor
  {
    get => this.mEffectColor;
    set
    {
      if (this.mEffectColor.Equals((object) value))
        return;
      this.mEffectColor = value;
      if (this.mEffectStyle == UILabel.Effect.None)
        return;
      this.hasChanged = true;
    }
  }

  public Vector2 effectDistance
  {
    get => this.mEffectDistance;
    set
    {
      if (!Vector2.op_Inequality(this.mEffectDistance, value))
        return;
      this.mEffectDistance = value;
      this.hasChanged = true;
    }
  }

  public UILabel.Effect effectStyle
  {
    get => this.mEffectStyle;
    set
    {
      if (this.mEffectStyle == value)
        return;
      this.mEffectStyle = value;
      this.hasChanged = true;
    }
  }

  public UIFont font
  {
    get => this.mFont;
    set
    {
      if (!Object.op_Inequality((Object) this.mFont, (Object) value))
        return;
      this.mFont = value;
      this.material = Object.op_Equality((Object) this.mFont, (Object) null) ? (Material) null : this.mFont.material;
      this.mChanged = true;
      this.hasChanged = true;
      this.MarkAsChanged();
    }
  }

  private bool hasChanged
  {
    get => this.mShouldBeProcessed || this.mLastText != this.text || this.mLastWidth != this.mMaxLineWidth || this.mLastEncoding != this.mEncoding || this.mLastCount != this.mMaxLineCount || this.mLastPass != this.mPassword || this.mLastShow != this.mShowLastChar || this.mLastEffect != this.mEffectStyle;
    set
    {
      if (value)
      {
        this.mChanged = true;
        this.mShouldBeProcessed = true;
      }
      else
      {
        this.mShouldBeProcessed = false;
        this.mLastText = this.text;
        this.mLastWidth = this.mMaxLineWidth;
        this.mLastEncoding = this.mEncoding;
        this.mLastCount = this.mMaxLineCount;
        this.mLastPass = this.mPassword;
        this.mLastShow = this.mShowLastChar;
        this.mLastEffect = this.mEffectStyle;
      }
    }
  }

  public int lineWidth
  {
    get => this.mMaxLineWidth;
    set
    {
      if (this.mMaxLineWidth == value)
        return;
      this.mMaxLineWidth = value;
      this.hasChanged = true;
      if (!this.shrinkToFit)
        return;
      this.MakePixelPerfect();
    }
  }

  public override Material material
  {
    get
    {
      Material material = base.material;
      if (Object.op_Equality((Object) material, (Object) null))
      {
        material = Object.op_Equality((Object) this.mFont, (Object) null) ? (Material) null : this.mFont.material;
        this.material = material;
      }
      return material;
    }
  }

  public int maxLineCount
  {
    get => this.mMaxLineCount;
    set
    {
      if (this.mMaxLineCount == value)
        return;
      this.mMaxLineCount = Mathf.Max(value, 0);
      this.hasChanged = true;
      if (value != 1)
        return;
      this.mPassword = false;
    }
  }

  public bool multiLine
  {
    get => this.mMaxLineCount != 1;
    set
    {
      if (this.mMaxLineCount != 1 == value)
        return;
      this.mMaxLineCount = !value ? 1 : 0;
      this.hasChanged = true;
      if (!value)
        return;
      this.mPassword = false;
    }
  }

  public bool password
  {
    get => this.mPassword;
    set
    {
      if (this.mPassword == value)
        return;
      if (value)
      {
        this.mMaxLineCount = 1;
        this.mEncoding = false;
      }
      this.mPassword = value;
      this.hasChanged = true;
    }
  }

  public string processedText
  {
    get
    {
      if (Vector3.op_Inequality(this.mLastScale, this.cachedTransform.localScale))
      {
        this.mLastScale = this.cachedTransform.localScale;
        this.mShouldBeProcessed = true;
      }
      if (this.hasChanged)
        this.ProcessText();
      return this.mProcessedText;
    }
  }

  public override Vector2 relativeSize
  {
    get
    {
      if (Object.op_Equality((Object) this.mFont, (Object) null))
        return Vector2.op_Implicit(Vector3.one);
      if (this.hasChanged)
        this.ProcessText();
      return this.mSize;
    }
  }

  public bool showLastPasswordChar
  {
    get => this.mShowLastChar;
    set
    {
      if (this.mShowLastChar == value)
        return;
      this.mShowLastChar = value;
      this.hasChanged = true;
    }
  }

  public bool shrinkToFit
  {
    get => this.mShrinkToFit;
    set
    {
      if (this.mShrinkToFit == value)
        return;
      this.mShrinkToFit = value;
      this.hasChanged = true;
    }
  }

  public bool supportEncoding
  {
    get => this.mEncoding;
    set
    {
      if (this.mEncoding == value)
        return;
      this.mEncoding = value;
      this.hasChanged = true;
      if (!value)
        return;
      this.mPassword = false;
    }
  }

  public UIFont.SymbolStyle symbolStyle
  {
    get => this.mSymbols;
    set
    {
      if (this.mSymbols == value)
        return;
      this.mSymbols = value;
      this.hasChanged = true;
    }
  }

  public string text
  {
    get => this.mText;
    set
    {
      if (string.IsNullOrEmpty(value))
      {
        if (!string.IsNullOrEmpty(this.mText))
          this.mText = string.Empty;
        this.hasChanged = true;
      }
      else
      {
        if (!(this.mText != value))
          return;
        this.mText = value;
        this.hasChanged = true;
        if (!this.shrinkToFit)
          return;
        this.MakePixelPerfect();
      }
    }
  }

  public enum Effect
  {
    None,
    Shadow,
    Outline,
  }
}
