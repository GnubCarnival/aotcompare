// Decompiled with JetBrains decompiler
// Type: DragDropItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item")]
public class DragDropItem : MonoBehaviour
{
  private bool mIsDragging;
  private Transform mParent;
  private bool mSticky;
  private Transform mTrans;
  public GameObject prefab;

  private void Awake() => this.mTrans = ((Component) this).transform;

  private void Drop()
  {
    Collider collider = ((RaycastHit) ref UICamera.lastHit).collider;
    DragDropContainer component = Object.op_Equality((Object) collider, (Object) null) ? (DragDropContainer) null : ((Component) collider).gameObject.GetComponent<DragDropContainer>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      this.mTrans.parent = ((Component) component).transform;
      Vector3 localPosition = this.mTrans.localPosition;
      localPosition.z = 0.0f;
      this.mTrans.localPosition = localPosition;
    }
    else
      this.mTrans.parent = this.mParent;
    this.UpdateTable();
    NGUITools.MarkParentAsChanged(((Component) this).gameObject);
  }

  private void OnDrag(Vector2 delta)
  {
    if (!((Behaviour) this).enabled || UICamera.currentTouchID <= -2)
      return;
    if (!this.mIsDragging)
    {
      this.mIsDragging = true;
      this.mParent = this.mTrans.parent;
      this.mTrans.parent = DragDropRoot.root;
      Vector3 localPosition = this.mTrans.localPosition;
      localPosition.z = 0.0f;
      this.mTrans.localPosition = localPosition;
      NGUITools.MarkParentAsChanged(((Component) this).gameObject);
    }
    else
    {
      Transform mTrans = this.mTrans;
      mTrans.localPosition = Vector3.op_Addition(mTrans.localPosition, Vector2.op_Implicit(delta));
    }
  }

  private void OnPress(bool isPressed)
  {
    if (!((Behaviour) this).enabled)
      return;
    if (isPressed)
    {
      if (!UICamera.current.stickyPress)
      {
        this.mSticky = true;
        UICamera.current.stickyPress = true;
      }
    }
    else if (this.mSticky)
    {
      this.mSticky = false;
      UICamera.current.stickyPress = false;
    }
    this.mIsDragging = false;
    Collider collider = ((Component) this).collider;
    if (Object.op_Inequality((Object) collider, (Object) null))
      collider.enabled = !isPressed;
    if (isPressed)
      return;
    this.Drop();
  }

  private void UpdateTable()
  {
    UITable inParents = NGUITools.FindInParents<UITable>(((Component) this).gameObject);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    inParents.repositionNow = true;
  }
}
