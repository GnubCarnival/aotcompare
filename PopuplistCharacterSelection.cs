﻿// Decompiled with JetBrains decompiler
// Type: PopuplistCharacterSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class PopuplistCharacterSelection : MonoBehaviour
{
  public GameObject ACL;
  public GameObject BLA;
  public GameObject GAS;
  public GameObject SPD;

  private void onCharacterChange()
  {
    string selection = ((Component) this).GetComponent<UIPopupList>().selection;
    HeroStat heroStat;
    if (selection == "Set 1" || selection == "Set 2" || selection == "Set 3")
    {
      HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(selection.ToUpper());
      heroStat = heroCostume != null ? heroCostume.stat : new HeroStat();
    }
    else
      heroStat = HeroStat.getInfo(((Component) this).GetComponent<UIPopupList>().selection);
    this.SPD.transform.localScale = new Vector3((float) heroStat.SPD, 20f, 0.0f);
    this.GAS.transform.localScale = new Vector3((float) heroStat.GAS, 20f, 0.0f);
    this.BLA.transform.localScale = new Vector3((float) heroStat.BLA, 20f, 0.0f);
    this.ACL.transform.localScale = new Vector3((float) heroStat.ACL, 20f, 0.0f);
  }
}
