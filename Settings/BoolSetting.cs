// Decompiled with JetBrains decompiler
// Type: Settings.BoolSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;

namespace Settings
{
  internal class BoolSetting : TypedSetting<bool>
  {
    public BoolSetting()
      : base(false)
    {
    }

    public BoolSetting(bool defaultValue)
      : base(defaultValue)
    {
    }

    public override void DeserializeFromJsonObject(JSONNode json) => this.Value = json.AsBool;

    public override JSONNode SerializeToJsonObject() => (JSONNode) new JSONBool(this.Value);
  }
}
