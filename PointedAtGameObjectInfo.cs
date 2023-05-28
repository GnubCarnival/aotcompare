// Decompiled with JetBrains decompiler
// Type: PointedAtGameObjectInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
  private void OnGUI()
  {
    if (!Object.op_Inequality((Object) InputToEvent.goPointedAt, (Object) null))
      return;
    PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    GUI.Label(new Rect(Input.mousePosition.x + 5f, (float) ((double) Screen.height - (double) Input.mousePosition.y - 15.0), 300f, 30f), string.Format("ViewID {0} InstID {1} Lvl {2} {3}", (object) photonView.viewID, (object) photonView.instantiationId, (object) photonView.prefix, !photonView.isSceneView ? (!photonView.isMine ? (object) ("owner: " + photonView.ownerId.ToString()) : (object) "mine") : (object) "scene"));
  }
}
