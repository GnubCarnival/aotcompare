// Decompiled with JetBrains decompiler
// Type: UI.SettingsGeneralPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine.Events;

namespace UI
{
  internal class SettingsGeneralPanel : SettingsCategoryPanel
  {
    public override void Setup(BasePanel parent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SettingsGeneralPanel.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new SettingsGeneralPanel.\u003C\u003Ec__DisplayClass0_0();
      base.Setup(parent);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass00.settingsPopup = (SettingsPopup) parent;
      // ISSUE: reference to a compiler-generated field
      string localeCategory = cDisplayClass00.settingsPopup.LocaleCategory;
      string subCategory = "General";
      GeneralSettings generalSettings = SettingsManager.GeneralSettings;
      ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.Language, "Language", UIManager.GetLanguages(), UIManager.GetLocaleCommon("RequireRestart"), 160f, onDropdownOptionSelect: new UnityAction((object) cDisplayClass00, __methodptr(\u003CSetup\u003Eb__0)));
      ElementFactory.CreateSliderSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.Volume, UIManager.GetLocale(localeCategory, subCategory, "Volume"), elementWidth: 135f);
      ElementFactory.CreateSliderSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.MouseSpeed, UIManager.GetLocale(localeCategory, subCategory, "MouseSpeed"), elementWidth: 135f);
      ElementFactory.CreateSliderSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.CameraDistance, UIManager.GetLocale(localeCategory, subCategory, "CameraDistance"), elementWidth: 135f);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.InvertMouse, UIManager.GetLocale(localeCategory, subCategory, "InvertMouse"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.CameraTilt, UIManager.GetLocale(localeCategory, subCategory, "CameraTilt"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) generalSettings.MinimapEnabled, UIManager.GetLocale(localeCategory, subCategory, "MinimapEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) generalSettings.SnapshotsEnabled, UIManager.GetLocale(localeCategory, subCategory, "SnapshotsEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) generalSettings.SnapshotsShowInGame, UIManager.GetLocale(localeCategory, subCategory, "SnapshotsShowInGame"));
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) generalSettings.SnapshotsMinimumDamage, UIManager.GetLocale(localeCategory, subCategory, "SnapshotsMinimumDamage"), elementWidth: 100f);
    }
  }
}
