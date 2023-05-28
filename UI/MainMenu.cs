// Decompiled with JetBrains decompiler
// Type: UI.MainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class MainMenu : BaseMenu
  {
    public BasePopup _singleplayerPopup;
    public BasePopup _multiplayerMapPopup;
    public BasePopup _settingsPopup;
    public BasePopup _toolsPopup;
    public BasePopup _multiplayerRoomListPopup;
    public BasePopup _editProfilePopup;
    public BasePopup _questsPopup;
    protected Text _multiplayerStatusLabel;

    public override void Setup()
    {
      base.Setup();
      if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
        ElementFactory.InstantiateAndBind(((Component) this).transform, "MainBackground").AddComponent<IgnoreScaler>();
      this.SetupIntroPanel();
      this.SetupLabels();
    }

    public void ShowMultiplayerRoomListPopup()
    {
      this.HideAllPopups();
      this._multiplayerRoomListPopup.Show();
    }

    public void ShowMultiplayerMapPopup()
    {
      this.HideAllPopups();
      this._multiplayerMapPopup.Show();
    }

    protected override void SetupPopups()
    {
      base.SetupPopups();
      this._singleplayerPopup = ElementFactory.CreateHeadedPanel<SingleplayerPopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._multiplayerMapPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerMapPopup>(((Component) this).transform, "MultiplayerMapPopup").GetComponent<BasePopup>();
      this._editProfilePopup = ElementFactory.CreateHeadedPanel<EditProfilePopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._toolsPopup = ElementFactory.CreateHeadedPanel<ToolsPopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._multiplayerRoomListPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerRoomListPopup>(((Component) this).transform, "MultiplayerRoomListPopup").GetComponent<BasePopup>();
      this._questsPopup = ElementFactory.CreateHeadedPanel<QuestPopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._popups.Add(this._singleplayerPopup);
      this._popups.Add(this._multiplayerMapPopup);
      this._popups.Add(this._editProfilePopup);
      this._popups.Add(this._settingsPopup);
      this._popups.Add(this._toolsPopup);
      this._popups.Add(this._multiplayerRoomListPopup);
      this._popups.Add(this._questsPopup);
    }

    private void SetupIntroPanel()
    {
      GameObject gameObject = ElementFactory.InstantiateAndBind(((Component) this).transform, "IntroPanel");
      ElementFactory.SetAnchor(gameObject, (TextAnchor) 8, (TextAnchor) 8, new Vector2(-10f, 30f));
      foreach (Transform transform in gameObject.transform.Find("Buttons"))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainMenu.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120 = new MainMenu.\u003C\u003Ec__DisplayClass12_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.introButton = ((Component) transform).gameObject.AddComponent<IntroButton>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) cDisplayClass120.introButton.onClick).AddListener(new UnityAction((object) cDisplayClass120, __methodptr(\u003CSetupIntroPanel\u003Eb__0)));
      }
    }

    private void SetupLabels()
    {
      GameObject gameObject = ElementFactory.InstantiateAndBind(((Component) this).transform, "Aottg2DonateButton");
      ElementFactory.SetAnchor(gameObject, (TextAnchor) 2, (TextAnchor) 2, new Vector2(-20f, -20f));
      // ISSUE: method pointer
      ((UnityEvent) gameObject.GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetupLabels\u003Eb__13_0)));
      this._multiplayerStatusLabel = ElementFactory.CreateDefaultLabel(((Component) this).transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
      ElementFactory.SetAnchor(((Component) this._multiplayerStatusLabel).gameObject, (TextAnchor) 0, (TextAnchor) 0, new Vector2(20f, -20f));
      ((Graphic) this._multiplayerStatusLabel).color = Color.white;
      Text component = ElementFactory.CreateDefaultLabel(((Component) this).transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
      ElementFactory.SetAnchor(((Component) component).gameObject, (TextAnchor) 7, (TextAnchor) 7, new Vector2(0.0f, 20f));
      ((Graphic) component).color = Color.white;
      if (ApplicationConfig.DevelopmentMode)
        component.text = "RC MOD DEVELOPMENT VERSION";
      else
        component.text = "RC Mod Version 5/5/2022.";
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this._multiplayerStatusLabel, (Object) null))
        return;
      this._multiplayerStatusLabel.text = PhotonNetwork.connectionStateDetailed.ToString();
      if (!PhotonNetwork.connected)
        return;
      Text multiplayerStatusLabel = this._multiplayerStatusLabel;
      multiplayerStatusLabel.text = multiplayerStatusLabel.text + " ping:" + PhotonNetwork.GetPing().ToString();
    }

    private void OnIntroButtonClick(string name)
    {
      this.HideAllPopups();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
      {
        case 382306744:
          if (!(name == "ToolsButton"))
            break;
          this._toolsPopup.Show();
          break;
        case 537677866:
          if (!(name == "Donate"))
            break;
          Application.OpenURL("https://www.patreon.com/aottg2");
          break;
        case 779534544:
          if (!(name == "SingleplayerButton"))
            break;
          this._singleplayerPopup.Show();
          break;
        case 790829587:
          if (!(name == "MultiplayerButton"))
            break;
          this._multiplayerMapPopup.Show();
          break;
        case 1074483388:
          if (!(name == "SettingsButton"))
            break;
          this._settingsPopup.Show();
          break;
        case 2612973458:
          if (!(name == "QuestsButton"))
            break;
          this._questsPopup.Show();
          break;
        case 3831491030:
          if (!(name == "QuitButton"))
            break;
          Application.Quit();
          break;
        case 4279240974:
          if (!(name == "ProfileButton"))
            break;
          this._editProfilePopup.Show();
          break;
      }
    }
  }
}
