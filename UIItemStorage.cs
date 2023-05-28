// Decompiled with JetBrains decompiler
// Type: UIItemStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
  public UIWidget background;
  public int maxColumns = 4;
  public int maxItemCount = 8;
  public int maxRows = 4;
  private List<InvGameItem> mItems = new List<InvGameItem>();
  public int padding = 10;
  public int spacing = 128;
  public GameObject template;

  public InvGameItem GetItem(int slot) => slot < this.items.Count ? this.mItems[slot] : (InvGameItem) null;

  public InvGameItem Replace(int slot, InvGameItem item)
  {
    if (slot >= this.maxItemCount)
      return item;
    InvGameItem invGameItem = this.items[slot];
    this.mItems[slot] = item;
    return invGameItem;
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) this.template, (Object) null))
      return;
    int num = 0;
    Bounds bounds = new Bounds();
    for (int index1 = 0; index1 < this.maxRows; ++index1)
    {
      for (int index2 = 0; index2 < this.maxColumns; ++index2)
      {
        GameObject gameObject = NGUITools.AddChild(((Component) this).gameObject, this.template);
        gameObject.transform.localPosition = new Vector3((float) this.padding + ((float) index2 + 0.5f) * (float) this.spacing, (float) -this.padding - ((float) index1 + 0.5f) * (float) this.spacing, 0.0f);
        UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.storage = this;
          component.slot = num;
        }
        ((Bounds) ref bounds).Encapsulate(new Vector3((float) this.padding * 2f + (float) ((index2 + 1) * this.spacing), (float) -this.padding * 2f - (float) ((index1 + 1) * this.spacing), 0.0f));
        if (++num >= this.maxItemCount)
        {
          if (!Object.op_Inequality((Object) this.background, (Object) null))
            return;
          ((Component) this.background).transform.localScale = ((Bounds) ref bounds).size;
          return;
        }
      }
    }
    if (!Object.op_Inequality((Object) this.background, (Object) null))
      return;
    ((Component) this.background).transform.localScale = ((Bounds) ref bounds).size;
  }

  public List<InvGameItem> items
  {
    get
    {
      while (this.mItems.Count < this.maxItemCount)
        this.mItems.Add((InvGameItem) null);
      return this.mItems;
    }
  }
}
