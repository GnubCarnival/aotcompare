﻿// Decompiled with JetBrains decompiler
// Type: Settings.ProfileSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Settings
{
  internal class ProfileSettings : SaveableSettingsContainer
  {
    public NameSetting Name = new NameSetting("GUEST" + Random.Range(0, 100000).ToString(), 200, 40);
    public NameSetting Guild = new NameSetting(string.Empty, 200, 40);

    protected override string FileName => "Profile.json";

    protected override void LoadLegacy()
    {
      this.Name.Value = SettingsManager.MultiplayerSettings.Name.Value;
      this.Guild.Value = SettingsManager.MultiplayerSettings.Guild.Value;
    }
  }
}
