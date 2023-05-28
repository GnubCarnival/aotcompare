﻿// Decompiled with JetBrains decompiler
// Type: VertexPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using UnityEngine;

public class VertexPool
{
  public const int BlockSize = 36;
  public float BoundsScheduleTime = 1f;
  public bool ColorChanged;
  public Color[] Colors;
  public float ElapsedTime;
  protected bool FirstUpdate = true;
  protected int IndexTotal;
  protected int IndexUsed;
  public bool IndiceChanged;
  public int[] Indices;
  public Material Material;
  public Mesh Mesh;
  public bool UVChanged;
  public Vector2[] UVs;
  public bool VertChanged;
  protected bool VertCountChanged;
  protected int VertexTotal;
  protected int VertexUsed;
  public Vector3[] Vertices;

  public VertexPool(Mesh mesh, Material material)
  {
    this.VertexTotal = this.VertexUsed = 0;
    this.VertCountChanged = false;
    this.Mesh = mesh;
    this.Material = material;
    this.InitArrays();
    this.Vertices = this.Mesh.vertices;
    this.Indices = this.Mesh.triangles;
    this.Colors = this.Mesh.colors;
    this.UVs = this.Mesh.uv;
    this.IndiceChanged = this.ColorChanged = this.UVChanged = this.VertChanged = true;
  }

  public RibbonTrail AddRibbonTrail(
    float width,
    int maxelemnt,
    float len,
    Vector3 pos,
    int stretchType,
    float maxFps)
  {
    return new RibbonTrail(this.GetVertices(maxelemnt * 2, (maxelemnt - 1) * 6), width, maxelemnt, len, pos, stretchType, maxFps);
  }

  public Sprite AddSprite(
    float width,
    float height,
    STYPE type,
    ORIPOINT ori,
    Camera cam,
    int uvStretch,
    float maxFps)
  {
    return new Sprite(this.GetVertices(4, 6), width, height, type, ori, cam, uvStretch, maxFps);
  }

  public void EnlargeArrays(int count, int icount)
  {
    Vector3[] vertices1 = this.Vertices;
    this.Vertices = new Vector3[this.Vertices.Length + count];
    Vector3[] vertices2 = this.Vertices;
    vertices1.CopyTo((Array) vertices2, 0);
    Vector2[] uvs1 = this.UVs;
    this.UVs = new Vector2[this.UVs.Length + count];
    Vector2[] uvs2 = this.UVs;
    uvs1.CopyTo((Array) uvs2, 0);
    Color[] colors1 = this.Colors;
    this.Colors = new Color[this.Colors.Length + count];
    Color[] colors2 = this.Colors;
    colors1.CopyTo((Array) colors2, 0);
    int[] indices1 = this.Indices;
    this.Indices = new int[this.Indices.Length + icount];
    int[] indices2 = this.Indices;
    indices1.CopyTo((Array) indices2, 0);
    this.VertCountChanged = true;
    this.IndiceChanged = true;
    this.ColorChanged = true;
    this.UVChanged = true;
    this.VertChanged = true;
  }

  public Material GetMaterial() => this.Material;

  public VertexPool.VertexSegment GetVertices(int vcount, int icount)
  {
    int count = 0;
    int icount1 = 0;
    if (this.VertexUsed + vcount >= this.VertexTotal)
      count = (vcount / 36 + 1) * 36;
    if (this.IndexUsed + icount >= this.IndexTotal)
      icount1 = (icount / 36 + 1) * 36;
    this.VertexUsed += vcount;
    this.IndexUsed += icount;
    if (count != 0 || icount1 != 0)
    {
      this.EnlargeArrays(count, icount1);
      this.VertexTotal += count;
      this.IndexTotal += icount1;
    }
    return new VertexPool.VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
  }

  protected void InitArrays()
  {
    this.Vertices = new Vector3[4];
    this.UVs = new Vector2[4];
    this.Colors = new Color[4];
    this.Indices = new int[6];
    this.VertexTotal = 4;
    this.IndexTotal = 6;
  }

  public void LateUpdate()
  {
    if (this.VertCountChanged)
      this.Mesh.Clear();
    this.Mesh.vertices = this.Vertices;
    if (this.UVChanged)
      this.Mesh.uv = this.UVs;
    if (this.ColorChanged)
      this.Mesh.colors = this.Colors;
    if (this.IndiceChanged)
      this.Mesh.triangles = this.Indices;
    this.ElapsedTime += Time.deltaTime;
    if ((double) this.ElapsedTime > (double) this.BoundsScheduleTime || this.FirstUpdate)
    {
      this.RecalculateBounds();
      this.ElapsedTime = 0.0f;
    }
    if ((double) this.ElapsedTime > (double) this.BoundsScheduleTime)
      this.FirstUpdate = false;
    this.VertCountChanged = false;
    this.IndiceChanged = false;
    this.ColorChanged = false;
    this.UVChanged = false;
    this.VertChanged = false;
  }

  public void RecalculateBounds() => this.Mesh.RecalculateBounds();

  public class VertexSegment
  {
    public int IndexCount;
    public int IndexStart;
    public VertexPool Pool;
    public int VertCount;
    public int VertStart;

    public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
    {
      this.VertStart = start;
      this.VertCount = count;
      this.IndexCount = icount;
      this.IndexStart = istart;
      this.Pool = pool;
    }
  }
}