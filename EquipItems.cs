// Decompiled with JetBrains decompiler
// Type: EquipItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Items")]
public class EquipItems : MonoBehaviour
{
  public int[] itemIDs;

  private void Start()
  {
    if (this.itemIDs != null && this.itemIDs.Length != 0)
    {
      InvEquipment invEquipment = ((Component) this).GetComponent<InvEquipment>();
      if (Object.op_Equality((Object) invEquipment, (Object) null))
        invEquipment = ((Component) this).gameObject.AddComponent<InvEquipment>();
      int num = 12;
      int index = 0;
      for (int length = this.itemIDs.Length; index < length; ++index)
      {
        int itemId = this.itemIDs[index];
        InvBaseItem byId = InvDatabase.FindByID(itemId);
        if (byId != null)
        {
          InvGameItem invGameItem = new InvGameItem(itemId, byId)
          {
            quality = (InvGameItem.Quality) Random.Range(0, num),
            itemLevel = NGUITools.RandomRange(byId.minItemLevel, byId.maxItemLevel)
          };
          invEquipment.Equip(invGameItem);
        }
        else
          Debug.LogWarning((object) ("Can't resolve the item ID of " + itemId.ToString()));
      }
    }
    Object.Destroy((Object) this);
  }
}
