// Decompiled with JetBrains decompiler
// Type: UI.ExportPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class ExportPopup : PromptPopup
  {
    private InputSettingElement _element;
    public StringSetting ExportSetting = new StringSetting(string.Empty);

    protected override string Title => UIManager.GetLocaleCommon("Export");

    protected override float Width => 500f;

    protected override float Height => 600f;

    protected override int VerticalPadding => 20;

    protected override int HorizontalPadding => 20;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Done"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__12_0)));
      this._element = ElementFactory.CreateInputSetting(this.SinglePanel, style, (BaseSetting) this.ExportSetting, string.Empty, elementWidth: 460f, elementHeight: 440f, multiLine: true).GetComponent<InputSettingElement>();
    }

    public void Show(string value)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this.ExportSetting.Value = value;
      this._element.SyncElement();
    }

    private void OnButtonClick(string name)
    {
      if (!(name == "Done"))
        return;
      this.Hide();
    }
  }
}
