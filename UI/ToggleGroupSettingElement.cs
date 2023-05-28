// Decompiled with JetBrains decompiler
// Type: UI.ToggleGroupSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ToggleGroupSettingElement : BaseSettingElement
  {
    protected ToggleGroup _toggleGroup;
    protected GameObject _optionsPanel;
    protected string[] _options;
    protected List<Toggle> _toggles = new List<Toggle>();
    private float _checkMarkSizeMultiplier = 0.67f;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.String,
      SettingType.Int
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string[] options,
      string tooltip,
      float elementWidth,
      float elementHeight)
    {
      this._options = options.Length != 0 ? options : throw new ArgumentException("ToggleGroup cannot have 0 options.");
      this._optionsPanel = ((Component) ((Component) this).transform.Find("Options")).gameObject;
      this._toggleGroup = this._optionsPanel.GetComponent<ToggleGroup>();
      for (int index = 0; index < options.Length; ++index)
        this._toggles.Add(this.CreateOptionToggle(options[index], index, style, elementWidth, elementHeight));
      ((Component) ((Component) this).gameObject.transform.Find("Label")).GetComponent<LayoutElement>().preferredHeight = elementHeight;
      this.Setup(setting, style, title, tooltip);
    }

    protected Toggle CreateOptionToggle(
      string option,
      int index,
      ElementStyle style,
      float width,
      float height)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ToggleGroupSettingElement.\u003C\u003Ec__DisplayClass8_0 cDisplayClass80 = new ToggleGroupSettingElement.\u003C\u003Ec__DisplayClass8_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.option = option;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.index = index;
      GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("ToggleGroupOption");
      gameObject.transform.SetParent(this._optionsPanel.transform, false);
      ((Graphic) ((Component) gameObject.transform.Find("Label")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
      // ISSUE: reference to a compiler-generated field
      this.SetupLabel(((Component) gameObject.transform.Find("Label")).gameObject, cDisplayClass80.option, style.FontSize);
      LayoutElement component1 = ((Component) gameObject.transform.Find("Background")).GetComponent<LayoutElement>();
      RectTransform component2 = ((Component) ((Component) component1).transform.Find("Checkmark")).GetComponent<RectTransform>();
      component1.preferredWidth = width;
      component1.preferredHeight = height;
      component2.sizeDelta = new Vector2(width * this._checkMarkSizeMultiplier, height * this._checkMarkSizeMultiplier);
      Toggle component3 = gameObject.GetComponent<Toggle>();
      component3.group = this._toggleGroup;
      component3.isOn = false;
      // ISSUE: method pointer
      ((UnityEvent<bool>) component3.onValueChanged).AddListener(new UnityAction<bool>((object) cDisplayClass80, __methodptr(\u003CCreateOptionToggle\u003Eb__0)));
      ((Selectable) component3).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
      ((Graphic) ((Component) component2).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
      return component3;
    }

    protected void OnValueChanged(string option, int index, bool value)
    {
      if (!value)
        return;
      if (this._settingType == SettingType.String)
      {
        ((TypedSetting<string>) this._setting).Value = option;
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        ((TypedSetting<int>) this._setting).Value = index;
      }
    }

    public override void SyncElement()
    {
      this._toggleGroup.SetAllTogglesOff();
      if (this._settingType == SettingType.String)
      {
        this._toggles[this.FindOptionIndex(((TypedSetting<string>) this._setting).Value)].isOn = true;
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        this._toggles[((TypedSetting<int>) this._setting).Value].isOn = true;
      }
    }

    private int FindOptionIndex(string option)
    {
      for (int optionIndex = 0; optionIndex < this._options.Length; ++optionIndex)
      {
        if (this._options[optionIndex] == option)
          return optionIndex;
      }
      throw new ArgumentOutOfRangeException("Option not found");
    }
  }
}
