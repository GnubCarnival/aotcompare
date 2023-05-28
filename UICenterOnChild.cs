// Decompiled with JetBrains decompiler
// Type: UICenterOnChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center On Child")]
public class UICenterOnChild : MonoBehaviour
{
  private GameObject mCenteredObject;
  private UIDraggablePanel mDrag;
  public SpringPanel.OnFinished onFinished;
  public float springStrength = 8f;

  private void OnDragFinished()
  {
    if (!((Behaviour) this).enabled)
      return;
    this.Recenter();
  }

  private void OnEnable() => this.Recenter();

  public void Recenter()
  {
    if (Object.op_Equality((Object) this.mDrag, (Object) null))
    {
      this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(((Component) this).gameObject);
      if (Object.op_Equality((Object) this.mDrag, (Object) null))
      {
        Debug.LogWarning((object) (((object) this).GetType().ToString() + " requires " + (object) typeof (UIDraggablePanel) + " on a parent object in order to work"), (Object) this);
        ((Behaviour) this).enabled = false;
        return;
      }
      this.mDrag.onDragFinished = new UIDraggablePanel.OnDragFinished(this.OnDragFinished);
      if (Object.op_Inequality((Object) this.mDrag.horizontalScrollBar, (Object) null))
        this.mDrag.horizontalScrollBar.onDragFinished = new UIScrollBar.OnDragFinished(this.OnDragFinished);
      if (Object.op_Inequality((Object) this.mDrag.verticalScrollBar, (Object) null))
        this.mDrag.verticalScrollBar.onDragFinished = new UIScrollBar.OnDragFinished(this.OnDragFinished);
    }
    if (!Object.op_Inequality((Object) this.mDrag.panel, (Object) null))
      return;
    Vector4 clipRange = this.mDrag.panel.clipRange;
    Transform cachedTransform = this.mDrag.panel.cachedTransform;
    Vector3 vector3_1 = cachedTransform.localPosition;
    vector3_1.x += clipRange.x;
    vector3_1.y += clipRange.y;
    vector3_1 = cachedTransform.parent.TransformPoint(vector3_1);
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, Vector3.op_Multiply(this.mDrag.currentMomentum, this.mDrag.momentumAmount * 0.1f));
    this.mDrag.currentMomentum = Vector3.zero;
    float num1 = float.MaxValue;
    Transform transform1 = (Transform) null;
    Transform transform2 = ((Component) this).transform;
    int num2 = 0;
    for (int childCount = transform2.childCount; num2 < childCount; ++num2)
    {
      Transform child = transform2.GetChild(num2);
      float num3 = Vector3.SqrMagnitude(Vector3.op_Subtraction(child.position, vector3_2));
      if ((double) num3 < (double) num1)
      {
        num1 = num3;
        transform1 = child;
      }
    }
    if (Object.op_Inequality((Object) transform1, (Object) null))
    {
      this.mCenteredObject = ((Component) transform1).gameObject;
      Vector3 vector3_3 = Vector3.op_Subtraction(cachedTransform.InverseTransformPoint(transform1.position), cachedTransform.InverseTransformPoint(vector3_1));
      if ((double) this.mDrag.scale.x == 0.0)
        vector3_3.x = 0.0f;
      if ((double) this.mDrag.scale.y == 0.0)
        vector3_3.y = 0.0f;
      if ((double) this.mDrag.scale.z == 0.0)
        vector3_3.z = 0.0f;
      SpringPanel.Begin(((Component) this.mDrag).gameObject, Vector3.op_Subtraction(cachedTransform.localPosition, vector3_3), this.springStrength).onFinished = this.onFinished;
    }
    else
      this.mCenteredObject = (GameObject) null;
  }

  public GameObject centeredObject => this.mCenteredObject;
}
