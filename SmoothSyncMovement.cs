// Decompiled with JetBrains decompiler
// Type: SmoothSyncMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class SmoothSyncMovement : MonoBehaviour
{
  private Vector3 correctCameraPos;
  public Quaternion correctCameraRot;
  private Vector3 correctPlayerPos = Vector3.zero;
  private Quaternion correctPlayerRot = Quaternion.identity;
  private Vector3 correctPlayerVelocity = Vector3.zero;
  public bool disabled;
  public bool noVelocity;
  public bool PhotonCamera;
  public float SmoothingDelay = 5f;

  public void Awake()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      ((Behaviour) this).enabled = false;
    this.correctPlayerPos = ((Component) this).transform.position;
    this.correctPlayerRot = ((Component) this).transform.rotation;
    if (!Object.op_Equality((Object) ((Component) this).rigidbody, (Object) null))
      return;
    this.noVelocity = true;
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting)
    {
      stream.SendNext((object) ((Component) this).transform.position);
      stream.SendNext((object) ((Component) this).transform.rotation);
      if (!this.noVelocity)
        stream.SendNext((object) ((Component) this).rigidbody.velocity);
      if (!this.PhotonCamera)
        return;
      stream.SendNext((object) ((Component) Camera.main).transform.rotation);
    }
    else
    {
      this.correctPlayerPos = (Vector3) stream.ReceiveNext();
      this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
      if (!this.noVelocity)
        this.correctPlayerVelocity = (Vector3) stream.ReceiveNext();
      if (!this.PhotonCamera)
        return;
      this.correctCameraRot = (Quaternion) stream.ReceiveNext();
    }
  }

  public void Update()
  {
    if (this.disabled || this.photonView.isMine)
      return;
    ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
    ((Component) this).transform.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
    if (this.noVelocity)
      return;
    ((Component) this).rigidbody.velocity = this.correctPlayerVelocity;
  }
}
