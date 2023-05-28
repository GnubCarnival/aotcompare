// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerCreatePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class MultiplayerCreatePopup : PromptPopup
  {
    protected override string Title => UIManager.GetLocale("MainMenu", nameof (MultiplayerCreatePopup), nameof (Title));

    protected override float Width => 800f;

    protected override float Height => 500f;

    protected override bool DoublePanel => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      MultiplayerGameSettings multiplayerGameSettings = SettingsManager.MultiplayerGameSettings;
      string category = "MainMenu";
      string subCategory1 = nameof (MultiplayerCreatePopup);
      string subCategory2 = "SingleplayerPopup";
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(themePanel: this.ThemePanel);
      ElementStyle style3 = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Start"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__8_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__8_1)));
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style2, (BaseSetting) multiplayerGameSettings.Map, UIManager.GetLocaleCommon("Map"), this.GetMapOptions(), elementWidth: 180f, optionsWidth: new float?(360f));
      ElementFactory.CreateDropdownSetting(this.DoublePanelLeft, style2, (BaseSetting) SettingsManager.WeatherSettings.WeatherSets.GetSelectedSetIndex(), UIManager.GetLocale(category, subCategory2, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
      ElementFactory.CreateToggleGroupSetting(this.DoublePanelLeft, style2, (BaseSetting) multiplayerGameSettings.Difficulty, UIManager.GetLocale(category, subCategory2, "Difficulty"), UIManager.GetLocaleArray(category, subCategory2, "DifficultyOptions"));
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style3, (BaseSetting) multiplayerGameSettings.Name, UIManager.GetLocale(category, subCategory1, "ServerName"), elementWidth: 200f);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style3, (BaseSetting) multiplayerGameSettings.Password, UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style3, (BaseSetting) multiplayerGameSettings.MaxPlayers, UIManager.GetLocale(category, subCategory1, "MaxPlayers"), elementWidth: 200f);
      ElementFactory.CreateInputSetting(this.DoublePanelRight, style3, (BaseSetting) multiplayerGameSettings.MaxTime, UIManager.GetLocale(category, subCategory1, "MaxTime"), elementWidth: 200f);
    }

    private string[] GetMapOptions()
    {
      LevelInfo.Init();
      int[] numArray = new int[18]
      {
        0,
        3,
        4,
        5,
        17,
        6,
        7,
        8,
        9,
        10,
        11,
        19,
        20,
        21,
        22,
        23,
        25,
        26
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
          this.StartMultiplayer();
          break;
      }
    }

    private void StartMultiplayer()
    {
      MultiplayerGameSettings multiplayerGameSettings = SettingsManager.MultiplayerGameSettings;
      string str1 = multiplayerGameSettings.Name.Value;
      int maxPlayers = multiplayerGameSettings.MaxPlayers.Value;
      int num = multiplayerGameSettings.MaxTime.Value;
      string str2 = multiplayerGameSettings.Map.Value;
      string str3 = "normal";
      if (multiplayerGameSettings.Difficulty.Value == 2)
        str3 = "abnormal";
      else if (multiplayerGameSettings.Difficulty.Value == 1)
        str3 = "hard";
      string str4 = "day";
      string unencrypted = multiplayerGameSettings.Password.Value;
      if (unencrypted.Length > 0)
        unencrypted = new SimpleAES().Encrypt(unencrypted);
      PhotonNetwork.CreateRoom(str1 + "`" + str2 + "`" + str3 + "`" + (object) num + "`" + str4 + "`" + unencrypted + "`" + (object) Random.Range(0, 50000), true, true, maxPlayers);
    }
  }
}
