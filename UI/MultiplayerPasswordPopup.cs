// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerPasswordPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class MultiplayerPasswordPopup : PromptPopup
  {
    protected StringSetting _enteredPassword = new StringSetting(string.Empty);
    protected string _actualPassword;
    protected string _roomName;
    protected GameObject _incorrectPasswordLabel;

    protected override string Title => UIManager.GetLocaleCommon("Password");

    protected override int VerticalPadding => 10;

    protected override int HorizontalPadding => 20;

    protected override float VerticalSpacing => 10f;

    protected override float Width => 300f;

    protected override float Height => 250f;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      float elementWidth = 200f;
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(20, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Confirm"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__18_1)));
      ElementFactory.CreateDefaultLabel(this.SinglePanel, style2, string.Empty);
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) this._enteredPassword, string.Empty, elementWidth: elementWidth);
      this._incorrectPasswordLabel = ElementFactory.CreateDefaultLabel(this.SinglePanel, style2, UIManager.GetLocale("MainMenu", nameof (MultiplayerPasswordPopup), "IncorrectPassword"));
      ((Graphic) this._incorrectPasswordLabel.GetComponent<Text>()).color = Color.red;
    }

    public void Show(string actualPassword, string roomName)
    {
      this._actualPassword = actualPassword;
      this._roomName = roomName;
      this._incorrectPasswordLabel.SetActive(false);
      this.Show();
    }

    protected void OnButtonClick(string name)
    {
      switch (name)
      {
        case "Confirm":
          try
          {
            if (this._enteredPassword.Value == new SimpleAES().Decrypt(this._actualPassword))
            {
              PhotonNetwork.JoinRoom(this._roomName);
              this.Hide();
              break;
            }
            this._incorrectPasswordLabel.SetActive(true);
            break;
          }
          catch
          {
            this._incorrectPasswordLabel.SetActive(true);
            break;
          }
        case "Back":
          this.Hide();
          break;
      }
    }
  }
}
