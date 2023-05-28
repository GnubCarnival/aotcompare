// Decompiled with JetBrains decompiler
// Type: RibbonTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RibbonTrail
{
  public const int CHAIN_EMPTY = 99999;
  protected Color Color = Color.white;
  protected float ElapsedTime;
  public int ElemCount;
  public RibbonTrail.Element[] ElementArray;
  protected float ElemLength;
  protected float Fps;
  public int Head;
  protected Vector3 HeadPosition;
  protected bool IndexDirty;
  protected Vector2 LowerLeftUV;
  public int MaxElements;
  public float SquaredElemLength;
  protected int StretchType;
  public int Tail;
  protected float TrailLength;
  protected float UnitWidth;
  protected Vector2 UVDimensions;
  protected VertexPool.VertexSegment Vertexsegment;

  public RibbonTrail(
    VertexPool.VertexSegment segment,
    float width,
    int maxelemnt,
    float len,
    Vector3 pos,
    int stretchType,
    float maxFps)
  {
    if (maxelemnt <= 2)
      Debug.LogError((object) "ribbon trail's maxelement should > 2!");
    this.MaxElements = maxelemnt;
    this.Vertexsegment = segment;
    this.ElementArray = new RibbonTrail.Element[this.MaxElements];
    this.Head = this.Tail = 99999;
    this.SetTrailLen(len);
    this.UnitWidth = width;
    this.HeadPosition = pos;
    this.StretchType = stretchType;
    RibbonTrail.Element dtls = new RibbonTrail.Element(this.HeadPosition, this.UnitWidth);
    this.IndexDirty = false;
    this.Fps = 1f / maxFps;
    this.AddElememt(dtls);
    this.AddElememt(new RibbonTrail.Element(this.HeadPosition, this.UnitWidth));
  }

  public void AddElememt(RibbonTrail.Element dtls)
  {
    if (this.Head == 99999)
    {
      this.Tail = this.MaxElements - 1;
      this.Head = this.Tail;
      this.IndexDirty = true;
      ++this.ElemCount;
    }
    else
    {
      if (this.Head == 0)
        this.Head = this.MaxElements - 1;
      else
        --this.Head;
      if (this.Head == this.Tail)
      {
        if (this.Tail == 0)
          this.Tail = this.MaxElements - 1;
        else
          --this.Tail;
      }
      else
        ++this.ElemCount;
    }
    this.ElementArray[this.Head] = dtls;
    this.IndexDirty = true;
  }

  public void Reset() => this.ResetElementsPos();

  public void ResetElementsPos()
  {
    if (this.Head == 99999 || this.Head == this.Tail)
      return;
    int num = this.Head;
    while (true)
    {
      int index = num;
      if (index == this.MaxElements)
        index = 0;
      this.ElementArray[index].Position = this.HeadPosition;
      if (index != this.Tail)
        num = index + 1;
      else
        break;
    }
  }

  public void SetColor(Color color) => this.Color = color;

  public void SetHeadPosition(Vector3 pos) => this.HeadPosition = pos;

  public void SetTrailLen(float len)
  {
    this.TrailLength = len;
    this.ElemLength = this.TrailLength / (float) (this.MaxElements - 1);
    this.SquaredElemLength = this.ElemLength * this.ElemLength;
  }

  public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
  {
    this.LowerLeftUV = lowerleft;
    this.UVDimensions = dimensions;
  }

  public void Smooth()
  {
    if (this.ElemCount <= 3)
      return;
    RibbonTrail.Element element1 = this.ElementArray[this.Head];
    int index1 = this.Head + 1;
    if (index1 == this.MaxElements)
      index1 = 0;
    int index2 = index1 + 1;
    if (index2 == this.MaxElements)
      index2 = 0;
    RibbonTrail.Element element2 = this.ElementArray[index1];
    RibbonTrail.Element element3 = this.ElementArray[index2];
    float num1 = Vector3.Angle(Vector3.op_Subtraction(element1.Position, element2.Position), Vector3.op_Subtraction(element2.Position, element3.Position));
    if ((double) num1 <= 60.0)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Division(Vector3.op_Addition(element1.Position, element3.Position), 2f), element2.Position);
    Vector3 zero = Vector3.zero;
    float num2 = (float) (0.10000000149011612 / ((double) num1 / 60.0));
    element2.Position = Vector3.SmoothDamp(element2.Position, Vector3.op_Addition(element2.Position, Vector3.op_Multiply(((Vector3) ref vector3).normalized, element2.Width)), ref zero, num2);
  }

  public void Update()
  {
    this.ElapsedTime += Time.deltaTime;
    if ((double) this.ElapsedTime < (double) this.Fps)
      return;
    this.ElapsedTime -= this.Fps;
    bool flag = false;
    while (!flag)
    {
      RibbonTrail.Element element1 = this.ElementArray[this.Head];
      int index = this.Head + 1;
      if (index == this.MaxElements)
        index = 0;
      RibbonTrail.Element element2 = this.ElementArray[index];
      Vector3 headPosition = this.HeadPosition;
      Vector3 vector3_1 = Vector3.op_Subtraction(headPosition, element2.Position);
      if ((double) ((Vector3) ref vector3_1).sqrMagnitude >= (double) this.SquaredElemLength)
      {
        Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, this.ElemLength / ((Vector3) ref vector3_1).magnitude);
        element1.Position = Vector3.op_Addition(element2.Position, vector3_2);
        this.AddElememt(new RibbonTrail.Element(headPosition, this.UnitWidth));
        vector3_1 = Vector3.op_Subtraction(headPosition, element1.Position);
        if ((double) ((Vector3) ref vector3_1).sqrMagnitude <= (double) this.SquaredElemLength)
          flag = true;
      }
      else
      {
        element1.Position = headPosition;
        flag = true;
      }
      if ((this.Tail + 1) % this.MaxElements == this.Head)
      {
        RibbonTrail.Element element3 = this.ElementArray[this.Tail];
        RibbonTrail.Element element4 = this.ElementArray[this.Tail != 0 ? this.Tail - 1 : this.MaxElements - 1];
        Vector3 vector3_3 = Vector3.op_Subtraction(element3.Position, element4.Position);
        float magnitude = ((Vector3) ref vector3_3).magnitude;
        if ((double) magnitude > 1E-06)
        {
          float num = this.ElemLength - ((Vector3) ref vector3_1).magnitude;
          Vector3 vector3_4 = Vector3.op_Multiply(vector3_3, num / magnitude);
          element3.Position = Vector3.op_Addition(element4.Position, vector3_4);
        }
      }
    }
    this.UpdateVertices(((Component) Camera.main).transform.position);
    this.UpdateIndices();
  }

  public void UpdateIndices()
  {
    if (!this.IndexDirty)
      return;
    VertexPool pool = this.Vertexsegment.Pool;
    if (this.Head != 99999 && this.Head != this.Tail)
    {
      int num1 = this.Head;
      int num2 = 0;
      while (true)
      {
        int num3 = num1 + 1;
        if (num3 == this.MaxElements)
          num3 = 0;
        if (num3 * 2 >= 65536)
          Debug.LogError((object) "Too many elements!");
        int num4 = this.Vertexsegment.VertStart + num3 * 2;
        int num5 = this.Vertexsegment.VertStart + num1 * 2;
        int index = this.Vertexsegment.IndexStart + num2 * 6;
        pool.Indices[index] = num5;
        pool.Indices[index + 1] = num5 + 1;
        pool.Indices[index + 2] = num4;
        pool.Indices[index + 3] = num5 + 1;
        pool.Indices[index + 4] = num4 + 1;
        pool.Indices[index + 5] = num4;
        if (num3 != this.Tail)
        {
          num1 = num3;
          ++num2;
        }
        else
          break;
      }
      pool.IndiceChanged = true;
    }
    this.IndexDirty = false;
  }

  public void UpdateVertices(Vector3 eyePos)
  {
    float num1 = 0.0f;
    float num2 = this.ElemLength * (float) (this.MaxElements - 2);
    if (this.Head == 99999 || this.Head == this.Tail)
      return;
    int index1 = this.Head;
    int index2 = this.Head;
    while (true)
    {
      if (index2 == this.MaxElements)
        index2 = 0;
      RibbonTrail.Element element = this.ElementArray[index2];
      if (index2 * 2 >= 65536)
        Debug.LogError((object) "Too many elements!");
      int index3 = this.Vertexsegment.VertStart + index2 * 2;
      int index4 = index2 + 1;
      if (index4 == this.MaxElements)
        index4 = 0;
      Vector3 vector3_1 = Vector3.Cross(index2 != this.Head ? (index2 != this.Tail ? Vector3.op_Subtraction(this.ElementArray[index4].Position, this.ElementArray[index1].Position) : Vector3.op_Subtraction(element.Position, this.ElementArray[index1].Position)) : Vector3.op_Subtraction(this.ElementArray[index4].Position, element.Position), Vector3.op_Subtraction(eyePos, element.Position));
      ((Vector3) ref vector3_1).Normalize();
      Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, element.Width * 0.5f);
      Vector3 vector3_3 = Vector3.op_Subtraction(element.Position, vector3_2);
      Vector3 vector3_4 = Vector3.op_Addition(element.Position, vector3_2);
      VertexPool pool = this.Vertexsegment.Pool;
      float num3 = this.StretchType != 0 ? num1 / num2 * Mathf.Abs(this.UVDimensions.x) : num1 / num2 * Mathf.Abs(this.UVDimensions.y);
      Vector2 zero = Vector2.zero;
      pool.Vertices[index3] = vector3_3;
      pool.Colors[index3] = this.Color;
      if (this.StretchType == 0)
      {
        zero.x = this.LowerLeftUV.x + this.UVDimensions.x;
        zero.y = this.LowerLeftUV.y - num3;
      }
      else
      {
        zero.x = this.LowerLeftUV.x + num3;
        zero.y = this.LowerLeftUV.y;
      }
      pool.UVs[index3] = zero;
      pool.Vertices[index3 + 1] = vector3_4;
      pool.Colors[index3 + 1] = this.Color;
      if (this.StretchType == 0)
      {
        zero.x = this.LowerLeftUV.x;
        zero.y = this.LowerLeftUV.y - num3;
      }
      else
      {
        zero.x = this.LowerLeftUV.x + num3;
        zero.y = this.LowerLeftUV.y - Mathf.Abs(this.UVDimensions.y);
      }
      pool.UVs[index3 + 1] = zero;
      if (index2 != this.Tail)
      {
        index1 = index2;
        Vector3 vector3_5 = Vector3.op_Subtraction(this.ElementArray[index4].Position, element.Position);
        num1 += ((Vector3) ref vector3_5).magnitude;
        ++index2;
      }
      else
        break;
    }
    this.Vertexsegment.Pool.UVChanged = true;
    this.Vertexsegment.Pool.VertChanged = true;
    this.Vertexsegment.Pool.ColorChanged = true;
  }

  public class Element
  {
    public Vector3 Position;
    public float Width;

    public Element(Vector3 position, float width)
    {
      this.Position = position;
      this.Width = width;
    }
  }
}
