// Decompiled with JetBrains decompiler
// Type: PhotonNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonNetwork
{
  private static bool _mAutomaticallySyncScene = false;
  private static bool autoJoinLobbyField = true;
  public static bool InstantiateInRoomOnly = true;
  private static bool isOfflineMode = false;
  internal static int lastUsedViewSubId = 0;
  internal static int lastUsedViewSubIdStatic = 0;
  public static PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;
  private static bool m_autoCleanUpPlayerObjects = true;
  private static bool m_isMessageQueueRunning = true;
  internal static List<int> manuallyAllocatedViewIds = new List<int>();
  public static readonly int MAX_VIEW_IDS = 1000;
  internal static NetworkingPeer networkingPeer;
  private static Room offlineModeRoom = (Room) null;
  public static PhotonNetwork.EventCallback OnEventCall;
  internal static readonly PhotonHandler photonMono;
  public static ServerSettings PhotonServerSettings = (ServerSettings) Resources.Load(nameof (PhotonServerSettings), typeof (ServerSettings));
  public static float precisionForFloatSynchronization = 0.01f;
  public static float precisionForQuaternionSynchronization = 1f;
  public static float precisionForVectorSynchronization = 9.9E-05f;
  public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
  private static int sendInterval = 50;
  private static int sendIntervalOnSerialize = 100;
  public static HashSet<GameObject> SendMonoMessageTargets;
  public const string serverSettingsAssetFile = "PhotonServerSettings";
  public const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";
  public static bool UseNameServer = true;
  public static bool UsePrefabCache = true;
  public const string versionPUN = "1.28";

  static PhotonNetwork()
  {
    Application.runInBackground = true;
    GameObject gameObject = new GameObject();
    PhotonNetwork.photonMono = gameObject.AddComponent<PhotonHandler>();
    ((Object) gameObject).name = "PhotonMono";
    ((Object) gameObject).hideFlags = (HideFlags) 1;
    PhotonNetwork.networkingPeer = new NetworkingPeer((IPhotonPeerListener) PhotonNetwork.photonMono, string.Empty, (ConnectionProtocol) 0);
    CustomTypes.Register();
  }

  private static int[] AllocateSceneViewIDs(int countOfNewViews)
  {
    int[] numArray = new int[countOfNewViews];
    for (int index = 0; index < countOfNewViews; ++index)
      numArray[index] = PhotonNetwork.AllocateViewID(0);
    return numArray;
  }

  public static int AllocateViewID()
  {
    int num = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
    PhotonNetwork.manuallyAllocatedViewIds.Add(num);
    return num;
  }

  private static int AllocateViewID(int ownerId)
  {
    if (ownerId == 0)
    {
      int num1 = PhotonNetwork.lastUsedViewSubIdStatic;
      int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
      for (int index = 1; index < PhotonNetwork.MAX_VIEW_IDS; ++index)
      {
        num1 = (num1 + 1) % PhotonNetwork.MAX_VIEW_IDS;
        if (num1 != 0)
        {
          int key = num1 + num2;
          if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(key))
          {
            PhotonNetwork.lastUsedViewSubIdStatic = num1;
            return key;
          }
        }
      }
      throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of subIds, as all room viewIDs are used.", (object) ownerId));
    }
    int num3 = PhotonNetwork.lastUsedViewSubId;
    int num4 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
    for (int index = 1; index < PhotonNetwork.MAX_VIEW_IDS; ++index)
    {
      num3 = (num3 + 1) % PhotonNetwork.MAX_VIEW_IDS;
      if (num3 != 0)
      {
        int key = num3 + num4;
        if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(key) && !PhotonNetwork.manuallyAllocatedViewIds.Contains(key))
        {
          PhotonNetwork.lastUsedViewSubId = num3;
          return key;
        }
      }
    }
    throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", (object) ownerId));
  }

  public static bool CloseConnection(PhotonPlayer kickPlayer)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return false;
    if (!PhotonNetwork.player.isMasterClient)
    {
      Debug.LogError((object) "CloseConnection: Only the masterclient can kick another player.");
      return false;
    }
    if (kickPlayer == null)
    {
      Debug.LogError((object) "CloseConnection: No such player connected!");
      return false;
    }
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
    {
      TargetActors = new int[1]{ kickPlayer.ID }
    };
    return PhotonNetwork.networkingPeer.OpRaiseEvent((byte) 203, (object) null, true, raiseEventOptions);
  }

  public static bool ConnectToBestCloudServer(string gameVersion)
  {
    if (Object.op_Equality((Object) PhotonNetwork.PhotonServerSettings, (Object) null))
    {
      Debug.LogError((object) "Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
      return false;
    }
    if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
      return PhotonNetwork.ConnectUsingSettings(gameVersion);
    PhotonNetwork.networkingPeer.IsInitialConnect = true;
    PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
    CloudRegionCode codeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
    if (codeInPreferences == CloudRegionCode.none)
      return PhotonNetwork.networkingPeer.ConnectToNameServer();
    Debug.Log((object) ("Best region found in PlayerPrefs. Connecting to: " + codeInPreferences.ToString()));
    return PhotonNetwork.networkingPeer.ConnectToRegionMaster(codeInPreferences);
  }

  public static bool ConnectToMaster(
    string masterServerAddress,
    int port,
    string appID,
    string gameVersion)
  {
    if (PhotonNetwork.networkingPeer.PeerState != null)
    {
      Debug.LogWarning((object) ("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState.ToString()));
      return false;
    }
    if (PhotonNetwork.offlineMode)
    {
      PhotonNetwork.offlineMode = false;
      Debug.LogWarning((object) "ConnectToMaster() disabled the offline mode. No longer offline.");
    }
    if (!PhotonNetwork.isMessageQueueRunning)
    {
      PhotonNetwork.isMessageQueueRunning = true;
      Debug.LogWarning((object) "ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
    }
    PhotonNetwork.networkingPeer.SetApp(appID, gameVersion);
    PhotonNetwork.networkingPeer.IsUsingNameServer = false;
    PhotonNetwork.networkingPeer.IsInitialConnect = true;
    PhotonNetwork.networkingPeer.MasterServerAddress = masterServerAddress + ":" + port.ToString();
    return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
  }

  public static bool ConnectUsingSettings(string gameVersion)
  {
    if (Object.op_Equality((Object) PhotonNetwork.PhotonServerSettings, (Object) null))
    {
      Debug.LogError((object) "Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
      return false;
    }
    PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
    PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
    if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
    {
      PhotonNetwork.offlineMode = true;
      return true;
    }
    if (PhotonNetwork.offlineMode)
      Debug.LogWarning((object) "ConnectUsingSettings() disabled the offline mode. No longer offline.");
    PhotonNetwork.offlineMode = false;
    PhotonNetwork.isMessageQueueRunning = true;
    PhotonNetwork.networkingPeer.IsInitialConnect = true;
    if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
    {
      PhotonNetwork.networkingPeer.IsUsingNameServer = false;
      PhotonNetwork.networkingPeer.MasterServerAddress = PhotonNetwork.PhotonServerSettings.ServerAddress + ":" + PhotonNetwork.PhotonServerSettings.ServerPort.ToString();
      return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
    }
    return PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion ? PhotonNetwork.ConnectToBestCloudServer(gameVersion) : PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.PreferredRegion);
  }

  public static bool CreateRoom(string roomName) => PhotonNetwork.CreateRoom(roomName, (RoomOptions) null, (TypedLobby) null);

  public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
  {
    if (PhotonNetwork.offlineMode)
    {
      if (PhotonNetwork.offlineModeRoom != null)
      {
        Debug.LogError((object) "CreateRoom failed. In offline mode you still have to leave a room to enter another.");
        return false;
      }
      PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
      return true;
    }
    if (PhotonNetwork.networkingPeer.server == ServerConnection.MasterServer && PhotonNetwork.connectedAndReady)
      return PhotonNetwork.networkingPeer.OpCreateGame(roomName, roomOptions, typedLobby);
    Debug.LogError((object) "CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
    return false;
  }

  [Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
  public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers)
  {
    RoomOptions roomOptions = new RoomOptions()
    {
      isVisible = isVisible,
      isOpen = isOpen,
      maxPlayers = maxPlayers
    };
    return PhotonNetwork.CreateRoom(roomName, roomOptions, (TypedLobby) null);
  }

  [Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
  public static bool CreateRoom(
    string roomName,
    bool isVisible,
    bool isOpen,
    int maxPlayers,
    Hashtable customRoomProperties,
    string[] propsToListInLobby)
  {
    RoomOptions roomOptions = new RoomOptions()
    {
      isVisible = isVisible,
      isOpen = isOpen,
      maxPlayers = maxPlayers,
      customRoomProperties = customRoomProperties,
      customRoomPropertiesForLobby = propsToListInLobby
    };
    return PhotonNetwork.CreateRoom(roomName, roomOptions, (TypedLobby) null);
  }

  public static void Destroy(PhotonView targetView)
  {
    if (Object.op_Inequality((Object) targetView, (Object) null))
      PhotonNetwork.networkingPeer.RemoveInstantiatedGO(((Component) targetView).gameObject, !PhotonNetwork.inRoom);
    else
      Debug.LogError((object) "Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
  }

  public static void Destroy(GameObject targetGo) => PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetGo, !PhotonNetwork.inRoom);

  public static void DestroyAll()
  {
    if (PhotonNetwork.isMasterClient)
      PhotonNetwork.networkingPeer.DestroyAll(false);
    else
      Debug.LogError((object) "Couldn't call DestroyAll() as only the master client is allowed to call this.");
  }

  public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
  {
    if (PhotonNetwork.player == null)
      Debug.LogError((object) "DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
    PhotonNetwork.DestroyPlayerObjects(targetPlayer.ID);
  }

  public static void DestroyPlayerObjects(int targetPlayerId)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    if (PhotonNetwork.player.isMasterClient || targetPlayerId == PhotonNetwork.player.ID)
      PhotonNetwork.networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
    else
      Debug.LogError((object) ("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + PhotonNetwork.isMasterClient.ToString()));
  }

  public static void Disconnect()
  {
    if (PhotonNetwork.offlineMode)
    {
      PhotonNetwork.offlineMode = false;
      PhotonNetwork.offlineModeRoom = (Room) null;
      PhotonNetwork.networkingPeer.State = PeerStates.Disconnecting;
      PhotonNetwork.networkingPeer.OnStatusChanged((StatusCode) 1025);
    }
    else
    {
      if (PhotonNetwork.networkingPeer == null)
        return;
      ((PhotonPeer) PhotonNetwork.networkingPeer).Disconnect();
    }
  }

  public static void FetchServerTimestamp()
  {
    if (PhotonNetwork.networkingPeer == null)
      return;
    PhotonNetwork.networkingPeer.FetchServerTimestamp();
  }

  public static bool FindFriends(string[] friendsToFind) => PhotonNetwork.networkingPeer != null && !PhotonNetwork.isOfflineMode && PhotonNetwork.networkingPeer.OpFindFriends(friendsToFind);

  public static int GetPing() => PhotonNetwork.networkingPeer.RoundTripTime;

  public static RoomInfo[] GetRoomList() => !PhotonNetwork.offlineMode && PhotonNetwork.networkingPeer != null ? PhotonNetwork.networkingPeer.mGameListCopy : new RoomInfo[0];

  [Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
  public static void InitializeSecurity()
  {
  }

  public static GameObject Instantiate(
    string prefabName,
    Vector3 position,
    Quaternion rotation,
    int group)
  {
    return PhotonNetwork.Instantiate(prefabName, position, rotation, group, (object[]) null);
  }

  public static GameObject Instantiate(
    string prefabName,
    Vector3 position,
    Quaternion rotation,
    int group,
    object[] data)
  {
    if (!PhotonNetwork.connected || PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom)
    {
      Debug.LogError((object) ("Failed to Instantiate prefab: " + prefabName + ". Client should be in a room. Current connectionStateDetailed: " + (object) PhotonNetwork.connectionStateDetailed));
      return (GameObject) null;
    }
    GameObject gameObject;
    if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
    {
      gameObject = !prefabName.StartsWith("RCAsset/") ? (GameObject) Resources.Load(prefabName, typeof (GameObject)) : FengGameManagerMKII.InstantiateCustomAsset(prefabName);
      if (PhotonNetwork.UsePrefabCache)
        PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
    }
    if (Object.op_Equality((Object) gameObject, (Object) null))
    {
      Debug.LogError((object) ("Failed to Instantiate prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)"));
      return (GameObject) null;
    }
    if (Object.op_Equality((Object) gameObject.GetComponent<PhotonView>(), (Object) null))
    {
      Debug.LogError((object) ("Failed to Instantiate prefab:" + prefabName + ". Prefab must have a PhotonView component."));
      return (GameObject) null;
    }
    int[] viewIDs = new int[gameObject.GetPhotonViewsInChildren().Length];
    for (int index = 0; index < viewIDs.Length; ++index)
      viewIDs[index] = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
    Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, viewIDs, data, false);
    return PhotonNetwork.networkingPeer.DoInstantiate2(evData, PhotonNetwork.networkingPeer.mLocalActor, gameObject);
  }

  public static GameObject InstantiateSceneObject(
    string prefabName,
    Vector3 position,
    Quaternion rotation,
    int group,
    object[] data)
  {
    if (!PhotonNetwork.connected || PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom)
    {
      Debug.LogError((object) ("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client should be in a room. Current connectionStateDetailed: " + (object) PhotonNetwork.connectionStateDetailed));
      return (GameObject) null;
    }
    if (!PhotonNetwork.isMasterClient)
    {
      Debug.LogError((object) ("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client is not the MasterClient in this room."));
      return (GameObject) null;
    }
    GameObject gameObject;
    if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
    {
      gameObject = (GameObject) Resources.Load(prefabName, typeof (GameObject));
      if (PhotonNetwork.UsePrefabCache)
        PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
    }
    if (Object.op_Equality((Object) gameObject, (Object) null))
    {
      Debug.LogError((object) ("Failed to InstantiateSceneObject prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)"));
      return (GameObject) null;
    }
    if (Object.op_Equality((Object) gameObject.GetComponent<PhotonView>(), (Object) null))
    {
      Debug.LogError((object) ("Failed to InstantiateSceneObject prefab:" + prefabName + ". Prefab must have a PhotonView component."));
      return (GameObject) null;
    }
    int[] viewIDs = PhotonNetwork.AllocateSceneViewIDs(gameObject.GetPhotonViewsInChildren().Length);
    if (viewIDs == null)
    {
      Debug.LogError((object) ("Failed to InstantiateSceneObject prefab: " + prefabName + ". No ViewIDs are free to use. Max is: " + (object) PhotonNetwork.MAX_VIEW_IDS));
      return (GameObject) null;
    }
    Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, viewIDs, data, true);
    return PhotonNetwork.networkingPeer.DoInstantiate2(evData, PhotonNetwork.networkingPeer.mLocalActor, gameObject);
  }

  public static void InternalCleanPhotonMonoFromSceneIfStuck()
  {
    if (!(Object.FindObjectsOfType(typeof (PhotonHandler)) is PhotonHandler[] objectsOfType) || objectsOfType.Length == 0)
      return;
    Debug.Log((object) "Cleaning up hidden PhotonHandler instances in scene. Please save it. This is not an issue.");
    foreach (PhotonHandler photonHandler in objectsOfType)
    {
      ((Object) ((Component) photonHandler).gameObject).hideFlags = (HideFlags) 0;
      if (Object.op_Inequality((Object) ((Component) photonHandler).gameObject, (Object) null) && ((Object) ((Component) photonHandler).gameObject).name == "PhotonMono")
        Object.DestroyImmediate((Object) ((Component) photonHandler).gameObject);
      Object.DestroyImmediate((Object) photonHandler);
    }
  }

  public static bool JoinLobby() => PhotonNetwork.JoinLobby((TypedLobby) null);

  public static bool JoinLobby(TypedLobby typedLobby)
  {
    if (!PhotonNetwork.connected || PhotonNetwork.Server != ServerConnection.MasterServer)
      return false;
    if (typedLobby == null)
      typedLobby = TypedLobby.Default;
    int num = PhotonNetwork.networkingPeer.OpJoinLobby(typedLobby) ? 1 : 0;
    if (num == 0)
      return num != 0;
    PhotonNetwork.networkingPeer.lobby = typedLobby;
    return num != 0;
  }

  public static bool JoinOrCreateRoom(
    string roomName,
    RoomOptions roomOptions,
    TypedLobby typedLobby)
  {
    if (PhotonNetwork.offlineMode)
    {
      if (PhotonNetwork.offlineModeRoom != null)
      {
        Debug.LogError((object) "JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
        return false;
      }
      PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
      return true;
    }
    if (PhotonNetwork.networkingPeer.server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
    {
      Debug.LogError((object) "JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
      return false;
    }
    if (!string.IsNullOrEmpty(roomName))
      return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, roomOptions, typedLobby, true);
    Debug.LogError((object) "JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
    return false;
  }

  public static bool JoinRandomRoom() => PhotonNetwork.JoinRandomRoom((Hashtable) null, (byte) 0, MatchmakingMode.FillRoom, (TypedLobby) null, (string) null);

  public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers) => PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, (TypedLobby) null, (string) null);

  public static bool JoinRandomRoom(
    Hashtable expectedCustomRoomProperties,
    byte expectedMaxPlayers,
    MatchmakingMode matchingType,
    TypedLobby typedLobby,
    string sqlLobbyFilter)
  {
    if (PhotonNetwork.offlineMode)
    {
      if (PhotonNetwork.offlineModeRoom != null)
      {
        Debug.LogError((object) "JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
        return false;
      }
      PhotonNetwork.offlineModeRoom = new Room("offline room", (RoomOptions) null);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
      return true;
    }
    if (PhotonNetwork.networkingPeer.server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
    {
      Debug.LogError((object) "JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
      return false;
    }
    Hashtable hashtable = new Hashtable();
    ((IDictionary) hashtable).MergeStringKeys((IDictionary) expectedCustomRoomProperties);
    if (expectedMaxPlayers > (byte) 0)
      hashtable[(object) byte.MaxValue] = (object) expectedMaxPlayers;
    return PhotonNetwork.networkingPeer.OpJoinRandomRoom(hashtable, (byte) 0, (Hashtable) null, matchingType, typedLobby, sqlLobbyFilter);
  }

  public static bool JoinRoom(string roomName)
  {
    if (PhotonNetwork.offlineMode)
    {
      if (PhotonNetwork.offlineModeRoom != null)
      {
        Debug.LogError((object) "JoinRoom failed. In offline mode you still have to leave a room to enter another.");
        return false;
      }
      PhotonNetwork.offlineModeRoom = new Room(roomName, (RoomOptions) null);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
      return true;
    }
    if (PhotonNetwork.networkingPeer.server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
    {
      Debug.LogError((object) "JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
      return false;
    }
    if (!string.IsNullOrEmpty(roomName))
      return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, (RoomOptions) null, (TypedLobby) null, false);
    Debug.LogError((object) "JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
    return false;
  }

  [Obsolete("Use overload with roomOptions and TypedLobby parameter.")]
  public static bool JoinRoom(string roomName, bool createIfNotExists)
  {
    if (PhotonNetwork.connectionStateDetailed == PeerStates.Joining || PhotonNetwork.connectionStateDetailed == PeerStates.Joined || PhotonNetwork.connectionStateDetailed == PeerStates.ConnectedToGameserver)
      Debug.LogError((object) "JoinRoom aborted: You can only join a room while not currently connected/connecting to a room.");
    else if (PhotonNetwork.room != null)
      Debug.LogError((object) "JoinRoom aborted: You are already in a room!");
    else if (roomName == string.Empty)
    {
      Debug.LogError((object) "JoinRoom aborted: You must specifiy a room name!");
    }
    else
    {
      if (!PhotonNetwork.offlineMode)
        return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, (RoomOptions) null, (TypedLobby) null, createIfNotExists);
      PhotonNetwork.offlineModeRoom = new Room(roomName, (RoomOptions) null);
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
      return true;
    }
    return false;
  }

  public static bool LeaveLobby() => PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer && PhotonNetwork.networkingPeer.OpLeaveLobby();

  public static bool RejoinRoom(
    string roomName,
    RoomOptions roomOptions,
    TypedLobby typedLobby,
    bool createIfNotExists,
    Hashtable Hash)
  {
    bool onGameServer = PhotonNetwork.networkingPeer.server == ServerConnection.GameServer;
    if (!onGameServer)
    {
      Room room = new Room(roomName, roomOptions);
      PhotonNetwork.networkingPeer.mRoomToEnterLobby = (TypedLobby) null;
      if (createIfNotExists)
        PhotonNetwork.networkingPeer.mRoomToEnterLobby = !PhotonNetwork.networkingPeer.insideLobby ? (TypedLobby) null : PhotonNetwork.networkingPeer.lobby;
    }
    return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, roomOptions, PhotonNetwork.networkingPeer.mRoomToEnterLobby, createIfNotExists, Hash, onGameServer);
  }

  public static bool LeaveRoom()
  {
    if (PhotonNetwork.offlineMode)
    {
      PhotonNetwork.offlineModeRoom = (Room) null;
      NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
      return true;
    }
    if (PhotonNetwork.room == null)
      Debug.LogWarning((object) ("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: " + PhotonNetwork.connectionStateDetailed.ToString()));
    return PhotonNetwork.networkingPeer.OpLeave();
  }

  public static void LoadLevel(int levelNumber)
  {
    PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced((object) levelNumber);
    PhotonNetwork.isMessageQueueRunning = false;
    PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
    Application.LoadLevel(levelNumber);
  }

  public static void LoadLevel(string levelName)
  {
    PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced((object) levelName);
    PhotonNetwork.isMessageQueueRunning = false;
    PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
    Application.LoadLevel(levelName);
  }

  public static void NetworkStatisticsReset() => PhotonNetwork.networkingPeer.TrafficStatsReset();

  public static string NetworkStatisticsToString() => PhotonNetwork.networkingPeer != null && !PhotonNetwork.offlineMode ? PhotonNetwork.networkingPeer.VitalStatsToString(false) : "Offline or in OfflineMode. No VitalStats available.";

  public static void OverrideBestCloudServer(CloudRegionCode region) => PhotonHandler.BestRegionCodeInPreferences = region;

  public static bool RaiseEvent(
    byte eventCode,
    object eventContent,
    bool sendReliable,
    RaiseEventOptions options)
  {
    if (PhotonNetwork.inRoom && eventCode < byte.MaxValue)
      return PhotonNetwork.networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
    Debug.LogWarning((object) "RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
    return false;
  }

  public static void RefreshCloudServerRating() => throw new NotImplementedException("not available at the moment");

  public static void RemoveRPCs(PhotonPlayer targetPlayer)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    if (!targetPlayer.isLocal && !PhotonNetwork.isMasterClient)
      Debug.LogError((object) "Error; Only the MasterClient can call RemoveRPCs for other players.");
    else
      PhotonNetwork.networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
  }

  public static void RemoveRPCs(PhotonView targetPhotonView)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
  }

  public static void RemoveRPCsInGroup(int targetGroup)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.RemoveRPCsInGroup(targetGroup);
  }

  internal static void RPC(
    PhotonView view,
    string methodName,
    PhotonPlayer targetPlayer,
    params object[] parameters)
  {
    if (PhotonNetwork.VerifyCanUseNetwork())
    {
      if (PhotonNetwork.room == null)
      {
        Debug.LogWarning((object) "Cannot send RPCs in Lobby, only processed locally");
      }
      else
      {
        if (PhotonNetwork.player == null)
          Debug.LogError((object) ("Error; Sending RPC to player null! Aborted \"" + methodName + "\""));
        if (PhotonNetwork.networkingPeer != null)
          PhotonNetwork.networkingPeer.RPC(view, methodName, targetPlayer, parameters);
        else
          Debug.LogWarning((object) ("Could not execute RPC " + methodName + ". Possible scene loading in progress?"));
      }
    }
    PhotonNetwork.LogRPC(methodName, parameters);
  }

  internal static void RPC(
    PhotonView view,
    string methodName,
    PhotonTargets target,
    params object[] parameters)
  {
    if (PhotonNetwork.VerifyCanUseNetwork())
    {
      if (PhotonNetwork.room == null)
        Debug.LogWarning((object) "Cannot send RPCs in Lobby! RPC dropped.");
      else if (PhotonNetwork.networkingPeer != null)
        PhotonNetwork.networkingPeer.RPC(view, methodName, target, parameters);
      else
        Debug.LogWarning((object) ("Could not execute RPC " + methodName + ". Possible scene loading in progress?"));
    }
    PhotonNetwork.LogRPC(methodName, parameters);
  }

  private static void LogRPC(string methodName, params object[] parameters)
  {
  }

  public static void SendOutgoingCommands()
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    do
      ;
    while (PhotonNetwork.networkingPeer.SendOutgoingCommands());
  }

  public static void SetLevelPrefix(short prefix)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.SetLevelPrefix(prefix);
  }

  public static bool SetMasterClient(PhotonPlayer masterClientPlayer) => PhotonNetwork.networkingPeer.SetMasterClient(masterClientPlayer.ID, true);

  public static void SetPlayerCustomProperties(Hashtable customProperties)
  {
    if (customProperties == null)
    {
      customProperties = new Hashtable();
      foreach (object key in ((Dictionary<object, object>) PhotonNetwork.player.customProperties).Keys)
        customProperties[(object) (string) key] = (object) null;
    }
    if (PhotonNetwork.room != null && PhotonNetwork.room.isLocalClientInside)
      PhotonNetwork.player.SetCustomProperties(customProperties);
    else
      PhotonNetwork.player.InternalCacheProperties(customProperties);
  }

  public static void SetReceivingEnabled(int group, bool enabled)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.SetReceivingEnabled(group, enabled);
  }

  public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.SetReceivingEnabled(enableGroups, disableGroups);
  }

  public static void SetSendingEnabled(int group, bool enabled)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.SetSendingEnabled(group, enabled);
  }

  public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
  {
    if (!PhotonNetwork.VerifyCanUseNetwork())
      return;
    PhotonNetwork.networkingPeer.SetSendingEnabled(enableGroups, disableGroups);
  }

  public static void SwitchToProtocol(ConnectionProtocol cp)
  {
    if (PhotonNetwork.networkingPeer.UsedProtocol == cp)
      return;
    try
    {
      ((PhotonPeer) PhotonNetwork.networkingPeer).Disconnect();
      PhotonNetwork.networkingPeer.StopThread();
    }
    catch
    {
    }
    PhotonNetwork.networkingPeer = new NetworkingPeer((IPhotonPeerListener) PhotonNetwork.photonMono, string.Empty, cp);
    Debug.Log((object) ("Protocol switched to: " + cp.ToString()));
  }

  public static void UnAllocateViewID(int viewID)
  {
    PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
    if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(viewID))
      return;
    Debug.LogWarning((object) string.Format("Unallocated manually used viewID: {0} but found it used still in a PhotonView: {1}", (object) viewID, (object) PhotonNetwork.networkingPeer.photonViewList[viewID]));
  }

  private static bool VerifyCanUseNetwork()
  {
    if (PhotonNetwork.connected)
      return true;
    Debug.LogError((object) "Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
    return false;
  }

  public static bool WebRpc(string name, object parameters) => PhotonNetwork.networkingPeer.WebRpc(name, parameters);

  public static AuthenticationValues AuthValues
  {
    get => PhotonNetwork.networkingPeer != null ? PhotonNetwork.networkingPeer.CustomAuthenticationValues : (AuthenticationValues) null;
    set
    {
      if (PhotonNetwork.networkingPeer == null)
        return;
      PhotonNetwork.networkingPeer.CustomAuthenticationValues = value;
    }
  }

  public static bool autoCleanUpPlayerObjects
  {
    get => PhotonNetwork.m_autoCleanUpPlayerObjects;
    set
    {
      if (PhotonNetwork.room != null)
        Debug.LogError((object) "Setting autoCleanUpPlayerObjects while in a room is not supported.");
      else
        PhotonNetwork.m_autoCleanUpPlayerObjects = value;
    }
  }

  public static bool autoJoinLobby
  {
    get => PhotonNetwork.autoJoinLobbyField;
    set => PhotonNetwork.autoJoinLobbyField = value;
  }

  public static bool automaticallySyncScene
  {
    get => PhotonNetwork._mAutomaticallySyncScene;
    set
    {
      PhotonNetwork._mAutomaticallySyncScene = value;
      if (!PhotonNetwork._mAutomaticallySyncScene || PhotonNetwork.room == null)
        return;
      PhotonNetwork.networkingPeer.LoadLevelIfSynced();
    }
  }

  public static bool connected
  {
    get
    {
      if (PhotonNetwork.offlineMode)
        return true;
      return PhotonNetwork.networkingPeer != null && !PhotonNetwork.networkingPeer.IsInitialConnect && PhotonNetwork.networkingPeer.State != PeerStates.PeerCreated && PhotonNetwork.networkingPeer.State != PeerStates.Disconnected && PhotonNetwork.networkingPeer.State != PeerStates.Disconnecting && PhotonNetwork.networkingPeer.State != PeerStates.ConnectingToNameServer;
    }
  }

  public static bool connectedAndReady
  {
    get
    {
      if (!PhotonNetwork.connected)
        return false;
      if (!PhotonNetwork.offlineMode)
      {
        switch (PhotonNetwork.connectionStateDetailed)
        {
          case PeerStates.PeerCreated:
          case PeerStates.ConnectingToGameserver:
          case PeerStates.Joining:
          case PeerStates.Leaving:
          case PeerStates.ConnectingToMasterserver:
          case PeerStates.Disconnecting:
          case PeerStates.Disconnected:
          case PeerStates.ConnectingToNameServer:
          case PeerStates.Authenticating:
            return false;
        }
      }
      return true;
    }
  }

  public static bool connecting => PhotonNetwork.networkingPeer.IsInitialConnect && !PhotonNetwork.offlineMode;

  public static ConnectionState connectionState
  {
    get
    {
      if (PhotonNetwork.offlineMode)
        return ConnectionState.Connected;
      if (PhotonNetwork.networkingPeer != null)
      {
        PeerStateValue peerState = PhotonNetwork.networkingPeer.PeerState;
        switch ((int) peerState)
        {
          case 0:
            return ConnectionState.Disconnected;
          case 1:
            return ConnectionState.Connecting;
          case 2:
            break;
          case 3:
            return ConnectionState.Connected;
          case 4:
            return ConnectionState.Disconnecting;
          default:
            if (peerState == 10)
              return ConnectionState.InitializingApplication;
            break;
        }
      }
      return ConnectionState.Disconnected;
    }
  }

  public static PeerStates connectionStateDetailed => PhotonNetwork.offlineMode ? (PhotonNetwork.offlineModeRoom != null ? PeerStates.Joined : PeerStates.ConnectedToMaster) : (PhotonNetwork.networkingPeer == null ? PeerStates.Disconnected : PhotonNetwork.networkingPeer.State);

  public static int countOfPlayers => PhotonNetwork.networkingPeer.mPlayersInRoomsCount + PhotonNetwork.networkingPeer.mPlayersOnMasterCount;

  public static int countOfPlayersInRooms => PhotonNetwork.networkingPeer.mPlayersInRoomsCount;

  public static int countOfPlayersOnMaster => PhotonNetwork.networkingPeer.mPlayersOnMasterCount;

  public static int countOfRooms => PhotonNetwork.networkingPeer.mGameCount;

  public static bool CrcCheckEnabled
  {
    get => PhotonNetwork.networkingPeer.CrcEnabled;
    set
    {
      if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
        PhotonNetwork.networkingPeer.CrcEnabled = value;
      else
        Debug.Log((object) ("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + PhotonNetwork.networkingPeer.CrcEnabled.ToString()));
    }
  }

  public static List<FriendInfo> Friends { get; set; }

  public static int FriendsListAge => PhotonNetwork.networkingPeer != null ? PhotonNetwork.networkingPeer.FriendsListAge : 0;

  public static string gameVersion
  {
    get => PhotonNetwork.networkingPeer.mAppVersion;
    set => PhotonNetwork.networkingPeer.mAppVersion = value;
  }

  public static bool inRoom => PhotonNetwork.connectionStateDetailed == PeerStates.Joined;

  public static bool insideLobby => PhotonNetwork.networkingPeer.insideLobby;

  public static bool isMasterClient => PhotonNetwork.offlineMode || PhotonNetwork.networkingPeer.mMasterClient == PhotonNetwork.networkingPeer.mLocalActor;

  public static bool isMessageQueueRunning
  {
    get => PhotonNetwork.m_isMessageQueueRunning;
    set
    {
      if (value)
        PhotonHandler.StartFallbackSendAckThread();
      PhotonNetwork.networkingPeer.IsSendingOnlyAcks = !value;
      PhotonNetwork.m_isMessageQueueRunning = value;
    }
  }

  public static bool isNonMasterClientInRoom => !PhotonNetwork.isMasterClient && PhotonNetwork.room != null;

  public static TypedLobby lobby
  {
    get => PhotonNetwork.networkingPeer.lobby;
    set => PhotonNetwork.networkingPeer.lobby = value;
  }

  public static PhotonPlayer masterClient => PhotonNetwork.networkingPeer == null ? (PhotonPlayer) null : PhotonNetwork.networkingPeer.mMasterClient;

  [Obsolete("Used for compatibility with Unity networking only.")]
  public static int maxConnections
  {
    get => PhotonNetwork.room == null ? 0 : PhotonNetwork.room.maxPlayers;
    set => PhotonNetwork.room.maxPlayers = value;
  }

  public static int MaxResendsBeforeDisconnect
  {
    get => PhotonNetwork.networkingPeer.SentCountAllowance;
    set
    {
      if (value < 3)
        value = 3;
      if (value > 10)
        value = 10;
      PhotonNetwork.networkingPeer.SentCountAllowance = value;
    }
  }

  public static bool NetworkStatisticsEnabled
  {
    get => PhotonNetwork.networkingPeer.TrafficStatsEnabled;
    set => PhotonNetwork.networkingPeer.TrafficStatsEnabled = value;
  }

  public static bool offlineMode
  {
    get => PhotonNetwork.isOfflineMode;
    set
    {
      if (value == PhotonNetwork.isOfflineMode)
        return;
      if (value && PhotonNetwork.connected)
      {
        Debug.LogError((object) "Can't start OFFLINE mode while connected!");
      }
      else
      {
        if (PhotonNetwork.networkingPeer.PeerState != null)
          ((PhotonPeer) PhotonNetwork.networkingPeer).Disconnect();
        PhotonNetwork.isOfflineMode = value;
        if (PhotonNetwork.isOfflineMode)
        {
          NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
          PhotonNetwork.networkingPeer.ChangeLocalID(1);
          PhotonNetwork.networkingPeer.mMasterClient = PhotonNetwork.player;
        }
        else
        {
          PhotonNetwork.offlineModeRoom = (Room) null;
          PhotonNetwork.networkingPeer.ChangeLocalID(-1);
          PhotonNetwork.networkingPeer.mMasterClient = (PhotonPlayer) null;
        }
      }
    }
  }

  public static PhotonPlayer[] otherPlayers => PhotonNetwork.networkingPeer == null ? new PhotonPlayer[0] : PhotonNetwork.networkingPeer.mOtherPlayerListCopy;

  public static int PacketLossByCrcCheck => PhotonNetwork.networkingPeer.PacketLossByCrc;

  public static PhotonPlayer player => PhotonNetwork.networkingPeer == null ? (PhotonPlayer) null : PhotonNetwork.networkingPeer.mLocalActor;

  public static PhotonPlayer[] playerList => PhotonNetwork.networkingPeer == null ? new PhotonPlayer[0] : PhotonNetwork.networkingPeer.mPlayerListCopy;

  public static string playerName
  {
    get => PhotonNetwork.networkingPeer.PlayerName;
    set => PhotonNetwork.networkingPeer.PlayerName = value;
  }

  public static int ResentReliableCommands => PhotonNetwork.networkingPeer.ResentReliableCommands;

  public static Room room => PhotonNetwork.isOfflineMode ? PhotonNetwork.offlineModeRoom : PhotonNetwork.networkingPeer.mCurrentGame;

  public static int sendRate
  {
    get => 1000 / PhotonNetwork.sendInterval;
    set
    {
      PhotonNetwork.sendInterval = 1000 / value;
      if (Object.op_Inequality((Object) PhotonNetwork.photonMono, (Object) null))
        PhotonNetwork.photonMono.updateInterval = PhotonNetwork.sendInterval;
      if (value >= PhotonNetwork.sendRateOnSerialize)
        return;
      PhotonNetwork.sendRateOnSerialize = value;
    }
  }

  public static int sendRateOnSerialize
  {
    get => 1000 / PhotonNetwork.sendIntervalOnSerialize;
    set
    {
      if (value > PhotonNetwork.sendRate)
      {
        Debug.LogError((object) "Error, can not set the OnSerialize SendRate more often then the overall SendRate");
        value = PhotonNetwork.sendRate;
      }
      PhotonNetwork.sendIntervalOnSerialize = 1000 / value;
      if (!Object.op_Inequality((Object) PhotonNetwork.photonMono, (Object) null))
        return;
      PhotonNetwork.photonMono.updateIntervalOnSerialize = PhotonNetwork.sendIntervalOnSerialize;
    }
  }

  public static ServerConnection Server => PhotonNetwork.networkingPeer.server;

  public static string ServerAddress => PhotonNetwork.networkingPeer != null ? PhotonNetwork.networkingPeer.ServerAddress : "<not connected>";

  public static double time => PhotonNetwork.offlineMode ? (double) Time.time : (double) PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds / 1000.0;

  public static int unreliableCommandsLimit
  {
    get => PhotonNetwork.networkingPeer.LimitOfUnreliableCommands;
    set => PhotonNetwork.networkingPeer.LimitOfUnreliableCommands = value;
  }

  public delegate void EventCallback(byte eventCode, object content, int senderId);
}
