// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.SnapshotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.IO;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
  internal class SnapshotManager : MonoBehaviour
  {
    private static SnapshotManager _instance;
    public static readonly string SnapshotPath = Application.dataPath + "/UserData/Snapshots";
    private static readonly string SnapshotTempPath = Application.dataPath + "/UserData/Snapshots/Temp";
    private static readonly string SnapshotFilePrefix = "Snapshot";
    private static readonly int MaxSnapshots = 500;
    private static int _currentSnapshotSaveId = 0;
    private static int _maxSnapshotSaveId = 0;
    private static int[] _damages = new int[SnapshotManager.MaxSnapshots];

    public static void Init()
    {
      SnapshotManager._instance = SingletonFactory.CreateSingleton<SnapshotManager>(SnapshotManager._instance);
      SnapshotManager.ClearTemp();
    }

    private void OnApplicationQuit() => SnapshotManager.ClearTemp();

    private static void ClearTemp()
    {
      if (!Directory.Exists(SnapshotManager.SnapshotTempPath))
        return;
      try
      {
        Directory.Delete(SnapshotManager.SnapshotTempPath, true);
      }
      catch (Exception ex)
      {
        Debug.Log((object) string.Format("Error deleting snapshot temp folder: {0}", (object) ex.Message));
      }
    }

    private static string GetFileName(int snapshotId) => SnapshotManager.SnapshotFilePrefix + snapshotId.ToString();

    public static void AddSnapshot(Texture2D texture, int damage)
    {
      try
      {
        if (!Directory.Exists(SnapshotManager.SnapshotTempPath))
          Directory.CreateDirectory(SnapshotManager.SnapshotTempPath);
        File.WriteAllBytes(SnapshotManager.SnapshotTempPath + "/" + SnapshotManager.GetFileName(SnapshotManager._currentSnapshotSaveId), SnapshotManager.SerializeSnapshot(texture));
        SnapshotManager._damages[SnapshotManager._currentSnapshotSaveId] = damage;
        ++SnapshotManager._currentSnapshotSaveId;
        ++SnapshotManager._maxSnapshotSaveId;
        SnapshotManager._maxSnapshotSaveId = Math.Min(SnapshotManager._maxSnapshotSaveId, SnapshotManager.MaxSnapshots);
        if (SnapshotManager._currentSnapshotSaveId < SnapshotManager.MaxSnapshots)
          return;
        SnapshotManager._currentSnapshotSaveId = 0;
      }
      catch (Exception ex)
      {
        Debug.Log((object) string.Format("Exception while adding snapshot: {0}", (object) ex.Message));
      }
    }

    private static byte[] SerializeSnapshot(Texture2D texture)
    {
      Color32[] pixels32 = texture.GetPixels32();
      byte[] numArray = new byte[pixels32.Length * 3 + 8];
      int index = 0;
      foreach (byte num in BitConverter.GetBytes(((Texture) texture).width))
      {
        numArray[index] = num;
        ++index;
      }
      foreach (byte num in BitConverter.GetBytes(((Texture) texture).height))
      {
        numArray[index] = num;
        ++index;
      }
      foreach (Color32 color32 in pixels32)
      {
        numArray[index] = color32.r;
        numArray[index + 1] = color32.g;
        numArray[index + 2] = color32.b;
        index += 3;
      }
      return numArray;
    }

    private static Texture2D DeserializeSnapshot(byte[] bytes)
    {
      Texture2D texture2D = new Texture2D(BitConverter.ToInt32(bytes, 0), BitConverter.ToInt32(bytes, 4), (TextureFormat) 3, false);
      int index1 = 8;
      Color32[] color32Array = new Color32[(bytes.Length - 8) / 3];
      for (int index2 = 0; index2 < color32Array.Length; ++index2)
      {
        color32Array[index2] = new Color32(bytes[index1], bytes[index1 + 1], bytes[index1 + 2], byte.MaxValue);
        index1 += 3;
      }
      texture2D.SetPixels32(color32Array);
      texture2D.Apply();
      return texture2D;
    }

    public static void SaveSnapshotFinish(Texture2D texture, string fileName)
    {
      if (!Directory.Exists(SnapshotManager.SnapshotPath))
        Directory.CreateDirectory(SnapshotManager.SnapshotPath);
      File.WriteAllBytes(SnapshotManager.SnapshotPath + "/" + fileName, texture.EncodeToPNG());
    }

    public static int GetDamage(int index) => index >= SnapshotManager._maxSnapshotSaveId ? 0 : SnapshotManager._damages[index];

    public static Texture2D GetSnapshot(int index)
    {
      if (index >= SnapshotManager._maxSnapshotSaveId)
        return (Texture2D) null;
      string path = SnapshotManager.SnapshotTempPath + "/" + SnapshotManager.GetFileName(index);
      if (!File.Exists(path))
        return (Texture2D) null;
      Texture2D snapshot = SnapshotManager.DeserializeSnapshot(File.ReadAllBytes(path));
      FengGameManagerMKII.instance.unloadAssets();
      return snapshot;
    }

    public static int GetLength() => SnapshotManager._maxSnapshotSaveId;
  }
}
