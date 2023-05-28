// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerSettingsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class MultiplayerSettingsPopup : PromptPopup
  {
    protected override string Title => UIManager.GetLocale("MainMenu", nameof (MultiplayerSettingsPopup), nameof (Title));

    protected override float Width => 480f;

    protected override float Height => 550f;

    protected override bool DoublePanel => false;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string category = "MainMenu";
      string subCategory = nameof (MultiplayerSettingsPopup);
      MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
      float elementWidth = 180f;
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(titleWidth: 160f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Save"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_0)));
      ElementFactory.CreateToggleGroupSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.LobbyMode, UIManager.GetLocale(category, subCategory, "Lobby"), UIManager.GetLocaleArray(category, subCategory, "LobbyOptions"), UIManager.GetLocale(category, subCategory, "LobbyTooltip"));
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.CustomAppId, UIManager.GetLocale(category, subCategory, "LobbyCustom"), elementWidth: elementWidth);
      this.CreateHorizontalDivider(this.SinglePanel);
      ElementFactory.CreateToggleGroupSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.AppIdMode, UIManager.GetLocale(category, subCategory, "AppId"), UIManager.GetLocaleArray(category, subCategory, "AppIdOptions"), UIManager.GetLocale(category, subCategory, "AppIdTooltip"));
      ElementFactory.CreateInputSetting(this.SinglePanel, style2, (BaseSetting) multiplayerSettings.CustomAppId, UIManager.GetLocale(category, subCategory, "AppIdCustom"), elementWidth: elementWidth);
    }

    protected void OnSaveButtonClick()
    {
      SettingsManager.MultiplayerSettings.Save();
      this.Hide();
    }
  }
}
