// Decompiled with JetBrains decompiler
// Type: Weather.WeatherSchedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weather
{
  internal class WeatherSchedule
  {
    private static Dictionary<string, WeatherAction> NameToWeatherAction = RCextensions.EnumToDict<WeatherAction>();
    private static Dictionary<string, WeatherEffect> NameToWeatherEffect = RCextensions.EnumToDict<WeatherEffect>();
    private static Dictionary<string, WeatherValueSelectType> NameToWeatherValueSelectType = RCextensions.EnumToDict<WeatherValueSelectType>();
    public List<WeatherEvent> Events = new List<WeatherEvent>();

    public WeatherSchedule()
    {
    }

    public WeatherSchedule(string csv) => this.DeserializeFromCSV(csv);

    public string SerializeToCSV()
    {
      List<string> stringList1 = new List<string>();
      foreach (WeatherEvent weatherEvent in this.Events)
      {
        List<string> stringList2 = new List<string>();
        stringList2.Add(weatherEvent.Action.ToString());
        if (weatherEvent.Effect != WeatherEffect.None)
          stringList2.Add(weatherEvent.Effect.ToString());
        if (weatherEvent.ValueSelectType != WeatherValueSelectType.None)
        {
          if (weatherEvent.Action != WeatherAction.Label)
            stringList2.Add(weatherEvent.ValueSelectType.ToString());
          if (weatherEvent.ValueSelectType == WeatherValueSelectType.RandomFromList)
          {
            for (int index = 0; index < weatherEvent.Values.Count; ++index)
              stringList2.Add(this.SerializeRandomListValue(weatherEvent.GetValueType(), weatherEvent.Values[index], weatherEvent.Weights[index]));
          }
          else
          {
            foreach (object obj in weatherEvent.Values)
              stringList2.Add(this.SerializeValue(weatherEvent.GetValueType(), obj));
          }
        }
        stringList1.Add(string.Join(",", stringList2.ToArray()));
      }
      return string.Join(";\n", stringList1.ToArray());
    }

    public string DeserializeFromCSV(string csv)
    {
      this.Events.Clear();
      string[] strArray = csv.Split(';');
      int num = 1;
      for (int index = 0; index < strArray.Length; ++index)
      {
        try
        {
          string line = strArray[index].Trim();
          num += strArray[index].Split('\n').Length - 1;
          if (line != string.Empty)
          {
            if (!line.StartsWith("//"))
              this.Events.Add(this.DeserializeLine(line));
          }
        }
        catch (Exception ex)
        {
          return string.Format("Import failed at line {0}", (object) num);
        }
      }
      return "";
    }

    private string SerializeValue(WeatherValueType type, object value)
    {
      string str = "";
      switch (type)
      {
        case WeatherValueType.Float:
          str = ((float) value).ToString();
          break;
        case WeatherValueType.Int:
          str = ((int) value).ToString();
          break;
        case WeatherValueType.String:
          str = (string) value;
          break;
        case WeatherValueType.Color:
          str = this.SerializeColor((Color) value);
          break;
        case WeatherValueType.Bool:
          str = Convert.ToInt32((bool) value).ToString();
          break;
      }
      return str.Replace(",", string.Empty).Replace(";", string.Empty);
    }

    private string SerializeRandomListValue(WeatherValueType type, object value, float weight) => this.SerializeValue(type, value) + "-" + weight.ToString();

    private string SerializeColor(Color color)
    {
      string[] strArray = new string[4]
      {
        color.r.ToString(),
        color.g.ToString(),
        color.b.ToString(),
        color.a.ToString()
      };
      return (double) color.a == 1.0 && (double) color.r == (double) color.g && (double) color.r == (double) color.b ? strArray[0] : string.Join("-", strArray);
    }

    private WeatherEvent DeserializeLine(string line)
    {
      WeatherEvent weatherEvent1 = new WeatherEvent();
      string[] strArray1 = line.Split(',');
      int num1 = 0;
      WeatherEvent weatherEvent2 = weatherEvent1;
      Dictionary<string, WeatherAction> nameToWeatherAction = WeatherSchedule.NameToWeatherAction;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      string key = strArray2[index1];
      int num3 = (int) nameToWeatherAction[key];
      weatherEvent2.Action = (WeatherAction) num3;
      if (weatherEvent1.SupportsWeatherEffects())
        weatherEvent1.Effect = WeatherSchedule.NameToWeatherEffect[strArray1[num2++]];
      if (weatherEvent1.Action == WeatherAction.Label)
        weatherEvent1.ValueSelectType = WeatherValueSelectType.Constant;
      else if (weatherEvent1.SupportsWeatherValueSelectTypes())
        weatherEvent1.ValueSelectType = WeatherSchedule.NameToWeatherValueSelectType[strArray1[num2++]];
      if (weatherEvent1.ValueSelectType == WeatherValueSelectType.RandomFromList)
      {
        for (int index2 = num2; index2 < strArray1.Length; ++index2)
        {
          string[] strArray3 = strArray1[index2].Split('-');
          weatherEvent1.Values.Add(this.DeserializeValue(weatherEvent1.GetValueType(), strArray3[0]));
          if (strArray3.Length > 1)
            weatherEvent1.Weights.Add(float.Parse(strArray3[1]));
          else
            weatherEvent1.Weights.Add(1f);
        }
      }
      else
      {
        for (int index3 = num2; index3 < strArray1.Length; ++index3)
          weatherEvent1.Values.Add(this.DeserializeValue(weatherEvent1.GetValueType(), strArray1[index3]));
      }
      return weatherEvent1;
    }

    private object DeserializeValue(WeatherValueType type, string item)
    {
      switch (type)
      {
        case WeatherValueType.Float:
          return (object) float.Parse(item);
        case WeatherValueType.Int:
          return (object) int.Parse(item);
        case WeatherValueType.String:
          return (object) item;
        case WeatherValueType.Color:
          return (object) this.DeserializeColor(item);
        case WeatherValueType.Bool:
          return (object) Convert.ToBoolean(int.Parse(item));
        default:
          return (object) null;
      }
    }

    private Color DeserializeColor(string item)
    {
      string[] strArray = item.Split('-');
      if (strArray.Length != 1)
        return new Color(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]), float.Parse(strArray[3]));
      float num = float.Parse(strArray[0]);
      return new Color(num, num, num, 1f);
    }
  }
}
