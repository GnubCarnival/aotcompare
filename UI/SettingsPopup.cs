// Decompiled with JetBrains decompiler
// Type: UI.SettingsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class SettingsPopup : BasePopup
  {
    public string LocaleCategory = nameof (SettingsPopup);
    public KeybindPopup KeybindPopup;
    public ColorPickPopup ColorPickPopup;
    public SetNamePopup SetNamePopup;
    public ImportPopup ImportPopup;
    public ExportPopup ExportPopup;
    public EditWeatherSchedulePopup EditWeatherSchedulePopup;
    private List<BaseSettingsContainer> _ignoreDefaultButtonSettings = new List<BaseSettingsContainer>();
    private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

    protected override string Title => string.Empty;

    protected override float Width => 1010f;

    protected override float Height => 630f;

    protected override bool CategoryPanel => true;

    protected override bool CategoryButtons => true;

    protected override string DefaultCategoryPanel => "General";

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      this.SetupBottomButtons();
      this.SetupSettingsList();
    }

    protected override void SetupTopButtons()
    {
      ElementStyle style = new ElementStyle(28, themePanel: this.ThemePanel);
      string[] strArray = new string[8]
      {
        "General",
        "Graphics",
        "UI",
        "Keybinds",
        "Skins",
        "CustomMap",
        "Game",
        "Ability"
      };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SettingsPopup.\u003C\u003Ec__DisplayClass22_0 cDisplayClass220 = new SettingsPopup.\u003C\u003Ec__DisplayClass22_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass220.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass220.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        GameObject categoryButton = ElementFactory.CreateCategoryButton(this.TopBar, style, UIManager.GetLocale(this.LocaleCategory, "Top", cDisplayClass220.buttonName + "Button"), new UnityAction((object) cDisplayClass220, __methodptr(\u003CSetupTopButtons\u003Eb__0)));
        // ISSUE: reference to a compiler-generated field
        this._topButtons.Add(cDisplayClass220.buttonName, categoryButton.GetComponent<Button>());
      }
      base.SetupTopButtons();
    }

    protected override void RegisterCategoryPanels()
    {
      this._categoryPanelTypes.Add("General", typeof (SettingsGeneralPanel));
      this._categoryPanelTypes.Add("Graphics", typeof (SettingsGraphicsPanel));
      this._categoryPanelTypes.Add("UI", typeof (SettingsUIPanel));
      this._categoryPanelTypes.Add("Keybinds", typeof (SettingsKeybindsPanel));
      this._categoryPanelTypes.Add("Skins", typeof (SettingsSkinsPanel));
      this._categoryPanelTypes.Add("CustomMap", typeof (SettingsCustomMapPanel));
      this._categoryPanelTypes.Add("Game", typeof (SettingsGamePanel));
      this._categoryPanelTypes.Add("Ability", typeof (SettingsAbilityPanel));
    }

    protected override void SetupPopups()
    {
      base.SetupPopups();
      this.KeybindPopup = ElementFactory.CreateHeadedPanel<KeybindPopup>(((Component) this).transform).GetComponent<KeybindPopup>();
      this.ColorPickPopup = ElementFactory.CreateHeadedPanel<ColorPickPopup>(((Component) this).transform).GetComponent<ColorPickPopup>();
      this.SetNamePopup = ElementFactory.CreateHeadedPanel<SetNamePopup>(((Component) this).transform).GetComponent<SetNamePopup>();
      this.ImportPopup = ElementFactory.CreateHeadedPanel<ImportPopup>(((Component) this).transform).GetComponent<ImportPopup>();
      this.ExportPopup = ElementFactory.CreateHeadedPanel<ExportPopup>(((Component) this).transform).GetComponent<ExportPopup>();
      this.EditWeatherSchedulePopup = ElementFactory.CreateHeadedPanel<EditWeatherSchedulePopup>(((Component) this).transform).GetComponent<EditWeatherSchedulePopup>();
      this._popups.Add((BasePopup) this.KeybindPopup);
      this._popups.Add((BasePopup) this.ColorPickPopup);
      this._popups.Add((BasePopup) this.SetNamePopup);
      this._popups.Add((BasePopup) this.ImportPopup);
      this._popups.Add((BasePopup) this.ExportPopup);
      this._popups.Add((BasePopup) this.EditWeatherSchedulePopup);
    }

    private void SetupSettingsList()
    {
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.GeneralSettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.GraphicsSettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.UISettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.InputSettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.CustomSkinSettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.AbilitySettings);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.LegacyGameSettingsUI);
      this._saveableSettings.Add((SaveableSettingsContainer) SettingsManager.WeatherSettings);
      this._ignoreDefaultButtonSettings.Add((BaseSettingsContainer) SettingsManager.CustomSkinSettings);
      this._ignoreDefaultButtonSettings.Add((BaseSettingsContainer) SettingsManager.WeatherSettings);
    }

    private void SetupBottomButtons()
    {
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      string[] strArray = new string[5]
      {
        "Default",
        "Load",
        "Save",
        "Continue",
        "Quit"
      };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SettingsPopup.\u003C\u003Ec__DisplayClass26_0 cDisplayClass260 = new SettingsPopup.\u003C\u003Ec__DisplayClass26_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass260.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass260.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon(cDisplayClass260.buttonName), onClick: new UnityAction((object) cDisplayClass260, __methodptr(\u003CSetupBottomButtons\u003Eb__0)));
      }
    }

    private void OnConfirmSetDefault()
    {
      foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
      {
        if (!this._ignoreDefaultButtonSettings.Contains((BaseSettingsContainer) saveableSetting))
        {
          saveableSetting.SetDefault();
          saveableSetting.Save();
        }
      }
      this.RebuildCategoryPanel();
      UIManager.CurrentMenu.MessagePopup.Show("Settings reset to default.");
    }

    private void OnBottomBarButtonClick(string name)
    {
      switch (name)
      {
        case "Save":
          foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
            saveableSetting.Save();
          if (Application.loadedLevel == 0)
          {
            this.Hide();
            break;
          }
          GameMenu.TogglePause(false);
          break;
        case "Load":
          foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
            saveableSetting.Load();
          this.RebuildCategoryPanel();
          UIManager.CurrentMenu.MessagePopup.Show("Settings loaded from file.");
          break;
        case "Continue":
          if (Application.loadedLevel == 0)
          {
            this.Hide();
            break;
          }
          GameMenu.TogglePause(false);
          break;
        case "Default":
          // ISSUE: method pointer
          UIManager.CurrentMenu.ConfirmPopup.Show("Are you sure you want to reset to default?", new UnityAction((object) this, __methodptr(\u003COnBottomBarButtonClick\u003Eb__28_0)), "Reset default");
          break;
        case "Quit":
          foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
            saveableSetting.Load();
          if (Application.loadedLevel == 0)
          {
            Application.Quit();
            break;
          }
          GameMenu.TogglePause(false);
          if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();
          IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
          FengGameManagerMKII.instance.gameStart = false;
          FengGameManagerMKII.instance.DestroyAllExistingCloths();
          Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
          Application.LoadLevel("menu");
          break;
      }
    }

    public override void Hide()
    {
      if (((Component) this).gameObject.activeSelf)
      {
        foreach (BaseSettingsContainer saveableSetting in this._saveableSettings)
          saveableSetting.Apply();
      }
      base.Hide();
    }
  }
}
