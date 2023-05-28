// Decompiled with JetBrains decompiler
// Type: UI.ColorSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ColorSettingElement : BaseSettingElement
  {
    private Image _image;
    private ColorPickPopup _colorPickPopup;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Color
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      ColorPickPopup colorPickPopup,
      string tooltip,
      float elementWidth,
      float elementHeight)
    {
      this._colorPickPopup = colorPickPopup;
      GameObject gameObject = ((Component) ((Component) this).transform.Find("ColorButton")).gameObject;
      gameObject.GetComponent<LayoutElement>().preferredWidth = elementWidth;
      gameObject.GetComponent<LayoutElement>().preferredHeight = elementHeight;
      // ISSUE: method pointer
      ((UnityEvent) gameObject.GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__4_0)));
      ((Selectable) gameObject.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
      this._image = ((Component) gameObject.transform.Find("Border/Image")).GetComponent<Image>();
      this.Setup(setting, style, title, tooltip);
    }

    protected void OnButtonClicked() => this._colorPickPopup.Show((ColorSetting) this._setting, this._image);

    public override void SyncElement() => ((Graphic) this._image).color = ((TypedSetting<Color>) this._setting).Value;
  }
}
