// Decompiled with JetBrains decompiler
// Type: HERO_ON_MENU
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class HERO_ON_MENU : MonoBehaviour
{
  private Vector3 cameraOffset;
  private Transform cameraPref;
  public int costumeId;
  private Transform head;
  public float headRotationX;
  public float headRotationY;

  private void LateUpdate()
  {
    Transform head = this.head;
    Quaternion rotation = this.head.rotation;
    double num1 = (double) ((Quaternion) ref rotation).eulerAngles.x + (double) this.headRotationX;
    rotation = this.head.rotation;
    double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y + (double) this.headRotationY;
    rotation = this.head.rotation;
    double z = (double) ((Quaternion) ref rotation).eulerAngles.z;
    Quaternion quaternion = Quaternion.Euler((float) num1, (float) num2, (float) z);
    head.rotation = quaternion;
    if (this.costumeId != 9)
      return;
    GameObject.Find("MainCamera_Mono").transform.position = Vector3.op_Addition(this.cameraPref.position, this.cameraOffset);
  }

  private void Start()
  {
    HERO_SETUP component = ((Component) this).gameObject.GetComponent<HERO_SETUP>();
    HeroCostume.init2();
    component.init();
    component.myCostume = HeroCostume.costume[this.costumeId];
    component.setCharacterComponent();
    this.head = ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
    this.cameraPref = ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
    if (this.costumeId == 9)
      this.cameraOffset = Vector3.op_Subtraction(GameObject.Find("MainCamera_Mono").transform.position, this.cameraPref.position);
    if (component.myCostume.sex == SEX.FEMALE)
    {
      ((Component) this).animation.Play("stand");
      ((Component) this).animation["stand"].normalizedTime = Random.Range(0.0f, 1f);
    }
    else
    {
      ((Component) this).animation.Play("stand_levi");
      ((Component) this).animation["stand_levi"].normalizedTime = Random.Range(0.0f, 1f);
    }
    float num = 0.5f;
    ((Component) this).animation["stand"].speed = num;
    ((Component) this).animation["stand_levi"].speed = num;
  }
}
