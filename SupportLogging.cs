// Decompiled with JetBrains decompiler
// Type: SupportLogging
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Text;
using UnityEngine;

public class SupportLogging : MonoBehaviour
{
  public bool LogTrafficStats;

  private void LogBasics()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", (object) "1.28");
    stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} ", (object) PhotonNetwork.networkingPeer.mAppId.Substring(0, 8), (object) PhotonNetwork.networkingPeer.mAppVersionPun);
    stringBuilder.AppendFormat("Server: {0}. Region: {1} ", (object) PhotonNetwork.ServerAddress, (object) PhotonNetwork.networkingPeer.CloudRegion);
    stringBuilder.AppendFormat("HostType: {0} ", (object) PhotonNetwork.PhotonServerSettings.HostType);
    Debug.Log((object) stringBuilder.ToString());
  }

  public void LogStats()
  {
    if (!this.LogTrafficStats)
      return;
    Debug.Log((object) ("SupportLogger " + PhotonNetwork.NetworkStatisticsToString()));
  }

  public void OnApplicationQuit() => this.CancelInvoke();

  public void OnConnectedToPhoton()
  {
    Debug.Log((object) "SupportLogger OnConnectedToPhoton().");
    this.LogBasics();
    if (!this.LogTrafficStats)
      return;
    PhotonNetwork.NetworkStatisticsEnabled = true;
  }

  public void OnCreatedRoom() => Debug.Log((object) ("SupportLogger OnCreatedRoom(" + (object) PhotonNetwork.room + "). " + (object) PhotonNetwork.lobby));

  public void OnFailedToConnectToPhoton(DisconnectCause cause)
  {
    Debug.Log((object) ("SupportLogger OnFailedToConnectToPhoton(" + cause.ToString() + ")."));
    this.LogBasics();
  }

  public void OnJoinedLobby() => Debug.Log((object) ("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby?.ToString() + ")."));

  public void OnJoinedRoom() => Debug.Log((object) ("SupportLogger OnJoinedRoom(" + (object) PhotonNetwork.room + "). " + (object) PhotonNetwork.lobby));

  public void OnLeftRoom() => Debug.Log((object) "SupportLogger OnLeftRoom().");

  public void Start()
  {
    if (!this.LogTrafficStats)
      return;
    this.InvokeRepeating("LogStats", 10f, 10f);
  }
}
