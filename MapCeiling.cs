// Decompiled with JetBrains decompiler
// Type: MapCeiling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

public class MapCeiling : MonoBehaviour
{
  private GameObject _barrierRef;
  private Color _color;
  private float _minAlpha;
  private float _maxAlpha = 0.6f;
  private float _minimumHeight = 3f;
  private static float _forestHeight = 280f;
  private static float _cityHeight = 210f;
  private static float _forestWidth = 1320f;
  private static float _cityWidth = 1400f;
  private static float _depth = 20f;

  public static void CreateMapCeiling()
  {
    if (FengGameManagerMKII.level.StartsWith("The Forest"))
    {
      MapCeiling.CreateMapCeilingWithDimensions(MapCeiling._forestHeight, MapCeiling._forestWidth, MapCeiling._depth);
    }
    else
    {
      if (!FengGameManagerMKII.level.StartsWith("The City"))
        return;
      MapCeiling.CreateMapCeilingWithDimensions(MapCeiling._cityHeight, MapCeiling._cityWidth, MapCeiling._depth);
    }
  }

  private static void CreateMapCeilingWithDimensions(float height, float width, float depth)
  {
    GameObject gameObject = new GameObject();
    gameObject.AddComponent<MapCeiling>();
    gameObject.transform.position = new Vector3(0.0f, height, 0.0f);
    gameObject.transform.rotation = Quaternion.identity;
    gameObject.transform.localScale = new Vector3(width, depth, width);
  }

  private void Start()
  {
    this.CreateCeilingPart("barrier");
    this._barrierRef = this.CreateCeilingPart("killcuboid");
    this._color = new Color(1f, 0.0f, 0.0f, this._maxAlpha);
    this.UpdateTransparency();
  }

  private GameObject CreateCeilingPart(string asset)
  {
    GameObject ceilingPart = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(asset), Vector3.zero, Quaternion.identity);
    ceilingPart.transform.position = ((Component) this).transform.position;
    ceilingPart.transform.rotation = ((Component) this).transform.rotation;
    ceilingPart.transform.localScale = ((Component) this).transform.localScale;
    return ceilingPart;
  }

  private void Update() => this.UpdateTransparency();

  private float getMinAlpha() => this._minAlpha;

  private void setMinAlpha(float newMinAlpha) => this._minAlpha = (double) newMinAlpha <= 1.0 && (double) newMinAlpha >= 0.0 ? newMinAlpha : throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");

  public float getMaxAlpha() => this._maxAlpha;

  public void setMaxAlpha(float newMaxAlpha) => this._maxAlpha = (double) newMaxAlpha <= 1.0 && (double) newMaxAlpha >= 0.0 ? newMaxAlpha : throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");

  public void UpdateTransparency()
  {
    if (!Object.op_Inequality((Object) Camera.main, (Object) null) || !Object.op_Inequality((Object) this._barrierRef, (Object) null) || !Object.op_Inequality((Object) this._barrierRef.renderer, (Object) null))
      return;
    float x = this._maxAlpha;
    try
    {
      float inMin = this._barrierRef.transform.position.y / this._minimumHeight;
      x = (double) ((Component) Camera.main).transform.position.y >= (double) inMin ? this.Map(((Component) Camera.main).transform.position.y, inMin, this._barrierRef.transform.position.y, this._minAlpha, this._maxAlpha) : this._minAlpha;
      x = this.fadeByGradient(x);
    }
    catch
    {
    }
    this._color.a = x;
    this._barrierRef.renderer.material.color = this._color;
  }

  public float fadeByGradient(float x) => Mathf.Clamp(10f * x * x, this._minAlpha, this._maxAlpha);

  public float Map(float x, float inMin, float inMax, float outMin, float outMax)
  {
    if ((double) x > (double) inMax || (double) x < (double) inMin)
      throw new Exception("Error,\npublic float map(float x, float inMin, float inMax, float outMin, float outMax)\nis not defined for values (x > inMax || x < inMin)");
    return (float) (((double) x - (double) inMin) * ((double) outMax - (double) outMin) / ((double) inMax - (double) inMin)) + outMin;
  }
}
