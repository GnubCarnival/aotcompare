// Decompiled with JetBrains decompiler
// Type: UIGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
[ExecuteInEditMode]
public class UIGrid : MonoBehaviour
{
  public UIGrid.Arrangement arrangement;
  public float cellHeight = 200f;
  public float cellWidth = 200f;
  public bool hideInactive = true;
  public int maxPerLine;
  private bool mStarted;
  public bool repositionNow;
  public bool sorted;

  public void Reposition()
  {
    if (!this.mStarted)
    {
      this.repositionNow = true;
    }
    else
    {
      Transform transform1 = ((Component) this).transform;
      int num1 = 0;
      int num2 = 0;
      if (this.sorted)
      {
        List<Transform> transformList = new List<Transform>();
        for (int index = 0; index < transform1.childCount; ++index)
        {
          Transform child = transform1.GetChild(index);
          if (Object.op_Inequality((Object) child, (Object) null) && (!this.hideInactive || NGUITools.GetActive(((Component) child).gameObject)))
            transformList.Add(child);
        }
        transformList.Sort(new Comparison<Transform>(UIGrid.SortByName));
        int index1 = 0;
        for (int count = transformList.Count; index1 < count; ++index1)
        {
          Transform transform2 = transformList[index1];
          if (NGUITools.GetActive(((Component) transform2).gameObject) || !this.hideInactive)
          {
            float z = transform2.localPosition.z;
            transform2.localPosition = this.arrangement != UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float) num2, -this.cellHeight * (float) num1, z) : new Vector3(this.cellWidth * (float) num1, -this.cellHeight * (float) num2, z);
            if (++num1 >= this.maxPerLine && this.maxPerLine > 0)
            {
              num1 = 0;
              ++num2;
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < transform1.childCount; ++index)
        {
          Transform child = transform1.GetChild(index);
          if (NGUITools.GetActive(((Component) child).gameObject) || !this.hideInactive)
          {
            float z = child.localPosition.z;
            child.localPosition = this.arrangement != UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float) num2, -this.cellHeight * (float) num1, z) : new Vector3(this.cellWidth * (float) num1, -this.cellHeight * (float) num2, z);
            if (++num1 >= this.maxPerLine && this.maxPerLine > 0)
            {
              num1 = 0;
              ++num2;
            }
          }
        }
      }
      UIDraggablePanel inParents = NGUITools.FindInParents<UIDraggablePanel>(((Component) this).gameObject);
      if (!Object.op_Inequality((Object) inParents, (Object) null))
        return;
      inParents.UpdateScrollbars(true);
    }
  }

  public static int SortByName(Transform a, Transform b) => string.Compare(((Object) a).name, ((Object) b).name);

  private void Start()
  {
    this.mStarted = true;
    this.Reposition();
  }

  private void Update()
  {
    if (!this.repositionNow)
      return;
    this.repositionNow = false;
    this.Reposition();
  }

  public enum Arrangement
  {
    Horizontal,
    Vertical,
  }
}
