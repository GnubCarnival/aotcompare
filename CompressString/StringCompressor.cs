// Decompiled with JetBrains decompiler
// Type: CompressString.StringCompressor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CompressString
{
  internal static class StringCompressor
  {
    public static string CompressString(string text)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(text);
      MemoryStream memoryStream = new MemoryStream();
      using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress, true))
        gzipStream.Write(bytes, 0, bytes.Length);
      memoryStream.Position = 0L;
      byte[] numArray1 = new byte[memoryStream.Length];
      memoryStream.Read(numArray1, 0, numArray1.Length);
      byte[] numArray2 = new byte[numArray1.Length + 4];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 4, numArray1.Length);
      Buffer.BlockCopy((Array) BitConverter.GetBytes(bytes.Length), 0, (Array) numArray2, 0, 4);
      return Convert.ToBase64String(numArray2);
    }

    public static string DecompressString(string compressedText)
    {
      byte[] buffer = Convert.FromBase64String(compressedText);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int int32 = BitConverter.ToInt32(buffer, 0);
        memoryStream.Write(buffer, 4, buffer.Length - 4);
        byte[] numArray = new byte[int32];
        memoryStream.Position = 0L;
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
          gzipStream.Read(numArray, 0, numArray.Length);
        return Encoding.UTF8.GetString(numArray);
      }
    }
  }
}
