// Decompiled with JetBrains decompiler
// Type: UI.SetNamePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
  internal class SetNamePopup : PromptPopup
  {
    private UnityAction _onSave;
    private InputSettingElement _element;
    public StringSetting NameSetting = new StringSetting(string.Empty);
    private string _initialValue;

    protected override string Title => string.Empty;

    protected override float Width => 320f;

    protected override float Height => 240f;

    protected override int VerticalPadding => 40;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, 100f, this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__12_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__12_1)));
      this._element = ElementFactory.CreateInputSetting(this.SinglePanel, style, (BaseSetting) this.NameSetting, UIManager.GetLocaleCommon("SetName"), elementWidth: 120f).GetComponent<InputSettingElement>();
    }

    public void Show(string initialValue, UnityAction onSave, string title)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._initialValue = initialValue;
      this._onSave = onSave;
      this.SetTitle(title);
      this.NameSetting.Value = initialValue;
      this._element.SyncElement();
    }

    private void OnButtonClick(string name)
    {
      switch (name)
      {
        case "Cancel":
          this.Hide();
          break;
        case "Save":
          if (this.NameSetting.Value == string.Empty)
            this.NameSetting.Value = this._initialValue;
          this._onSave.Invoke();
          this.Hide();
          break;
      }
    }
  }
}
