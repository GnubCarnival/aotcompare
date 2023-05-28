// Decompiled with JetBrains decompiler
// Type: Settings.CityCustomSkinSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal class CityCustomSkinSet : BaseSetSetting
  {
    public ListSetting<StringSetting> Houses = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 8);
    public StringSetting Ground = new StringSetting(string.Empty, 200);
    public StringSetting Wall = new StringSetting(string.Empty, 200);
    public StringSetting Gate = new StringSetting(string.Empty, 200);

    protected override bool Validate() => this.Houses.Value.Count == 8;
  }
}
