// Decompiled with JetBrains decompiler
// Type: UI.ColorPickPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ColorPickPopup : PromptPopup
  {
    private float PreviewWidth = 90f;
    private float PreviewHeight = 40f;
    private Image _image;
    private ColorSetting _setting;
    private Image _preview;
    private FloatSetting _red = new FloatSetting(0.0f, 0.0f, 1f);
    private FloatSetting _green = new FloatSetting(0.0f, 0.0f, 1f);
    private FloatSetting _blue = new FloatSetting(0.0f, 0.0f, 1f);
    private FloatSetting _alpha = new FloatSetting(0.0f, 0.0f, 1f);
    private List<GameObject> _sliders = new List<GameObject>();

    protected override string Title => UIManager.GetLocale("SettingsPopup", nameof (ColorPickPopup), nameof (Title));

    protected override float Width => 450f;

    protected override float Height => 450f;

    protected override float VerticalSpacing => 20f;

    protected override TextAnchor PanelAlignment => (TextAnchor) 1;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__20_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__20_1)));
      GameObject gameObject = ElementFactory.InstantiateAndBind(this.SinglePanel, "ColorPreview");
      gameObject.GetComponent<LayoutElement>().preferredWidth = this.PreviewWidth;
      gameObject.GetComponent<LayoutElement>().preferredHeight = this.PreviewHeight;
      this._preview = ((Component) gameObject.transform.Find("Image")).GetComponent<Image>();
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this._preview, (Object) null))
        return;
      ((Graphic) this._preview).color = this.GetColorFromSliders();
    }

    public void Show(ColorSetting setting, Image image)
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      this.Show();
      this._setting = setting;
      this._image = image;
      this._red.Value = setting.Value.r;
      this._green.Value = setting.Value.g;
      this._blue.Value = setting.Value.b;
      this._alpha.MinValue = setting.MinAlpha;
      this._alpha.Value = setting.Value.a;
      ((Graphic) this._preview).color = this.GetColorFromSliders();
      this.CreateSliders();
    }

    private void CreateSliders()
    {
      foreach (Object slider in this._sliders)
        Object.Destroy(slider);
      ElementStyle style = new ElementStyle(titleWidth: 85f, themePanel: this.ThemePanel);
      this._sliders.Add(ElementFactory.CreateSliderInputSetting(this.SinglePanel, style, (BaseSetting) this._red, "Red", decimalPlaces: 3));
      this._sliders.Add(ElementFactory.CreateSliderInputSetting(this.SinglePanel, style, (BaseSetting) this._green, "Green", decimalPlaces: 3));
      this._sliders.Add(ElementFactory.CreateSliderInputSetting(this.SinglePanel, style, (BaseSetting) this._blue, "Blue", decimalPlaces: 3));
      this._sliders.Add(ElementFactory.CreateSliderInputSetting(this.SinglePanel, style, (BaseSetting) this._alpha, "Alpha", decimalPlaces: 3));
    }

    private void OnButtonClick(string name)
    {
      switch (name)
      {
        case "Cancel":
          this.Hide();
          break;
        case "Save":
          this._setting.Value = this.GetColorFromSliders();
          ((Graphic) this._image).color = this._setting.Value;
          this.Hide();
          break;
      }
    }

    private Color GetColorFromSliders() => new Color(this._red.Value, this._green.Value, this._blue.Value, Mathf.Clamp(this._alpha.Value, this._setting.MinAlpha, 1f));
  }
}
