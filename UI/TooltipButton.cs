// Decompiled with JetBrains decompiler
// Type: UI.TooltipButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class TooltipButton : Button
  {
    private string _tooltipMessage;

    private void Awake()
    {
      ((Selectable) this).transition = (Selectable.Transition) 1;
      ((Selectable) this).targetGraphic = ((Component) this).GetComponent<Graphic>();
    }

    public virtual void Setup(string tooltipMessage, ElementStyle style)
    {
      this._tooltipMessage = tooltipMessage;
      ((Selectable) this).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
    }

    protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      ((Selectable) this).DoStateTransition(state, instant);
      if (Object.op_Equality((Object) UIManager.CurrentMenu, (Object) null))
        return;
      TooltipPopup tooltipPopup = UIManager.CurrentMenu.TooltipPopup;
      if (state == 2 || state == 1)
      {
        tooltipPopup.Show(this._tooltipMessage, this);
      }
      else
      {
        if (state != null || !Object.op_Equality((Object) tooltipPopup.Caller, (Object) this))
          return;
        UIManager.CurrentMenu.TooltipPopup.Hide();
      }
    }
  }
}
