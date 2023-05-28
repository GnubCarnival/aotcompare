// Decompiled with JetBrains decompiler
// Type: RegionTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

internal class RegionTrigger : MonoBehaviour
{
  public string myName;
  public RCEvent playerEventEnter;
  public RCEvent playerEventExit;
  public RCEvent titanEventEnter;
  public RCEvent titanEventExit;

  public void CopyTrigger(RegionTrigger copyTrigger)
  {
    this.playerEventEnter = copyTrigger.playerEventEnter;
    this.titanEventEnter = copyTrigger.titanEventEnter;
    this.playerEventExit = copyTrigger.playerEventExit;
    this.titanEventExit = copyTrigger.titanEventExit;
    this.myName = copyTrigger.myName;
  }

  private void OnTriggerEnter(Collider other)
  {
    GameObject gameObject = ((Component) ((Component) other).transform).gameObject;
    if (gameObject.layer == 8)
    {
      if (this.playerEventEnter == null)
        return;
      HERO component = gameObject.GetComponent<HERO>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      string rcVariableName = (string) FengGameManagerMKII.RCVariableNames[(object) ("OnPlayerEnterRegion[" + this.myName + "]")];
      if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName))
        FengGameManagerMKII.playerVariables[(object) rcVariableName] = (object) component.photonView.owner;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName, (object) component.photonView.owner);
      this.playerEventEnter.checkEvent();
    }
    else
    {
      if (gameObject.layer != 11 || this.titanEventEnter == null)
        return;
      TITAN component = ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      string rcVariableName = (string) FengGameManagerMKII.RCVariableNames[(object) ("OnTitanEnterRegion[" + this.myName + "]")];
      if (((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) rcVariableName))
        FengGameManagerMKII.titanVariables[(object) rcVariableName] = (object) component;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) rcVariableName, (object) component);
      this.titanEventEnter.checkEvent();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    GameObject gameObject = ((Component) ((Component) other).transform.root).gameObject;
    if (gameObject.layer == 8)
    {
      if (this.playerEventExit == null)
        return;
      HERO component = gameObject.GetComponent<HERO>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      string rcVariableName = (string) FengGameManagerMKII.RCVariableNames[(object) ("OnPlayerLeaveRegion[" + this.myName + "]")];
      if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName))
        FengGameManagerMKII.playerVariables[(object) rcVariableName] = (object) component.photonView.owner;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName, (object) component.photonView.owner);
    }
    else
    {
      if (gameObject.layer != 11 || this.titanEventExit == null)
        return;
      TITAN component = gameObject.GetComponent<TITAN>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      string rcVariableName = (string) FengGameManagerMKII.RCVariableNames[(object) ("OnTitanLeaveRegion[" + this.myName + "]")];
      if (((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) rcVariableName))
        FengGameManagerMKII.titanVariables[(object) rcVariableName] = (object) component;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) rcVariableName, (object) component);
      this.titanEventExit.checkEvent();
    }
  }
}
