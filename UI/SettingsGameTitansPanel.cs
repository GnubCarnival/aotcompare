// Decompiled with JetBrains decompiler
// Type: UI.SettingsGameTitansPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsGameTitansPanel : SettingsCategoryPanel
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
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanNumberEnabled, "Custom titan number");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanNumber, "Titan amount", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnEnabled, "Custom titan spawns", "Spawn rates must add up to 100.");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnNormal, "Normal", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnAberrant, "Aberrant", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnJumper, "Jumper", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnCrawler, "Crawler", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSpawnPunk, "Punk", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSizeEnabled, "Custom titan sizes");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSizeMin, "Minimum size", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) legacyGameSettingsUi.TitanSizeMax, "Maximum size", elementWidth: elementWidth);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanHealthMode, "Titan health", new string[3]
      {
        "Off",
        "Fixed",
        "Scaled"
      });
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanHealthMin, "Minimum health", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanHealthMax, "Maximum health", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanArmorEnabled, "Titan armor");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanArmor, "Armor amount", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanExplodeEnabled, "Titan explode");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.TitanExplodeRadius, "Explode radius", elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) legacyGameSettingsUi.RockThrowEnabled, "Punk rock throwing");
    }
  }
}
