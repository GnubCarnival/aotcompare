// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.DebugTesting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using Utility;

namespace ApplicationManagers
{
  internal class DebugTesting : MonoBehaviour
  {
    private static DebugTesting _instance;

    public static void Init() => DebugTesting._instance = SingletonFactory.CreateSingleton<DebugTesting>(DebugTesting._instance);

    public static void RunTests()
    {
      int num = ApplicationConfig.DevelopmentMode ? 1 : 0;
    }

    public static void Log(object message) => Debug.Log(message);

    private void Update()
    {
    }

    public static void RunDebugCommand(string command)
    {
      if (!ApplicationConfig.DevelopmentMode)
      {
        Debug.Log((object) "Debug commands are not available in release mode.");
      }
      else
      {
        string[] strArray1 = command.Split(' ');
        switch (strArray1[0])
        {
          case "spawnasset":
            string str = strArray1[1];
            string[] strArray2 = strArray1[2].Split(',');
            Vector3 vector3;
            // ISSUE: explicit constructor call
            ((Vector3) ref vector3).\u002Ector(float.Parse(strArray2[0]), float.Parse(strArray2[1]), float.Parse(strArray2[2]));
            string[] strArray3 = strArray1[3].Split(',');
            Quaternion quaternion;
            // ISSUE: explicit constructor call
            ((Quaternion) ref quaternion).\u002Ector(float.Parse(strArray3[0]), float.Parse(strArray3[1]), float.Parse(strArray3[2]), float.Parse(strArray3[3]));
            Object.Instantiate(FengGameManagerMKII.RCassets.Load(str), vector3, quaternion);
            break;
          default:
            Debug.Log((object) "Invalid debug command.");
            break;
        }
      }
    }
  }
}
