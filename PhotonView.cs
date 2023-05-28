// Decompiled with JetBrains decompiler
// Type: PhotonView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Photon View &v")]
public class PhotonView : MonoBehaviour
{
  protected internal bool destroyedByPhotonNetworkOrQuit;
  protected internal bool didAwake;
  private bool failedToFindOnSerialize;
  public int group;
  private object[] instantiationDataField;
  public int instantiationId;
  protected internal object[] lastOnSerializeDataReceived;
  protected internal object[] lastOnSerializeDataSent;
  protected internal bool mixedModeIsReliable;
  public Component observed;
  private MethodInfo OnSerializeMethodInfo;
  public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;
  public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
  public int ownerId;
  public int prefixBackup = -1;
  public int subId;
  public ViewSynchronization synchronization;

  protected internal void Awake()
  {
    PhotonNetwork.networkingPeer.RegisterPhotonView(this);
    this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
    this.didAwake = true;
  }

  protected internal void ExecuteOnSerialize(PhotonStream pStream, PhotonMessageInfo info)
  {
    if (this.failedToFindOnSerialize)
      return;
    if (this.OnSerializeMethodInfo == null && !NetworkingPeer.GetMethod(this.observed as MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out this.OnSerializeMethodInfo))
    {
      Debug.LogError((object) ("The observed monobehaviour (" + ((Object) this.observed).name + ") of this PhotonView does not implement OnPhotonSerializeView()!"));
      this.failedToFindOnSerialize = true;
    }
    else
      this.OnSerializeMethodInfo.Invoke((object) this.observed, new object[2]
      {
        (object) pStream,
        (object) info
      });
  }

  public static PhotonView Find(int viewID) => PhotonNetwork.networkingPeer.GetPhotonView(viewID);

  public static PhotonView Get(Component component) => component.GetComponent<PhotonView>();

  public static PhotonView Get(GameObject gameObj) => gameObj.GetComponent<PhotonView>();

  protected internal void OnApplicationQuit() => this.destroyedByPhotonNetworkOrQuit = true;

  protected internal void OnDestroy()
  {
    if (!this.destroyedByPhotonNetworkOrQuit)
      PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
    if (!this.destroyedByPhotonNetworkOrQuit && !Application.isLoadingLevel)
    {
      if (this.instantiationId > 0)
        Debug.LogError((object) ("OnDestroy() seems to be called without PhotonNetwork.Destroy()?! GameObject: " + (object) ((Component) this).gameObject + " Application.isLoadingLevel: " + (object) Application.isLoadingLevel));
      else if (this.viewID <= 0)
        Debug.LogWarning((object) string.Format("OnDestroy manually allocated PhotonView {0}. The viewID is 0. Was it ever (manually) set?", (object) this));
      else if (this.isMine && !PhotonNetwork.manuallyAllocatedViewIds.Contains(this.viewID))
        Debug.LogWarning((object) string.Format("OnDestroy manually allocated PhotonView {0}. The viewID is local (isMine) but not in manuallyAllocatedViewIds list. Use UnAllocateViewID() after you destroyed the PV.", (object) this));
    }
    if (!PhotonNetwork.networkingPeer.instantiatedObjects.ContainsKey(this.instantiationId))
      return;
    bool flag = Object.op_Equality((Object) PhotonNetwork.networkingPeer.instantiatedObjects[this.instantiationId], (Object) ((Component) this).gameObject);
    if (!flag)
      return;
    Debug.LogWarning((object) string.Format("OnDestroy for PhotonView {0} but GO is still in instantiatedObjects. instantiationId: {1}. Use PhotonNetwork.Destroy(). {2} Identical with this: {3} PN.Destroyed called for this PV: {4}", (object) this, (object) this.instantiationId, !Application.isLoadingLevel ? (object) string.Empty : (object) "Loading new scene caused this.", (object) flag, (object) this.destroyedByPhotonNetworkOrQuit));
  }

  public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters) => PhotonNetwork.RPC(this, methodName, targetPlayer, parameters);

  public void RPC(string methodName, PhotonTargets target, params object[] parameters)
  {
    if (PhotonNetwork.networkingPeer.hasSwitchedMC && target == PhotonTargets.MasterClient)
      PhotonNetwork.RPC(this, methodName, PhotonNetwork.masterClient, parameters);
    else
      PhotonNetwork.RPC(this, methodName, target, parameters);
  }

  public virtual string ToString() => string.Format("View ({3}){0} on {1} {2}", (object) this.viewID, Object.op_Equality((Object) ((Component) this).gameObject, (Object) null) ? (object) "GO==null" : (object) ((Object) ((Component) this).gameObject).name, !this.isSceneView ? (object) string.Empty : (object) "(scene)", (object) this.prefix);

  public object[] instantiationData
  {
    get
    {
      if (!this.didAwake)
        this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
      return this.instantiationDataField;
    }
    set => this.instantiationDataField = value;
  }

  public bool isMine
  {
    get
    {
      if (this.ownerId == PhotonNetwork.player.ID)
        return true;
      return this.isSceneView && PhotonNetwork.isMasterClient;
    }
  }

  public bool isSceneView => this.ownerId == 0;

  public PhotonPlayer owner => PhotonPlayer.Find(this.ownerId);

  public int OwnerActorNr => this.ownerId;

  public int prefix
  {
    get
    {
      if (this.prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
        this.prefixBackup = (int) PhotonNetwork.networkingPeer.currentLevelPrefix;
      return this.prefixBackup;
    }
    set => this.prefixBackup = value;
  }

  public int viewID
  {
    get => this.ownerId * PhotonNetwork.MAX_VIEW_IDS + this.subId;
    set
    {
      int num = !this.didAwake ? 0 : (this.subId == 0 ? 1 : 0);
      this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
      this.subId = value % PhotonNetwork.MAX_VIEW_IDS;
      if (num == 0)
        return;
      PhotonNetwork.networkingPeer.RegisterPhotonView(this);
    }
  }
}
