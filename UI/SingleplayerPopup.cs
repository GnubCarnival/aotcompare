// Decompiled with JetBrains decompiler
// Type: UI.SingleplayerPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SingleplayerPopup : BasePopup
  {
    private string[] _characterOptions = new string[12]
    {
      "Mikasa",
      "Levi",
      "Armin",
      "Marco",
      "Jean",
      "Eren",
      "Titan_Eren",
      "Petra",
      "Sasha",
      "Set 1",
      "Set 2",
      "Set 3"
    };
    private string[] _costumeOptions = new string[3]
    {
      "Costume 1",
      "Costume 2",
      "Costume 3"
    };

    protected override string Title => UIManager.GetLocale("MainMenu", nameof (SingleplayerPopup), nameof (Title));

    protected override float Width => 800f;

    protected override float Height => 510f;

    protected override bool DoublePanel => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SingleplayerGameSettings singleplayerGameSettings = SettingsManager.SingleplayerGameSettings;
      string category = "MainMenu";
      string subCategory = nameof (SingleplayerPopup);
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Start"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_1)));
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style2, (BaseSetting) singleplayerGameSettings.Map, UIManager.GetLocaleCommon("Map"), this.GetMapOptions(), elementWidth: 180f, optionsWidth: new float?(360f));
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style2, (BaseSetting) SettingsManager.WeatherSettings.WeatherSets.SelectedSetIndex, UIManager.GetLocale(category, subCategory, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelLeft, style2, (BaseSetting) singleplayerGameSettings.Difficulty, UIManager.GetLocale(category, subCategory, "Difficulty"), UIManager.GetLocaleArray(category, subCategory, "DifficultyOptions"));
      ElementFactory.CreateDropdownSetting(this.DoublePanelRight, style2, (BaseSetting) singleplayerGameSettings.Character, UIManager.GetLocale(category, subCategory, "Character"), this._characterOptions, elementWidth: 180f);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style2, (BaseSetting) singleplayerGameSettings.Costume, UIManager.GetLocale(category, subCategory, "Costume"), this._costumeOptions);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelRight, style2, (BaseSetting) singleplayerGameSettings.CameraType, UIManager.GetLocale(category, subCategory, "Camera"), RCextensions.EnumToStringArray<CAMERA_TYPE>());
    }

    private string[] GetMapOptions()
    {
      LevelInfo.Init();
      int[] numArray = new int[7]
      {
        15,
        16,
        12,
        13,
        14,
        24,
        18
      };
      List<string> stringList = new List<string>();
      foreach (int index in numArray)
        stringList.Add(LevelInfo.levels[index].name);
      return stringList.ToArray();
    }

    private void OnButtonClick(string name)
    {
      switch (name)
      {
        case "Back":
          this.Hide();
          break;
        case "Start":
          this.StartSinglePlayer();
          break;
      }
    }

    private void StartSinglePlayer()
    {
      SingleplayerGameSettings singleplayerGameSettings = SettingsManager.SingleplayerGameSettings;
      IN_GAME_MAIN_CAMERA.difficulty = singleplayerGameSettings.Difficulty.Value;
      IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
      IN_GAME_MAIN_CAMERA.singleCharacter = singleplayerGameSettings.Character.Value.ToUpper();
      IN_GAME_MAIN_CAMERA.cameraMode = (CAMERA_TYPE) singleplayerGameSettings.CameraType.Value;
      CheckBoxCostume.costumeSet = singleplayerGameSettings.Costume.Value + 1;
      FengGameManagerMKII.level = singleplayerGameSettings.Map.Value;
      Application.LoadLevel(LevelInfo.getInfo(singleplayerGameSettings.Map.Value).mapName);
    }
  }
}
