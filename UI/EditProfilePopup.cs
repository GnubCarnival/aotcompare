// Decompiled with JetBrains decompiler
// Type: UI.EditProfilePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class EditProfilePopup : BasePopup
  {
    protected override string Title => string.Empty;

    protected override float Width => 730f;

    protected override float Height => 660f;

    protected override bool CategoryPanel => true;

    protected override bool CategoryButtons => true;

    protected override string DefaultCategoryPanel => "Profile";

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      this.SetupBottomButtons();
    }

    protected override void SetupTopButtons()
    {
      ElementStyle style = new ElementStyle(28, themePanel: this.ThemePanel);
      string[] strArray = new string[2]
      {
        "Profile",
        "Stats"
      };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditProfilePopup.\u003C\u003Ec__DisplayClass13_0 cDisplayClass130 = new EditProfilePopup.\u003C\u003Ec__DisplayClass13_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass130.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass130.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        GameObject categoryButton = ElementFactory.CreateCategoryButton(this.TopBar, style, UIManager.GetLocaleCommon(cDisplayClass130.buttonName), new UnityAction((object) cDisplayClass130, __methodptr(\u003CSetupTopButtons\u003Eb__0)));
        // ISSUE: reference to a compiler-generated field
        this._topButtons.Add(cDisplayClass130.buttonName, categoryButton.GetComponent<Button>());
      }
      base.SetupTopButtons();
    }

    protected override void RegisterCategoryPanels()
    {
      this._categoryPanelTypes.Add("Profile", typeof (EditProfileProfilePanel));
      this._categoryPanelTypes.Add("Stats", typeof (EditProfileStatsPanel));
    }

    protected override void SetupPopups() => base.SetupPopups();

    private void SetupBottomButtons()
    {
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      string[] strArray = new string[1]{ "Save" };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditProfilePopup.\u003C\u003Ec__DisplayClass16_0 cDisplayClass160 = new EditProfilePopup.\u003C\u003Ec__DisplayClass16_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass160.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass160.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon(cDisplayClass160.buttonName), onClick: new UnityAction((object) cDisplayClass160, __methodptr(\u003CSetupBottomButtons\u003Eb__0)));
      }
    }

    private void OnBottomBarButtonClick(string name)
    {
      switch (name)
      {
        case "Save":
          SettingsManager.ProfileSettings.Save();
          this.Hide();
          break;
      }
    }
  }
}
