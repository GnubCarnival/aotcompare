// Decompiled with JetBrains decompiler
// Type: Weather.WeatherManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using CustomSkins;
using GameManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Weather
{
  internal class WeatherManager : MonoBehaviour
  {
    private static WeatherManager _instance;
    private const float LerpDelay = 0.05f;
    private const float SyncDelay = 5f;
    private HashSet<WeatherEffect> LowEffects = new HashSet<WeatherEffect>()
    {
      WeatherEffect.Daylight,
      WeatherEffect.AmbientLight,
      WeatherEffect.Flashlight,
      WeatherEffect.Skybox
    };
    private static Dictionary<string, Material> SkyboxMaterials = new Dictionary<string, Material>();
    private static Dictionary<string, Dictionary<string, Material>> SkyboxBlendedMaterials = new Dictionary<string, Dictionary<string, Material>>();
    private static Shader _blendedShader;
    private List<WeatherScheduleRunner> _scheduleRunners = new List<WeatherScheduleRunner>();
    private Dictionary<WeatherEffect, BaseWeatherEffect> _effects = new Dictionary<WeatherEffect, BaseWeatherEffect>();
    public WeatherSet _currentWeather = new WeatherSet();
    public WeatherSet _targetWeather = new WeatherSet();
    public WeatherSet _startWeather = new WeatherSet();
    public Dictionary<int, float> _targetWeatherStartTimes = new Dictionary<int, float>();
    public Dictionary<int, float> _targetWeatherEndTimes = new Dictionary<int, float>();
    private List<WeatherEffect> _needApply = new List<WeatherEffect>();
    public float _currentTime;
    public bool _needSync;
    public Dictionary<WeatherScheduleRunner, float> _currentScheduleWait = new Dictionary<WeatherScheduleRunner, float>();
    private float _currentLerpWait;
    private float _currentSyncWait;
    private bool _finishedLoading;
    private Light _mainLight;
    private Skybox _skybox;

    public static void Init() => WeatherManager._instance = SingletonFactory.CreateSingleton<WeatherManager>(WeatherManager._instance);

    public static void FinishLoadAssets()
    {
      WeatherManager.LoadSkyboxes();
      ThunderWeatherEffect.FinishLoadAssets();
      WeatherManager._instance.StartCoroutine(WeatherManager._instance.RestartWeather());
    }

    private static void LoadSkyboxes()
    {
      WeatherManager._blendedShader = AssetBundleManager.InstantiateAsset<Shader>("SkyboxBlendShader");
      string[] stringArray1 = RCextensions.EnumToStringArray<WeatherSkybox>();
      string[] stringArray2 = RCextensions.EnumToStringArray<SkyboxCustomSkinPartId>();
      foreach (string key in stringArray1)
        WeatherManager.SkyboxMaterials.Add(key, AssetBundleManager.InstantiateAsset<Material>(key.ToString() + "Skybox"));
      foreach (string str1 in stringArray1)
      {
        WeatherManager.SkyboxBlendedMaterials.Add(str1, new Dictionary<string, Material>());
        foreach (string str2 in stringArray1)
        {
          Material blendedSkybox = WeatherManager.CreateBlendedSkybox(WeatherManager._blendedShader, stringArray2, str1, str2);
          WeatherManager.SkyboxBlendedMaterials[str1].Add(str2, blendedSkybox);
        }
      }
    }

    public static void TakeFlashlight(Transform parent)
    {
      if (!WeatherManager._instance._effects.ContainsKey(WeatherEffect.Flashlight) || !Object.op_Inequality((Object) WeatherManager._instance._effects[WeatherEffect.Flashlight], (Object) null))
        return;
      WeatherManager._instance._effects[WeatherEffect.Flashlight].SetParent(parent);
    }

    private static Material CreateBlendedSkybox(
      Shader shader,
      string[] parts,
      string skybox1,
      string skybox2)
    {
      Material skybox = new Material(shader);
      foreach (string part in parts)
      {
        string str = "_" + part + "Tex";
        skybox.SetTexture(str, WeatherManager.SkyboxMaterials[skybox1].GetTexture(str));
        skybox.SetTexture(str + "2", WeatherManager.SkyboxMaterials[skybox2].GetTexture(str));
      }
      WeatherManager.SetSkyboxBlend(skybox, 0.0f);
      return skybox;
    }

    private static void SetSkyboxBlend(Material skybox, float blend) => skybox.SetFloat("_Blend", blend);

    private void Cache()
    {
      this._mainLight = GameObject.Find("mainLight").GetComponent<Light>();
      this._skybox = ((Component) Camera.main).GetComponent<Skybox>();
    }

    private void ResetSkyboxColors()
    {
      foreach (string key1 in WeatherManager.SkyboxBlendedMaterials.Keys)
      {
        foreach (string key2 in WeatherManager.SkyboxBlendedMaterials[key1].Keys)
          WeatherManager.SkyboxBlendedMaterials[key1][key2].SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
      }
    }

    private IEnumerator RestartWeather()
    {
      while (Object.op_Equality((Object) Camera.main, (Object) null))
        yield return (object) null;
      this.Cache();
      this.ResetSkyboxColors();
      this._scheduleRunners.Clear();
      this._effects.Clear();
      this._currentWeather.SetDefault();
      this._startWeather.SetDefault();
      this._targetWeather.SetDefault();
      this._targetWeatherStartTimes.Clear();
      this._targetWeatherEndTimes.Clear();
      this._needApply.Clear();
      this._currentTime = 0.0f;
      this._currentScheduleWait.Clear();
      this.CreateEffects();
      if (Application.loadedLevel == 0 && SettingsManager.GraphicsSettings.AnimatedIntro.Value)
        this.SetMainMenuWeather();
      this.ApplyCurrentWeather(true, true);
      bool flag = SettingsManager.GraphicsSettings.WeatherEffects.Value == 0;
      if (Application.loadedLevel != 0 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
      {
        if (!flag)
        {
          this._currentWeather.Copy((BaseSetting) SettingsManager.WeatherSettings.WeatherSets.GetSelectedSet());
          this.CreateScheduleRunners(this._currentWeather.Schedule.Value);
          this._currentWeather.Schedule.SetDefault();
        }
        if (this._currentWeather.UseSchedule.Value)
        {
          foreach (WeatherScheduleRunner scheduleRunner in this._scheduleRunners)
          {
            scheduleRunner.ProcessSchedule();
            scheduleRunner.ConsumeSchedule();
          }
        }
        this.SyncWeather();
        this._currentSyncWait = 5f;
        this._needSync = false;
      }
      this._currentLerpWait = 0.05f;
      this._finishedLoading = true;
    }

    private void SetMainMenuWeather()
    {
      this._currentWeather.Rain.Value = 0.45f;
      this._currentWeather.Thunder.Value = 0.1f;
      this._currentWeather.Skybox.Value = "Storm";
      this._currentWeather.FogDensity.Value = 0.01f;
      this._currentWeather.Daylight.Value = new Color(0.1f, 0.1f, 0.1f);
      this._currentWeather.AmbientLight.Value = new Color(0.1f, 0.1f, 0.1f);
    }

    private void CreateScheduleRunners(string schedule)
    {
      WeatherScheduleRunner key = new WeatherScheduleRunner(this);
      foreach (WeatherEvent weatherEvent in new WeatherSchedule(schedule).Events)
      {
        if (weatherEvent.Action == WeatherAction.BeginSchedule)
        {
          key = new WeatherScheduleRunner(this);
          this._scheduleRunners.Add(key);
          this._currentScheduleWait.Add(key, 0.0f);
        }
        key.Schedule.Events.Add(weatherEvent);
      }
    }

    private void CreateEffects()
    {
      this._effects.Add(WeatherEffect.Rain, (BaseWeatherEffect) AssetBundleManager.InstantiateAsset<GameObject>("RainEffect").AddComponent<RainWeatherEffect>());
      this._effects.Add(WeatherEffect.Snow, (BaseWeatherEffect) AssetBundleManager.InstantiateAsset<GameObject>("SnowEffect").AddComponent<SnowWeatherEffect>());
      this._effects.Add(WeatherEffect.Wind, (BaseWeatherEffect) AssetBundleManager.InstantiateAsset<GameObject>("WindEffect").AddComponent<WindWeatherEffect>());
      this._effects.Add(WeatherEffect.Thunder, (BaseWeatherEffect) AssetBundleManager.InstantiateAsset<GameObject>("ThunderEffect").AddComponent<ThunderWeatherEffect>());
      Transform transform = ((Component) Camera.main).transform;
      foreach (BaseWeatherEffect baseWeatherEffect in this._effects.Values)
      {
        baseWeatherEffect.Setup(transform);
        baseWeatherEffect.Randomize();
        baseWeatherEffect.Disable();
      }
      this.CreateFlashlight();
    }

    private void CreateFlashlight()
    {
      this._effects.Add(WeatherEffect.Flashlight, (BaseWeatherEffect) AssetBundleManager.InstantiateAsset<GameObject>("FlashlightEffect").AddComponent<FlashlightWeatherEffect>());
      this._effects[WeatherEffect.Flashlight].Setup((Transform) null);
      this._effects[WeatherEffect.Flashlight].Disable();
      if (!Object.op_Inequality((Object) IN_GAME_MAIN_CAMERA.Instance, (Object) null))
        return;
      WeatherManager.TakeFlashlight(((Component) IN_GAME_MAIN_CAMERA.Instance).transform);
    }

    private void FixedUpdate()
    {
      if (!this._finishedLoading)
        return;
      this._currentTime += Time.fixedDeltaTime;
      if (this._targetWeatherStartTimes.Count > 0)
      {
        this._currentLerpWait -= Time.fixedDeltaTime;
        if ((double) this._currentLerpWait <= 0.0)
        {
          this.LerpCurrentWeatherToTarget();
          this.ApplyCurrentWeather(false, false);
          this._currentLerpWait = 0.05f;
        }
      }
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !PhotonNetwork.isMasterClient || !this._currentWeather.UseSchedule.Value)
        return;
      foreach (WeatherScheduleRunner key in new List<WeatherScheduleRunner>((IEnumerable<WeatherScheduleRunner>) this._currentScheduleWait.Keys))
      {
        this._currentScheduleWait[key] -= Time.fixedDeltaTime;
        if ((double) this._currentScheduleWait[key] <= 0.0)
          key.ConsumeSchedule();
      }
      this._currentSyncWait -= Time.fixedDeltaTime;
      if ((double) this._currentSyncWait > 0.0 || !this._needSync)
        return;
      this.LerpCurrentWeatherToTarget();
      this.SyncWeather();
      this._needSync = false;
      this._currentSyncWait = 5f;
    }

    private void SyncWeather()
    {
      this.ApplyCurrentWeather(false, true);
      if (!PhotonNetwork.isMasterClient || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        return;
      CustomRPCManager.PhotonView.RPC("SetWeatherRPC", PhotonTargets.Others, (object) this._currentWeather.SerializeToJsonString(), (object) this._startWeather.SerializeToJsonString(), (object) this._targetWeather.SerializeToJsonString(), (object) this._targetWeatherStartTimes, (object) this._targetWeatherEndTimes, (object) this._currentTime);
    }

    private void OnLevelWasLoaded(int level)
    {
      WindWeatherEffect.WindEnabled = false;
      foreach (List<LightningParticle> lightningParticleList in ThunderWeatherEffect.LightningPool)
      {
        foreach (LightningParticle lightningParticle in lightningParticleList)
          lightningParticle.Disable();
      }
      if (!(Application.loadedLevelName != "characterCreation") || !(Application.loadedLevelName != "SnapShot"))
        return;
      this._finishedLoading = false;
      this.StartCoroutine(this.RestartWeather());
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
      if (!PhotonNetwork.isMasterClient)
        return;
      CustomRPCManager.PhotonView.RPC("SetWeatherRPC", player, (object) this._currentWeather.SerializeToJsonString(), (object) this._startWeather.SerializeToJsonString(), (object) this._targetWeather.SerializeToJsonString(), (object) this._targetWeatherStartTimes, (object) this._targetWeatherEndTimes, (object) this._currentTime);
    }

    private void LerpCurrentWeatherToTarget()
    {
      List<int> intList = new List<int>();
      foreach (KeyValuePair<int, float> targetWeatherEndTime in this._targetWeatherEndTimes)
      {
        float blend;
        if ((double) targetWeatherEndTime.Value <= (double) this._currentTime)
        {
          intList.Add(targetWeatherEndTime.Key);
          blend = 1f;
        }
        else
        {
          float weatherStartTime = this._targetWeatherStartTimes[targetWeatherEndTime.Key];
          float num = targetWeatherEndTime.Value;
          blend = Mathf.Clamp((this._currentTime - weatherStartTime) / Mathf.Max(num - weatherStartTime, 1f), 0.0f, 1f);
        }
        string key = ((WeatherEffect) targetWeatherEndTime.Key).ToString();
        BaseSetting setting1 = (BaseSetting) this._startWeather.Settings[(object) key];
        BaseSetting setting2 = (BaseSetting) this._currentWeather.Settings[(object) key];
        BaseSetting setting3 = (BaseSetting) this._targetWeather.Settings[(object) key];
        switch (targetWeatherEndTime.Key)
        {
          case 1:
          case 2:
          case 4:
          case 5:
          case 7:
            ((TypedSetting<Color>) setting2).Value = Color.Lerp(((TypedSetting<Color>) setting1).Value, ((TypedSetting<Color>) setting3).Value, blend);
            break;
          case 3:
            Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
            if (Object.op_Inequality((Object) blendedSkybox, (Object) null))
            {
              if ((double) blend >= 1.0)
                ((TypedSetting<string>) setting2).Value = ((TypedSetting<string>) setting3).Value;
              WeatherManager.SetSkyboxBlend(blendedSkybox, blend);
              break;
            }
            break;
          case 6:
          case 8:
          case 9:
          case 10:
          case 11:
            ((TypedSetting<float>) setting2).Value = Mathf.Lerp(((TypedSetting<float>) setting1).Value, ((TypedSetting<float>) setting3).Value, blend);
            break;
        }
        this._needApply.Add((WeatherEffect) targetWeatherEndTime.Key);
      }
      foreach (int key in intList)
      {
        this._targetWeatherStartTimes.Remove(key);
        this._targetWeatherEndTimes.Remove(key);
      }
    }

    private void ApplyCurrentWeather(bool firstStart, bool applyAll)
    {
      if (applyAll)
        this._needApply = RCextensions.EnumToList<WeatherEffect>();
      WeatherEffectLevel weatherEffectLevel = (WeatherEffectLevel) SettingsManager.GraphicsSettings.WeatherEffects.Value;
      foreach (WeatherEffect key in this._needApply)
      {
        if (firstStart || weatherEffectLevel != WeatherEffectLevel.Low || this.LowEffects.Contains(key))
        {
          switch (key)
          {
            case WeatherEffect.Daylight:
              this._mainLight.color = this._currentWeather.Daylight.Value;
              continue;
            case WeatherEffect.AmbientLight:
              RenderSettings.ambientLight = this._currentWeather.AmbientLight.Value;
              continue;
            case WeatherEffect.Skybox:
              this.StartCoroutine(this.WaitAndApplySkybox());
              continue;
            case WeatherEffect.SkyboxColor:
              Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
              if (Object.op_Inequality((Object) blendedSkybox, (Object) null))
              {
                blendedSkybox.SetColor("_Tint", this._currentWeather.SkyboxColor.Value);
                continue;
              }
              continue;
            case WeatherEffect.Flashlight:
              ((FlashlightWeatherEffect) this._effects[WeatherEffect.Flashlight]).SetColor(this._currentWeather.Flashlight.Value);
              if ((double) this._currentWeather.Flashlight.Value.a > 0.0 && Color.op_Inequality(this._currentWeather.Flashlight.Value, Color.black))
              {
                if (!((Component) this._effects[WeatherEffect.Flashlight]).gameObject.activeSelf)
                {
                  this._effects[WeatherEffect.Flashlight].Enable();
                  continue;
                }
                continue;
              }
              this._effects[WeatherEffect.Flashlight].Disable();
              continue;
            case WeatherEffect.FogDensity:
              if ((double) this._currentWeather.FogDensity.Value > 0.0)
              {
                RenderSettings.fog = true;
                RenderSettings.fogMode = (FogMode) 2;
                RenderSettings.fogDensity = this._currentWeather.FogDensity.Value * 0.05f;
                continue;
              }
              RenderSettings.fog = false;
              continue;
            case WeatherEffect.FogColor:
              RenderSettings.fogColor = this._currentWeather.FogColor.Value;
              continue;
            case WeatherEffect.Rain:
            case WeatherEffect.Thunder:
            case WeatherEffect.Snow:
            case WeatherEffect.Wind:
              float level = ((TypedSetting<float>) this._currentWeather.Settings[(object) key.ToString()]).Value;
              this._effects[key].SetLevel(level);
              if ((double) level > 0.0)
              {
                if (!((Component) this._effects[key]).gameObject.activeSelf)
                {
                  this._effects[key].Randomize();
                  this._effects[key].Enable();
                  continue;
                }
                continue;
              }
              this._effects[key].Disable(true);
              continue;
            default:
              continue;
          }
        }
      }
      this._needApply.Clear();
    }

    private IEnumerator WaitAndApplySkybox()
    {
      yield return (object) new WaitForEndOfFrame();
      Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
      if (Object.op_Inequality((Object) blendedSkybox, (Object) null) && Object.op_Inequality((Object) this._skybox.material, (Object) blendedSkybox) && Object.op_Equality((Object) SkyboxCustomSkinLoader.SkyboxMaterial, (Object) null))
      {
        blendedSkybox.SetColor("_Tint", this._currentWeather.SkyboxColor.Value);
        this._skybox.material = blendedSkybox;
        if (Object.op_Inequality((Object) IN_GAME_MAIN_CAMERA.Instance, (Object) null))
          IN_GAME_MAIN_CAMERA.Instance.UpdateSnapshotSkybox();
      }
    }

    private Material GetBlendedSkybox(string skybox1, string skybox2) => WeatherManager.SkyboxBlendedMaterials.ContainsKey(skybox1) && WeatherManager.SkyboxBlendedMaterials[skybox1].ContainsKey(skybox2) ? WeatherManager.SkyboxBlendedMaterials[skybox1][skybox2] : (Material) null;

    public static void OnSetWeatherRPC(
      string currentWeatherJson,
      string startWeatherJson,
      string targetWeatherJson,
      Dictionary<int, float> targetWeatherStartTimes,
      Dictionary<int, float> targetWeatherEndTimes,
      float currentTime,
      PhotonMessageInfo info)
    {
      if (info != null && info.sender != PhotonNetwork.masterClient || SettingsManager.GraphicsSettings.WeatherEffects.Value == 0)
        return;
      WeatherManager._instance.StartCoroutine(WeatherManager._instance.WaitAndFinishOnSetWeather(currentWeatherJson, startWeatherJson, targetWeatherJson, targetWeatherStartTimes, targetWeatherEndTimes, currentTime));
    }

    private IEnumerator WaitAndFinishOnSetWeather(
      string currentWeatherJson,
      string startWeatherJson,
      string targetWeatherJson,
      Dictionary<int, float> targetWeatherStartTimes,
      Dictionary<int, float> targetWeatherEndTimes,
      float currentTime)
    {
      while (!this._finishedLoading)
        yield return (object) null;
      this._currentWeather.DeserializeFromJsonString(currentWeatherJson);
      this._startWeather.DeserializeFromJsonString(startWeatherJson);
      this._targetWeather.DeserializeFromJsonString(targetWeatherJson);
      this._targetWeatherStartTimes = targetWeatherStartTimes;
      this._targetWeatherEndTimes = targetWeatherEndTimes;
      this._currentTime = currentTime;
      this.LerpCurrentWeatherToTarget();
      this.ApplyCurrentWeather(false, true);
    }
  }
}
