// Decompiled with JetBrains decompiler
// Type: Settings.NameSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;

namespace Settings
{
  internal class NameSetting : StringSetting
  {
    public int MaxStrippedLength = int.MaxValue;

    public NameSetting()
      : base(string.Empty)
    {
    }

    public NameSetting(string defaultValue, int maxLength = 2147483647, int maxStrippedLength = 2147483647)
      : base(defaultValue)
    {
      this.MaxLength = maxLength;
      this.MaxStrippedLength = maxStrippedLength;
    }

    public override void DeserializeFromJsonObject(JSONNode json) => this.Value = json.Value;

    public override JSONNode SerializeToJsonObject() => (JSONNode) new JSONString(this.Value);

    protected override string SanitizeValue(string value)
    {
      if (value.Length > this.MaxLength)
        return value.Substring(0, this.MaxLength);
      string str = value.StripHex();
      return str.Length > this.MaxStrippedLength ? str.Substring(0, this.MaxStrippedLength) : value;
    }
  }
}
