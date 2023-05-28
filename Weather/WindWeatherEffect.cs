// Decompiled with JetBrains decompiler
// Type: Weather.WindWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Weather
{
  internal class WindWeatherEffect : BaseWeatherEffect
  {
    public static bool WindEnabled = false;
    public static Vector3 WindDirection = Vector3.zero;

    protected override Vector3 _positionOffset => Vector3.op_Multiply(Vector3.up, 0.0f);

    public override void Setup(Transform parent) => base.Setup(parent);

    public override void Randomize()
    {
      Vector3 vector3 = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
      WindWeatherEffect.WindDirection = ((Vector3) ref vector3).normalized;
    }

    public override void Disable(bool fadeOut = false)
    {
      WindWeatherEffect.WindEnabled = false;
      base.Disable(fadeOut);
    }

    public override void SetLevel(float level)
    {
      base.SetLevel(level);
      if ((double) level <= 0.0)
        return;
      WindWeatherEffect.WindEnabled = true;
      if ((double) level < 0.5)
        this.SetAudioVolume(0, 1f);
      else
        this.SetAudioVolume(1, 0.5f);
    }
  }
}
