// Decompiled with JetBrains decompiler
// Type: SpectatorMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
  public bool disable;
  private float speed = 100f;

  private void Start()
  {
  }

  private void Update()
  {
    if (this.disable)
      return;
    float speed = this.speed;
    if (SettingsManager.InputSettings.Human.Jump.GetKey())
      speed *= 3f;
    float num1 = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0.0f : -1f) : 1f;
    float num2 = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0.0f : 1f) : -1f;
    Transform transform1 = ((Component) this).transform;
    if ((double) num1 > 0.0)
    {
      Transform transform2 = transform1;
      transform2.position = Vector3.op_Addition(transform2.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.forward, speed), Time.deltaTime));
    }
    else if ((double) num1 < 0.0)
    {
      Transform transform3 = transform1;
      transform3.position = Vector3.op_Subtraction(transform3.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.forward, speed), Time.deltaTime));
    }
    if ((double) num2 > 0.0)
    {
      Transform transform4 = transform1;
      transform4.position = Vector3.op_Addition(transform4.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.right, speed), Time.deltaTime));
    }
    else if ((double) num2 < 0.0)
    {
      Transform transform5 = transform1;
      transform5.position = Vector3.op_Subtraction(transform5.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.right, speed), Time.deltaTime));
    }
    if (SettingsManager.InputSettings.Human.HookLeft.GetKey())
    {
      Transform transform6 = transform1;
      transform6.position = Vector3.op_Subtraction(transform6.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.up, speed), Time.deltaTime));
    }
    else
    {
      if (!SettingsManager.InputSettings.Human.HookRight.GetKey())
        return;
      Transform transform7 = transform1;
      transform7.position = Vector3.op_Addition(transform7.position, Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.up, speed), Time.deltaTime));
    }
  }
}
