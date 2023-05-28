// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerLanPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class MultiplayerLanPopup : PromptPopup
  {
    protected override string Title => "LAN";

    protected override float Width => 400f;

    protected override float Height => 370f;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string category = "MainMenu";
      string subCategory = nameof (MultiplayerLanPopup);
      MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
      float elementWidth = 200f;
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocale(category, subCategory, "Connect"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__8_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__8_1)));
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.LanIP, "IP", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.LanPort, "Port", elementWidth: elementWidth);
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.LanPassword, "Password (optional)", elementWidth: elementWidth);
    }

    protected void OnButtonClick(string name)
    {
      switch (name)
      {
        case "Connect":
          SettingsManager.MultiplayerSettings.ConnectLAN();
          break;
        case "Back":
          this.Hide();
          break;
      }
    }
  }
}
