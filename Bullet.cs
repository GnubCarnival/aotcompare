// Decompiled with JetBrains decompiler
// Type: Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using CustomSkins;
using Photon;
using System.Collections;
using UnityEngine;

internal class Bullet : MonoBehaviour
{
  private Vector3 heightOffSet = Vector3.op_Multiply(Vector3.up, 0.48f);
  private bool isdestroying;
  private float killTime;
  private float killTime2;
  private Vector3 launchOffSet = Vector3.zero;
  private bool left = true;
  public bool leviMode;
  public float leviShootTime;
  private LineRenderer lineRenderer;
  private GameObject master;
  private GameObject myRef;
  public TITAN myTitan;
  private ArrayList nodes = new ArrayList();
  private int phase;
  private GameObject rope;
  private int spiralcount;
  private ArrayList spiralNodes;
  private Vector3 velocity = Vector3.zero;
  private Vector3 velocity2 = Vector3.zero;
  private bool _hasSkin;
  private float _lastLength;
  private float _skinTiling;
  private bool _isTransparent;

  private void SetSkin()
  {
    HumanCustomSkinLoader customSkinLoader = this.master.GetComponent<HERO>()._customSkinLoader;
    HookCustomSkinPart hookCustomSkinPart = this.left ? customSkinLoader.HookL : customSkinLoader.HookR;
    if (hookCustomSkinPart == null)
      return;
    if (Object.op_Inequality((Object) hookCustomSkinPart.HookMaterial, (Object) null))
    {
      this._hasSkin = true;
      ((Renderer) this.lineRenderer).material = hookCustomSkinPart.HookMaterial;
      this._skinTiling = this.left ? customSkinLoader.HookLTiling : customSkinLoader.HookRTiling;
    }
    if (!hookCustomSkinPart.Transparent)
      return;
    this._hasSkin = true;
    this._isTransparent = true;
    ((Renderer) this.lineRenderer).enabled = false;
  }

  private void UpdateSkin()
  {
    if (!this._hasSkin || this._isTransparent)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, this.myRef.transform.position);
    float magnitude = ((Vector3) ref vector3).magnitude;
    if ((double) magnitude == (double) this._lastLength)
      return;
    this._lastLength = magnitude;
    ((Renderer) this.lineRenderer).material.mainTextureScale = new Vector2(this._skinTiling * magnitude, 1f);
  }

  public void checkTitan()
  {
    GameObject mainObject = ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
    if (!Object.op_Inequality((Object) mainObject, (Object) null) || !Object.op_Inequality((Object) this.master, (Object) null) || !Object.op_Equality((Object) this.master, (Object) mainObject))
      return;
    LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("PlayerAttackBox"));
    RaycastHit raycastHit;
    if (!Physics.Raycast(((Component) this).transform.position, this.velocity, ref raycastHit, 10f, ((LayerMask) ref layerMask).value))
      return;
    Collider collider = ((RaycastHit) ref raycastHit).collider;
    if (!((Object) collider).name.Contains("PlayerDetectorRC"))
      return;
    TITAN component = ((Component) ((Component) collider).transform.root).gameObject.GetComponent<TITAN>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    if (Object.op_Equality((Object) this.myTitan, (Object) null))
    {
      this.myTitan = component;
      this.myTitan.isHooked = true;
    }
    else
    {
      if (!Object.op_Inequality((Object) this.myTitan, (Object) component))
        return;
      this.myTitan.isHooked = false;
      this.myTitan = component;
      this.myTitan.isHooked = true;
    }
  }

  public void disable()
  {
    this.phase = 2;
    this.killTime = 0.0f;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
      return;
    this.photonView.RPC("setPhase", PhotonTargets.Others, (object) 2);
  }

  private void FixedUpdate()
  {
    if ((this.phase == 2 || this.phase == 1) && this.leviMode)
    {
      ++this.spiralcount;
      if (this.spiralcount >= 60)
      {
        this.isdestroying = true;
        this.removeMe();
        return;
      }
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
    {
      if (this.phase != 0)
        return;
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(this.velocity, Time.deltaTime), 50f), Vector3.op_Multiply(this.velocity2, Time.deltaTime)));
      this.nodes.Add((object) new Vector3(((Component) this).gameObject.transform.position.x, ((Component) this).gameObject.transform.position.y, ((Component) this).gameObject.transform.position.z));
    }
    else
    {
      if (this.phase != 0)
        return;
      this.checkTitan();
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(this.velocity, Time.deltaTime), 50f), Vector3.op_Multiply(this.velocity2, Time.deltaTime)));
      LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"));
      LayerMask layerMask2 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
      LayerMask layerMask3 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("NetworkObject"));
      LayerMask layerMask4 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask2) | LayerMask.op_Implicit(layerMask3));
      bool flag1 = false;
      RaycastHit raycastHit;
      if (this.nodes.Count <= 1 ? Physics.Linecast((Vector3) this.nodes[this.nodes.Count - 1], ((Component) this).gameObject.transform.position, ref raycastHit, ((LayerMask) ref layerMask4).value) : Physics.Linecast((Vector3) this.nodes[this.nodes.Count - 2], ((Component) this).gameObject.transform.position, ref raycastHit, ((LayerMask) ref layerMask4).value))
      {
        bool flag2 = true;
        if (((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform).gameObject.layer == LayerMask.NameToLayer("EnemyBox"))
        {
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            this.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, (object) ((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform.root).gameObject.GetPhotonView().viewID);
          this.master.GetComponent<HERO>().lastHook = ((Component) ((RaycastHit) ref raycastHit).collider).transform.root;
          ((Component) this).transform.parent = ((Component) ((RaycastHit) ref raycastHit).collider).transform;
        }
        else if (((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform).gameObject.layer == LayerMask.NameToLayer("Ground"))
          this.master.GetComponent<HERO>().lastHook = (Transform) null;
        else if (((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform).gameObject.layer == LayerMask.NameToLayer("NetworkObject") && ((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform).gameObject.tag == "Player" && !this.leviMode)
        {
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            this.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, (object) ((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform.root).gameObject.GetPhotonView().viewID);
          this.master.GetComponent<HERO>().hookToHuman(((Component) ((Component) ((RaycastHit) ref raycastHit).collider).transform.root).gameObject, ((Component) this).transform.position);
          ((Component) this).transform.parent = ((Component) ((RaycastHit) ref raycastHit).collider).transform;
          this.master.GetComponent<HERO>().lastHook = (Transform) null;
        }
        else
          flag2 = false;
        if (this.phase == 2)
          flag2 = false;
        if (flag2)
        {
          this.master.GetComponent<HERO>().launch(((RaycastHit) ref raycastHit).point, this.left, this.leviMode);
          ((Component) this).transform.position = ((RaycastHit) ref raycastHit).point;
          if (this.phase != 2)
          {
            this.phase = 1;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
              this.photonView.RPC("setPhase", PhotonTargets.Others, (object) 1);
              this.photonView.RPC("tieMeTo", PhotonTargets.Others, (object) ((Component) this).transform.position);
            }
            if (this.leviMode)
            {
              Vector3 position = this.master.transform.position;
              Quaternion rotation = this.master.transform.rotation;
              Vector3 eulerAngles = ((Quaternion) ref rotation).eulerAngles;
              this.getSpiral(position, eulerAngles);
            }
            flag1 = true;
          }
        }
      }
      this.nodes.Add((object) new Vector3(((Component) this).gameObject.transform.position.x, ((Component) this).gameObject.transform.position.y, ((Component) this).gameObject.transform.position.z));
      if (flag1)
        return;
      this.killTime2 += Time.deltaTime;
      if ((double) this.killTime2 <= 0.800000011920929)
        return;
      this.phase = 4;
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
        return;
      this.photonView.RPC("setPhase", PhotonTargets.Others, (object) 4);
    }
  }

  private void getSpiral(Vector3 masterposition, Vector3 masterrotation)
  {
    float num1 = 1.2f;
    float num2 = 30f;
    float num3 = 0.5f;
    num1 = 30f;
    double num4 = 0.05000000074505806 + (double) this.spiralcount * 0.029999999329447746;
    float num5 = this.spiralcount >= 5 ? (float) (1.2000000476837158 + (double) (60 - this.spiralcount) * 0.10000000149011612) : Vector2.Distance(new Vector2(masterposition.x, masterposition.z), new Vector2(((Component) this).gameObject.transform.position.x, ((Component) this).gameObject.transform.position.z));
    float num6 = num3 - (float) this.spiralcount * 0.06f;
    float num7 = num5 / num2;
    double num8 = (double) num2;
    float num9 = (float) (num4 / num8 * 2.0 * 3.1415929794311523);
    float num10 = num6 * 6.283185f;
    this.spiralNodes = new ArrayList();
    for (int index = 1; (double) index <= (double) num2; ++index)
    {
      float num11 = (float) ((double) index * (double) num7 * (1.0 + 0.05000000074505806 * (double) index));
      double num12 = (double) index * (double) num9 + (double) num10 + 1.2566369771957397 + (double) masterrotation.y * 0.017300000414252281;
      this.spiralNodes.Add((object) new Vector3(Mathf.Cos((float) num12) * num11, 0.0f, -Mathf.Sin((float) num12) * num11));
    }
  }

  public bool isHooked() => this.phase == 1;

  private void killObject()
  {
    Object.Destroy((Object) this.rope);
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  public void launch(
    Vector3 v,
    Vector3 v2,
    string launcher_ref,
    bool isLeft,
    GameObject hero,
    bool leviMode = false)
  {
    if (this.phase == 2)
      return;
    this.master = hero;
    this.velocity = v;
    this.velocity2 = (double) Mathf.Abs(Mathf.Acos(Vector3.Dot(((Vector3) ref v).normalized, ((Vector3) ref v2).normalized)) * 57.29578f) <= 90.0 ? Vector3.Project(v2, v) : Vector3.zero;
    if (launcher_ref == "hookRefL1")
      this.myRef = hero.GetComponent<HERO>().hookRefL1;
    if (launcher_ref == "hookRefL2")
      this.myRef = hero.GetComponent<HERO>().hookRefL2;
    if (launcher_ref == "hookRefR1")
      this.myRef = hero.GetComponent<HERO>().hookRefR1;
    if (launcher_ref == "hookRefR2")
      this.myRef = hero.GetComponent<HERO>().hookRefR2;
    this.nodes = new ArrayList();
    this.nodes.Add((object) this.myRef.transform.position);
    this.phase = 0;
    this.leviMode = leviMode;
    this.left = isLeft;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
    {
      this.photonView.RPC("myMasterIs", PhotonTargets.Others, (object) hero.GetComponent<HERO>().photonView.viewID, (object) launcher_ref);
      this.photonView.RPC("setVelocityAndLeft", PhotonTargets.Others, (object) v, (object) this.velocity2, (object) this.left);
    }
    ((Component) this).transform.position = this.myRef.transform.position;
    ((Component) this).transform.rotation = Quaternion.LookRotation(((Vector3) ref v).normalized);
    this.SetSkin();
  }

  [RPC]
  private void myMasterIs(int id, string launcherRef, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner || id < 0)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet myMasterIs");
    }
    else
    {
      PhotonView photonView = PhotonView.Find(id);
      if (photonView.owner != info.sender)
      {
        FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet myMasterIs");
      }
      else
      {
        if (Object.op_Equality((Object) photonView, (Object) null) || Object.op_Equality((Object) ((Component) photonView).gameObject, (Object) null))
          return;
        this.master = ((Component) PhotonView.Find(id)).gameObject;
        if (launcherRef == "hookRefL1")
          this.myRef = this.master.GetComponent<HERO>().hookRefL1;
        if (launcherRef == "hookRefL2")
          this.myRef = this.master.GetComponent<HERO>().hookRefL2;
        if (launcherRef == "hookRefR1")
          this.myRef = this.master.GetComponent<HERO>().hookRefR1;
        if (!(launcherRef == "hookRefR2"))
          return;
        this.myRef = this.master.GetComponent<HERO>().hookRefR2;
      }
    }
  }

  private void netLaunch(Vector3 newPosition)
  {
    this.nodes = new ArrayList();
    this.nodes.Add((object) newPosition);
  }

  private void netUpdateLeviSpiral(
    Vector3 newPosition,
    Vector3 masterPosition,
    Vector3 masterrotation)
  {
    this.phase = 2;
    this.leviMode = true;
    this.getSpiral(masterPosition, masterrotation);
    Vector3 vector3_1 = Vector3.op_Subtraction(masterPosition, (Vector3) this.spiralNodes[0]);
    this.lineRenderer.SetVertexCount(this.spiralNodes.Count - (int) ((double) this.spiralcount * 0.5));
    for (int index = 0; (double) index <= (double) (this.spiralNodes.Count - 1) - (double) this.spiralcount * 0.5; ++index)
    {
      if (this.spiralcount < 5)
      {
        Vector3 vector3_2 = Vector3.op_Addition((Vector3) this.spiralNodes[index], vector3_1);
        float num = (float) (this.spiralNodes.Count - 1) - (float) this.spiralcount * 0.5f;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3_2).\u002Ector(vector3_2.x, (float) ((double) vector3_2.y * (((double) num - (double) index) / (double) num) + (double) newPosition.y * ((double) index / (double) num)), vector3_2.z);
        this.lineRenderer.SetPosition(index, vector3_2);
      }
      else
        this.lineRenderer.SetPosition(index, Vector3.op_Addition((Vector3) this.spiralNodes[index], vector3_1));
    }
  }

  private void netUpdatePhase1(Vector3 newPosition, Vector3 masterPosition)
  {
    this.lineRenderer.SetVertexCount(2);
    this.lineRenderer.SetPosition(0, newPosition);
    this.lineRenderer.SetPosition(1, masterPosition);
    ((Component) this).transform.position = newPosition;
  }

  private void OnDestroy()
  {
    if (Object.op_Inequality((Object) FengGameManagerMKII.instance, (Object) null))
      FengGameManagerMKII.instance.removeHook(this);
    if (Object.op_Inequality((Object) this.myTitan, (Object) null))
      this.myTitan.isHooked = false;
    Object.Destroy((Object) this.rope);
  }

  public void removeMe()
  {
    this.isdestroying = true;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
    {
      PhotonNetwork.Destroy(this.photonView);
      PhotonNetwork.RemoveRPCs(this.photonView);
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      Object.Destroy((Object) this.rope);
      Object.Destroy((Object) ((Component) this).gameObject);
    }
  }

  private void setLinePhase0()
  {
    if (Object.op_Equality((Object) this.master, (Object) null))
    {
      Object.Destroy((Object) this.rope);
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      if (this.nodes.Count <= 0)
        return;
      Vector3 vector3 = Vector3.op_Subtraction(this.myRef.transform.position, (Vector3) this.nodes[0]);
      this.lineRenderer.SetVertexCount(this.nodes.Count);
      for (int index = 0; index <= this.nodes.Count - 1; ++index)
        this.lineRenderer.SetPosition(index, Vector3.op_Addition((Vector3) this.nodes[index], Vector3.op_Multiply(vector3, Mathf.Pow(0.75f, (float) index))));
      if (this.nodes.Count <= 1)
        return;
      this.lineRenderer.SetPosition(1, this.myRef.transform.position);
    }
  }

  [RPC]
  private void setPhase(int value, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet setphase");
    else
      this.phase = value;
  }

  [RPC]
  private void setVelocityAndLeft(Vector3 value, Vector3 v2, bool l, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet setvelocity");
    }
    else
    {
      this.velocity = value;
      this.velocity2 = v2;
      this.left = l;
      ((Component) this).transform.rotation = Quaternion.LookRotation(((Vector3) ref value).normalized);
      this.SetSkin();
    }
  }

  private void Awake()
  {
    this.rope = (GameObject) Object.Instantiate(Resources.Load("rope"));
    this.lineRenderer = this.rope.GetComponent<LineRenderer>();
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addHook(this);
  }

  [RPC]
  private void tieMeTo(Vector3 p, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet tieMeTo");
    else
      ((Component) this).transform.position = p;
  }

  [RPC]
  private void tieMeToOBJ(int id, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "bullet TieMeToObj");
    else
      ((Component) this).transform.parent = ((Component) PhotonView.Find(id)).gameObject.transform;
  }

  public void update()
  {
    if (Object.op_Equality((Object) this.master, (Object) null))
    {
      this.removeMe();
    }
    else
    {
      if (this.isdestroying)
        return;
      if (this.leviMode)
      {
        this.leviShootTime += Time.deltaTime;
        if ((double) this.leviShootTime > 0.40000000596046448)
        {
          this.phase = 2;
          ((Renderer) ((Component) this).gameObject.GetComponent<MeshRenderer>()).enabled = false;
        }
      }
      if (this.phase == 0)
        this.setLinePhase0();
      else if (this.phase == 1)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, this.myRef.transform.position);
        Vector3.op_Addition(((Component) this).transform.position, this.myRef.transform.position);
        Vector3 velocity = this.master.rigidbody.velocity;
        float magnitude1 = ((Vector3) ref velocity).magnitude;
        double magnitude2 = (double) ((Vector3) ref vector3).magnitude;
        int num1 = Mathf.Clamp((int) ((magnitude2 + (double) magnitude1) / 5.0), 2, 6);
        this.lineRenderer.SetVertexCount(num1);
        this.lineRenderer.SetPosition(0, this.myRef.transform.position);
        int num2 = 1;
        float num3 = Mathf.Pow((float) magnitude2, 0.3f);
        for (; num2 < num1; ++num2)
        {
          int num4 = num1 / 2;
          float num5 = (float) Mathf.Abs(num2 - num4);
          float num6 = Mathf.Pow(((float) num4 - num5) / (float) num4, 0.5f);
          float num7 = (float) (((double) num3 + (double) magnitude1) * 0.001500000013038516) * num6;
          this.lineRenderer.SetPosition(num2, Vector3.op_Subtraction(Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Addition(new Vector3(Random.Range(-num7, num7), Random.Range(-num7, num7), Random.Range(-num7, num7)), this.myRef.transform.position), Vector3.op_Multiply(vector3, (float) num2 / (float) num1)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, num3), 0.05f), num6)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(velocity, 1f / 1000f), num6), num3)));
        }
        this.lineRenderer.SetPosition(num1 - 1, ((Component) this).transform.position);
      }
      else if (this.phase == 2)
      {
        if (!this.leviMode)
        {
          this.lineRenderer.SetVertexCount(2);
          this.lineRenderer.SetPosition(0, ((Component) this).transform.position);
          this.lineRenderer.SetPosition(1, this.myRef.transform.position);
          this.killTime += Time.deltaTime * 0.2f;
          this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
          if ((double) this.killTime > 0.10000000149011612)
            this.removeMe();
        }
        else
        {
          Vector3 position = this.master.transform.position;
          Quaternion rotation = this.master.transform.rotation;
          Vector3 eulerAngles = ((Quaternion) ref rotation).eulerAngles;
          this.getSpiral(position, eulerAngles);
          Vector3 vector3_1 = Vector3.op_Subtraction(this.myRef.transform.position, (Vector3) this.spiralNodes[0]);
          this.lineRenderer.SetVertexCount(this.spiralNodes.Count - (int) ((double) this.spiralcount * 0.5));
          for (int index = 0; (double) index <= (double) (this.spiralNodes.Count - 1) - (double) this.spiralcount * 0.5; ++index)
          {
            if (this.spiralcount < 5)
            {
              Vector3 vector3_2 = Vector3.op_Addition((Vector3) this.spiralNodes[index], vector3_1);
              float num = (float) (this.spiralNodes.Count - 1) - (float) this.spiralcount * 0.5f;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_2).\u002Ector(vector3_2.x, (float) ((double) vector3_2.y * (((double) num - (double) index) / (double) num) + (double) ((Component) this).gameObject.transform.position.y * ((double) index / (double) num)), vector3_2.z);
              this.lineRenderer.SetPosition(index, vector3_2);
            }
            else
              this.lineRenderer.SetPosition(index, Vector3.op_Addition((Vector3) this.spiralNodes[index], vector3_1));
          }
        }
      }
      else if (this.phase == 4)
      {
        Transform transform = ((Component) this).gameObject.transform;
        transform.position = Vector3.op_Addition(transform.position, Vector3.op_Addition(this.velocity, Vector3.op_Multiply(this.velocity2, Time.deltaTime)));
        this.nodes.Add((object) new Vector3(((Component) this).gameObject.transform.position.x, ((Component) this).gameObject.transform.position.y, ((Component) this).gameObject.transform.position.z));
        Vector3 vector3 = Vector3.op_Subtraction(this.myRef.transform.position, (Vector3) this.nodes[0]);
        for (int index = 0; index <= this.nodes.Count - 1; ++index)
        {
          this.lineRenderer.SetVertexCount(this.nodes.Count);
          this.lineRenderer.SetPosition(index, Vector3.op_Addition((Vector3) this.nodes[index], Vector3.op_Multiply(vector3, Mathf.Pow(0.5f, (float) index))));
        }
        this.killTime2 += Time.deltaTime;
        if ((double) this.killTime2 > 0.800000011920929)
        {
          this.killTime += Time.deltaTime * 0.2f;
          this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
          if ((double) this.killTime > 0.10000000149011612)
            this.removeMe();
        }
      }
      this.UpdateSkin();
    }
  }
}
