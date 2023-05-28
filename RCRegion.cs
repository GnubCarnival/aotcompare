// Decompiled with JetBrains decompiler
// Type: RCRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RCRegion
{
  private float dimX;
  private float dimY;
  private float dimZ;
  public Vector3 location;
  public GameObject myBox;

  public RCRegion(Vector3 loc, float x, float y, float z)
  {
    this.location = loc;
    this.dimX = x;
    this.dimY = y;
    this.dimZ = z;
  }

  public float GetRandomX() => this.location.x + Random.Range((float) (-(double) this.dimX / 2.0), this.dimX / 2f);

  public float GetRandomY() => this.location.y + Random.Range((float) (-(double) this.dimY / 2.0), this.dimY / 2f);

  public float GetRandomZ() => this.location.z + Random.Range((float) (-(double) this.dimZ / 2.0), this.dimZ / 2f);
}
