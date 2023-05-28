// Decompiled with JetBrains decompiler
// Type: ManualPhotonViewAllocator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
  public GameObject Prefab;

  public void AllocateManualPhotonView()
  {
    PhotonView photonView = ((Component) this).gameObject.GetPhotonView();
    if (Object.op_Equality((Object) photonView, (Object) null))
    {
      Debug.LogError((object) "Can't do manual instantiation without PhotonView component.");
    }
    else
    {
      object[] objArray = new object[1]
      {
        (object) PhotonNetwork.AllocateViewID()
      };
      photonView.RPC("InstantiateRpc", PhotonTargets.AllBuffered, objArray);
    }
  }

  [RPC]
  public void InstantiateRpc(int viewID)
  {
    GameObject go = Object.Instantiate((Object) this.Prefab, Vector3.op_Addition(InputToEvent.inputHitPos, new Vector3(0.0f, 5f, 0.0f)), Quaternion.identity) as GameObject;
    go.GetPhotonView().viewID = viewID;
    go.GetComponent<OnClickDestroy>().DestroyByRpc = true;
  }
}
