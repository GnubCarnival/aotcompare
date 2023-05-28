// Decompiled with JetBrains decompiler
// Type: Settings.KeybindSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
  internal class KeybindSetting : BaseSetting
  {
    public List<InputKey> InputKeys = new List<InputKey>();
    protected string[] _defaultKeyStrings;

    public KeybindSetting(string[] defaultKeyStrings)
    {
      this._defaultKeyStrings = defaultKeyStrings;
      this.SetDefault();
    }

    public override void SetDefault() => this.LoadFromStringArray(this._defaultKeyStrings);

    protected void LoadFromStringArray(string[] keyStrings)
    {
      this.InputKeys.Clear();
      foreach (string keyString in keyStrings)
        this.InputKeys.Add(new InputKey(keyString));
    }

    public override string ToString()
    {
      List<string> stringList = new List<string>();
      foreach (InputKey inputKey in this.InputKeys)
      {
        if (!inputKey.IsNone())
          stringList.Add(inputKey.ToString());
      }
      return stringList.Count == 0 ? "None" : string.Join(" / ", stringList.ToArray());
    }

    public bool Contains(InputKey key)
    {
      foreach (object inputKey in this.InputKeys)
      {
        if (inputKey.Equals((object) key))
          return true;
      }
      return false;
    }

    public bool Contains(KeyCode key)
    {
      foreach (InputKey inputKey in this.InputKeys)
      {
        if (inputKey.MatchesKeyCode(key))
          return true;
      }
      return false;
    }

    public bool GetKeyDown()
    {
      foreach (InputKey inputKey in this.InputKeys)
      {
        if (inputKey.GetKeyDown())
          return true;
      }
      return false;
    }

    public bool GetKey()
    {
      foreach (InputKey inputKey in this.InputKeys)
      {
        if (inputKey.GetKey())
          return true;
      }
      return false;
    }

    public bool GetKeyUp()
    {
      foreach (InputKey inputKey in this.InputKeys)
      {
        if (inputKey.GetKeyUp())
          return true;
      }
      return false;
    }

    public override JSONNode SerializeToJsonObject()
    {
      JSONArray jsonObject = new JSONArray();
      foreach (InputKey inputKey in this.InputKeys)
        jsonObject.Add((JSONNode) new JSONString(inputKey.ToString()));
      return (JSONNode) jsonObject;
    }

    public override void DeserializeFromJsonObject(JSONNode json)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, JSONNode> keyValuePair in (JSONNode) json.AsArray)
      {
        JSONString jsonString = (JSONString) (JSONNode) keyValuePair;
        stringList.Add(jsonString.Value);
      }
      this.LoadFromStringArray(stringList.ToArray());
    }
  }
}
