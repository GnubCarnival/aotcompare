﻿// Decompiled with JetBrains decompiler
// Type: TypewriterEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (UILabel))]
[AddComponentMenu("NGUI/Examples/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
  public int charsPerSecond = 40;
  private UILabel mLabel;
  private float mNextChar;
  private int mOffset;
  private string mText;

  private void Update()
  {
    if (Object.op_Equality((Object) this.mLabel, (Object) null))
    {
      this.mLabel = ((Component) this).GetComponent<UILabel>();
      this.mLabel.supportEncoding = false;
      this.mLabel.symbolStyle = UIFont.SymbolStyle.None;
      this.mText = this.mLabel.font.WrapText(this.mLabel.text, (float) this.mLabel.lineWidth / this.mLabel.cachedTransform.localScale.x, this.mLabel.maxLineCount, false, UIFont.SymbolStyle.None);
    }
    if (this.mOffset < this.mText.Length)
    {
      if ((double) this.mNextChar > (double) Time.time)
        return;
      this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
      float num = 1f / (float) this.charsPerSecond;
      switch (this.mText[this.mOffset])
      {
        case '\n':
        case '!':
        case '.':
        case '?':
          num *= 4f;
          break;
      }
      this.mNextChar = Time.time + num;
      this.mLabel.text = this.mText.Substring(0, ++this.mOffset);
    }
    else
      Object.Destroy((Object) this);
  }
}
