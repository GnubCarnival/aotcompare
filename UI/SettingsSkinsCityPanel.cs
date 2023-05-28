// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsCityPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsSkinsCityPanel : SettingsCategoryPanel
  {
    protected override float VerticalSpacing => 20f;

    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsSkinsPanel.Parent;
      CityCustomSkinSet selectedSet = (CityCustomSkinSet) SettingsManager.CustomSkinSettings.City.GetSelectedSet();
      string localeCategory = parent1.LocaleCategory;
      string subCategory = "Skins.City";
      ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel);
      settingsSkinsPanel.CreateCommonSettings(this.DoublePanelLeft, this.DoublePanelRight);
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) selectedSet.Ground, UIManager.GetLocale(localeCategory, "Skins.Common", "Ground"), elementWidth: 260f);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) selectedSet.Wall, UIManager.GetLocale(localeCategory, subCategory, "Wall"), elementWidth: 260f);
      ElementFactory.CreateInputSetting(this.DoublePanelLeft, style, (BaseSetting) selectedSet.Gate, UIManager.GetLocale(localeCategory, subCategory, "Gate"), elementWidth: 260f);
      settingsSkinsPanel.CreateSkinListStringSettings(selectedSet.Houses, this.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Houses"));
    }
  }
}
