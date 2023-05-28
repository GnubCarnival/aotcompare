// Decompiled with JetBrains decompiler
// Type: AuthenticationValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;

public class AuthenticationValues
{
  public string AuthParameters;
  public CustomAuthenticationType AuthType;
  public string Secret;

  public virtual void SetAuthParameters(string user, string token) => this.AuthParameters = "username=" + Uri.EscapeDataString(user) + "&token=" + Uri.EscapeDataString(token);

  public virtual void SetAuthPostData(string stringData) => this.AuthPostData = !string.IsNullOrEmpty(stringData) ? (object) stringData : (object) (string) null;

  public virtual void SetAuthPostData(byte[] byteData) => this.AuthPostData = (object) byteData;

  public override string ToString() => this.AuthParameters + " s: " + this.Secret;

  public object AuthPostData { get; private set; }
}
