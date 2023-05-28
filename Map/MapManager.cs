// Decompiled with JetBrains decompiler
// Type: Map.MapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Map
{
  internal class MapManager : MonoBehaviour
  {
    private static MapManager _instance;

    public static void Init() => MapManager._instance = SingletonFactory.CreateSingleton<MapManager>(MapManager._instance);

    public static void LoadObjects(List<MapScriptGameObject> objects)
    {
      foreach (MapScriptGameObject scriptGameObject in objects)
        ;
    }
  }
}
