// Decompiled with JetBrains decompiler
// Type: UI.SettingsGraphicsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsGraphicsPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string localeCategory = ((SettingsPopup) parent).LocaleCategory;
      string subCategory = "Graphics";
      GraphicsSettings graphicsSettings = SettingsManager.GraphicsSettings;
      ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.OverallQuality, UIManager.GetLocale(localeCategory, subCategory, "OverallQuality"), UIManager.GetLocaleArray(localeCategory, subCategory, "OverallQualityOptions"), elementWidth: 200f);
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.TextureQuality, UIManager.GetLocale(localeCategory, subCategory, "TextureQuality"), UIManager.GetLocaleArray(localeCategory, subCategory, "TextureQualityOptions"), elementWidth: 200f);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.VSync, UIManager.GetLocale(localeCategory, subCategory, "VSync"));
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.FPSCap, UIManager.GetLocale(localeCategory, subCategory, "FPSCap"), elementWidth: 100f);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.ShowFPS, UIManager.GetLocale(localeCategory, subCategory, "ShowFPS"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.ExclusiveFullscreen, UIManager.GetLocale(localeCategory, subCategory, "ExclusiveFullscreen"), UIManager.GetLocale(localeCategory, subCategory, "ExclusiveFullscreenTooltip"));
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) graphicsSettings.InterpolationEnabled, UIManager.GetLocale(localeCategory, subCategory, "InterpolationEnabled"), UIManager.GetLocale(localeCategory, subCategory, "InterpolationEnabledTooltip"));
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.WeatherEffects, UIManager.GetLocale(localeCategory, subCategory, "WeatherEffects"), UIManager.GetLocaleArray(localeCategory, subCategory, "WeatherEffectsOptions"), elementWidth: 200f);
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.AntiAliasing, UIManager.GetLocale(localeCategory, subCategory, "AntiAliasing"), UIManager.GetLocaleArray(localeCategory, subCategory, "AntiAliasingOptions"), elementWidth: 200f);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.RenderDistance, UIManager.GetLocale(localeCategory, subCategory, "RenderDistance"), UIManager.GetLocale(localeCategory, subCategory, "RenderDistanceTooltip"), 100f);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.AnimatedIntro, UIManager.GetLocale(localeCategory, subCategory, "AnimatedIntro"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.WindEffectEnabled, UIManager.GetLocale(localeCategory, subCategory, "WindEffectEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.WeaponTrailEnabled, UIManager.GetLocale(localeCategory, subCategory, "WeaponTrailEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.BlurEnabled, UIManager.GetLocale(localeCategory, subCategory, "BlurEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) graphicsSettings.MipmapEnabled, UIManager.GetLocale(localeCategory, subCategory, "MipmapEnabled"), UIManager.GetLocale(localeCategory, subCategory, "MipmapEnabledTooltip"));
    }
  }
}
