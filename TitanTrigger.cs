// Decompiled with JetBrains decompiler
// Type: TitanTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Constants;
using UnityEngine;

public class TitanTrigger : MonoBehaviour
{
  public bool isCollide;

  private void OnTriggerEnter(Collider other)
  {
    if (this.isCollide)
      return;
    GameObject gameObject = ((Component) ((Component) other).transform.root).gameObject;
    if (gameObject.layer != PhysicsLayer.Players)
      return;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        GameObject mainObject = ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
        if (!Object.op_Inequality((Object) mainObject, (Object) null) || !Object.op_Equality((Object) mainObject, (Object) gameObject))
          break;
        this.isCollide = true;
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!gameObject.GetPhotonView().isMine)
          break;
        this.isCollide = true;
        break;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (!this.isCollide)
      return;
    GameObject gameObject = ((Component) ((Component) other).transform.root).gameObject;
    if (gameObject.layer != PhysicsLayer.Players)
      return;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        GameObject mainObject = ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
        if (!Object.op_Inequality((Object) mainObject, (Object) null) || !Object.op_Equality((Object) mainObject, (Object) gameObject))
          break;
        this.isCollide = false;
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!gameObject.GetPhotonView().isMine)
          break;
        this.isCollide = false;
        break;
    }
  }
}
