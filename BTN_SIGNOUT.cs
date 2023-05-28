// Decompiled with JetBrains decompiler
// Type: BTN_SIGNOUT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_SIGNOUT : MonoBehaviour
{
  public GameObject logincomponent;
  public GameObject loginPanel;

  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(this.loginPanel, true);
    this.logincomponent.GetComponent<LoginFengKAI>().logout();
  }
}
