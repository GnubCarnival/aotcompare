// Decompiled with JetBrains decompiler
// Type: PhotonStream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

public class PhotonStream
{
  private byte currentItem;
  internal List<object> data;
  private bool write;

  public PhotonStream(bool write, object[] incomingData)
  {
    this.write = write;
    if (incomingData == null)
      this.data = new List<object>();
    else
      this.data = new List<object>((IEnumerable<object>) incomingData);
  }

  public object ReceiveNext()
  {
    if (this.write)
    {
      Debug.LogError((object) "Error: you cannot read this stream that you are writing!");
      return (object) null;
    }
    object next = this.data[(int) this.currentItem];
    ++this.currentItem;
    return next;
  }

  public void SendNext(object obj)
  {
    if (!this.write)
      Debug.LogError((object) "Error: you cannot write/send to this stream that you are reading!");
    else
      this.data.Add(obj);
  }

  public void Serialize(ref PhotonPlayer obj)
  {
    if (this.write)
    {
      this.data.Add((object) obj);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      obj = (PhotonPlayer) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref bool myBool)
  {
    if (this.write)
    {
      this.data.Add((object) myBool);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      myBool = (bool) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref char value)
  {
    if (this.write)
    {
      this.data.Add((object) value);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      value = (char) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref short value)
  {
    if (this.write)
    {
      this.data.Add((object) value);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      value = (short) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref int myInt)
  {
    if (this.write)
    {
      this.data.Add((object) myInt);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      myInt = (int) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref float obj)
  {
    if (this.write)
    {
      this.data.Add((object) obj);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      obj = (float) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref string value)
  {
    if (this.write)
    {
      this.data.Add((object) value);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      value = (string) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref Quaternion obj)
  {
    if (this.write)
    {
      this.data.Add((object) obj);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      obj = (Quaternion) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref Vector2 obj)
  {
    if (this.write)
    {
      this.data.Add((object) obj);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      obj = (Vector2) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public void Serialize(ref Vector3 obj)
  {
    if (this.write)
    {
      this.data.Add((object) obj);
    }
    else
    {
      if (this.data.Count <= (int) this.currentItem)
        return;
      obj = (Vector3) this.data[(int) this.currentItem];
      ++this.currentItem;
    }
  }

  public object[] ToArray() => this.data.ToArray();

  public int Count => this.data.Count;

  public bool isReading => !this.write;

  public bool isWriting => this.write;
}
