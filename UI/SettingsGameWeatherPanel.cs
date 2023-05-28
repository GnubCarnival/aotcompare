// Decompiled with JetBrains decompiler
// Type: UI.SettingsGameWeatherPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using Weather;

namespace UI
{
  internal class SettingsGameWeatherPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    protected override float VerticalSpacing => 20f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsGamePanel settingsGamePanel = (SettingsGamePanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsGamePanel.Parent;
      settingsGamePanel.CreateGategoryDropdown(this.DoublePanelLeft, false, 205f);
      ElementStyle style = new ElementStyle(titleWidth: 180f, themePanel: this.ThemePanel);
      WeatherSettings weatherSettings = SettingsManager.WeatherSettings;
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel), (BaseSetting) weatherSettings.WeatherSets.GetSelectedSetIndex(), "Weather set", weatherSettings.WeatherSets.GetSetNames(), "* = preset and cannot be modified or deleted. Create a new set to save custom settings.", 205f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__4_0)));
      GameObject horizontalGroup1 = ElementFactory.CreateHorizontalGroup(this.DoublePanelLeft, 10f);
      string[] strArray1 = new string[4]
      {
        "Create",
        "Delete",
        "Rename",
        "Copy"
      };
      foreach (string str in strArray1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass4_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass40.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass40.button = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(horizontalGroup1.transform, style, UIManager.GetLocaleCommon(cDisplayClass40.button), onClick: new UnityAction((object) cDisplayClass40, __methodptr(\u003CSetup\u003Eb__1)));
      }
      WeatherSet selectedSet = (WeatherSet) weatherSettings.WeatherSets.GetSelectedSet();
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      ElementStyle elementStyle = new ElementStyle(titleWidth: 150f, themePanel: this.ThemePanel);
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) selectedSet.UseSchedule, "Use schedule", "Follow a programmed weather schedule.");
      ElementFactory.CreateToggleSetting(this.DoublePanelLeft, style, (BaseSetting) selectedSet.ScheduleLoop, "Loop schedule");
      GameObject horizontalGroup2 = ElementFactory.CreateHorizontalGroup(this.DoublePanelLeft, 10f);
      string[] strArray2 = new string[2]
      {
        "Import",
        "Export"
      };
      foreach (string str in strArray2)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass4_1 cDisplayClass41 = new SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass4_1();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass41.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass41.button = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(horizontalGroup2.transform, style, UIManager.GetLocaleCommon(cDisplayClass41.button), onClick: new UnityAction((object) cDisplayClass41, __methodptr(\u003CSetup\u003Eb__2)));
      }
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Skybox, "Skybox", RCextensions.EnumToStringArray<WeatherSkybox>());
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.SkyboxColor, "Skybox color", parent1.ColorPickPopup);
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Daylight, "Daylight", parent1.ColorPickPopup);
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.AmbientLight, "Ambient light", parent1.ColorPickPopup);
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Flashlight, "Flashlight", parent1.ColorPickPopup);
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.FogColor, "Fog color", parent1.ColorPickPopup);
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.FogDensity, "Fog density");
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Rain, "Rain");
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Thunder, "Thunder");
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Snow, "Snow");
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) selectedSet.Wind, "Wind");
    }

    private void OnWeatherPanelButtonClick(string name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new SettingsGameWeatherPanel.\u003C\u003Ec__DisplayClass5_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.name = name;
      SettingsPopup parent = (SettingsPopup) this.Parent.Parent;
      SetNamePopup setNamePopup = parent.SetNamePopup;
      WeatherSettings weatherSettings = SettingsManager.WeatherSettings;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass50.name == null)
        return;
      // ISSUE: reference to a compiler-generated field
      string name1 = cDisplayClass50.name;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name1))
      {
        case 89770346:
          int num = name1 == "Edit schedule" ? 1 : 0;
          break;
        case 1463683828:
          if (!(name1 == "Import"))
            break;
          // ISSUE: method pointer
          parent.ImportPopup.Show(new UnityAction((object) cDisplayClass50, __methodptr(\u003COnWeatherPanelButtonClick\u003Eb__3)));
          break;
        case 1469573738:
          if (!(name1 == "Delete") || !weatherSettings.WeatherSets.CanDeleteSelectedSet())
            break;
          // ISSUE: method pointer
          UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), new UnityAction((object) cDisplayClass50, __methodptr(\u003COnWeatherPanelButtonClick\u003Eb__1)), UIManager.GetLocaleCommon("Delete"));
          break;
        case 1703884388:
          if (!(name1 == "Copy"))
            break;
          // ISSUE: method pointer
          setNamePopup.Show("New set", new UnityAction((object) cDisplayClass50, __methodptr(\u003COnWeatherPanelButtonClick\u003Eb__2)), UIManager.GetLocaleCommon("Copy"));
          break;
        case 2567824509:
          if (!(name1 == "Create"))
            break;
          // ISSUE: method pointer
          setNamePopup.Show("New set", new UnityAction((object) cDisplayClass50, __methodptr(\u003COnWeatherPanelButtonClick\u003Eb__0)), UIManager.GetLocaleCommon("Create"));
          break;
        case 3355849203:
          if (!(name1 == "Rename") || !weatherSettings.WeatherSets.CanEditSelectedSet())
            break;
          string initialValue = weatherSettings.WeatherSets.GetSelectedSet().Name.Value;
          // ISSUE: method pointer
          setNamePopup.Show(initialValue, new UnityAction((object) cDisplayClass50, __methodptr(\u003COnWeatherPanelButtonClick\u003Eb__4)), UIManager.GetLocaleCommon("Rename"));
          break;
        case 3898821075:
          if (!(name1 == "Export"))
            break;
          parent.ExportPopup.Show(((WeatherSet) weatherSettings.WeatherSets.GetSelectedSet()).Schedule.Value);
          break;
      }
    }

    private void OnWeatherSetOperationFinish(string name)
    {
      SettingsPopup parent = (SettingsPopup) this.Parent.Parent;
      SetNamePopup setNamePopup = parent.SetNamePopup;
      SetSettingsContainer<WeatherSet> weatherSets = SettingsManager.WeatherSettings.WeatherSets;
      switch (name)
      {
        case "Create":
          weatherSets.CreateSet(setNamePopup.NameSetting.Value);
          weatherSets.GetSelectedSetIndex().Value = weatherSets.GetSets().GetCount() - 1;
          break;
        case "Delete":
          weatherSets.DeleteSelectedSet();
          weatherSets.GetSelectedSetIndex().Value = 0;
          break;
        case "Rename":
          weatherSets.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
          break;
        case "Copy":
          weatherSets.CopySelectedSet(setNamePopup.NameSetting.Value);
          weatherSets.GetSelectedSetIndex().Value = weatherSets.GetSets().GetCount() - 1;
          break;
        case "Import":
          ImportPopup importPopup = parent.ImportPopup;
          ((WeatherSet) weatherSets.GetSelectedSet()).Schedule.Value = importPopup.ImportSetting.Value;
          break;
      }
      this.Parent.RebuildCategoryPanel();
    }
  }
}
