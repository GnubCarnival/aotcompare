// Decompiled with JetBrains decompiler
// Type: PhotonMessageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


public class PhotonMessageInfo
{
  public PhotonView photonView;
  public PhotonPlayer sender;
  private int timeInt;

  public PhotonMessageInfo()
  {
    this.sender = PhotonNetwork.player;
    this.timeInt = (int) (PhotonNetwork.time * 1000.0);
    this.photonView = (PhotonView) null;
  }

  public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
  {
    this.sender = player;
    this.timeInt = timestamp;
    this.photonView = view;
  }

  public override string ToString() => string.Format("[PhotonMessageInfo: player='{1}' timestamp={0}]", (object) this.timestamp, (object) this.sender);

  public double timestamp => (double) this.timeInt / 1000.0;
}
