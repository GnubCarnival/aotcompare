// Decompiled with JetBrains decompiler
// Type: Utility.SettingsUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

namespace Utility
{
  internal class SettingsUtil
  {
    public static void SetSettingValue(BaseSetting setting, SettingType type, object value)
    {
      switch (type)
      {
        case SettingType.Bool:
          ((TypedSetting<bool>) setting).Value = (bool) value;
          break;
        case SettingType.Int:
          ((TypedSetting<int>) setting).Value = (int) value;
          break;
        case SettingType.Float:
          ((TypedSetting<float>) setting).Value = (float) value;
          break;
        case SettingType.String:
          ((TypedSetting<string>) setting).Value = (string) value;
          break;
        case SettingType.Color:
          ((TypedSetting<Color>) setting).Value = (Color) value;
          break;
        default:
          Debug.Log((object) "Attempting to set invalid setting value.");
          break;
      }
    }

    public static object DeserializeValueFromJson(SettingType type, string json)
    {
      BaseSetting baseSetting = SettingsUtil.CreateBaseSetting(type);
      if (baseSetting == null)
        return (object) baseSetting;
      baseSetting.DeserializeFromJsonString(json);
      return (object) baseSetting;
    }

    public static BaseSetting CreateBaseSetting(SettingType type)
    {
      switch (type)
      {
        case SettingType.Bool:
          return (BaseSetting) new BoolSetting();
        case SettingType.Int:
          return (BaseSetting) new IntSetting();
        case SettingType.Float:
          return (BaseSetting) new FloatSetting();
        case SettingType.String:
          return (BaseSetting) new StringSetting();
        case SettingType.Color:
          return (BaseSetting) new ColorSetting();
        default:
          return (BaseSetting) null;
      }
    }
  }
}
