// Decompiled with JetBrains decompiler
// Type: UI.BaseMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal abstract class BaseMenu : MonoBehaviour
  {
    protected List<BasePopup> _popups = new List<BasePopup>();
    public TooltipPopup TooltipPopup;
    public MessagePopup MessagePopup;
    public ConfirmPopup ConfirmPopup;

    public virtual void Setup() => this.SetupPopups();

    public void ApplyScale() => this.StartCoroutine(this.WaitAndApplyScale());

    protected IEnumerator WaitAndApplyScale()
    {
      BaseMenu baseMenu = this;
      float num = 1f / SettingsManager.UISettings.UIMasterScale.Value;
      ((Component) baseMenu).GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f * num, 1080f * num);
      yield return (object) new WaitForEndOfFrame();
      yield return (object) new WaitForEndOfFrame();
      UIManager.CurrentCanvasScale = ((Transform) ((Component) baseMenu).GetComponent<RectTransform>()).localScale.x;
      foreach (BaseScaler componentsInChild in ((Component) baseMenu).GetComponentsInChildren<BaseScaler>(true))
        componentsInChild.ApplyScale();
    }

    protected virtual void SetupPopups()
    {
      this.TooltipPopup = ElementFactory.CreateTooltipPopup<TooltipPopup>(((Component) this).transform).GetComponent<TooltipPopup>();
      this.MessagePopup = ElementFactory.CreateHeadedPanel<MessagePopup>(((Component) this).transform).GetComponent<MessagePopup>();
      this.ConfirmPopup = ElementFactory.CreateHeadedPanel<ConfirmPopup>(((Component) this).transform).GetComponent<ConfirmPopup>();
      this._popups.Add((BasePopup) this.TooltipPopup);
      this._popups.Add((BasePopup) this.MessagePopup);
      this._popups.Add((BasePopup) this.ConfirmPopup);
    }

    protected virtual void HideAllPopups()
    {
      foreach (BasePanel popup in this._popups)
        popup.Hide();
    }
  }
}
