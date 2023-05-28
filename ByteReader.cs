// Decompiled with JetBrains decompiler
// Type: ByteReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ByteReader
{
  private byte[] mBuffer;
  private int mOffset;

  public ByteReader(byte[] bytes) => this.mBuffer = bytes;

  public ByteReader(TextAsset asset) => this.mBuffer = asset.bytes;

  public Dictionary<string, string> ReadDictionary()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    char[] separator = new char[1]{ '=' };
    while (this.canRead)
    {
      string str1 = this.ReadLine();
      if (str1 == null)
        return dictionary;
      if (!str1.StartsWith("//"))
      {
        string[] strArray = str1.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length == 2)
        {
          string key = strArray[0].Trim();
          string str2 = strArray[1].Trim().Replace("\\n", "\n");
          dictionary[key] = str2;
        }
      }
    }
    return dictionary;
  }

  public string ReadLine()
  {
    int length = this.mBuffer.Length;
    while (this.mOffset < length && this.mBuffer[this.mOffset] < (byte) 32)
      ++this.mOffset;
    int mOffset = this.mOffset;
    if (mOffset >= length)
    {
      this.mOffset = length;
      return (string) null;
    }
    while (mOffset < length)
    {
      switch (this.mBuffer[mOffset++])
      {
        case 10:
        case 13:
          goto label_8;
        default:
          continue;
      }
    }
    ++mOffset;
label_8:
    string str = ByteReader.ReadLine(this.mBuffer, this.mOffset, mOffset - this.mOffset - 1);
    this.mOffset = mOffset;
    return str;
  }

  private static string ReadLine(byte[] buffer, int start, int count) => Encoding.UTF8.GetString(buffer, start, count);

  public bool canRead => this.mBuffer != null && this.mOffset < this.mBuffer.Length;
}
