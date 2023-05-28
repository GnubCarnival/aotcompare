// Decompiled with JetBrains decompiler
// Type: Settings.ColorSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
  internal class ColorSetting : TypedSetting<Color>
  {
    public float MinAlpha;

    public ColorSetting()
      : base(Color.white)
    {
    }

    public ColorSetting(Color defaultValue, float minAlpha = 0.0f)
    {
      this.MinAlpha = minAlpha;
      this.DefaultValue = this.SanitizeValue(defaultValue);
      this.Value = this.DefaultValue;
    }

    protected override Color SanitizeValue(Color value)
    {
      value.r = Mathf.Clamp(value.r, 0.0f, 1f);
      value.g = Mathf.Clamp(value.g, 0.0f, 1f);
      value.b = Mathf.Clamp(value.b, 0.0f, 1f);
      value.a = Mathf.Clamp(value.a, this.MinAlpha, 1f);
      return value;
    }

    public override JSONNode SerializeToJsonObject()
    {
      JSONArray jsonObject = new JSONArray();
      jsonObject.Add((JSONNode) new JSONNumber((double) this.Value.r));
      jsonObject.Add((JSONNode) new JSONNumber((double) this.Value.g));
      jsonObject.Add((JSONNode) new JSONNumber((double) this.Value.b));
      jsonObject.Add((JSONNode) new JSONNumber((double) this.Value.a));
      return (JSONNode) jsonObject;
    }

    public override void DeserializeFromJsonObject(JSONNode json)
    {
      JSONArray asArray = json.AsArray;
      this.Value = new Color(asArray[0].AsFloat, asArray[1].AsFloat, asArray[2].AsFloat, asArray[3].AsFloat);
    }
  }
}
