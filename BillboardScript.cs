// Decompiled with JetBrains decompiler
// Type: BillboardScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BillboardScript : MonoBehaviour
{
  public void Main()
  {
  }

  public void Update()
  {
    ((Component) this).transform.LookAt(((Component) Camera.main).transform.position);
    ((Component) this).transform.Rotate(Vector3.op_Multiply(Vector3.left, -90f));
  }
}
