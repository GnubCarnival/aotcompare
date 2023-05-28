// Decompiled with JetBrains decompiler
// Type: Room
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomInfo
{
  internal Room(string roomName, RoomOptions options)
    : base(roomName, (Hashtable) null)
  {
    if (options == null)
      options = new RoomOptions();
    this.visibleField = options.isVisible;
    this.openField = options.isOpen;
    this.maxPlayersField = (byte) options.maxPlayers;
    this.autoCleanUpField = false;
    this.CacheProperties(options.customRoomProperties);
    this.propertiesListedInLobby = options.customRoomPropertiesForLobby;
  }

  public void SetCustomProperties(Hashtable propertiesToSet)
  {
    if (propertiesToSet == null)
      return;
    ((IDictionary) this.customProperties).MergeStringKeys((IDictionary) propertiesToSet);
    ((IDictionary) this.customProperties).StripKeysWithNullValues();
    Hashtable stringKeys = ((IDictionary) propertiesToSet).StripToStringKeys();
    if (!PhotonNetwork.offlineMode)
      PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfRoom(stringKeys, true, (byte) 0);
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, (object) propertiesToSet);
  }

  public void SetPropertiesListedInLobby(string[] propsListedInLobby)
  {
    PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable()
    {
      [(object) (byte) 250] = (object) propsListedInLobby
    }, false, (byte) 0);
    this.propertiesListedInLobby = propsListedInLobby;
  }

  public override string ToString() => string.Format("Room: '{0}' {1},{2} {4}/{3} players.", (object) this.nameField, !this.visibleField ? (object) "hidden" : (object) "visible", !this.openField ? (object) "closed" : (object) "open", (object) this.maxPlayersField, (object) this.playerCount);

  public new string ToStringFull() => string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", (object) this.nameField, !this.visibleField ? (object) "hidden" : (object) "visible", !this.openField ? (object) "closed" : (object) "open", (object) this.maxPlayersField, (object) this.playerCount, (object) ((IDictionary) this.customProperties).ToStringFull());

  public bool autoCleanUp => this.autoCleanUpField;

  public int maxPlayers
  {
    get => (int) this.maxPlayersField;
    set
    {
      if (!this.Equals((object) PhotonNetwork.room))
        Debug.LogWarning((object) "Can't set maxPlayers when not in that room.");
      if (value > (int) byte.MaxValue)
      {
        Debug.LogWarning((object) ("Can't set Room.maxPlayers to: " + value.ToString() + ". Using max value: 255."));
        value = (int) byte.MaxValue;
      }
      if (value != (int) this.maxPlayersField && !PhotonNetwork.offlineMode)
      {
        Hashtable gameProperties = new Hashtable();
        ((Dictionary<object, object>) gameProperties).Add((object) byte.MaxValue, (object) (byte) value);
        PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, (byte) 0);
      }
      this.maxPlayersField = (byte) value;
    }
  }

  public new string name
  {
    get => this.nameField;
    internal set => this.nameField = value;
  }

  public new bool open
  {
    get => this.openField;
    set
    {
      if (!this.Equals((object) PhotonNetwork.room))
        Debug.LogWarning((object) "Can't set open when not in that room.");
      if (value != this.openField && !PhotonNetwork.offlineMode)
      {
        Hashtable gameProperties = new Hashtable();
        ((Dictionary<object, object>) gameProperties).Add((object) (byte) 253, (object) value);
        PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, (byte) 0);
      }
      this.openField = value;
    }
  }

  public new int playerCount => PhotonNetwork.playerList != null ? PhotonNetwork.playerList.Length : 0;

  public string[] propertiesListedInLobby { get; private set; }

  public new bool visible
  {
    get => this.visibleField;
    set
    {
      if (!this.Equals((object) PhotonNetwork.room))
        Debug.LogWarning((object) "Can't set visible when not in that room.");
      if (value != this.visibleField && !PhotonNetwork.offlineMode)
      {
        Hashtable gameProperties = new Hashtable();
        ((Dictionary<object, object>) gameProperties).Add((object) (byte) 254, (object) value);
        PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, (byte) 0);
      }
      this.visibleField = value;
    }
  }
}
