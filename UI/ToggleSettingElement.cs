// Decompiled with JetBrains decompiler
// Type: UI.ToggleSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ToggleSettingElement : BaseSettingElement
  {
    protected Toggle _toggle;
    private float _checkMarkSizeMultiplier = 0.66f;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Bool
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string tooltip,
      float elementWidth,
      float elementHeight)
    {
      this._toggle = ((Component) ((Component) this).transform.Find("Toggle")).GetComponent<Toggle>();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this._toggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CSetup\u003Eb__4_0)));
      LayoutElement component1 = ((Component) ((Component) this._toggle).transform.Find("Background")).GetComponent<LayoutElement>();
      RectTransform component2 = ((Component) ((Component) component1).transform.Find("Checkmark")).GetComponent<RectTransform>();
      component1.preferredHeight = elementHeight;
      component1.preferredWidth = elementWidth;
      component2.sizeDelta = new Vector2(elementWidth * this._checkMarkSizeMultiplier, elementHeight * this._checkMarkSizeMultiplier);
      this.Setup(setting, style, title, tooltip);
      ((Graphic) ((Component) component2).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
      ((Selectable) this._toggle).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
    }

    protected void OnValueChanged(bool value) => ((TypedSetting<bool>) this._setting).Value = value;

    public override void SyncElement() => this._toggle.isOn = ((TypedSetting<bool>) this._setting).Value;
  }
}
