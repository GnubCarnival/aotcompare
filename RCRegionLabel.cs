// Decompiled with JetBrains decompiler
// Type: RCRegionLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RCRegionLabel : MonoBehaviour
{
  public GameObject myLabel;

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.myLabel, (Object) null) || !this.myLabel.GetComponent<UILabel>().isVisible)
      return;
    this.myLabel.transform.LookAt(Vector3.op_Subtraction(Vector3.op_Multiply(2f, this.myLabel.transform.position), ((Component) Camera.main).transform.position));
  }
}
