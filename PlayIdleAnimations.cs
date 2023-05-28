﻿// Decompiled with JetBrains decompiler
// Type: PlayIdleAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
  private Animation mAnim;
  private List<AnimationClip> mBreaks = new List<AnimationClip>();
  private AnimationClip mIdle;
  private int mLastIndex;
  private float mNextBreak;

  private void Start()
  {
    this.mAnim = ((Component) this).GetComponentInChildren<Animation>();
    if (Object.op_Equality((Object) this.mAnim, (Object) null))
    {
      Debug.LogWarning((object) (NGUITools.GetHierarchy(((Component) this).gameObject) + " has no Animation component"));
      Object.Destroy((Object) this);
    }
    else
    {
      foreach (AnimationState animationState in this.mAnim)
      {
        if (((Object) animationState.clip).name == "idle")
        {
          animationState.layer = 0;
          this.mIdle = animationState.clip;
          this.mAnim.Play(((Object) this.mIdle).name);
        }
        else if (((Object) animationState.clip).name.StartsWith("idle"))
        {
          animationState.layer = 1;
          this.mBreaks.Add(animationState.clip);
        }
      }
      if (this.mBreaks.Count != 0)
        return;
      Object.Destroy((Object) this);
    }
  }

  private void Update()
  {
    if ((double) this.mNextBreak >= (double) Time.time)
      return;
    if (this.mBreaks.Count == 1)
    {
      AnimationClip mBreak = this.mBreaks[0];
      this.mNextBreak = Time.time + mBreak.length + Random.Range(5f, 15f);
      this.mAnim.CrossFade(((Object) mBreak).name);
    }
    else
    {
      int index = Random.Range(0, this.mBreaks.Count - 1);
      if (this.mLastIndex == index)
      {
        ++index;
        if (index >= this.mBreaks.Count)
          index = 0;
      }
      this.mLastIndex = index;
      AnimationClip mBreak = this.mBreaks[index];
      this.mNextBreak = Time.time + mBreak.length + Random.Range(2f, 8f);
      this.mAnim.CrossFade(((Object) mBreak).name);
    }
  }
}
