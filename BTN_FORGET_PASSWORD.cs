// Decompiled with JetBrains decompiler
// Type: BTN_FORGET_PASSWORD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class BTN_FORGET_PASSWORD : MonoBehaviour
{
  public GameObject email;
  private bool invalid;
  public GameObject logincomponent;
  public GameObject output;

  private string DomainMapper(Match match)
  {
    IdnMapping idnMapping = new IdnMapping();
    string ascii = match.Groups[2].Value;
    try
    {
      ascii = idnMapping.GetAscii(ascii);
    }
    catch (ArgumentException ex)
    {
      this.invalid = true;
    }
    return match.Groups[1].Value + ascii;
  }

  public bool IsValidEmail(string strIn)
  {
    this.invalid = false;
    if (string.IsNullOrEmpty(strIn))
      return false;
    strIn = Regex.Replace(strIn, "(@)(.+)S", new MatchEvaluator(this.DomainMapper));
    return !this.invalid && Regex.IsMatch(strIn, "^(?(\")(\"[^\"]+?\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\S%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9]{2,17}))S", RegexOptions.IgnoreCase);
  }

  private void OnClick()
  {
    if (!this.IsValidEmail(this.email.GetComponent<UIInput>().text))
    {
      this.output.GetComponent<UILabel>().text = "This e-mail address is not valid.";
    }
    else
    {
      this.logincomponent.GetComponent<LoginFengKAI>().resetPassword(this.email.GetComponent<UIInput>().text);
      this.output.GetComponent<UILabel>().text = "please wait...";
    }
  }
}
