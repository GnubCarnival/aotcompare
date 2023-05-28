// Decompiled with JetBrains decompiler
// Type: UI.InputFieldPasteable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  public class InputFieldPasteable : InputField
  {
    protected bool IsModifier() => Application.platform == 1 ? Input.GetKey((KeyCode) 310) || Input.GetKey((KeyCode) 309) : Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305);

    protected bool IsCopy() => Input.GetKeyDown((KeyCode) 99);

    protected bool IsPaste() => Input.GetKeyDown((KeyCode) 118);

    protected bool IsCut() => Input.GetKeyDown((KeyCode) 120);

    protected virtual void Append(char input)
    {
      if (Application.platform == 1 && this.IsModifier() && (this.IsCopy() || this.IsCut() || this.IsPaste()))
        return;
      base.Append(input);
    }

    protected virtual void Append(string input)
    {
      if (Application.platform == 1 && this.IsModifier() && (this.IsCopy() || this.IsCut()))
        return;
      if (this.multiLine && this.IsModifier() && this.IsPaste())
      {
        input = this.GetClipboard();
        int num = this.caretPosition;
        if (this.caretPosition != this.m_CaretSelectPosition && this.text.Length > 0)
        {
          int length = Math.Min(this.caretPosition, this.m_CaretSelectPosition);
          int startIndex = Math.Max(this.caretPosition, this.m_CaretSelectPosition);
          if (startIndex >= this.text.Length)
            this.text = this.text.Substring(0, length);
          else
            this.text = this.text.Substring(0, length) + this.text.Substring(startIndex, this.text.Length - startIndex);
          num = length;
        }
        if (num >= this.text.Length || this.text.Length == 0)
          this.text += input;
        else
          this.text = this.text.Substring(0, num) + input + this.text.Substring(num, this.text.Length - num);
        ((UnityEvent<string>) this.onValueChange).Invoke(this.text);
        this.m_CaretSelectPosition = this.caretPosition = Math.Min(num + input.Length, this.text.Length);
      }
      else if (!this.multiLine && Application.platform == 1 && this.IsModifier() && this.IsPaste())
      {
        foreach (char ch in this.GetClipboard())
          base.Append(ch);
      }
      else
        base.Append(input);
    }

    private string GetClipboard()
    {
      TextEditor textEditor = new TextEditor();
      textEditor.multiline = this.multiLine;
      textEditor.Paste();
      return textEditor.content.text;
    }
  }
}
