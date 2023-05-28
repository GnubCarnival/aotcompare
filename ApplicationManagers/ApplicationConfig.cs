﻿// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.ApplicationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.IO;
using UnityEngine;

namespace ApplicationManagers
{
  internal class ApplicationConfig
  {
    private static readonly string DevelopmentConfigPath = Application.dataPath + "/DevelopmentConfig";
    public static bool DevelopmentMode = false;
    public const string LauncherVersion = "1.0";
    public const int AssetBundleVersion = 20211122;
    public const string GameVersion = "5/5/2022";

    public static void Init()
    {
      if (!File.Exists(ApplicationConfig.DevelopmentConfigPath))
        return;
      ApplicationConfig.DevelopmentMode = true;
    }
  }
}
