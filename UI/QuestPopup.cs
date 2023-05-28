// Decompiled with JetBrains decompiler
// Type: UI.QuestPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class QuestPopup : BasePopup
  {
    public StringSetting TierSelection = new StringSetting("Bronze");
    public StringSetting CompletedSelection = new StringSetting("In Progress");

    protected override string Title => string.Empty;

    protected override float Width => 990f;

    protected override float Height => 740f;

    protected override bool CategoryPanel => true;

    protected override bool CategoryButtons => true;

    protected override string DefaultCategoryPanel => "Daily";

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      this.SetupBottomButtons();
    }

    public void CreateAchievmentDropdowns(Transform panel)
    {
      ElementStyle style = new ElementStyle(titleWidth: 0.0f, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panel, style, (BaseSetting) this.TierSelection, "", new string[3]
      {
        "Bronze",
        "Silver",
        "Gold"
      }, elementWidth: 180f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateAchievmentDropdowns\u003Eb__15_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDropdownSetting(panel, style, (BaseSetting) this.CompletedSelection, "", new string[2]
      {
        "In Progress",
        "Completed"
      }, elementWidth: 180f, onDropdownOptionSelect: new UnityAction((object) this, __methodptr(\u003CCreateAchievmentDropdowns\u003Eb__15_1)));
    }

    protected override void SetupTopButtons()
    {
      ElementStyle style = new ElementStyle(28, themePanel: this.ThemePanel);
      string[] strArray = new string[3]
      {
        "Daily",
        "Weekly",
        "Achievments"
      };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        QuestPopup.\u003C\u003Ec__DisplayClass16_0 cDisplayClass160 = new QuestPopup.\u003C\u003Ec__DisplayClass16_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass160.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass160.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string title = cDisplayClass160.buttonName == "Daily" || cDisplayClass160.buttonName == "Weekly" ? UIManager.GetLocale("MainMenu", "QuestsPopup", cDisplayClass160.buttonName) : UIManager.GetLocaleCommon(cDisplayClass160.buttonName);
        // ISSUE: method pointer
        GameObject categoryButton = ElementFactory.CreateCategoryButton(this.TopBar, style, title, new UnityAction((object) cDisplayClass160, __methodptr(\u003CSetupTopButtons\u003Eb__0)));
        // ISSUE: reference to a compiler-generated field
        this._topButtons.Add(cDisplayClass160.buttonName, categoryButton.GetComponent<Button>());
      }
      base.SetupTopButtons();
    }

    protected override void RegisterCategoryPanels()
    {
      this._categoryPanelTypes.Add("Daily", typeof (QuestDailyPanel));
      this._categoryPanelTypes.Add("Weekly", typeof (QuestWeeklyPanel));
      this._categoryPanelTypes.Add("Achievments", typeof (QuestAchievmentsPanel));
    }

    protected override void SetupPopups() => base.SetupPopups();

    private void SetupBottomButtons()
    {
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      string[] strArray = new string[1]{ "Back" };
      foreach (string str in strArray)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        QuestPopup.\u003C\u003Ec__DisplayClass19_0 cDisplayClass190 = new QuestPopup.\u003C\u003Ec__DisplayClass19_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass190.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass190.buttonName = str;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon(cDisplayClass190.buttonName), onClick: new UnityAction((object) cDisplayClass190, __methodptr(\u003CSetupBottomButtons\u003Eb__0)));
      }
    }

    private void OnBottomBarButtonClick(string name)
    {
      switch (name)
      {
        case "Back":
          this.Hide();
          break;
      }
    }
  }
}
