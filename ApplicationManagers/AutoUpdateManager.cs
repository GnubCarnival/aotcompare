// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.AutoUpdateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
  public class AutoUpdateManager : MonoBehaviour
  {
    public static AutoUpdateStatus Status = AutoUpdateStatus.Updating;
    public static bool CloseFailureBox = false;
    private static AutoUpdateManager _instance;
    private static readonly string RootDataPath = Application.dataPath;
    private static readonly string Platform = File.ReadAllText(AutoUpdateManager.RootDataPath + "/PlatformInfo");
    private static readonly string RootUpdateURL = "http://aottgrc.com/Patch";
    private static readonly string LauncherVersionURL = AutoUpdateManager.RootUpdateURL + "/LauncherVersion.txt";
    public static readonly string PlatformUpdateURL = AutoUpdateManager.RootUpdateURL + "/" + AutoUpdateManager.Platform;
    private static readonly string ChecksumURL = AutoUpdateManager.PlatformUpdateURL + "/Checksum.txt";

    public static void Init()
    {
      AutoUpdateManager._instance = SingletonFactory.CreateSingleton<AutoUpdateManager>(AutoUpdateManager._instance);
      AutoUpdateManager.StartUpdate();
    }

    public static void StartUpdate() => AutoUpdateManager.Status = AutoUpdateStatus.Updated;

    private IEnumerator StartUpdateCoroutine()
    {
      AutoUpdateManager.Status = AutoUpdateStatus.Updating;
      bool downloadedFile = false;
      if (Application.platform == 1 && !AutoUpdateManager.RootDataPath.Contains("Applications"))
      {
        AutoUpdateManager.Status = AutoUpdateStatus.MacTranslocated;
      }
      else
      {
        WWW www = new WWW(AutoUpdateManager.LauncherVersionURL);
        bool flag;
        try
        {
          yield return (object) www;
          if (www.error != null)
          {
            this.OnUpdateFail("Error fetching launcher version", www.error);
            flag = false;
          }
          else if (!float.TryParse(www.text, out float _))
          {
            this.OnUpdateFail("Received an invalid launcher version", www.text);
            flag = false;
          }
          else if (www.text != "1.0")
          {
            this.OnLauncherOutdated();
            flag = false;
          }
          else
            goto label_27;
          goto label_29;
        }
        finally
        {
          ((IDisposable) www)?.Dispose();
        }
label_27:
        www = (WWW) null;
        www = new WWW(AutoUpdateManager.ChecksumURL);
        List<string> list;
        try
        {
          yield return (object) www;
          if (www.error != null)
          {
            this.OnUpdateFail("Error fetching checksum", www.error);
            yield break;
          }
          else
            list = ((IEnumerable<string>) www.text.Split('\n')).ToList<string>();
        }
        finally
        {
          ((IDisposable) www)?.Dispose();
        }
        www = (WWW) null;
        foreach (string str1 in list)
        {
          char[] chArray = new char[1]{ ':' };
          string[] strArray = str1.Split(chArray);
          string fileName = strArray[0].Trim();
          string str2 = strArray[1].Trim();
          string filePath = AutoUpdateManager.RootDataPath + "/" + fileName;
          string str3;
          if (File.Exists(filePath))
          {
            try
            {
              str3 = this.GenerateMD5(filePath);
            }
            catch (Exception ex)
            {
              this.OnUpdateFail("Error generating checksum for " + fileName, ex.Message);
              flag = false;
              goto label_25;
            }
          }
          else
            str3 = string.Empty;
          if (str3 != str2)
          {
            Debug.Log((object) ("File diff found, downloading " + fileName));
            downloadedFile = true;
            www = new WWW(AutoUpdateManager.PlatformUpdateURL + "/" + fileName);
            try
            {
              yield return (object) www;
              if (www.error != null)
              {
                this.OnUpdateFail("Error fetching file " + fileName, www.error);
                flag = false;
              }
              else
              {
                try
                {
                  Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                  File.WriteAllBytes(filePath, www.bytes);
                  goto label_26;
                }
                catch (Exception ex)
                {
                  this.OnUpdateFail("Error writing file " + fileName, ex.Message);
                  flag = false;
                }
              }
              goto label_25;
            }
            finally
            {
              ((IDisposable) www)?.Dispose();
            }
label_26:
            www = (WWW) null;
          }
          fileName = (string) null;
          filePath = (string) null;
          continue;
label_25:
          goto label_29;
        }
        AutoUpdateManager.Status = !downloadedFile ? AutoUpdateStatus.Updated : AutoUpdateStatus.NeedRestart;
        yield break;
label_29:
        return flag;
      }
    }

    private void OnUpdateFail(string message, string error)
    {
      Debug.Log((object) (message + ": " + error));
      AutoUpdateManager.Status = AutoUpdateStatus.FailedUpdate;
    }

    private void OnLauncherOutdated() => AutoUpdateManager.Status = AutoUpdateStatus.LauncherOutdated;

    private string GenerateMD5(string filePath)
    {
      byte[] buffer = File.ReadAllBytes(filePath);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in MD5.Create().ComputeHash(buffer))
        stringBuilder.Append(num.ToString("X2"));
      return stringBuilder.ToString();
    }
  }
}
