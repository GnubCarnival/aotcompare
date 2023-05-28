// Decompiled with JetBrains decompiler
// Type: UITable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : MonoBehaviour
{
  public int columns;
  public UITable.Direction direction;
  public bool hideInactive = true;
  public bool keepWithinPanel;
  private List<Transform> mChildren = new List<Transform>();
  private UIDraggablePanel mDrag;
  private UIPanel mPanel;
  private bool mStarted;
  public UITable.OnReposition onReposition;
  public Vector2 padding = Vector2.zero;
  public bool repositionNow;
  public bool sorted;

  private void LateUpdate()
  {
    if (!this.repositionNow)
      return;
    this.repositionNow = false;
    this.Reposition();
  }

  public void Reposition()
  {
    if (this.mStarted)
    {
      Transform transform = ((Component) this).transform;
      this.mChildren.Clear();
      List<Transform> children = this.children;
      if (children.Count > 0)
        this.RepositionVariableSize(children);
      if (Object.op_Inequality((Object) this.mDrag, (Object) null))
      {
        this.mDrag.UpdateScrollbars(true);
        this.mDrag.RestrictWithinBounds(true);
      }
      else if (Object.op_Inequality((Object) this.mPanel, (Object) null))
        this.mPanel.ConstrainTargetToBounds(transform, true);
      if (this.onReposition == null)
        return;
      this.onReposition();
    }
    else
      this.repositionNow = true;
  }

  private void RepositionVariableSize(List<Transform> children)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    int length1 = this.columns <= 0 ? 1 : children.Count / this.columns + 1;
    int length2 = this.columns <= 0 ? children.Count : this.columns;
    Bounds[,] boundsArray1 = new Bounds[length1, length2];
    Bounds[] boundsArray2 = new Bounds[length2];
    Bounds[] boundsArray3 = new Bounds[length1];
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    for (int count = children.Count; index3 < count; ++index3)
    {
      Transform child = children[index3];
      Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(child);
      Vector3 localScale = child.localScale;
      ((Bounds) ref relativeWidgetBounds).min = Vector3.Scale(((Bounds) ref relativeWidgetBounds).min, localScale);
      ((Bounds) ref relativeWidgetBounds).max = Vector3.Scale(((Bounds) ref relativeWidgetBounds).max, localScale);
      boundsArray1[index2, index1] = relativeWidgetBounds;
      ((Bounds) ref boundsArray2[index1]).Encapsulate(relativeWidgetBounds);
      ((Bounds) ref boundsArray3[index2]).Encapsulate(relativeWidgetBounds);
      if (++index1 >= this.columns && this.columns > 0)
      {
        index1 = 0;
        ++index2;
      }
    }
    int index4 = 0;
    int index5 = 0;
    int index6 = 0;
    for (int count = children.Count; index6 < count; ++index6)
    {
      Transform child = children[index6];
      Bounds bounds1 = boundsArray1[index5, index4];
      Bounds bounds2 = boundsArray2[index4];
      Bounds bounds3 = boundsArray3[index5];
      Vector3 localPosition = child.localPosition;
      localPosition.x = num1 + ((Bounds) ref bounds1).extents.x - ((Bounds) ref bounds1).center.x;
      localPosition.x += ((Bounds) ref bounds1).min.x - ((Bounds) ref bounds2).min.x + this.padding.x;
      if (this.direction == UITable.Direction.Down)
      {
        localPosition.y = -num2 - ((Bounds) ref bounds1).extents.y - ((Bounds) ref bounds1).center.y;
        localPosition.y += (float) (((double) ((Bounds) ref bounds1).max.y - (double) ((Bounds) ref bounds1).min.y - (double) ((Bounds) ref bounds3).max.y + (double) ((Bounds) ref bounds3).min.y) * 0.5) - this.padding.y;
      }
      else
      {
        localPosition.y = num2 + ((Bounds) ref bounds1).extents.y - ((Bounds) ref bounds1).center.y;
        localPosition.y += (float) (((double) ((Bounds) ref bounds1).max.y - (double) ((Bounds) ref bounds1).min.y - (double) ((Bounds) ref bounds3).max.y + (double) ((Bounds) ref bounds3).min.y) * 0.5) - this.padding.y;
      }
      num1 += (float) ((double) ((Bounds) ref bounds2).max.x - (double) ((Bounds) ref bounds2).min.x + (double) this.padding.x * 2.0);
      child.localPosition = localPosition;
      if (++index4 >= this.columns && this.columns > 0)
      {
        index4 = 0;
        ++index5;
        num1 = 0.0f;
        num2 += ((Bounds) ref bounds3).size.y + this.padding.y * 2f;
      }
    }
  }

  public static int SortByName(Transform a, Transform b) => string.Compare(((Object) a).name, ((Object) b).name);

  private void Start()
  {
    this.mStarted = true;
    if (this.keepWithinPanel)
    {
      this.mPanel = NGUITools.FindInParents<UIPanel>(((Component) this).gameObject);
      this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(((Component) this).gameObject);
    }
    this.Reposition();
  }

  public List<Transform> children
  {
    get
    {
      if (this.mChildren.Count == 0)
      {
        Transform transform = ((Component) this).transform;
        this.mChildren.Clear();
        for (int index = 0; index < transform.childCount; ++index)
        {
          Transform child = transform.GetChild(index);
          if (Object.op_Inequality((Object) child, (Object) null) && Object.op_Inequality((Object) ((Component) child).gameObject, (Object) null) && (!this.hideInactive || NGUITools.GetActive(((Component) child).gameObject)))
            this.mChildren.Add(child);
        }
        if (this.sorted)
          this.mChildren.Sort(new Comparison<Transform>(UITable.SortByName));
      }
      return this.mChildren;
    }
  }

  public enum Direction
  {
    Down,
    Up,
  }

  public delegate void OnReposition();
}
