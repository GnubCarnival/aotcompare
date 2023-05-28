// Decompiled with JetBrains decompiler
// Type: UI.SliderSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class SliderSettingElement : BaseSettingElement
  {
    protected Slider _slider;
    protected Text _valueLabel;
    protected NumberFormatInfo _formatInfo;

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
      float elementWidth,
      float elementHeight,
      int decimalPlaces)
    {
      this._formatInfo = new NumberFormatInfo();
      this._formatInfo.NumberDecimalDigits = decimalPlaces;
      this._slider = ((Component) ((Component) this).transform.Find("Slider")).GetComponent<Slider>();
      this._valueLabel = ((Component) ((Component) this).transform.Find("Value")).GetComponent<Text>();
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
      ((Component) this._slider).GetComponent<LayoutElement>().preferredWidth = elementWidth;
      ((Component) this._slider).GetComponent<LayoutElement>().preferredHeight = elementHeight;
      // ISSUE: method pointer
      ((UnityEvent<float>) this._slider.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CSetup\u003Eb__5_0)));
      this._valueLabel.fontSize = style.FontSize;
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderBackgroundColor");
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Fill Area/Fill")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderFillColor");
      ((Graphic) ((Component) ((Component) this._slider).transform.Find("Handle Slide Area/Handle")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderHandleColor");
      ((Graphic) this._valueLabel).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
      this.Setup(setting, style, title, tooltip);
    }

    protected void OnValueChanged(float value)
    {
      if (this._settingType == SettingType.Float)
        ((TypedSetting<float>) this._setting).Value = value;
      else if (this._settingType == SettingType.Int)
        ((TypedSetting<int>) this._setting).Value = (int) value;
      this.UpdateValueLabel();
    }

    protected void UpdateValueLabel()
    {
      if (this._settingType == SettingType.Float)
      {
        this._valueLabel.text = string.Format((IFormatProvider) this._formatInfo, "{0:N}", (object) this._slider.value);
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        this._valueLabel.text = ((int) this._slider.value).ToString();
      }
    }

    public override void SyncElement()
    {
      if (this._settingType == SettingType.Float)
        this._slider.value = ((TypedSetting<float>) this._setting).Value;
      else if (this._settingType == SettingType.Int)
        this._slider.value = (float) ((TypedSetting<int>) this._setting).Value;
      this.UpdateValueLabel();
    }
  }
}
