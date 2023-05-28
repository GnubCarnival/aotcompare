// Decompiled with JetBrains decompiler
// Type: EquipRandomItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
  public InvEquipment equipment;

  private void OnClick()
  {
    if (!Object.op_Inequality((Object) this.equipment, (Object) null))
      return;
    List<InvBaseItem> items = InvDatabase.list[0].items;
    if (items.Count == 0)
      return;
    int num1 = 12;
    int num2 = Random.Range(0, items.Count);
    InvBaseItem bi = items[num2];
    this.equipment.Equip(new InvGameItem(num2, bi)
    {
      quality = (InvGameItem.Quality) Random.Range(0, num1),
      itemLevel = NGUITools.RandomRange(bi.minItemLevel, bi.maxItemLevel)
    });
  }
}
