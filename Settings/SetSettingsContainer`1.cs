// Decompiled with JetBrains decompiler
// Type: Settings.SetSettingsContainer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;

namespace Settings
{
  internal class SetSettingsContainer<T> : BaseSettingsContainer, ISetSettingsContainer where T : BaseSetSetting, new()
  {
    public IntSetting SelectedSetIndex = new IntSetting(0, 0);
    public ListSetting<T> Sets = new ListSetting<T>(new T());

    protected override bool Validate() => this.Sets.GetCount() > 0;

    public BaseSetSetting GetSelectedSet() => (BaseSetSetting) this.Sets.GetItemAt(Math.Max(Math.Min(this.SelectedSetIndex.Value, this.Sets.GetCount() - 1), 0));

    public IntSetting GetSelectedSetIndex() => this.SelectedSetIndex;

    public void CreateSet(string name)
    {
      T obj = new T();
      obj.Name.Value = name;
      this.Sets.Value.Add(obj);
    }

    public void CopySelectedSet(string name)
    {
      T obj = new T();
      obj.Copy((BaseSetting) this.GetSelectedSet());
      obj.Name.Value = name;
      obj.Preset.Value = false;
      this.Sets.Value.Add(obj);
    }

    public bool CanDeleteSelectedSet() => this.Sets.GetCount() > 1 && this.CanEditSelectedSet();

    public bool CanEditSelectedSet() => !this.GetSelectedSet().Preset.Value;

    public void DeleteSelectedSet() => this.Sets.Value.Remove((T) this.GetSelectedSet());

    public IListSetting GetSets() => (IListSetting) this.Sets;

    public void SetPresetsFromJsonString(string json)
    {
      SetSettingsContainer<T> settingsContainer = new SetSettingsContainer<T>();
      settingsContainer.DeserializeFromJsonString(json);
      this.Sets.Value.RemoveAll((Predicate<T>) (x => x.Preset.Value));
      for (int index = 0; index < settingsContainer.Sets.Value.Count; ++index)
      {
        settingsContainer.Sets.Value[index].Preset.Value = true;
        this.Sets.Value.Insert(index, settingsContainer.Sets.Value[index]);
      }
    }

    public string[] GetSetNames()
    {
      List<string> stringList = new List<string>();
      foreach (BaseSetSetting baseSetSetting in this.Sets.GetItems())
        stringList.Add(baseSetSetting.Name.Value);
      return stringList.ToArray();
    }
  }
}
