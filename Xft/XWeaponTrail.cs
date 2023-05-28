// Decompiled with JetBrains decompiler
// Type: Xft.XWeaponTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
  public class XWeaponTrail : MonoBehaviour
  {
    public float Fps = 60f;
    public int Granularity = 60;
    public int MaxFrame = 14;
    protected float mElapsedTime;
    protected float mFadeElapsedime;
    protected float mFadeT = 1f;
    protected float mFadeTime = 1f;
    protected XWeaponTrail.Element mHeadElem = new XWeaponTrail.Element();
    protected bool mInited;
    protected bool mIsFading;
    protected GameObject mMeshObj;
    protected List<XWeaponTrail.Element> mSnapshotList = new List<XWeaponTrail.Element>();
    protected Spline mSpline = new Spline();
    protected float mTrailWidth;
    protected VertexPool mVertexPool;
    protected VertexPool.VertexSegment mVertexSegment;
    public Color MyColor = Color.white;
    public Material MyMaterial;
    public Transform PointEnd;
    public Transform PointStart;
    public static string Version = "1.0.1";

    public void Activate()
    {
      this.MaxFrame = 14;
      this.Init();
      if (Object.op_Equality((Object) this.mMeshObj, (Object) null))
      {
        this.InitMeshObj();
      }
      else
      {
        ((Component) this).gameObject.SetActive(true);
        if (Object.op_Inequality((Object) this.mMeshObj, (Object) null))
          this.mMeshObj.SetActive(true);
        this.mFadeT = 1f;
        this.mIsFading = false;
        this.mFadeTime = 1f;
        this.mFadeElapsedime = 0.0f;
        this.mElapsedTime = 0.0f;
        for (int index = 0; index < this.mSnapshotList.Count; ++index)
        {
          this.mSnapshotList[index].PointStart = this.PointStart.position;
          this.mSnapshotList[index].PointEnd = this.PointEnd.position;
          this.mSpline.ControlPoints[index].Position = this.mSnapshotList[index].Pos;
          this.mSpline.ControlPoints[index].Normal = Vector3.op_Subtraction(this.mSnapshotList[index].PointEnd, this.mSnapshotList[index].PointStart);
        }
        this.RefreshSpline();
        this.UpdateVertex();
      }
    }

    public void Deactivate()
    {
      ((Component) this).gameObject.SetActive(false);
      if (!Object.op_Inequality((Object) this.mMeshObj, (Object) null))
        return;
      this.mMeshObj.SetActive(false);
    }

    public void Init()
    {
      if (this.mInited)
        return;
      Vector3 vector3 = Vector3.op_Subtraction(this.PointStart.position, this.PointEnd.position);
      this.mTrailWidth = ((Vector3) ref vector3).magnitude;
      this.InitMeshObj();
      this.InitOriginalElements();
      this.InitSpline();
      this.mInited = true;
    }

    private void InitMeshObj()
    {
      this.mMeshObj = new GameObject("_XWeaponTrailMesh: " + ((Object) ((Component) this).gameObject).name);
      this.mMeshObj.layer = ((Component) this).gameObject.layer;
      this.mMeshObj.SetActive(true);
      MeshFilter meshFilter = this.mMeshObj.AddComponent<MeshFilter>();
      MeshRenderer meshRenderer = this.mMeshObj.AddComponent<MeshRenderer>();
      ((Renderer) meshRenderer).castShadows = false;
      ((Renderer) meshRenderer).receiveShadows = false;
      ((Component) meshRenderer).renderer.sharedMaterial = this.MyMaterial;
      meshFilter.sharedMesh = new Mesh();
      this.mVertexPool = new VertexPool(meshFilter.sharedMesh, this.MyMaterial);
      this.mVertexSegment = this.mVertexPool.GetVertices(this.Granularity * 3, (this.Granularity - 1) * 12);
      this.UpdateIndices();
    }

    private void InitOriginalElements()
    {
      this.mSnapshotList.Clear();
      this.mSnapshotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
      this.mSnapshotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
    }

    private void InitSpline()
    {
      this.mSpline.Granularity = this.Granularity;
      this.mSpline.Clear();
      for (int index = 0; index < this.MaxFrame; ++index)
        this.mSpline.AddControlPoint(this.CurHeadPos, Vector3.op_Subtraction(this.PointStart.position, this.PointEnd.position));
    }

    public void lateUpdate()
    {
      if (!this.mInited)
        return;
      this.mVertexPool.LateUpdate();
    }

    private void OnDrawGizmos()
    {
      if (!Object.op_Inequality((Object) this.PointEnd, (Object) null) || !Object.op_Inequality((Object) this.PointStart, (Object) null))
        return;
      Vector3 vector3 = Vector3.op_Subtraction(this.PointStart.position, this.PointEnd.position);
      float magnitude = ((Vector3) ref vector3).magnitude;
      if ((double) magnitude < 1.4012984643248171E-45)
        return;
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(this.PointStart.position, magnitude * 0.04f);
      Gizmos.color = Color.blue;
      Gizmos.DrawSphere(this.PointEnd.position, magnitude * 0.04f);
    }

    private void RecordCurElem()
    {
      XWeaponTrail.Element element = new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position);
      if (this.mSnapshotList.Count < this.MaxFrame)
      {
        this.mSnapshotList.Insert(1, element);
      }
      else
      {
        this.mSnapshotList.RemoveAt(this.mSnapshotList.Count - 1);
        this.mSnapshotList.Insert(1, element);
      }
    }

    private void RefreshSpline()
    {
      for (int index = 0; index < this.mSnapshotList.Count; ++index)
      {
        this.mSpline.ControlPoints[index].Position = this.mSnapshotList[index].Pos;
        this.mSpline.ControlPoints[index].Normal = Vector3.op_Subtraction(this.mSnapshotList[index].PointEnd, this.mSnapshotList[index].PointStart);
      }
      this.mSpline.RefreshSpline();
    }

    private void Start() => this.Init();

    public void StopSmoothly(float fadeTime)
    {
      this.mIsFading = true;
      this.mFadeTime = fadeTime;
    }

    public void update()
    {
      if (!this.mInited)
        return;
      if (Object.op_Equality((Object) this.mMeshObj, (Object) null))
      {
        this.InitMeshObj();
      }
      else
      {
        this.UpdateHeadElem();
        this.mElapsedTime += Time.deltaTime;
        if ((double) this.mElapsedTime < (double) this.UpdateInterval)
          return;
        this.mElapsedTime -= this.UpdateInterval;
        this.RecordCurElem();
        this.RefreshSpline();
        this.UpdateFade();
        this.UpdateVertex();
      }
    }

    private void UpdateFade()
    {
      if (!this.mIsFading)
        return;
      this.mFadeElapsedime += Time.deltaTime;
      this.mFadeT = 1f - this.mFadeElapsedime / this.mFadeTime;
      if ((double) this.mFadeT >= 0.0)
        return;
      this.Deactivate();
    }

    private void UpdateHeadElem()
    {
      this.mSnapshotList[0].PointStart = this.PointStart.position;
      this.mSnapshotList[0].PointEnd = this.PointEnd.position;
    }

    private void UpdateIndices()
    {
      VertexPool pool = this.mVertexSegment.Pool;
      for (int index1 = 0; index1 < this.Granularity - 1; ++index1)
      {
        int num1 = this.mVertexSegment.VertStart + index1 * 3;
        int num2 = this.mVertexSegment.VertStart + (index1 + 1) * 3;
        int index2 = this.mVertexSegment.IndexStart + index1 * 12;
        pool.Indices[index2] = num2;
        pool.Indices[index2 + 1] = num2 + 1;
        pool.Indices[index2 + 2] = num1;
        pool.Indices[index2 + 3] = num2 + 1;
        pool.Indices[index2 + 4] = num1 + 1;
        pool.Indices[index2 + 5] = num1;
        pool.Indices[index2 + 6] = num2 + 1;
        pool.Indices[index2 + 7] = num2 + 2;
        pool.Indices[index2 + 8] = num1 + 1;
        pool.Indices[index2 + 9] = num2 + 2;
        pool.Indices[index2 + 10] = num1 + 2;
        pool.Indices[index2 + 11] = num1 + 1;
      }
      pool.IndiceChanged = true;
    }

    private void UpdateVertex()
    {
      VertexPool pool = this.mVertexSegment.Pool;
      for (int index1 = 0; index1 < this.Granularity; ++index1)
      {
        int index2 = this.mVertexSegment.VertStart + index1 * 3;
        float num = (float) index1 / (float) this.Granularity;
        float tl = num * this.mFadeT;
        Vector2 zero = Vector2.zero;
        Vector3 vector3_1 = this.mSpline.InterpolateByLen(tl);
        Vector3 vector3_2 = this.mSpline.InterpolateNormalByLen(tl);
        Vector3 vector3_3 = Vector3.op_Addition(vector3_1, Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, this.mTrailWidth), 0.5f));
        Vector3 vector3_4 = Vector3.op_Subtraction(vector3_1, Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, this.mTrailWidth), 0.5f));
        pool.Vertices[index2] = vector3_3;
        pool.Colors[index2] = this.MyColor;
        zero.x = 0.0f;
        zero.y = num;
        pool.UVs[index2] = zero;
        pool.Vertices[index2 + 1] = vector3_1;
        pool.Colors[index2 + 1] = this.MyColor;
        zero.x = 0.5f;
        zero.y = num;
        pool.UVs[index2 + 1] = zero;
        pool.Vertices[index2 + 2] = vector3_4;
        pool.Colors[index2 + 2] = this.MyColor;
        zero.x = 1f;
        zero.y = num;
        pool.UVs[index2 + 2] = zero;
      }
      this.mVertexSegment.Pool.UVChanged = true;
      this.mVertexSegment.Pool.VertChanged = true;
      this.mVertexSegment.Pool.ColorChanged = true;
    }

    public Vector3 CurHeadPos => Vector3.op_Division(Vector3.op_Addition(this.PointStart.position, this.PointEnd.position), 2f);

    public float TrailWidth => this.mTrailWidth;

    public float UpdateInterval => 1f / this.Fps;

    public class Element
    {
      public Vector3 PointEnd;
      public Vector3 PointStart;

      public Element()
      {
      }

      public Element(Vector3 start, Vector3 end)
      {
        this.PointStart = start;
        this.PointEnd = end;
      }

      public Vector3 Pos => Vector3.op_Division(Vector3.op_Addition(this.PointStart, this.PointEnd), 2f);
    }
  }
}
