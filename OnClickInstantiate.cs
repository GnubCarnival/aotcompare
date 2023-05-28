// Decompiled with JetBrains decompiler
// Type: OnClickInstantiate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class OnClickInstantiate : MonoBehaviour
{
  public int InstantiateType;
  private string[] InstantiateTypeNames = new string[2]
  {
    "Mine",
    "Scene"
  };
  public GameObject Prefab;
  public bool showGui;

  private void OnClick()
  {
    if (PhotonNetwork.connectionStateDetailed != PeerStates.Joined)
      return;
    switch (this.InstantiateType)
    {
      case 0:
        PhotonNetwork.Instantiate(((Object) this.Prefab).name, Vector3.op_Addition(InputToEvent.inputHitPos, new Vector3(0.0f, 5f, 0.0f)), Quaternion.identity, 0);
        break;
      case 1:
        PhotonNetwork.InstantiateSceneObject(((Object) this.Prefab).name, Vector3.op_Addition(InputToEvent.inputHitPos, new Vector3(0.0f, 5f, 0.0f)), Quaternion.identity, 0, (object[]) null);
        break;
    }
  }

  private void OnGUI()
  {
    if (!this.showGui)
      return;
    GUILayout.BeginArea(new Rect((float) (Screen.width - 180), 0.0f, 180f, 50f));
    this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, new GUILayoutOption[0]);
    GUILayout.EndArea();
  }
}
