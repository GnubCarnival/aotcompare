// Decompiled with JetBrains decompiler
// Type: Settings.IntSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using System;

namespace Settings
{
  internal class IntSetting : TypedSetting<int>
  {
    public int MinValue = int.MinValue;
    public int MaxValue = int.MaxValue;

    public IntSetting()
      : base(0)
    {
    }

    public IntSetting(int defaultValue, int minValue = -2147483648, int maxValue = 2147483647)
    {
      this.MinValue = minValue;
      this.MaxValue = maxValue;
      this.DefaultValue = this.SanitizeValue(defaultValue);
      this.SetDefault();
    }

    public override void DeserializeFromJsonObject(JSONNode json) => this.Value = json.AsInt;

    public override JSONNode SerializeToJsonObject() => (JSONNode) new JSONNumber((double) this.Value);

    protected override int SanitizeValue(int value)
    {
      value = Math.Min(value, this.MaxValue);
      value = Math.Max(value, this.MinValue);
      return value;
    }
  }
}
