// Decompiled with JetBrains decompiler
// Type: KickState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;

public class KickState
{
  public int id;
  private int kickCount;
  private string kickers;
  private ArrayList kickers2;
  public string name;

  public void addKicker(string n)
  {
    if (this.kickers.Contains(n))
      return;
    this.kickers += n;
    ++this.kickCount;
  }

  public int getKickCount() => this.kickCount;

  public void init(string n)
  {
    this.name = n;
    this.kickers = string.Empty;
    this.kickCount = 0;
  }
}
