// Decompiled with JetBrains decompiler
// Type: UISpriteAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite Animation")]
[RequireComponent(typeof (UISprite))]
[ExecuteInEditMode]
public class UISpriteAnimation : MonoBehaviour
{
  private bool mActive = true;
  private float mDelta;
  [HideInInspector]
  [SerializeField]
  private int mFPS = 30;
  private int mIndex;
  [SerializeField]
  [HideInInspector]
  private bool mLoop = true;
  [HideInInspector]
  [SerializeField]
  private string mPrefix = string.Empty;
  private UISprite mSprite;
  private List<string> mSpriteNames = new List<string>();

  private void RebuildSpriteList()
  {
    if (Object.op_Equality((Object) this.mSprite, (Object) null))
      this.mSprite = ((Component) this).GetComponent<UISprite>();
    this.mSpriteNames.Clear();
    if (!Object.op_Inequality((Object) this.mSprite, (Object) null) || !Object.op_Inequality((Object) this.mSprite.atlas, (Object) null))
      return;
    List<UIAtlas.Sprite> spriteList = this.mSprite.atlas.spriteList;
    int index = 0;
    for (int count = spriteList.Count; index < count; ++index)
    {
      UIAtlas.Sprite sprite = spriteList[index];
      if (string.IsNullOrEmpty(this.mPrefix) || sprite.name.StartsWith(this.mPrefix))
        this.mSpriteNames.Add(sprite.name);
    }
    this.mSpriteNames.Sort();
  }

  public void Reset()
  {
    this.mActive = true;
    this.mIndex = 0;
    if (!Object.op_Inequality((Object) this.mSprite, (Object) null) || this.mSpriteNames.Count <= 0)
      return;
    this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
    this.mSprite.MakePixelPerfect();
  }

  private void Start() => this.RebuildSpriteList();

  private void Update()
  {
    if (!this.mActive || this.mSpriteNames.Count <= 1 || !Application.isPlaying || (double) this.mFPS <= 0.0)
      return;
    this.mDelta += Time.deltaTime;
    float num = 1f / (float) this.mFPS;
    if ((double) num >= (double) this.mDelta)
      return;
    this.mDelta = (double) num <= 0.0 ? 0.0f : this.mDelta - num;
    if (++this.mIndex >= this.mSpriteNames.Count)
    {
      this.mIndex = 0;
      this.mActive = this.loop;
    }
    if (!this.mActive)
      return;
    this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
    this.mSprite.MakePixelPerfect();
  }

  public int frames => this.mSpriteNames.Count;

  public int framesPerSecond
  {
    get => this.mFPS;
    set => this.mFPS = value;
  }

  public bool isPlaying => this.mActive;

  public bool loop
  {
    get => this.mLoop;
    set => this.mLoop = value;
  }

  public string namePrefix
  {
    get => this.mPrefix;
    set
    {
      if (!(this.mPrefix != value))
        return;
      this.mPrefix = value;
      this.RebuildSpriteList();
    }
  }
}
