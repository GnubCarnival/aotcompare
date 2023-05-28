// Decompiled with JetBrains decompiler
// Type: UI.IntroButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class IntroButton : Button
  {
    private float _fadeTime = 0.1f;
    private Image _hoverImage;

    protected virtual void Awake()
    {
      this._hoverImage = ((Component) ((Component) this).transform.Find("HoverImage")).GetComponent<Image>();
      ((Graphic) this._hoverImage).canvasRenderer.SetAlpha(0.0f);
      ((Selectable) this).transition = (Selectable.Transition) 1;
      ((Selectable) this).targetGraphic = ((Component) ((Component) this).transform.Find("Content/Label")).GetComponent<Graphic>();
      if (((Object) ((Component) this).gameObject).name.StartsWith("Settings") || ((Object) ((Component) this).gameObject).name.StartsWith("Quit") || ((Object) ((Component) this).gameObject).name.StartsWith("Profile"))
        ((Component) ((Selectable) this).targetGraphic).GetComponent<Text>().text = UIManager.GetLocaleCommon(((Object) ((Component) this).gameObject).name.Replace("Button", string.Empty));
      else
        ((Component) ((Selectable) this).targetGraphic).GetComponent<Text>().text = UIManager.GetLocale("MainMenu", "Intro", ((Object) ((Component) this).gameObject).name);
      ColorBlock colorBlock = new ColorBlock();
      ColorBlock themeColorBlock = UIManager.GetThemeColorBlock("MainMenu", nameof (IntroButton), "");
      ((ColorBlock) ref colorBlock).normalColor = ((ColorBlock) ref themeColorBlock).normalColor;
      ((ColorBlock) ref colorBlock).highlightedColor = ((ColorBlock) ref themeColorBlock).highlightedColor;
      ((ColorBlock) ref colorBlock).pressedColor = ((ColorBlock) ref themeColorBlock).pressedColor;
      ((ColorBlock) ref colorBlock).colorMultiplier = 1f;
      ((ColorBlock) ref colorBlock).fadeDuration = this._fadeTime;
      ((Selectable) this).colors = colorBlock;
      Navigation navigation = new Navigation();
      ((Navigation) ref navigation).mode = (Navigation.Mode) 0;
      ((Selectable) this).navigation = navigation;
    }

    protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      ((Selectable) this).DoStateTransition(state, instant);
      Image component = ((Component) ((Component) this).transform.Find("Content/Icon")).GetComponent<Image>();
      if (state == 2 || state == 1)
      {
        ((Graphic) this._hoverImage).CrossFadeAlpha(1f, this._fadeTime, true);
        if (state == 2)
        {
          ((Graphic) component).CrossFadeColor(UIManager.GetThemeColor("MainMenu", nameof (IntroButton), "PressedColor"), this._fadeTime, true, true);
        }
        else
        {
          if (state != 1)
            return;
          ((Graphic) component).CrossFadeColor(UIManager.GetThemeColor("MainMenu", nameof (IntroButton), "HighlightedColor"), this._fadeTime, true, true);
        }
      }
      else
      {
        if (state != null)
          return;
        ((Graphic) this._hoverImage).CrossFadeAlpha(0.0f, this._fadeTime, true);
        ((Graphic) component).CrossFadeColor(UIManager.GetThemeColor("MainMenu", nameof (IntroButton), "NormalColor"), this._fadeTime, true, true);
      }
    }
  }
}
