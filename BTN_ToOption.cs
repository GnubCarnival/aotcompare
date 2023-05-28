// Decompiled with JetBrains decompiler
// Type: BTN_ToOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_ToOption : MonoBehaviour
{
  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelOption, true);
  }
}
