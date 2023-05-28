// Decompiled with JetBrains decompiler
// Type: CheckBoxCostume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CheckBoxCostume : MonoBehaviour
{
  public static int costumeSet;
  public int set = 1;

  private void OnActivate(bool yes)
  {
    if (!yes)
      return;
    CheckBoxCostume.costumeSet = this.set;
  }
}
