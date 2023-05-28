// Decompiled with JetBrains decompiler
// Type: MovementUpdate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class MovementUpdate : MonoBehaviour
{
  public bool disabled;
  private Vector3 lastPosition;
  private Quaternion lastRotation;
  private Vector3 lastVelocity;
  private Vector3 targetPosition;

  private void Start()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      this.disabled = true;
      ((Behaviour) this).enabled = false;
    }
    else if (((Component) this).networkView.isMine)
      ((Component) this).networkView.RPC("updateMovement", (RPCMode) 5, new object[4]
      {
        (object) ((Component) this).transform.position,
        (object) ((Component) this).transform.rotation,
        (object) ((Component) this).transform.localScale,
        (object) Vector3.zero
      });
    else
      this.targetPosition = ((Component) this).transform.position;
  }

  private void Update()
  {
    if (this.disabled || Network.peerType == null || Network.peerType == 3)
      return;
    if (((Component) this).networkView.isMine)
    {
      if ((double) Vector3.Distance(((Component) this).transform.position, this.lastPosition) >= 0.5)
      {
        this.lastPosition = ((Component) this).transform.position;
        ((Component) this).networkView.RPC("updateMovement", (RPCMode) 1, new object[4]
        {
          (object) ((Component) this).transform.position,
          (object) ((Component) this).transform.rotation,
          (object) ((Component) this).transform.localScale,
          (object) ((Component) this).rigidbody.velocity
        });
      }
      else if ((double) Vector3.Distance(((Component) ((Component) this).transform).rigidbody.velocity, this.lastVelocity) >= 0.10000000149011612)
      {
        this.lastVelocity = ((Component) ((Component) this).transform).rigidbody.velocity;
        ((Component) this).networkView.RPC("updateMovement", (RPCMode) 1, new object[4]
        {
          (object) ((Component) this).transform.position,
          (object) ((Component) this).transform.rotation,
          (object) ((Component) this).transform.localScale,
          (object) ((Component) this).rigidbody.velocity
        });
      }
      else
      {
        if ((double) Quaternion.Angle(((Component) this).transform.rotation, this.lastRotation) < 1.0)
          return;
        this.lastRotation = ((Component) this).transform.rotation;
        ((Component) this).networkView.RPC("updateMovement", (RPCMode) 1, new object[4]
        {
          (object) ((Component) this).transform.position,
          (object) ((Component) this).transform.rotation,
          (object) ((Component) this).transform.localScale,
          (object) ((Component) this).rigidbody.velocity
        });
      }
    }
    else
      ((Component) this).transform.position = Vector3.Slerp(((Component) this).transform.position, this.targetPosition, Time.deltaTime * 2f);
  }

  [RPC]
  private void updateMovement(
    Vector3 newPosition,
    Quaternion newRotation,
    Vector3 newScale,
    Vector3 veloctiy)
  {
    this.targetPosition = newPosition;
    ((Component) this).transform.rotation = newRotation;
    ((Component) this).transform.localScale = newScale;
    ((Component) this).rigidbody.velocity = veloctiy;
  }
}
