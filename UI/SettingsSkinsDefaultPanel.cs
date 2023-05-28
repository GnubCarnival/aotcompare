// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsDefaultPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace UI
{
  internal class SettingsSkinsDefaultPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    protected override float VerticalSpacing => 20f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel) parent;
      settingsSkinsPanel.CreateCommonSettings(this.DoublePanelLeft, this.DoublePanelRight);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      settingsSkinsPanel.CreateSkinStringSettings(this.DoublePanelLeft, this.DoublePanelRight);
    }
  }
}
