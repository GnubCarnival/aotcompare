// Decompiled with JetBrains decompiler
// Type: CustomTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using UnityEngine;

internal static class CustomTypes
{
  public const byte PhotonPlayerCode = 80;
  public const short PhotonPlayerLength = 4;
  public const byte QuaternionCode = 81;
  public const short QuaternionLength = 16;
  public const byte Vector2Code = 87;
  public const short Vector2Length = 8;
  public const byte Vector3Code = 86;
  public const short Vector3Length = 12;

  private static object DeserializePhotonPlayer(StreamBuffer buff, short length)
  {
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance((int) length, ref num);
    int ID;
    Protocol.Deserialize(ref ID, bufferAndAdvance, ref num);
    return (object) PhotonPlayer.Find(ID) ?? (object) PhotonNetwork.player;
  }

  private static object DeserializeQuaternion(StreamBuffer buff, short length)
  {
    Quaternion quaternion = new Quaternion();
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance((int) length, ref num);
    Protocol.Deserialize(ref quaternion.w, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref quaternion.x, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref quaternion.y, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref quaternion.z, bufferAndAdvance, ref num);
    return (object) quaternion;
  }

  private static object DeserializeVector2(StreamBuffer buff, short length)
  {
    Vector2 vector2 = new Vector2();
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance((int) length, ref num);
    Protocol.Deserialize(ref vector2.x, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref vector2.y, bufferAndAdvance, ref num);
    return (object) vector2;
  }

  private static object DeserializeVector3(StreamBuffer buff, short length)
  {
    Vector3 vector3 = new Vector3();
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance((int) length, ref num);
    Protocol.Deserialize(ref vector3.x, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref vector3.y, bufferAndAdvance, ref num);
    Protocol.Deserialize(ref vector3.z, bufferAndAdvance, ref num);
    return (object) vector3;
  }

  private static short SerializePhotonPlayer(StreamBuffer buff, object customobject)
  {
    int num;
    Protocol.Serialize(((PhotonPlayer) customobject).ID, buff.GetBufferAndAdvance(4, ref num), ref num);
    return 4;
  }

  private static short SerializeQuaternion(StreamBuffer buff, object obj)
  {
    Quaternion quaternion = (Quaternion) obj;
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance(16, ref num);
    Protocol.Serialize(quaternion.w, bufferAndAdvance, ref num);
    Protocol.Serialize(quaternion.x, bufferAndAdvance, ref num);
    Protocol.Serialize(quaternion.y, bufferAndAdvance, ref num);
    Protocol.Serialize(quaternion.z, bufferAndAdvance, ref num);
    return 16;
  }

  private static short SerializeVector2(StreamBuffer buff, object customobject)
  {
    Vector2 vector2 = (Vector2) customobject;
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance(8, ref num);
    Protocol.Serialize(vector2.x, bufferAndAdvance, ref num);
    Protocol.Serialize(vector2.y, bufferAndAdvance, ref num);
    return 8;
  }

  private static short SerializeVector3(StreamBuffer buff, object customobject)
  {
    Vector3 vector3 = (Vector3) customobject;
    int num;
    byte[] bufferAndAdvance = buff.GetBufferAndAdvance(12, ref num);
    Protocol.Serialize(vector3.x, bufferAndAdvance, ref num);
    Protocol.Serialize(vector3.y, bufferAndAdvance, ref num);
    Protocol.Serialize(vector3.z, bufferAndAdvance, ref num);
    return 12;
  }

  public static void Register()
  {
    // ISSUE: method pointer
    // ISSUE: method pointer
    PhotonPeer.RegisterType(typeof (Quaternion), (byte) 81, new SerializeStreamMethod((object) null, __methodptr(SerializeQuaternion)), new DeserializeStreamMethod((object) null, __methodptr(DeserializeQuaternion)));
    // ISSUE: method pointer
    // ISSUE: method pointer
    PhotonPeer.RegisterType(typeof (Vector2), (byte) 87, new SerializeStreamMethod((object) null, __methodptr(SerializeVector2)), new DeserializeStreamMethod((object) null, __methodptr(DeserializeVector2)));
    // ISSUE: method pointer
    // ISSUE: method pointer
    PhotonPeer.RegisterType(typeof (Vector3), (byte) 86, new SerializeStreamMethod((object) null, __methodptr(SerializeVector3)), new DeserializeStreamMethod((object) null, __methodptr(DeserializeVector3)));
    // ISSUE: method pointer
    // ISSUE: method pointer
    PhotonPeer.RegisterType(typeof (PhotonPlayer), (byte) 80, new SerializeStreamMethod((object) null, __methodptr(SerializePhotonPlayer)), new DeserializeStreamMethod((object) null, __methodptr(DeserializePhotonPlayer)));
  }
}
