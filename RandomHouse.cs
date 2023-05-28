// Decompiled with JetBrains decompiler
// Type: RandomHouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RandomHouse : MonoBehaviour
{
  private void Start() => ((Component) this).transform.localScale = new Vector3(4f + Random.Range(0.0f, 4f), 4f + Random.Range(0.0f, 6f), 4f + Random.Range(2f, 18f));

  private void Update()
  {
  }
}
