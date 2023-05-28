// Decompiled with JetBrains decompiler
// Type: Cannon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using Settings;
using System;
using UnityEngine;

internal class Cannon : MonoBehaviour
{
  public Transform ballPoint;
  public Transform barrel;
  private Quaternion correctBarrelRot = Quaternion.identity;
  private Vector3 correctPlayerPos = Vector3.zero;
  private Quaternion correctPlayerRot = Quaternion.identity;
  public float currentRot;
  public Transform firingPoint;
  public bool isCannonGround;
  public GameObject myCannonBall;
  public LineRenderer myCannonLine;
  public HERO myHero;
  public string settings;
  public float SmoothingDelay = 5f;

  public void Awake()
  {
    if (!Object.op_Inequality((Object) this.photonView, (Object) null))
      return;
    this.photonView.observed = (Component) this;
    this.barrel = ((Component) this).transform.Find("Barrel");
    this.correctPlayerPos = ((Component) this).transform.position;
    this.correctPlayerRot = ((Component) this).transform.rotation;
    this.correctBarrelRot = this.barrel.rotation;
    if (this.photonView.isMine)
    {
      this.firingPoint = this.barrel.Find("FiringPoint");
      this.ballPoint = this.barrel.Find("BallPoint");
      this.myCannonLine = ((Component) this.ballPoint).GetComponent<LineRenderer>();
      if (((Object) ((Component) this).gameObject).name.Contains("CannonGround"))
        this.isCannonGround = true;
    }
    if (!PhotonNetwork.isMasterClient)
      return;
    PhotonPlayer owner = this.photonView.owner;
    if (FengGameManagerMKII.instance.allowedToCannon.ContainsKey(owner.ID))
    {
      this.settings = FengGameManagerMKII.instance.allowedToCannon[owner.ID].settings;
      this.photonView.RPC("SetSize", PhotonTargets.All, (object) this.settings);
      int viewId = FengGameManagerMKII.instance.allowedToCannon[owner.ID].viewID;
      FengGameManagerMKII.instance.allowedToCannon.Remove(owner.ID);
      CannonPropRegion component = ((Component) PhotonView.Find(viewId)).gameObject.GetComponent<CannonPropRegion>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      component.disabled = true;
      component.destroyed = true;
      PhotonNetwork.Destroy(((Component) component).gameObject);
    }
    else
    {
      if (owner.isLocal || FengGameManagerMKII.instance.restartingMC)
        return;
      FengGameManagerMKII.instance.kickPlayerRC(owner, false, "spawning cannon without request.");
    }
  }

  public void Fire()
  {
    if ((double) this.myHero.skillCDDuration > 0.0)
      return;
    foreach (EnemyCheckCollider componentsInChild in PhotonNetwork.Instantiate("FX/boom2", this.firingPoint.position, this.firingPoint.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>())
      componentsInChild.dmg = 0;
    this.myCannonBall = PhotonNetwork.Instantiate("RCAsset/CannonBallObject", this.ballPoint.position, this.firingPoint.rotation, 0);
    this.myCannonBall.rigidbody.velocity = Vector3.op_Multiply(this.firingPoint.forward, 300f);
    this.myCannonBall.GetComponent<CannonBall>().myHero = this.myHero;
    this.myHero.skillCDDuration = 3.5f;
  }

  public void OnDestroy()
  {
    if (!PhotonNetwork.isMasterClient || FengGameManagerMKII.instance.isRestarting)
      return;
    string[] strArray = this.settings.Split(',');
    if (!(strArray[0] == "photon"))
      return;
    if (strArray.Length > 15)
    {
      GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])), 0);
      go.GetComponent<CannonPropRegion>().settings = this.settings;
      go.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, (object) this.settings);
    }
    else
      PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = this.settings;
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting)
    {
      stream.SendNext((object) ((Component) this).transform.position);
      stream.SendNext((object) ((Component) this).transform.rotation);
      stream.SendNext((object) this.barrel.rotation);
    }
    else
    {
      this.correctPlayerPos = (Vector3) stream.ReceiveNext();
      this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
      this.correctBarrelRot = (Quaternion) stream.ReceiveNext();
    }
  }

  [RPC]
  public void SetSize(string settings, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    string[] strArray = settings.Split(',');
    if (strArray.Length <= 15)
      return;
    float num1 = 1f;
    GameObject gameObject = ((Component) this).gameObject;
    if (strArray[2] != "default")
    {
      if (strArray[2].StartsWith("transparent"))
      {
        float result;
        if (float.TryParse(strArray[2].Substring(11), out result))
          num1 = result;
        foreach (Renderer componentsInChild in gameObject.GetComponentsInChildren<Renderer>())
        {
          componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load("transparent");
          if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
            componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
        }
      }
      else
      {
        foreach (Renderer componentsInChild in gameObject.GetComponentsInChildren<Renderer>())
        {
          if (!((Object) componentsInChild).name.Contains("Line Renderer"))
          {
            componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load(strArray[2]);
            if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
              componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
          }
        }
      }
    }
    float num2 = gameObject.transform.localScale.x * Convert.ToSingle(strArray[3]) - 1f / 1000f;
    float num3 = gameObject.transform.localScale.y * Convert.ToSingle(strArray[4]);
    float num4 = gameObject.transform.localScale.z * Convert.ToSingle(strArray[5]);
    gameObject.transform.localScale = new Vector3(num2, num3, num4);
    if (!(strArray[6] != "0"))
      return;
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num1);
    foreach (MeshFilter componentsInChild in gameObject.GetComponentsInChildren<MeshFilter>())
    {
      Mesh mesh = componentsInChild.mesh;
      Color[] colorArray = new Color[mesh.vertexCount];
      for (int index = 0; index < mesh.vertexCount; ++index)
        colorArray[index] = color;
      mesh.colors = colorArray;
    }
  }

  public void Update()
  {
    if (!this.photonView.isMine)
    {
      ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
      ((Component) this).transform.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
      this.barrel.rotation = Quaternion.Lerp(this.barrel.rotation, this.correctBarrelRot, Time.deltaTime * this.SmoothingDelay);
    }
    else
    {
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector(0.0f, -30f, 0.0f);
      Vector3 vector3_2 = this.ballPoint.position;
      Vector3 vector3_3 = Vector3.op_Multiply(this.ballPoint.forward, 300f);
      float num1 = 40f / ((Vector3) ref vector3_3).magnitude;
      this.myCannonLine.SetWidth(0.5f, 40f);
      this.myCannonLine.SetVertexCount(100);
      for (int index = 0; index < 100; ++index)
      {
        this.myCannonLine.SetPosition(index, vector3_2);
        vector3_2 = Vector3.op_Addition(vector3_2, Vector3.op_Addition(Vector3.op_Multiply(vector3_3, num1), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(0.5f, vector3_1), num1), num1)));
        vector3_3 = Vector3.op_Addition(vector3_3, Vector3.op_Multiply(vector3_1, num1));
      }
      float num2 = 30f;
      if (SettingsManager.InputSettings.Interaction.CannonSlow.GetKey())
        num2 = 5f;
      if (this.isCannonGround)
      {
        if (SettingsManager.InputSettings.General.Forward.GetKey())
        {
          if ((double) this.currentRot <= 32.0)
          {
            this.currentRot += Time.deltaTime * num2;
            this.barrel.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * num2));
          }
        }
        else if (SettingsManager.InputSettings.General.Back.GetKey() && (double) this.currentRot >= -18.0)
        {
          this.currentRot += Time.deltaTime * -num2;
          this.barrel.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * -num2));
        }
        if (SettingsManager.InputSettings.General.Left.GetKey())
          ((Component) this).transform.Rotate(new Vector3(0.0f, Time.deltaTime * -num2, 0.0f));
        else if (SettingsManager.InputSettings.General.Right.GetKey())
          ((Component) this).transform.Rotate(new Vector3(0.0f, Time.deltaTime * num2, 0.0f));
      }
      else
      {
        if (SettingsManager.InputSettings.General.Forward.GetKey())
        {
          if ((double) this.currentRot >= -50.0)
          {
            this.currentRot += Time.deltaTime * -num2;
            this.barrel.Rotate(new Vector3(Time.deltaTime * -num2, 0.0f, 0.0f));
          }
        }
        else if (SettingsManager.InputSettings.General.Back.GetKey() && (double) this.currentRot <= 40.0)
        {
          this.currentRot += Time.deltaTime * num2;
          this.barrel.Rotate(new Vector3(Time.deltaTime * num2, 0.0f, 0.0f));
        }
        if (SettingsManager.InputSettings.General.Left.GetKey())
          ((Component) this).transform.Rotate(new Vector3(0.0f, Time.deltaTime * -num2, 0.0f));
        else if (SettingsManager.InputSettings.General.Right.GetKey())
          ((Component) this).transform.Rotate(new Vector3(0.0f, Time.deltaTime * num2, 0.0f));
      }
      if (SettingsManager.InputSettings.Interaction.CannonFire.GetKey())
      {
        this.Fire();
      }
      else
      {
        if (!SettingsManager.InputSettings.Interaction.Interact.GetKeyDown())
          return;
        if (Object.op_Inequality((Object) this.myHero, (Object) null))
        {
          this.myHero.isCannon = false;
          this.myHero.myCannonRegion = (CannonPropRegion) null;
          ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(((Component) this.myHero).gameObject);
          this.myHero.baseRigidBody.velocity = Vector3.zero;
          this.myHero.photonView.RPC("ReturnFromCannon", PhotonTargets.Others);
          this.myHero.skillCDLast = this.myHero.skillCDLastCannon;
          this.myHero.skillCDDuration = this.myHero.skillCDLast;
        }
        PhotonNetwork.Destroy(((Component) this).gameObject);
      }
    }
  }
}
