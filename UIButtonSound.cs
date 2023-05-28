// Decompiled with JetBrains decompiler
// Type: UIButtonSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
  public AudioClip audioClip;
  public float pitch = 1f;
  public UIButtonSound.Trigger trigger;
  public float volume = 1f;

  private void OnClick()
  {
    if (!((Behaviour) this).enabled || this.trigger != UIButtonSound.Trigger.OnClick)
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  private void OnHover(bool isOver)
  {
    if (!((Behaviour) this).enabled || (!isOver || this.trigger != UIButtonSound.Trigger.OnMouseOver) && (isOver || this.trigger != UIButtonSound.Trigger.OnMouseOut))
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled || (!isPressed || this.trigger != UIButtonSound.Trigger.OnPress) && (isPressed || this.trigger != UIButtonSound.Trigger.OnRelease))
      return;
    NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
  }

  public enum Trigger
  {
    OnClick,
    OnMouseOver,
    OnMouseOut,
    OnPress,
    OnRelease,
  }
}
