// Decompiled with JetBrains decompiler
// Type: UI.SliderInputSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class SliderInputSettingElement : BaseSettingElement
  {
    protected Slider _slider;
    protected InputField _inputField;
    protected int _inputFontSizeOffset = -4;
    protected NumberFormatInfo _formatInfo;
    protected bool _fixedInputField;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Float,
      SettingType.Int
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string tooltip,
      float sliderWidth,
      float sliderHeight,
      float inputWidth,
      float inputHeight,
      int decimalPlaces)
    {
      this._formatInfo = new NumberFormatInfo();
      this._formatInfo.NumberDecimalDigits = decimalPlaces;
      this._slider = ((Component) ((Component) this).transform.Find("Slider")).GetComponent<Slider>();
      this._settingType = this.GetSettingType(setting);
      if (this._settingType == SettingType.Int)
      {
        this._slider.wholeNumbers = true;
        this._slider.minValue = (float) ((IntSetting) setting).MinValue;
        this._slider.maxValue = (float) ((IntSetting) setting).MaxValue;
      }
      else if (this._settingType == SettingType.Float)
      {
        this._slider.wholeNumbers = false;
        this._slider.minValue = ((FloatSetting) setting).MinValue;
        this._slider.maxValue = ((FloatSetting) setting).MaxValue;
      }
      ((Component) this._slider).GetComponent<LayoutElement>().preferredWidth = sliderWidth;
      ((Component) this._slider).GetComponent<LayoutElement>().preferredHeight = sliderHeight;
      // ISSUE: method pointer
      ((UnityEvent<float>) this._slider.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CSetup\u003Eb__7_0)));
      this._inputField = ((Component) ((Component) this).transform.Find("InputField")).GetComponent<InputField>();
      ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>().fontSize = style.FontSize + this._inputFontSizeOffset;
      ((Component) this._inputField).GetComponent<LayoutElement>().preferredWidth = inputWidth;
      ((Component) this._inputField).GetComponent<LayoutElement>().preferredHeight = inputHeight;
      this._settingType = this.GetSettingType(setting);
      if (this._settingType == SettingType.Float)
        this._inputField.contentType = (InputField.ContentType) 3;
      else if (this._settingType == SettingType.Int)
        this._inputField.contentType = (InputField.ContentType) 2;
      // ISSUE: method pointer
      ((UnityEvent<string>) this._inputField.onValueChange).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CSetup\u003Eb__7_1)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this._inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CSetup\u003Eb__7_2)));
      this.Setup(setting, style, title, tooltip);
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderBackgroundColor");
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Fill Area/Fill")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderFillColor");
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Handle Slide Area/Handle")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderHandleColor");
      ((Selectable) this._inputField).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Input");
      ((Graphic) ((Component) ((Component) this._inputField).transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
      this._inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputSelectionColor");
      this.StartCoroutine(this.WaitAndFixInputField());
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this._inputField, (Object) null) || this._fixedInputField)
        return;
      ((Component) this._inputField).gameObject.SetActive(false);
      ((Component) this._inputField).gameObject.SetActive(true);
    }

    private IEnumerator WaitAndFixInputField()
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) new WaitForEndOfFrame();
      ((Component) this._inputField).gameObject.SetActive(false);
      ((Component) this._inputField).gameObject.SetActive(true);
      this._fixedInputField = true;
    }

    protected void OnSliderValueChanged(float value)
    {
      if (this._settingType == SettingType.Float)
        ((TypedSetting<float>) this._setting).Value = value;
      else if (this._settingType == SettingType.Int)
        ((TypedSetting<int>) this._setting).Value = (int) value;
      this.SyncInput();
    }

    protected void OnInputValueChanged(string value)
    {
      if (value == string.Empty)
        return;
      if (this._settingType == SettingType.Float)
      {
        float result;
        if (!float.TryParse(value, out result))
          return;
        ((TypedSetting<float>) this._setting).Value = Mathf.Clamp(result, this._slider.minValue, this._slider.maxValue);
      }
      else
      {
        int result;
        if (this._settingType != SettingType.Int || !int.TryParse(value, out result))
          return;
        ((TypedSetting<int>) this._setting).Value = (int) Mathf.Clamp((float) result, this._slider.minValue, this._slider.maxValue);
      }
    }

    protected void OnInputFinishEditing(string value) => this.SyncElement();

    protected void SyncSlider()
    {
      if (this._settingType == SettingType.Float)
      {
        this._slider.value = ((TypedSetting<float>) this._setting).Value;
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        this._slider.value = (float) ((TypedSetting<int>) this._setting).Value;
      }
    }

    protected void SyncInput()
    {
      if (this._settingType == SettingType.Float)
      {
        this._inputField.text = string.Format((IFormatProvider) this._formatInfo, "{0:N}", (object) ((TypedSetting<float>) this._setting).Value);
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        this._inputField.text = ((TypedSetting<int>) this._setting).Value.ToString();
      }
    }

    public override void SyncElement()
    {
      this.SyncSlider();
      this.SyncInput();
    }
  }
}
