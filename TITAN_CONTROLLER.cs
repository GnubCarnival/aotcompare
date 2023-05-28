// Decompiled with JetBrains decompiler
// Type: TITAN_CONTROLLER
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
  public bool bite;
  public bool bitel;
  public bool biter;
  public bool chopl;
  public bool chopr;
  public bool choptl;
  public bool choptr;
  public bool cover;
  public Camera currentCamera;
  public float currentDirection;
  public bool grabbackl;
  public bool grabbackr;
  public bool grabfrontl;
  public bool grabfrontr;
  public bool grabnapel;
  public bool grabnaper;
  public bool isAttackDown;
  public bool isAttackIIDown;
  public bool isHorse;
  public bool isJumpDown;
  public bool isSuicide;
  public bool isWALKDown;
  public bool sit;
  public float targetDirection;

  private void Start()
  {
    this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
      return;
    ((Behaviour) this).enabled = false;
  }

  private void Update()
  {
    if (this.isHorse)
    {
      int num1 = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0 : -1) : 1;
      int num2 = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0 : 1) : -1;
      if (num2 != 0 || num1 != 0)
      {
        Quaternion rotation = ((Component) this.currentCamera).transform.rotation;
        this.targetDirection = ((Quaternion) ref rotation).eulerAngles.y + (float) (-(double) (Mathf.Atan2((float) num1, (float) num2) * 57.29578f) + 90.0);
      }
      else
        this.targetDirection = -874f;
      this.isAttackDown = false;
      this.isAttackIIDown = false;
      if ((double) this.targetDirection != -874.0)
        this.currentDirection = this.targetDirection;
      Quaternion rotation1 = ((Component) this.currentCamera).transform.rotation;
      float num3 = ((Quaternion) ref rotation1).eulerAngles.y - this.currentDirection;
      if ((double) num3 >= 180.0)
      {
        float num4 = num3 - 360f;
      }
      if (SettingsManager.InputSettings.Human.HorseJump.GetKey())
        this.isAttackDown = true;
      this.isWALKDown = SettingsManager.InputSettings.Human.HorseWalk.GetKey();
    }
    else
    {
      int num5 = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0 : -1) : 1;
      int num6 = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0 : 1) : -1;
      if (num6 != 0 || num5 != 0)
      {
        Quaternion rotation = ((Component) this.currentCamera).transform.rotation;
        this.targetDirection = ((Quaternion) ref rotation).eulerAngles.y + (float) (-(double) (Mathf.Atan2((float) num5, (float) num6) * 57.29578f) + 90.0);
      }
      else
        this.targetDirection = -874f;
      this.isAttackDown = false;
      this.isJumpDown = false;
      this.isAttackIIDown = false;
      this.isSuicide = false;
      this.grabbackl = false;
      this.grabbackr = false;
      this.grabfrontl = false;
      this.grabfrontr = false;
      this.grabnapel = false;
      this.grabnaper = false;
      this.choptl = false;
      this.chopr = false;
      this.chopl = false;
      this.choptr = false;
      this.bite = false;
      this.bitel = false;
      this.biter = false;
      this.cover = false;
      this.sit = false;
      if ((double) this.targetDirection != -874.0)
        this.currentDirection = this.targetDirection;
      Quaternion rotation2 = ((Component) this.currentCamera).transform.rotation;
      float num7 = ((Quaternion) ref rotation2).eulerAngles.y - this.currentDirection;
      if ((double) num7 >= 180.0)
        num7 -= 360f;
      if (SettingsManager.InputSettings.Titan.AttackPunch.GetKey())
        this.isAttackDown = true;
      if (SettingsManager.InputSettings.Titan.AttackSlam.GetKey())
        this.isAttackIIDown = true;
      if (SettingsManager.InputSettings.Titan.Jump.GetKey())
        this.isJumpDown = true;
      if (SettingsManager.InputSettings.General.ChangeCharacter.GetKey())
        this.isSuicide = true;
      if (SettingsManager.InputSettings.Titan.CoverNape.GetKey())
        this.cover = true;
      if (SettingsManager.InputSettings.Titan.Sit.GetKey())
        this.sit = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && (double) num7 >= 0.0)
        this.grabfrontr = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && (double) num7 < 0.0)
        this.grabfrontl = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && (double) num7 >= 0.0)
        this.grabbackr = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && (double) num7 < 0.0)
        this.grabbackl = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && (double) num7 >= 0.0)
        this.grabnaper = true;
      if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && (double) num7 < 0.0)
        this.grabnapel = true;
      if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && (double) num7 >= 0.0)
        this.choptr = true;
      if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && (double) num7 < 0.0)
        this.choptl = true;
      if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (double) num7 > 7.5)
        this.biter = true;
      if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (double) num7 < -7.5)
        this.bitel = true;
      if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (double) num7 >= -7.5 && (double) num7 <= 7.5)
        this.bite = true;
      this.isWALKDown = SettingsManager.InputSettings.Titan.Walk.GetKey();
    }
  }
}
