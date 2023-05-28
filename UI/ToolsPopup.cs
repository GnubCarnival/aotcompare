// Decompiled with JetBrains decompiler
// Type: UI.ToolsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class ToolsPopup : BasePopup
  {
    protected override string Title => UIManager.GetLocale("MainMenu", nameof (ToolsPopup), nameof (Title));

    protected override float Width => 280f;

    protected override float Height => 355f;

    protected override float VerticalSpacing => 20f;

    protected override int VerticalPadding => 20;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string category = "MainMenu";
      string subCategory = nameof (ToolsPopup);
      float elementWidth = 210f;
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonMapEditor"), elementWidth, onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_1)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonCharacterEditor"), elementWidth, onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_2)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonSnapshotViewer"), elementWidth, onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__10_3)));
    }

    protected void OnButtonClick(string name)
    {
      switch (name)
      {
        case "MapEditor":
          FengGameManagerMKII.settingsOld[64] = (object) 101;
          Application.LoadLevel(2);
          break;
        case "CharacterEditor":
          Application.LoadLevel("characterCreation");
          break;
        case "SnapshotViewer":
          Application.LoadLevel("SnapShot");
          break;
        case "Back":
          this.Hide();
          break;
      }
    }
  }
}
