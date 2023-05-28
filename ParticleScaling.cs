// Decompiled with JetBrains decompiler
// Type: ParticleScaling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class ParticleScaling : MonoBehaviour
{
  public void OnWillRenderObject()
  {
    ((Component) ((Component) this).GetComponent<ParticleSystem>()).renderer.material.SetVector("_Center", Vector4.op_Implicit(((Component) this).transform.position));
    ((Component) ((Component) this).GetComponent<ParticleSystem>()).renderer.material.SetVector("_Scaling", Vector4.op_Implicit(((Component) this).transform.lossyScale));
    ((Component) ((Component) this).GetComponent<ParticleSystem>()).renderer.material.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
    Material material = ((Component) ((Component) this).GetComponent<ParticleSystem>()).renderer.material;
    Matrix4x4 worldToCameraMatrix = Camera.current.worldToCameraMatrix;
    Matrix4x4 inverse = ((Matrix4x4) ref worldToCameraMatrix).inverse;
    material.SetMatrix("_CameraInv", inverse);
  }
}
