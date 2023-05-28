// Decompiled with JetBrains decompiler
// Type: UIGeometry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class UIGeometry
{
  public BetterList<Color32> cols = new BetterList<Color32>();
  private Vector3 mRtpNormal;
  private Vector4 mRtpTan;
  private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();
  public BetterList<Vector2> uvs = new BetterList<Vector2>();
  public BetterList<Vector3> verts = new BetterList<Vector3>();

  public void ApplyOffset(Vector3 pivotOffset)
  {
    for (int index = 0; index < this.verts.size; ++index)
    {
      ref Vector3 local = ref this.verts.buffer[index];
      local = Vector3.op_Addition(local, pivotOffset);
    }
  }

  public void ApplyTransform(Matrix4x4 widgetToPanel, bool normals)
  {
    if (this.verts.size > 0)
    {
      this.mRtpVerts.Clear();
      int i = 0;
      for (int size = this.verts.size; i < size; ++i)
        this.mRtpVerts.Add(((Matrix4x4) ref widgetToPanel).MultiplyPoint3x4(this.verts[i]));
      Vector3 vector3_1 = ((Matrix4x4) ref widgetToPanel).MultiplyVector(Vector3.back);
      this.mRtpNormal = ((Vector3) ref vector3_1).normalized;
      Vector3 vector3_2 = ((Matrix4x4) ref widgetToPanel).MultiplyVector(Vector3.right);
      Vector3 normalized = ((Vector3) ref vector3_2).normalized;
      this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
    }
    else
      this.mRtpVerts.Clear();
  }

  public void Clear()
  {
    this.verts.Clear();
    this.uvs.Clear();
    this.cols.Clear();
    this.mRtpVerts.Clear();
  }

  public void WriteToBuffers(
    BetterList<Vector3> v,
    BetterList<Vector2> u,
    BetterList<Color32> c,
    BetterList<Vector3> n,
    BetterList<Vector4> t)
  {
    if (this.mRtpVerts == null || this.mRtpVerts.size <= 0)
      return;
    if (n == null)
    {
      for (int index = 0; index < this.mRtpVerts.size; ++index)
      {
        v.Add(this.mRtpVerts.buffer[index]);
        u.Add(this.uvs.buffer[index]);
        c.Add(this.cols.buffer[index]);
      }
    }
    else
    {
      for (int index = 0; index < this.mRtpVerts.size; ++index)
      {
        v.Add(this.mRtpVerts.buffer[index]);
        u.Add(this.uvs.buffer[index]);
        c.Add(this.cols.buffer[index]);
        n.Add(this.mRtpNormal);
        t.Add(this.mRtpTan);
      }
    }
  }

  public bool hasTransformed => this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;

  public bool hasVertices => this.verts.size > 0;
}
