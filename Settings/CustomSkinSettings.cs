// Decompiled with JetBrains decompiler
// Type: Settings.CustomSkinSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.IO;
using UnityEngine;

namespace Settings
{
  internal class CustomSkinSettings : SaveableSettingsContainer
  {
    public HumanCustomSkinSettings Human = new HumanCustomSkinSettings();
    public BaseCustomSkinSettings<TitanCustomSkinSet> Titan = new BaseCustomSkinSettings<TitanCustomSkinSet>();
    public BaseCustomSkinSettings<ShifterCustomSkinSet> Shifter = new BaseCustomSkinSettings<ShifterCustomSkinSet>();
    public BaseCustomSkinSettings<SkyboxCustomSkinSet> Skybox = new BaseCustomSkinSettings<SkyboxCustomSkinSet>();
    public BaseCustomSkinSettings<ForestCustomSkinSet> Forest = new BaseCustomSkinSettings<ForestCustomSkinSet>();
    public BaseCustomSkinSettings<CityCustomSkinSet> City = new BaseCustomSkinSettings<CityCustomSkinSet>();
    public BaseCustomSkinSettings<CustomLevelCustomSkinSet> CustomLevel = new BaseCustomSkinSettings<CustomLevelCustomSkinSet>();

    protected override string FileName => "CustomSkins.json";

    public override void Load()
    {
      string filePath = this.GetFilePath();
      if (File.Exists(filePath))
      {
        string str = File.ReadAllText(filePath);
        if (this.Encrypted)
          str = new SimpleAES().Decrypt(str);
        this.DeserializeFromJsonString(str);
        ICustomSkinSettings[] customSkinSettingsArray = new ICustomSkinSettings[7]
        {
          (ICustomSkinSettings) this.Human,
          (ICustomSkinSettings) this.Titan,
          (ICustomSkinSettings) this.Shifter,
          (ICustomSkinSettings) this.Skybox,
          (ICustomSkinSettings) this.Forest,
          (ICustomSkinSettings) this.City,
          (ICustomSkinSettings) this.CustomLevel
        };
        foreach (ICustomSkinSettings customSkinSettings in customSkinSettingsArray)
        {
          if (customSkinSettings.GetSkinSets().GetItems().Count > 0)
          {
            customSkinSettings.GetSets().Clear();
            foreach (BaseSetSetting baseSetSetting in customSkinSettings.GetSkinSets().GetItems())
              customSkinSettings.GetSets().AddItem((BaseSetting) baseSetSetting);
          }
          customSkinSettings.GetSkinSets().Clear();
        }
      }
      else
      {
        try
        {
          this.LoadLegacy();
        }
        catch
        {
          Debug.Log((object) "Exception occurred while loading legacy settings.");
        }
      }
    }
  }
}
