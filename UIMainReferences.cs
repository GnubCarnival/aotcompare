// Decompiled with JetBrains decompiler
// Type: UIMainReferences
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using UnityEngine;

public class UIMainReferences : MonoBehaviour
{
  public GameObject panelCredits;
  public GameObject PanelDisconnect;
  public GameObject panelMain;
  public GameObject PanelMultiJoinPrivate;
  public GameObject PanelMultiPWD;
  public GameObject panelMultiROOM;
  public GameObject panelMultiSet;
  public GameObject panelMultiStart;
  public GameObject PanelMultiWait;
  public GameObject panelOption;
  public GameObject panelSingleSet;
  public GameObject PanelSnapShot;
  public static string Version = "01042015";

  private void Awake()
  {
    NGUITools.SetActive(this.panelMain, false);
    Object.Destroy((Object) GameObject.Find("PopupListLang"));
    MainApplicationManager.Init();
  }
}
