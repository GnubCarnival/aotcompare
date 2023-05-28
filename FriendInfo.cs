// Decompiled with JetBrains decompiler
// Type: FriendInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


public class FriendInfo
{
  public override string ToString() => string.Format("{0}\t is: {1}", (object) this.Name, this.IsOnline ? (!this.IsInRoom ? (object) "on master" : (object) "playing") : (object) "offline");

  public bool IsInRoom => this.IsOnline && !string.IsNullOrEmpty(this.Room);

  public bool IsOnline { get; protected internal set; }

  public string Name { get; protected internal set; }

  public string Room { get; protected internal set; }
}
