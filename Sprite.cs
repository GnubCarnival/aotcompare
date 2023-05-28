// Decompiled with JetBrains decompiler
// Type: Sprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class Sprite
{
  public Color Color;
  protected bool ColorChanged;
  protected float ElapsedTime;
  protected float Fps;
  protected Matrix4x4 LastMat;
  private Matrix4x4 LocalMat;
  protected Vector2 LowerLeftUV;
  public Camera MainCamera;
  public STransform MyTransform;
  private ORIPOINT OriPoint;
  protected Vector3 RotateAxis;
  private Quaternion Rotation;
  private Vector3 ScaleVector;
  private STYPE Type;
  protected bool UVChanged;
  protected Vector2 UVDimensions;
  private int UVStretch;
  public Vector3 v1 = Vector3.zero;
  public Vector3 v2 = Vector3.zero;
  public Vector3 v3 = Vector3.zero;
  public Vector3 v4 = Vector3.zero;
  protected VertexPool.VertexSegment Vertexsegment;
  private Matrix4x4 WorldMat;

  public Sprite(
    VertexPool.VertexSegment segment,
    float width,
    float height,
    STYPE type,
    ORIPOINT oripoint,
    Camera cam,
    int uvStretch,
    float maxFps)
  {
    this.UVChanged = this.ColorChanged = false;
    this.MyTransform.position = Vector3.zero;
    this.MyTransform.rotation = Quaternion.identity;
    this.LocalMat = this.WorldMat = Matrix4x4.identity;
    this.Vertexsegment = segment;
    this.UVStretch = uvStretch;
    this.LastMat = Matrix4x4.identity;
    this.ElapsedTime = 0.0f;
    this.Fps = 1f / maxFps;
    this.OriPoint = oripoint;
    this.RotateAxis = Vector3.zero;
    this.SetSizeXZ(width, height);
    this.RotateAxis.y = 1f;
    this.Type = type;
    this.MainCamera = cam;
    this.ResetSegment();
  }

  public void Init(Color color, Vector2 lowerLeftUV, Vector2 uvDimensions)
  {
    this.SetUVCoord(lowerLeftUV, uvDimensions);
    this.SetColor(color);
    this.SetRotation(Quaternion.identity);
    this.SetScale(1f, 1f);
    this.SetRotation(0.0f);
  }

  public void Reset()
  {
    this.MyTransform.Reset();
    this.SetColor(Color.white);
    this.SetUVCoord(Vector2.zero, Vector2.zero);
    this.ScaleVector = Vector3.one;
    this.Rotation = Quaternion.identity;
    VertexPool pool = this.Vertexsegment.Pool;
    int vertStart = this.Vertexsegment.VertStart;
    pool.Vertices[vertStart] = Vector3.zero;
    pool.Vertices[vertStart + 1] = Vector3.zero;
    pool.Vertices[vertStart + 2] = Vector3.zero;
    pool.Vertices[vertStart + 3] = Vector3.zero;
  }

  public void ResetSegment()
  {
    VertexPool pool = this.Vertexsegment.Pool;
    int indexStart = this.Vertexsegment.IndexStart;
    int vertStart = this.Vertexsegment.VertStart;
    pool.Indices[indexStart] = vertStart;
    pool.Indices[indexStart + 1] = vertStart + 3;
    pool.Indices[indexStart + 2] = vertStart + 1;
    pool.Indices[indexStart + 3] = vertStart + 3;
    pool.Indices[indexStart + 4] = vertStart + 2;
    pool.Indices[indexStart + 5] = vertStart + 1;
    pool.Vertices[vertStart] = Vector3.zero;
    pool.Vertices[vertStart + 1] = Vector3.zero;
    pool.Vertices[vertStart + 2] = Vector3.zero;
    pool.Vertices[vertStart + 3] = Vector3.zero;
    pool.Colors[vertStart] = Color.white;
    pool.Colors[vertStart + 1] = Color.white;
    pool.Colors[vertStart + 2] = Color.white;
    pool.Colors[vertStart + 3] = Color.white;
    pool.UVs[vertStart] = Vector2.zero;
    pool.UVs[vertStart + 1] = Vector2.zero;
    pool.UVs[vertStart + 2] = Vector2.zero;
    pool.UVs[vertStart + 3] = Vector2.zero;
    int num1;
    bool flag1 = (num1 = 1) != 0;
    pool.VertChanged = num1 != 0;
    int num2;
    bool flag2 = (num2 = flag1 ? 1 : 0) != 0;
    pool.ColorChanged = num2 != 0;
    int num3;
    bool flag3 = (num3 = flag2 ? 1 : 0) != 0;
    pool.IndiceChanged = num3 != 0;
    pool.UVChanged = flag3;
  }

  public void SetColor(Color c)
  {
    this.Color = c;
    this.ColorChanged = true;
  }

  public void SetPosition(Vector3 pos) => this.MyTransform.position = pos;

  public void SetRotation(float angle) => this.Rotation = Quaternion.AngleAxis(angle, this.RotateAxis);

  public void SetRotation(Quaternion q) => this.MyTransform.rotation = q;

  public void SetRotationFaceTo(Vector3 dir) => this.MyTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

  public void SetRotationTo(Vector3 dir)
  {
    if (!Vector3.op_Inequality(dir, Vector3.zero))
      return;
    Quaternion quaternion = Quaternion.identity;
    Vector3 vector3 = dir;
    vector3.y = 0.0f;
    if (Vector3.op_Equality(vector3, Vector3.zero))
      vector3 = Vector3.up;
    if (this.OriPoint == ORIPOINT.CENTER)
    {
      Quaternion rotation = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 1f), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.LEFT_UP)
    {
      Quaternion rotation = Quaternion.FromToRotation(((Matrix4x4) ref this.LocalMat).MultiplyPoint3x4(this.v3), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
    {
      Quaternion rotation = Quaternion.FromToRotation(((Matrix4x4) ref this.LocalMat).MultiplyPoint3x4(this.v4), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
    {
      Quaternion rotation = Quaternion.FromToRotation(((Matrix4x4) ref this.LocalMat).MultiplyPoint3x4(this.v1), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.RIGHT_UP)
    {
      Quaternion rotation = Quaternion.FromToRotation(((Matrix4x4) ref this.LocalMat).MultiplyPoint3x4(this.v2), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
    {
      Quaternion rotation = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 1f), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.TOP_CENTER)
    {
      Quaternion rotation = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1f), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
    {
      Quaternion rotation = Quaternion.FromToRotation(new Vector3(-1f, 0.0f, 0.0f), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
    {
      Quaternion rotation = Quaternion.FromToRotation(new Vector3(1f, 0.0f, 0.0f), vector3);
      quaternion = Quaternion.op_Multiply(Quaternion.FromToRotation(vector3, dir), rotation);
    }
    this.MyTransform.rotation = quaternion;
  }

  public void SetScale(float width, float height)
  {
    this.ScaleVector.x = width;
    this.ScaleVector.z = height;
  }

  public void SetSizeXZ(float width, float height)
  {
    this.v1 = new Vector3((float) (-(double) width / 2.0), 0.0f, height / 2f);
    this.v2 = new Vector3((float) (-(double) width / 2.0), 0.0f, (float) (-(double) height / 2.0));
    this.v3 = new Vector3(width / 2f, 0.0f, (float) (-(double) height / 2.0));
    this.v4 = new Vector3(width / 2f, 0.0f, height / 2f);
    Vector3 vector3 = Vector3.zero;
    if (this.OriPoint == ORIPOINT.LEFT_UP)
      vector3 = this.v3;
    else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
      vector3 = this.v4;
    else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
      vector3 = this.v1;
    else if (this.OriPoint == ORIPOINT.RIGHT_UP)
      vector3 = this.v2;
    else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
    {
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, height / 2f);
    }
    else if (this.OriPoint == ORIPOINT.TOP_CENTER)
    {
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, (float) (-(double) height / 2.0));
    }
    else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
    {
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(width / 2f, 0.0f, 0.0f);
    }
    else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
    {
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector((float) (-(double) width / 2.0), 0.0f, 0.0f);
    }
    this.v1 = Vector3.op_Addition(this.v1, vector3);
    this.v2 = Vector3.op_Addition(this.v2, vector3);
    this.v3 = Vector3.op_Addition(this.v3, vector3);
    this.v4 = Vector3.op_Addition(this.v4, vector3);
  }

  public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
  {
    this.LowerLeftUV = lowerleft;
    this.UVDimensions = dimensions;
    this.UVChanged = true;
  }

  public void Transform()
  {
    ((Matrix4x4) ref this.LocalMat).SetTRS(Vector3.zero, this.Rotation, this.ScaleVector);
    if (this.Type == STYPE.BILLBOARD)
    {
      UnityEngine.Transform transform = ((Component) this.MainCamera).transform;
      this.MyTransform.LookAt(Vector3.op_Addition(this.MyTransform.position, Quaternion.op_Multiply(transform.rotation, Vector3.up)), Quaternion.op_Multiply(transform.rotation, Vector3.back));
    }
    ((Matrix4x4) ref this.WorldMat).SetTRS(this.MyTransform.position, this.MyTransform.rotation, Vector3.one);
    Matrix4x4 matrix4x4 = Matrix4x4.op_Multiply(this.WorldMat, this.LocalMat);
    VertexPool pool = this.Vertexsegment.Pool;
    int vertStart = this.Vertexsegment.VertStart;
    Vector3 vector3_1 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(this.v1);
    Vector3 vector3_2 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(this.v2);
    Vector3 vector3_3 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(this.v3);
    Vector3 vector3_4 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(this.v4);
    if (this.Type == STYPE.BILLBOARD_SELF)
    {
      Vector3 zero1 = Vector3.zero;
      Vector3 zero2 = Vector3.zero;
      Vector3 vector3_5;
      Vector3 vector3_6;
      float magnitude;
      if (this.UVStretch == 0)
      {
        vector3_5 = Vector3.op_Division(Vector3.op_Addition(vector3_1, vector3_4), 2f);
        vector3_6 = Vector3.op_Division(Vector3.op_Addition(vector3_2, vector3_3), 2f);
        Vector3 vector3_7 = Vector3.op_Subtraction(vector3_4, vector3_1);
        magnitude = ((Vector3) ref vector3_7).magnitude;
      }
      else
      {
        vector3_5 = Vector3.op_Division(Vector3.op_Addition(vector3_1, vector3_2), 2f);
        vector3_6 = Vector3.op_Division(Vector3.op_Addition(vector3_4, vector3_3), 2f);
        Vector3 vector3_8 = Vector3.op_Subtraction(vector3_2, vector3_1);
        magnitude = ((Vector3) ref vector3_8).magnitude;
      }
      Vector3 vector3_9 = Vector3.op_Subtraction(vector3_5, vector3_6);
      Vector3 vector3_10 = Vector3.Cross(vector3_9, Vector3.op_Subtraction(((Component) this.MainCamera).transform.position, vector3_5));
      ((Vector3) ref vector3_10).Normalize();
      vector3_10 = Vector3.op_Multiply(vector3_10, magnitude * 0.5f);
      Vector3 vector3_11 = Vector3.Cross(vector3_9, Vector3.op_Subtraction(((Component) this.MainCamera).transform.position, vector3_6));
      ((Vector3) ref vector3_11).Normalize();
      vector3_11 = Vector3.op_Multiply(vector3_11, magnitude * 0.5f);
      if (this.UVStretch == 0)
      {
        vector3_1 = Vector3.op_Subtraction(vector3_5, vector3_10);
        vector3_4 = Vector3.op_Addition(vector3_5, vector3_10);
        vector3_2 = Vector3.op_Subtraction(vector3_6, vector3_11);
        vector3_3 = Vector3.op_Addition(vector3_6, vector3_11);
      }
      else
      {
        vector3_1 = Vector3.op_Subtraction(vector3_5, vector3_10);
        vector3_2 = Vector3.op_Addition(vector3_5, vector3_10);
        vector3_4 = Vector3.op_Subtraction(vector3_6, vector3_11);
        vector3_3 = Vector3.op_Addition(vector3_6, vector3_11);
      }
    }
    pool.Vertices[vertStart] = vector3_1;
    pool.Vertices[vertStart + 1] = vector3_2;
    pool.Vertices[vertStart + 2] = vector3_3;
    pool.Vertices[vertStart + 3] = vector3_4;
  }

  public void Update(bool force)
  {
    this.ElapsedTime += Time.deltaTime;
    if (!((double) this.ElapsedTime > (double) this.Fps | force))
      return;
    this.Transform();
    if (this.UVChanged)
      this.UpdateUV();
    if (this.ColorChanged)
      this.UpdateColor();
    this.UVChanged = this.ColorChanged = false;
    if (force)
      return;
    this.ElapsedTime -= this.Fps;
  }

  public void UpdateColor()
  {
    VertexPool pool = this.Vertexsegment.Pool;
    int vertStart = this.Vertexsegment.VertStart;
    pool.Colors[vertStart] = this.Color;
    pool.Colors[vertStart + 1] = this.Color;
    pool.Colors[vertStart + 2] = this.Color;
    pool.Colors[vertStart + 3] = this.Color;
    this.Vertexsegment.Pool.ColorChanged = true;
  }

  public void UpdateUV()
  {
    VertexPool pool = this.Vertexsegment.Pool;
    int vertStart = this.Vertexsegment.VertStart;
    if ((double) this.UVDimensions.y > 0.0)
    {
      pool.UVs[vertStart] = Vector2.op_Addition(this.LowerLeftUV, Vector2.op_Multiply(Vector2.up, this.UVDimensions.y));
      pool.UVs[vertStart + 1] = this.LowerLeftUV;
      pool.UVs[vertStart + 2] = Vector2.op_Addition(this.LowerLeftUV, Vector2.op_Multiply(Vector2.right, this.UVDimensions.x));
      pool.UVs[vertStart + 3] = Vector2.op_Addition(this.LowerLeftUV, this.UVDimensions);
    }
    else
    {
      pool.UVs[vertStart] = this.LowerLeftUV;
      pool.UVs[vertStart + 1] = Vector2.op_Addition(this.LowerLeftUV, Vector2.op_Multiply(Vector2.up, this.UVDimensions.y));
      pool.UVs[vertStart + 2] = Vector2.op_Addition(this.LowerLeftUV, this.UVDimensions);
      pool.UVs[vertStart + 3] = Vector2.op_Addition(this.LowerLeftUV, Vector2.op_Multiply(Vector2.right, this.UVDimensions.x));
    }
    this.Vertexsegment.Pool.UVChanged = true;
  }
}
