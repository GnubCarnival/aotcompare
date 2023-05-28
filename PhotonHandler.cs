// Decompiled with JetBrains decompiler
// Type: PhotonHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using Photon;
using System;
using UnityEngine;

internal class PhotonHandler : MonoBehaviour, IPhotonPeerListener
{
  public static bool AppQuits;
  internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;
  private int nextSendTickCount;
  private int nextSendTickCountOnSerialize;
  public static System.Type PingImplementation;
  private const string PlayerPrefsKey = "PUNCloudBestRegion";
  private static bool sendThreadShouldRun;
  public static PhotonHandler SP;
  public int updateInterval;
  public int updateIntervalOnSerialize;

  protected void Awake()
  {
    if (Object.op_Inequality((Object) PhotonHandler.SP, (Object) null) && Object.op_Inequality((Object) PhotonHandler.SP, (Object) this) && Object.op_Inequality((Object) ((Component) PhotonHandler.SP).gameObject, (Object) null))
      Object.DestroyImmediate((Object) ((Component) PhotonHandler.SP).gameObject);
    PhotonHandler.SP = this;
    Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
    this.updateInterval = 1000 / PhotonNetwork.sendRate;
    this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
    PhotonHandler.StartFallbackSendAckThread();
  }

  public void DebugReturn(DebugLevel level, string message)
  {
    if (level == 1)
      Debug.LogError((object) message);
    else if (level == 2)
      Debug.LogWarning((object) message);
    else if (level == 3 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
    {
      Debug.Log((object) message);
    }
    else
    {
      if (level != 5 || PhotonNetwork.logLevel != PhotonLogLevel.Full)
        return;
      Debug.Log((object) message);
    }
  }

  public static bool FallbackSendAckThread()
  {
    if (PhotonHandler.sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
      PhotonNetwork.networkingPeer.SendAcksOnly();
    return PhotonHandler.sendThreadShouldRun;
  }

  protected void OnApplicationQuit()
  {
    PhotonHandler.AppQuits = true;
    PhotonHandler.StopFallbackSendAckThread();
    PhotonNetwork.Disconnect();
  }

  protected void OnCreatedRoom() => PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced((object) Application.loadedLevelName);

  public void OnEvent(EventData photonEvent)
  {
  }

  protected void OnJoinedRoom() => PhotonNetwork.networkingPeer.LoadLevelIfSynced();

  protected void OnLevelWasLoaded(int level)
  {
    PhotonNetwork.networkingPeer.NewSceneLoaded();
    PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced((object) Application.loadedLevelName);
  }

  public void OnOperationResponse(OperationResponse operationResponse)
  {
  }

  public void OnStatusChanged(StatusCode statusCode)
  {
  }

  public static void StartFallbackSendAckThread()
  {
    if (PhotonHandler.sendThreadShouldRun)
      return;
    PhotonHandler.sendThreadShouldRun = true;
    int num = (int) SupportClass.CallInBackground(new Func<bool>(PhotonHandler.FallbackSendAckThread), 100, "");
  }

  public static void StopFallbackSendAckThread() => PhotonHandler.sendThreadShouldRun = false;

  protected void Update()
  {
    if (PhotonNetwork.networkingPeer == null)
    {
      Debug.LogError((object) "NetworkPeer broke!");
    }
    else
    {
      if (PhotonNetwork.connectionStateDetailed == PeerStates.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerStates.Disconnected || PhotonNetwork.offlineMode || !PhotonNetwork.isMessageQueueRunning)
        return;
      bool flag1 = true;
      while (PhotonNetwork.isMessageQueueRunning & flag1)
        flag1 = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
      int num1 = (int) ((double) Time.realtimeSinceStartup * 1000.0);
      if (PhotonNetwork.isMessageQueueRunning && num1 > this.nextSendTickCountOnSerialize)
      {
        PhotonNetwork.networkingPeer.RunViewUpdate();
        this.nextSendTickCountOnSerialize = num1 + this.updateIntervalOnSerialize;
        this.nextSendTickCount = 0;
      }
      int num2 = (int) ((double) Time.realtimeSinceStartup * 1000.0);
      if (num2 <= this.nextSendTickCount)
        return;
      bool flag2 = true;
      while (PhotonNetwork.isMessageQueueRunning & flag2)
        flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
      this.nextSendTickCount = num2 + this.updateInterval;
    }
  }

  internal static CloudRegionCode BestRegionCodeInPreferences
  {
    get
    {
      string codeAsString = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
      return !string.IsNullOrEmpty(codeAsString) ? Region.Parse(codeAsString) : CloudRegionCode.none;
    }
    set
    {
      if (value == CloudRegionCode.none)
        PlayerPrefs.DeleteKey("PUNCloudBestRegion");
      else
        PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
    }
  }
}
