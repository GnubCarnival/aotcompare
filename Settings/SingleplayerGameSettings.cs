﻿// Decompiled with JetBrains decompiler
// Type: Settings.SingleplayerGameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal class SingleplayerGameSettings : BaseSettingsContainer
  {
    public StringSetting Map = new StringSetting("[S]Tutorial");
    public IntSetting Difficulty = new IntSetting(0);
    public StringSetting Character = new StringSetting("Mikasa");
    public IntSetting Costume = new IntSetting(0);
    public IntSetting CameraType = new IntSetting(0);
  }
}
