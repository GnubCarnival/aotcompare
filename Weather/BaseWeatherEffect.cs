// Decompiled with JetBrains decompiler
// Type: Weather.BaseWeatherEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weather
{
  internal class BaseWeatherEffect : MonoBehaviour
  {
    protected Transform _parent;
    protected Transform _transform;
    protected float _level;
    protected float _maxParticles;
    protected float _particleMultiplier;
    protected List<ParticleEmitter> _particleEmitters = new List<ParticleEmitter>();
    protected List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    protected List<AudioSource> _audioSources = new List<AudioSource>();
    protected Dictionary<AudioSource, float> _audioTargetVolumes = new Dictionary<AudioSource, float>();
    protected Dictionary<AudioSource, float> _audioStartTimes = new Dictionary<AudioSource, float>();
    protected Dictionary<AudioSource, float> _audioStartVolumes = new Dictionary<AudioSource, float>();
    protected bool _isDisabling;

    protected virtual Vector3 _positionOffset => Vector3.zero;

    protected virtual float _audioFadeTime => 2f;

    public virtual void Disable(bool fadeOut = false)
    {
      if (!((Component) this).gameObject.activeSelf)
        return;
      if (fadeOut)
      {
        if (this._isDisabling)
          return;
        this.StartCoroutine(this.WaitAndDisable());
      }
      else
      {
        this.StopAllCoroutines();
        this.StopAllAudio();
        this.StopAllEmitters();
        this.StopAllParticleSystems();
        ((Component) this).gameObject.SetActive(false);
        this._isDisabling = false;
      }
    }

    public virtual void Enable()
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      ((Component) this).gameObject.SetActive(true);
      this._isDisabling = false;
    }

    private IEnumerator WaitAndDisable()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      BaseWeatherEffect baseWeatherEffect = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        ((Component) baseWeatherEffect).gameObject.SetActive(false);
        baseWeatherEffect._isDisabling = false;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      baseWeatherEffect._isDisabling = true;
      baseWeatherEffect.StopAllAudio(true);
      baseWeatherEffect.StopAllEmitters();
      baseWeatherEffect.StopAllParticleSystems();
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (object) new WaitForSeconds(baseWeatherEffect._audioFadeTime);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    public virtual void Randomize()
    {
    }

    public virtual void SetParent(Transform parent) => this._parent = parent;

    public virtual void SetLevel(float level) => this._level = level;

    public virtual void Setup(Transform parent)
    {
      this._transform = ((Component) this).transform;
      this._parent = parent;
      if (SettingsManager.GraphicsSettings.WeatherEffects.Value == 3)
      {
        this._maxParticles = 500f;
        this._particleMultiplier = 1f;
      }
      else
      {
        this._maxParticles = 200f;
        this._particleMultiplier = 0.7f;
      }
      this._particleEmitters = ((IEnumerable<ParticleEmitter>) ((Component) this).GetComponentsInChildren<ParticleEmitter>()).OrderBy<ParticleEmitter, string>((Func<ParticleEmitter, string>) (x => ((Object) ((Component) x).gameObject).name)).ToList<ParticleEmitter>();
      this._particleSystems = ((IEnumerable<ParticleSystem>) ((Component) this).GetComponentsInChildren<ParticleSystem>()).OrderBy<ParticleSystem, string>((Func<ParticleSystem, string>) (x => ((Object) ((Component) x).gameObject).name)).ToList<ParticleSystem>();
      this._audioSources = ((IEnumerable<AudioSource>) ((Component) this).GetComponentsInChildren<AudioSource>()).OrderBy<AudioSource, string>((Func<AudioSource, string>) (x => ((Object) ((Component) x).gameObject).name)).ToList<AudioSource>();
      foreach (ParticleEmitter particleEmitter in this._particleEmitters)
      {
        particleEmitter.emit = false;
        ((Component) particleEmitter).transform.localPosition = this._positionOffset;
        ((Component) particleEmitter).transform.localRotation = Quaternion.identity;
      }
      foreach (ParticleSystem particleSystem in this._particleSystems)
      {
        particleSystem.Stop();
        ((Component) particleSystem).transform.localPosition = this._positionOffset;
        ((Component) particleSystem).transform.localRotation = Quaternion.identity;
      }
      this.StopAllAudio();
    }

    protected virtual void SetActiveEmitter(int index)
    {
      this.StopAllEmitters();
      this.StopAllParticleSystems();
      this._particleEmitters[index].emit = true;
    }

    protected virtual void StopAllEmitters()
    {
      foreach (ParticleEmitter particleEmitter in this._particleEmitters)
        particleEmitter.emit = false;
    }

    protected virtual void SetActiveParticleSystem(int index)
    {
      this.StopAllEmitters();
      this.StopAllParticleSystems();
      if (this._particleSystems[index].isPlaying)
        return;
      this._particleSystems[index].Play();
    }

    protected virtual void StopAllParticleSystems()
    {
      foreach (ParticleSystem particleSystem in this._particleSystems)
        particleSystem.Stop();
    }

    protected virtual void SetActiveAudio(int index, float volume)
    {
      for (int index1 = 0; index1 < this._audioSources.Count; ++index1)
      {
        if (index1 == index)
          this.SetAudioVolume(index1, volume);
        else
          this.SetAudioVolume(index1, 0.0f);
      }
    }

    protected virtual void SetAudioVolume(int index, float volume) => this.SetAudioVolume(this._audioSources[index], volume);

    protected virtual void SetAudioVolume(AudioSource audio, float volume)
    {
      volume = Mathf.Clamp(volume, 0.0f, 1f);
      if ((double) this._audioTargetVolumes[audio] == (double) volume)
        return;
      this._audioTargetVolumes[audio] = volume;
      if ((double) volume != 0.0)
        return;
      this._audioStartTimes[audio] = Time.time;
      this._audioStartVolumes[audio] = audio.volume;
    }

    protected virtual void StopAllAudio(bool fadeOut = false)
    {
      if (fadeOut)
      {
        foreach (AudioSource audioSource in this._audioSources)
          this.SetAudioVolume(audioSource, 0.0f);
      }
      else
      {
        foreach (AudioSource audioSource in this._audioSources)
        {
          audioSource.Stop();
          this._audioTargetVolumes[audioSource] = 0.0f;
          this._audioStartTimes[audioSource] = 0.0f;
          this._audioStartVolumes[audioSource] = 0.0f;
        }
      }
    }

    protected virtual float ClampParticles(float count) => Mathf.Min(count * this._particleMultiplier, this._maxParticles);

    protected virtual void LateUpdate()
    {
      this._transform.position = this._parent.position;
      this.UpdateAudio();
    }

    protected virtual void UpdateAudio()
    {
      foreach (AudioSource audioSource in this._audioSources)
      {
        if ((double) this._audioTargetVolumes[audioSource] == 0.0)
        {
          if (audioSource.isPlaying)
          {
            audioSource.volume = this.GetLerpedVolume(audioSource);
            if ((double) audioSource.volume == 0.0)
              audioSource.Pause();
          }
        }
        else if (audioSource.isPlaying)
        {
          if ((double) audioSource.volume != (double) this._audioTargetVolumes[audioSource])
            audioSource.volume = this.GetLerpedVolume(audioSource);
        }
        else
        {
          this._audioStartTimes[audioSource] = Time.time;
          this._audioStartVolumes[audioSource] = 0.0f;
          audioSource.volume = this.GetLerpedVolume(audioSource);
          audioSource.Play();
        }
      }
    }

    protected virtual float GetLerpedVolume(AudioSource audio)
    {
      float num = Mathf.Clamp((Time.time - this._audioStartTimes[audio]) / this._audioFadeTime, 0.0f, 1f);
      return Mathf.Lerp(audio.volume, this._audioTargetVolumes[audio], num);
    }
  }
}
