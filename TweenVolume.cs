// Decompiled with JetBrains decompiler
// Type: TweenVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Tween/Volume")]
public class TweenVolume : UITweener
{
  public float from;
  private AudioSource mSource;
  public float to = 1f;

  public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
  {
    TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration);
    tweenVolume.from = tweenVolume.volume;
    tweenVolume.to = targetVolume;
    if ((double) duration <= 0.0)
    {
      tweenVolume.Sample(1f, true);
      ((Behaviour) tweenVolume).enabled = false;
    }
    return tweenVolume;
  }

  protected override void OnUpdate(float factor, bool isFinished)
  {
    this.volume = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);
    ((Behaviour) this.mSource).enabled = (double) this.mSource.volume > 0.0099999997764825821;
  }

  public AudioSource audioSource
  {
    get
    {
      if (Object.op_Equality((Object) this.mSource, (Object) null))
      {
        this.mSource = ((Component) this).audio;
        if (Object.op_Equality((Object) this.mSource, (Object) null))
        {
          this.mSource = ((Component) this).GetComponentInChildren<AudioSource>();
          if (Object.op_Equality((Object) this.mSource, (Object) null))
          {
            Debug.LogError((object) "TweenVolume needs an AudioSource to work with", (Object) this);
            ((Behaviour) this).enabled = false;
          }
        }
      }
      return this.mSource;
    }
  }

  public float volume
  {
    get => this.audioSource.volume;
    set => this.audioSource.volume = value;
  }
}
