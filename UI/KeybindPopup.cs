// Decompiled with JetBrains decompiler
// Type: UI.KeybindPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class KeybindPopup : PromptPopup
  {
    private InputKey _setting;
    private Text _settingLabel;
    private Text _displayLabel;
    private InputKey _buffer;
    private bool _isDone;

    protected override string Title => UIManager.GetLocale("SettingsPopup", nameof (KeybindPopup), nameof (Title));

    protected override float Width => 300f;

    protected override float Height => 250f;

    protected override float VerticalSpacing => 15f;

    protected override int VerticalPadding => 30;

    protected override TextAnchor PanelAlignment => (TextAnchor) 4;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocale("SettingsPopup", nameof (KeybindPopup), "Unbind"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__17_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__17_1)));
      ElementFactory.CreateDefaultLabel(this.SinglePanel, style, UIManager.GetLocale("SettingsPopup", nameof (KeybindPopup), "CurrentKey") + ":").GetComponent<Text>();
      this._displayLabel = ElementFactory.CreateDefaultLabel(this.SinglePanel, style, string.Empty).GetComponent<Text>();
      this._buffer = new InputKey();
    }

    private void Update()
    {
      if (this._setting == null || this._isDone || !this._buffer.ReadNextInput())
        return;
      this._isDone = true;
      if (this._buffer.ToString() == "Mouse0")
        this.StartCoroutine(this.WaitAndUpdateSetting());
      else
        this.UpdateSetting();
    }

    private IEnumerator WaitAndUpdateSetting()
    {
      yield return (object) new WaitForEndOfFrame();
      this.UpdateSetting();
    }

    private void UpdateSetting()
    {
      this._setting.LoadFromString(this._buffer.ToString());
      this._settingLabel.text = this._setting.ToString();
      ((Component) this).gameObject.SetActive(false);
    }

    public void Show(InputKey setting, Text label)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._setting = setting;
      this._settingLabel = label;
      this._displayLabel.text = this._setting.ToString();
      this._isDone = false;
    }

    private void OnButtonClick(string name)
    {
      if (name == "Unbind")
      {
        InputKey setting = this._setting;
        SpecialKey specialKey = SpecialKey.None;
        string serializedKey = specialKey.ToString();
        setting.LoadFromString(serializedKey);
        Text settingLabel = this._settingLabel;
        specialKey = SpecialKey.None;
        string str = specialKey.ToString();
        settingLabel.text = str;
      }
      this._isDone = true;
      this.Hide();
    }
  }
}
