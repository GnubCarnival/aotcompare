// Decompiled with JetBrains decompiler
// Type: UI.HeadedPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class HeadedPanel : BasePanel
  {
    protected Transform BottomBar;
    protected Transform TopBar;
    protected Dictionary<string, Button> _topButtons = new Dictionary<string, Button>();

    protected virtual string Title => "Default";

    protected virtual float TopBarHeight => 65f;

    protected virtual float BottomBarHeight => 65f;

    protected override float BorderVerticalPadding => 5f;

    protected override float BorderHorizontalPadding => 5f;

    protected override int VerticalPadding => 25;

    protected override int HorizontalPadding => 35;

    protected virtual int TitleFontSize => 30;

    protected virtual int ButtonFontSize => 28;

    protected virtual bool CategoryButtons => false;

    public override void Setup(BasePanel parent = null)
    {
      this.TopBar = ((Component) this).transform.Find("Background/TopBar");
      this.BottomBar = ((Component) this).transform.Find("Background/BottomBar");
      Transform transform1 = ((Component) this).transform.Find("Background/TopBarLine");
      Transform transform2 = ((Component) this).transform.Find("Background/BottomBarLine");
      ((Component) this.TopBar).GetComponent<RectTransform>().SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.TopBarHeight);
      ((Component) this.BottomBar).GetComponent<RectTransform>().SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.BottomBarHeight);
      ((Component) transform1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -this.TopBarHeight);
      ((Component) transform2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, this.BottomBarHeight);
      if (Object.op_Inequality((Object) this.TopBar.Find("Label"), (Object) null))
      {
        if (this.CategoryButtons)
        {
          ((Component) this.TopBar.Find("Label")).gameObject.SetActive(false);
        }
        else
        {
          ((Component) this.TopBar.Find("Label")).GetComponent<Text>().fontSize = this.TitleFontSize;
          ((Graphic) ((Component) this.TopBar.Find("Label")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "TitleColor");
          this.SetTitle(this.Title);
        }
      }
      ((Graphic) ((Component) this.TopBar).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "TopBarColor");
      ((Graphic) ((Component) this.BottomBar).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BottomBarColor");
      ((Graphic) ((Component) transform1).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
      ((Graphic) ((Component) transform2).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
      ((Graphic) ((Component) ((Component) this).transform.Find("Border")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
      ((Graphic) ((Component) ((Component) this).transform.Find("Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BackgroundColor");
      base.Setup(parent);
      if (!this.CategoryButtons)
        return;
      this.SetupTopButtons();
      this.SetTopButton(this._currentCategoryPanelName.Value);
    }

    public override void SetCategoryPanel(string name)
    {
      base.SetCategoryPanel(name);
      this.SetTopButton(name);
    }

    protected virtual void SetTopButton(string name)
    {
      if (this._topButtons.Count <= 0)
        return;
      foreach (Selectable selectable in this._topButtons.Values)
        selectable.interactable = true;
      ((Selectable) this._topButtons[name]).interactable = false;
    }

    protected void SetTitle(string title) => ((Component) this.TopBar.Find("Label")).GetComponent<Text>().text = title;

    protected virtual void SetupTopButtons()
    {
      Canvas.ForceUpdateCanvases();
      float num1 = 0.0f;
      foreach (Button button in this._topButtons.Values)
      {
        double num2 = (double) num1;
        Rect rect = ((Component) button).GetComponent<RectTransform>().rect;
        double width = (double) ((Rect) ref rect).width;
        num1 = (float) (num2 + width);
      }
      ((HorizontalOrVerticalLayoutGroup) ((Component) this.TopBar).GetComponent<HorizontalLayoutGroup>()).spacing = (this.Width - num1) / (float) (this._topButtons.Count + 1);
    }

    protected override float GetPanelHeight() => (float) ((double) this.Height - (double) ((Component) this.TopBar).GetComponent<RectTransform>().sizeDelta.y - (double) ((Component) this.BottomBar).GetComponent<RectTransform>().sizeDelta.y - (double) this.BorderVerticalPadding * 2.0);
  }
}
