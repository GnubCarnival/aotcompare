// Decompiled with JetBrains decompiler
// Type: BTN_save_snapshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BTN_save_snapshot : MonoBehaviour
{
  public GameObject info;
  public GameObject targetTexture;
  public GameObject[] thingsNeedToHide;

  private void Awake() => this.info.GetComponent<UILabel>().text = string.Empty;

  private void OnClick()
  {
    foreach (GameObject gameObject in this.thingsNeedToHide)
    {
      Transform transform = gameObject.transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.up, 10000f));
    }
    this.StartCoroutine(this.ScreenshotEncode());
    this.info.GetComponent<UILabel>().text = "Saving...";
  }

  [DebuggerHidden]
  private IEnumerator ScreenshotEncode() => (IEnumerator) new BTN_save_snapshot.ScreenshotEncodecIterator0()
  {
    fthis = this
  };
}
