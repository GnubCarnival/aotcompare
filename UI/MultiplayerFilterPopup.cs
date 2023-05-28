// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerFilterPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class MultiplayerFilterPopup : PromptPopup
  {
    protected override string Title => UIManager.GetLocaleCommon("Filters");

    protected override int VerticalPadding => 20;

    protected override int HorizontalPadding => 20;

    protected override float VerticalSpacing => 20f;

    protected override float Width => 370f;

    protected override float Height => 250f;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      MultiplayerRoomListPopup multiplayerRoomListPopup = (MultiplayerRoomListPopup) parent;
      string category = "MainMenu";
      string subCategory = nameof (MultiplayerFilterPopup);
      ElementStyle style1 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(titleWidth: 240f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style1, UIManager.GetLocaleCommon("Confirm"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__14_0)));
      ElementFactory.CreateToggleSetting(this.SinglePanel, style2, (BaseSetting) multiplayerRoomListPopup._filterShowFull, UIManager.GetLocale(category, subCategory, "ShowFull"));
      ElementFactory.CreateToggleSetting(this.SinglePanel, style2, (BaseSetting) multiplayerRoomListPopup._filterShowPassword, UIManager.GetLocale(category, subCategory, "ShowPassword"));
    }

    protected void OnButtonClick(string name)
    {
      if (!(name == "Confirm"))
        return;
      this.Hide();
      ((MultiplayerRoomListPopup) this.Parent).RefreshList();
    }
  }
}
