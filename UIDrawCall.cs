// Decompiled with JetBrains decompiler
// Type: UIDrawCall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Internal/Draw Call")]
[ExecuteInEditMode]
public class UIDrawCall : MonoBehaviour
{
  private Material mClippedMat;
  private UIDrawCall.Clipping mClipping;
  private Vector4 mClipRange;
  private Vector2 mClipSoft;
  private Material mDepthMat;
  private bool mDepthPass;
  private bool mEven = true;
  private MeshFilter mFilter;
  private int[] mIndices;
  private Mesh mMesh0;
  private Mesh mMesh1;
  private MeshRenderer mRen;
  private bool mReset = true;
  private Material mSharedMat;
  private Transform mTrans;

  private Mesh GetMesh(ref bool rebuildIndices, int vertexCount)
  {
    this.mEven = !this.mEven;
    if (this.mEven)
    {
      if (Object.op_Equality((Object) this.mMesh0, (Object) null))
      {
        this.mMesh0 = new Mesh();
        ((Object) this.mMesh0).hideFlags = (HideFlags) 4;
        ((Object) this.mMesh0).name = "Mesh0 for " + ((Object) this.mSharedMat).name;
        this.mMesh0.MarkDynamic();
        rebuildIndices = true;
      }
      else if (rebuildIndices || this.mMesh0.vertexCount != vertexCount)
      {
        rebuildIndices = true;
        this.mMesh0.Clear();
      }
      return this.mMesh0;
    }
    if (Object.op_Equality((Object) this.mMesh1, (Object) null))
    {
      this.mMesh1 = new Mesh();
      ((Object) this.mMesh1).hideFlags = (HideFlags) 4;
      ((Object) this.mMesh1).name = "Mesh1 for " + ((Object) this.mSharedMat).name;
      this.mMesh1.MarkDynamic();
      rebuildIndices = true;
    }
    else if (rebuildIndices || this.mMesh1.vertexCount != vertexCount)
    {
      rebuildIndices = true;
      this.mMesh1.Clear();
    }
    return this.mMesh1;
  }

  private void OnDestroy()
  {
    NGUITools.DestroyImmediate((Object) this.mMesh0);
    NGUITools.DestroyImmediate((Object) this.mMesh1);
    NGUITools.DestroyImmediate((Object) this.mClippedMat);
    NGUITools.DestroyImmediate((Object) this.mDepthMat);
  }

  private void OnWillRenderObject()
  {
    if (this.mReset)
    {
      this.mReset = false;
      this.UpdateMaterials();
    }
    if (!Object.op_Inequality((Object) this.mClippedMat, (Object) null))
      return;
    this.mClippedMat.mainTextureOffset = new Vector2(-this.mClipRange.x / this.mClipRange.z, -this.mClipRange.y / this.mClipRange.w);
    this.mClippedMat.mainTextureScale = new Vector2(1f / this.mClipRange.z, 1f / this.mClipRange.w);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(1000f, 1000f);
    if ((double) this.mClipSoft.x > 0.0)
      vector2.x = this.mClipRange.z / this.mClipSoft.x;
    if ((double) this.mClipSoft.y > 0.0)
      vector2.y = this.mClipRange.w / this.mClipSoft.y;
    this.mClippedMat.SetVector("_ClipSharpness", Vector4.op_Implicit(vector2));
  }

  public void Set(
    BetterList<Vector3> verts,
    BetterList<Vector3> norms,
    BetterList<Vector4> tans,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    int size = verts.size;
    if (size > 0 && size == uvs.size && size == cols.size && size % 4 == 0)
    {
      if (Object.op_Equality((Object) this.mFilter, (Object) null))
        this.mFilter = ((Component) this).gameObject.GetComponent<MeshFilter>();
      if (Object.op_Equality((Object) this.mFilter, (Object) null))
        this.mFilter = ((Component) this).gameObject.AddComponent<MeshFilter>();
      if (Object.op_Equality((Object) this.mRen, (Object) null))
        this.mRen = ((Component) this).gameObject.GetComponent<MeshRenderer>();
      if (Object.op_Equality((Object) this.mRen, (Object) null))
      {
        this.mRen = ((Component) this).gameObject.AddComponent<MeshRenderer>();
        this.UpdateMaterials();
      }
      else if (Object.op_Inequality((Object) this.mClippedMat, (Object) null) && Object.op_Inequality((Object) this.mClippedMat.mainTexture, (Object) this.mSharedMat.mainTexture))
        this.UpdateMaterials();
      if (verts.size < 65000)
      {
        int length = (size >> 1) * 3;
        bool rebuildIndices = this.mIndices == null || this.mIndices.Length != length;
        if (rebuildIndices)
        {
          this.mIndices = new int[length];
          int num1 = 0;
          for (int index1 = 0; index1 < size; index1 += 4)
          {
            int[] mIndices1 = this.mIndices;
            int index2 = num1;
            int num2 = index2 + 1;
            int num3 = index1;
            mIndices1[index2] = num3;
            int[] mIndices2 = this.mIndices;
            int index3 = num2;
            int num4 = index3 + 1;
            int num5 = index1 + 1;
            mIndices2[index3] = num5;
            int[] mIndices3 = this.mIndices;
            int index4 = num4;
            int num6 = index4 + 1;
            int num7 = index1 + 2;
            mIndices3[index4] = num7;
            int[] mIndices4 = this.mIndices;
            int index5 = num6;
            int num8 = index5 + 1;
            int num9 = index1 + 2;
            mIndices4[index5] = num9;
            int[] mIndices5 = this.mIndices;
            int index6 = num8;
            int num10 = index6 + 1;
            int num11 = index1 + 3;
            mIndices5[index6] = num11;
            int[] mIndices6 = this.mIndices;
            int index7 = num10;
            num1 = index7 + 1;
            int num12 = index1;
            mIndices6[index7] = num12;
          }
        }
        Mesh mesh = this.GetMesh(ref rebuildIndices, verts.size);
        mesh.vertices = verts.ToArray();
        if (norms != null)
          mesh.normals = norms.ToArray();
        if (tans != null)
          mesh.tangents = tans.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.colors32 = cols.ToArray();
        if (rebuildIndices)
          mesh.triangles = this.mIndices;
        mesh.RecalculateBounds();
        this.mFilter.mesh = mesh;
      }
      else
      {
        if (Object.op_Inequality((Object) this.mFilter.mesh, (Object) null))
          this.mFilter.mesh.Clear();
        Debug.LogError((object) ("Too many vertices on one panel: " + verts.size.ToString()));
      }
    }
    else
    {
      if (Object.op_Inequality((Object) this.mFilter.mesh, (Object) null))
        this.mFilter.mesh.Clear();
      Debug.LogError((object) ("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size.ToString()));
    }
  }

  private void UpdateMaterials()
  {
    if (this.mClipping != UIDrawCall.Clipping.None)
    {
      Shader shader = (Shader) null;
      if (this.mClipping != UIDrawCall.Clipping.None)
      {
        string str = ((Object) this.mSharedMat.shader).name.Replace(" (AlphaClip)", string.Empty).Replace(" (SoftClip)", string.Empty);
        if (this.mClipping == UIDrawCall.Clipping.HardClip || this.mClipping == UIDrawCall.Clipping.AlphaClip)
          shader = Shader.Find(str + " (AlphaClip)");
        else if (this.mClipping == UIDrawCall.Clipping.SoftClip)
          shader = Shader.Find(str + " (SoftClip)");
        if (Object.op_Equality((Object) shader, (Object) null))
          this.mClipping = UIDrawCall.Clipping.None;
      }
      if (Object.op_Inequality((Object) shader, (Object) null))
      {
        if (Object.op_Equality((Object) this.mClippedMat, (Object) null))
        {
          this.mClippedMat = new Material(this.mSharedMat);
          ((Object) this.mClippedMat).hideFlags = (HideFlags) 4;
        }
        this.mClippedMat.shader = shader;
        this.mClippedMat.CopyPropertiesFromMaterial(this.mSharedMat);
      }
      else if (Object.op_Inequality((Object) this.mClippedMat, (Object) null))
      {
        NGUITools.Destroy((Object) this.mClippedMat);
        this.mClippedMat = (Material) null;
      }
    }
    else if (Object.op_Inequality((Object) this.mClippedMat, (Object) null))
    {
      NGUITools.Destroy((Object) this.mClippedMat);
      this.mClippedMat = (Material) null;
    }
    if (this.mDepthPass)
    {
      if (Object.op_Equality((Object) this.mDepthMat, (Object) null))
      {
        this.mDepthMat = new Material(Shader.Find("Unlit/Depth Cutout"));
        ((Object) this.mDepthMat).hideFlags = (HideFlags) 4;
      }
      this.mDepthMat.mainTexture = this.mSharedMat.mainTexture;
    }
    else if (Object.op_Inequality((Object) this.mDepthMat, (Object) null))
    {
      NGUITools.Destroy((Object) this.mDepthMat);
      this.mDepthMat = (Material) null;
    }
    Material material = Object.op_Equality((Object) this.mClippedMat, (Object) null) ? this.mSharedMat : this.mClippedMat;
    if (Object.op_Inequality((Object) this.mDepthMat, (Object) null))
    {
      if (((Renderer) this.mRen).sharedMaterials != null && ((Renderer) this.mRen).sharedMaterials.Length == 2 && !Object.op_Inequality((Object) ((Renderer) this.mRen).sharedMaterials[1], (Object) material))
        return;
      ((Renderer) this.mRen).sharedMaterials = new Material[2]
      {
        this.mDepthMat,
        material
      };
    }
    else
    {
      if (!Object.op_Inequality((Object) ((Renderer) this.mRen).sharedMaterial, (Object) material))
        return;
      ((Renderer) this.mRen).sharedMaterials = new Material[1]
      {
        material
      };
    }
  }

  public Transform cachedTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.mTrans, (Object) null))
        this.mTrans = ((Component) this).transform;
      return this.mTrans;
    }
  }

  public UIDrawCall.Clipping clipping
  {
    get => this.mClipping;
    set
    {
      if (this.mClipping == value)
        return;
      this.mClipping = value;
      this.mReset = true;
    }
  }

  public Vector4 clipRange
  {
    get => this.mClipRange;
    set => this.mClipRange = value;
  }

  public Vector2 clipSoftness
  {
    get => this.mClipSoft;
    set => this.mClipSoft = value;
  }

  public bool depthPass
  {
    get => this.mDepthPass;
    set
    {
      if (this.mDepthPass == value)
        return;
      this.mDepthPass = value;
      this.mReset = true;
    }
  }

  public bool isClipped => Object.op_Inequality((Object) this.mClippedMat, (Object) null);

  public Material material
  {
    get => this.mSharedMat;
    set => this.mSharedMat = value;
  }

  public int triangles
  {
    get
    {
      Mesh mesh = !this.mEven ? this.mMesh1 : this.mMesh0;
      return !Object.op_Equality((Object) mesh, (Object) null) ? mesh.vertexCount >> 1 : 0;
    }
  }

  public enum Clipping
  {
    None,
    HardClip,
    AlphaClip,
    SoftClip,
  }
}
