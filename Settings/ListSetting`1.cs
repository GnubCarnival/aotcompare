// Decompiled with JetBrains decompiler
// Type: Settings.ListSetting`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using System.Collections.Generic;

namespace Settings
{
  internal class ListSetting<T> : TypedSetting<List<T>>, IListSetting where T : BaseSetting, new()
  {
    public ListSetting(List<T> defaultValue)
      : base(defaultValue)
    {
    }

    public ListSetting(T defaultValue)
    {
      this.DefaultValue = new List<T>() { defaultValue };
      this.SetDefault();
    }

    public ListSetting(T defaultValue, int count)
    {
      List<T> objList = new List<T>();
      JSONNode jsonObject = defaultValue.SerializeToJsonObject();
      for (int index = 0; index < count; ++index)
      {
        T to = new T();
        this.CopyLimits(defaultValue, to);
        to.DeserializeFromJsonObject(jsonObject);
        objList.Add(to);
      }
      this.DefaultValue = objList;
      this.SetDefault();
    }

    public ListSetting()
    {
      this.DefaultValue = new List<T>();
      this.SetDefault();
    }

    public override void SetDefault()
    {
      List<T> objList = new List<T>();
      foreach (T obj in this.DefaultValue)
      {
        T to = new T();
        this.CopyDefaultLimits(to);
        to.DeserializeFromJsonObject(obj.SerializeToJsonObject());
        objList.Add(to);
      }
      this.Value = objList;
    }

    public override void DeserializeFromJsonObject(JSONNode json)
    {
      List<T> objList = new List<T>();
      foreach (KeyValuePair<string, JSONNode> keyValuePair in (JSONNode) json.AsArray)
      {
        JSONNode json1 = (JSONNode) keyValuePair;
        T to = new T();
        this.CopyDefaultLimits(to);
        to.DeserializeFromJsonObject(json1);
        objList.Add(to);
      }
      this.Value = objList;
    }

    public override JSONNode SerializeToJsonObject()
    {
      JSONArray jsonObject = new JSONArray();
      foreach (T obj in this.Value)
      {
        BaseSetting baseSetting = (BaseSetting) obj;
        jsonObject.Add(baseSetting.SerializeToJsonObject());
      }
      return (JSONNode) jsonObject;
    }

    public int GetCount() => this.Value.Count;

    public BaseSetting GetItemAt(int index) => (BaseSetting) this.Value[index];

    public List<BaseSetting> GetItems()
    {
      List<BaseSetting> items = new List<BaseSetting>();
      foreach (T obj in this.Value)
      {
        BaseSetting baseSetting = (BaseSetting) obj;
        items.Add(baseSetting);
      }
      return items;
    }

    public void AddItem(BaseSetting item) => this.Value.Add((T) item);

    public void Clear() => this.Value.Clear();

    private void CopyLimits(T from, T to)
    {
      if ((object) from is IntSetting)
      {
        ((IntSetting) (object) to).MinValue = ((IntSetting) (object) from).MinValue;
        ((IntSetting) (object) to).MaxValue = ((IntSetting) (object) from).MaxValue;
      }
      else if ((object) from is ColorSetting)
        ((ColorSetting) (object) to).MinAlpha = ((ColorSetting) (object) from).MinAlpha;
      else if ((object) from is FloatSetting)
      {
        ((FloatSetting) (object) to).MinValue = ((FloatSetting) (object) from).MinValue;
        ((FloatSetting) (object) to).MaxValue = ((FloatSetting) (object) from).MaxValue;
      }
      else
      {
        if (!((object) from is StringSetting))
          return;
        ((StringSetting) (object) to).MaxLength = ((StringSetting) (object) from).MaxLength;
      }
    }

    private void CopyDefaultLimits(T to)
    {
      if (this.DefaultValue.Count <= 0)
        return;
      this.CopyLimits(this.DefaultValue[0], to);
    }
  }
}
