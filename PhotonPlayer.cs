// Decompiled with JetBrains decompiler
// Type: PhotonPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer
{
  private int actorID;
  public readonly bool isLocal;
  private string nameField;
  public object TagObject;

  public static void CleanProperties()
  {
    if (PhotonNetwork.player == null)
      return;
    ((Dictionary<object, object>) PhotonNetwork.player.customProperties).Clear();
    PhotonPlayer player = PhotonNetwork.player;
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.name, (object) LoginFengKAI.player.name);
    player.SetCustomProperties(propertiesToSet);
  }

  protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
  {
    this.actorID = -1;
    this.nameField = string.Empty;
    this.customProperties = new Hashtable();
    this.isLocal = isLocal;
    this.actorID = actorID;
    this.InternalCacheProperties(properties);
  }

  public PhotonPlayer(bool isLocal, int actorID, string name)
  {
    this.actorID = -1;
    this.nameField = string.Empty;
    this.customProperties = new Hashtable();
    this.isLocal = isLocal;
    this.actorID = actorID;
    this.nameField = name;
  }

  public override bool Equals(object p) => p is PhotonPlayer photonPlayer && this.GetHashCode() == photonPlayer.GetHashCode();

  public static PhotonPlayer Find(int ID)
  {
    for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
    {
      PhotonPlayer player = PhotonNetwork.playerList[index];
      if (player.ID == ID)
        return player;
    }
    return (PhotonPlayer) null;
  }

  public Hashtable ChangeLocalPlayer(int NewID, string inputname)
  {
    this.actorID = NewID;
    string str;
    this.nameField = str = string.Format("{0}_ID:{1}", (object) inputname, (object) NewID);
    Hashtable hashtable = new Hashtable();
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.name, (object) str);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.guildName, (object) LoginFengKAI.player.guildname);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.kills, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.max_dmg, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.total_dmg, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.deaths, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.dead, (object) true);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.isTitan, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.RCteam, (object) 0);
    ((Dictionary<object, object>) hashtable).Add((object) PhotonPlayerProperty.currentLevel, (object) string.Empty);
    Hashtable propertiesToSet = hashtable;
    PhotonNetwork.AllocateViewID();
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    return propertiesToSet;
  }

  public PhotonPlayer Get(int id) => PhotonPlayer.Find(id);

  public override int GetHashCode() => this.ID;

  public PhotonPlayer GetNext() => this.GetNextFor(this.ID);

  public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer) => currentPlayer == null ? (PhotonPlayer) null : this.GetNextFor(currentPlayer.ID);

  public PhotonPlayer GetNextFor(int currentPlayerId)
  {
    if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
      return (PhotonPlayer) null;
    Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
    int key1 = int.MaxValue;
    int key2 = currentPlayerId;
    foreach (int key3 in mActors.Keys)
    {
      if (key3 < key2)
        key2 = key3;
      else if (key3 > currentPlayerId && key3 < key1)
        key1 = key3;
    }
    return key1 != int.MaxValue ? mActors[key1] : mActors[key2];
  }

  internal void InternalCacheProperties(Hashtable properties)
  {
    if (properties == null || ((Dictionary<object, object>) properties).Count == 0 || this.customProperties.Equals((object) properties))
      return;
    if (((Dictionary<object, object>) properties).ContainsKey((object) byte.MaxValue))
      this.nameField = (string) properties[(object) byte.MaxValue];
    ((IDictionary) this.customProperties).MergeStringKeys((IDictionary) properties);
    ((IDictionary) this.customProperties).StripKeysWithNullValues();
  }

  internal void InternalChangeLocalID(int newID)
  {
    if (!this.isLocal)
      Debug.LogError((object) "ERROR You should never change PhotonPlayer IDs!");
    else
      this.actorID = newID;
  }

  public void SetCustomProperties(Hashtable propertiesToSet)
  {
    if (propertiesToSet == null)
      return;
    ((IDictionary) this.customProperties).MergeStringKeys((IDictionary) propertiesToSet);
    ((IDictionary) this.customProperties).StripKeysWithNullValues();
    Hashtable stringKeys = ((IDictionary) propertiesToSet).StripToStringKeys();
    if (this.actorID > 0 && !PhotonNetwork.offlineMode)
      PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(this.actorID, stringKeys, true, (byte) 0);
    NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, (object) this, (object) propertiesToSet);
  }

  public override string ToString() => string.IsNullOrEmpty(this.name) ? string.Format("#{0:00}{1}", (object) this.ID, !this.isMasterClient ? (object) string.Empty : (object) "(master)") : string.Format("'{0}'{1}", (object) this.name, !this.isMasterClient ? (object) string.Empty : (object) "(master)");

  public string ToStringFull() => string.Format("#{0:00} '{1}' {2}", (object) this.ID, (object) this.name, (object) ((IDictionary) this.customProperties).ToStringFull());

  public Hashtable allProperties
  {
    get
    {
      Hashtable target = new Hashtable();
      ((IDictionary) target).Merge((IDictionary) this.customProperties);
      target[(object) byte.MaxValue] = (object) this.name;
      return target;
    }
  }

  public Hashtable customProperties { get; private set; }

  public int ID => this.actorID;

  public bool isMasterClient => PhotonNetwork.networkingPeer.mMasterClient == this;

  public string name
  {
    get => this.nameField;
    set
    {
      if (!this.isLocal)
        Debug.LogError((object) "Error: Cannot change the name of a remote player!");
      else
        this.nameField = value;
    }
  }
}
