// Decompiled with JetBrains decompiler
// Type: RockScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RockScript : MonoBehaviour
{
  private Vector3 desPt = new Vector3(-200f, 0.0f, -280f);
  private bool disable;
  private float g = 500f;
  private float speed = 800f;
  private Vector3 vh;
  private Vector3 vv;

  private void Start()
  {
    ((Component) this).transform.position = new Vector3(0.0f, 0.0f, 676f);
    this.vh = Vector3.op_Subtraction(this.desPt, ((Component) this).transform.position);
    this.vv = new Vector3(0.0f, (float) ((double) this.g * (double) ((Vector3) ref this.vh).magnitude / (2.0 * (double) this.speed)), 0.0f);
    ((Vector3) ref this.vh).Normalize();
    this.vh = Vector3.op_Multiply(this.vh, this.speed);
  }

  private void Update()
  {
    if (this.disable)
      return;
    this.vv = Vector3.op_Addition(this.vv, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.up), this.g), Time.deltaTime));
    Transform transform1 = ((Component) this).transform;
    transform1.position = Vector3.op_Addition(transform1.position, Vector3.op_Multiply(this.vv, Time.deltaTime));
    Transform transform2 = ((Component) this).transform;
    transform2.position = Vector3.op_Addition(transform2.position, Vector3.op_Multiply(this.vh, Time.deltaTime));
    if ((double) Vector3.Distance(this.desPt, ((Component) this).transform.position) >= 20.0 && (double) ((Component) this).transform.position.y >= 0.0)
      return;
    ((Component) this).transform.position = this.desPt;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      if (FengGameManagerMKII.LAN)
        Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
      else
        PhotonNetwork.Instantiate("FX/boom1_CT_KICK", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
    }
    else
      Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f));
    this.disable = true;
  }
}
