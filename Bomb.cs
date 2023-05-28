// Decompiled with JetBrains decompiler
// Type: Bomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Constants;
using GameProgress;
using Photon;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Bomb : MonoBehaviour
{
  private Vector3 correctPlayerPos = Vector3.zero;
  private Quaternion correctPlayerRot = Quaternion.identity;
  private Vector3 correctPlayerVelocity = Vector3.zero;
  public bool Disabled;
  public float SmoothingDelay = 10f;
  public float BombRadius;
  private TITAN _collidedTitan;
  private SphereCollider _sphereCollider;
  private List<GameObject> _hideUponDestroy;
  private ParticleSystem _trail;
  private ParticleSystem _flame;
  private float _DisabledTrailFadeMultiplier = 0.6f;
  private HERO _owner;
  private bool _receivedNonZeroVelocity;
  private bool _ownerIsUpdated;

  public void Setup(HERO owner, float bombRadius)
  {
    this._owner = owner;
    this.BombRadius = bombRadius;
  }

  public void Awake()
  {
    if (!Object.op_Inequality((Object) this.photonView, (Object) null))
      return;
    this.photonView.observed = (Component) this;
    this.correctPlayerPos = ((Component) this).transform.position;
    this.correctPlayerRot = ((Component) this).transform.rotation;
    PhotonPlayer owner = this.photonView.owner;
    this._trail = ((Component) ((Component) this).transform.Find("Trail")).GetComponent<ParticleSystem>();
    this._flame = ((Component) ((Component) this).transform.Find("Flame")).GetComponent<ParticleSystem>();
    this._sphereCollider = ((Component) this).GetComponent<SphereCollider>();
    this._hideUponDestroy = new List<GameObject>();
    this._hideUponDestroy.Add(((Component) ((Component) this).transform.Find("Flame")).gameObject);
    this._hideUponDestroy.Add(((Component) ((Component) this).transform.Find("ThunderSpearModel")).gameObject);
    if (SettingsManager.AbilitySettings.ShowBombColors.Value)
    {
      Color bombColor = BombUtil.GetBombColor(owner, 1f);
      this._trail.startColor = bombColor;
      this._flame.startColor = bombColor;
    }
    if (!this.photonView.isMine)
      return;
    this.photonView.RPC("IsUpdatedRPC", PhotonTargets.All);
  }

  [RPC]
  private void IsUpdatedRPC(PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    this._ownerIsUpdated = true;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!this.photonView.isMine || this.Disabled)
      return;
    this.Explode(this.BombRadius);
  }

  public void DestroySelf()
  {
    if (!this.photonView.isMine || this.Disabled)
      return;
    this.photonView.RPC("DisableRPC", PhotonTargets.All);
    this.StartCoroutine(this.WaitAndFinishDestroyCoroutine(1.5f));
  }

  private IEnumerator WaitAndFinishDestroyCoroutine(float time)
  {
    Bomb bomb = this;
    yield return (object) new WaitForSeconds(time);
    if (Object.op_Inequality((Object) bomb._collidedTitan, (Object) null))
      bomb._collidedTitan.isThunderSpear = false;
    PhotonNetwork.Destroy(((Component) bomb).gameObject);
  }

  [RPC]
  public void DisableRPC(PhotonMessageInfo info = null)
  {
    if (this.Disabled || info != null && info.sender != this.photonView.owner)
      return;
    foreach (GameObject gameObject in this._hideUponDestroy)
      gameObject.SetActive(false);
    ((Collider) this._sphereCollider).enabled = false;
    this.SetDisabledTrailFade();
    ((Component) this).rigidbody.velocity = Vector3.zero;
    this.Disabled = true;
  }

  private void SetDisabledTrailFade()
  {
    int particleCount = this._trail.particleCount;
    float num = this._trail.startLifetime * this._DisabledTrailFadeMultiplier;
    ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[particleCount];
    this._trail.GetParticles(particleArray);
    for (int index = 0; index < particleCount; ++index)
    {
      ref ParticleSystem.Particle local = ref particleArray[index];
      ((ParticleSystem.Particle) ref local).lifetime = ((ParticleSystem.Particle) ref local).lifetime * this._DisabledTrailFadeMultiplier;
      ((ParticleSystem.Particle) ref particleArray[index]).startLifetime = num;
    }
    this._trail.SetParticles(particleArray, particleCount);
  }

  public void Explode(float radius)
  {
    if (this.Disabled)
      return;
    PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", ((Component) this).transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
    this.KillPlayersInRadius(radius);
    this.DestroySelf();
  }

  private void KillPlayersInRadius(float radius)
  {
    foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
    {
      GameObject gameObject = ((Component) player).gameObject;
      if ((double) Vector3.Distance(gameObject.transform.position, ((Component) this).transform.position) < (double) radius && !gameObject.GetPhotonView().isMine && !player.bombImmune)
      {
        PhotonPlayer owner = gameObject.GetPhotonView().owner;
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam] != null && owner.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
        {
          int num1 = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]);
          int num2 = RCextensions.returnIntFromObject(owner.customProperties[(object) PhotonPlayerProperty.RCteam]);
          if (num1 == 0 || num1 != num2)
            this.KillPlayer(player);
        }
        else
          this.KillPlayer(player);
      }
    }
  }

  private void KillPlayer(HERO hero)
  {
    hero.markDie();
    hero.photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) (RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]) + " "));
    FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
    GameProgressManager.RegisterHumanKill(((Component) this._owner).gameObject, hero, KillWeapon.ThunderSpear);
  }

  private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting)
    {
      stream.SendNext((object) ((Component) this).transform.position);
      stream.SendNext((object) ((Component) this).transform.rotation);
      stream.SendNext((object) ((Component) this).rigidbody.velocity);
    }
    else
    {
      this.correctPlayerPos = (Vector3) stream.ReceiveNext();
      this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
      this.correctPlayerVelocity = (Vector3) stream.ReceiveNext();
    }
  }

  private void Update()
  {
    if (this.photonView.isMine)
      return;
    ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
    ((Component) this).transform.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
    ((Component) this).rigidbody.velocity = this.correctPlayerVelocity;
    if (Vector3.op_Inequality(((Component) this).rigidbody.velocity, Vector3.zero))
    {
      this._receivedNonZeroVelocity = true;
    }
    else
    {
      if (this._ownerIsUpdated || !this._receivedNonZeroVelocity || this.Disabled)
        return;
      this.Disabled = true;
      foreach (GameObject gameObject in this._hideUponDestroy)
        gameObject.SetActive(false);
    }
  }

  private void FixedUpdate()
  {
    if (this.Disabled || !this.photonView.isMine)
      return;
    this.CheckCollide();
  }

  private void CheckCollide()
  {
    LayerMask layerMask = LayerMask.op_Implicit(1 << PhysicsLayer.PlayerAttackBox | 1 << PhysicsLayer.PlayerHitBox);
    foreach (Collider collider in Physics.OverlapSphere(Vector3.op_Addition(((Component) this).transform.position, this._sphereCollider.center), this._sphereCollider.radius, LayerMask.op_Implicit(layerMask)))
    {
      if (((Object) collider).name.Contains("PlayerDetectorRC"))
      {
        TITAN component = ((Component) ((Component) collider).transform.root).gameObject.GetComponent<TITAN>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          if (Object.op_Equality((Object) this._collidedTitan, (Object) null))
          {
            this._collidedTitan = component;
            this._collidedTitan.isThunderSpear = true;
          }
          else if (Object.op_Inequality((Object) this._collidedTitan, (Object) component))
          {
            this._collidedTitan.isThunderSpear = false;
            this._collidedTitan = component;
            this._collidedTitan.isThunderSpear = true;
          }
        }
      }
      else if (((Component) collider).gameObject.layer == PhysicsLayer.PlayerHitBox && !((Component) ((Component) collider).transform.root).gameObject.GetComponent<HERO>().photonView.isMine)
        this.Explode(this.BombRadius);
    }
  }
}
