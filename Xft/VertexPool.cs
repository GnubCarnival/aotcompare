﻿// Decompiled with JetBrains decompiler
// Type: Xft.VertexPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
  public class VertexPool
  {
    public const int BlockSize = 108;
    public float BoundsScheduleTime = 1f;
    public bool ColorChanged;
    public Color[] Colors;
    public float ElapsedTime;
    public bool FirstUpdate = true;
    protected int IndexTotal;
    protected int IndexUsed;
    public bool IndiceChanged;
    public int[] Indices;
    public Material Material;
    public Mesh Mesh;
    protected List<VertexPool.VertexSegment> SegmentList = new List<VertexPool.VertexSegment>();
    public bool UV2Changed;
    public bool UVChanged;
    public Vector2[] UVs;
    public Vector2[] UVs2;
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
      this.IndiceChanged = this.ColorChanged = this.UVChanged = this.UV2Changed = this.VertChanged = true;
    }

    public void EnlargeArrays(int count, int icount)
    {
      Vector3[] vertices = this.Vertices;
      this.Vertices = new Vector3[this.Vertices.Length + count];
      vertices.CopyTo((Array) this.Vertices, 0);
      Vector2[] uvs = this.UVs;
      this.UVs = new Vector2[this.UVs.Length + count];
      uvs.CopyTo((Array) this.UVs, 0);
      Vector2[] uvs2 = this.UVs2;
      this.UVs2 = new Vector2[this.UVs2.Length + count];
      uvs2.CopyTo((Array) this.UVs2, 0);
      this.InitDefaultShaderParam(this.UVs2);
      Color[] colors = this.Colors;
      this.Colors = new Color[this.Colors.Length + count];
      colors.CopyTo((Array) this.Colors, 0);
      int[] indices = this.Indices;
      this.Indices = new int[this.Indices.Length + icount];
      indices.CopyTo((Array) this.Indices, 0);
      this.VertCountChanged = true;
      this.IndiceChanged = true;
      this.ColorChanged = true;
      this.UVChanged = true;
      this.VertChanged = true;
      this.UV2Changed = true;
    }

    public Material GetMaterial() => this.Material;

    public VertexPool.VertexSegment GetRopeVertexSeg(int maxcount) => this.GetVertices(maxcount * 2, (maxcount - 1) * 6);

    public VertexPool.VertexSegment GetVertices(int vcount, int icount)
    {
      int count = 0;
      int icount1 = 0;
      if (this.VertexUsed + vcount >= this.VertexTotal)
        count = (vcount / 108 + 1) * 108;
      if (this.IndexUsed + icount >= this.IndexTotal)
        icount1 = (icount / 108 + 1) * 108;
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
      this.UVs2 = new Vector2[4];
      this.Colors = new Color[4];
      this.Indices = new int[6];
      this.VertexTotal = 4;
      this.IndexTotal = 6;
      this.InitDefaultShaderParam(this.UVs2);
    }

    private void InitDefaultShaderParam(Vector2[] uv2)
    {
      for (int index = 0; index < uv2.Length; ++index)
      {
        uv2[index].x = 1f;
        uv2[index].y = 0.0f;
      }
    }

    public void LateUpdate()
    {
      if (this.VertCountChanged)
        this.Mesh.Clear();
      this.Mesh.vertices = this.Vertices;
      if (this.UVChanged)
        this.Mesh.uv = this.UVs;
      if (this.UV2Changed)
        this.Mesh.uv2 = this.UVs2;
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
      this.UV2Changed = false;
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

      public void ClearIndices()
      {
        for (int indexStart = this.IndexStart; indexStart < this.IndexStart + this.IndexCount; ++indexStart)
          this.Pool.Indices[indexStart] = 0;
        this.Pool.IndiceChanged = true;
      }
    }
  }
}
