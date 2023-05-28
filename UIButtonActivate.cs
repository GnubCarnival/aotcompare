// Decompiled with JetBrains decompiler
// Type: UIButtonActivate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
  public bool state = true;
  public GameObject target;

  private void OnClick()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null))
      return;
    NGUITools.SetActive(this.target, this.state);
  }
}
