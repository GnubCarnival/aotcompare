// Decompiled with JetBrains decompiler
// Type: EanFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.IO;

internal class EanFile
{
  public int AnimCount;
  public EanAnimation[] Anims;
  public int Header;
  public int Reserved;
  public int Version;

  public void Load(BinaryReader br, FileStream fs)
  {
    this.Header = br.ReadInt32();
    this.Version = br.ReadInt32();
    this.Reserved = br.ReadInt32();
    this.AnimCount = br.ReadInt32();
    this.Anims = new EanAnimation[this.AnimCount];
    for (int index = 0; index < this.AnimCount; ++index)
    {
      this.Anims[index] = new EanAnimation();
      this.Anims[index].Load(br, fs);
    }
  }
}
