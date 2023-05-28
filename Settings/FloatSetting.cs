// Decompiled with JetBrains decompiler
// Type: Settings.FloatSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
  internal class FloatSetting : TypedSetting<float>
  {
    public float MinValue = float.MinValue;
    public float MaxValue = float.MaxValue;

    public FloatSetting()
      : base(0.0f)
    {
    }

    public FloatSetting(float defaultValue, float minValue = -3.40282347E+38f, float maxValue = 3.40282347E+38f)
    {
      this.MinValue = minValue;
      this.MaxValue = maxValue;
      this.DefaultValue = this.SanitizeValue(defaultValue);
      this.SetDefault();
    }

    public override void DeserializeFromJsonObject(JSONNode json) => this.Value = json.AsFloat;

    public override JSONNode SerializeToJsonObject() => (JSONNode) new JSONNumber((double) this.Value);

    protected override float SanitizeValue(float value) => Mathf.Clamp(value, this.MinValue, this.MaxValue);
  }
}
