// Decompiled with JetBrains decompiler
// Type: Utility.SingletonFactory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

namespace Utility
{
  internal class SingletonFactory : MonoBehaviour
  {
    public static T CreateSingleton<T>(T instance) where T : Component
    {
      if (Object.op_Inequality((Object) (object) instance, (Object) null))
        throw new Exception(string.Format("Attempting to create duplicate singleton of {0}", (object) typeof (T).Name));
      GameObject gameObject = new GameObject();
      instance = gameObject.AddComponent<T>();
      Object.DontDestroyOnLoad((Object) gameObject);
      return instance;
    }
  }
}
