// Decompiled with JetBrains decompiler
// Type: BTN_Connect_To_Server_On_List
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_Connect_To_Server_On_List : MonoBehaviour
{
  public int index;
  public string roomName;

  private void OnClick() => ((Component) ((Component) this).transform.parent.parent).GetComponent<PanelMultiJoin>().connectToIndex(this.index, this.roomName);
}
