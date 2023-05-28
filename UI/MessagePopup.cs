// Decompiled with JetBrains decompiler
// Type: UI.MessagePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class MessagePopup : PromptPopup
  {
    protected float LabelHeight = 60f;
    private Text _label;

    protected override string Title => string.Empty;

    protected override float Width => 300f;

    protected override float Height => 240f;

    protected override int VerticalPadding => 30;

    protected override int HorizontalPadding => 30;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel), UIManager.GetLocaleCommon("Okay"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__14_0)));
      this._label = ElementFactory.CreateDefaultLabel(this.SinglePanel, style, string.Empty).GetComponent<Text>();
      ((Component) this._label).GetComponent<LayoutElement>().preferredHeight = this.LabelHeight;
      ((Component) this._label).GetComponent<LayoutElement>().preferredWidth = this.Width - (float) (this.HorizontalPadding * 2);
    }

    public void Show(string message)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._label.text = message;
    }

    private void OnButtonClick(string name) => this.Hide();
  }
}
