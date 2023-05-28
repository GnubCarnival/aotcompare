// Decompiled with JetBrains decompiler
// Type: Settings.AbilitySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Settings
{
  internal class AbilitySettings : SaveableSettingsContainer
  {
    public ColorSetting BombColor = new ColorSetting(new Color(1f, 1f, 1f, 1f), 0.5f);
    public IntSetting BombRadius = new IntSetting(6, 0, 10);
    public IntSetting BombRange = new IntSetting(3, 0, 3);
    public IntSetting BombSpeed = new IntSetting(6, 0, 10);
    public IntSetting BombCooldown = new IntSetting(1, 0, 6);
    public BoolSetting ShowBombColors = new BoolSetting(false);
    public BoolSetting UseOldEffect = new BoolSetting(false);

    protected override string FileName => "Ability01.json";

    protected override bool Validate() => this.BombRadius.Value + this.BombRange.Value + this.BombSpeed.Value + this.BombCooldown.Value <= 16;
  }
}
