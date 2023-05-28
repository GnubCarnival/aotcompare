// Decompiled with JetBrains decompiler
// Type: Weather.FlashlightWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Weather
{
  internal class FlashlightWeatherEffect : BaseWeatherEffect
  {
    private Light _light;

    protected override Vector3 _positionOffset => Vector3.op_Multiply(Vector3.up, 0.0f);

    public override void Randomize()
    {
    }

    public override void Setup(Transform parent)
    {
      base.Setup(parent);
      this._light = ((Component) this).GetComponentInChildren<Light>();
      this._light.range = 120f;
      this._light.intensity = 1f;
      this._light.spotAngle = 60f;
      this.SetColor(Color.black);
    }

    public virtual void SetColor(Color color) => this._light.color = color;

    protected override void LateUpdate()
    {
      if (Object.op_Inequality((Object) this._parent, (Object) null))
      {
        if (!((Component) this._light).gameObject.activeSelf)
          ((Component) this._light).gameObject.SetActive(true);
        this._transform.rotation = Quaternion.op_Multiply(this._parent.rotation, Quaternion.Euler(353f, 0.0f, 0.0f));
        this._transform.position = this._parent.position;
      }
      else
      {
        if (!((Component) this._light).gameObject.activeSelf)
          return;
        ((Component) this._light).gameObject.SetActive(false);
      }
    }
  }
}
