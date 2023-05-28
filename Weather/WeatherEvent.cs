// Decompiled with JetBrains decompiler
// Type: Weather.WeatherEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Weather
{
  internal class WeatherEvent
  {
    private static string[] AllWeatherEffects = RCextensions.EnumToStringArrayExceptNone<WeatherEffect>();
    private static string[] AllWeatherValueSelectTypes = RCextensions.EnumToStringArrayExceptNone<WeatherValueSelectType>();
    public WeatherAction Action;
    public WeatherEffect Effect;
    public WeatherValueSelectType ValueSelectType;
    public List<object> Values = new List<object>();
    public List<float> Weights = new List<float>();

    public object GetValue()
    {
      WeatherValueType valueType = this.GetValueType();
      switch (this.ValueSelectType)
      {
        case WeatherValueSelectType.Constant:
          return this.Values[0];
        case WeatherValueSelectType.RandomBetween:
          switch (valueType)
          {
            case WeatherValueType.Float:
              return (object) Random.Range((float) this.Values[0], (float) this.Values[1]);
            case WeatherValueType.Int:
              return (object) Random.Range((int) this.Values[0], (int) this.Values[1] + 1);
            case WeatherValueType.Color:
              Color color1 = (Color) this.Values[0];
              Color color2 = (Color) this.Values[1];
              if (!color1.IsGray() || !color2.IsGray())
                return (object) new Color(Random.Range(Mathf.Min(color1.r, color2.r), Mathf.Max(color1.r, color2.r)), Random.Range(Mathf.Min(color1.g, color2.g), Mathf.Max(color1.g, color2.g)), Random.Range(Mathf.Min(color1.b, color2.b), Mathf.Max(color1.b, color2.b)), Random.Range(Mathf.Min(color1.a, color2.a), Mathf.Max(color1.a, color2.a)));
              float num = Random.Range(color1.r, color2.r);
              return (object) new Color(num, num, num);
          }
          break;
        case WeatherValueSelectType.RandomFromList:
          return this.GetRandomFromList();
      }
      return (object) null;
    }

    private object GetRandomFromList()
    {
      float num1 = 0.0f;
      foreach (float weight in this.Weights)
        num1 += weight;
      float num2 = Random.Range(0.0f, num1);
      float num3 = 0.0f;
      for (int index = 0; index < this.Values.Count; ++index)
      {
        if ((double) num2 >= (double) num3 && (double) num2 < (double) num3 + (double) this.Weights[index])
          return this.Values[index];
        num3 += this.Weights[index];
      }
      return this.Values[0];
    }

    public WeatherValueType GetValueType()
    {
      switch (this.Action)
      {
        case WeatherAction.BeginSchedule:
        case WeatherAction.EndSchedule:
        case WeatherAction.EndRepeat:
        case WeatherAction.SetDefaultAll:
        case WeatherAction.SetDefault:
        case WeatherAction.SetTargetDefaultAll:
        case WeatherAction.SetTargetDefault:
        case WeatherAction.Return:
          return WeatherValueType.None;
        case WeatherAction.RepeatNext:
        case WeatherAction.BeginRepeat:
          return WeatherValueType.Int;
        case WeatherAction.SetTargetTimeAll:
        case WeatherAction.SetTargetTime:
        case WeatherAction.Wait:
          return WeatherValueType.Float;
        case WeatherAction.Goto:
        case WeatherAction.Label:
        case WeatherAction.LoadSkybox:
          return WeatherValueType.String;
        default:
          switch (this.Effect)
          {
            case WeatherEffect.Daylight:
            case WeatherEffect.AmbientLight:
            case WeatherEffect.SkyboxColor:
            case WeatherEffect.Flashlight:
            case WeatherEffect.FogColor:
              return WeatherValueType.Color;
            case WeatherEffect.Skybox:
              return WeatherValueType.String;
            case WeatherEffect.FogDensity:
            case WeatherEffect.Rain:
            case WeatherEffect.Thunder:
            case WeatherEffect.Snow:
            case WeatherEffect.Wind:
              return WeatherValueType.Float;
            default:
              return WeatherValueType.None;
          }
      }
    }

    public SettingType GetSettingType()
    {
      switch (this.GetValueType())
      {
        case WeatherValueType.Float:
          return SettingType.Float;
        case WeatherValueType.Int:
          return SettingType.Int;
        case WeatherValueType.String:
          return SettingType.String;
        case WeatherValueType.Color:
          return SettingType.Color;
        case WeatherValueType.Bool:
          return SettingType.Bool;
        default:
          return SettingType.None;
      }
    }

    public string[] SupportedWeatherEffects()
    {
      switch (this.Action)
      {
        case WeatherAction.SetDefault:
        case WeatherAction.SetValue:
        case WeatherAction.SetTargetDefault:
        case WeatherAction.SetTargetValue:
        case WeatherAction.SetTargetTime:
          return WeatherEvent.AllWeatherEffects;
        default:
          return new string[0];
      }
    }

    public bool SupportsWeatherEffects() => this.SupportedWeatherEffects().Length != 0;

    public string[] SupportedWeatherValueSelectTypes()
    {
      switch (this.GetValueType())
      {
        case WeatherValueType.Float:
        case WeatherValueType.Int:
        case WeatherValueType.Color:
          return WeatherEvent.AllWeatherValueSelectTypes;
        case WeatherValueType.String:
        case WeatherValueType.Bool:
          if (this.Action == WeatherAction.LoadSkybox || this.Action == WeatherAction.Label)
            return new string[1]
            {
              WeatherValueSelectType.Constant.ToString()
            };
          string[] strArray = new string[2];
          WeatherValueSelectType weatherValueSelectType = WeatherValueSelectType.Constant;
          strArray[0] = weatherValueSelectType.ToString();
          weatherValueSelectType = WeatherValueSelectType.RandomFromList;
          strArray[1] = weatherValueSelectType.ToString();
          return strArray;
        default:
          return new string[0];
      }
    }

    public bool SupportsWeatherValueSelectTypes() => this.SupportedWeatherValueSelectTypes().Length != 0;
  }
}
