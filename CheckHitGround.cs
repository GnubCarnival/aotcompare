// Decompiled with JetBrains decompiler
// Type: CheckHitGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CheckHitGround : MonoBehaviour
{
  public bool isGrounded;

  private void OnTriggerEnter(Collider other)
  {
    if (((Component) other).gameObject.layer == LayerMask.NameToLayer("Ground"))
      this.isGrounded = true;
    if (((Component) other).gameObject.layer != LayerMask.NameToLayer("EnemyAABB"))
      return;
    this.isGrounded = true;
  }

  private void OnTriggerStay(Collider other)
  {
    if (((Component) other).gameObject.layer == LayerMask.NameToLayer("Ground"))
      this.isGrounded = true;
    if (((Component) other).gameObject.layer != LayerMask.NameToLayer("EnemyAABB"))
      return;
    this.isGrounded = true;
  }
}
