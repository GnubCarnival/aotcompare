// Decompiled with JetBrains decompiler
// Type: Settings.PresetSettingsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Settings
{
  internal abstract class PresetSettingsContainer : SaveableSettingsContainer
  {
    protected virtual string PresetFolderPath => Application.dataPath + "/Resources/Presets";

    public override void Load()
    {
      string presetFilePath = this.GetPresetFilePath();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (File.Exists(presetFilePath))
      {
        this.DeserializeFromJsonString(File.ReadAllText(presetFilePath));
        foreach (string key in (IEnumerable) this.Settings.Keys)
          dictionary.Add(key, ((BaseSetting) this.Settings[(object) key]).SerializeToJsonString());
      }
      this.SetDefault();
      base.Load();
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        BaseSetting setting = (BaseSetting) this.Settings[(object) keyValuePair.Key];
        if (setting is ISetSettingsContainer)
          ((ISetSettingsContainer) setting).SetPresetsFromJsonString(keyValuePair.Value);
      }
    }

    protected virtual string GetPresetFilePath() => this.PresetFolderPath + "/" + this.FileName;
  }
}
