// Decompiled with JetBrains decompiler
// Type: Settings.ICustomSkinSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal interface ICustomSkinSettings : ISetSettingsContainer
  {
    BoolSetting GetSkinsLocal();

    BoolSetting GetSkinsEnabled();

    IListSetting GetSkinSets();
  }
}
