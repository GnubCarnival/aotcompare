// Decompiled with JetBrains decompiler
// Type: LevelMovingBrick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelMovingBrick : MonoBehaviour
{
  private Vector3 pointA;
  private Vector3 pointB;
  public GameObject pointGOA;
  public GameObject pointGOB;
  public float speed = 10f;
  public bool towardsA = true;

  private void Start()
  {
    this.pointA = this.pointGOA.transform.position;
    this.pointB = this.pointGOB.transform.position;
    Object.Destroy((Object) this.pointGOA);
    Object.Destroy((Object) this.pointGOB);
  }

  private void Update()
  {
    if (this.towardsA)
    {
      ((Component) this).transform.position = Vector3.MoveTowards(((Component) this).transform.position, this.pointA, this.speed * Time.deltaTime);
      if ((double) Vector3.Distance(((Component) this).transform.position, this.pointA) >= 2.0)
        return;
      this.towardsA = false;
    }
    else
    {
      ((Component) this).transform.position = Vector3.MoveTowards(((Component) this).transform.position, this.pointB, this.speed * Time.deltaTime);
      if ((double) Vector3.Distance(((Component) this).transform.position, this.pointB) >= 2.0)
        return;
      this.towardsA = true;
    }
  }
}
