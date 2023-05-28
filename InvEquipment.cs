// Decompiled with JetBrains decompiler
// Type: InvEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equipment")]
public class InvEquipment : MonoBehaviour
{
  private InvAttachmentPoint[] mAttachments;
  private InvGameItem[] mItems;

  public InvGameItem Equip(InvGameItem item)
  {
    if (item != null)
    {
      InvBaseItem baseItem = item.baseItem;
      if (baseItem != null)
        return this.Replace(baseItem.slot, item);
      Debug.LogWarning((object) ("Can't resolve the item ID of " + item.baseItemID.ToString()));
    }
    return item;
  }

  public InvGameItem GetItem(InvBaseItem.Slot slot)
  {
    if (slot != InvBaseItem.Slot.None)
    {
      int index = (int) (slot - 1);
      if (this.mItems != null && index < this.mItems.Length)
        return this.mItems[index];
    }
    return (InvGameItem) null;
  }

  public bool HasEquipped(InvBaseItem.Slot slot)
  {
    if (this.mItems != null)
    {
      int index = 0;
      for (int length = this.mItems.Length; index < length; ++index)
      {
        InvBaseItem baseItem = this.mItems[index].baseItem;
        if (baseItem != null && baseItem.slot == slot)
          return true;
      }
    }
    return false;
  }

  public bool HasEquipped(InvGameItem item)
  {
    if (this.mItems != null)
    {
      int index = 0;
      for (int length = this.mItems.Length; index < length; ++index)
      {
        if (this.mItems[index] == item)
          return true;
      }
    }
    return false;
  }

  public InvGameItem Replace(InvBaseItem.Slot slot, InvGameItem item)
  {
    InvBaseItem baseItem = item == null ? (InvBaseItem) null : item.baseItem;
    if (slot != InvBaseItem.Slot.None)
    {
      if (baseItem != null && baseItem.slot != slot)
        return item;
      if (this.mItems == null)
        this.mItems = new InvGameItem[8];
      InvGameItem mItem = this.mItems[(int) (slot - 1)];
      this.mItems[(int) (slot - 1)] = item;
      if (this.mAttachments == null)
        this.mAttachments = ((Component) this).GetComponentsInChildren<InvAttachmentPoint>();
      int index = 0;
      for (int length = this.mAttachments.Length; index < length; ++index)
      {
        InvAttachmentPoint mAttachment = this.mAttachments[index];
        if (mAttachment.slot == slot)
        {
          GameObject gameObject = mAttachment.Attach(baseItem == null ? (GameObject) null : baseItem.attachment);
          if (baseItem != null && Object.op_Inequality((Object) gameObject, (Object) null))
          {
            Renderer renderer = gameObject.renderer;
            if (Object.op_Inequality((Object) renderer, (Object) null))
              renderer.material.color = baseItem.color;
          }
        }
      }
      return mItem;
    }
    if (item != null)
      Debug.LogWarning((object) ("Can't equip \"" + item.name + "\" because it doesn't specify an item slot"));
    return item;
  }

  public InvGameItem Unequip(InvBaseItem.Slot slot) => this.Replace(slot, (InvGameItem) null);

  public InvGameItem Unequip(InvGameItem item)
  {
    if (item != null)
    {
      InvBaseItem baseItem = item.baseItem;
      if (baseItem != null)
        return this.Replace(baseItem.slot, (InvGameItem) null);
    }
    return item;
  }

  public InvGameItem[] equippedItems => this.mItems;
}
