// Decompiled with JetBrains decompiler
// Type: ParentFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class ParentFollow : MonoBehaviour
{
  private Transform bTransform;
  public bool isActiveInScene;
  private Transform parent;

  private void Awake()
  {
    this.bTransform = ((Component) this).transform;
    this.isActiveInScene = true;
  }

  public void RemoveParent() => this.parent = (Transform) null;

  public void SetParent(Transform transform)
  {
    this.parent = transform;
    this.bTransform.rotation = transform.rotation;
  }

  private void Update()
  {
    if (!this.isActiveInScene || !Object.op_Inequality((Object) this.parent, (Object) null))
      return;
    this.bTransform.position = this.parent.position;
  }
}
