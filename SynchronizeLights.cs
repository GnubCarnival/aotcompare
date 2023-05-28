// Decompiled with JetBrains decompiler
// Type: SynchronizeLights
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class SynchronizeLights : MonoBehaviour
{
  public Light light0;
  public Light light1;

  private void LateUpdate()
  {
    if (Object.op_Inequality((Object) this.light0, (Object) null))
    {
      Vector3 vector3 = Quaternion.op_Multiply(((Component) this.light0).transform.rotation, new Vector3(0.0f, 0.0f, -1f));
      ((Component) this).renderer.material.SetVector("_LightDirection0", new Vector4(vector3.x, vector3.y, vector3.z, 0.0f));
      ((Component) this).renderer.material.SetColor("_MyLightColor0", this.light0.color);
    }
    if (!Object.op_Inequality((Object) this.light1, (Object) null))
      return;
    Vector3 vector3_1 = Quaternion.op_Multiply(((Component) this.light1).transform.rotation, new Vector3(0.0f, 0.0f, -1f));
    ((Component) this).renderer.material.SetVector("_LightDirection1", new Vector4(vector3_1.x, vector3_1.y, vector3_1.z, 0.0f));
    ((Component) this).renderer.material.SetColor("_MyLightColor1", this.light1.color);
  }
}
