// Decompiled with JetBrains decompiler
// Type: Weather.RainWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Weather
{
  internal class RainWeatherEffect : BaseWeatherEffect
  {
    protected override Vector3 _positionOffset => Vector3.op_Multiply(Vector3.up, 30f);

    public override void Randomize()
    {
      float num1 = Random.Range(0.0f, 20f);
      float num2 = Random.Range(-num1, num1);
      foreach (ParticleEmitter particleEmitter in this._particleEmitters)
      {
        ((Component) particleEmitter).transform.localPosition = this._positionOffset;
        ((Component) particleEmitter).transform.localRotation = Quaternion.identity;
        ((Component) particleEmitter).transform.RotateAround(this._transform.position, Vector3.forward, num2);
        ((Component) particleEmitter).transform.RotateAround(this._transform.position, Vector3.up, Random.Range(0.0f, 360f));
      }
    }

    public override void SetLevel(float level)
    {
      base.SetLevel(level);
      if ((double) level <= 0.0)
        return;
      if ((double) level < 0.5)
      {
        float num = level / 0.5f;
        this.SetActiveEmitter(0);
        this._particleEmitters[0].minEmission = this._particleEmitters[0].maxEmission = this.ClampParticles((float) (50.0 + 150.0 * (double) num));
        this._particleEmitters[0].minSize = this._particleEmitters[0].maxSize = (float) (30.0 + 30.0 * (double) num);
        this.SetActiveAudio(0, (float) (0.25 + 0.25 * (double) num));
      }
      else
      {
        float num = (float) (((double) level - 0.5) / 0.5);
        this.SetActiveEmitter(1);
        this._particleEmitters[1].minEmission = this._particleEmitters[1].maxEmission = this.ClampParticles((float) (100.0 + 150.0 * (double) num));
        this._particleEmitters[1].minSize = this._particleEmitters[1].maxSize = (float) (50.0 + (double) num * 10.0);
        this.SetActiveAudio(1, (float) (0.25 + 0.25 * (double) num));
      }
    }

    public override void Setup(Transform parent)
    {
      base.Setup(parent);
      this._particleEmitters[0].localVelocity = Vector3.op_Multiply(Vector3.down, 100f);
      this._particleEmitters[1].localVelocity = Vector3.op_Multiply(Vector3.down, 100f);
      this._particleEmitters[0].rndVelocity = new Vector3(10f, 0.0f, 10f);
      this._particleEmitters[1].rndVelocity = new Vector3(10f, 0.0f, 10f);
    }
  }
}
