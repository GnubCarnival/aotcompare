// Decompiled with JetBrains decompiler
// Type: ClickToMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class ClickToMove : MonoBehaviour
{
  public int smooth;
  private Vector3 targetPosition;

  public void Main()
  {
  }

  public void Update()
  {
    if (Input.GetKeyDown((KeyCode) 323))
    {
      Plane plane;
      // ISSUE: explicit constructor call
      ((Plane) ref plane).\u002Ector(Vector3.up, ((Component) this).transform.position);
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      float num = 0.0f;
      if (((Plane) ref plane).Raycast(ray, ref num))
      {
        Vector3 point = ((Ray) ref ray).GetPoint(num);
        this.targetPosition = ((Ray) ref ray).GetPoint(num);
        Vector3 position = ((Component) this).transform.position;
        ((Component) this).transform.rotation = Quaternion.LookRotation(Vector3.op_Subtraction(point, position));
      }
    }
    ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.targetPosition, Time.deltaTime * (float) this.smooth);
  }
}
