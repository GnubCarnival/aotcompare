// Decompiled with JetBrains decompiler
// Type: CheckBoxCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CheckBoxCamera : MonoBehaviour
{
  public CAMERA_TYPE camera;

  private void OnSelectionChange(bool yes)
  {
    if (!yes)
      return;
    IN_GAME_MAIN_CAMERA.cameraMode = this.camera;
    PlayerPrefs.SetString("cameraType", this.camera.ToString().ToUpper());
  }

  private void Start()
  {
    if (!PlayerPrefs.HasKey("cameraType"))
      return;
    if (this.camera.ToString().ToUpper() == PlayerPrefs.GetString("cameraType").ToUpper())
      ((Component) this).GetComponent<UICheckbox>().isChecked = true;
    else
      ((Component) this).GetComponent<UICheckbox>().isChecked = false;
  }
}
