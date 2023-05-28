// Decompiled with JetBrains decompiler
// Type: BTN_Server_List_PgUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_Server_List_PgUp : MonoBehaviour
{
  private void OnClick() => GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().pageUp();
}
