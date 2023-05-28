// Decompiled with JetBrains decompiler
// Type: DownloadTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof (UITexture))]
public class DownloadTexture : MonoBehaviour
{
  private Material mMat;
  private Texture2D mTex;
  public string url = "http://www.tasharen.com/misc/logo.png";

  private void OnDestroy()
  {
    if (Object.op_Inequality((Object) this.mMat, (Object) null))
      Object.Destroy((Object) this.mMat);
    if (!Object.op_Inequality((Object) this.mTex, (Object) null))
      return;
    Object.Destroy((Object) this.mTex);
  }

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new DownloadTexture.StartcIterator7()
  {
    fthis = this
  };
}
