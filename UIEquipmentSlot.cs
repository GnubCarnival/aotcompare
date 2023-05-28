// Decompiled with JetBrains decompiler
// Type: UIEquipmentSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
  public InvEquipment equipment;
  public InvBaseItem.Slot slot;

  protected override InvGameItem Replace(InvGameItem item) => !Object.op_Equality((Object) this.equipment, (Object) null) ? this.equipment.Replace(this.slot, item) : item;

  protected override InvGameItem observedItem => !Object.op_Equality((Object) this.equipment, (Object) null) ? this.equipment.GetItem(this.slot) : (InvGameItem) null;
}
