// Decompiled with JetBrains decompiler
// Type: UI.SettingsGameMiscPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsGameMiscPanel : SettingsCategoryPanel
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
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanPerWavesEnabled, "Custom titans/wave");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanPerWaves, "Titan amount", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanMaxWavesEnabled, "Custom max waves");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanMaxWaves, "Wave amount", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.EndlessRespawnEnabled, "Endless respawn");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.EndlessRespawnTime, "Respawn time", elementWidth: elementWidth);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.PunksEveryFive, "Punks every 5 waves");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.GlobalMinimapDisable, "Global minimap disable");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.PreserveKDR, "Preserve KDR", "Preserve player stats when they leave and rejoin the room.");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.RacingEndless, "Endless racing", "Racing round continues even if someone finishes.");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.RacingStartTime, "Racing start time", elementWidth: elementWidth);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.KickShifters, "Kick shifters");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.AllowHorses, "Allow horses");
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.GlobalHideNames, "Global hide names");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 160f, themePanel: this.ThemePanel), (BaseSetting) legacyGameSettingsUi.Motd, "MOTD", elementWidth: 200f);
    }
  }
}
