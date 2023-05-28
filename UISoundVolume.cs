// Decompiled with JetBrains decompiler
// Type: UISoundVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Sound Volume")]
[RequireComponent(typeof (UISlider))]
public class UISoundVolume : MonoBehaviour
{
  private UISlider mSlider;

  private void Awake()
  {
    this.mSlider = ((Component) this).GetComponent<UISlider>();
    this.mSlider.sliderValue = NGUITools.soundVolume;
    this.mSlider.eventReceiver = ((Component) this).gameObject;
  }

  private void OnSliderChange(float val) => NGUITools.soundVolume = val;
}
