// Decompiled with JetBrains decompiler
// Type: SmoothSyncMovement3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class SmoothSyncMovement3 : MonoBehaviour
{
  private Vector3 correctPlayerPos = Vector3.zero;
  private Quaternion correctPlayerRot = Quaternion.identity;
  public bool disabled;
  public float SmoothingDelay = 5f;

  public void Awake()
  {
    this.SmoothingDelay = 10f;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      ((Behaviour) this).enabled = false;
    this.correctPlayerPos = ((Component) this).transform.position;
    this.correctPlayerRot = ((Component) this).transform.rotation;
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting)
    {
      stream.SendNext((object) ((Component) this).transform.position);
      stream.SendNext((object) ((Component) this).transform.rotation);
    }
    else
    {
      this.correctPlayerPos = (Vector3) stream.ReceiveNext();
      this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
    }
  }

  public void Update()
  {
    if (this.disabled || this.photonView.isMine)
      return;
    ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
    ((Component) this).transform.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
  }
}
