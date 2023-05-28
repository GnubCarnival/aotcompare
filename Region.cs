// Decompiled with JetBrains decompiler
// Type: Region
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;

public class Region
{
  public CloudRegionCode Code;
  public string HostAndPort;
  public int Ping;

  public static CloudRegionCode Parse(string codeAsString)
  {
    codeAsString = codeAsString.ToLower();
    CloudRegionCode cloudRegionCode = CloudRegionCode.none;
    if (Enum.IsDefined(typeof (CloudRegionCode), (object) codeAsString))
      cloudRegionCode = (CloudRegionCode) Enum.Parse(typeof (CloudRegionCode), codeAsString);
    return cloudRegionCode;
  }

  public override string ToString() => string.Format("'{0}' \t{1}ms \t{2}", (object) this.Code, (object) this.Ping, (object) this.HostAndPort);
}
