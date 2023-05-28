// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsTitanPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
  internal class SettingsSkinsTitanPanel : SettingsCategoryPanel
  {
    protected override float VerticalSpacing => 20f;

    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsSkinsPanel.Parent;
      TitanCustomSkinSet selectedSet = (TitanCustomSkinSet) SettingsManager.CustomSkinSettings.Titan.GetSelectedSet();
      string localeCategory = parent1.LocaleCategory;
      string subCategory = "Skins.Titan";
      settingsSkinsPanel.CreateCommonSettings(this.DoublePanelLeft, this.DoublePanelRight);
      ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.RandomizedPairs, UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairs"), UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairsTooltip"));
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      ElementFactory.CreateDefaultLabel(this.DoublePanelLeft, style, UIManager.GetLocale(localeCategory, subCategory, "Hairs"));
      List<string> stringList = new List<string>()
      {
        "Random"
      };
      for (int index = 0; index < 10; ++index)
        stringList.Add("Hair " + index.ToString());
      string[] array = stringList.ToArray();
      style.TitleWidth = 0.0f;
      for (int index = 0; index < selectedSet.Hairs.GetCount(); ++index)
      {
        GameObject horizontalGroup = ElementFactory.CreateHorizontalGroup(this.DoublePanelLeft, 20f);
        ElementFactory.CreateInputSetting(horizontalGroup.transform, style, selectedSet.Hairs.GetItemAt(index), string.Empty, elementWidth: 260f);
        ElementFactory.CreateDropdownSetting(horizontalGroup.transform, style, selectedSet.HairModels.GetItemAt(index), string.Empty, array);
      }
      settingsSkinsPanel.CreateSkinListStringSettings(selectedSet.Bodies, this.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Bodies"));
      this.CreateHorizontalDivider(this.DoublePanelRight);
      settingsSkinsPanel.CreateSkinListStringSettings(selectedSet.Eyes, this.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Eyes"));
    }
  }
}
