// Decompiled with JetBrains decompiler
// Type: Settings.BaseCustomSkinSettings`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal class BaseCustomSkinSettings<T> : 
    SetSettingsContainer<T>,
    ICustomSkinSettings,
    ISetSettingsContainer
    where T : BaseSetSetting, new()
  {
    public BoolSetting SkinsLocal = new BoolSetting(false);
    public BoolSetting SkinsEnabled = new BoolSetting(true);
    public ListSetting<T> SkinSets = new ListSetting<T>();

    public BoolSetting GetSkinsEnabled() => this.SkinsEnabled;

    public IListSetting GetSkinSets() => (IListSetting) this.SkinSets;

    public BoolSetting GetSkinsLocal() => this.SkinsLocal;
  }
}
