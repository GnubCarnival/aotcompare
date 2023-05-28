// Decompiled with JetBrains decompiler
// Type: PBitStream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;

public class PBitStream
{
  private int currentByte;
  private List<byte> streamBytes;
  private int totalBits;

  public PBitStream() => this.streamBytes = new List<byte>(1);

  public PBitStream(int bitCount) => this.streamBytes = new List<byte>(PBitStream.BytesForBits(bitCount));

  public PBitStream(IEnumerable<byte> bytes, int bitCount)
  {
    this.streamBytes = new List<byte>(bytes);
    this.BitCount = bitCount;
  }

  public void Add(bool val)
  {
    int index = this.totalBits / 8;
    if (index > this.streamBytes.Count - 1 || this.totalBits == 0)
      this.streamBytes.Add((byte) 0);
    if (val)
    {
      int num = 7 - this.totalBits % 8;
      this.streamBytes[index] |= (byte) (1 << num);
    }
    ++this.totalBits;
  }

  public static int BytesForBits(int bitCount) => bitCount <= 0 ? 0 : (bitCount - 1) / 8 + 1;

  public bool Get(int bitIndex) => ((int) this.streamBytes[bitIndex / 8] & (int) (byte) (1 << 7 - bitIndex % 8)) > 0;

  public bool GetNext()
  {
    if (this.Position > this.totalBits)
      throw new Exception("End of PBitStream reached. Can't read more.");
    int position;
    this.Position = (position = this.Position) + 1;
    return this.Get(position);
  }

  public void Set(int bitIndex, bool value) => this.streamBytes[bitIndex / 8] |= (byte) (1 << 7 - bitIndex % 8);

  public byte[] ToBytes() => this.streamBytes.ToArray();

  public int BitCount
  {
    get => this.totalBits;
    private set => this.totalBits = value;
  }

  public int ByteCount => PBitStream.BytesForBits(this.totalBits);

  public int Position { get; set; }
}
