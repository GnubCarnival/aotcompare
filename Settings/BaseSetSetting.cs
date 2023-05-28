// Decompiled with JetBrains decompiler
// Type: Settings.BaseSetSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal abstract class BaseSetSetting : BaseSettingsContainer
  {
    public StringSetting Name = new StringSetting("Set 1");
    public BoolSetting Preset = new BoolSetting(false);
  }
}
