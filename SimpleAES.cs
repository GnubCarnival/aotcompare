// Decompiled with JetBrains decompiler
// Type: SimpleAES
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SimpleAES
{
  private ICryptoTransform decryptor;
  private UTF8Encoding encoder;
  private ICryptoTransform encryptor;
  private static byte[] key = new byte[32]
  {
    (byte) 123,
    (byte) 217,
    (byte) 19,
    (byte) 11,
    (byte) 24,
    (byte) 26,
    (byte) 85,
    (byte) 45,
    (byte) 114,
    (byte) 184,
    (byte) 27,
    (byte) 162,
    (byte) 37,
    (byte) 112,
    (byte) 222,
    (byte) 209,
    (byte) 241,
    (byte) 24,
    (byte) 175,
    (byte) 144,
    (byte) 173,
    (byte) 53,
    (byte) 196,
    (byte) 29,
    (byte) 24,
    (byte) 26,
    (byte) 17,
    (byte) 218,
    (byte) 131,
    (byte) 236,
    (byte) 53,
    (byte) 209
  };
  private static byte[] vector = new byte[16]
  {
    (byte) 146,
    (byte) 64,
    (byte) 191,
    (byte) 111,
    (byte) 23,
    (byte) 3,
    (byte) 113,
    (byte) 119,
    (byte) 231,
    (byte) 121,
    (byte) 221,
    (byte) 112,
    (byte) 79,
    (byte) 32,
    (byte) 114,
    (byte) 156
  };

  public SimpleAES()
  {
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    this.encryptor = rijndaelManaged.CreateEncryptor(SimpleAES.key, SimpleAES.vector);
    this.decryptor = rijndaelManaged.CreateDecryptor(SimpleAES.key, SimpleAES.vector);
    this.encoder = new UTF8Encoding();
  }

  public string Decrypt(string encrypted) => this.encoder.GetString(this.Decrypt(Convert.FromBase64String(encrypted)));

  public byte[] Decrypt(byte[] buffer) => this.Transform(buffer, this.decryptor);

  public string Encrypt(string unencrypted) => Convert.ToBase64String(this.Encrypt(this.encoder.GetBytes(unencrypted)));

  public byte[] Encrypt(byte[] buffer) => this.Transform(buffer, this.encryptor);

  protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
  {
    MemoryStream memoryStream = new MemoryStream();
    using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, transform, CryptoStreamMode.Write))
      cryptoStream.Write(buffer, 0, buffer.Length);
    return memoryStream.ToArray();
  }
}
