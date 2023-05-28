// Decompiled with JetBrains decompiler
// Type: BTN_Create_Game
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_Create_Game : MonoBehaviour
{
  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiSet, true);
  }
}
