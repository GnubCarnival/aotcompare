// Decompiled with JetBrains decompiler
// Type: UIStorageSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
  public int slot;
  public UIItemStorage storage;

  protected override InvGameItem Replace(InvGameItem item) => !Object.op_Equality((Object) this.storage, (Object) null) ? this.storage.Replace(this.slot, item) : item;

  protected override InvGameItem observedItem => !Object.op_Equality((Object) this.storage, (Object) null) ? this.storage.GetItem(this.slot) : (InvGameItem) null;
}
