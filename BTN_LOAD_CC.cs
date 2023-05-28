// Decompiled with JetBrains decompiler
// Type: BTN_LOAD_CC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_LOAD_CC : MonoBehaviour
{
  public GameObject manager;

  private void OnClick() => this.manager.GetComponent<CustomCharacterManager>().LoadData();
}
