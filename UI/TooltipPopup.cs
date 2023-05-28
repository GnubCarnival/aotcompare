// Decompiled with JetBrains decompiler
// Type: UI.TooltipPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class TooltipPopup : BasePopup
  {
    private Text _label;
    private RectTransform _panel;
    public TooltipButton Caller;

    protected override float AnimationTime => 0.15f;

    protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

    public override void Setup(BasePanel parent = null)
    {
      this._label = ((Component) ((Component) this).transform.Find("Panel/Label")).GetComponent<Text>();
      this._label.text = string.Empty;
      this._panel = ((Component) ((Component) this).transform.Find("Panel")).GetComponent<RectTransform>();
      ((Graphic) this._label).color = UIManager.GetThemeColor(this.ThemePanel, "DefaultSetting", "TooltipTextColor");
      ((Graphic) ((Component) ((Transform) this._panel).Find("Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "DefaultSetting", "TooltipBackgroundColor");
    }

    public void Show(string message, TooltipButton caller)
    {
      if (((Component) this).gameObject.activeSelf)
      {
        this.StopAllCoroutines();
        this.SetTransformAlpha(this.MaxFadeAlpha);
      }
      this._label.text = message;
      this.Caller = caller;
      Canvas.ForceUpdateCanvases();
      this.SetTooltipPosition();
      this.Show();
    }

    private void SetTooltipPosition()
    {
      float num = (float) ((double) ((Component) this).GetComponent<RectTransform>().sizeDelta.x * 0.5 + 40.0) * UIManager.CurrentCanvasScale;
      Vector3 position = ((Component) this.Caller).transform.position;
      if ((double) position.x + (double) num > (double) Screen.width)
        position.x -= num;
      else
        position.x += num;
      ((Component) this).transform.position = position;
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this.Caller, (Object) null))
        return;
      this.SetTooltipPosition();
    }
  }
}
