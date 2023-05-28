// Decompiled with JetBrains decompiler
// Type: BTN_START_MULTI_SERVER
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_START_MULTI_SERVER : MonoBehaviour
{
  private void OnClick()
  {
    string text = GameObject.Find("InputServerName").GetComponent<UIInput>().label.text;
    int maxPlayers = int.Parse(GameObject.Find("InputMaxPlayer").GetComponent<UIInput>().label.text);
    int num = int.Parse(GameObject.Find("InputMaxTime").GetComponent<UIInput>().label.text);
    string selection = GameObject.Find("PopupListMap").GetComponent<UIPopupList>().selection;
    string str1 = !GameObject.Find("CheckboxHard").GetComponent<UICheckbox>().isChecked ? (!GameObject.Find("CheckboxAbnormal").GetComponent<UICheckbox>().isChecked ? "normal" : "abnormal") : "hard";
    string str2 = "day";
    string unencrypted = GameObject.Find("InputStartServerPWD").GetComponent<UIInput>().label.text;
    if (unencrypted.Length > 0)
      unencrypted = new SimpleAES().Encrypt(unencrypted);
    PhotonNetwork.CreateRoom(text + "`" + selection + "`" + str1 + "`" + (object) num + "`" + str2 + "`" + unencrypted + "`" + (object) Random.Range(0, 50000), true, true, maxPlayers);
  }
}
