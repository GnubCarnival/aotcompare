// Decompiled with JetBrains decompiler
// Type: RoomInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;

public class RoomInfo
{
  protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;
  private Hashtable customPropertiesField = new Hashtable();
  protected byte maxPlayersField;
  protected string nameField;
  protected bool openField = true;
  protected bool visibleField = true;

  protected internal RoomInfo(string roomName, Hashtable properties)
  {
    this.CacheProperties(properties);
    this.nameField = roomName;
  }

  protected internal void CacheProperties(Hashtable propertiesToCache)
  {
    if (propertiesToCache == null || ((Dictionary<object, object>) propertiesToCache).Count == 0 || this.customPropertiesField.Equals((object) propertiesToCache))
      return;
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) (byte) 251))
    {
      this.removedFromList = (bool) propertiesToCache[(object) (byte) 251];
      if (this.removedFromList)
        return;
    }
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) byte.MaxValue))
      this.maxPlayersField = (byte) propertiesToCache[(object) byte.MaxValue];
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) (byte) 253))
      this.openField = (bool) propertiesToCache[(object) (byte) 253];
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) (byte) 254))
      this.visibleField = (bool) propertiesToCache[(object) (byte) 254];
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) (byte) 252))
      this.playerCount = (int) (byte) propertiesToCache[(object) (byte) 252];
    if (((Dictionary<object, object>) propertiesToCache).ContainsKey((object) (byte) 249))
      this.autoCleanUpField = (bool) propertiesToCache[(object) (byte) 249];
    ((IDictionary) this.customPropertiesField).MergeStringKeys((IDictionary) propertiesToCache);
  }

  public override bool Equals(object p) => p is Room room && this.nameField.Equals(room.nameField);

  public override int GetHashCode() => this.nameField.GetHashCode();

  public override string ToString() => string.Format("Room: '{0}' {1},{2} {4}/{3} players.", (object) this.nameField, !this.visibleField ? (object) "hidden" : (object) "visible", !this.openField ? (object) "closed" : (object) "open", (object) this.maxPlayersField, (object) this.playerCount);

  public string ToStringFull() => string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", (object) this.nameField, !this.visibleField ? (object) "hidden" : (object) "visible", !this.openField ? (object) "closed" : (object) "open", (object) this.maxPlayersField, (object) this.playerCount, (object) ((IDictionary) this.customPropertiesField).ToStringFull());

  public Hashtable customProperties => this.customPropertiesField;

  public bool isLocalClientInside { get; set; }

  public byte maxPlayers => this.maxPlayersField;

  public string name => this.nameField;

  public bool open => this.openField;

  public int playerCount { get; private set; }

  public bool removedFromList { get; internal set; }

  public bool visible => this.visibleField;
}
