// Decompiled with JetBrains decompiler
// Type: UIItemSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemSlot : MonoBehaviour
{
  public UIWidget background;
  public AudioClip errorSound;
  public AudioClip grabSound;
  public UISprite icon;
  public UILabel label;
  private static InvGameItem mDraggedItem;
  private InvGameItem mItem;
  private string mText = string.Empty;
  public AudioClip placeSound;

  private void OnClick()
  {
    if (UIItemSlot.mDraggedItem != null)
    {
      this.OnDrop((GameObject) null);
    }
    else
    {
      if (this.mItem == null)
        return;
      UIItemSlot.mDraggedItem = this.Replace((InvGameItem) null);
      if (UIItemSlot.mDraggedItem != null)
        NGUITools.PlaySound(this.grabSound);
      this.UpdateCursor();
    }
  }

  private void OnDrag(Vector2 delta)
  {
    if (UIItemSlot.mDraggedItem != null || this.mItem == null)
      return;
    UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
    UIItemSlot.mDraggedItem = this.Replace((InvGameItem) null);
    NGUITools.PlaySound(this.grabSound);
    this.UpdateCursor();
  }

  private void OnDrop(GameObject go)
  {
    InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
    if (UIItemSlot.mDraggedItem == invGameItem)
      NGUITools.PlaySound(this.errorSound);
    else if (invGameItem != null)
      NGUITools.PlaySound(this.grabSound);
    else
      NGUITools.PlaySound(this.placeSound);
    UIItemSlot.mDraggedItem = invGameItem;
    this.UpdateCursor();
  }

  private void OnTooltip(bool show)
  {
    InvGameItem mItem = !show ? (InvGameItem) null : this.mItem;
    if (mItem != null)
    {
      InvBaseItem baseItem = mItem.baseItem;
      if (baseItem != null)
      {
        string tooltipText = "[" + NGUITools.EncodeColor(mItem.color) + "]" + mItem.name + "[-]\n" + "[AFAFAF]Level " + (object) mItem.itemLevel + " " + (object) baseItem.slot;
        List<InvStat> stats = mItem.CalculateStats();
        int index = 0;
        for (int count = stats.Count; index < count; ++index)
        {
          InvStat invStat = stats[index];
          if (invStat.amount != 0)
          {
            string str = invStat.amount >= 0 ? tooltipText + "\n[00FF00]+" + invStat.amount.ToString() : tooltipText + "\n[FF0000]" + invStat.amount.ToString();
            if (invStat.modifier == InvStat.Modifier.Percent)
              str += "%";
            tooltipText = str + " " + invStat.id.ToString() + "[-]";
          }
        }
        if (!string.IsNullOrEmpty(baseItem.description))
          tooltipText = tooltipText + "\n[FF9900]" + baseItem.description;
        UITooltip.ShowText(tooltipText);
        return;
      }
    }
    UITooltip.ShowText((string) null);
  }

  protected abstract InvGameItem Replace(InvGameItem item);

  private void Update()
  {
    InvGameItem observedItem = this.observedItem;
    if (this.mItem == observedItem)
      return;
    this.mItem = observedItem;
    InvBaseItem baseItem = observedItem == null ? (InvBaseItem) null : observedItem.baseItem;
    if (Object.op_Inequality((Object) this.label, (Object) null))
    {
      string name = observedItem == null ? (string) null : observedItem.name;
      if (string.IsNullOrEmpty(this.mText))
        this.mText = this.label.text;
      this.label.text = name == null ? this.mText : name;
    }
    if (Object.op_Inequality((Object) this.icon, (Object) null))
    {
      if (baseItem == null || Object.op_Equality((Object) baseItem.iconAtlas, (Object) null))
      {
        ((Behaviour) this.icon).enabled = false;
      }
      else
      {
        this.icon.atlas = baseItem.iconAtlas;
        this.icon.spriteName = baseItem.iconName;
        ((Behaviour) this.icon).enabled = true;
        this.icon.MakePixelPerfect();
      }
    }
    if (!Object.op_Inequality((Object) this.background, (Object) null))
      return;
    this.background.color = observedItem == null ? Color.white : observedItem.color;
  }

  private void UpdateCursor()
  {
    if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
      UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
    else
      UICursor.Clear();
  }

  protected abstract InvGameItem observedItem { get; }
}
