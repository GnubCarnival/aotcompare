// Decompiled with JetBrains decompiler
// Type: UIDragPanelContents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Panel Contents")]
[ExecuteInEditMode]
public class UIDragPanelContents : MonoBehaviour
{
  public UIDraggablePanel draggablePanel;
  [HideInInspector]
  [SerializeField]
  private UIPanel panel;

  private void Awake()
  {
    if (!Object.op_Inequality((Object) this.panel, (Object) null))
      return;
    if (Object.op_Equality((Object) this.draggablePanel, (Object) null))
    {
      this.draggablePanel = ((Component) this.panel).GetComponent<UIDraggablePanel>();
      if (Object.op_Equality((Object) this.draggablePanel, (Object) null))
        this.draggablePanel = ((Component) this.panel).gameObject.AddComponent<UIDraggablePanel>();
    }
    this.panel = (UIPanel) null;
  }

  private void OnDrag(Vector2 delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggablePanel, (Object) null))
      return;
    this.draggablePanel.Drag();
  }

  private void OnPress(bool pressed)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggablePanel, (Object) null))
      return;
    this.draggablePanel.Press(pressed);
  }

  private void OnScroll(float delta)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !Object.op_Inequality((Object) this.draggablePanel, (Object) null))
      return;
    this.draggablePanel.Scroll(delta);
  }

  private void Start()
  {
    if (!Object.op_Equality((Object) this.draggablePanel, (Object) null))
      return;
    this.draggablePanel = NGUITools.FindInParents<UIDraggablePanel>(((Component) this).gameObject);
  }
}
