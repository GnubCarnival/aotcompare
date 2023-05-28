// Decompiled with JetBrains decompiler
// Type: Weather.WeatherScheduleRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Weather
{
  internal class WeatherScheduleRunner
  {
    private const int ScheduleMaxRecursion = 200;
    private int _currentScheduleLine;
    private LinkedList<int> _callerStack = new LinkedList<int>();
    private Dictionary<string, int> _scheduleLabels = new Dictionary<string, int>();
    private Dictionary<int, int> _repeatStartLines = new Dictionary<int, int>();
    private Dictionary<int, int> _repeatCurrentCounts = new Dictionary<int, int>();
    private WeatherManager _manager;
    public WeatherSchedule Schedule = new WeatherSchedule();

    public WeatherScheduleRunner(WeatherManager manager) => this._manager = manager;

    public void ProcessSchedule()
    {
      for (int index = 0; index < this.Schedule.Events.Count - 1; ++index)
      {
        if (this.Schedule.Events[index].Action == WeatherAction.RepeatNext)
        {
          this.Schedule.Events[index].Action = WeatherAction.BeginRepeat;
          WeatherEvent weatherEvent = new WeatherEvent()
          {
            Action = WeatherAction.EndRepeat
          };
          this.Schedule.Events.Insert(index + 2, weatherEvent);
        }
      }
      int num = -1;
      for (int index = 0; index < this.Schedule.Events.Count; ++index)
      {
        WeatherEvent weatherEvent = this.Schedule.Events[index];
        if (weatherEvent.Action == WeatherAction.Label)
        {
          string key = (string) weatherEvent.GetValue();
          if (!this._scheduleLabels.ContainsKey(key))
            this._scheduleLabels.Add(key, index);
        }
        else if (weatherEvent.Action == WeatherAction.BeginRepeat)
        {
          num = index;
          this._repeatCurrentCounts.Add(index, 0);
        }
        else if (weatherEvent.Action == WeatherAction.EndRepeat && num >= 0)
        {
          this._repeatStartLines.Add(index, num);
          num = -1;
        }
      }
    }

    public void ConsumeSchedule()
    {
      int num1 = 0;
      bool flag1 = false;
      while (!flag1)
      {
        ++num1;
        if (num1 > 200)
        {
          Debug.Log((object) "Weather schedule reached max usage (did you forget to use waits?)");
          this.Schedule.Events.Clear();
          break;
        }
        if (this.Schedule.Events.Count == 0 || this._currentScheduleLine < 0 || this._currentScheduleLine >= this.Schedule.Events.Count)
          break;
        WeatherEvent weatherEvent = this.Schedule.Events[this._currentScheduleLine];
        switch (weatherEvent.Action)
        {
          case WeatherAction.BeginRepeat:
            this._repeatCurrentCounts[this._currentScheduleLine] = (int) weatherEvent.GetValue();
            break;
          case WeatherAction.EndRepeat:
            int repeatStartLine = this._repeatStartLines[this._currentScheduleLine];
            if (this._repeatCurrentCounts.ContainsKey(repeatStartLine) && this._repeatCurrentCounts[repeatStartLine] > 0)
            {
              this._currentScheduleLine = repeatStartLine + 1;
              --this._repeatCurrentCounts[repeatStartLine];
              break;
            }
            break;
          case WeatherAction.SetDefaultAll:
            bool flag2 = this._manager._currentWeather.ScheduleLoop.Value;
            this._manager._currentWeather.SetDefault();
            this._manager._currentWeather.UseSchedule.Value = true;
            this._manager._currentWeather.ScheduleLoop.Value = flag2;
            this._manager._needSync = true;
            break;
          case WeatherAction.SetDefault:
            ((BaseSetting) this._manager._currentWeather.Settings[(object) weatherEvent.Effect.ToString()]).SetDefault();
            this._manager._needSync = true;
            break;
          case WeatherAction.SetValue:
            SettingsUtil.SetSettingValue((BaseSetting) this._manager._currentWeather.Settings[(object) weatherEvent.Effect.ToString()], weatherEvent.GetSettingType(), weatherEvent.GetValue());
            this._manager._needSync = true;
            break;
          case WeatherAction.SetTargetDefaultAll:
            this._manager._targetWeather.SetDefault();
            this._manager._needSync = true;
            break;
          case WeatherAction.SetTargetDefault:
            ((BaseSetting) this._manager._targetWeather.Settings[(object) weatherEvent.Effect.ToString()]).SetDefault();
            this._manager._needSync = true;
            break;
          case WeatherAction.SetTargetValue:
            SettingsUtil.SetSettingValue((BaseSetting) this._manager._targetWeather.Settings[(object) weatherEvent.Effect.ToString()], weatherEvent.GetSettingType(), weatherEvent.GetValue());
            this._manager._needSync = true;
            break;
          case WeatherAction.SetTargetTimeAll:
            this._manager._targetWeatherStartTimes.Clear();
            this._manager._targetWeatherEndTimes.Clear();
            float num2 = this._manager._currentTime + (float) weatherEvent.GetValue();
            foreach (WeatherEffect key in RCextensions.EnumToList<WeatherEffect>())
            {
              this._manager._targetWeatherStartTimes.Add((int) key, this._manager._currentTime);
              this._manager._targetWeatherEndTimes.Add((int) key, num2);
            }
            this._manager._needSync = true;
            break;
          case WeatherAction.SetTargetTime:
            if (!this._manager._targetWeatherStartTimes.ContainsKey((int) weatherEvent.Effect))
            {
              this._manager._targetWeatherStartTimes.Add((int) weatherEvent.Effect, 0.0f);
              this._manager._targetWeatherEndTimes.Add((int) weatherEvent.Effect, 0.0f);
            }
            BaseSetting setting1 = (BaseSetting) this._manager._startWeather.Settings[(object) weatherEvent.Effect.ToString()];
            BaseSetting setting2 = (BaseSetting) this._manager._currentWeather.Settings[(object) weatherEvent.Effect.ToString()];
            this._manager._targetWeatherStartTimes[(int) weatherEvent.Effect] = this._manager._currentTime;
            this._manager._targetWeatherEndTimes[(int) weatherEvent.Effect] = this._manager._currentTime + (float) weatherEvent.GetValue();
            BaseSetting other = setting2;
            setting1.Copy(other);
            this._manager._needSync = true;
            break;
          case WeatherAction.Wait:
            this._manager._currentScheduleWait[this] = (float) weatherEvent.GetValue();
            flag1 = true;
            break;
          case WeatherAction.Goto:
            string key1 = (string) weatherEvent.GetValue();
            if (key1 != "NextLine" && this._scheduleLabels.ContainsKey(key1))
            {
              this._callerStack.AddLast(this._currentScheduleLine);
              if (this._callerStack.Count > 200)
                this._callerStack.RemoveFirst();
              this._currentScheduleLine = this._scheduleLabels[key1];
              break;
            }
            break;
          case WeatherAction.Return:
            if (this._callerStack.Count > 0)
            {
              this._currentScheduleLine = this._callerStack.Last.Value;
              this._callerStack.RemoveLast();
              break;
            }
            break;
        }
        ++this._currentScheduleLine;
        if (this._currentScheduleLine >= this.Schedule.Events.Count && this._manager._currentWeather.ScheduleLoop.Value)
          this._currentScheduleLine = 0;
      }
    }
  }
}
