// Decompiled with JetBrains decompiler
// Type: BloodSplatterScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BloodSplatterScript : MonoBehaviour
{
  private GameObject[] bloodInstances;
  public int bloodLocalRotationYOffset;
  public Transform bloodPosition;
  public Transform bloodPrefab;
  public Transform bloodRotation;
  public int maxAmountBloodPrefabs = 20;

  public void Main()
  {
  }

  public void Update()
  {
    if (!Input.GetMouseButtonDown(0))
      return;
    this.bloodRotation.Rotate(0.0f, (float) this.bloodLocalRotationYOffset, 0.0f);
    Object.Instantiate((Object) this.bloodPrefab, this.bloodPosition.position, this.bloodRotation.rotation);
    this.bloodInstances = GameObject.FindGameObjectsWithTag("blood");
    if (this.bloodInstances.Length < this.maxAmountBloodPrefabs)
      return;
    Object.Destroy((Object) this.bloodInstances[0]);
  }
}
