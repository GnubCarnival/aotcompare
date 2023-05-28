// Decompiled with JetBrains decompiler
// Type: LoadbalancingPeer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;

internal class LoadbalancingPeer : PhotonPeer
{
  public LoadbalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
    : base(listener, protocolType)
  {
  }

  public virtual bool OpAuthenticate(
    string appId,
    string appVersion,
    string userId,
    AuthenticationValues authValues,
    string regionCode)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpAuthenticate()");
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (authValues != null && authValues.Secret != null)
    {
      dictionary[(byte) 221] = (object) authValues.Secret;
      return this.OpCustom((byte) 230, dictionary, true, (byte) 0, false);
    }
    dictionary[(byte) 220] = (object) appVersion;
    dictionary[(byte) 224] = (object) appId;
    if (!string.IsNullOrEmpty(regionCode))
      dictionary[(byte) 210] = (object) regionCode;
    if (!string.IsNullOrEmpty(userId))
      dictionary[(byte) 225] = (object) userId;
    if (authValues != null && authValues.AuthType != CustomAuthenticationType.None)
    {
      if (!this.IsEncryptionAvailable)
      {
        this.Listener.DebugReturn((DebugLevel) 1, "OpAuthenticate() failed. When you want Custom Authentication encryption is mandatory.");
        return false;
      }
      dictionary[(byte) 217] = (object) (byte) authValues.AuthType;
      if (!string.IsNullOrEmpty(authValues.Secret))
        dictionary[(byte) 221] = (object) authValues.Secret;
      if (!string.IsNullOrEmpty(authValues.AuthParameters))
        dictionary[(byte) 216] = (object) authValues.AuthParameters;
      if (authValues.AuthPostData != null)
        dictionary[(byte) 214] = authValues.AuthPostData;
    }
    int num = this.OpCustom((byte) 230, dictionary, true, (byte) 0, this.IsEncryptionAvailable) ? 1 : 0;
    if (num != 0)
      return num != 0;
    this.Listener.DebugReturn((DebugLevel) 1, "Error calling OpAuthenticate! Did not work. Check log output, CustomAuthenticationValues and if you're connected.");
    return num != 0;
  }

  public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
  {
    if (this.DebugOut >= 5)
      this.Listener.DebugReturn((DebugLevel) 5, "OpChangeGroups()");
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (groupsToRemove != null)
      dictionary[(byte) 239] = (object) groupsToRemove;
    if (groupsToAdd != null)
      dictionary[(byte) 238] = (object) groupsToAdd;
    return this.OpCustom((byte) 248, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpCreateRoom(
    string roomName,
    RoomOptions roomOptions,
    TypedLobby lobby,
    Hashtable playerProperties,
    bool onGameServer)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpCreateRoom()");
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (!string.IsNullOrEmpty(roomName))
      dictionary[byte.MaxValue] = (object) roomName;
    if (lobby != null)
    {
      dictionary[(byte) 213] = (object) lobby.Name;
      dictionary[(byte) 212] = (object) (byte) lobby.Type;
    }
    if (onGameServer)
    {
      if (playerProperties != null && ((Dictionary<object, object>) playerProperties).Count > 0)
      {
        dictionary[(byte) 249] = (object) playerProperties;
        dictionary[(byte) 250] = (object) true;
      }
      if (roomOptions == null)
        roomOptions = new RoomOptions();
      Hashtable target = new Hashtable();
      dictionary[(byte) 248] = (object) target;
      ((IDictionary) target).MergeStringKeys((IDictionary) roomOptions.customRoomProperties);
      target[(object) (byte) 253] = (object) roomOptions.isOpen;
      target[(object) (byte) 254] = (object) roomOptions.isVisible;
      target[(object) (byte) 250] = (object) roomOptions.customRoomPropertiesForLobby;
      if (roomOptions.maxPlayers > 0)
        target[(object) byte.MaxValue] = (object) roomOptions.maxPlayers;
      if (roomOptions.cleanupCacheOnLeave)
      {
        dictionary[(byte) 241] = (object) true;
        target[(object) (byte) 249] = (object) true;
      }
    }
    return this.OpCustom((byte) 227, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpFindFriends(string[] friendsToFind)
  {
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (friendsToFind != null && friendsToFind.Length != 0)
      dictionary[(byte) 1] = (object) friendsToFind;
    return this.OpCustom((byte) 222, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpGetRegions(string appId) => this.OpCustom((byte) 220, new Dictionary<byte, object>()
  {
    [(byte) 224] = (object) appId
  }, true, (byte) 0, true);

  public virtual bool OpJoinLobby(TypedLobby lobby)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpJoinLobby()");
    Dictionary<byte, object> dictionary = (Dictionary<byte, object>) null;
    if (lobby != null && !lobby.IsDefault)
    {
      dictionary = new Dictionary<byte, object>();
      dictionary[(byte) 213] = (object) lobby.Name;
      dictionary[(byte) 212] = (object) (byte) lobby.Type;
    }
    return this.OpCustom((byte) 229, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpJoinRandomRoom(
    Hashtable expectedCustomRoomProperties,
    byte expectedMaxPlayers,
    Hashtable playerProperties,
    MatchmakingMode matchingType,
    TypedLobby typedLobby,
    string sqlLobbyFilter)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpJoinRandomRoom()");
    Hashtable target = new Hashtable();
    ((IDictionary) target).MergeStringKeys((IDictionary) expectedCustomRoomProperties);
    if (expectedMaxPlayers > (byte) 0)
      target[(object) byte.MaxValue] = (object) expectedMaxPlayers;
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (((Dictionary<object, object>) target).Count > 0)
      dictionary[(byte) 248] = (object) target;
    if (playerProperties != null && ((Dictionary<object, object>) playerProperties).Count > 0)
      dictionary[(byte) 249] = (object) playerProperties;
    if (matchingType != MatchmakingMode.FillRoom)
      dictionary[(byte) 223] = (object) (byte) matchingType;
    if (typedLobby != null)
    {
      dictionary[(byte) 213] = (object) typedLobby.Name;
      dictionary[(byte) 212] = (object) (byte) typedLobby.Type;
    }
    if (!string.IsNullOrEmpty(sqlLobbyFilter))
      dictionary[(byte) 245] = (object) sqlLobbyFilter;
    return this.OpCustom((byte) 225, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpJoinRoom(
    string roomName,
    RoomOptions roomOptions,
    TypedLobby lobby,
    bool createIfNotExists,
    Hashtable playerProperties,
    bool onGameServer)
  {
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    if (!string.IsNullOrEmpty(roomName))
      dictionary[byte.MaxValue] = (object) roomName;
    if (createIfNotExists)
    {
      dictionary[(byte) 215] = (object) true;
      if (lobby != null)
      {
        dictionary[(byte) 213] = (object) lobby.Name;
        dictionary[(byte) 212] = (object) (byte) lobby.Type;
      }
    }
    if (onGameServer)
    {
      if (playerProperties != null && ((Dictionary<object, object>) playerProperties).Count > 0)
      {
        dictionary[(byte) 249] = (object) playerProperties;
        dictionary[(byte) 250] = (object) true;
      }
      if (createIfNotExists)
      {
        if (roomOptions == null)
          roomOptions = new RoomOptions();
        Hashtable target = new Hashtable();
        dictionary[(byte) 248] = (object) target;
        ((IDictionary) target).MergeStringKeys((IDictionary) roomOptions.customRoomProperties);
        target[(object) (byte) 253] = (object) roomOptions.isOpen;
        target[(object) (byte) 254] = (object) roomOptions.isVisible;
        target[(object) (byte) 250] = (object) roomOptions.customRoomPropertiesForLobby;
        if (roomOptions.maxPlayers > 0)
          target[(object) byte.MaxValue] = (object) roomOptions.maxPlayers;
        if (roomOptions.cleanupCacheOnLeave)
        {
          dictionary[(byte) 241] = (object) true;
          target[(object) (byte) 249] = (object) true;
        }
      }
    }
    return this.OpCustom((byte) 226, dictionary, true, (byte) 0, false);
  }

  public virtual bool OpLeaveLobby()
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpLeaveLobby()");
    return this.OpCustom((byte) 228, (Dictionary<byte, object>) null, true, (byte) 0, false);
  }

  public virtual bool OpRaiseEvent(
    byte eventCode,
    object customEventContent,
    bool sendReliable,
    RaiseEventOptions raiseEventOptions)
  {
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    dictionary[(byte) 244] = (object) eventCode;
    if (customEventContent != null)
      dictionary[(byte) 245] = customEventContent;
    if (raiseEventOptions == null)
    {
      raiseEventOptions = RaiseEventOptions.Default;
    }
    else
    {
      if (raiseEventOptions.CachingOption != EventCaching.DoNotCache)
        dictionary[(byte) 247] = (object) (byte) raiseEventOptions.CachingOption;
      if (raiseEventOptions.Receivers != ReceiverGroup.Others)
        dictionary[(byte) 246] = (object) (byte) raiseEventOptions.Receivers;
      if (raiseEventOptions.InterestGroup != (byte) 0)
        dictionary[(byte) 240] = (object) raiseEventOptions.InterestGroup;
      if (raiseEventOptions.TargetActors != null)
        dictionary[(byte) 252] = (object) raiseEventOptions.TargetActors;
      if (raiseEventOptions.ForwardToWebhook)
        dictionary[(byte) 234] = (object) true;
    }
    return this.OpCustom((byte) 253, dictionary, sendReliable, raiseEventOptions.SequenceChannel, false);
  }

  public bool OpSetCustomPropertiesOfActor(
    int actorNr,
    Hashtable actorProperties,
    bool broadcast,
    byte channelId)
  {
    return this.OpSetPropertiesOfActor(actorNr, ((IDictionary) actorProperties).StripToStringKeys(), broadcast, channelId);
  }

  public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId) => this.OpSetPropertiesOfRoom(((IDictionary) gameProperties).StripToStringKeys(), broadcast, channelId);

  protected bool OpSetPropertiesOfActor(
    int actorNr,
    Hashtable actorProperties,
    bool broadcast,
    byte channelId)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpSetPropertiesOfActor()");
    if (actorNr <= 0 || actorProperties == null)
    {
      if (this.DebugOut >= 3)
        this.Listener.DebugReturn((DebugLevel) 3, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties != null.");
      return false;
    }
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    dictionary.Add((byte) 251, (object) actorProperties);
    dictionary.Add((byte) 254, (object) actorNr);
    if (broadcast)
      dictionary.Add((byte) 250, (object) broadcast);
    return this.OpCustom((byte) 252, dictionary, broadcast, channelId, false);
  }

  public bool OpSetPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
  {
    if (this.DebugOut >= 3)
      this.Listener.DebugReturn((DebugLevel) 3, "OpSetPropertiesOfRoom()");
    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
    dictionary.Add((byte) 251, (object) gameProperties);
    if (broadcast)
      dictionary.Add((byte) 250, (object) true);
    return this.OpCustom((byte) 252, dictionary, broadcast, channelId, false);
  }

  protected void OpSetPropertyOfRoom(byte propCode, object value) => this.OpSetPropertiesOfRoom(new Hashtable()
  {
    [(object) propCode] = value
  }, true, (byte) 0);
}
