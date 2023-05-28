// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SettingsSkinsPanel : SettingsCategoryPanel
  {
    protected Dictionary<string, ICustomSkinSettings> _settings = new Dictionary<string, ICustomSkinSettings>();

    protected override bool CategoryPanel => true;

    protected override string DefaultCategoryPanel => "Human";

    public void CreateCommonSettings(Transform panelLeft, Transform panelRight)
    {
      ElementStyle style1 = new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      ICustomSkinSettings currentSettings = this.GetCurrentSettings();
      string str1 = this._currentCategoryPanelName.Value;
      string[] options = new string[7]
      {
        "Human",
        "Titan",
        "Shifter",
        "Skybox",
        "Forest",
        "City",
        "Custom level"
      };
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panelLeft, style1, (BaseSetting) this._currentCategoryPanelName, UIManager.GetLocaleCommon("Category"), options, elementWidth: 260f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateCommonSettings\u003Eb__5_0)));
      string subCategory = "Skins.Common";
      string localeCategory = ((SettingsPopup) this.Parent).LocaleCategory;
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panelLeft, style1, (BaseSetting) currentSettings.GetSelectedSetIndex(), UIManager.GetLocale(localeCategory, subCategory, "Set"), currentSettings.GetSetNames(), elementWidth: 260f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateCommonSettings\u003Eb__5_1)));
      GameObject horizontalGroup = ElementFactory.CreateHorizontalGroup(panelLeft, 10f, (TextAnchor) 2);
      string[] strArray = new string[4]
      {
        "Create",
        "Delete",
        "Rename",
        "Copy"
      };
      foreach (string str2 in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SettingsSkinsPanel.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new SettingsSkinsPanel.\u003C\u003Ec__DisplayClass5_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass50.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass50.button = str2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(horizontalGroup.transform, style1, UIManager.GetLocaleCommon(cDisplayClass50.button), onClick: new UnityAction((object) cDisplayClass50, __methodptr(\u003CCreateCommonSettings\u003Eb__2)));
      }
      ElementFactory.CreateToggleSetting(panelRight, style2, (BaseSetting) currentSettings.GetSkinsEnabled(), str1 + " " + UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsEnabled"));
      ElementFactory.CreateToggleSetting(panelRight, style2, (BaseSetting) currentSettings.GetSkinsLocal(), str1 + " " + UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsLocal"), UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsLocalTooltip"));
    }

    private void OnSkinsPanelButtonClick(string name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SettingsSkinsPanel.\u003C\u003Ec__DisplayClass6_0 cDisplayClass60 = new SettingsSkinsPanel.\u003C\u003Ec__DisplayClass6_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass60.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass60.name = name;
      SetNamePopup setNamePopup = ((SettingsPopup) this.Parent).SetNamePopup;
      ICustomSkinSettings setting = this._settings[this._currentCategoryPanelName.Value];
      // ISSUE: reference to a compiler-generated field
      switch (cDisplayClass60.name)
      {
        case "Create":
          // ISSUE: method pointer
          setNamePopup.Show("New set", new UnityAction((object) cDisplayClass60, __methodptr(\u003COnSkinsPanelButtonClick\u003Eb__0)), UIManager.GetLocaleCommon("Create"));
          break;
        case "Delete":
          if (!setting.CanDeleteSelectedSet())
            break;
          // ISSUE: method pointer
          UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), new UnityAction((object) cDisplayClass60, __methodptr(\u003COnSkinsPanelButtonClick\u003Eb__1)), UIManager.GetLocaleCommon("Delete"));
          break;
        case "Rename":
          string initialValue = setting.GetSelectedSet().Name.Value;
          // ISSUE: method pointer
          setNamePopup.Show(initialValue, new UnityAction((object) cDisplayClass60, __methodptr(\u003COnSkinsPanelButtonClick\u003Eb__2)), UIManager.GetLocaleCommon("Rename"));
          break;
        case "Copy":
          // ISSUE: method pointer
          setNamePopup.Show("New set", new UnityAction((object) cDisplayClass60, __methodptr(\u003COnSkinsPanelButtonClick\u003Eb__3)), UIManager.GetLocaleCommon("Copy"));
          break;
      }
    }

    private void OnSkinsSetOperationFinish(string name)
    {
      SetNamePopup setNamePopup = ((SettingsPopup) this.Parent).SetNamePopup;
      ICustomSkinSettings currentSettings = this.GetCurrentSettings();
      switch (name)
      {
        case "Create":
          currentSettings.CreateSet(setNamePopup.NameSetting.Value);
          currentSettings.GetSelectedSetIndex().Value = currentSettings.GetSets().GetCount() - 1;
          break;
        case "Delete":
          currentSettings.DeleteSelectedSet();
          currentSettings.GetSelectedSetIndex().Value = 0;
          break;
        case "Rename":
          currentSettings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
          break;
        case "Copy":
          currentSettings.CopySelectedSet(setNamePopup.NameSetting.Value);
          currentSettings.GetSelectedSetIndex().Value = currentSettings.GetSets().GetCount() - 1;
          break;
      }
      this.RebuildCategoryPanel();
    }

    public void CreateSkinListStringSettings(
      ListSetting<StringSetting> list,
      Transform panel,
      string title)
    {
      ElementStyle style = new ElementStyle(titleWidth: 0.0f, themePanel: this.ThemePanel);
      ElementFactory.CreateDefaultLabel(panel, style, title);
      foreach (StringSetting setting in list.Value)
        ElementFactory.CreateInputSetting(panel, style, (BaseSetting) setting, string.Empty, elementWidth: 420f);
    }

    public void CreateSkinStringSettings(
      Transform panelLeft,
      Transform panelRight,
      float titleWidth = 140f,
      float elementWidth = 260f,
      int leftCount = 0)
    {
      ElementStyle style = new ElementStyle(titleWidth: titleWidth, themePanel: this.ThemePanel);
      BaseSettingsContainer selectedSet = (BaseSettingsContainer) this.GetCurrentSettings().GetSelectedSet();
      string localeCategory = ((SettingsPopup) this.Parent).LocaleCategory;
      string str = "Skins." + this._currentCategoryPanelName.Value;
      int num = 1;
      foreach (DictionaryEntry setting1 in selectedSet.Settings)
      {
        BaseSetting setting2 = (BaseSetting) setting1.Value;
        string key = (string) setting1.Key;
        Transform parent = num <= leftCount ? panelLeft : panelRight;
        if ((setting2.GetType() == typeof (StringSetting) || setting2.GetType() == typeof (FloatSetting)) && key != "Name")
        {
          string subCategory = str;
          if (key == "Ground")
            subCategory = "Skins.Common";
          ElementFactory.CreateInputSetting(parent, style, setting2, UIManager.GetLocale(localeCategory, subCategory, key), elementWidth: elementWidth);
          ++num;
        }
      }
    }

    public ICustomSkinSettings GetCurrentSettings() => this._settings[this._currentCategoryPanelName.Value];

    protected override void RegisterCategoryPanels()
    {
      this._categoryPanelTypes.Add("Human", typeof (SettingsSkinsHumanPanel));
      this._categoryPanelTypes.Add("Titan", typeof (SettingsSkinsTitanPanel));
      this._categoryPanelTypes.Add("Forest", typeof (SettingsSkinsForestPanel));
      this._categoryPanelTypes.Add("City", typeof (SettingsSkinsCityPanel));
      this._categoryPanelTypes.Add("Shifter", typeof (SettingsSkinsDefaultPanel));
      this._categoryPanelTypes.Add("Skybox", typeof (SettingsSkinsDefaultPanel));
      this._categoryPanelTypes.Add("Custom level", typeof (SettingsSkinsDefaultPanel));
      this._settings.Add("Human", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.Human);
      this._settings.Add("Titan", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.Titan);
      this._settings.Add("Forest", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.Forest);
      this._settings.Add("City", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.City);
      this._settings.Add("Shifter", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.Shifter);
      this._settings.Add("Skybox", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.Skybox);
      this._settings.Add("Custom level", (ICustomSkinSettings) SettingsManager.CustomSkinSettings.CustomLevel);
    }
  }
}
