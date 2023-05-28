// Decompiled with JetBrains decompiler
// Type: Settings.SaveableSettingsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.IO;
using UnityEngine;

namespace Settings
{
  internal abstract class SaveableSettingsContainer : BaseSettingsContainer
  {
    protected virtual string FolderPath => Application.dataPath + "/UserData/Settings";

    protected abstract string FileName { get; }

    protected virtual bool Encrypted => false;

    protected override void Setup()
    {
      this.RegisterSettings();
      this.Load();
      this.Apply();
    }

    public virtual void Save()
    {
      Directory.CreateDirectory(this.FolderPath);
      string str = this.SerializeToJsonString();
      if (this.Encrypted)
        str = new SimpleAES().Encrypt(str);
      File.WriteAllText(this.GetFilePath(), str);
    }

    public virtual void Load()
    {
      string filePath = this.GetFilePath();
      if (File.Exists(filePath))
      {
        string str = File.ReadAllText(filePath);
        if (this.Encrypted)
          str = new SimpleAES().Decrypt(str);
        this.DeserializeFromJsonString(str);
      }
      else
      {
        try
        {
          this.LoadLegacy();
        }
        catch
        {
          Debug.Log((object) "Exception occurred while loading legacy settings.");
        }
      }
    }

    protected virtual void LoadLegacy()
    {
    }

    protected virtual string GetFilePath() => this.FolderPath + "/" + this.FileName;
  }
}
