// Decompiled with JetBrains decompiler
// Type: BTN_SAVE_CC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_SAVE_CC : MonoBehaviour
{
  public GameObject manager;

  private void OnClick() => this.manager.GetComponent<CustomCharacterManager>().SaveData();
}
