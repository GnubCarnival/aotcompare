// Decompiled with JetBrains decompiler
// Type: MovementUpdate1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class MovementUpdate1 : MonoBehaviour
{
  public bool disabled;
  private Vector3 lastPosition;
  private Quaternion lastRotation;
  private Vector3 lastVelocity;

  private void Start()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      this.disabled = true;
      ((Behaviour) this).enabled = false;
    }
    else if (((Component) this).networkView.isMine)
      ((Component) this).networkView.RPC("updateMovement1", (RPCMode) 5, new object[3]
      {
        (object) ((Component) this).transform.position,
        (object) ((Component) this).transform.rotation,
        (object) ((Component) this).transform.lossyScale
      });
    else
      ((Behaviour) this).enabled = false;
  }

  private void Update()
  {
    if (this.disabled)
      return;
    ((Component) this).networkView.RPC("updateMovement1", (RPCMode) 1, new object[3]
    {
      (object) ((Component) this).transform.position,
      (object) ((Component) this).transform.rotation,
      (object) ((Component) this).transform.lossyScale
    });
  }

  [RPC]
  private void updateMovement1(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
  {
    ((Component) this).transform.position = newPosition;
    ((Component) this).transform.rotation = newRotation;
    ((Component) this).transform.localScale = newScale;
  }
}
