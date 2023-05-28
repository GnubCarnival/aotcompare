// Decompiled with JetBrains decompiler
// Type: Weather.SnowWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Weather
{
  internal class SnowWeatherEffect : BaseWeatherEffect
  {
    protected override Vector3 _positionOffset => Vector3.op_Multiply(Vector3.up, 0.0f);

    public override void Randomize()
    {
      this._particleEmitters[0].rndVelocity = new Vector3(20f, 20f, 20f);
      this._particleEmitters[0].minEnergy = this._particleEmitters[0].maxEnergy = 1.2f;
      this._particleEmitters[1].rndVelocity = new Vector3(5f, 5f, 5f);
      this._particleEmitters[1].localVelocity = new Vector3(20f * Util.GetRandomSign(), 0.0f, 0.0f);
      this._particleEmitters[1].minEnergy = this._particleEmitters[0].maxEnergy = 1.2f;
    }

    public override void SetLevel(float level)
    {
      base.SetLevel(level);
      if ((double) level <= 0.0)
        return;
      if ((double) level <= 0.5)
      {
        float num = level / 0.5f;
        this.SetActiveEmitter(0);
        this.SetActiveAudio(0, (float) (0.25 + 0.25 * (double) num));
        this._particleEmitters[0].minEmission = this._particleEmitters[0].maxEmission = this.ClampParticles((float) (100.0 + (double) num * 300.0));
        this._particleEmitters[0].minSize = this._particleEmitters[0].maxSize = 25f;
      }
      else
      {
        float num = (float) (((double) level - 0.5) / 0.5);
        this.SetActiveEmitter(1);
        this.SetAudioVolume(1, (float) (0.25 + 0.25 * (double) num));
        this._particleEmitters[1].minEmission = this._particleEmitters[1].maxEmission = this.ClampParticles((float) (200.0 + (double) num * 200.0));
        this._particleEmitters[1].minSize = this._particleEmitters[1].maxSize = 12f;
      }
    }

    public override void Setup(Transform parent) => base.Setup(parent);
  }
}
