// Decompiled with JetBrains decompiler
// Type: GameObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public static class GameObjectExtensions
{
  public static bool GetActive(this GameObject target) => target.activeInHierarchy;
}
