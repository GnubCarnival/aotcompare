﻿// Decompiled with JetBrains decompiler
// Type: Settings.MultiplayerSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
  internal class MultiplayerSettings : SaveableSettingsContainer
  {
    public static string PublicLobby = "01042015";
    public static string PrivateLobby = "verified343";
    public static string PublicAppId = "5578b046-8264-438c-99c5-fb15c71b6744";
    public IntSetting LobbyMode = new IntSetting(0);
    public IntSetting AppIdMode = new IntSetting(0);
    public StringSetting CustomLobby = new StringSetting(string.Empty);
    public StringSetting CustomAppId = new StringSetting(string.Empty);
    public StringSetting LanIP = new StringSetting(string.Empty);
    public IntSetting LanPort = new IntSetting(5055);
    public StringSetting LanPassword = new StringSetting(string.Empty);
    public MultiplayerServerType CurrentMultiplayerServerType;
    public readonly Dictionary<MultiplayerRegion, string> CloudAddresses = new Dictionary<MultiplayerRegion, string>()
    {
      {
        MultiplayerRegion.EU,
        "app-eu.exitgamescloud.com"
      },
      {
        MultiplayerRegion.US,
        "app-us.exitgamescloud.com"
      },
      {
        MultiplayerRegion.SA,
        "app-sa.exitgames.com"
      },
      {
        MultiplayerRegion.ASIA,
        "app-asia.exitgamescloud.com"
      }
    };
    public readonly Dictionary<MultiplayerRegion, string> PublicAddresses = new Dictionary<MultiplayerRegion, string>()
    {
      {
        MultiplayerRegion.EU,
        "135.125.239.180"
      },
      {
        MultiplayerRegion.US,
        "142.44.242.29"
      },
      {
        MultiplayerRegion.SA,
        "172.107.193.233"
      },
      {
        MultiplayerRegion.ASIA,
        "51.79.164.137"
      }
    };
    public readonly int DefaultPort = 5055;
    public StringSetting Name = new StringSetting("GUEST" + Random.Range(0, 100000).ToString(), 50);
    public StringSetting Guild = new StringSetting(string.Empty, 50);

    protected override string FileName => "Multiplayer.json";

    public void ConnectServer(MultiplayerRegion region)
    {
      FengGameManagerMKII.JustLeftRoom = false;
      PhotonNetwork.Disconnect();
      if (this.AppIdMode.Value == 0)
      {
        string publicAddress = this.PublicAddresses[region];
        this.CurrentMultiplayerServerType = MultiplayerServerType.Public;
        int defaultPort = this.DefaultPort;
        string empty = string.Empty;
        string currentLobby = this.GetCurrentLobby();
        PhotonNetwork.ConnectToMaster(publicAddress, defaultPort, empty, currentLobby);
      }
      else
      {
        string cloudAddress = this.CloudAddresses[region];
        this.CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
        int defaultPort = this.DefaultPort;
        string appID = this.CustomAppId.Value;
        string currentLobby = this.GetCurrentLobby();
        PhotonNetwork.ConnectToMaster(cloudAddress, defaultPort, appID, currentLobby);
      }
    }

    public string GetCurrentLobby()
    {
      if (this.LobbyMode.Value == 0)
        return MultiplayerSettings.PublicLobby;
      return this.LobbyMode.Value == 1 ? MultiplayerSettings.PrivateLobby : this.CustomLobby.Value;
    }

    public void ConnectLAN()
    {
      PhotonNetwork.Disconnect();
      if (!PhotonNetwork.ConnectToMaster(this.LanIP.Value, this.LanPort.Value, string.Empty, this.GetCurrentLobby()))
        return;
      this.CurrentMultiplayerServerType = MultiplayerServerType.LAN;
      FengGameManagerMKII.PrivateServerAuthPass = this.LanPassword.Value;
    }

    public void ConnectOffline()
    {
      PhotonNetwork.Disconnect();
      PhotonNetwork.offlineMode = true;
      this.CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
      FengGameManagerMKII.instance.OnJoinedLobby();
    }
  }
}