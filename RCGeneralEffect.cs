// Decompiled with JetBrains decompiler
// Type: RCGeneralEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class RCGeneralEffect : MonoBehaviour
{
  private void Awake() => Object.Destroy((Object) ((Component) this).gameObject, 1.5f);
}
