// Decompiled with JetBrains decompiler
// Type: QuitOnEscapeOrBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class QuitOnEscapeOrBack : MonoBehaviour
{
  private void Update()
  {
    if (!Input.GetKeyDown((KeyCode) 27))
      return;
    Application.Quit();
  }
}
