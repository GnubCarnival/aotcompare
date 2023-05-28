// Decompiled with JetBrains decompiler
// Type: HERO_DEAD_BODY_SETUP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class HERO_DEAD_BODY_SETUP : MonoBehaviour
{
  public GameObject blood_arm_l;
  public GameObject blood_arm_r;
  public GameObject blood_lower;
  public GameObject blood_upper;
  public GameObject blood_upper1;
  public GameObject blood_upper2;
  public GameObject chest;
  public GameObject col_chest;
  public GameObject col_head;
  public GameObject col_lower_arm_l;
  public GameObject col_lower_arm_r;
  public GameObject col_shin_l;
  public GameObject col_shin_r;
  public GameObject col_thigh_l;
  public GameObject col_thigh_r;
  public GameObject col_upper_arm_l;
  public GameObject col_upper_arm_r;
  public GameObject hand_l;
  public GameObject hand_r;
  public GameObject head;
  public GameObject leg;
  private float lifetime = 15f;

  public void init(string aniname, float time, BODY_PARTS part)
  {
    ((Component) this).animation.Play(aniname);
    ((Component) this).animation[aniname].normalizedTime = time;
    ((Component) this).animation[aniname].speed = 0.0f;
    switch (part)
    {
      case BODY_PARTS.UPPER:
        ((Collider) this.col_upper_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_upper_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_r.GetComponent<CapsuleCollider>()).enabled = false;
        Object.Destroy((Object) this.leg);
        Object.Destroy((Object) this.hand_l);
        Object.Destroy((Object) this.hand_r);
        Object.Destroy((Object) this.blood_lower);
        Object.Destroy((Object) this.blood_arm_l);
        Object.Destroy((Object) this.blood_arm_r);
        ((Component) this).gameObject.GetComponent<HERO_SETUP>().createHead2();
        ((Component) this).gameObject.GetComponent<HERO_SETUP>().createUpperBody2();
        break;
      case BODY_PARTS.ARM_L:
        ((Collider) this.col_upper_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_head.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_chest.GetComponent<BoxCollider>()).enabled = false;
        Object.Destroy((Object) this.head);
        Object.Destroy((Object) this.chest);
        Object.Destroy((Object) this.leg);
        Object.Destroy((Object) this.hand_r);
        Object.Destroy((Object) this.blood_lower);
        Object.Destroy((Object) this.blood_upper);
        Object.Destroy((Object) this.blood_upper1);
        Object.Destroy((Object) this.blood_upper2);
        Object.Destroy((Object) this.blood_arm_r);
        ((Component) this).gameObject.GetComponent<HERO_SETUP>().createLeftArm();
        break;
      case BODY_PARTS.ARM_R:
        ((Collider) this.col_upper_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_thigh_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_shin_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_head.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_chest.GetComponent<BoxCollider>()).enabled = false;
        Object.Destroy((Object) this.head);
        Object.Destroy((Object) this.chest);
        Object.Destroy((Object) this.leg);
        Object.Destroy((Object) this.hand_l);
        Object.Destroy((Object) this.blood_lower);
        Object.Destroy((Object) this.blood_upper);
        Object.Destroy((Object) this.blood_upper1);
        Object.Destroy((Object) this.blood_upper2);
        Object.Destroy((Object) this.blood_arm_l);
        ((Component) this).gameObject.GetComponent<HERO_SETUP>().createRightArm();
        break;
      case BODY_PARTS.LOWER:
        ((Collider) this.col_upper_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_l.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_upper_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_lower_arm_r.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_head.GetComponent<CapsuleCollider>()).enabled = false;
        ((Collider) this.col_chest.GetComponent<BoxCollider>()).enabled = false;
        Object.Destroy((Object) this.head);
        Object.Destroy((Object) this.chest);
        Object.Destroy((Object) this.hand_l);
        Object.Destroy((Object) this.hand_r);
        Object.Destroy((Object) this.blood_upper);
        Object.Destroy((Object) this.blood_upper1);
        Object.Destroy((Object) this.blood_upper2);
        Object.Destroy((Object) this.blood_arm_l);
        Object.Destroy((Object) this.blood_arm_r);
        ((Component) this).gameObject.GetComponent<HERO_SETUP>().createLowerBody();
        break;
    }
  }

  private void Start()
  {
  }

  private void Update()
  {
    this.lifetime -= Time.deltaTime;
    if ((double) this.lifetime > 0.0)
      return;
    ((Component) this).gameObject.GetComponent<HERO_SETUP>().deleteCharacterComponent2();
    Object.Destroy((Object) ((Component) this).gameObject);
  }
}
