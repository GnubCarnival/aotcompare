// Decompiled with JetBrains decompiler
// Type: BTN_Enter_PWD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_Enter_PWD : MonoBehaviour
{
  private void OnClick()
  {
    if (GameObject.Find("InputEnterPWD").GetComponent<UIInput>().label.text == new SimpleAES().Decrypt(PanelMultiJoinPWD.Password))
    {
      PhotonNetwork.JoinRoom(PanelMultiJoinPWD.roomName);
    }
    else
    {
      NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, false);
      NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, true);
      GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().refresh();
    }
  }
}
