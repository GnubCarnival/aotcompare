// Decompiled with JetBrains decompiler
// Type: UI.ImportPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI
{
  internal class ImportPopup : PromptPopup
  {
    private UnityAction _onSave;
    private InputSettingElement _element;
    private Text _text;
    public StringSetting ImportSetting = new StringSetting(string.Empty);

    protected override string Title => UIManager.GetLocaleCommon("Import");

    protected override float Width => 500f;

    protected override float Height => 600f;

    protected override int VerticalPadding => 20;

    protected override int HorizontalPadding => 20;

    protected override float VerticalSpacing => 10f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__16_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__16_1)));
      this._element = ElementFactory.CreateInputSetting(this.SinglePanel, style, (BaseSetting) this.ImportSetting, string.Empty, elementWidth: 460f, elementHeight: 390f, multiLine: true).GetComponent<InputSettingElement>();
      this._text = ElementFactory.CreateDefaultLabel(this.SinglePanel, style, "").GetComponent<Text>();
      ((Graphic) this._text).color = Color.red;
    }

    public void Show(UnityAction onSave)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._onSave = onSave;
      this.ImportSetting.Value = string.Empty;
      this._text.text = string.Empty;
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
          string str = new WeatherSchedule().DeserializeFromCSV(this.ImportSetting.Value);
          if (str != string.Empty)
          {
            this._text.text = str;
            break;
          }
          this._onSave.Invoke();
          this.Hide();
          break;
      }
    }
  }
}
