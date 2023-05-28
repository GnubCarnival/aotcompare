// Decompiled with JetBrains decompiler
// Type: Horse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class Horse : MonoBehaviour
{
  private float awayTimer;
  private TITAN_CONTROLLER controller;
  public GameObject dust;
  public GameObject myHero;
  private Vector3 setPoint;
  private float speed = 45f;
  private string State = "idle";
  private float timeElapsed;
  private float _idleTime;

  private void crossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  private void followed()
  {
    if (!Object.op_Inequality((Object) this.myHero, (Object) null))
      return;
    this.State = "follow";
    this.setPoint = Vector3.op_Addition(Vector3.op_Addition(this.myHero.transform.position, Vector3.op_Multiply(Vector3.right, (float) Random.Range(-6, 6))), Vector3.op_Multiply(Vector3.forward, (float) Random.Range(-6, 6)));
    this.setPoint.y = this.getHeight(Vector3.op_Addition(this.setPoint, Vector3.op_Multiply(Vector3.up, 5f)));
    this.awayTimer = 0.0f;
  }

  private float getHeight(Vector3 pt)
  {
    LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    RaycastHit raycastHit;
    return Physics.Raycast(pt, Vector3.op_UnaryNegation(Vector3.up), ref raycastHit, 1000f, ((LayerMask) ref layerMask).value) ? ((RaycastHit) ref raycastHit).point.y : 0.0f;
  }

  public bool IsGrounded()
  {
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
    return Physics.Raycast(Vector3.op_Addition(((Component) this).gameObject.transform.position, Vector3.op_Multiply(Vector3.up, 0.1f)), Vector3.op_UnaryNegation(Vector3.up), 0.3f, ((LayerMask) ref layerMask2).value);
  }

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) this.myHero, (Object) null) && this.photonView.isMine)
      PhotonNetwork.Destroy(((Component) this).gameObject);
    if (this.State == "mounted")
    {
      if (Object.op_Equality((Object) this.myHero, (Object) null))
      {
        this.unmounted();
        return;
      }
      this.myHero.transform.position = Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 1.68f));
      this.myHero.transform.rotation = ((Component) this).transform.rotation;
      this.myHero.rigidbody.velocity = ((Component) this).rigidbody.velocity;
      if ((double) this.controller.targetDirection != -874.0)
      {
        Transform transform = ((Component) this).gameObject.transform;
        Quaternion rotation = ((Component) this).gameObject.transform.rotation;
        Quaternion quaternion1 = Quaternion.Euler(0.0f, this.controller.targetDirection, 0.0f);
        double num1 = 100.0 * (double) Time.deltaTime;
        Vector3 velocity1 = ((Component) this).rigidbody.velocity;
        double num2 = (double) ((Vector3) ref velocity1).magnitude + 20.0;
        double num3 = num1 / num2;
        Quaternion quaternion2 = Quaternion.Lerp(rotation, quaternion1, (float) num3);
        transform.rotation = quaternion2;
        Vector3 velocity2;
        if (this.controller.isWALKDown)
        {
          ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.forward, this.speed), 0.6f), (ForceMode) 5);
          velocity2 = ((Component) this).rigidbody.velocity;
          if ((double) ((Vector3) ref velocity2).magnitude >= (double) this.speed * 0.60000002384185791)
          {
            Rigidbody rigidbody = ((Component) this).rigidbody;
            double num4 = -(double) this.speed * 0.60000002384185791;
            velocity2 = ((Component) this).rigidbody.velocity;
            Vector3 normalized = ((Vector3) ref velocity2).normalized;
            Vector3 vector3 = Vector3.op_Multiply((float) num4, normalized);
            rigidbody.AddForce(vector3, (ForceMode) 5);
          }
        }
        else
        {
          ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(((Component) this).transform.forward, this.speed), (ForceMode) 5);
          Vector3 velocity3 = ((Component) this).rigidbody.velocity;
          if ((double) ((Vector3) ref velocity3).magnitude >= (double) this.speed)
          {
            Rigidbody rigidbody = ((Component) this).rigidbody;
            double num5 = -(double) this.speed;
            Vector3 velocity4 = ((Component) this).rigidbody.velocity;
            Vector3 normalized = ((Vector3) ref velocity4).normalized;
            Vector3 vector3 = Vector3.op_Multiply((float) num5, normalized);
            rigidbody.AddForce(vector3, (ForceMode) 5);
          }
        }
        velocity2 = ((Component) this).rigidbody.velocity;
        if ((double) ((Vector3) ref velocity2).magnitude > 8.0)
        {
          if (!((Component) this).animation.IsPlaying("horse_Run"))
            this.crossFade("horse_Run", 0.1f);
          if (!this.myHero.animation.IsPlaying("horse_run"))
            this.myHero.GetComponent<HERO>().crossFade("horse_run", 0.1f);
          if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
          {
            this.dust.GetComponent<ParticleSystem>().enableEmission = true;
            this.photonView.RPC("setDust", PhotonTargets.Others, (object) true);
          }
        }
        else
        {
          if (!((Component) this).animation.IsPlaying("horse_WALK"))
            this.crossFade("horse_WALK", 0.1f);
          if (!this.myHero.animation.IsPlaying("horse_idle"))
            this.myHero.GetComponent<HERO>().crossFade("horse_idle", 0.1f);
          if (this.dust.GetComponent<ParticleSystem>().enableEmission)
          {
            this.dust.GetComponent<ParticleSystem>().enableEmission = false;
            this.photonView.RPC("setDust", PhotonTargets.Others, (object) false);
          }
        }
      }
      else
      {
        this.toIdleAnimation();
        Vector3 velocity = ((Component) this).rigidbody.velocity;
        if ((double) ((Vector3) ref velocity).magnitude > 15.0)
        {
          if (!this.myHero.animation.IsPlaying("horse_run"))
            this.myHero.GetComponent<HERO>().crossFade("horse_run", 0.1f);
        }
        else if (!this.myHero.animation.IsPlaying("horse_idle"))
          this.myHero.GetComponent<HERO>().crossFade("horse_idle", 0.1f);
      }
      if ((this.controller.isAttackDown || this.controller.isAttackIIDown) && this.IsGrounded())
        ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.up, 25f), (ForceMode) 2);
    }
    else if (this.State == "follow")
    {
      if (Object.op_Equality((Object) this.myHero, (Object) null))
      {
        this.unmounted();
        return;
      }
      Vector3 velocity = ((Component) this).rigidbody.velocity;
      if ((double) ((Vector3) ref velocity).magnitude > 8.0)
      {
        if (!((Component) this).animation.IsPlaying("horse_Run"))
          this.crossFade("horse_Run", 0.1f);
        if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
        {
          this.dust.GetComponent<ParticleSystem>().enableEmission = true;
          this.photonView.RPC("setDust", PhotonTargets.Others, (object) true);
        }
      }
      else
      {
        if (!((Component) this).animation.IsPlaying("horse_WALK"))
          this.crossFade("horse_WALK", 0.1f);
        if (this.dust.GetComponent<ParticleSystem>().enableEmission)
        {
          this.dust.GetComponent<ParticleSystem>().enableEmission = false;
          this.photonView.RPC("setDust", PhotonTargets.Others, (object) false);
        }
      }
      double horizontalAngle = (double) FengMath.getHorizontalAngle(((Component) this).transform.position, this.setPoint);
      Quaternion rotation1 = ((Component) this).gameObject.transform.rotation;
      double num6 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
      float num7 = -Mathf.DeltaAngle((float) horizontalAngle, (float) num6);
      Transform transform = ((Component) this).gameObject.transform;
      Quaternion rotation2 = ((Component) this).gameObject.transform.rotation;
      rotation1 = ((Component) this).gameObject.transform.rotation;
      Quaternion quaternion3 = Quaternion.Euler(0.0f, (float) ((double) ((Quaternion) ref rotation1).eulerAngles.y + (double) num7), 0.0f);
      double num8 = 200.0 * (double) Time.deltaTime;
      velocity = ((Component) this).rigidbody.velocity;
      double num9 = (double) ((Vector3) ref velocity).magnitude + 20.0;
      double num10 = num8 / num9;
      Quaternion quaternion4 = Quaternion.Lerp(rotation2, quaternion3, (float) num10);
      transform.rotation = quaternion4;
      if ((double) Vector3.Distance(this.setPoint, ((Component) this).transform.position) < 20.0)
      {
        ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.forward, this.speed), 0.7f), (ForceMode) 5);
        velocity = ((Component) this).rigidbody.velocity;
        if ((double) ((Vector3) ref velocity).magnitude >= (double) this.speed)
        {
          Rigidbody rigidbody = ((Component) this).rigidbody;
          double num11 = -(double) this.speed * 0.699999988079071;
          velocity = ((Component) this).rigidbody.velocity;
          Vector3 normalized = ((Vector3) ref velocity).normalized;
          Vector3 vector3 = Vector3.op_Multiply((float) num11, normalized);
          rigidbody.AddForce(vector3, (ForceMode) 5);
        }
      }
      else
      {
        ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(((Component) this).transform.forward, this.speed), (ForceMode) 5);
        velocity = ((Component) this).rigidbody.velocity;
        if ((double) ((Vector3) ref velocity).magnitude >= (double) this.speed)
        {
          Rigidbody rigidbody = ((Component) this).rigidbody;
          double num12 = -(double) this.speed;
          velocity = ((Component) this).rigidbody.velocity;
          Vector3 normalized = ((Vector3) ref velocity).normalized;
          Vector3 vector3 = Vector3.op_Multiply((float) num12, normalized);
          rigidbody.AddForce(vector3, (ForceMode) 5);
        }
      }
      this.timeElapsed += Time.deltaTime;
      if ((double) this.timeElapsed > 0.60000002384185791)
      {
        this.timeElapsed = 0.0f;
        if ((double) Vector3.Distance(this.myHero.transform.position, this.setPoint) > 20.0)
          this.followed();
      }
      if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.position) < 5.0)
        this.unmounted();
      if ((double) Vector3.Distance(this.setPoint, ((Component) this).transform.position) < 5.0)
        this.unmounted();
      this.awayTimer += Time.deltaTime;
      if ((double) this.awayTimer > 6.0)
      {
        this.awayTimer = 0.0f;
        LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
        if (Physics.Linecast(Vector3.op_Addition(((Component) this).transform.position, Vector3.up), Vector3.op_Addition(this.myHero.transform.position, Vector3.up), ((LayerMask) ref layerMask).value))
          ((Component) this).transform.position = new Vector3(this.myHero.transform.position.x, this.getHeight(Vector3.op_Addition(this.myHero.transform.position, Vector3.op_Multiply(Vector3.up, 5f))), this.myHero.transform.position.z);
      }
    }
    else if (this.State == "idle")
    {
      this.toIdleAnimation();
      if (Object.op_Inequality((Object) this.myHero, (Object) null) && (double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.position) > 20.0)
        this.followed();
    }
    ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -50f * ((Component) this).rigidbody.mass, 0.0f));
  }

  public void mounted()
  {
    this.State = nameof (mounted);
    ((Behaviour) ((Component) this).gameObject.GetComponent<TITAN_CONTROLLER>()).enabled = true;
    if (!Object.op_Inequality((Object) this.myHero, (Object) null))
      return;
    this.myHero.GetComponent<HERO>().SetInterpolationIfEnabled(false);
  }

  [RPC]
  private void netCrossFade(string aniName, float time) => ((Component) this).animation.CrossFade(aniName, time);

  [RPC]
  private void netPlayAnimation(string aniName) => ((Component) this).animation.Play(aniName);

  [RPC]
  private void netPlayAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
  }

  public void playAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  [RPC]
  private void setDust(bool enable)
  {
    if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
      return;
    this.dust.GetComponent<ParticleSystem>().enableEmission = enable;
  }

  private void Start() => this.controller = ((Component) this).gameObject.GetComponent<TITAN_CONTROLLER>();

  private void toIdleAnimation()
  {
    Vector3 velocity1 = ((Component) this).rigidbody.velocity;
    if ((double) ((Vector3) ref velocity1).magnitude > 0.10000000149011612)
    {
      Vector3 velocity2 = ((Component) this).rigidbody.velocity;
      if ((double) ((Vector3) ref velocity2).magnitude > 15.0)
      {
        if (!((Component) this).animation.IsPlaying("horse_Run"))
          this.crossFade("horse_Run", 0.1f);
        if (this.dust.GetComponent<ParticleSystem>().enableEmission)
          return;
        this.dust.GetComponent<ParticleSystem>().enableEmission = true;
        this.photonView.RPC("setDust", PhotonTargets.Others, (object) true);
      }
      else
      {
        if (!((Component) this).animation.IsPlaying("horse_WALK"))
          this.crossFade("horse_WALK", 0.1f);
        if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
          return;
        this.dust.GetComponent<ParticleSystem>().enableEmission = false;
        this.photonView.RPC("setDust", PhotonTargets.Others, (object) false);
      }
    }
    else
    {
      if ((double) this._idleTime <= 0.0)
      {
        if (((Component) this).animation.IsPlaying("horse_idle0"))
        {
          float num = Random.Range(0.0f, 1f);
          if ((double) num < 0.33000001311302185)
            this.crossFade("horse_idle1", 0.1f);
          else if ((double) num < 0.6600000262260437)
            this.crossFade("horse_idle2", 0.1f);
          else
            this.crossFade("horse_idle3", 0.1f);
          this._idleTime = 1f;
        }
        else
        {
          this.crossFade("horse_idle0", 0.1f);
          this._idleTime = Random.Range(1f, 4f);
        }
      }
      if (this.dust.GetComponent<ParticleSystem>().enableEmission)
      {
        this.dust.GetComponent<ParticleSystem>().enableEmission = false;
        this.photonView.RPC("setDust", PhotonTargets.Others, (object) false);
      }
      this._idleTime -= Time.deltaTime;
    }
  }

  public void unmounted()
  {
    this.State = "idle";
    ((Behaviour) ((Component) this).gameObject.GetComponent<TITAN_CONTROLLER>()).enabled = false;
    if (!Object.op_Inequality((Object) this.myHero, (Object) null))
      return;
    this.myHero.GetComponent<HERO>().SetInterpolationIfEnabled(true);
  }
}
