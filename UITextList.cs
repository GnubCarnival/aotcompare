// Decompiled with JetBrains decompiler
// Type: UITextList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
  public int maxEntries = 50;
  public float maxHeight;
  public float maxWidth;
  protected List<UITextList.Paragraph> mParagraphs = new List<UITextList.Paragraph>();
  protected float mScroll;
  protected bool mSelected;
  protected char[] mSeparator = new char[1]{ '\n' };
  protected int mTotalLines;
  public UITextList.Style style;
  public bool supportScrollWheel = true;
  public UILabel textLabel;

  public void Add(string text) => this.Add(text, true);

  protected void Add(string text, bool updateVisible)
  {
    UITextList.Paragraph paragraph;
    if (this.mParagraphs.Count < this.maxEntries)
    {
      paragraph = new UITextList.Paragraph();
    }
    else
    {
      paragraph = this.mParagraphs[0];
      this.mParagraphs.RemoveAt(0);
    }
    paragraph.text = text;
    this.mParagraphs.Add(paragraph);
    if (Object.op_Inequality((Object) this.textLabel, (Object) null) && Object.op_Inequality((Object) this.textLabel.font, (Object) null))
    {
      paragraph.lines = this.textLabel.font.WrapText(paragraph.text, this.maxWidth / ((Component) this.textLabel).transform.localScale.y, this.textLabel.maxLineCount, this.textLabel.supportEncoding, this.textLabel.symbolStyle).Split(this.mSeparator);
      this.mTotalLines = 0;
      int index = 0;
      for (int count = this.mParagraphs.Count; index < count; ++index)
        this.mTotalLines += this.mParagraphs[index].lines.Length;
    }
    if (!updateVisible)
      return;
    this.UpdateVisibleText();
  }

  private void Awake()
  {
    if (Object.op_Equality((Object) this.textLabel, (Object) null))
      this.textLabel = ((Component) this).GetComponentInChildren<UILabel>();
    if (Object.op_Inequality((Object) this.textLabel, (Object) null))
      this.textLabel.lineWidth = 0;
    Collider collider = ((Component) this).collider;
    if (!Object.op_Inequality((Object) collider, (Object) null))
      return;
    Bounds bounds;
    if ((double) this.maxHeight <= 0.0)
    {
      bounds = collider.bounds;
      this.maxHeight = ((Bounds) ref bounds).size.y / ((Component) this).transform.lossyScale.y;
    }
    if ((double) this.maxWidth > 0.0)
      return;
    bounds = collider.bounds;
    this.maxWidth = ((Bounds) ref bounds).size.x / ((Component) this).transform.lossyScale.x;
  }

  public void Clear()
  {
    this.mParagraphs.Clear();
    this.UpdateVisibleText();
  }

  private void OnScroll(float val)
  {
    if (!this.mSelected || !this.supportScrollWheel)
      return;
    val *= this.style != UITextList.Style.Chat ? -10f : 10f;
    this.mScroll = Mathf.Max(0.0f, this.mScroll + val);
    this.UpdateVisibleText();
  }

  private void OnSelect(bool selected) => this.mSelected = selected;

  protected void UpdateVisibleText()
  {
    if (!Object.op_Inequality((Object) this.textLabel, (Object) null) || !Object.op_Inequality((Object) this.textLabel.font, (Object) null))
      return;
    int num1 = 0;
    int num2 = (double) this.maxHeight <= 0.0 ? 100000 : Mathf.FloorToInt(this.maxHeight / this.textLabel.cachedTransform.localScale.y);
    int num3 = Mathf.RoundToInt(this.mScroll);
    if (num2 + num3 > this.mTotalLines)
    {
      num3 = Mathf.Max(0, this.mTotalLines - num2);
      this.mScroll = (float) num3;
    }
    if (this.style == UITextList.Style.Chat)
      num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
    StringBuilder stringBuilder = new StringBuilder();
    int index1 = 0;
    for (int count = this.mParagraphs.Count; index1 < count; ++index1)
    {
      UITextList.Paragraph mParagraph = this.mParagraphs[index1];
      int index2 = 0;
      for (int length = mParagraph.lines.Length; index2 < length; ++index2)
      {
        string line = mParagraph.lines[index2];
        if (num3 > 0)
        {
          --num3;
        }
        else
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append("\n");
          stringBuilder.Append(line);
          ++num1;
          if (num1 >= num2)
            break;
        }
      }
      if (num1 >= num2)
        break;
    }
    this.textLabel.text = stringBuilder.ToString();
  }

  protected class Paragraph
  {
    public string[] lines;
    public string text;
  }

  public enum Style
  {
    Text,
    Chat,
  }
}
