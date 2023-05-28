// Decompiled with JetBrains decompiler
// Type: Weather.LightningParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weather
{
  public class LightningParticle : MonoBehaviour
  {
    private const float FadeInTime = 0.5f;
    private const float StayTime = 0.3f;
    private const float FadeOutTime = 1f;
    private const float ChaosFactor = 0.2f;
    private const float StartWidth = 2f;
    private const float EndWidth = 2f;
    protected Color LightningColor = new Color(228f, 245f, (float) byte.MaxValue);
    private static Random _random = new Random();
    protected LineRenderer _lineRenderer;
    protected int _startIndex;
    protected List<AudioSource> _audioSources = new List<AudioSource>();

    private static void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
    {
      if (Vector3.op_Equality(directionNormalized, Vector3.zero))
      {
        side = Vector3.right;
      }
      else
      {
        float x = directionNormalized.x;
        float y = directionNormalized.y;
        float z = directionNormalized.z;
        float num1 = Mathf.Abs(x);
        float num2 = Mathf.Abs(y);
        float num3 = Mathf.Abs(z);
        float num4;
        float num5;
        float num6;
        if ((double) num1 >= (double) num2 && (double) num2 >= (double) num3)
        {
          num4 = 1f;
          num5 = 1f;
          num6 = (float) -((double) y * (double) num4 + (double) z * (double) num5) / x;
        }
        else if ((double) num2 >= (double) num3)
        {
          num6 = 1f;
          num5 = 1f;
          num4 = (float) -((double) x * (double) num6 + (double) z * (double) num5) / y;
        }
        else
        {
          num6 = 1f;
          num4 = 1f;
          num5 = (float) -((double) x * (double) num6 + (double) y * (double) num4) / z;
        }
        ref Vector3 local = ref side;
        Vector3 vector3 = new Vector3(num6, num4, num5);
        Vector3 normalized = ((Vector3) ref vector3).normalized;
        local = normalized;
      }
    }

    public static List<Vector3> GenerateLightningBoltPositions(
      Vector3 start,
      Vector3 end,
      int generation,
      float offsetAmount = 0.0f)
    {
      int index1 = 0;
      List<KeyValuePair<Vector3, Vector3>> keyValuePairList = new List<KeyValuePair<Vector3, Vector3>>();
      keyValuePairList.Add(new KeyValuePair<Vector3, Vector3>(start, end));
      if ((double) offsetAmount <= 0.0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(end, start);
        offsetAmount = ((Vector3) ref vector3).magnitude * 0.2f;
      }
      KeyValuePair<Vector3, Vector3> keyValuePair;
      while (generation-- > 0)
      {
        int num = index1;
        index1 = keyValuePairList.Count;
        for (int index2 = num; index2 < index1; ++index2)
        {
          keyValuePair = keyValuePairList[index2];
          start = keyValuePair.Key;
          keyValuePair = keyValuePairList[index2];
          end = keyValuePair.Value;
          Vector3 vector3 = Vector3.op_Multiply(Vector3.op_Addition(start, end), 0.5f);
          Vector3 result;
          LightningParticle.RandomVector(ref start, ref end, offsetAmount, out result);
          Vector3 key = Vector3.op_Addition(vector3, result);
          keyValuePairList.Add(new KeyValuePair<Vector3, Vector3>(start, key));
          keyValuePairList.Add(new KeyValuePair<Vector3, Vector3>(key, end));
        }
        offsetAmount *= 0.5f;
      }
      List<Vector3> lightningBoltPositions = new List<Vector3>();
      List<Vector3> vector3List1 = lightningBoltPositions;
      keyValuePair = keyValuePairList[index1];
      Vector3 key1 = keyValuePair.Key;
      vector3List1.Add(key1);
      for (int index3 = index1; index3 < keyValuePairList.Count; ++index3)
      {
        List<Vector3> vector3List2 = lightningBoltPositions;
        keyValuePair = keyValuePairList[index3];
        Vector3 vector3 = keyValuePair.Value;
        vector3List2.Add(vector3);
      }
      return lightningBoltPositions;
    }

    private static void RandomVector(
      ref Vector3 start,
      ref Vector3 end,
      float offsetAmount,
      out Vector3 result)
    {
      Vector3 vector3 = Vector3.op_Subtraction(end, start);
      Vector3 normalized = ((Vector3) ref vector3).normalized;
      Vector3 side;
      LightningParticle.GetPerpendicularVector(ref normalized, out side);
      float num1 = ((float) LightningParticle._random.NextDouble() + 0.1f) * offsetAmount;
      float num2 = (float) LightningParticle._random.NextDouble() * 360f;
      result = Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(num2, normalized), side), num1);
    }

    private void Awake()
    {
      this._lineRenderer = ((Component) this).GetComponent<LineRenderer>();
      this._lineRenderer.SetVertexCount(0);
      this._audioSources = ((IEnumerable<AudioSource>) ((Component) this).GetComponentsInChildren<AudioSource>()).OrderBy<AudioSource, string>((Func<AudioSource, string>) (x => ((Object) ((Component) x).gameObject).name)).ToList<AudioSource>();
    }

    public void Disable()
    {
      foreach (AudioSource audioSource in this._audioSources)
        audioSource.Stop();
      this._lineRenderer.SetColors(Color.clear, Color.clear);
      ((Component) this).gameObject.SetActive(false);
    }

    public void Enable()
    {
      this._lineRenderer.SetColors(Color.clear, Color.clear);
      ((Component) this).gameObject.SetActive(true);
    }

    public void Strike(bool sound) => this.StartCoroutine(this.StrikeCoroutine(sound));

    public void PlayAudio()
    {
      this.SetVolume(0.3f);
      this._audioSources[Random.Range(0, 2)].Play();
    }

    public void Setup(Vector3 start, Vector3 end, int generation)
    {
      List<Vector3> lightningBoltPositions = LightningParticle.GenerateLightningBoltPositions(start, end, generation);
      this._lineRenderer.SetVertexCount(lightningBoltPositions.Count);
      for (int index = 0; index < lightningBoltPositions.Count; ++index)
        this._lineRenderer.SetPosition(index, lightningBoltPositions[index]);
    }

    private IEnumerator StrikeCoroutine(bool sound)
    {
      Color color = this.LightningColor;
      float maxAlpha = Application.loadedLevel == 0 ? 0.3f : 1f;
      color.a = 0.0f;
      this._lineRenderer.SetColors(color, color);
      this._lineRenderer.SetWidth(2f, 2f);
      float startTime = Time.time;
      while ((double) Time.time - (double) startTime < 0.5)
      {
        color.a = Mathf.Clamp((float) (((double) Time.time - (double) startTime) / 0.5), 0.0f, 1f) * maxAlpha;
        this._lineRenderer.SetColors(color, color);
        yield return (object) new WaitForEndOfFrame();
      }
      if (sound)
        this.PlayAudio();
      color.a = maxAlpha;
      this._lineRenderer.SetColors(color, color);
      yield return (object) new WaitForSeconds(0.3f);
      startTime = Time.time;
      while ((double) Time.time - (double) startTime < 1.0)
      {
        float num = Mathf.Clamp((float) (((double) Time.time - (double) startTime) / 1.0), 0.0f, 1f);
        color.a = (float) ((1.0 - (double) num) * (1.0 - (double) num)) * maxAlpha;
        this._lineRenderer.SetColors(color, color);
        this.SetVolume((float) (0.30000001192092896 * (1.0 - (double) num)));
        yield return (object) new WaitForEndOfFrame();
      }
      this.Disable();
    }

    private void SetVolume(float volume)
    {
      foreach (AudioSource audioSource in this._audioSources)
        audioSource.volume = volume;
    }
  }
}
