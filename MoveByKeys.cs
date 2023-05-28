// Decompiled with JetBrains decompiler
// Type: MoveByKeys
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class MoveByKeys : MonoBehaviour
{
  public float speed = 10f;

  private void Start() => ((Behaviour) this).enabled = this.photonView.isMine;

  private void Update()
  {
    if (Input.GetKey((KeyCode) 97))
    {
      Transform transform = ((Component) this).transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.left, this.speed * Time.deltaTime));
    }
    if (Input.GetKey((KeyCode) 100))
    {
      Transform transform = ((Component) this).transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.right, this.speed * Time.deltaTime));
    }
    if (Input.GetKey((KeyCode) 119))
    {
      Transform transform = ((Component) this).transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.forward, this.speed * Time.deltaTime));
    }
    if (!Input.GetKey((KeyCode) 115))
      return;
    Transform transform1 = ((Component) this).transform;
    transform1.position = Vector3.op_Addition(transform1.position, Vector3.op_Multiply(Vector3.back, this.speed * Time.deltaTime));
  }
}
