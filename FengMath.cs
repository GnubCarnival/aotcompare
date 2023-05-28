// Decompiled with JetBrains decompiler
// Type: FengMath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class FengMath
{
  public static float getHorizontalAngle(Vector3 from, Vector3 to)
  {
    Vector3 vector3 = Vector3.op_Subtraction(to, from);
    return (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
  }

  public static Quaternion getHorizontalRotation(Vector3 from, Vector3 to)
  {
    Vector3 vector3 = Vector3.op_Subtraction(from, to);
    return Quaternion.Euler(0.0f, (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766), 0.0f);
  }
}
