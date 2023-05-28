// Decompiled with JetBrains decompiler
// Type: UI.ConfirmPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ConfirmPopup : PromptPopup
  {
    protected float LabelHeight = 60f;
    private Text _label;
    private UnityAction _onConfirm;

    protected override string Title => UIManager.GetLocaleCommon("Confirm");

    protected override float Width => 300f;

    protected override float Height => 240f;

    protected override int VerticalPadding => 30;

    protected override int HorizontalPadding => 30;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style1 = new ElementStyle(themePanel: this.ThemePanel);
      ElementStyle style2 = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style2, UIManager.GetLocaleCommon("Confirm"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__15_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style2, UIManager.GetLocaleCommon("Cancel"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__15_1)));
      this._label = ElementFactory.CreateDefaultLabel(this.SinglePanel, style1, string.Empty).GetComponent<Text>();
      ((Component) this._label).GetComponent<LayoutElement>().preferredWidth = this.Width - (float) (this.HorizontalPadding * 2);
      ((Component) this._label).GetComponent<LayoutElement>().preferredHeight = this.LabelHeight;
    }

    public void Show(string message, UnityAction onConfirm, string title = null)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._label.text = message;
      this._onConfirm = onConfirm;
      if (title != null)
        this.SetTitle(title);
      else
        this.SetTitle(this.Title);
    }

    private void OnButtonClick(string name)
    {
      if (name == "Confirm")
        this._onConfirm.Invoke();
      this.Hide();
    }
  }
}
