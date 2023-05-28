// Decompiled with JetBrains decompiler
// Type: BTN_leave_lobby
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_leave_lobby : MonoBehaviour
{
  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiStart, true);
    PhotonNetwork.Disconnect();
  }
}
