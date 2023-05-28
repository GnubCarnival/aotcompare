// Decompiled with JetBrains decompiler
// Type: InputToEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class InputToEvent : MonoBehaviour
{
  public bool DetectPointedAtGameObject;
  public static Vector3 inputHitPos;
  private GameObject lastGo;

  private void Press(Vector2 screenPos)
  {
    this.lastGo = this.RaycastObject(screenPos);
    if (!Object.op_Inequality((Object) this.lastGo, (Object) null))
      return;
    this.lastGo.SendMessage("OnPress", (SendMessageOptions) 1);
  }

  private GameObject RaycastObject(Vector2 screenPos)
  {
    RaycastHit raycastHit;
    if (!Physics.Raycast(((Component) this).camera.ScreenPointToRay(Vector2.op_Implicit(screenPos)), ref raycastHit, 200f))
      return (GameObject) null;
    InputToEvent.inputHitPos = ((RaycastHit) ref raycastHit).point;
    return ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
  }

  private void Release(Vector2 screenPos)
  {
    if (!Object.op_Inequality((Object) this.lastGo, (Object) null))
      return;
    if (Object.op_Equality((Object) this.RaycastObject(screenPos), (Object) this.lastGo))
      this.lastGo.SendMessage("OnClick", (SendMessageOptions) 1);
    this.lastGo.SendMessage("OnRelease", (SendMessageOptions) 1);
    this.lastGo = (GameObject) null;
  }

  private void Update()
  {
    if (this.DetectPointedAtGameObject)
      InputToEvent.goPointedAt = this.RaycastObject(Vector2.op_Implicit(Input.mousePosition));
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      if (((Touch) ref touch).phase == null)
      {
        this.Press(((Touch) ref touch).position);
      }
      else
      {
        if (((Touch) ref touch).phase != 3)
          return;
        this.Release(((Touch) ref touch).position);
      }
    }
    else
    {
      if (Input.GetMouseButtonDown(0))
        this.Press(Vector2.op_Implicit(Input.mousePosition));
      if (!Input.GetMouseButtonUp(0))
        return;
      this.Release(Vector2.op_Implicit(Input.mousePosition));
    }
  }

  public static GameObject goPointedAt { get; private set; }
}
