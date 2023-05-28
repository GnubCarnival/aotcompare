// Decompiled with JetBrains decompiler
// Type: Xft.Spline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
  public class Spline
  {
    public int Granularity = 20;
    private List<SplineControlPoint> mControlPoints = new List<SplineControlPoint>();
    private List<SplineControlPoint> mSegments = new List<SplineControlPoint>();

    public SplineControlPoint AddControlPoint(Vector3 pos, Vector3 up)
    {
      SplineControlPoint splineControlPoint = new SplineControlPoint();
      splineControlPoint.Init(this);
      splineControlPoint.Position = pos;
      splineControlPoint.Normal = up;
      this.mControlPoints.Add(splineControlPoint);
      splineControlPoint.ControlPointIndex = this.mControlPoints.Count - 1;
      return splineControlPoint;
    }

    public static Vector3 CatmulRom(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
    {
      double num1 = -0.5;
      double num2 = 1.5;
      double num3 = -1.5;
      double num4 = 0.5;
      double num5 = -2.5;
      double num6 = 2.0;
      double num7 = -0.5;
      double num8 = -0.5;
      double num9 = 0.5;
      double num10 = num1 * (double) T0.x + num2 * (double) P0.x + num3 * (double) P1.x + num4 * (double) T1.x;
      double num11 = (double) T0.x + num5 * (double) P0.x + num6 * (double) P1.x + num7 * (double) T1.x;
      double num12 = num8 * (double) T0.x + num9 * (double) P1.x;
      double x = (double) P0.x;
      double num13 = num1 * (double) T0.y + num2 * (double) P0.y + num3 * (double) P1.y + num4 * (double) T1.y;
      double num14 = (double) T0.y + num5 * (double) P0.y + num6 * (double) P1.y + num7 * (double) T1.y;
      double num15 = num8 * (double) T0.y + num9 * (double) P1.y;
      double y = (double) P0.y;
      double num16 = num1 * (double) T0.z + num2 * (double) P0.z + num3 * (double) P1.z + num4 * (double) T1.z;
      double num17 = (double) T0.z + num5 * (double) P0.z + num6 * (double) P1.z + num7 * (double) T1.z;
      double num18 = num8 * (double) T0.z + num9 * (double) P1.z;
      double z = (double) P0.z;
      return new Vector3((float) (((num10 * (double) f + num11) * (double) f + num12) * (double) f + x), (float) (((num13 * (double) f + num14) * (double) f + num15) * (double) f + y), (float) (((num16 * (double) f + num17) * (double) f + num18) * (double) f + z));
    }

    public void Clear() => this.mControlPoints.Clear();

    public Vector3 InterpolateByLen(float tl)
    {
      float localF;
      return this.LenToSegment(tl, out localF).Interpolate(localF);
    }

    public Vector3 InterpolateNormalByLen(float tl)
    {
      float localF;
      return this.LenToSegment(tl, out localF).InterpolateNormal(localF);
    }

    public SplineControlPoint LenToSegment(float t, out float localF)
    {
      SplineControlPoint segment = (SplineControlPoint) null;
      t = Mathf.Clamp01(t);
      float num1 = t * this.mSegments[this.mSegments.Count - 1].Dist;
      int index;
      for (index = 0; index < this.mSegments.Count; ++index)
      {
        if ((double) this.mSegments[index].Dist >= (double) num1)
        {
          segment = this.mSegments[index];
          break;
        }
      }
      if (index == 0)
      {
        localF = 0.0f;
        return segment;
      }
      SplineControlPoint mSegment = this.mSegments[segment.SegmentIndex - 1];
      float num2 = segment.Dist - mSegment.Dist;
      localF = (num1 - mSegment.Dist) / num2;
      return mSegment;
    }

    public SplineControlPoint NextControlPoint(SplineControlPoint controlpoint)
    {
      if (this.mControlPoints.Count == 0)
        return (SplineControlPoint) null;
      int index = controlpoint.ControlPointIndex + 1;
      return index >= this.mControlPoints.Count ? (SplineControlPoint) null : this.mControlPoints[index];
    }

    public Vector3 NextNormal(SplineControlPoint controlpoint)
    {
      SplineControlPoint splineControlPoint = this.NextControlPoint(controlpoint);
      return splineControlPoint != null ? splineControlPoint.Normal : controlpoint.Normal;
    }

    public Vector3 NextPosition(SplineControlPoint controlpoint)
    {
      SplineControlPoint splineControlPoint = this.NextControlPoint(controlpoint);
      return splineControlPoint != null ? splineControlPoint.Position : controlpoint.Position;
    }

    public SplineControlPoint PreviousControlPoint(SplineControlPoint controlpoint)
    {
      if (this.mControlPoints.Count == 0)
        return (SplineControlPoint) null;
      int index = controlpoint.ControlPointIndex - 1;
      return index < 0 ? (SplineControlPoint) null : this.mControlPoints[index];
    }

    public Vector3 PreviousNormal(SplineControlPoint controlpoint)
    {
      SplineControlPoint splineControlPoint = this.PreviousControlPoint(controlpoint);
      return splineControlPoint != null ? splineControlPoint.Normal : controlpoint.Normal;
    }

    public Vector3 PreviousPosition(SplineControlPoint controlpoint)
    {
      SplineControlPoint splineControlPoint = this.PreviousControlPoint(controlpoint);
      return splineControlPoint != null ? splineControlPoint.Position : controlpoint.Position;
    }

    private void RefreshDistance()
    {
      if (this.mSegments.Count < 1)
        return;
      this.mSegments[0].Dist = 0.0f;
      for (int index = 1; index < this.mSegments.Count; ++index)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.mSegments[index].Position, this.mSegments[index - 1].Position);
        float magnitude = ((Vector3) ref vector3).magnitude;
        this.mSegments[index].Dist = this.mSegments[index - 1].Dist + magnitude;
      }
    }

    public void RefreshSpline()
    {
      this.mSegments.Clear();
      for (int index = 0; index < this.mControlPoints.Count; ++index)
      {
        if (this.mControlPoints[index].IsValid)
        {
          this.mSegments.Add(this.mControlPoints[index]);
          this.mControlPoints[index].SegmentIndex = this.mSegments.Count - 1;
        }
      }
      this.RefreshDistance();
    }

    public List<SplineControlPoint> ControlPoints => this.mControlPoints;

    public SplineControlPoint this[int index] => index > -1 && index < this.mSegments.Count ? this.mSegments[index] : (SplineControlPoint) null;

    public List<SplineControlPoint> Segments => this.mSegments;
  }
}
