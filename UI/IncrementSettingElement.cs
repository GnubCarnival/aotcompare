// Decompiled with JetBrains decompiler
// Type: UI.IncrementSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class IncrementSettingElement : BaseSettingElement
  {
    protected Text _valueLabel;
    protected string[] _options;
    protected UnityAction _onValueChanged;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Int
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string tooltip,
      float elementWidth,
      float elementHeight,
      string[] options,
      UnityAction onValueChanged)
    {
      this._valueLabel = ((Component) ((Component) this).transform.Find("Increment/ValueLabel")).GetComponent<Text>();
      this._valueLabel.fontSize = style.FontSize;
      this._options = options;
      this._onValueChanged = onValueChanged;
      Button component1 = ((Component) ((Component) this).transform.Find("Increment/LeftButton")).GetComponent<Button>();
      Button component2 = ((Component) ((Component) this).transform.Find("Increment/RightButton")).GetComponent<Button>();
      LayoutElement component3 = ((Component) component1).GetComponent<LayoutElement>();
      LayoutElement component4 = ((Component) component2).GetComponent<LayoutElement>();
      // ISSUE: method pointer
      ((UnityEvent) component1.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__5_0)));
      // ISSUE: method pointer
      ((UnityEvent) component2.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__5_1)));
      component3.preferredWidth = component4.preferredWidth = elementWidth;
      component3.preferredHeight = component4.preferredHeight = elementHeight;
      this.Setup(setting, style, title, tooltip);
      ((Selectable) component1).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
      ((Selectable) component2).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
      ((Graphic) this._valueLabel).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
    }

    protected void OnButtonPressed(bool increment)
    {
      if (this._settingType == SettingType.Int)
      {
        if (increment)
          ++((IntSetting) this._setting).Value;
        else
          --((IntSetting) this._setting).Value;
      }
      this.UpdateValueLabel();
      if (this._onValueChanged == null)
        return;
      this._onValueChanged.Invoke();
    }

    protected void UpdateValueLabel()
    {
      if (this._settingType != SettingType.Int)
        return;
      if (this._options == null)
        this._valueLabel.text = ((TypedSetting<int>) this._setting).Value.ToString();
      else
        this._valueLabel.text = this._options[((TypedSetting<int>) this._setting).Value];
    }

    public override void SyncElement() => this.UpdateValueLabel();
  }
}
