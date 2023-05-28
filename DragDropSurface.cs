// Decompiled with JetBrains decompiler
// Type: DragDropSurface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Surface")]
public class DragDropSurface : MonoBehaviour
{
  public bool rotatePlacedObject;

  private void OnDrop(GameObject go)
  {
    DragDropItem component = go.GetComponent<DragDropItem>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    Transform transform = NGUITools.AddChild(((Component) this).gameObject, component.prefab).transform;
    transform.position = ((RaycastHit) ref UICamera.lastHit).point;
    if (this.rotatePlacedObject)
      transform.rotation = Quaternion.op_Multiply(Quaternion.LookRotation(((RaycastHit) ref UICamera.lastHit).normal), Quaternion.Euler(90f, 0.0f, 0.0f));
    Object.Destroy((Object) go);
  }
}
