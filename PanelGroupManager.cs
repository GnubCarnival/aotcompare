// Decompiled with JetBrains decompiler
// Type: PanelGroupManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class PanelGroupManager
{
  public GameObject[] panelGroup;

  public void ActivePanel(int index)
  {
    foreach (GameObject gameObject in this.panelGroup)
      gameObject.SetActive(false);
    this.panelGroup[index].SetActive(true);
  }
}
