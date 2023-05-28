// Decompiled with JetBrains decompiler
// Type: SimpleJSONFixed.JSONNodeType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace SimpleJSONFixed
{
  public enum JSONNodeType
  {
    Array = 1,
    Object = 2,
    String = 3,
    Number = 4,
    NullValue = 5,
    Boolean = 6,
    None = 7,
    Custom = 255, // 0x000000FF
  }
}
