// Decompiled with JetBrains decompiler
// Type: UI.SettingsCustomMapPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SettingsCustomMapPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string localeCategory = ((SettingsPopup) parent).LocaleCategory;
      LegacyGameSettings legacyGameSettingsUi = SettingsManager.LegacyGameSettingsUI;
      ElementStyle style1 = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(themePanel: this.ThemePanel);
      ElementStyle style3 = new ElementStyle(28, themePanel: this.ThemePanel);
      ElementFactory.CreateDefaultLabel(this.DoublePanelLeft, style1, "Map script");
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style2, (BaseSetting) legacyGameSettingsUi.LevelScript, string.Empty, elementWidth: 420f, elementHeight: 300f, multiLine: true);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(this.DoublePanelLeft, 0.0f, (TextAnchor) 1).transform, style3, "Clear", onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__2_0)));
      string[] options = new string[5]
      {
        "Survive",
        "Waves",
        "PVP",
        "Racing",
        "Custom"
      };
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style1, (BaseSetting) legacyGameSettingsUi.GameType, "Game mode", options);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style1, (BaseSetting) legacyGameSettingsUi.TitanSpawnCap, "Titan cap");
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateDefaultLabel(this.DoublePanelRight, style1, "Logic script");
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style2, (BaseSetting) legacyGameSettingsUi.LogicScript, string.Empty, elementWidth: 420f, elementHeight: 300f, multiLine: true);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(this.DoublePanelRight, 0.0f, (TextAnchor) 1).transform, style3, "Clear", onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__2_1)));
    }

    private void OnCustomMapButtonClick(string name)
    {
      switch (name)
      {
        case "ClearMap":
          SettingsManager.LegacyGameSettingsUI.LevelScript.Value = string.Empty;
          break;
        case "ClearLogic":
          SettingsManager.LegacyGameSettingsUI.LogicScript.Value = string.Empty;
          break;
      }
      this.Parent.RebuildCategoryPanel();
    }
  }
}
