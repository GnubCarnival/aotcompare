// Decompiled with JetBrains decompiler
// Type: UI.SettingsKeybindsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SettingsKeybindsPanel : SettingsCategoryPanel
  {
    protected string[] _categories = new string[6]
    {
      "General",
      "Human",
      "Titan",
      "Shifter",
      "Interaction",
      "RC Editor"
    };

    protected override bool CategoryPanel => true;

    protected override string DefaultCategoryPanel => "General";

    public void CreateGategoryDropdown(Transform panel)
    {
      ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panel, style, (BaseSetting) this._currentCategoryPanelName, "Category", this._categories, elementWidth: 260f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateGategoryDropdown\u003Eb__5_0)));
    }

    protected override void RegisterCategoryPanels()
    {
      foreach (string category in this._categories)
        this._categoryPanelTypes.Add(category, typeof (SettingsKeybindsDefaultPanel));
    }
  }
}
