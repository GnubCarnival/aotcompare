// Decompiled with JetBrains decompiler
// Type: PVPcheckPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using System.Collections;
using UnityEngine;

public class PVPcheckPoint : MonoBehaviour
{
  private bool annie;
  public GameObject[] chkPtNextArr;
  public GameObject[] chkPtPreviousArr;
  public static ArrayList chkPts;
  private float getPtsInterval = 20f;
  private float getPtsTimer;
  public bool hasAnnie;
  private float hitTestR = 15f;
  public GameObject humanCyc;
  public float humanPt;
  public float humanPtMax = 40f;
  public int id;
  public bool isBase;
  public int normalTitanRate = 70;
  private bool playerOn;
  public float size = 1f;
  private float spawnTitanTimer;
  public CheckPointState state;
  private GameObject supply;
  private float syncInterval = 0.6f;
  private float syncTimer;
  public GameObject titanCyc;
  public float titanInterval = 30f;
  private bool titanOn;
  public float titanPt;
  public float titanPtMax = 40f;
  private float _lastTitanPt;
  private float _lastHumanPt;

  [RPC]
  private void changeHumanPt(float pt) => this.humanPt = pt;

  [RPC]
  private void changeState(int num)
  {
    if (num == 0)
      this.state = CheckPointState.Non;
    if (num == 1)
      this.state = CheckPointState.Human;
    if (num != 2)
      return;
    this.state = CheckPointState.Titan;
  }

  [RPC]
  private void changeTitanPt(float pt) => this.titanPt = pt;

  private void checkIfBeingCapture()
  {
    this.playerOn = false;
    this.titanOn = false;
    GameObject[] gameObjectsWithTag1 = GameObject.FindGameObjectsWithTag("Player");
    GameObject[] gameObjectsWithTag2 = GameObject.FindGameObjectsWithTag("titan");
    for (int index = 0; index < gameObjectsWithTag1.Length; ++index)
    {
      if ((double) Vector3.Distance(gameObjectsWithTag1[index].transform.position, ((Component) this).transform.position) < (double) this.hitTestR)
      {
        this.playerOn = true;
        if (this.state == CheckPointState.Human && gameObjectsWithTag1[index].GetPhotonView().isMine)
        {
          if (Object.op_Inequality((Object) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint, (Object) ((Component) this).gameObject))
          {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = ((Component) this).gameObject;
            GameObject.Find("Chatroom").GetComponent<InRoomChat>().addLINE("<color=#A8FF24>Respawn point changed to point" + this.id.ToString() + "</color>");
            break;
          }
          break;
        }
      }
    }
    for (int index = 0; index < gameObjectsWithTag2.Length; ++index)
    {
      if ((double) Vector3.Distance(gameObjectsWithTag2[index].transform.position, ((Component) this).transform.position) < (double) this.hitTestR + 5.0 && (Object.op_Equality((Object) gameObjectsWithTag2[index].GetComponent<TITAN>(), (Object) null) || !gameObjectsWithTag2[index].GetComponent<TITAN>().hasDie))
      {
        this.titanOn = true;
        if (this.state == CheckPointState.Titan && gameObjectsWithTag2[index].GetPhotonView().isMine && Object.op_Inequality((Object) gameObjectsWithTag2[index].GetComponent<TITAN>(), (Object) null) && gameObjectsWithTag2[index].GetComponent<TITAN>().nonAI)
        {
          if (!Object.op_Inequality((Object) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint, (Object) ((Component) this).gameObject))
            break;
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = ((Component) this).gameObject;
          GameObject.Find("Chatroom").GetComponent<InRoomChat>().addLINE("<color=#A8FF24>Respawn point changed to point" + this.id.ToString() + "</color>");
          break;
        }
      }
    }
  }

  private bool checkIfHumanWins()
  {
    for (int index = 0; index < PVPcheckPoint.chkPts.Count; ++index)
    {
      if ((PVPcheckPoint.chkPts[index] as PVPcheckPoint).state != CheckPointState.Human)
        return false;
    }
    return true;
  }

  private bool checkIfTitanWins()
  {
    for (int index = 0; index < PVPcheckPoint.chkPts.Count; ++index)
    {
      if ((PVPcheckPoint.chkPts[index] as PVPcheckPoint).state != CheckPointState.Titan)
        return false;
    }
    return true;
  }

  private float getHeight(Vector3 pt)
  {
    LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    RaycastHit raycastHit;
    return Physics.Raycast(pt, Vector3.op_UnaryNegation(Vector3.up), ref raycastHit, 1000f, ((LayerMask) ref layerMask).value) ? ((RaycastHit) ref raycastHit).point.y : 0.0f;
  }

  public string getStateString()
  {
    if (this.state == CheckPointState.Human)
      return "[" + ColorSet.color_human + "]H[-]";
    return this.state == CheckPointState.Titan ? "[" + ColorSet.color_titan_player + "]T[-]" : "[" + ColorSet.color_D + "]_[-]";
  }

  private void humanGetsPoint()
  {
    if ((double) this.humanPt >= (double) this.humanPtMax)
    {
      this.humanPt = this.humanPtMax;
      this.titanPt = 0.0f;
      this.syncPts();
      this.state = CheckPointState.Human;
      this.photonView.RPC("changeState", PhotonTargets.All, (object) 1);
      if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName != "The City I")
        this.supply = PhotonNetwork.Instantiate("aot_supply", Vector3.op_Subtraction(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, ((Component) this).transform.position.y - this.getHeight(((Component) this).transform.position))), ((Component) this).transform.rotation, 0);
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().PVPhumanScore += 2;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkPVPpts();
      if (!this.checkIfHumanWins())
        return;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
    }
    else
      this.humanPt += Time.deltaTime;
  }

  private void humanLosePoint()
  {
    if ((double) this.humanPt <= 0.0)
      return;
    this.humanPt -= Time.deltaTime * 3f;
    if ((double) this.humanPt > 0.0)
      return;
    this.humanPt = 0.0f;
    this.syncPts();
    if (this.state == CheckPointState.Titan)
      return;
    this.state = CheckPointState.Non;
    this.photonView.RPC("changeState", PhotonTargets.Others, (object) 0);
  }

  private void newTitan()
  {
    GameObject gameObject = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().spawnTitan(this.normalTitanRate, Vector3.op_Subtraction(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, ((Component) this).transform.position.y - this.getHeight(((Component) this).transform.position))), ((Component) this).transform.rotation);
    if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "The City I")
      gameObject.GetComponent<TITAN>().chaseDistance = 120f;
    else
      gameObject.GetComponent<TITAN>().chaseDistance = 200f;
    gameObject.GetComponent<TITAN>().PVPfromCheckPt = this;
  }

  private void Start()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      Object.Destroy((Object) ((Component) this).gameObject);
    else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE)
    {
      if (this.photonView.isMine)
        Object.Destroy((Object) ((Component) this).gameObject);
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      PVPcheckPoint.chkPts.Add((object) this);
      IComparer comparer = (IComparer) new IComparerPVPchkPtID();
      PVPcheckPoint.chkPts.Sort(comparer);
      if ((double) this.humanPt == (double) this.humanPtMax)
      {
        this.state = CheckPointState.Human;
        if (this.photonView.isMine && LevelInfo.getInfo(FengGameManagerMKII.level).mapName != "The City I")
          this.supply = PhotonNetwork.Instantiate("aot_supply", Vector3.op_Subtraction(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, ((Component) this).transform.position.y - this.getHeight(((Component) this).transform.position))), ((Component) this).transform.rotation, 0);
      }
      else if (this.photonView.isMine && !this.hasAnnie)
      {
        if (Random.Range(0, 100) < 50)
        {
          int num = Random.Range(1, 2);
          for (int index = 0; index < num; ++index)
            this.newTitan();
        }
        if (this.isBase)
          this.newTitan();
      }
      if ((double) this.titanPt == (double) this.titanPtMax)
        this.state = CheckPointState.Titan;
      this.hitTestR = 15f * this.size;
      ((Component) this).transform.localScale = new Vector3(this.size, this.size, this.size);
    }
  }

  private void syncPts()
  {
    if ((double) this.titanPt != (double) this._lastTitanPt)
    {
      this.photonView.RPC("changeTitanPt", PhotonTargets.Others, (object) this.titanPt);
      this._lastTitanPt = this.titanPt;
    }
    if ((double) this.humanPt == (double) this._lastHumanPt)
      return;
    this.photonView.RPC("changeHumanPt", PhotonTargets.Others, (object) this.humanPt);
    this._lastHumanPt = this.humanPt;
  }

  public void OnPhotonPlayerConnected(PhotonPlayer player)
  {
    if (!PhotonNetwork.isMasterClient)
      return;
    object[] objArray1 = new object[1]
    {
      (object) this.titanPt
    };
    this.photonView.RPC("changeTitanPt", player, objArray1);
    object[] objArray2 = new object[1]
    {
      (object) this.humanPt
    };
    this.photonView.RPC("changeHumanPt", player, objArray2);
  }

  private void titanGetsPoint()
  {
    if ((double) this.titanPt >= (double) this.titanPtMax)
    {
      this.titanPt = this.titanPtMax;
      this.humanPt = 0.0f;
      this.syncPts();
      if (this.state == CheckPointState.Human && Object.op_Inequality((Object) this.supply, (Object) null))
        PhotonNetwork.Destroy(this.supply);
      this.state = CheckPointState.Titan;
      this.photonView.RPC("changeState", PhotonTargets.All, (object) 2);
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().PVPtitanScore += 2;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkPVPpts();
      if (this.checkIfTitanWins())
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
      if (this.hasAnnie)
      {
        if (!this.annie)
        {
          this.annie = true;
          PhotonNetwork.Instantiate("FEMALE_TITAN", Vector3.op_Subtraction(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, ((Component) this).transform.position.y - this.getHeight(((Component) this).transform.position))), ((Component) this).transform.rotation, 0);
        }
        else
          this.newTitan();
      }
      else
        this.newTitan();
    }
    else
      this.titanPt += Time.deltaTime;
  }

  private void titanLosePoint()
  {
    if ((double) this.titanPt <= 0.0)
      return;
    this.titanPt -= Time.deltaTime * 3f;
    if ((double) this.titanPt > 0.0)
      return;
    this.titanPt = 0.0f;
    this.syncPts();
    if (this.state == CheckPointState.Human)
      return;
    this.state = CheckPointState.Non;
    this.photonView.RPC("changeState", PhotonTargets.All, (object) 0);
  }

  private void Update()
  {
    float num1 = this.humanPt / this.humanPtMax;
    float num2 = this.titanPt / this.titanPtMax;
    if (!this.photonView.isMine)
    {
      float num3 = this.humanPt / this.humanPtMax;
      float num4 = this.titanPt / this.titanPtMax;
      this.humanCyc.transform.localScale = new Vector3(num3, num3, 1f);
      this.titanCyc.transform.localScale = new Vector3(num4, num4, 1f);
      this.syncTimer += Time.deltaTime;
      if ((double) this.syncTimer <= (double) this.syncInterval)
        return;
      this.syncTimer = 0.0f;
      this.checkIfBeingCapture();
    }
    else
    {
      if (this.state == CheckPointState.Non)
      {
        if (this.playerOn && !this.titanOn)
        {
          this.humanGetsPoint();
          this.titanLosePoint();
        }
        else if (this.titanOn && !this.playerOn)
        {
          this.titanGetsPoint();
          this.humanLosePoint();
        }
        else
        {
          this.humanLosePoint();
          this.titanLosePoint();
        }
      }
      else if (this.state == CheckPointState.Human)
      {
        if (this.titanOn && !this.playerOn)
          this.titanGetsPoint();
        else
          this.titanLosePoint();
        this.getPtsTimer += Time.deltaTime;
        if ((double) this.getPtsTimer > (double) this.getPtsInterval)
        {
          this.getPtsTimer = 0.0f;
          if (!this.isBase)
            ++GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().PVPhumanScore;
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkPVPpts();
        }
      }
      else if (this.state == CheckPointState.Titan)
      {
        if (this.playerOn && !this.titanOn)
          this.humanGetsPoint();
        else
          this.humanLosePoint();
        this.getPtsTimer += Time.deltaTime;
        if ((double) this.getPtsTimer > (double) this.getPtsInterval)
        {
          this.getPtsTimer = 0.0f;
          if (!this.isBase)
            ++GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().PVPtitanScore;
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkPVPpts();
        }
        this.spawnTitanTimer += Time.deltaTime;
        if ((double) this.spawnTitanTimer > (double) this.titanInterval)
        {
          this.spawnTitanTimer = 0.0f;
          if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "The City I")
          {
            if (GameObject.FindGameObjectsWithTag("titan").Length < 12)
              this.newTitan();
          }
          else if (GameObject.FindGameObjectsWithTag("titan").Length < 20)
            this.newTitan();
        }
      }
      this.syncTimer += Time.deltaTime;
      if ((double) this.syncTimer > (double) this.syncInterval)
      {
        this.syncTimer = 0.0f;
        this.checkIfBeingCapture();
        this.syncPts();
      }
      float num5 = this.humanPt / this.humanPtMax;
      float num6 = this.titanPt / this.titanPtMax;
      this.humanCyc.transform.localScale = new Vector3(num5, num5, 1f);
      this.titanCyc.transform.localScale = new Vector3(num6, num6, 1f);
    }
  }

  public GameObject chkPtNext => this.chkPtNextArr.Length == 0 ? (GameObject) null : this.chkPtNextArr[Random.Range(0, this.chkPtNextArr.Length)];

  public GameObject chkPtPrevious => this.chkPtPreviousArr.Length == 0 ? (GameObject) null : this.chkPtPreviousArr[Random.Range(0, this.chkPtPreviousArr.Length)];
}
