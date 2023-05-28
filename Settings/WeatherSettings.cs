// Decompiled with JetBrains decompiler
// Type: Settings.WeatherSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Settings
{
  internal class WeatherSettings : PresetSettingsContainer
  {
    public SetSettingsContainer<WeatherSet> WeatherSets = new SetSettingsContainer<WeatherSet>();

    protected override string FileName => "Weather.json";
  }
}
