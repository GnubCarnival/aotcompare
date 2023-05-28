// Decompiled with JetBrains decompiler
// Type: RotateSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RotateSample : MonoBehaviour
{
  private void Start() => iTween.RotateBy(((Component) this).gameObject, iTween.Hash((object) "x", (object) 0.25, (object) "easeType", (object) "easeInOutBack", (object) "loopType", (object) "pingPong", (object) "delay", (object) 0.4));
}
