// Decompiled with JetBrains decompiler
// Type: Settings.BaseSettingsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Settings
{
  internal abstract class BaseSettingsContainer : BaseSetting
  {
    public OrderedDictionary Settings = new OrderedDictionary();

    public BaseSettingsContainer() => this.Setup();

    protected virtual void Setup()
    {
      this.RegisterSettings();
      this.Apply();
    }

    protected void RegisterSettings()
    {
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) this.GetType().GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (field => field.FieldType.IsSubclassOf(typeof (BaseSetting)))))
        this.Settings.Add((object) fieldInfo.Name, (object) (BaseSetting) fieldInfo.GetValue((object) this));
    }

    public override void SetDefault()
    {
      foreach (BaseSetting baseSetting in (IEnumerable) this.Settings.Values)
        baseSetting.SetDefault();
    }

    public virtual void Apply()
    {
    }

    public override JSONNode SerializeToJsonObject()
    {
      JSONObject jsonObject = new JSONObject();
      foreach (string key in (IEnumerable) this.Settings.Keys)
        jsonObject.Add(key, ((BaseSetting) this.Settings[(object) key]).SerializeToJsonObject());
      return (JSONNode) jsonObject;
    }

    public override void DeserializeFromJsonObject(JSONNode json)
    {
      JSONObject jsonObject = (JSONObject) json;
      foreach (string key in (IEnumerable) this.Settings.Keys)
      {
        if (jsonObject[key] != (object) null)
          ((BaseSetting) this.Settings[(object) key]).DeserializeFromJsonObject(jsonObject[key]);
      }
      if (this.Validate())
        return;
      this.SetDefault();
    }

    protected virtual bool Validate() => true;
  }
}
