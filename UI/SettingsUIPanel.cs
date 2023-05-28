// Decompiled with JetBrains decompiler
// Type: UI.SettingsUIPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsUIPanel : SettingsCategoryPanel
  {
    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string localeCategory = ((SettingsPopup) parent).LocaleCategory;
      string subCategory = "UI";
      UISettings uiSettings = SettingsManager.UISettings;
      ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.UITheme, UIManager.GetLocale(localeCategory, subCategory, "Theme"), UIManager.GetUIThemes(), UIManager.GetLocaleCommon("RequireRestart"), 160f);
      ElementFactory.CreateSliderSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.UIMasterScale, UIManager.GetLocale(localeCategory, subCategory, "UIScale"), elementWidth: 135f);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.GameFeed, UIManager.GetLocale(localeCategory, subCategory, "GameFeed"), UIManager.GetLocale(localeCategory, subCategory, "GameFeedTooltip"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.ShowEmotes, UIManager.GetLocale(localeCategory, subCategory, "ShowEmotes"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.ShowInterpolation, UIManager.GetLocale(localeCategory, subCategory, "ShowInterpolation"), UIManager.GetLocale(localeCategory, subCategory, "ShowInterpolationTooltip"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.HideNames, UIManager.GetLocale(localeCategory, subCategory, "HideNames"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) SettingsManager.UISettings.DisableNameColors, UIManager.GetLocale(localeCategory, subCategory, "DisableNameColors"));
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.UISettings.CrosshairStyle, UIManager.GetLocale(localeCategory, subCategory, "CrosshairStyle"), UIManager.GetLocaleArray(localeCategory, subCategory, "CrosshairStyleOptions"), elementWidth: 200f);
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 150f, themePanel: this.ThemePanel), (BaseSetting) SettingsManager.UISettings.CrosshairScale, UIManager.GetLocale(localeCategory, subCategory, "CrosshairScale"), elementWidth: 185f);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.UISettings.ShowCrosshairDistance, UIManager.GetLocale(localeCategory, subCategory, "ShowCrosshairDistance"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.UISettings.ShowCrosshairArrows, UIManager.GetLocale(localeCategory, subCategory, "ShowCrosshairArrows"));
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.UISettings.Speedometer, UIManager.GetLocale(localeCategory, subCategory, "Speedometer"), UIManager.GetLocaleArray(localeCategory, subCategory, "SpeedometerOptions"));
    }
  }
}
