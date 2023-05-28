// Decompiled with JetBrains decompiler
// Type: UIInputValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (UIInput))]
[AddComponentMenu("NGUI/Interaction/Input Validator")]
public class UIInputValidator : MonoBehaviour
{
  public UIInputValidator.Validation logic;

  private void Start() => ((Component) this).GetComponent<UIInput>().validator = new UIInput.Validator(this.Validate);

  private char Validate(string text, char ch)
  {
    if (this.logic == UIInputValidator.Validation.None || !((Behaviour) this).enabled)
      return ch;
    if (this.logic == UIInputValidator.Validation.Integer)
    {
      if (ch >= '0' && ch <= '9' || ch == '-' && text.Length == 0)
        return ch;
    }
    else if (this.logic == UIInputValidator.Validation.Float)
    {
      if (ch >= '0' && ch <= '9' || ch == '-' && text.Length == 0 || ch == '.' && !text.Contains("."))
        return ch;
    }
    else if (this.logic == UIInputValidator.Validation.Alphanumeric)
    {
      if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9')
        return ch;
    }
    else if (this.logic == UIInputValidator.Validation.Username)
    {
      if (ch >= 'A' && ch <= 'Z')
        return (char) ((int) ch - 65 + 97);
      if (ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9')
        return ch;
    }
    else if (this.logic == UIInputValidator.Validation.Name)
    {
      char ch1 = text.Length <= 0 ? ' ' : text[text.Length - 1];
      if (ch >= 'a' && ch <= 'z')
        return ch1 == ' ' ? (char) ((int) ch - 97 + 65) : ch;
      if (ch >= 'A' && ch <= 'Z')
        return ch1 != ' ' && ch1 != '\'' ? (char) ((int) ch - 65 + 97) : ch;
      if (ch == '\'')
      {
        if (ch1 != ' ' && ch1 != '\'' && !text.Contains("'"))
          return ch;
      }
      else if (ch == ' ' && ch1 != ' ' && ch1 != '\'')
        return ch;
    }
    return char.MinValue;
  }

  public enum Validation
  {
    None,
    Integer,
    Float,
    Alphanumeric,
    Username,
    Name,
  }
}
