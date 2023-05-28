// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsForestPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsSkinsForestPanel : SettingsCategoryPanel
  {
    protected override float VerticalSpacing => 20f;

    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsSkinsPanel.Parent;
      ForestCustomSkinSet selectedSet = (ForestCustomSkinSet) SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
      string localeCategory = parent1.LocaleCategory;
      string subCategory = "Skins.Forest";
      settingsSkinsPanel.CreateCommonSettings(this.DoublePanelLeft, this.DoublePanelRight);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel), (BaseSetting) selectedSet.RandomizedPairs, UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairs"), UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairsTooltip"));
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel), (BaseSetting) selectedSet.Ground, UIManager.GetLocale(localeCategory, "Skins.Common", "Ground"), elementWidth: 260f);
      settingsSkinsPanel.CreateSkinListStringSettings(selectedSet.TreeTrunks, this.DoublePanelLeft, UIManager.GetLocale(localeCategory, subCategory, "TreeTrunks"));
      settingsSkinsPanel.CreateSkinListStringSettings(selectedSet.TreeLeafs, this.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "TreeLeafs"));
    }
  }
}
