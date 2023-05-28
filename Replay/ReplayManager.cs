// Decompiled with JetBrains decompiler
// Type: Replay.ReplayManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using Utility;

namespace Replay
{
  internal class ReplayManager : MonoBehaviour
  {
    private static ReplayManager _instance;

    public static void Init() => ReplayManager._instance = SingletonFactory.CreateSingleton<ReplayManager>(ReplayManager._instance);
  }
}
