// Decompiled with JetBrains decompiler
// Type: ConnectAndJoinRandom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class ConnectAndJoinRandom : MonoBehaviour
{
  public bool AutoConnect = true;
  private bool ConnectInUpdate = true;

  public virtual void OnConnectedToMaster()
  {
    if (PhotonNetwork.networkingPeer.AvailableRegions != null)
      Debug.LogWarning((object) ("List of available regions counts " + (object) PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + (object) PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + (object) PhotonNetwork.networkingPeer.CloudRegion));
    Debug.Log((object) "OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
    PhotonNetwork.JoinRandomRoom();
  }

  public virtual void OnFailedToConnectToPhoton(DisconnectCause cause) => Debug.LogError((object) ("Cause: " + cause.ToString()));

  public virtual void OnJoinedLobby() => Debug.Log((object) "OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");

  public void OnJoinedRoom() => Debug.Log((object) "OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");

  public virtual void OnPhotonRandomJoinFailed()
  {
    Debug.Log((object) "OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
    PhotonNetwork.CreateRoom((string) null, new RoomOptions()
    {
      maxPlayers = 4
    }, (TypedLobby) null);
  }

  public virtual void Start() => PhotonNetwork.autoJoinLobby = false;

  public virtual void Update()
  {
    if (!this.ConnectInUpdate || !this.AutoConnect || PhotonNetwork.connected)
      return;
    Debug.Log((object) "Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
    this.ConnectInUpdate = false;
    PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel.ToString());
  }
}
