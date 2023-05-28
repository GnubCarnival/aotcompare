// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerMapPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class MultiplayerMapPopup : BasePopup
  {
    protected MultiplayerSettingsPopup _multiplayerSettingsPopup;
    protected MultiplayerLanPopup _lanPopup;

    protected override string ThemePanel => nameof (MultiplayerMapPopup);

    protected override int HorizontalPadding => 0;

    protected override int VerticalPadding => 0;

    protected override float VerticalSpacing => 0.0f;

    protected override string Title => UIManager.GetLocale("MainMenu", nameof (MultiplayerMapPopup), nameof (Title));

    protected override bool HasPremadeContent => true;

    protected override float Width => 900f;

    protected override float Height => 560f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      GameObject gameObject = ((Component) this.SinglePanel.Find("MultiplayerMap")).gameObject;
      foreach (Button componentsInChild in gameObject.GetComponentsInChildren<Button>())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MultiplayerMapPopup.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180 = new MultiplayerMapPopup.\u003C\u003Ec__DisplayClass18_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass180.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass180.button = componentsInChild;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) cDisplayClass180.button.onClick).AddListener(new UnityAction((object) cDisplayClass180, __methodptr(\u003CSetup\u003Eb__4)));
        // ISSUE: reference to a compiler-generated field
        ((Selectable) ((Component) cDisplayClass180.button).GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
        // ISSUE: reference to a compiler-generated field
        ((Graphic) ((Component) ((Component) cDisplayClass180.button).transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
      }
      string category = "MainMenu";
      string subCategory = nameof (MultiplayerMapPopup);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, "LAN", onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocale(category, subCategory, "ButtonOffline"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_1)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocale(category, subCategory, "ButtonServer"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_2)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_3)));
      ((Graphic) gameObject.GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "MapColor");
    }

    protected override void SetupPopups()
    {
      base.SetupPopups();
      this._multiplayerSettingsPopup = ElementFactory.CreateHeadedPanel<MultiplayerSettingsPopup>(((Component) this).transform).GetComponent<MultiplayerSettingsPopup>();
      this._lanPopup = ElementFactory.CreateHeadedPanel<MultiplayerLanPopup>(((Component) this).transform).GetComponent<MultiplayerLanPopup>();
      this._popups.Add((BasePopup) this._multiplayerSettingsPopup);
      this._popups.Add((BasePopup) this._lanPopup);
    }

    private void OnButtonClick(string name)
    {
      this.HideAllPopups();
      MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
      {
        case 1836253938:
          if (!(name == "Server"))
            break;
          this._multiplayerSettingsPopup.Show();
          break;
        case 2256991139:
          if (!(name == "ButtonASIA"))
            break;
          multiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
          break;
        case 2570753840:
          if (!(name == "LAN"))
            break;
          this._lanPopup.Show();
          break;
        case 2686248205:
          if (!(name == "ButtonUS"))
            break;
          multiplayerSettings.ConnectServer(MultiplayerRegion.US);
          break;
        case 3256289893:
          if (!(name == "ButtonSA"))
            break;
          multiplayerSettings.ConnectServer(MultiplayerRegion.SA);
          break;
        case 3264564162:
          if (!(name == "Back"))
            break;
          this.Hide();
          break;
        case 3296475080:
          if (!(name == "Offline"))
            break;
          multiplayerSettings.ConnectOffline();
          break;
        case 3661909963:
          if (!(name == "ButtonEU"))
            break;
          multiplayerSettings.ConnectServer(MultiplayerRegion.EU);
          break;
      }
    }
  }
}
