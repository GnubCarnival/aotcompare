// Decompiled with JetBrains decompiler
// Type: BodyPushBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BodyPushBox : MonoBehaviour
{
  public GameObject parent;

  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "bodyCollider"))
      return;
    BodyPushBox component = ((Component) other).gameObject.GetComponent<BodyPushBox>();
    if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) component.parent, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(component.parent.transform.position, this.parent.transform.position);
    float radius1 = ((Component) this).gameObject.GetComponent<CapsuleCollider>().radius;
    float radius2 = ((Component) this).gameObject.GetComponent<CapsuleCollider>().radius;
    vector3.y = 0.0f;
    float num;
    if ((double) ((Vector3) ref vector3).magnitude > 0.0)
    {
      num = radius1 + radius2 - ((Vector3) ref vector3).magnitude;
      ((Vector3) ref vector3).Normalize();
    }
    else
    {
      num = radius1 + radius2;
      vector3.x = 1f;
    }
  }
}
