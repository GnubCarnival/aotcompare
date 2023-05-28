// Decompiled with JetBrains decompiler
// Type: BTN_TO_REGISTER
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_TO_REGISTER : MonoBehaviour
{
  public GameObject registerPanel;

  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(this.registerPanel, true);
  }
}
