// Decompiled with JetBrains decompiler
// Type: LoginFengKAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
  private string ChangeGuildURL = "http://aotskins.com/version/guild.php";
  private string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";
  private string CheckUserURL = "http://aotskins.com/version/login.php";
  private string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";
  public string formText = string.Empty;
  private string GetInfoURL = "http://aotskins.com/version/getinfo.php";
  public PanelLoginGroupManager loginGroup;
  public GameObject output;
  public GameObject output2;
  public GameObject panelChangeGUILDNAME;
  public GameObject panelChangePassword;
  public GameObject panelForget;
  public GameObject panelLogin;
  public GameObject panelRegister;
  public GameObject panelStatus;
  public static PlayerInfoPHOTON player;
  private static string playerGUILDName = string.Empty;
  private static string playerName = string.Empty;
  private static string playerPassword = string.Empty;
  private string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";

  public void cGuild(string name)
  {
    if (LoginFengKAI.playerName == string.Empty)
    {
      this.logout();
      NGUITools.SetActive(this.panelChangeGUILDNAME, false);
      NGUITools.SetActive(this.panelLogin, true);
      this.output.GetComponent<UILabel>().text = "Please sign in.";
    }
    else
      this.StartCoroutine(this.changeGuild(name));
  }

  [DebuggerHidden]
  private IEnumerator changeGuild(string name) => (IEnumerator) new LoginFengKAI.changeGuildcIterator5()
  {
    name = name,
    Sname = name,
    fthis = this
  };

  [DebuggerHidden]
  private IEnumerator changePassword(string oldpassword, string password, string password2) => (IEnumerator) new LoginFengKAI.changePasswordcIterator4()
  {
    oldpassword = oldpassword,
    password = password,
    password2 = password2,
    Soldpassword = oldpassword,
    Spassword = password,
    Spassword2 = password2,
    fthis = this
  };

  private void clearCOOKIE()
  {
    LoginFengKAI.playerName = string.Empty;
    LoginFengKAI.playerPassword = string.Empty;
  }

  public void cpassword(string oldpassword, string password, string password2)
  {
    if (LoginFengKAI.playerName == string.Empty)
    {
      this.logout();
      NGUITools.SetActive(this.panelChangePassword, false);
      NGUITools.SetActive(this.panelLogin, true);
      this.output.GetComponent<UILabel>().text = "Please sign in.";
    }
    else
      this.StartCoroutine(this.changePassword(oldpassword, password, password2));
  }

  [DebuggerHidden]
  private IEnumerator ForgetPassword(string email) => (IEnumerator) new LoginFengKAI.ForgetPasswordcIterator6()
  {
    email = email,
    Semail = email,
    fthis = this
  };

  [DebuggerHidden]
  private IEnumerator getInfo() => (IEnumerator) new LoginFengKAI.getInfocIterator2()
  {
    fthis = this
  };

  public void login(string name, string password) => this.StartCoroutine(this.Login(name, password));

  [DebuggerHidden]
  private IEnumerator Login(string name, string password) => (IEnumerator) new LoginFengKAI.LogincIterator1()
  {
    name = name,
    password = password,
    Sname = name,
    Spassword = password,
    fthis = this
  };

  public void logout()
  {
    this.clearCOOKIE();
    LoginFengKAI.player = new PlayerInfoPHOTON();
    LoginFengKAI.player.initAsGuest();
    this.output.GetComponent<UILabel>().text = "Welcome," + LoginFengKAI.player.name;
  }

  [DebuggerHidden]
  private IEnumerator Register(string name, string password, string password2, string email) => (IEnumerator) new LoginFengKAI.RegistercIterator3()
  {
    name = name,
    password = password,
    password2 = password2,
    email = email,
    Sname = name,
    Spassword = password,
    Spassword2 = password2,
    Semail = email,
    fthis = this
  };

  public void resetPassword(string email) => this.StartCoroutine(this.ForgetPassword(email));

  public void signup(string name, string password, string password2, string email) => this.StartCoroutine(this.Register(name, password, password2, email));

  private void Start()
  {
    if (LoginFengKAI.player == null)
    {
      LoginFengKAI.player = new PlayerInfoPHOTON();
      LoginFengKAI.player.initAsGuest();
    }
    if (LoginFengKAI.playerName != string.Empty)
    {
      NGUITools.SetActive(this.panelLogin, false);
      NGUITools.SetActive(this.panelStatus, true);
      this.StartCoroutine(this.getInfo());
    }
    else
      this.output.GetComponent<UILabel>().text = "Welcome," + LoginFengKAI.player.name;
  }
}
