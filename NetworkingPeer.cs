// Decompiled with JetBrains decompiler
// Type: NetworkingPeer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

internal class NetworkingPeer : LoadbalancingPeer, IPhotonPeerListener
{
  private HashSet<int> allowedReceivingGroups;
  private HashSet<int> blockSendingGroups;
  protected internal short currentLevelPrefix;
  protected internal const string CurrentSceneProperty = "curScn";
  private bool didAuthenticate;
  private IPhotonPeerListener externalListener;
  private string[] friendListRequested;
  private int friendListTimestamp;
  public bool hasSwitchedMC;
  public bool insideLobby;
  public Dictionary<int, GameObject> instantiatedObjects;
  private bool isFetchingFriends;
  public bool IsInitialConnect;
  protected internal bool loadingLevelAndPausedNetwork;
  public Dictionary<int, PhotonPlayer> mActors;
  protected internal string mAppId;
  protected internal string mAppVersion;
  public Dictionary<string, RoomInfo> mGameList;
  public RoomInfo[] mGameListCopy;
  private JoinType mLastJoinType;
  public PhotonPlayer mMasterClient;
  private Dictionary<System.Type, List<MethodInfo>> monoRPCMethodsCache;
  public PhotonPlayer[] mOtherPlayerListCopy;
  public PhotonPlayer[] mPlayerListCopy;
  private bool mPlayernameHasToBeUpdated;
  public string NameServerAddress;
  protected internal Dictionary<int, PhotonView> photonViewList;
  private string playername;
  public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
  private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort;
  public bool requestSecurity;
  private readonly Dictionary<string, int> rpcShortcuts;
  private Dictionary<int, object[]> tempInstantiationData;
  public static bool UsePrefabCache = true;

  static NetworkingPeer() => NetworkingPeer.ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>()
  {
    {
      (ConnectionProtocol) 0,
      5055
    },
    {
      (ConnectionProtocol) 1,
      4530
    }
  };

  public NetworkingPeer(
    IPhotonPeerListener listener,
    string playername,
    ConnectionProtocol connectionProtocol)
    : base(listener, connectionProtocol)
  {
    this.playername = string.Empty;
    this.mActors = new Dictionary<int, PhotonPlayer>();
    this.mOtherPlayerListCopy = new PhotonPlayer[0];
    this.mPlayerListCopy = new PhotonPlayer[0];
    this.requestSecurity = true;
    this.monoRPCMethodsCache = new Dictionary<System.Type, List<MethodInfo>>();
    this.mGameList = new Dictionary<string, RoomInfo>();
    this.mGameListCopy = new RoomInfo[0];
    this.instantiatedObjects = new Dictionary<int, GameObject>();
    this.allowedReceivingGroups = new HashSet<int>();
    this.blockSendingGroups = new HashSet<int>();
    this.photonViewList = new Dictionary<int, PhotonView>();
    this.NameServerAddress = "ns.exitgamescloud.com";
    this.tempInstantiationData = new Dictionary<int, object[]>();
    this.Listener = (IPhotonPeerListener) this;
    this.lobby = TypedLobby.Default;
    this.LimitOfUnreliableCommands = 40;
    this.externalListener = listener;
    this.PlayerName = playername;
    this.mLocalActor = new PhotonPlayer(true, -1, this.playername);
    this.AddNewPlayer(this.mLocalActor.ID, this.mLocalActor);
    this.rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
    for (int index = 0; index < PhotonNetwork.PhotonServerSettings.RpcList.Count; ++index)
      this.rpcShortcuts[PhotonNetwork.PhotonServerSettings.RpcList[index]] = index;
    this.State = PeerStates.PeerCreated;
    this.SerializationProtocolType = (SerializationProtocol) 0;
  }

  private void AddNewPlayer(int ID, PhotonPlayer player)
  {
    if (!this.mActors.ContainsKey(ID))
    {
      this.mActors[ID] = player;
      this.RebuildPlayerListCopies();
    }
    else
      Debug.LogError((object) ("Adding player twice: " + ID.ToString()));
  }

  private bool AlmostEquals(object[] lastData, object[] currentContent)
  {
    if (lastData != null || currentContent != null)
    {
      if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
        return false;
      for (int index = 0; index < currentContent.Length; ++index)
      {
        if (!this.ObjectIsSameWithInprecision(currentContent[index], lastData[index]))
          return false;
      }
    }
    return true;
  }

  public void ChangeLocalID(int newID)
  {
    if (this.mLocalActor == null)
      Debug.LogWarning((object) string.Format("Local actor is null or not in mActors! mLocalActor: {0} mActors==null: {1} newID: {2}", (object) this.mLocalActor, (object) (this.mActors == null), (object) newID));
    if (this.mActors.ContainsKey(this.mLocalActor.ID))
      this.mActors.Remove(this.mLocalActor.ID);
    this.mLocalActor.InternalChangeLocalID(newID);
    this.mActors[this.mLocalActor.ID] = this.mLocalActor;
    this.RebuildPlayerListCopies();
  }

  public void checkLAN()
  {
    if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.Cloud || this.MasterServerAddress == null || !(this.MasterServerAddress != string.Empty) || this.mGameserver == null || !(this.mGameserver != string.Empty) || !this.MasterServerAddress.Contains(":") || !this.mGameserver.Contains(":"))
      return;
    this.mGameserver = this.MasterServerAddress.Split(':')[0] + ":" + this.mGameserver.Split(':')[1];
  }

  private void CheckMasterClient(int leavingPlayerId)
  {
    bool flag1 = this.mMasterClient != null && this.mMasterClient.ID == leavingPlayerId;
    bool flag2 = leavingPlayerId > 0;
    if (!(!flag2 | flag1))
      return;
    if (this.mActors.Count <= 1)
    {
      this.mMasterClient = this.mLocalActor;
    }
    else
    {
      int key1 = int.MaxValue;
      foreach (int key2 in this.mActors.Keys)
      {
        if (key2 < key1 && key2 != leavingPlayerId)
          key1 = key2;
      }
      this.mMasterClient = this.mActors[key1];
    }
    if (!flag2)
      return;
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, (object) this.mMasterClient);
  }

  private bool CheckTypeMatch(ParameterInfo[] methodParameters, System.Type[] callParameterTypes)
  {
    if (methodParameters.Length < callParameterTypes.Length)
      return false;
    for (int index = 0; index < callParameterTypes.Length; ++index)
    {
      System.Type parameterType = methodParameters[index].ParameterType;
      if (callParameterTypes[index] != null && !parameterType.Equals(callParameterTypes[index]))
        return false;
    }
    return true;
  }

  public void CleanRpcBufferIfMine(PhotonView view)
  {
    if (view.ownerId != this.mLocalActor.ID && !this.mLocalActor.isMasterClient)
      Debug.LogError((object) ("Cannot remove cached RPCs on a PhotonView thats not ours! " + (object) view.owner + " scene: " + (object) view.isSceneView));
    else
      this.OpCleanRpcBuffer(view);
  }

  public bool Connect(string serverAddress, ServerConnection type)
  {
    if (PhotonHandler.AppQuits)
    {
      Debug.LogWarning((object) "Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
      return false;
    }
    if (PhotonNetwork.connectionStateDetailed == PeerStates.Disconnecting)
    {
      Debug.LogError((object) ("Connect() failed. Can't connect while disconnecting (still). Current state: " + PhotonNetwork.connectionStateDetailed.ToString()));
      return false;
    }
    bool flag = base.Connect(serverAddress, string.Empty);
    if (flag)
    {
      switch (type)
      {
        case ServerConnection.MasterServer:
          this.State = PeerStates.ConnectingToMasterserver;
          return flag;
        case ServerConnection.GameServer:
          this.State = PeerStates.ConnectingToGameserver;
          return flag;
        case ServerConnection.NameServer:
          this.State = PeerStates.ConnectingToNameServer;
          return flag;
      }
    }
    return flag;
  }

  public virtual bool Connect(string serverAddress, string applicationName)
  {
    Debug.LogError((object) "Avoid using this directly. Thanks.");
    return false;
  }

  public bool ConnectToNameServer()
  {
    if (PhotonHandler.AppQuits)
    {
      Debug.LogWarning((object) "Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
      return false;
    }
    this.IsUsingNameServer = true;
    this.CloudRegion = CloudRegionCode.none;
    if (this.State != PeerStates.ConnectedToNameServer)
    {
      string str = this.NameServerAddress;
      if (!str.Contains(":"))
      {
        int num = 0;
        NetworkingPeer.ProtocolToNameServerPort.TryGetValue(this.UsedProtocol, out num);
        str = string.Format("{0}:{1}", (object) str, (object) num);
        Debug.Log((object) ("Server to connect to: " + str + " settings protocol: " + (object) PhotonNetwork.PhotonServerSettings.Protocol));
      }
      if (!base.Connect(str, "ns"))
        return false;
      this.State = PeerStates.ConnectingToNameServer;
    }
    return true;
  }

  public bool ConnectToRegionMaster(CloudRegionCode region)
  {
    if (PhotonHandler.AppQuits)
    {
      Debug.LogWarning((object) "Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
      return false;
    }
    this.IsUsingNameServer = true;
    this.CloudRegion = region;
    if (this.State == PeerStates.ConnectedToNameServer)
      return this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, region.ToString());
    string str = this.NameServerAddress;
    if (!str.Contains(":"))
    {
      int num = 0;
      NetworkingPeer.ProtocolToNameServerPort.TryGetValue(this.UsedProtocol, out num);
      str = string.Format("{0}:{1}", (object) str, (object) num);
    }
    if (!base.Connect(str, "ns"))
      return false;
    this.State = PeerStates.ConnectingToNameServer;
    return true;
  }

  public void DebugReturn(DebugLevel level, string message) => this.externalListener.DebugReturn(level, message);

  private bool DeltaCompressionRead(PhotonView view, Hashtable data)
  {
    if (!((Dictionary<object, object>) data).ContainsKey((object) (byte) 1))
    {
      if (view.lastOnSerializeDataReceived == null || !(data[(object) (byte) 2] is object[] objArray))
        return false;
      if (!(data[(object) (byte) 3] is int[] target))
        target = new int[0];
      object[] serializeDataReceived = view.lastOnSerializeDataReceived;
      for (int nr = 0; nr < objArray.Length; ++nr)
      {
        if (objArray[nr] == null && !Extensions.Contains(target, nr))
          objArray[nr] = serializeDataReceived[nr];
      }
      data[(object) (byte) 1] = (object) objArray;
    }
    return true;
  }

  private bool DeltaCompressionWrite(PhotonView view, Hashtable data)
  {
    if (view.lastOnSerializeDataSent != null)
    {
      object[] serializeDataSent = view.lastOnSerializeDataSent;
      if (!(data[(object) (byte) 1] is object[] objArray1))
        return false;
      if (serializeDataSent.Length != objArray1.Length)
        return true;
      object[] objArray2 = new object[objArray1.Length];
      int num = 0;
      List<int> intList = new List<int>();
      for (int index = 0; index < objArray2.Length; ++index)
      {
        object one = objArray1[index];
        object two = serializeDataSent[index];
        if (this.ObjectIsSameWithInprecision(one, two))
        {
          ++num;
        }
        else
        {
          objArray2[index] = objArray1[index];
          if (one == null)
            intList.Add(index);
        }
      }
      if (num > 0)
      {
        ((Dictionary<object, object>) data).Remove((object) (byte) 1);
        if (num == objArray1.Length)
          return false;
        data[(object) (byte) 2] = (object) objArray2;
        if (intList.Count > 0)
          data[(object) (byte) 3] = (object) intList.ToArray();
      }
    }
    return true;
  }

  public void DestroyAll(bool localOnly)
  {
    if (!localOnly)
    {
      this.OpRemoveCompleteCache();
      this.SendDestroyOfAll();
    }
    this.LocalCleanupAnythingInstantiated(true);
  }

  public void DestroyPlayerObjects(int playerId, bool localOnly)
  {
    if (playerId <= 0)
    {
      Debug.LogError((object) ("Failed to Destroy objects of playerId: " + playerId.ToString()));
    }
    else
    {
      if (!localOnly)
      {
        this.OpRemoveFromServerInstantiationsOfPlayer(playerId);
        this.OpCleanRpcBuffer(playerId);
        this.SendDestroyOfPlayer(playerId);
      }
      Queue<GameObject> gameObjectQueue = new Queue<GameObject>();
      int num1 = playerId * PhotonNetwork.MAX_VIEW_IDS;
      int num2 = num1 + PhotonNetwork.MAX_VIEW_IDS;
      foreach (KeyValuePair<int, GameObject> instantiatedObject in this.instantiatedObjects)
      {
        if (instantiatedObject.Key > num1 && instantiatedObject.Key < num2)
          gameObjectQueue.Enqueue(instantiatedObject.Value);
      }
      foreach (GameObject go in gameObjectQueue)
        this.RemoveInstantiatedGO(go, true);
    }
  }

  public virtual void Disconnect()
  {
    if (this.PeerState == null)
    {
      if (PhotonHandler.AppQuits)
        return;
      Debug.LogWarning((object) string.Format("Can't execute Disconnect() while not connected. Nothing changed. State: {0}", (object) this.State));
    }
    else
    {
      this.State = PeerStates.Disconnecting;
      base.Disconnect();
    }
  }

  private void DisconnectToReconnect()
  {
    switch (this.server)
    {
      case ServerConnection.MasterServer:
        this.State = PeerStates.DisconnectingFromMasterserver;
        base.Disconnect();
        break;
      case ServerConnection.GameServer:
        this.State = PeerStates.DisconnectingFromGameserver;
        base.Disconnect();
        break;
      case ServerConnection.NameServer:
        this.State = PeerStates.DisconnectingFromNameServer;
        base.Disconnect();
        break;
    }
  }

  private void DisconnectToReconnect2()
  {
    this.checkLAN();
    switch (this.server)
    {
      case ServerConnection.MasterServer:
        this.State = FengGameManagerMKII.returnPeerState(2);
        base.Disconnect();
        break;
      case ServerConnection.GameServer:
        this.State = FengGameManagerMKII.returnPeerState(3);
        base.Disconnect();
        break;
      case ServerConnection.NameServer:
        this.State = FengGameManagerMKII.returnPeerState(4);
        base.Disconnect();
        break;
    }
  }

  internal GameObject DoInstantiate(
    Hashtable evData,
    PhotonPlayer photonPlayer,
    GameObject resourceGameObject)
  {
    string key = (string) evData[(object) (byte) 0];
    int timestamp = (int) evData[(object) (byte) 6];
    int num1 = (int) evData[(object) (byte) 7];
    Vector3 vector3 = !((Dictionary<object, object>) evData).ContainsKey((object) (byte) 1) ? Vector3.zero : (Vector3) evData[(object) (byte) 1];
    Quaternion identity = Quaternion.identity;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 2))
      identity = (Quaternion) evData[(object) (byte) 2];
    int num2 = 0;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 3))
      num2 = (int) evData[(object) (byte) 3];
    short num3 = 0;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 8))
      num3 = (short) evData[(object) (byte) 8];
    int[] viewIDS;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 4))
      viewIDS = (int[]) evData[(object) (byte) 4];
    else
      viewIDS = new int[1]{ num1 };
    if (!InstantiateTracker.instance.checkObj(key, photonPlayer, viewIDS))
      return (GameObject) null;
    object[] instantiationData = !((Dictionary<object, object>) evData).ContainsKey((object) (byte) 5) ? (object[]) null : (object[]) evData[(object) (byte) 5];
    if (num2 != 0 && !this.allowedReceivingGroups.Contains(num2))
      return (GameObject) null;
    if (Object.op_Equality((Object) resourceGameObject, (Object) null))
    {
      if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(key, out resourceGameObject))
      {
        resourceGameObject = (GameObject) Resources.Load(key, typeof (GameObject));
        if (NetworkingPeer.UsePrefabCache)
          NetworkingPeer.PrefabCache.Add(key, resourceGameObject);
      }
      if (Object.op_Equality((Object) resourceGameObject, (Object) null))
      {
        Debug.LogError((object) ("PhotonNetwork error: Could not Instantiate the prefab [" + key + "]. Please verify you have this gameobject in a Resources folder."));
        return (GameObject) null;
      }
    }
    PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
    if (photonViewsInChildren.Length != viewIDS.Length)
      throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
    for (int index = 0; index < viewIDS.Length; ++index)
    {
      photonViewsInChildren[index].viewID = viewIDS[index];
      photonViewsInChildren[index].prefix = (int) num3;
      photonViewsInChildren[index].instantiationId = num1;
    }
    this.StoreInstantiationData(num1, instantiationData);
    GameObject gameObject = (GameObject) Object.Instantiate((Object) resourceGameObject, vector3, identity);
    for (int index = 0; index < viewIDS.Length; ++index)
    {
      photonViewsInChildren[index].viewID = 0;
      photonViewsInChildren[index].prefix = -1;
      photonViewsInChildren[index].prefixBackup = -1;
      photonViewsInChildren[index].instantiationId = -1;
    }
    this.RemoveInstantiationData(num1);
    if (this.instantiatedObjects.ContainsKey(num1))
    {
      GameObject instantiatedObject = this.instantiatedObjects[num1];
      string str = string.Empty;
      if (Object.op_Inequality((Object) instantiatedObject, (Object) null))
      {
        foreach (PhotonView photonViewsInChild in instantiatedObject.GetPhotonViewsInChildren())
        {
          if (Object.op_Inequality((Object) photonViewsInChild, (Object) null))
            str = str + ((object) photonViewsInChild).ToString() + ", ";
        }
      }
      Debug.LogError((object) string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", (object) gameObject, (object) num1, (object) this.instantiatedObjects.Count, (object) instantiatedObject, (object) str, (object) PhotonNetwork.lastUsedViewSubId, (object) PhotonNetwork.lastUsedViewSubIdStatic, (object) this.photonViewList.Count));
      this.RemoveInstantiatedGO(instantiatedObject, true);
    }
    this.instantiatedObjects.Add(num1, gameObject);
    gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), (object) new PhotonMessageInfo(photonPlayer, timestamp, (PhotonView) null), (SendMessageOptions) 1);
    return gameObject;
  }

  internal GameObject DoInstantiate2(
    Hashtable evData,
    PhotonPlayer photonPlayer,
    GameObject resourceGameObject)
  {
    string key = (string) evData[(object) (byte) 0];
    int timestamp = (int) evData[(object) (byte) 6];
    int num1 = (int) evData[(object) (byte) 7];
    Vector3 vector3 = !((Dictionary<object, object>) evData).ContainsKey((object) (byte) 1) ? Vector3.zero : (Vector3) evData[(object) (byte) 1];
    Quaternion identity = Quaternion.identity;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 2))
      identity = (Quaternion) evData[(object) (byte) 2];
    int num2 = 0;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 3))
      num2 = (int) evData[(object) (byte) 3];
    short num3 = 0;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 8))
      num3 = (short) evData[(object) (byte) 8];
    int[] viewIDS;
    if (((Dictionary<object, object>) evData).ContainsKey((object) (byte) 4))
      viewIDS = (int[]) evData[(object) (byte) 4];
    else
      viewIDS = new int[1]{ num1 };
    if (!InstantiateTracker.instance.checkObj(key, photonPlayer, viewIDS))
      return (GameObject) null;
    object[] instantiationData = !((Dictionary<object, object>) evData).ContainsKey((object) (byte) 5) ? (object[]) null : (object[]) evData[(object) (byte) 5];
    if (num2 != 0 && !this.allowedReceivingGroups.Contains(num2))
      return (GameObject) null;
    if (Object.op_Equality((Object) resourceGameObject, (Object) null))
    {
      if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(key, out resourceGameObject))
      {
        resourceGameObject = !key.StartsWith("RCAsset/") ? (GameObject) Resources.Load(key, typeof (GameObject)) : FengGameManagerMKII.InstantiateCustomAsset(key);
        if (NetworkingPeer.UsePrefabCache)
          NetworkingPeer.PrefabCache.Add(key, resourceGameObject);
      }
      if (Object.op_Equality((Object) resourceGameObject, (Object) null))
      {
        Debug.LogError((object) ("PhotonNetwork error: Could not Instantiate the prefab [" + key + "]. Please verify you have this gameobject in a Resources folder."));
        return (GameObject) null;
      }
    }
    PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
    if (photonViewsInChildren.Length != viewIDS.Length)
      throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
    for (int index = 0; index < viewIDS.Length; ++index)
    {
      photonViewsInChildren[index].viewID = viewIDS[index];
      photonViewsInChildren[index].prefix = (int) num3;
      photonViewsInChildren[index].instantiationId = num1;
    }
    this.StoreInstantiationData(num1, instantiationData);
    GameObject gameObject = (GameObject) Object.Instantiate((Object) resourceGameObject, vector3, identity);
    for (int index = 0; index < viewIDS.Length; ++index)
    {
      photonViewsInChildren[index].viewID = 0;
      photonViewsInChildren[index].prefix = -1;
      photonViewsInChildren[index].prefixBackup = -1;
      photonViewsInChildren[index].instantiationId = -1;
    }
    this.RemoveInstantiationData(num1);
    if (this.instantiatedObjects.ContainsKey(num1))
    {
      GameObject instantiatedObject = this.instantiatedObjects[num1];
      string str = string.Empty;
      if (Object.op_Inequality((Object) instantiatedObject, (Object) null))
      {
        foreach (PhotonView photonViewsInChild in instantiatedObject.GetPhotonViewsInChildren())
        {
          if (Object.op_Inequality((Object) photonViewsInChild, (Object) null))
            str = str + ((object) photonViewsInChild).ToString() + ", ";
        }
      }
      Debug.LogError((object) string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", (object) gameObject, (object) num1, (object) this.instantiatedObjects.Count, (object) instantiatedObject, (object) str, (object) PhotonNetwork.lastUsedViewSubId, (object) PhotonNetwork.lastUsedViewSubIdStatic, (object) this.photonViewList.Count));
      this.RemoveInstantiatedGO(instantiatedObject, true);
    }
    this.instantiatedObjects.Add(num1, gameObject);
    gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), (object) new PhotonMessageInfo(photonPlayer, timestamp, (PhotonView) null), (SendMessageOptions) 1);
    return gameObject;
  }

  public void ExecuteRPC(Hashtable rpcData, PhotonPlayer sender)
  {
    if (rpcData == null || !((Dictionary<object, object>) rpcData).ContainsKey((object) (byte) 0))
    {
      Debug.LogError((object) ("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString((IDictionary) rpcData)));
    }
    else
    {
      int viewID = (int) rpcData[(object) (byte) 0];
      int num1 = 0;
      if (((Dictionary<object, object>) rpcData).ContainsKey((object) (byte) 1))
        num1 = (int) (short) rpcData[(object) (byte) 1];
      string rpc;
      if (((Dictionary<object, object>) rpcData).ContainsKey((object) (byte) 5))
      {
        int index = (int) (byte) rpcData[(object) (byte) 5];
        if (index > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
        {
          Debug.LogError((object) ("Could not find RPC with index: " + index.ToString() + ". Going to ignore! Check PhotonServerSettings.RpcList"));
          return;
        }
        rpc = PhotonNetwork.PhotonServerSettings.RpcList[index];
      }
      else
        rpc = (string) rpcData[(object) (byte) 3];
      object[] parameters1 = (object[]) null;
      if (((Dictionary<object, object>) rpcData).ContainsKey((object) (byte) 4))
        parameters1 = (object[]) rpcData[(object) (byte) 4];
      if (parameters1 == null)
        parameters1 = new object[0];
      PhotonView photonView = this.GetPhotonView(viewID);
      if (Object.op_Equality((Object) photonView, (Object) null))
      {
        int num2 = viewID / PhotonNetwork.MAX_VIEW_IDS;
        bool flag1 = num2 == this.mLocalActor.ID;
        bool flag2 = num2 == sender.ID;
        if (flag1)
          Debug.LogWarning((object) ("Received RPC \"" + rpc + "\" for viewID " + (object) viewID + " but this PhotonView does not exist! View was/is ours." + (!flag2 ? (object) " Remote called." : (object) " Owner called.")));
        else
          Debug.LogError((object) ("Received RPC \"" + rpc + "\" for viewID " + (object) viewID + " but this PhotonView does not exist! Was remote PV." + (!flag2 ? (object) " Remote called." : (object) " Owner called.")));
      }
      else if (photonView.prefix != num1)
        Debug.LogError((object) ("Received RPC \"" + rpc + "\" on viewID " + (object) viewID + " with a prefix of " + (object) num1 + ", our prefix is " + (object) photonView.prefix + ". The RPC has been ignored."));
      else if (rpc == string.Empty)
      {
        Debug.LogError((object) ("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString((IDictionary) rpcData)));
      }
      else
      {
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
          Debug.Log((object) ("Received RPC: " + rpc));
        if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
          return;
        System.Type[] callParameterTypes = new System.Type[0];
        if (parameters1.Length != 0)
        {
          callParameterTypes = new System.Type[parameters1.Length];
          int index1 = 0;
          for (int index2 = 0; index2 < parameters1.Length; ++index2)
          {
            object obj = parameters1[index2];
            callParameterTypes[index1] = obj != null ? obj.GetType() : (System.Type) null;
            ++index1;
          }
        }
        int num3 = 0;
        int num4 = 0;
        foreach (MonoBehaviour component in ((Component) photonView).GetComponents<MonoBehaviour>())
        {
          if (Object.op_Equality((Object) component, (Object) null))
          {
            Debug.LogError((object) "ERROR You have missing MonoBehaviours on your gameobjects!");
          }
          else
          {
            System.Type type = component.GetType();
            List<MethodInfo> methodInfoList = (List<MethodInfo>) null;
            if (this.monoRPCMethodsCache.ContainsKey(type))
              methodInfoList = this.monoRPCMethodsCache[type];
            if (methodInfoList == null)
            {
              List<MethodInfo> methods = SupportClass.GetMethods(type, typeof (UnityEngine.RPC));
              this.monoRPCMethodsCache[type] = methods;
              methodInfoList = methods;
            }
            if (methodInfoList != null)
            {
              for (int index = 0; index < methodInfoList.Count; ++index)
              {
                MethodInfo methodInfo = methodInfoList[index];
                if (methodInfo.Name == rpc)
                {
                  ++num4;
                  ParameterInfo[] parameters2 = methodInfo.GetParameters();
                  if (parameters2.Length == callParameterTypes.Length)
                  {
                    if (this.CheckTypeMatch(parameters2, callParameterTypes))
                    {
                      ++num3;
                      object obj = methodInfo.Invoke((object) component, parameters1);
                      if (methodInfo.ReturnType == typeof (IEnumerator))
                        component.StartCoroutine((IEnumerator) obj);
                    }
                  }
                  else if (parameters2.Length - 1 == callParameterTypes.Length)
                  {
                    if (this.CheckTypeMatch(parameters2, callParameterTypes) && parameters2[parameters2.Length - 1].ParameterType == typeof (PhotonMessageInfo))
                    {
                      ++num3;
                      int timestamp = (int) rpcData[(object) (byte) 2];
                      object[] parameters3 = new object[parameters1.Length + 1];
                      parameters1.CopyTo((Array) parameters3, 0);
                      parameters3[parameters3.Length - 1] = (object) new PhotonMessageInfo(sender, timestamp, photonView);
                      object obj = methodInfo.Invoke((object) component, parameters3);
                      if (methodInfo.ReturnType == typeof (IEnumerator))
                        component.StartCoroutine((IEnumerator) obj);
                    }
                  }
                  else if (parameters2.Length == 1 && parameters2[0].ParameterType.IsArray)
                  {
                    ++num3;
                    object[] parameters4 = new object[1]
                    {
                      (object) parameters1
                    };
                    object obj = methodInfo.Invoke((object) component, parameters4);
                    if (methodInfo.ReturnType == typeof (IEnumerator))
                      component.StartCoroutine((IEnumerator) obj);
                  }
                }
              }
            }
          }
        }
        if (num3 == 1)
          return;
        string str = string.Empty;
        for (int index = 0; index < callParameterTypes.Length; ++index)
        {
          System.Type type = callParameterTypes[index];
          if (str != string.Empty)
            str += ", ";
          str = type != null ? str + type.Name : str + "null";
        }
        if (num3 == 0)
        {
          if (num4 == 0)
            Debug.LogError((object) ("PhotonView with ID " + (object) viewID + " has no method \"" + rpc + "\" marked with the [RPC](C#) or @RPC(JS) property! Args: " + str));
          else
            Debug.LogError((object) ("PhotonView with ID " + (object) viewID + " has no method \"" + rpc + "\" that takes " + (object) callParameterTypes.Length + " argument(s): " + str));
        }
        else
          Debug.LogError((object) ("PhotonView with ID " + (object) viewID + " has " + (object) num3 + " methods \"" + rpc + "\" that takes " + (object) callParameterTypes.Length + " argument(s): " + str + ". Should be just one?"));
      }
    }
  }

  public object[] FetchInstantiationData(int instantiationId)
  {
    object[] objArray = (object[]) null;
    if (instantiationId == 0)
      return (object[]) null;
    this.tempInstantiationData.TryGetValue(instantiationId, out objArray);
    return objArray;
  }

  private void GameEnteredOnGameServer(OperationResponse operationResponse)
  {
    if (operationResponse.ReturnCode == (short) 0)
    {
      this.State = PeerStates.Joined;
      this.mRoomToGetInto.isLocalClientInside = true;
      Hashtable pActorProperties = (Hashtable) operationResponse[(byte) 249];
      this.ReadoutProperties((Hashtable) operationResponse[(byte) 248], pActorProperties, 0);
      this.ChangeLocalID((int) operationResponse[(byte) 254]);
      this.CheckMasterClient(-1);
      if (this.mPlayernameHasToBeUpdated)
        this.SendPlayerName();
      if (operationResponse.OperationCode != (byte) 227)
        return;
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
    }
    else
    {
      switch (operationResponse.OperationCode)
      {
        case 225:
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
          {
            Debug.Log((object) ("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage));
            if (operationResponse.ReturnCode == (short) 32758)
              Debug.Log((object) "Most likely the game became empty during the switch to GameServer.");
          }
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, (object) operationResponse.ReturnCode, (object) operationResponse.DebugMessage);
          break;
        case 226:
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
          {
            Debug.Log((object) ("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage));
            if (operationResponse.ReturnCode == (short) 32758)
              Debug.Log((object) "Most likely the game became empty during the switch to GameServer.");
          }
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, (object) operationResponse.ReturnCode, (object) operationResponse.DebugMessage);
          break;
        case 227:
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
            Debug.Log((object) ("Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage));
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, (object) operationResponse.ReturnCode, (object) operationResponse.DebugMessage);
          break;
      }
      this.DisconnectToReconnect2();
    }
  }

  private Hashtable GetActorPropertiesForActorNr(Hashtable actorProperties, int actorNr) => ((Dictionary<object, object>) actorProperties).ContainsKey((object) actorNr) ? (Hashtable) actorProperties[(object) actorNr] : actorProperties;

  public int GetInstantiatedObjectsId(GameObject go)
  {
    int instantiatedObjectsId = -1;
    if (Object.op_Equality((Object) go, (Object) null))
    {
      Debug.LogError((object) "GetInstantiatedObjectsId() for GO == null.");
      return instantiatedObjectsId;
    }
    PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
    if (photonViewsInChildren != null && photonViewsInChildren.Length != 0 && Object.op_Inequality((Object) photonViewsInChildren[0], (Object) null))
      return photonViewsInChildren[0].instantiationId;
    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
      Debug.Log((object) ("GetInstantiatedObjectsId failed for GO: " + go?.ToString()));
    return instantiatedObjectsId;
  }

  private Hashtable GetLocalActorProperties()
  {
    if (PhotonNetwork.player != null)
      return PhotonNetwork.player.allProperties;
    return new Hashtable()
    {
      [(object) byte.MaxValue] = (object) this.PlayerName
    };
  }

  protected internal static bool GetMethod(
    MonoBehaviour monob,
    string methodType,
    out MethodInfo mi)
  {
    mi = (MethodInfo) null;
    if (Object.op_Inequality((Object) monob, (Object) null) && !string.IsNullOrEmpty(methodType))
    {
      List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), (System.Type) null);
      for (int index = 0; index < methods.Count; ++index)
      {
        MethodInfo methodInfo = methods[index];
        if (methodInfo.Name.Equals(methodType))
        {
          mi = methodInfo;
          return true;
        }
      }
    }
    return false;
  }

  public PhotonView GetPhotonView(int viewID)
  {
    PhotonView photonView1 = (PhotonView) null;
    this.photonViewList.TryGetValue(viewID, out photonView1);
    if (Object.op_Equality((Object) photonView1, (Object) null))
    {
      foreach (PhotonView photonView2 in Object.FindObjectsOfType(typeof (PhotonView)) as PhotonView[])
      {
        if (photonView2.viewID == viewID)
        {
          if (photonView2.didAwake)
            Debug.LogWarning((object) ("Had to lookup view that wasn't in dict: " + ((object) photonView2)?.ToString()));
          return photonView2;
        }
      }
    }
    return photonView1;
  }

  private PhotonPlayer GetPlayerWithID(int number) => this.mActors != null && this.mActors.ContainsKey(number) ? this.mActors[number] : (PhotonPlayer) null;

  public bool GetRegions()
  {
    if (this.server != ServerConnection.NameServer)
      return false;
    int num = this.OpGetRegions(this.mAppId) ? 1 : 0;
    if (num == 0)
      return num != 0;
    this.AvailableRegions = (List<Region>) null;
    return num != 0;
  }

  private void HandleEventLeave(int actorID)
  {
    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
      Debug.Log((object) ("HandleEventLeave for player ID: " + actorID.ToString()));
    if (actorID < 0 || !this.mActors.ContainsKey(actorID))
    {
      Debug.LogError((object) string.Format("Received event Leave for unknown player ID: {0}", (object) actorID));
    }
    else
    {
      if (PhotonNetwork.player.ID == actorID)
        return;
      PhotonPlayer playerWithId = this.GetPlayerWithID(actorID);
      if (playerWithId == null)
        Debug.LogError((object) ("HandleEventLeave for player ID: " + actorID.ToString() + " has no PhotonPlayer!"));
      this.CheckMasterClient(actorID);
      if (this.mCurrentGame != null)
        this.DestroyPlayerObjects(actorID, true);
      this.RemovePlayer(actorID, playerWithId);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, (object) playerWithId);
    }
  }

  private void LeftLobbyCleanup()
  {
    this.mGameList = new Dictionary<string, RoomInfo>();
    this.mGameListCopy = new RoomInfo[0];
    if (!this.insideLobby)
      return;
    this.insideLobby = false;
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby);
  }

  private void LeftRoomCleanup()
  {
    int num1 = this.mRoomToGetInto != null ? 1 : 0;
    int num2 = this.mRoomToGetInto == null ? (PhotonNetwork.autoCleanUpPlayerObjects ? 1 : 0) : (this.mRoomToGetInto.autoCleanUp ? 1 : 0);
    this.hasSwitchedMC = false;
    this.mRoomToGetInto = (Room) null;
    this.mActors = new Dictionary<int, PhotonPlayer>();
    this.mPlayerListCopy = new PhotonPlayer[0];
    this.mOtherPlayerListCopy = new PhotonPlayer[0];
    this.mMasterClient = (PhotonPlayer) null;
    this.allowedReceivingGroups = new HashSet<int>();
    this.blockSendingGroups = new HashSet<int>();
    this.mGameList = new Dictionary<string, RoomInfo>();
    this.mGameListCopy = new RoomInfo[0];
    this.isFetchingFriends = false;
    this.ChangeLocalID(-1);
    if (num2 != 0)
    {
      this.LocalCleanupAnythingInstantiated(true);
      PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
    }
    if (num1 == 0)
      return;
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
  }

  protected internal void LoadLevelIfSynced()
  {
    if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null || !((Dictionary<object, object>) PhotonNetwork.room.customProperties).ContainsKey((object) "curScn"))
      return;
    object customProperty = PhotonNetwork.room.customProperties[(object) "curScn"];
    switch (customProperty)
    {
      case int num:
        if (Application.loadedLevel == num)
          break;
        PhotonNetwork.LoadLevel((int) customProperty);
        break;
      case string _:
        if (!(Application.loadedLevelName != (string) customProperty))
          break;
        PhotonNetwork.LoadLevel((string) customProperty);
        break;
    }
  }

  public void LocalCleanPhotonView(PhotonView view)
  {
    view.destroyedByPhotonNetworkOrQuit = true;
    this.photonViewList.Remove(view.viewID);
  }

  protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
  {
    if (this.tempInstantiationData.Count > 0)
      Debug.LogWarning((object) "It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
    if (destroyInstantiatedGameObjects)
    {
      foreach (GameObject go in new HashSet<GameObject>((IEnumerable<GameObject>) this.instantiatedObjects.Values))
        this.RemoveInstantiatedGO(go, true);
    }
    this.tempInstantiationData.Clear();
    this.instantiatedObjects = new Dictionary<int, GameObject>();
    PhotonNetwork.lastUsedViewSubId = 0;
    PhotonNetwork.lastUsedViewSubIdStatic = 0;
  }

  public void NewSceneLoaded()
  {
    if (this.loadingLevelAndPausedNetwork)
    {
      this.loadingLevelAndPausedNetwork = false;
      PhotonNetwork.isMessageQueueRunning = true;
    }
    List<int> intList = new List<int>();
    foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
    {
      if (Object.op_Equality((Object) photonView.Value, (Object) null))
        intList.Add(photonView.Key);
    }
    for (int index = 0; index < intList.Count; ++index)
      this.photonViewList.Remove(intList[index]);
    if (intList.Count <= 0 || PhotonNetwork.logLevel < PhotonLogLevel.Informational)
      return;
    Debug.Log((object) ("New level loaded. Removed " + intList.Count.ToString() + " scene view IDs from last level."));
  }

  private bool ObjectIsSameWithInprecision(object one, object two)
  {
    if (one == null || two == null)
      return one == null && two == null;
    if (one.Equals(two))
      return true;
    switch (one)
    {
      case Vector3 _:
        if (((Vector3) one).AlmostEquals((Vector3) two, PhotonNetwork.precisionForVectorSynchronization))
          return true;
        break;
      case Vector2 _:
        if (((Vector2) one).AlmostEquals((Vector2) two, PhotonNetwork.precisionForVectorSynchronization))
          return true;
        break;
      case Quaternion _:
        if (((Quaternion) one).AlmostEquals((Quaternion) two, PhotonNetwork.precisionForQuaternionSynchronization))
          return true;
        break;
      case float target:
        if (target.AlmostEquals((float) two, PhotonNetwork.precisionForFloatSynchronization))
          return true;
        break;
    }
    return false;
  }

  public void OnEvent(EventData photonEvent)
  {
    int num1 = -1;
    PhotonPlayer photonPlayer = (PhotonPlayer) null;
    if (photonEvent.Parameters.ContainsKey((byte) 254))
    {
      num1 = (int) photonEvent[(byte) 254];
      if (this.mActors.ContainsKey(num1))
        photonPlayer = this.mActors[num1];
    }
    else if (photonEvent.Parameters.Count == 0)
      return;
    switch (photonEvent.Code)
    {
      case 200:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        this.ExecuteRPC(photonEvent[(byte) 245] as Hashtable, photonPlayer);
        break;
      case 201:
      case 206:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        object obj1 = photonEvent[(byte) 245];
        if (obj1 != null && obj1 is Hashtable)
        {
          Hashtable hashtable = (Hashtable) photonEvent[(byte) 245];
          if (!(hashtable[(object) (byte) 0] is int))
            return;
          int networkTime = (int) hashtable[(object) (byte) 0];
          short correctPrefix = -1;
          short num2 = 1;
          if (((Dictionary<object, object>) hashtable).ContainsKey((object) (byte) 1))
          {
            if (!(hashtable[(object) (byte) 1] is short))
              return;
            correctPrefix = (short) hashtable[(object) (byte) 1];
            num2 = (short) 2;
          }
          for (short index = num2; (int) index < ((Dictionary<object, object>) hashtable).Count; ++index)
            this.OnSerializeRead(hashtable[(object) index] as Hashtable, photonPlayer, networkTime, correctPrefix);
          break;
        }
        break;
      case 202:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent[(byte) 245] is Hashtable)
        {
          Hashtable evData = (Hashtable) photonEvent[(byte) 245];
          if (evData[(object) (byte) 0] is string && (string) evData[(object) (byte) 0] != null)
          {
            this.DoInstantiate2(evData, photonPlayer, (GameObject) null);
            break;
          }
          break;
        }
        break;
      case 203:
        if (photonPlayer != null && photonPlayer.isMasterClient && !photonPlayer.isLocal)
        {
          PhotonNetwork.LeaveRoom();
          break;
        }
        break;
      case 204:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent[(byte) 245] is Hashtable)
        {
          Hashtable hashtable = (Hashtable) photonEvent[(byte) 245];
          if (hashtable[(object) (byte) 0] is int)
          {
            int key = (int) hashtable[(object) (byte) 0];
            GameObject go = (GameObject) null;
            this.instantiatedObjects.TryGetValue(key, out go);
            if (Object.op_Inequality((Object) go, (Object) null) && photonPlayer != null)
            {
              this.RemoveInstantiatedGO(go, true);
              break;
            }
            break;
          }
          break;
        }
        break;
      case 207:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent[(byte) 245] is Hashtable)
        {
          Hashtable hashtable = (Hashtable) photonEvent[(byte) 245];
          if (hashtable[(object) (byte) 0] is int)
          {
            int playerId = (int) hashtable[(object) (byte) 0];
            if (playerId < 0 && photonPlayer != null && (photonPlayer.isMasterClient || photonPlayer.isLocal))
            {
              this.DestroyAll(true);
              break;
            }
            if (photonPlayer != null && (photonPlayer.isMasterClient || photonPlayer.isLocal || playerId != PhotonNetwork.player.ID))
            {
              this.DestroyPlayerObjects(playerId, true);
              break;
            }
            break;
          }
          break;
        }
        break;
      case 208:
        if (photonEvent[(byte) 245] is Hashtable)
        {
          Hashtable hashtable = (Hashtable) photonEvent[(byte) 245];
          if (hashtable[(object) (byte) 1] is int)
          {
            int playerId = (int) hashtable[(object) (byte) 1];
            if (photonPlayer != null && photonPlayer.isMasterClient && playerId == photonPlayer.ID)
              return;
            if (photonPlayer != null && !photonPlayer.isMasterClient && !photonPlayer.isLocal)
            {
              if (!PhotonNetwork.isMasterClient)
                return;
              FengGameManagerMKII.noRestart = true;
              PhotonNetwork.SetMasterClient(PhotonNetwork.player);
              FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "stealing MC.");
              return;
            }
            if (playerId == this.mLocalActor.ID)
            {
              this.SetMasterClient(playerId, false);
              break;
            }
            if (photonPlayer == null || photonPlayer.isMasterClient || photonPlayer.isLocal)
            {
              this.SetMasterClient(playerId, false);
              break;
            }
            break;
          }
          break;
        }
        break;
      case 226:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        object obj2 = photonEvent[(byte) 229];
        object obj3 = photonEvent[(byte) 227];
        object obj4 = photonEvent[(byte) 228];
        if (obj2 is int && obj3 is int && obj4 is int num3)
        {
          this.mPlayersInRoomsCount = (int) obj2;
          this.mPlayersOnMasterCount = (int) obj3;
          this.mGameCount = num3;
          break;
        }
        break;
      case 228:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent.Parameters.ContainsKey((byte) 223) && photonEvent[(byte) 223] is int num4)
          this.mQueuePosition = num4;
        if (this.mQueuePosition == 0)
        {
          if (PhotonNetwork.autoJoinLobby)
          {
            this.State = FengGameManagerMKII.returnPeerState(0);
            this.OpJoinLobby(this.lobby);
            break;
          }
          this.State = FengGameManagerMKII.returnPeerState(1);
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
          break;
        }
        break;
      case 229:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        object obj5 = photonEvent[(byte) 222];
        if (obj5 is Hashtable)
        {
          foreach (DictionaryEntry dictionaryEntry in (Hashtable) obj5)
          {
            string key = (string) dictionaryEntry.Key;
            RoomInfo roomInfo = new RoomInfo(key, (Hashtable) dictionaryEntry.Value);
            if (roomInfo.removedFromList)
              this.mGameList.Remove(key);
            else
              this.mGameList[key] = roomInfo;
          }
          this.mGameListCopy = new RoomInfo[this.mGameList.Count];
          this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
          break;
        }
        break;
      case 230:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        object obj6 = photonEvent[(byte) 222];
        if (obj6 is Hashtable)
        {
          this.mGameList = new Dictionary<string, RoomInfo>();
          foreach (DictionaryEntry dictionaryEntry in (Hashtable) obj6)
          {
            string key = (string) dictionaryEntry.Key;
            this.mGameList[key] = new RoomInfo(key, (Hashtable) dictionaryEntry.Value);
          }
          this.mGameListCopy = new RoomInfo[this.mGameList.Count];
          this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
          break;
        }
        break;
      case 253:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent[(byte) 253] is int num5)
        {
          Hashtable gameProperties = (Hashtable) null;
          Hashtable pActorProperties = (Hashtable) null;
          if (num5 != 0)
          {
            object obj7 = photonEvent[(byte) 251];
            if (obj7 is Hashtable)
            {
              pActorProperties = (Hashtable) obj7;
              if (photonPlayer != null)
              {
                pActorProperties[(object) "sender"] = (object) photonPlayer;
                if (PhotonNetwork.isMasterClient && !photonPlayer.isLocal && num5 == photonPlayer.ID && (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statACL") || ((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statBLA") || ((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statGAS") || ((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statSPD")))
                {
                  if (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statACL") && RCextensions.returnIntFromObject(pActorProperties[(object) "statACL"]) > 150)
                  {
                    FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "excessive stats.");
                    return;
                  }
                  if (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statBLA") && RCextensions.returnIntFromObject(pActorProperties[(object) "statBLA"]) > 125)
                  {
                    FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "excessive stats.");
                    return;
                  }
                  if (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statGAS") && RCextensions.returnIntFromObject(pActorProperties[(object) "statGAS"]) > 150)
                  {
                    FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "excessive stats.");
                    return;
                  }
                  if (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "statSPD") && RCextensions.returnIntFromObject(pActorProperties[(object) "statSPD"]) > 140)
                  {
                    FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "excessive stats.");
                    return;
                  }
                }
                if (((Dictionary<object, object>) pActorProperties).ContainsKey((object) "name"))
                {
                  if (num5 != photonPlayer.ID)
                    InstantiateTracker.instance.resetPropertyTracking(num5);
                  else if (!InstantiateTracker.instance.PropertiesChanged(photonPlayer))
                    return;
                }
              }
            }
          }
          else
          {
            object obj8 = photonEvent[(byte) 251];
            if (obj8 == null || !(obj8 is Hashtable))
              return;
            gameProperties = (Hashtable) obj8;
          }
          this.ReadoutProperties(gameProperties, pActorProperties, num5);
          break;
        }
        break;
      case 254:
        this.HandleEventLeave(num1);
        break;
      case byte.MaxValue:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        object obj9 = photonEvent[(byte) 249];
        switch (obj9)
        {
          case null:
          case Hashtable _:
            Hashtable properties = (Hashtable) obj9;
            if (photonPlayer == null)
            {
              bool isLocal = this.mLocalActor.ID == num1;
              this.AddNewPlayer(num1, new PhotonPlayer(isLocal, num1, properties));
              this.ResetPhotonViewsOnSerialize();
            }
            if (num1 != this.mLocalActor.ID)
            {
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, (object) this.mActors[num1]);
              break;
            }
            object obj10 = photonEvent[(byte) 252];
            if (obj10 is int[])
            {
              foreach (int num6 in (int[]) obj10)
              {
                if (this.mLocalActor.ID != num6 && !this.mActors.ContainsKey(num6))
                  this.AddNewPlayer(num6, new PhotonPlayer(false, num6, string.Empty));
              }
              if (this.mLastJoinType == JoinType.JoinOrCreateOnDemand && this.mLocalActor.ID == 1)
                NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
              break;
            }
            break;
        }
        break;
      default:
        if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
          return;
        if (photonEvent.Code < (byte) 200 && PhotonNetwork.OnEventCall != null)
        {
          object content = photonEvent[(byte) 245];
          PhotonNetwork.OnEventCall(photonEvent.Code, content, num1);
          break;
        }
        break;
    }
    this.externalListener.OnEvent(photonEvent);
  }

  public void OnOperationResponse(OperationResponse operationResponse)
  {
    if (PhotonNetwork.networkingPeer.State == PeerStates.Disconnecting)
    {
      if (PhotonNetwork.logLevel < PhotonLogLevel.Informational)
        return;
      Debug.Log((object) ("OperationResponse ignored while disconnecting. Code: " + operationResponse.OperationCode.ToString()));
    }
    else
    {
      if (operationResponse.ReturnCode == (short) 0)
      {
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
          Debug.Log((object) operationResponse.ToString());
      }
      else if (operationResponse.ReturnCode == (short) -3)
        Debug.LogError((object) ("Operation " + operationResponse.OperationCode.ToString() + " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation."));
      else if (operationResponse.ReturnCode == (short) 32752)
        Debug.LogError((object) ("Operation " + (object) operationResponse.OperationCode + " failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: " + operationResponse.DebugMessage));
      else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        Debug.LogError((object) ("Operation failed: " + operationResponse.ToStringFull() + " Server: " + (object) this.server));
      if (operationResponse.Parameters.ContainsKey((byte) 221))
      {
        if (this.CustomAuthenticationValues == null)
          this.CustomAuthenticationValues = new AuthenticationValues();
        this.CustomAuthenticationValues.Secret = operationResponse[(byte) 221] as string;
      }
      byte operationCode = operationResponse.OperationCode;
      switch (operationCode)
      {
        case 219:
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, (object) operationResponse);
          break;
        case 220:
          if (operationResponse.ReturnCode != short.MaxValue)
          {
            string[] strArray1 = operationResponse[(byte) 210] as string[];
            string[] strArray2 = operationResponse[(byte) 230] as string[];
            if (strArray1 == null || strArray2 == null || strArray1.Length != strArray2.Length)
            {
              Debug.LogError((object) "The region arrays from Name Server are not ok. Must be non-null and same length.");
              break;
            }
            this.AvailableRegions = new List<Region>(strArray1.Length);
            for (int index = 0; index < strArray1.Length; ++index)
            {
              string str = strArray1[index];
              if (!string.IsNullOrEmpty(str))
              {
                CloudRegionCode cloudRegionCode = Region.Parse(str.ToLower());
                this.AvailableRegions.Add(new Region()
                {
                  Code = cloudRegionCode,
                  HostAndPort = strArray2[index]
                });
              }
            }
            break;
          }
          Debug.LogError((object) string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account."));
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, (object) DisconnectCause.InvalidAuthentication);
          this.State = PeerStates.Disconnecting;
          base.Disconnect();
          return;
        case 222:
          bool[] flagArray = operationResponse[(byte) 1] as bool[];
          string[] strArray = operationResponse[(byte) 2] as string[];
          if (flagArray == null || strArray == null || this.friendListRequested == null || flagArray.Length != this.friendListRequested.Length)
          {
            Debug.LogError((object) "FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
          }
          else
          {
            List<FriendInfo> friendInfoList = new List<FriendInfo>(this.friendListRequested.Length);
            for (int index = 0; index < this.friendListRequested.Length; ++index)
            {
              FriendInfo friendInfo = new FriendInfo()
              {
                Name = this.friendListRequested[index],
                Room = strArray[index],
                IsOnline = flagArray[index]
              };
              friendInfoList.Insert(index, friendInfo);
            }
            PhotonNetwork.Friends = friendInfoList;
          }
          this.friendListRequested = (string[]) null;
          this.isFetchingFriends = false;
          this.friendListTimestamp = Environment.TickCount;
          if (this.friendListTimestamp == 0)
            this.friendListTimestamp = 1;
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList);
          break;
        case 225:
          if (operationResponse.ReturnCode == (short) 0)
          {
            this.mRoomToGetInto.name = (string) operationResponse[byte.MaxValue];
            this.mGameserver = (string) operationResponse[(byte) 230];
            this.DisconnectToReconnect2();
            break;
          }
          if (operationResponse.ReturnCode != (short) 32760)
          {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
              Debug.LogWarning((object) string.Format("JoinRandom failed: {0}.", (object) operationResponse.ToStringFull()));
          }
          else if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            Debug.Log((object) "JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
          break;
        case 226:
          if (this.server == ServerConnection.GameServer)
          {
            this.GameEnteredOnGameServer(operationResponse);
            break;
          }
          if (operationResponse.ReturnCode == (short) 0)
          {
            this.mGameserver = (string) operationResponse[(byte) 230];
            this.DisconnectToReconnect2();
            break;
          }
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
            Debug.Log((object) string.Format("JoinRoom failed (room maybe closed by now). Client stays on masterserver: {0}. State: {1}", (object) operationResponse.ToStringFull(), (object) this.State));
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
          break;
        case 227:
          if (this.server != ServerConnection.GameServer)
          {
            if (operationResponse.ReturnCode != (short) 0)
            {
              if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                Debug.LogWarning((object) string.Format("CreateRoom failed, client stays on masterserver: {0}.", (object) operationResponse.ToStringFull()));
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
              break;
            }
            string str = (string) operationResponse[byte.MaxValue];
            if (!string.IsNullOrEmpty(str))
              this.mRoomToGetInto.name = str;
            this.mGameserver = (string) operationResponse[(byte) 230];
            this.DisconnectToReconnect2();
            break;
          }
          this.GameEnteredOnGameServer(operationResponse);
          break;
        case 228:
          this.State = PeerStates.Authenticated;
          this.LeftLobbyCleanup();
          break;
        case 229:
          this.State = PeerStates.JoinedLobby;
          this.insideLobby = true;
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby);
          break;
        case 230:
          if (operationResponse.ReturnCode == (short) 0)
          {
            if (this.server == ServerConnection.NameServer)
            {
              this.MasterServerAddress = operationResponse[(byte) 230] as string;
              this.DisconnectToReconnect2();
              break;
            }
            if (this.server == ServerConnection.MasterServer)
            {
              if (PhotonNetwork.autoJoinLobby)
              {
                this.State = PeerStates.Authenticated;
                this.OpJoinLobby(this.lobby);
                break;
              }
              this.State = PeerStates.ConnectedToMaster;
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
              break;
            }
            if (this.server == ServerConnection.GameServer)
            {
              this.State = PeerStates.Joining;
              if (this.mLastJoinType == JoinType.JoinGame || this.mLastJoinType == JoinType.JoinRandomGame || this.mLastJoinType == JoinType.JoinOrCreateOnDemand)
              {
                this.OpJoinRoom(this.mRoomToGetInto.name, this.mRoomOptionsForCreate, this.mRoomToEnterLobby, this.mLastJoinType == JoinType.JoinOrCreateOnDemand);
                break;
              }
              if (this.mLastJoinType == JoinType.CreateGame)
              {
                this.OpCreateGame(this.mRoomToGetInto.name, this.mRoomOptionsForCreate, this.mRoomToEnterLobby);
                break;
              }
              break;
            }
            break;
          }
          if (operationResponse.ReturnCode != (short) -2)
          {
            if (operationResponse.ReturnCode == short.MaxValue)
            {
              Debug.LogError((object) string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account."));
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, (object) DisconnectCause.InvalidAuthentication);
            }
            else if (operationResponse.ReturnCode == (short) 32755)
            {
              Debug.LogError((object) string.Format("Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()"));
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, (object) operationResponse.DebugMessage);
            }
            else
              Debug.LogError((object) string.Format("Authentication failed: '{0}' Code: {1}", (object) operationResponse.DebugMessage, (object) operationResponse.ReturnCode));
          }
          else
            Debug.LogError((object) string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' " + this.ServerAddress));
          this.State = PeerStates.Disconnecting;
          base.Disconnect();
          if (operationResponse.ReturnCode == (short) 32757)
          {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
              Debug.LogWarning((object) string.Format("Currently, the limit of users is reached for this title. Try again later. Disconnecting"));
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, (object) DisconnectCause.MaxCcuReached);
            break;
          }
          if (operationResponse.ReturnCode == (short) 32756)
          {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
              Debug.LogError((object) string.Format("The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting."));
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, (object) DisconnectCause.InvalidRegion);
            break;
          }
          if (operationResponse.ReturnCode == (short) 32753)
          {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
              Debug.LogError((object) string.Format("The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting."));
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, (object) DisconnectCause.AuthenticationTicketExpired);
            break;
          }
          break;
        default:
          switch (operationCode)
          {
            case 251:
              Hashtable pActorProperties = (Hashtable) operationResponse[(byte) 249];
              this.ReadoutProperties((Hashtable) operationResponse[(byte) 248], pActorProperties, 0);
              break;
            case 252:
            case 253:
              break;
            case 254:
              this.DisconnectToReconnect2();
              break;
            default:
              Debug.LogWarning((object) string.Format("OperationResponse unhandled: {0}", (object) operationResponse.ToString()));
              break;
          }
          break;
      }
      this.externalListener.OnOperationResponse(operationResponse);
    }
  }

  private void OnSerializeRead(
    Hashtable data,
    PhotonPlayer sender,
    int networkTime,
    short correctPrefix)
  {
    int viewID = (int) data[(object) (byte) 0];
    PhotonView photonView = this.GetPhotonView(viewID);
    if (Object.op_Equality((Object) photonView, (Object) null))
      Debug.LogWarning((object) ("Received OnSerialization for view ID " + (object) viewID + ". We have no such PhotonView! Ignored this if you're leaving a room. State: " + (object) this.State));
    else if (photonView.prefix > 0 && (int) correctPrefix != photonView.prefix)
    {
      Debug.LogError((object) ("Received OnSerialization for view ID " + (object) viewID + " with prefix " + (object) correctPrefix + ". Our prefix is " + (object) photonView.prefix));
    }
    else
    {
      if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
        return;
      if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
      {
        if (!this.DeltaCompressionRead(photonView, data))
        {
          if (PhotonNetwork.logLevel < PhotonLogLevel.Informational)
            return;
          Debug.Log((object) ("Skipping packet for " + ((Object) photonView).name + " [" + (object) photonView.viewID + "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game."));
          return;
        }
        photonView.lastOnSerializeDataReceived = data[(object) (byte) 1] as object[];
      }
      if (photonView.observed is MonoBehaviour)
      {
        PhotonStream pStream = new PhotonStream(false, data[(object) (byte) 1] as object[]);
        PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
        photonView.ExecuteOnSerialize(pStream, info);
      }
      else if (photonView.observed is Transform)
      {
        object[] objArray = data[(object) (byte) 1] as object[];
        Transform observed = (Transform) photonView.observed;
        if (objArray.Length >= 1 && objArray[0] != null)
          observed.localPosition = (Vector3) objArray[0];
        if (objArray.Length >= 2 && objArray[1] != null)
          observed.localRotation = (Quaternion) objArray[1];
        if (objArray.Length < 3 || objArray[2] == null)
          return;
        observed.localScale = (Vector3) objArray[2];
      }
      else if (photonView.observed is Rigidbody)
      {
        object[] objArray = data[(object) (byte) 1] as object[];
        Rigidbody observed = (Rigidbody) photonView.observed;
        if (objArray.Length >= 1 && objArray[0] != null)
          observed.velocity = (Vector3) objArray[0];
        if (objArray.Length < 2 || objArray[1] == null)
          return;
        observed.angularVelocity = (Vector3) objArray[1];
      }
      else
        Debug.LogError((object) "Type of observed is unknown when receiving.");
    }
  }

  private Hashtable OnSerializeWrite(PhotonView view)
  {
    List<object> objectList = new List<object>();
    if (view.observed is MonoBehaviour)
    {
      PhotonStream pStream = new PhotonStream(true, (object[]) null);
      PhotonMessageInfo info = new PhotonMessageInfo(this.mLocalActor, this.ServerTimeInMilliSeconds, view);
      view.ExecuteOnSerialize(pStream, info);
      if (pStream.Count == 0)
        return (Hashtable) null;
      objectList = pStream.data;
    }
    else if (view.observed is Transform)
    {
      Transform observed = (Transform) view.observed;
      if (view.onSerializeTransformOption == OnSerializeTransform.OnlyPosition || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
        objectList.Add((object) observed.localPosition);
      else
        objectList.Add((object) null);
      if (view.onSerializeTransformOption == OnSerializeTransform.OnlyRotation || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
        objectList.Add((object) observed.localRotation);
      else
        objectList.Add((object) null);
      if (view.onSerializeTransformOption == OnSerializeTransform.OnlyScale || view.onSerializeTransformOption == OnSerializeTransform.All)
        objectList.Add((object) observed.localScale);
    }
    else if (view.observed is Rigidbody)
    {
      Rigidbody observed = (Rigidbody) view.observed;
      if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyAngularVelocity)
        objectList.Add((object) observed.velocity);
      else
        objectList.Add((object) null);
      if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyVelocity)
        objectList.Add((object) observed.angularVelocity);
    }
    else
    {
      Debug.LogError((object) ("Observed type is not serializable: " + view.observed.GetType()?.ToString()));
      return (Hashtable) null;
    }
    object[] array = objectList.ToArray();
    if (view.synchronization == ViewSynchronization.UnreliableOnChange)
    {
      if (this.AlmostEquals(array, view.lastOnSerializeDataSent))
      {
        if (view.mixedModeIsReliable)
          return (Hashtable) null;
        view.mixedModeIsReliable = true;
        view.lastOnSerializeDataSent = array;
      }
      else
      {
        view.mixedModeIsReliable = false;
        view.lastOnSerializeDataSent = array;
      }
    }
    Hashtable data = new Hashtable();
    data[(object) (byte) 0] = (object) view.viewID;
    data[(object) (byte) 1] = (object) array;
    if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
    {
      int num = this.DeltaCompressionWrite(view, data) ? 1 : 0;
      view.lastOnSerializeDataSent = array;
      if (num == 0)
        return (Hashtable) null;
    }
    return data;
  }

  public void OnStatusChanged(StatusCode statusCode)
  {
    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
      Debug.Log((object) string.Format("OnStatusChanged: {0}", (object) statusCode.ToString()));
    switch (statusCode - 1022)
    {
      case 0:
      case 1:
        this.State = PeerStates.PeerCreated;
        if (this.CustomAuthenticationValues != null)
          this.CustomAuthenticationValues.Secret = (string) null;
        NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, (object) (DisconnectCause) statusCode);
        goto case 8;
      case 2:
        if (this.State == PeerStates.ConnectingToNameServer)
        {
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            Debug.Log((object) "Connected to NameServer.");
          this.server = ServerConnection.NameServer;
          if (this.CustomAuthenticationValues != null)
            this.CustomAuthenticationValues.Secret = (string) null;
        }
        if (this.State == PeerStates.ConnectingToGameserver)
        {
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            Debug.Log((object) "Connected to gameserver.");
          this.server = ServerConnection.GameServer;
          this.State = PeerStates.ConnectedToGameserver;
        }
        if (this.State == PeerStates.ConnectingToMasterserver)
        {
          if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            Debug.Log((object) "Connected to masterserver.");
          this.server = ServerConnection.MasterServer;
          this.State = PeerStates.ConnectedToMaster;
          if (this.IsInitialConnect)
          {
            this.IsInitialConnect = false;
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
          }
        }
        this.EstablishEncryption();
        if (this.IsAuthorizeSecretAvailable)
        {
          this.didAuthenticate = this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
          if (this.didAuthenticate)
          {
            this.State = PeerStates.Authenticating;
            goto case 8;
          }
          else
            goto case 8;
        }
        else
          goto case 8;
      case 3:
        this.didAuthenticate = false;
        this.isFetchingFriends = false;
        if (this.server == ServerConnection.GameServer)
          this.LeftRoomCleanup();
        if (this.server == ServerConnection.MasterServer)
          this.LeftLobbyCleanup();
        if (this.State == PeerStates.DisconnectingFromMasterserver)
        {
          if (this.Connect(this.mGameserver, ServerConnection.GameServer))
          {
            this.State = PeerStates.ConnectingToGameserver;
            goto case 8;
          }
          else
            goto case 8;
        }
        else if (this.State == PeerStates.DisconnectingFromGameserver || this.State == PeerStates.DisconnectingFromNameServer)
        {
          if (this.Connect(this.MasterServerAddress, ServerConnection.MasterServer))
          {
            this.State = PeerStates.ConnectingToMasterserver;
            goto case 8;
          }
          else
            goto case 8;
        }
        else
        {
          if (this.CustomAuthenticationValues != null)
            this.CustomAuthenticationValues.Secret = (string) null;
          this.State = PeerStates.PeerCreated;
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton);
          goto case 8;
        }
      case 4:
        if (!this.IsInitialConnect)
        {
          this.State = PeerStates.PeerCreated;
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, (object) (DisconnectCause) statusCode);
        }
        else
        {
          Debug.LogError((object) ("Exception while connecting to: " + this.ServerAddress + ". Check if the server is available."));
          if (this.ServerAddress == null || this.ServerAddress.StartsWith("127.0.0.1"))
          {
            Debug.LogWarning((object) "The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
            if (this.ServerAddress == this.mGameserver)
              Debug.LogWarning((object) "This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
          }
          this.State = PeerStates.PeerCreated;
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, (object) (DisconnectCause) statusCode);
        }
        base.Disconnect();
        goto case 8;
      case 5:
      case 6:
      case 7:
label_56:
        Debug.LogError((object) ("Received unknown status code: " + statusCode.ToString()));
        goto case 8;
      case 8:
label_58:
        this.externalListener.OnStatusChanged(statusCode);
        break;
      default:
        switch (statusCode - 1039)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
            if (!this.IsInitialConnect)
            {
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, (object) (DisconnectCause) statusCode);
            }
            else
            {
              Debug.LogWarning((object) (statusCode.ToString() + " while connecting to: " + this.ServerAddress + ". Check if the server is available."));
              NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, (object) (DisconnectCause) statusCode);
            }
            if (this.CustomAuthenticationValues != null)
              this.CustomAuthenticationValues.Secret = (string) null;
            base.Disconnect();
            goto label_58;
          case 9:
            if (this.server == ServerConnection.NameServer)
            {
              this.State = PeerStates.ConnectedToNameServer;
              if (!this.didAuthenticate && this.CloudRegion == CloudRegionCode.none)
                this.OpGetRegions(this.mAppId);
            }
            if (!this.didAuthenticate && (!this.IsUsingNameServer || this.CloudRegion != CloudRegionCode.none))
            {
              this.didAuthenticate = this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
              if (this.didAuthenticate)
              {
                this.State = PeerStates.Authenticating;
                goto label_58;
              }
              else
                goto label_58;
            }
            else
              goto label_58;
          case 10:
            Debug.LogError((object) ("Encryption wasn't established: " + statusCode.ToString() + ". Going to authenticate anyways."));
            this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
            goto label_58;
          default:
            goto label_56;
        }
    }
  }

  public void OpCleanRpcBuffer(PhotonView view)
  {
    Hashtable customEventContent = new Hashtable();
    customEventContent[(object) (byte) 0] = (object) view.viewID;
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      CachingOption = EventCaching.RemoveFromRoomCache
    };
    this.OpRaiseEvent((byte) 200, (object) customEventContent, true, raiseEventOptions);
  }

  public void OpCleanRpcBuffer(int actorNumber)
  {
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      CachingOption = EventCaching.RemoveFromRoomCache
    };
    raiseEventOptions.TargetActors = new int[1]
    {
      actorNumber
    };
    this.OpRaiseEvent((byte) 200, (object) null, true, raiseEventOptions);
  }

  public bool OpCreateGame(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
  {
    bool onGameServer = this.server == ServerConnection.GameServer;
    if (!onGameServer)
    {
      this.mRoomOptionsForCreate = roomOptions;
      this.mRoomToGetInto = new Room(roomName, roomOptions);
      this.mRoomToEnterLobby = !this.insideLobby ? (TypedLobby) null : this.lobby;
    }
    this.mLastJoinType = JoinType.CreateGame;
    return this.OpCreateRoom(roomName, roomOptions, this.mRoomToEnterLobby, this.GetLocalActorProperties(), onGameServer);
  }

  public override bool OpFindFriends(string[] friendsToFind)
  {
    if (this.isFetchingFriends)
      return false;
    this.friendListRequested = friendsToFind;
    this.isFetchingFriends = true;
    return base.OpFindFriends(friendsToFind);
  }

  public override bool OpJoinRandomRoom(
    Hashtable expectedCustomRoomProperties,
    byte expectedMaxPlayers,
    Hashtable playerProperties,
    MatchmakingMode matchingType,
    TypedLobby typedLobby,
    string sqlLobbyFilter)
  {
    this.mRoomToGetInto = new Room((string) null, (RoomOptions) null);
    this.mRoomToEnterLobby = (TypedLobby) null;
    this.mLastJoinType = JoinType.JoinRandomGame;
    return base.OpJoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, playerProperties, matchingType, typedLobby, sqlLobbyFilter);
  }

  public bool OpJoinRoom(
    string roomName,
    RoomOptions roomOptions,
    TypedLobby typedLobby,
    bool createIfNotExists)
  {
    bool onGameServer = this.server == ServerConnection.GameServer;
    if (!onGameServer)
    {
      this.mRoomOptionsForCreate = roomOptions;
      this.mRoomToGetInto = new Room(roomName, roomOptions);
      this.mRoomToEnterLobby = (TypedLobby) null;
      if (createIfNotExists)
        this.mRoomToEnterLobby = !this.insideLobby ? (TypedLobby) null : this.lobby;
    }
    this.mLastJoinType = !createIfNotExists ? JoinType.JoinGame : JoinType.JoinOrCreateOnDemand;
    return this.OpJoinRoom(roomName, roomOptions, this.mRoomToEnterLobby, createIfNotExists, this.GetLocalActorProperties(), onGameServer);
  }

  public virtual bool OpLeave()
  {
    if (this.State == PeerStates.Joined)
      return this.OpCustom((byte) 254, (Dictionary<byte, object>) null, true, (byte) 0, false);
    Debug.LogWarning((object) ("Not sending leave operation. State is not 'Joined': " + this.State.ToString()));
    return false;
  }

  public override bool OpRaiseEvent(
    byte eventCode,
    object customEventContent,
    bool sendReliable,
    RaiseEventOptions raiseEventOptions)
  {
    return !PhotonNetwork.offlineMode && base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
  }

  public void OpRemoveCompleteCache() => this.OpRaiseEvent((byte) 0, (object) null, true, new RaiseEventOptions()
  {
    CachingOption = EventCaching.RemoveFromRoomCache,
    Receivers = ReceiverGroup.MasterClient
  });

  public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
  {
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      CachingOption = EventCaching.RemoveFromRoomCache
    };
    raiseEventOptions.TargetActors = new int[1]
    {
      actorNumber
    };
    this.OpRaiseEvent((byte) 0, (object) null, true, raiseEventOptions);
  }

  private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
  {
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      CachingOption = EventCaching.RemoveFromRoomCache
    };
    raiseEventOptions.TargetActors = new int[1]{ actorNr };
    this.OpRaiseEvent((byte) 202, (object) null, true, raiseEventOptions);
  }

  private void ReadoutProperties(
    Hashtable gameProperties,
    Hashtable pActorProperties,
    int targetActorNr)
  {
    if (this.mCurrentGame != null && gameProperties != null)
    {
      this.mCurrentGame.CacheProperties(gameProperties);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, (object) gameProperties);
      if (PhotonNetwork.automaticallySyncScene)
        this.LoadLevelIfSynced();
    }
    if (pActorProperties == null || ((Dictionary<object, object>) pActorProperties).Count <= 0)
      return;
    if (targetActorNr > 0)
    {
      PhotonPlayer playerWithId = this.GetPlayerWithID(targetActorNr);
      if (playerWithId == null)
        return;
      Hashtable propertiesForActorNr = this.GetActorPropertiesForActorNr(pActorProperties, targetActorNr);
      playerWithId.InternalCacheProperties(propertiesForActorNr);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, (object) playerWithId, (object) propertiesForActorNr);
    }
    else
    {
      foreach (object key in ((Dictionary<object, object>) pActorProperties).Keys)
      {
        int num = (int) key;
        Hashtable pActorProperty = (Hashtable) pActorProperties[key];
        string name = (string) pActorProperty[(object) byte.MaxValue];
        PhotonPlayer player = this.GetPlayerWithID(num);
        if (player == null)
        {
          player = new PhotonPlayer(false, num, name);
          this.AddNewPlayer(num, player);
        }
        player.InternalCacheProperties(pActorProperty);
        NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, (object) player, (object) pActorProperty);
      }
    }
  }

  private void RebuildPlayerListCopies()
  {
    this.mPlayerListCopy = new PhotonPlayer[this.mActors.Count];
    this.mActors.Values.CopyTo(this.mPlayerListCopy, 0);
    List<PhotonPlayer> photonPlayerList = new List<PhotonPlayer>();
    foreach (PhotonPlayer photonPlayer in this.mPlayerListCopy)
    {
      if (!photonPlayer.isLocal)
        photonPlayerList.Add(photonPlayer);
    }
    this.mOtherPlayerListCopy = photonPlayerList.ToArray();
  }

  public void RegisterPhotonView(PhotonView netView)
  {
    if (!Application.isPlaying)
    {
      this.photonViewList = new Dictionary<int, PhotonView>();
    }
    else
    {
      if (netView.subId == 0)
        return;
      if (this.photonViewList.ContainsKey(netView.viewID))
      {
        if (Object.op_Inequality((Object) netView, (Object) this.photonViewList[netView.viewID]))
          Debug.LogError((object) string.Format("PhotonView ID duplicate found: {0}. New: {1} old: {2}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.", (object) netView.viewID, (object) netView, (object) this.photonViewList[netView.viewID]));
        this.RemoveInstantiatedGO(((Component) this.photonViewList[netView.viewID]).gameObject, true);
      }
      this.photonViewList.Add(netView.viewID, netView);
      if (PhotonNetwork.logLevel < PhotonLogLevel.Full)
        return;
      Debug.Log((object) ("Registered PhotonView: " + netView.viewID.ToString()));
    }
  }

  public void RemoveAllInstantiatedObjects()
  {
    GameObject[] array = new GameObject[this.instantiatedObjects.Count];
    this.instantiatedObjects.Values.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      GameObject go = array[index];
      if (Object.op_Inequality((Object) go, (Object) null))
        this.RemoveInstantiatedGO(go, false);
    }
    if (this.instantiatedObjects.Count > 0)
      Debug.LogError((object) "RemoveAllInstantiatedObjects() this.instantiatedObjects.Count should be 0 by now.");
    this.instantiatedObjects = new Dictionary<int, GameObject>();
  }

  private void RemoveCacheOfLeftPlayers() => this.OpCustom((byte) 253, new Dictionary<byte, object>()
  {
    [(byte) 244] = (object) (byte) 0,
    [(byte) 247] = (object) (byte) 7
  }, true, (byte) 0, false);

  public void RemoveInstantiatedGO(GameObject go, bool localOnly)
  {
    if (Object.op_Equality((Object) go, (Object) null))
    {
      Debug.LogError((object) "Failed to 'network-remove' GameObject because it's null.");
    }
    else
    {
      PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
      if (componentsInChildren == null || componentsInChildren.Length == 0)
      {
        Debug.LogError((object) ("Failed to 'network-remove' GameObject because has no PhotonView components: " + go?.ToString()));
      }
      else
      {
        PhotonView photonView = componentsInChildren[0];
        int ownerActorNr = photonView.OwnerActorNr;
        int instantiationId = photonView.instantiationId;
        if (!localOnly)
        {
          if (!photonView.isMine && (!this.mLocalActor.isMasterClient || this.mActors.ContainsKey(ownerActorNr)))
          {
            Debug.LogError((object) ("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + ((object) photonView)?.ToString()));
            return;
          }
          if (instantiationId < 1)
          {
            Debug.LogError((object) ("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: " + ((object) photonView)?.ToString() + ". Not Destroying GameObject or PhotonViews!"));
            return;
          }
        }
        if (!localOnly)
          this.ServerCleanInstantiateAndDestroy(instantiationId, ownerActorNr);
        this.instantiatedObjects.Remove(instantiationId);
        for (int index = componentsInChildren.Length - 1; index >= 0; --index)
        {
          PhotonView view = componentsInChildren[index];
          if (Object.op_Inequality((Object) view, (Object) null))
          {
            if (view.instantiationId >= 1)
              this.LocalCleanPhotonView(view);
            if (!localOnly)
              this.OpCleanRpcBuffer(view);
          }
        }
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
          Debug.Log((object) ("Network destroy Instantiated GO: " + ((Object) go).name));
        Object.Destroy((Object) go);
      }
    }
  }

  private void RemoveInstantiationData(int instantiationId) => this.tempInstantiationData.Remove(instantiationId);

  private void RemovePlayer(int ID, PhotonPlayer player)
  {
    this.mActors.Remove(ID);
    if (player.isLocal)
      return;
    this.RebuildPlayerListCopies();
  }

  public void RemoveRPCsInGroup(int group)
  {
    foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
    {
      PhotonView view = photonView.Value;
      if (view.group == group)
        this.CleanRpcBufferIfMine(view);
    }
  }

  private void ResetPhotonViewsOnSerialize()
  {
    foreach (PhotonView photonView in this.photonViewList.Values)
      photonView.lastOnSerializeDataSent = (object[]) null;
  }

  private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
  {
    if (players == null || players.Length == 0)
      return -1;
    int num = int.MaxValue;
    for (int index = 0; index < players.Length; ++index)
    {
      PhotonPlayer player = players[index];
      if (player.ID != playerIdToIgnore && player.ID < num)
        num = player.ID;
    }
    return num;
  }

  internal void RPC(
    PhotonView view,
    string methodName,
    PhotonPlayer player,
    params object[] parameters)
  {
    if (this.blockSendingGroups.Contains(view.group))
      return;
    if (view.viewID < 1)
      Debug.LogError((object) ("Illegal view ID:" + (object) view.viewID + " method: " + methodName + " GO:" + ((Object) ((Component) view).gameObject).name));
    if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
      Debug.Log((object) ("Sending RPC \"" + methodName + "\" to player[" + (object) player + "]"));
    Hashtable hashtable = new Hashtable();
    hashtable[(object) (byte) 0] = (object) view.viewID;
    if (view.prefix > 0)
      hashtable[(object) (byte) 1] = (object) (short) view.prefix;
    hashtable[(object) (byte) 2] = (object) this.ServerTimeInMilliSeconds;
    int num = 0;
    if (this.rpcShortcuts.TryGetValue(methodName, out num))
      hashtable[(object) (byte) 5] = (object) (byte) num;
    else
      hashtable[(object) (byte) 3] = (object) methodName;
    if (parameters != null && parameters.Length != 0)
      hashtable[(object) (byte) 4] = (object) parameters;
    if (this.mLocalActor == player)
    {
      this.ExecuteRPC(hashtable, player);
    }
    else
    {
      RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
      {
        TargetActors = new int[1]{ player.ID }
      };
      this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions);
    }
  }

  internal void RPC(
    PhotonView view,
    string methodName,
    PhotonTargets target,
    params object[] parameters)
  {
    if (this.blockSendingGroups.Contains(view.group))
      return;
    if (view.viewID < 1)
      Debug.LogError((object) ("Illegal view ID:" + (object) view.viewID + " method: " + methodName + " GO:" + ((Object) ((Component) view).gameObject).name));
    if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
      Debug.Log((object) ("Sending RPC \"" + methodName + "\" to " + (object) target));
    Hashtable hashtable = new Hashtable();
    hashtable[(object) (byte) 0] = (object) view.viewID;
    if (view.prefix > 0)
      hashtable[(object) (byte) 1] = (object) (short) view.prefix;
    hashtable[(object) (byte) 2] = (object) this.ServerTimeInMilliSeconds;
    int num = 0;
    if (this.rpcShortcuts.TryGetValue(methodName, out num))
      hashtable[(object) (byte) 5] = (object) (byte) num;
    else
      hashtable[(object) (byte) 3] = (object) methodName;
    if (parameters != null && parameters.Length != 0)
      hashtable[(object) (byte) 4] = (object) parameters;
    switch (target)
    {
      case PhotonTargets.All:
        RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions()
        {
          InterestGroup = (byte) view.group
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions1);
        this.ExecuteRPC(hashtable, this.mLocalActor);
        break;
      case PhotonTargets.Others:
        RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions()
        {
          InterestGroup = (byte) view.group
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions2);
        break;
      case PhotonTargets.MasterClient:
        if (this.mMasterClient == this.mLocalActor)
        {
          this.ExecuteRPC(hashtable, this.mLocalActor);
          break;
        }
        RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions()
        {
          Receivers = ReceiverGroup.MasterClient
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions3);
        break;
      case PhotonTargets.AllBuffered:
        RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions()
        {
          CachingOption = EventCaching.AddToRoomCache
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions4);
        this.ExecuteRPC(hashtable, this.mLocalActor);
        break;
      case PhotonTargets.OthersBuffered:
        RaiseEventOptions raiseEventOptions5 = new RaiseEventOptions()
        {
          CachingOption = EventCaching.AddToRoomCache
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions5);
        break;
      case PhotonTargets.AllViaServer:
        RaiseEventOptions raiseEventOptions6 = new RaiseEventOptions()
        {
          InterestGroup = (byte) view.group,
          Receivers = ReceiverGroup.All
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions6);
        break;
      case PhotonTargets.AllBufferedViaServer:
        RaiseEventOptions raiseEventOptions7 = new RaiseEventOptions()
        {
          InterestGroup = (byte) view.group,
          Receivers = ReceiverGroup.All,
          CachingOption = EventCaching.AddToRoomCache
        };
        this.OpRaiseEvent((byte) 200, (object) hashtable, true, raiseEventOptions7);
        break;
      default:
        Debug.LogError((object) ("Unsupported target enum: " + target.ToString()));
        break;
    }
  }

  public void RunViewUpdate()
  {
    if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || this.mActors == null || this.mActors.Count <= 1)
      return;
    Dictionary<int, Hashtable> dictionary1 = new Dictionary<int, Hashtable>();
    Dictionary<int, Hashtable> dictionary2 = new Dictionary<int, Hashtable>();
    foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
    {
      PhotonView view = photonView.Value;
      if (Object.op_Inequality((Object) view.observed, (Object) null) && view.synchronization != ViewSynchronization.Off && (view.ownerId == this.mLocalActor.ID || view.isSceneView && this.mMasterClient == this.mLocalActor) && ((Component) view).gameObject.activeInHierarchy && !this.blockSendingGroups.Contains(view.group))
      {
        Hashtable hashtable1 = this.OnSerializeWrite(view);
        if (hashtable1 != null)
        {
          if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed || view.mixedModeIsReliable)
          {
            if (((Dictionary<object, object>) hashtable1).ContainsKey((object) (byte) 1) || ((Dictionary<object, object>) hashtable1).ContainsKey((object) (byte) 2))
            {
              if (!dictionary1.ContainsKey(view.group))
              {
                dictionary1[view.group] = new Hashtable();
                dictionary1[view.group][(object) (byte) 0] = (object) this.ServerTimeInMilliSeconds;
                if (this.currentLevelPrefix >= (short) 0)
                  dictionary1[view.group][(object) (byte) 1] = (object) this.currentLevelPrefix;
              }
              Hashtable hashtable2 = dictionary1[view.group];
              ((Dictionary<object, object>) hashtable2).Add((object) (short) ((Dictionary<object, object>) hashtable2).Count, (object) hashtable1);
            }
          }
          else
          {
            if (!dictionary2.ContainsKey(view.group))
            {
              dictionary2[view.group] = new Hashtable();
              dictionary2[view.group][(object) (byte) 0] = (object) this.ServerTimeInMilliSeconds;
              if (this.currentLevelPrefix >= (short) 0)
                dictionary2[view.group][(object) (byte) 1] = (object) this.currentLevelPrefix;
            }
            Hashtable hashtable3 = dictionary2[view.group];
            ((Dictionary<object, object>) hashtable3).Add((object) (short) ((Dictionary<object, object>) hashtable3).Count, (object) hashtable1);
          }
        }
      }
    }
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
    foreach (KeyValuePair<int, Hashtable> keyValuePair in dictionary1)
    {
      raiseEventOptions.InterestGroup = (byte) keyValuePair.Key;
      this.OpRaiseEvent((byte) 206, (object) keyValuePair.Value, true, raiseEventOptions);
    }
    foreach (KeyValuePair<int, Hashtable> keyValuePair in dictionary2)
    {
      raiseEventOptions.InterestGroup = (byte) keyValuePair.Key;
      this.OpRaiseEvent((byte) 201, (object) keyValuePair.Value, false, raiseEventOptions);
    }
  }

  private void SendDestroyOfAll() => this.OpRaiseEvent((byte) 207, (object) new Hashtable()
  {
    [(object) (byte) 0] = (object) -1
  }, true, (RaiseEventOptions) null);

  private void SendDestroyOfPlayer(int actorNr) => this.OpRaiseEvent((byte) 207, (object) new Hashtable()
  {
    [(object) (byte) 0] = (object) actorNr
  }, true, (RaiseEventOptions) null);

  internal Hashtable SendInstantiate(
    string prefabName,
    Vector3 position,
    Quaternion rotation,
    int group,
    int[] viewIDs,
    object[] data,
    bool isGlobalObject)
  {
    int viewId = viewIDs[0];
    Hashtable customEventContent = new Hashtable();
    customEventContent[(object) (byte) 0] = (object) prefabName;
    if (Vector3.op_Inequality(position, Vector3.zero))
      customEventContent[(object) (byte) 1] = (object) position;
    if (Quaternion.op_Inequality(rotation, Quaternion.identity))
      customEventContent[(object) (byte) 2] = (object) rotation;
    if (group != 0)
      customEventContent[(object) (byte) 3] = (object) group;
    if (viewIDs.Length > 1)
      customEventContent[(object) (byte) 4] = (object) viewIDs;
    if (data != null)
      customEventContent[(object) (byte) 5] = (object) data;
    if (this.currentLevelPrefix > (short) 0)
      customEventContent[(object) (byte) 8] = (object) this.currentLevelPrefix;
    customEventContent[(object) (byte) 6] = (object) this.ServerTimeInMilliSeconds;
    customEventContent[(object) (byte) 7] = (object) viewId;
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      CachingOption = !isGlobalObject ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal
    };
    this.OpRaiseEvent((byte) 202, (object) customEventContent, true, raiseEventOptions);
    return customEventContent;
  }

  public static void SendMonoMessage(
    PhotonNetworkingMessage methodString,
    params object[] parameters)
  {
    HashSet<GameObject> gameObjectSet;
    if (PhotonNetwork.SendMonoMessageTargets != null)
    {
      gameObjectSet = PhotonNetwork.SendMonoMessageTargets;
    }
    else
    {
      gameObjectSet = new HashSet<GameObject>();
      foreach (Component component in (Component[]) Object.FindObjectsOfType(typeof (MonoBehaviour)))
        gameObjectSet.Add(component.gameObject);
    }
    string str = methodString.ToString();
    foreach (GameObject gameObject in gameObjectSet)
    {
      if (parameters != null && parameters.Length == 1)
        gameObject.SendMessage(str, parameters[0], (SendMessageOptions) 1);
      else
        gameObject.SendMessage(str, (object) parameters, (SendMessageOptions) 1);
    }
  }

  private void SendPlayerName()
  {
    if (this.State == PeerStates.Joining)
    {
      this.mPlayernameHasToBeUpdated = true;
    }
    else
    {
      if (this.mLocalActor == null)
        return;
      this.mLocalActor.name = this.PlayerName;
      Hashtable actorProperties = new Hashtable();
      actorProperties[(object) byte.MaxValue] = (object) this.PlayerName;
      if (this.mLocalActor.ID <= 0)
        return;
      this.OpSetPropertiesOfActor(this.mLocalActor.ID, actorProperties, true, (byte) 0);
      this.mPlayernameHasToBeUpdated = false;
    }
  }

  private void ServerCleanInstantiateAndDestroy(int instantiateId, int actorNr)
  {
    Hashtable customEventContent = new Hashtable();
    customEventContent[(object) (byte) 7] = (object) instantiateId;
    RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions()
    {
      CachingOption = EventCaching.RemoveFromRoomCache
    };
    raiseEventOptions1.TargetActors = new int[1]{ actorNr };
    RaiseEventOptions raiseEventOptions2 = raiseEventOptions1;
    this.OpRaiseEvent((byte) 202, (object) customEventContent, true, raiseEventOptions2);
    this.OpRaiseEvent((byte) 204, (object) new Hashtable()
    {
      [(object) (byte) 0] = (object) instantiateId
    }, true, (RaiseEventOptions) null);
  }

  public void SetApp(string appId, string gameVersion)
  {
    this.mAppId = appId.Trim();
    if (string.IsNullOrEmpty(gameVersion))
      return;
    this.mAppVersion = gameVersion.Trim();
  }

  protected internal void SetLevelInPropsIfSynced(object levelId)
  {
    if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
      return;
    if (levelId == null)
    {
      Debug.LogError((object) "Parameter levelId can't be null!");
    }
    else
    {
      if (((Dictionary<object, object>) PhotonNetwork.room.customProperties).ContainsKey((object) "curScn"))
      {
        object customProperty = PhotonNetwork.room.customProperties[(object) "curScn"];
        if (customProperty is int num && Application.loadedLevel == num || customProperty is string && Application.loadedLevelName.Equals((string) customProperty))
          return;
      }
      Hashtable propertiesToSet = new Hashtable();
      switch (levelId)
      {
        case int num1:
          propertiesToSet[(object) "curScn"] = (object) num1;
          break;
        case string _:
          propertiesToSet[(object) "curScn"] = (object) (string) levelId;
          break;
        default:
          Debug.LogError((object) "Parameter levelId must be int or string!");
          break;
      }
      PhotonNetwork.room.SetCustomProperties(propertiesToSet);
      this.SendOutgoingCommands();
    }
  }

  public void SetLevelPrefix(short prefix) => this.currentLevelPrefix = prefix;

  protected internal bool SetMasterClient(int playerId, bool sync)
  {
    if (this.mMasterClient == null || this.mMasterClient.ID == -1 || !this.mActors.ContainsKey(playerId))
      return false;
    if (sync)
    {
      Hashtable customEventContent = new Hashtable();
      ((Dictionary<object, object>) customEventContent).Add((object) (byte) 1, (object) playerId);
      if (!this.OpRaiseEvent((byte) 208, (object) customEventContent, true, (RaiseEventOptions) null))
        return false;
    }
    this.hasSwitchedMC = true;
    this.mMasterClient = this.mActors[playerId];
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, (object) this.mMasterClient);
    return true;
  }

  public void SetReceivingEnabled(int group, bool enabled)
  {
    if (group <= 0)
      Debug.LogError((object) ("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + group.ToString() + ". The group number should be at least 1."));
    else if (enabled)
    {
      if (this.allowedReceivingGroups.Contains(group))
        return;
      this.allowedReceivingGroups.Add(group);
      this.OpChangeGroups((byte[]) null, new byte[1]
      {
        (byte) group
      });
    }
    else
    {
      if (!this.allowedReceivingGroups.Contains(group))
        return;
      this.allowedReceivingGroups.Remove(group);
      this.OpChangeGroups(new byte[1]{ (byte) group }, (byte[]) null);
    }
  }

  public void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
  {
    List<byte> byteList1 = new List<byte>();
    List<byte> byteList2 = new List<byte>();
    if (enableGroups != null)
    {
      for (int index = 0; index < enableGroups.Length; ++index)
      {
        int enableGroup = enableGroups[index];
        if (enableGroup <= 0)
          Debug.LogError((object) ("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + enableGroup.ToString() + ". The group number should be at least 1."));
        else if (!this.allowedReceivingGroups.Contains(enableGroup))
        {
          this.allowedReceivingGroups.Add(enableGroup);
          byteList1.Add((byte) enableGroup);
        }
      }
    }
    if (disableGroups != null)
    {
      for (int index = 0; index < disableGroups.Length; ++index)
      {
        int disableGroup = disableGroups[index];
        if (disableGroup <= 0)
          Debug.LogError((object) ("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + disableGroup.ToString() + ". The group number should be at least 1."));
        else if (byteList1.Contains((byte) disableGroup))
          Debug.LogError((object) ("Error: PhotonNetwork.SetReceivingEnabled disableGroups contains a group that is also in the enableGroups: " + disableGroup.ToString() + "."));
        else if (this.allowedReceivingGroups.Contains(disableGroup))
        {
          this.allowedReceivingGroups.Remove(disableGroup);
          byteList2.Add((byte) disableGroup);
        }
      }
    }
    this.OpChangeGroups(byteList2.Count <= 0 ? (byte[]) null : byteList2.ToArray(), byteList1.Count <= 0 ? (byte[]) null : byteList1.ToArray());
  }

  public void SetSendingEnabled(int group, bool enabled)
  {
    if (!enabled)
      this.blockSendingGroups.Add(group);
    else
      this.blockSendingGroups.Remove(group);
  }

  public void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
  {
    if (enableGroups != null)
    {
      foreach (int enableGroup in enableGroups)
      {
        if (this.blockSendingGroups.Contains(enableGroup))
          this.blockSendingGroups.Remove(enableGroup);
      }
    }
    if (disableGroups == null)
      return;
    foreach (int disableGroup in disableGroups)
    {
      if (!this.blockSendingGroups.Contains(disableGroup))
        this.blockSendingGroups.Add(disableGroup);
    }
  }

  private void StoreInstantiationData(int instantiationId, object[] instantiationData) => this.tempInstantiationData[instantiationId] = instantiationData;

  public bool WebRpc(string uriPath, object parameters) => this.OpCustom((byte) 219, new Dictionary<byte, object>()
  {
    {
      (byte) 209,
      (object) uriPath
    },
    {
      (byte) 208,
      parameters
    }
  }, true, (byte) 0, false);

  public List<Region> AvailableRegions { get; protected internal set; }

  public CloudRegionCode CloudRegion { get; protected internal set; }

  public AuthenticationValues CustomAuthenticationValues { get; set; }

  protected internal int FriendsListAge => this.isFetchingFriends || this.friendListTimestamp == 0 ? 0 : Environment.TickCount - this.friendListTimestamp;

  public bool IsAuthorizeSecretAvailable => false;

  public bool IsUsingNameServer { get; protected internal set; }

  public TypedLobby lobby { get; set; }

  protected internal string mAppVersionPun => string.Format("{0}_{1}", (object) this.mAppVersion, (object) "1.28");

  public string MasterServerAddress { get; protected internal set; }

  public Room mCurrentGame => this.mRoomToGetInto != null && this.mRoomToGetInto.isLocalClientInside ? this.mRoomToGetInto : (Room) null;

  public int mGameCount { get; internal set; }

  public string mGameserver { get; internal set; }

  public PhotonPlayer mLocalActor { get; internal set; }

  public int mPlayersInRoomsCount { get; internal set; }

  public int mPlayersOnMasterCount { get; internal set; }

  public int mQueuePosition { get; internal set; }

  internal RoomOptions mRoomOptionsForCreate { get; set; }

  internal TypedLobby mRoomToEnterLobby { get; set; }

  internal Room mRoomToGetInto { get; set; }

  public string PlayerName
  {
    get => this.playername;
    set
    {
      if (string.IsNullOrEmpty(value) || value.Equals(this.playername))
        return;
      if (this.mLocalActor != null)
        this.mLocalActor.name = value;
      this.playername = value;
      if (this.mCurrentGame == null)
        return;
      this.SendPlayerName();
    }
  }

  protected internal ServerConnection server { get; private set; }

  public PeerStates State { get; internal set; }
}
