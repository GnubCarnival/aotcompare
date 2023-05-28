// Decompiled with JetBrains decompiler
// Type: MoveSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class MoveSample : MonoBehaviour
{
  private void Start() => iTween.MoveBy(((Component) this).gameObject, iTween.Hash((object) "x", (object) 2, (object) "easeType", (object) "easeInOutExpo", (object) "loopType", (object) "pingPong", (object) "delay", (object) 0.1));
}
