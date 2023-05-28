// Decompiled with JetBrains decompiler
// Type: BTN_SSR_up
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_SSR_up : MonoBehaviour
{
  public GameObject panel;

  private void OnClick() => this.panel.GetComponent<SnapShotReview>().ShowPrevIMG();
}
