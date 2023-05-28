// Decompiled with JetBrains decompiler
// Type: UI.InputSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class InputSettingElement : BaseSettingElement
  {
    protected InputField _inputField;
    protected int _inputFontSizeOffset = -4;
    protected bool _fixedInputField;
    protected UnityAction _onValueChanged;
    protected UnityAction _onEndEdit;
    protected Transform _caret;
    protected bool _finishedSetup;
    protected object[] _setupParams;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Float,
      SettingType.Int,
      SettingType.String
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string tooltip,
      float elementWidth,
      float elementHeight,
      bool multiLine,
      UnityAction onValueChanged,
      UnityAction onEndEdit)
    {
      this._onValueChanged = onValueChanged;
      this._onEndEdit = onEndEdit;
      this._inputField = ((Component) ((Component) this).transform.Find("InputField")).gameObject.GetComponent<InputField>();
      if (Object.op_Equality((Object) this._inputField, (Object) null))
      {
        this._inputField = (InputField) ((Component) ((Component) this).transform.Find("InputField")).gameObject.AddComponent<InputFieldPasteable>();
        this._inputField.textComponent = ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>();
        ((Selectable) this._inputField).transition = (Selectable.Transition) 1;
        ((Selectable) this._inputField).targetGraphic = (Graphic) ((Component) this._inputField).GetComponent<Image>();
        this._inputField.text = "Default";
        this._inputField.text = string.Empty;
      }
      ((Selectable) this._inputField).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Input");
      ((Graphic) ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
      this._inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputSelectionColor");
      ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>().fontSize = style.FontSize + this._inputFontSizeOffset;
      ((Graphic) ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
      ((Component) this._inputField).GetComponent<LayoutElement>().preferredWidth = elementWidth;
      ((Component) this._inputField).GetComponent<LayoutElement>().preferredHeight = elementHeight;
      this._inputField.lineType = multiLine ? (InputField.LineType) 2 : (InputField.LineType) 0;
      this._settingType = this.GetSettingType(setting);
      if (this._settingType == SettingType.Float)
      {
        this._inputField.contentType = (InputField.ContentType) 3;
        this._inputField.characterLimit = 20;
      }
      else if (this._settingType == SettingType.Int)
      {
        this._inputField.contentType = (InputField.ContentType) 2;
        this._inputField.characterLimit = 10;
      }
      else if (this._settingType == SettingType.String)
      {
        this._inputField.contentType = (InputField.ContentType) 0;
        int maxLength = ((StringSetting) setting).MaxLength;
        this._inputField.characterLimit = maxLength != int.MaxValue ? maxLength : 0;
      }
      // ISSUE: method pointer
      ((UnityEvent<string>) this._inputField.onValueChange).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CSetup\u003Eb__10_0)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this._inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CSetup\u003Eb__10_1)));
      if (multiLine)
      {
        this._setupParams = new object[4]
        {
          (object) setting,
          (object) style,
          (object) title,
          (object) tooltip
        };
        this.StartCoroutine(this.WaitAndFinishSetup());
      }
      else
      {
        this.Setup(setting, style, title, tooltip);
        this.StartCoroutine(this.WaitAndFixInputField());
        this._finishedSetup = true;
      }
    }

    private void OnEnable()
    {
      if (Object.op_Inequality((Object) this._inputField, (Object) null) && !this._finishedSetup)
      {
        this.StartCoroutine(this.WaitAndFinishSetup());
      }
      else
      {
        if (!Object.op_Inequality((Object) this._inputField, (Object) null) || this._fixedInputField)
          return;
        this.StartCoroutine(this.WaitAndFixInputField());
      }
    }

    private IEnumerator WaitAndFinishSetup()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      InputSettingElement inputSettingElement = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        // ISSUE: reference to a compiler-generated method
        inputSettingElement.\u003C\u003En__0((BaseSetting) inputSettingElement._setupParams[0], (ElementStyle) inputSettingElement._setupParams[1], (string) inputSettingElement._setupParams[2], (string) inputSettingElement._setupParams[2]);
        inputSettingElement.StartCoroutine(inputSettingElement.WaitAndFixInputField());
        inputSettingElement._finishedSetup = true;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    private IEnumerator WaitAndFixInputField()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      InputSettingElement inputSettingElement = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        ((Component) inputSettingElement._inputField).gameObject.SetActive(false);
        ((Component) inputSettingElement._inputField).gameObject.SetActive(true);
        inputSettingElement.SyncElement();
        inputSettingElement._fixedInputField = true;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    protected void OnValueChanged(string value)
    {
      if (!this._finishedSetup)
        return;
      if (this._settingType == SettingType.String)
        ((TypedSetting<string>) this._setting).Value = this._inputField.text;
      else if (value != string.Empty)
      {
        if (this._settingType == SettingType.Float)
        {
          float result;
          if (float.TryParse(value, out result))
            ((TypedSetting<float>) this._setting).Value = result;
        }
        else
        {
          int result;
          if (this._settingType == SettingType.Int && int.TryParse(value, out result))
            ((TypedSetting<int>) this._setting).Value = result;
        }
      }
      if (this._onValueChanged == null)
        return;
      this._onValueChanged.Invoke();
    }

    protected void OnInputFinishEditing(string value)
    {
      if (!this._finishedSetup)
        return;
      this.SyncElement();
      if (this._onEndEdit == null)
        return;
      this._onEndEdit.Invoke();
    }

    public override void SyncElement()
    {
      if (!this._finishedSetup)
        return;
      if (this._settingType == SettingType.Float)
        this._inputField.text = ((TypedSetting<float>) this._setting).Value.ToString();
      else if (this._settingType == SettingType.Int)
        this._inputField.text = ((TypedSetting<int>) this._setting).Value.ToString();
      else if (this._settingType == SettingType.String)
        this._inputField.text = ((TypedSetting<string>) this._setting).Value;
      ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>().text = this._inputField.text;
    }

    private void Update()
    {
      if (Object.op_Implicit((Object) this._caret) || !Object.op_Inequality((Object) this._inputField, (Object) null))
        return;
      this._caret = ((Component) this._inputField).transform.Find(((Object) ((Component) this._inputField).transform).name + " Input Caret");
      if (!Object.op_Implicit((Object) this._caret) || Object.op_Implicit((Object) ((Component) this._caret).GetComponent<Graphic>()))
        return;
      ((Component) this._caret).gameObject.AddComponent<Image>();
    }
  }
}
