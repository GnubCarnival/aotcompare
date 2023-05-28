// Decompiled with JetBrains decompiler
// Type: InvAttachmentPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
  private GameObject mChild;
  private GameObject mPrefab;
  public InvBaseItem.Slot slot;

  public GameObject Attach(GameObject prefab)
  {
    if (Object.op_Inequality((Object) this.mPrefab, (Object) prefab))
    {
      this.mPrefab = prefab;
      if (Object.op_Inequality((Object) this.mChild, (Object) null))
        Object.Destroy((Object) this.mChild);
      if (Object.op_Inequality((Object) this.mPrefab, (Object) null))
      {
        Transform transform1 = ((Component) this).transform;
        this.mChild = Object.Instantiate((Object) this.mPrefab, transform1.position, transform1.rotation) as GameObject;
        Transform transform2 = this.mChild.transform;
        transform2.parent = transform1;
        transform2.localPosition = Vector3.zero;
        transform2.localRotation = Quaternion.identity;
        transform2.localScale = Vector3.one;
      }
    }
    return this.mChild;
  }
}
