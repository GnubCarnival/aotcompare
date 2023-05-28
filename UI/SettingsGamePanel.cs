// Decompiled with JetBrains decompiler
// Type: UI.SettingsGamePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SettingsGamePanel : SettingsCategoryPanel
  {
    protected override bool CategoryPanel => true;

    protected override string DefaultCategoryPanel => "Titans";

    public void CreateGategoryDropdown(Transform panel, bool includeReset = true, float elementWidth = 260f)
    {
      ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel);
      string[] options = new string[4]
      {
        "Titans",
        "PVP",
        "Misc",
        "Weather"
      };
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panel, style, (BaseSetting) this._currentCategoryPanelName, "Category", options, elementWidth: elementWidth, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateGategoryDropdown\u003Eb__4_0)));
      if (!includeReset)
        return;
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(panel, 0.0f, (TextAnchor) 5).transform, style, "Reset to default", onClick: new UnityAction((object) this, __methodptr(\u003CCreateGategoryDropdown\u003Eb__4_1)));
      this.CreateHorizontalDivider(panel);
    }

    protected void OnResetButtonClick()
    {
      SettingsManager.LegacyGameSettingsUI.SetDefault();
      this.RebuildCategoryPanel();
    }

    protected override void RegisterCategoryPanels()
    {
      this._categoryPanelTypes.Add("Titans", typeof (SettingsGameTitansPanel));
      this._categoryPanelTypes.Add("PVP", typeof (SettingsGamePVPPanel));
      this._categoryPanelTypes.Add("Misc", typeof (SettingsGameMiscPanel));
      this._categoryPanelTypes.Add("Weather", typeof (SettingsGameWeatherPanel));
    }
  }
}
