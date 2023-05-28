// Decompiled with JetBrains decompiler
// Type: Settings.GeneralSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Settings
{
  internal class GeneralSettings : SaveableSettingsContainer
  {
    public StringSetting Language = new StringSetting("English");
    public FloatSetting Volume = new FloatSetting(1f, 0.0f, 1f);
    public FloatSetting MouseSpeed = new FloatSetting(0.5f, 0.01f, 1f);
    public FloatSetting CameraDistance = new FloatSetting(1f, 0.0f, 1f);
    public BoolSetting InvertMouse = new BoolSetting(false);
    public BoolSetting CameraTilt = new BoolSetting(true);
    public BoolSetting SnapshotsEnabled = new BoolSetting(false);
    public BoolSetting SnapshotsShowInGame = new BoolSetting(false);
    public IntSetting SnapshotsMinimumDamage = new IntSetting(0, 0);
    public BoolSetting MinimapEnabled = new BoolSetting(false);

    protected override string FileName => "General.json";

    public override void Apply()
    {
      AudioListener.volume = this.Volume.Value;
      IN_GAME_MAIN_CAMERA.cameraDistance = this.CameraDistance.Value + 0.3f;
    }
  }
}
