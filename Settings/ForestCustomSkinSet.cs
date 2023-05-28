// Decompiled with JetBrains decompiler
// Type: Settings.ForestCustomSkinSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal class ForestCustomSkinSet : BaseSetSetting
  {
    public BoolSetting RandomizedPairs = new BoolSetting(false);
    public ListSetting<StringSetting> TreeTrunks = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 8);
    public ListSetting<StringSetting> TreeLeafs = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 8);
    public StringSetting Ground = new StringSetting(string.Empty, 200);

    protected override bool Validate() => this.TreeTrunks.Value.Count == 8 && this.TreeLeafs.Value.Count == 8;
  }
}
