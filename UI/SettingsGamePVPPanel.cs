// Decompiled with JetBrains decompiler
// Type: UI.SettingsGamePVPPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsGamePVPPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    protected override float VerticalSpacing => 20f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsGamePanel settingsGamePanel = (SettingsGamePanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsGamePanel.Parent;
      settingsGamePanel.CreateGategoryDropdown(this.DoublePanelLeft);
      float elementWidth = 120f;
      ElementStyle style = new ElementStyle(titleWidth: 240f, themePanel: this.ThemePanel);
      LegacyGameSettings legacyGameSettingsUi = SettingsManager.LegacyGameSettingsUI;
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.PointModeEnabled, "Point mode", "End game after player or team reaches certain number of points.");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.PointModeAmount, "Point amount", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.BombModeEnabled, "Bomb mode");
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.BombModeCeiling, "Bomb ceiling");
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.BombModeInfiniteGas, "Bomb infinite gas");
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.BombModeDisableTitans, "Bomb disable titans");
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TeamMode, "Team mode", new string[4]
      {
        "Off",
        "No sort",
        "Size lock",
        "Skill lock"
      });
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.InfectionModeEnabled, "Infection mode");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.InfectionModeAmount, "Starting titans", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.BladePVP, "Blade/AHSS PVP", new string[3]
      {
        "Off",
        "Teams",
        "FFA"
      });
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.FriendlyMode, "Friendly mode", "Prevent normal AHSS/Blade PVP.");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.AHSSAirReload, "AHSS air reload");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.CannonsFriendlyFire, "Cannons friendly fire");
    }
  }
}
