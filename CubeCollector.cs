// Decompiled with JetBrains decompiler
// Type: CubeCollector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CubeCollector : MonoBehaviour
{
  public int type;

  private void Start()
  {
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) GameObject.FindGameObjectWithTag("Player"), (Object) null) || (double) Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, ((Component) this).transform.position) >= 8.0)
      return;
    Object.Destroy((Object) ((Component) this).gameObject);
  }
}
