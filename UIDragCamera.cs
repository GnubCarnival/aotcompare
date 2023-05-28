// Decompiled with JetBrains decompiler
// Type: UIDragCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Camera")]
[ExecuteInEditMode]
public class UIDragCamera : IgnoreTimeScale
{
  public UIDraggableCamera draggableCamera;
  [SerializeField]
  [HideInInspector]
  private Component target;

  private void Awake()
  {
    if (Object.op_Inequality((Object) this.target, (Object) null))
    {
      if (Object.op_Equality((Object) this.draggableCamera, (Object) null))
      {
        this.draggableCamera = this.target.GetComponent<UIDraggableCamera>();
        if (Object.op_Equality((Object) this.draggableCamera, (Object) null))
          this.draggableCamera = this.target.gameObject.AddComponent<UIDraggableCamera>();
      }
      this.target = (Component) null;
    }
    else
    {
      if (!Object.op_Equality((Object) this.draggableCamera, (Object) null))
        return;
      this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(((Component) this).gameObject);
    }
  }

  private void OnDrag(Vector2 delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggableCamera, (Object) null))
      return;
    this.draggableCamera.Drag(delta);
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggableCamera, (Object) null))
      return;
    this.draggableCamera.Press(isPressed);
  }

  private void OnScroll(float delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggableCamera, (Object) null))
      return;
    this.draggableCamera.Scroll(delta);
  }
}
