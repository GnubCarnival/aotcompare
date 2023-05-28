// Decompiled with JetBrains decompiler
// Type: PopListCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class PopListCamera : MonoBehaviour
{
  private void Awake()
  {
    if (!PlayerPrefs.HasKey("cameraType"))
      return;
    ((Component) this).GetComponent<UIPopupList>().selection = PlayerPrefs.GetString("cameraType");
  }

  private void OnSelectionChange()
  {
    if (((Component) this).GetComponent<UIPopupList>().selection == "ORIGINAL")
      IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
    if (((Component) this).GetComponent<UIPopupList>().selection == "WOW")
      IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.WOW;
    if (((Component) this).GetComponent<UIPopupList>().selection == "TPS")
      IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
    PlayerPrefs.SetString("cameraType", ((Component) this).GetComponent<UIPopupList>().selection);
  }
}
