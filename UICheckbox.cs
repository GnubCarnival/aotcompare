// Decompiled with JetBrains decompiler
// Type: UICheckbox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class UICheckbox : MonoBehaviour
{
  public Animation checkAnimation;
  public UISprite checkSprite;
  public static UICheckbox current;
  public GameObject eventReceiver;
  public string functionName = "OnActivate";
  public bool instantTween;
  private bool mChecked = true;
  private bool mStarted;
  private Transform mTrans;
  public UICheckbox.OnStateChange onStateChange;
  [SerializeField]
  [HideInInspector]
  private bool option;
  public bool optionCanBeNone;
  public Transform radioButtonRoot;
  public bool startsChecked = true;

  private void Awake()
  {
    this.mTrans = ((Component) this).transform;
    if (Object.op_Inequality((Object) this.checkSprite, (Object) null))
      this.checkSprite.alpha = !this.startsChecked ? 0.0f : 1f;
    if (!this.option)
      return;
    this.option = false;
    if (!Object.op_Equality((Object) this.radioButtonRoot, (Object) null))
      return;
    this.radioButtonRoot = this.mTrans.parent;
  }

  private void OnClick()
  {
    if (!((Behaviour) this).enabled)
      return;
    this.isChecked = !this.isChecked;
  }

  private void Set(bool state)
  {
    if (!this.mStarted)
    {
      this.mChecked = state;
      this.startsChecked = state;
      if (!Object.op_Inequality((Object) this.checkSprite, (Object) null))
        return;
      this.checkSprite.alpha = !state ? 0.0f : 1f;
    }
    else
    {
      if (this.mChecked == state)
        return;
      if (Object.op_Inequality((Object) this.radioButtonRoot, (Object) null) & state)
      {
        UICheckbox[] componentsInChildren = ((Component) this.radioButtonRoot).GetComponentsInChildren<UICheckbox>(true);
        int index = 0;
        for (int length = componentsInChildren.Length; index < length; ++index)
        {
          UICheckbox uiCheckbox = componentsInChildren[index];
          if (Object.op_Inequality((Object) uiCheckbox, (Object) this) && Object.op_Equality((Object) uiCheckbox.radioButtonRoot, (Object) this.radioButtonRoot))
            uiCheckbox.Set(false);
        }
      }
      this.mChecked = state;
      if (Object.op_Inequality((Object) this.checkSprite, (Object) null))
      {
        if (this.instantTween)
          this.checkSprite.alpha = !this.mChecked ? 0.0f : 1f;
        else
          TweenAlpha.Begin(((Component) this.checkSprite).gameObject, 0.15f, !this.mChecked ? 0.0f : 1f);
      }
      UICheckbox.current = this;
      if (this.onStateChange != null)
        this.onStateChange(this.mChecked);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.functionName))
        this.eventReceiver.SendMessage(this.functionName, (object) this.mChecked, (SendMessageOptions) 1);
      UICheckbox.current = (UICheckbox) null;
      if (!Object.op_Inequality((Object) this.checkAnimation, (Object) null))
        return;
      ActiveAnimation.Play(this.checkAnimation, !state ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward);
    }
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.eventReceiver, (Object) null))
      this.eventReceiver = ((Component) this).gameObject;
    this.mChecked = !this.startsChecked;
    this.mStarted = true;
    this.Set(this.startsChecked);
  }

  public bool isChecked
  {
    get => this.mChecked;
    set
    {
      if (!(Object.op_Equality((Object) this.radioButtonRoot, (Object) null) | value) && !this.optionCanBeNone && this.mStarted)
        return;
      this.Set(value);
    }
  }

  public delegate void OnStateChange(bool state);
}
