// Decompiled with JetBrains decompiler
// Type: Settings.StringSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;

namespace Settings
{
  internal class StringSetting : TypedSetting<string>
  {
    public int MaxLength = int.MaxValue;

    public StringSetting()
      : base(string.Empty)
    {
    }

    public StringSetting(string defaultValue, int maxLength = 2147483647)
      : base(defaultValue)
    {
      this.MaxLength = maxLength;
    }

    public override void DeserializeFromJsonObject(JSONNode json) => this.Value = json.Value;

    public override JSONNode SerializeToJsonObject() => (JSONNode) new JSONString(this.Value);

    protected override string SanitizeValue(string value) => value.Length > this.MaxLength ? value.Substring(0, this.MaxLength) : value;
  }
}
