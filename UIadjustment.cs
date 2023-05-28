// Decompiled with JetBrains decompiler
// Type: UIadjustment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class UIadjustment : MonoBehaviour
{
  private void Start() => ((Component) this).transform.localScale = new Vector3((float) (Screen.width / 960), (float) (Screen.height / 600), 0.0f);

  private void Update() => ((Component) this).transform.localScale = new Vector3((float) (Screen.width / 960), (float) (Screen.height / 600), 0.0f);
}
