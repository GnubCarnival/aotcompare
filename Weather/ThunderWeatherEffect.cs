// Decompiled with JetBrains decompiler
// Type: Weather.ThunderWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Weather
{
  internal class ThunderWeatherEffect : BaseWeatherEffect
  {
    public static List<List<LightningParticle>> LightningPool = new List<List<LightningParticle>>();
    protected float _lightningWaitTime = Random.Range(10f, 20f);
    private const int MaxLightningParticles = 4;

    protected override Vector3 _positionOffset => Vector3.op_Multiply(Vector3.up, 0.0f);

    public static void FinishLoadAssets()
    {
      for (int index1 = 0; index1 < 10; ++index1)
      {
        Vector3 start = Vector3.op_Addition(Vector3.op_Multiply(Vector3.up, 1500f), Vector3.op_Multiply(Vector3.right, Random.Range(-1000f, 1000f)));
        Vector3 end = Vector3.op_Addition(Vector3.op_Multiply(Vector3.down, 300f), Vector3.op_Multiply(Vector3.right, Random.Range(-1000f, 1000f)));
        double num = (double) Vector3.Distance(start, end);
        int generation = 9;
        if (SettingsManager.GraphicsSettings.WeatherEffects.Value == 2)
          generation = 8;
        List<LightningParticle> lightningParticleList = new List<LightningParticle>();
        for (int index2 = 0; index2 < 4; ++index2)
        {
          LightningParticle lightningParticle = AssetBundleManager.InstantiateAsset<GameObject>("LightningParticle").AddComponent<LightningParticle>();
          Object.DontDestroyOnLoad((Object) ((Component) lightningParticle).gameObject);
          lightningParticle.Setup(start, end, generation);
          lightningParticleList.Add(lightningParticle);
          lightningParticle.Disable();
        }
        ThunderWeatherEffect.LightningPool.Add(lightningParticleList);
      }
    }

    public override void Randomize()
    {
    }

    public override void Setup(Transform parent) => base.Setup(parent);

    public override void SetLevel(float level)
    {
      base.SetLevel(level);
      if ((double) level <= 0.0)
        return;
      if ((double) level < 0.5)
        this.SetActiveAudio(0, 1f);
      else
        this.SetActiveAudio(1, 1f);
    }

    protected void FixedUpdate()
    {
      this._lightningWaitTime -= Time.fixedDeltaTime;
      if ((double) this._lightningWaitTime > 0.0)
        return;
      this._lightningWaitTime = (float) (20.0 - (double) this._level * 15.0);
      this._lightningWaitTime = Random.Range(this._lightningWaitTime * 0.5f, this._lightningWaitTime * 1.5f);
      this.CreateLightning();
    }

    protected void CreateLightning()
    {
      List<LightningParticle> lightningParticleList = ThunderWeatherEffect.LightningPool[Random.Range(0, ThunderWeatherEffect.LightningPool.Count)];
      int num1 = Random.Range(1, 4);
      float fieldOfView = Camera.main.fieldOfView;
      Vector3 vector3_1 = new Vector3(this._parent.forward.x, 0.0f, this._parent.forward.z);
      Vector3 normalized = ((Vector3) ref vector3_1).normalized;
      Quaternion.op_Multiply(Quaternion.AngleAxis(Random.Range((float) (-(double) fieldOfView * 0.5), fieldOfView * 0.5f), Vector3.up), normalized);
      float num2 = Random.Range(900f, 1400f);
      Vector3 vector3_2 = Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(normalized, num2));
      for (int index = 0; index < num1; ++index)
      {
        ((Component) lightningParticleList[index]).transform.position = vector3_2;
        ((Component) lightningParticleList[index]).transform.LookAt(this._parent);
        lightningParticleList[index].Enable();
        lightningParticleList[index].Strike(index == 0);
      }
    }
  }
}
