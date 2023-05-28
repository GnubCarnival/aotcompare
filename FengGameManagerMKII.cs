// Decompiled with JetBrains decompiler
// Type: FengGameManagerMKII
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using CustomSkins;
using ExitGames.Client.Photon;
using GameManagers;
using Photon;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UI;
using UnityEngine;

internal class FengGameManagerMKII : MonoBehaviour
{
  public static bool JustLeftRoom = false;
  public Dictionary<int, CannonValues> allowedToCannon;
  public Dictionary<string, Texture2D> assetCacheTextures;
  public static Hashtable banHash;
  public static Hashtable boolVariables;
  public static Dictionary<string, GameObject> CachedPrefabs;
  private ArrayList chatContent;
  public InRoomChat chatRoom;
  public GameObject checkpoint;
  private ArrayList cT;
  public static string currentLevel;
  private float currentSpeed;
  public static bool customLevelLoaded;
  public int cyanKills;
  public int difficulty;
  public float distanceSlider;
  private bool endRacing;
  private ArrayList eT;
  public static Hashtable floatVariables;
  private ArrayList fT;
  private float gameEndCD;
  private float gameEndTotalCDtime = 9f;
  public bool gameStart;
  private bool gameTimesUp;
  public static Hashtable globalVariables;
  public List<GameObject> groundList;
  public static bool hasLogged;
  private ArrayList heroes;
  public static Hashtable heroHash;
  private int highestwave = 1;
  private ArrayList hooks;
  private int humanScore;
  public static List<int> ignoreList;
  public static Hashtable imatitan;
  public static FengGameManagerMKII instance;
  public static Hashtable intVariables;
  public static bool isAssetLoaded;
  public bool isFirstLoad;
  private bool isLosing;
  private bool isPlayer1Winning;
  private bool isPlayer2Winning;
  public bool isRecompiling;
  public bool isRestarting;
  public bool isSpawning;
  public bool isUnloading;
  private bool isWinning;
  public bool justSuicide;
  private ArrayList kicklist;
  private ArrayList killInfoGO = new ArrayList();
  public static bool LAN;
  public static string level = string.Empty;
  public List<string[]> levelCache;
  public static Hashtable[] linkHash;
  private string localRacingResult;
  public static bool logicLoaded;
  public static int loginstate;
  public int magentaKills;
  private IN_GAME_MAIN_CAMERA mainCamera;
  public static bool masterRC;
  public int maxPlayers;
  private float maxSpeed;
  public float mouseSlider;
  private string myLastHero;
  private string myLastRespawnTag = "playerRespawn";
  public float myRespawnTime;
  public string name;
  public static string nameField;
  public bool needChooseSide;
  public static bool noRestart;
  public static string oldScript;
  public static string oldScriptLogic;
  public static string passwordField;
  public float pauseWaitTime;
  public string playerList;
  public List<Vector3> playerSpawnsC;
  public List<Vector3> playerSpawnsM;
  public List<PhotonPlayer> playersRPC;
  public static Hashtable playerVariables;
  public Dictionary<string, int[]> PreservedPlayerKDR;
  public static string PrivateServerAuthPass;
  public static string privateServerField;
  public static string privateLobbyField;
  public int PVPhumanScore;
  private int PVPhumanScoreMax = 200;
  public int PVPtitanScore;
  private int PVPtitanScoreMax = 200;
  public float qualitySlider;
  public List<GameObject> racingDoors;
  private ArrayList racingResult;
  public Vector3 racingSpawnPoint;
  public bool racingSpawnPointSet;
  public static AssetBundle RCassets;
  public static Hashtable RCEvents;
  public static Hashtable RCRegions;
  public static Hashtable RCRegionTriggers;
  public static Hashtable RCVariableNames;
  public List<float> restartCount;
  public bool restartingBomb;
  public bool restartingEren;
  public bool restartingHorse;
  public bool restartingMC;
  public bool restartingTitan;
  public float retryTime;
  public float roundTime;
  public Vector2 scroll;
  public Vector2 scroll2;
  public GameObject selectedObj;
  public static object[] settingsOld;
  private int single_kills;
  private int single_maxDamage;
  private int single_totalDamage;
  public List<GameObject> spectateSprites;
  private bool startRacing;
  public static Hashtable stringVariables;
  private int[] teamScores;
  private int teamWinner;
  public Texture2D textureBackgroundBlack;
  public Texture2D textureBackgroundBlue;
  public int time = 600;
  private float timeElapse;
  private float timeTotalServer;
  private ArrayList titans;
  private int titanScore;
  public List<TitanSpawner> titanSpawners;
  public List<Vector3> titanSpawns;
  public static Hashtable titanVariables;
  public float transparencySlider;
  public GameObject ui;
  public float updateTime;
  public static string usernameField;
  public int wave = 1;
  public Dictionary<string, Material> customMapMaterials;
  public float LastRoomPropertyCheckTime;
  private SkyboxCustomSkinLoader _skyboxCustomSkinLoader;
  private ForestCustomSkinLoader _forestCustomSkinLoader;
  private CityCustomSkinLoader _cityCustomSkinLoader;
  private CustomLevelCustomSkinLoader _customLevelCustomSkinLoader;

  public void OnJoinedLobby()
  {
    if (FengGameManagerMKII.JustLeftRoom)
    {
      PhotonNetwork.Disconnect();
      FengGameManagerMKII.JustLeftRoom = false;
    }
    else
    {
      if (!Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null) || !Object.op_Inequality((Object) ((Component) UIManager.CurrentMenu).GetComponent<MainMenu>(), (Object) null))
        return;
      ((Component) UIManager.CurrentMenu).GetComponent<MainMenu>().ShowMultiplayerRoomListPopup();
    }
  }

  private void Awake()
  {
    this._skyboxCustomSkinLoader = ((Component) this).gameObject.AddComponent<SkyboxCustomSkinLoader>();
    this._forestCustomSkinLoader = ((Component) this).gameObject.AddComponent<ForestCustomSkinLoader>();
    this._cityCustomSkinLoader = ((Component) this).gameObject.AddComponent<CityCustomSkinLoader>();
    this._customLevelCustomSkinLoader = ((Component) this).gameObject.AddComponent<CustomLevelCustomSkinLoader>();
    ((Component) this).gameObject.AddComponent<CustomRPCManager>();
  }

  private string getMaterialHash(string material, string x, string y) => material + "," + x + "," + y;

  public void addCamera(IN_GAME_MAIN_CAMERA c) => this.mainCamera = c;

  public void addCT(COLOSSAL_TITAN titan) => this.cT.Add((object) titan);

  public void addET(TITAN_EREN hero) => this.eT.Add((object) hero);

  public void addFT(FEMALE_TITAN titan) => this.fT.Add((object) titan);

  public void addHero(HERO hero) => this.heroes.Add((object) hero);

  public void addHook(Bullet h) => this.hooks.Add((object) h);

  public void addTime(float time) => this.timeTotalServer -= time;

  public void addTitan(TITAN titan) => this.titans.Add((object) titan);

  private void cache()
  {
    ClothFactory.ClearClothCache();
    this.chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
    this.playersRPC.Clear();
    this.titanSpawners.Clear();
    this.groundList.Clear();
    this.PreservedPlayerKDR = new Dictionary<string, int[]>();
    FengGameManagerMKII.noRestart = false;
    this.isSpawning = false;
    this.retryTime = 0.0f;
    FengGameManagerMKII.logicLoaded = false;
    FengGameManagerMKII.customLevelLoaded = true;
    this.isUnloading = false;
    this.isRecompiling = false;
    Time.timeScale = 1f;
    Camera.main.farClipPlane = 1500f;
    this.pauseWaitTime = 0.0f;
    this.spectateSprites = new List<GameObject>();
    this.isRestarting = false;
    if (PhotonNetwork.isMasterClient)
      this.StartCoroutine(this.WaitAndResetRestarts());
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
    {
      this.roundTime = 0.0f;
      if (FengGameManagerMKII.level.StartsWith("Custom"))
        FengGameManagerMKII.customLevelLoaded = false;
      if (PhotonNetwork.isMasterClient)
      {
        if (this.isFirstLoad)
          this.setGameSettings(this.checkGameGUI());
        if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
          this.StartCoroutine(this.respawnE((float) SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value));
      }
    }
    if (SettingsManager.UISettings.GameFeed.Value)
      this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round Start.");
    this.isFirstLoad = false;
    this.RecompilePlayerList(0.5f);
  }

  [RPC]
  private void Chat(string content, string sender, PhotonMessageInfo info)
  {
    if (sender != string.Empty)
      content = sender + ":" + content;
    content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
    this.chatRoom.addLINE(content);
  }

  [RPC]
  private void ChatPM(string sender, string content, PhotonMessageInfo info)
  {
    content = sender + ":" + content;
    content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
    this.chatRoom.addLINE(content);
  }

  private Hashtable checkGameGUI()
  {
    Hashtable hashtable = new Hashtable();
    LegacyGameSettings legacyGameSettingsUi = SettingsManager.LegacyGameSettingsUI;
    if (legacyGameSettingsUi.InfectionModeEnabled.Value)
    {
      legacyGameSettingsUi.BombModeEnabled.Value = false;
      legacyGameSettingsUi.TeamMode.Value = 0;
      legacyGameSettingsUi.PointModeEnabled.Value = false;
      legacyGameSettingsUi.BladePVP.Value = 0;
      if (legacyGameSettingsUi.InfectionModeAmount.Value > PhotonNetwork.countOfPlayers)
        legacyGameSettingsUi.InfectionModeAmount.Value = 1;
      ((Dictionary<object, object>) hashtable).Add((object) "infection", (object) legacyGameSettingsUi.InfectionModeAmount.Value);
      if (!SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || SettingsManager.LegacyGameSettings.InfectionModeAmount.Value != legacyGameSettingsUi.InfectionModeAmount.Value)
      {
        ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Clear();
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          Hashtable propertiesToSet = new Hashtable();
          ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
          player.SetCustomProperties(propertiesToSet);
        }
        int length = PhotonNetwork.playerList.Length;
        int num = legacyGameSettingsUi.InfectionModeAmount.Value;
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          if (length > 0 && (double) Random.Range(0.0f, 1f) <= (double) num / (double) length)
          {
            Hashtable propertiesToSet = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
            player.SetCustomProperties(propertiesToSet);
            ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Add((object) player.ID, (object) 2);
            --num;
          }
          --length;
        }
      }
    }
    if (legacyGameSettingsUi.BombModeEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "bomb", (object) 1);
    if (legacyGameSettingsUi.BombModeCeiling.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "bombCeiling", (object) 1);
    else
      ((Dictionary<object, object>) hashtable).Add((object) "bombCeiling", (object) 0);
    if (legacyGameSettingsUi.BombModeInfiniteGas.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "bombInfiniteGas", (object) 1);
    else
      ((Dictionary<object, object>) hashtable).Add((object) "bombInfiniteGas", (object) 0);
    if (legacyGameSettingsUi.GlobalHideNames.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "globalHideNames", (object) 1);
    if (legacyGameSettingsUi.GlobalMinimapDisable.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "globalDisableMinimap", (object) 1);
    if (legacyGameSettingsUi.TeamMode.Value > 0)
    {
      ((Dictionary<object, object>) hashtable).Add((object) "team", (object) legacyGameSettingsUi.TeamMode.Value);
      if (SettingsManager.LegacyGameSettings.TeamMode.Value != legacyGameSettingsUi.TeamMode.Value)
      {
        int num = 1;
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          switch (num)
          {
            case 1:
              this.photonView.RPC("setTeamRPC", player, (object) 1);
              num = 2;
              break;
            case 2:
              this.photonView.RPC("setTeamRPC", player, (object) 2);
              num = 1;
              break;
          }
        }
      }
    }
    if (legacyGameSettingsUi.PointModeEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "point", (object) legacyGameSettingsUi.PointModeAmount.Value);
    if (!legacyGameSettingsUi.RockThrowEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "rock", (object) 1);
    if (legacyGameSettingsUi.TitanExplodeEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "explode", (object) legacyGameSettingsUi.TitanExplodeRadius.Value);
    if (legacyGameSettingsUi.TitanHealthMode.Value > 0)
    {
      ((Dictionary<object, object>) hashtable).Add((object) "healthMode", (object) legacyGameSettingsUi.TitanHealthMode.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "healthLower", (object) legacyGameSettingsUi.TitanHealthMin.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "healthUpper", (object) legacyGameSettingsUi.TitanHealthMax.Value);
    }
    if (legacyGameSettingsUi.KickShifters.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "eren", (object) 1);
    if (legacyGameSettingsUi.TitanNumberEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "titanc", (object) legacyGameSettingsUi.TitanNumber.Value);
    if (legacyGameSettingsUi.TitanArmorEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "damage", (object) legacyGameSettingsUi.TitanArmor.Value);
    if (legacyGameSettingsUi.TitanSizeEnabled.Value)
    {
      ((Dictionary<object, object>) hashtable).Add((object) "sizeMode", (object) 1);
      ((Dictionary<object, object>) hashtable).Add((object) "sizeLower", (object) legacyGameSettingsUi.TitanSizeMin.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "sizeUpper", (object) legacyGameSettingsUi.TitanSizeMax.Value);
    }
    if (legacyGameSettingsUi.TitanSpawnEnabled.Value)
    {
      if ((double) legacyGameSettingsUi.TitanSpawnNormal.Value + (double) legacyGameSettingsUi.TitanSpawnAberrant.Value + (double) legacyGameSettingsUi.TitanSpawnCrawler.Value + (double) legacyGameSettingsUi.TitanSpawnJumper.Value + (double) legacyGameSettingsUi.TitanSpawnPunk.Value > 100.0)
      {
        legacyGameSettingsUi.TitanSpawnNormal.Value = 20f;
        legacyGameSettingsUi.TitanSpawnAberrant.Value = 20f;
        legacyGameSettingsUi.TitanSpawnCrawler.Value = 20f;
        legacyGameSettingsUi.TitanSpawnJumper.Value = 20f;
        legacyGameSettingsUi.TitanSpawnPunk.Value = 20f;
      }
      ((Dictionary<object, object>) hashtable).Add((object) "spawnMode", (object) 1);
      ((Dictionary<object, object>) hashtable).Add((object) "nRate", (object) legacyGameSettingsUi.TitanSpawnNormal.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "aRate", (object) legacyGameSettingsUi.TitanSpawnAberrant.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "jRate", (object) legacyGameSettingsUi.TitanSpawnJumper.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "cRate", (object) legacyGameSettingsUi.TitanSpawnCrawler.Value);
      ((Dictionary<object, object>) hashtable).Add((object) "pRate", (object) legacyGameSettingsUi.TitanSpawnPunk.Value);
    }
    if (legacyGameSettingsUi.AllowHorses.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "horse", (object) 1);
    if (legacyGameSettingsUi.TitanPerWavesEnabled.Value)
    {
      ((Dictionary<object, object>) hashtable).Add((object) "waveModeOn", (object) 1);
      ((Dictionary<object, object>) hashtable).Add((object) "waveModeNum", (object) legacyGameSettingsUi.TitanPerWaves.Value);
    }
    if (legacyGameSettingsUi.FriendlyMode.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "friendly", (object) 1);
    if (legacyGameSettingsUi.BladePVP.Value > 0)
      ((Dictionary<object, object>) hashtable).Add((object) "pvp", (object) legacyGameSettingsUi.BladePVP.Value);
    if (legacyGameSettingsUi.TitanMaxWavesEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "maxwave", (object) legacyGameSettingsUi.TitanMaxWaves.Value);
    if (legacyGameSettingsUi.EndlessRespawnEnabled.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "endless", (object) legacyGameSettingsUi.EndlessRespawnTime.Value);
    if (legacyGameSettingsUi.Motd.Value != string.Empty)
      ((Dictionary<object, object>) hashtable).Add((object) "motd", (object) legacyGameSettingsUi.Motd.Value);
    if (!legacyGameSettingsUi.AHSSAirReload.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "ahssReload", (object) 1);
    if (!legacyGameSettingsUi.PunksEveryFive.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "punkWaves", (object) 1);
    if (legacyGameSettingsUi.CannonsFriendlyFire.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "deadlycannons", (object) 1);
    if (legacyGameSettingsUi.RacingEndless.Value)
      ((Dictionary<object, object>) hashtable).Add((object) "asoracing", (object) 1);
    ((Dictionary<object, object>) hashtable).Add((object) "racingStartTime", (object) legacyGameSettingsUi.RacingStartTime.Value);
    LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
    legacyGameSettings.PreserveKDR.Value = legacyGameSettingsUi.PreserveKDR.Value;
    legacyGameSettings.TitanSpawnCap.Value = legacyGameSettingsUi.TitanSpawnCap.Value;
    legacyGameSettings.GameType.Value = legacyGameSettingsUi.GameType.Value;
    legacyGameSettings.LevelScript.Value = legacyGameSettingsUi.LevelScript.Value;
    legacyGameSettings.LogicScript.Value = legacyGameSettingsUi.LogicScript.Value;
    return hashtable;
  }

  private bool checkIsTitanAllDie()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
    {
      if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null) && !gameObject.GetComponent<TITAN>().hasDie || Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null))
        return false;
    }
    return true;
  }

  public void checkPVPpts()
  {
    if (this.PVPtitanScore >= this.PVPtitanScoreMax)
    {
      this.PVPtitanScore = this.PVPtitanScoreMax;
      this.gameLose2();
    }
    else
    {
      if (this.PVPhumanScore < this.PVPhumanScoreMax)
        return;
      this.PVPhumanScore = this.PVPhumanScoreMax;
      this.gameWin2();
    }
  }

  [RPC]
  private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    switch (gametype)
    {
      case 0:
        IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.KILL_TITAN;
        break;
      case 1:
        IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.SURVIVE_MODE;
        break;
      case 2:
        IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.PVP_AHSS;
        break;
      case 3:
        IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.RACING;
        break;
      case 4:
        IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.None;
        break;
    }
    if (!info.sender.isMasterClient || link.Length <= 6)
      return;
    this.StartCoroutine(this.clearlevelE(link));
  }

  private IEnumerator clearlevelE(string[] skybox)
  {
    FengGameManagerMKII fengGameManagerMkii = this;
    if (fengGameManagerMkii.IsValidSkybox(skybox))
    {
      yield return (object) fengGameManagerMkii.StartCoroutine(fengGameManagerMkii._skyboxCustomSkinLoader.LoadSkinsFromRPC((object[]) skybox));
      yield return (object) fengGameManagerMkii.StartCoroutine(fengGameManagerMkii._customLevelCustomSkinLoader.LoadSkinsFromRPC((object[]) skybox));
    }
    else
      SkyboxCustomSkinLoader.SkyboxMaterial = (Material) null;
    fengGameManagerMkii.StartCoroutine(fengGameManagerMkii.reloadSky());
  }

  public void compileScript(string str)
  {
    string[] strArray1 = str.Replace(" ", string.Empty).Split(new string[2]
    {
      "\n",
      "\r\n"
    }, StringSplitOptions.RemoveEmptyEntries);
    Hashtable hashtable = new Hashtable();
    int num1 = 0;
    int num2 = 0;
    bool flag = false;
    for (int index = 0; index < strArray1.Length; ++index)
    {
      if (strArray1[index] == "{")
        ++num1;
      else if (strArray1[index] == "}")
      {
        ++num2;
      }
      else
      {
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        foreach (char ch in strArray1[index])
        {
          switch (ch)
          {
            case '"':
              ++num5;
              break;
            case '(':
              ++num3;
              break;
            case ')':
              ++num4;
              break;
          }
        }
        if (num3 != num4)
        {
          this.chatRoom.addLINE("Script Error: Parentheses not equal! (line " + (index + 1).ToString() + ")");
          flag = true;
        }
        if (num5 % 2 != 0)
        {
          this.chatRoom.addLINE("Script Error: Quotations not equal! (line " + (index + 1).ToString() + ")");
          flag = true;
        }
      }
    }
    if (num1 != num2)
    {
      this.chatRoom.addLINE("Script Error: Bracket count not equivalent!");
      flag = true;
    }
    if (flag)
      return;
    try
    {
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        if (strArray1[index1].StartsWith("On") && strArray1[index1 + 1] == "{")
        {
          int key = index1;
          int num6 = index1 + 2;
          int num7 = 0;
          for (int index2 = index1 + 2; index2 < strArray1.Length; ++index2)
          {
            if (strArray1[index2] == "{")
              ++num7;
            if (strArray1[index2] == "}")
            {
              if (num7 > 0)
              {
                --num7;
              }
              else
              {
                num6 = index2 - 1;
                index2 = strArray1.Length;
              }
            }
          }
          ((Dictionary<object, object>) hashtable).Add((object) key, (object) num6);
          index1 = num6;
        }
      }
      foreach (int key1 in ((Dictionary<object, object>) hashtable).Keys)
      {
        string str1 = strArray1[key1];
        int num8 = (int) hashtable[(object) key1];
        string[] stringArray = new string[num8 - key1 + 1];
        int index3 = 0;
        for (int index4 = key1; index4 <= num8; ++index4)
        {
          stringArray[index3] = strArray1[index4];
          ++index3;
        }
        RCEvent block = this.parseBlock(stringArray, 0, 0, (RCCondition) null);
        if (str1.StartsWith("OnPlayerEnterRegion"))
        {
          int num9 = str1.IndexOf('[');
          int num10 = str1.IndexOf(']');
          string key2 = str1.Substring(num9 + 2, num10 - num9 - 3);
          int num11 = str1.IndexOf('(');
          int num12 = str1.IndexOf(')');
          string str2 = str1.Substring(num11 + 2, num12 - num11 - 3);
          if (((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).ContainsKey((object) key2))
          {
            RegionTrigger rcRegionTrigger = (RegionTrigger) FengGameManagerMKII.RCRegionTriggers[(object) key2];
            rcRegionTrigger.playerEventEnter = block;
            rcRegionTrigger.myName = key2;
            FengGameManagerMKII.RCRegionTriggers[(object) key2] = (object) rcRegionTrigger;
          }
          else
          {
            RegionTrigger regionTrigger = new RegionTrigger()
            {
              playerEventEnter = block,
              myName = key2
            };
            ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Add((object) key2, (object) regionTrigger);
          }
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) ("OnPlayerEnterRegion[" + key2 + "]"), (object) str2);
        }
        else if (str1.StartsWith("OnPlayerLeaveRegion"))
        {
          int num13 = str1.IndexOf('[');
          int num14 = str1.IndexOf(']');
          string key3 = str1.Substring(num13 + 2, num14 - num13 - 3);
          int num15 = str1.IndexOf('(');
          int num16 = str1.IndexOf(')');
          string str3 = str1.Substring(num15 + 2, num16 - num15 - 3);
          if (((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).ContainsKey((object) key3))
          {
            RegionTrigger rcRegionTrigger = (RegionTrigger) FengGameManagerMKII.RCRegionTriggers[(object) key3];
            rcRegionTrigger.playerEventExit = block;
            rcRegionTrigger.myName = key3;
            FengGameManagerMKII.RCRegionTriggers[(object) key3] = (object) rcRegionTrigger;
          }
          else
          {
            RegionTrigger regionTrigger = new RegionTrigger()
            {
              playerEventExit = block,
              myName = key3
            };
            ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Add((object) key3, (object) regionTrigger);
          }
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) ("OnPlayerExitRegion[" + key3 + "]"), (object) str3);
        }
        else if (str1.StartsWith("OnTitanEnterRegion"))
        {
          int num17 = str1.IndexOf('[');
          int num18 = str1.IndexOf(']');
          string key4 = str1.Substring(num17 + 2, num18 - num17 - 3);
          int num19 = str1.IndexOf('(');
          int num20 = str1.IndexOf(')');
          string str4 = str1.Substring(num19 + 2, num20 - num19 - 3);
          if (((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).ContainsKey((object) key4))
          {
            RegionTrigger rcRegionTrigger = (RegionTrigger) FengGameManagerMKII.RCRegionTriggers[(object) key4];
            rcRegionTrigger.titanEventEnter = block;
            rcRegionTrigger.myName = key4;
            FengGameManagerMKII.RCRegionTriggers[(object) key4] = (object) rcRegionTrigger;
          }
          else
          {
            RegionTrigger regionTrigger = new RegionTrigger()
            {
              titanEventEnter = block,
              myName = key4
            };
            ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Add((object) key4, (object) regionTrigger);
          }
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) ("OnTitanEnterRegion[" + key4 + "]"), (object) str4);
        }
        else if (str1.StartsWith("OnTitanLeaveRegion"))
        {
          int num21 = str1.IndexOf('[');
          int num22 = str1.IndexOf(']');
          string key5 = str1.Substring(num21 + 2, num22 - num21 - 3);
          int num23 = str1.IndexOf('(');
          int num24 = str1.IndexOf(')');
          string str5 = str1.Substring(num23 + 2, num24 - num23 - 3);
          if (((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).ContainsKey((object) key5))
          {
            RegionTrigger rcRegionTrigger = (RegionTrigger) FengGameManagerMKII.RCRegionTriggers[(object) key5];
            rcRegionTrigger.titanEventExit = block;
            rcRegionTrigger.myName = key5;
            FengGameManagerMKII.RCRegionTriggers[(object) key5] = (object) rcRegionTrigger;
          }
          else
          {
            RegionTrigger regionTrigger = new RegionTrigger()
            {
              titanEventExit = block,
              myName = key5
            };
            ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Add((object) key5, (object) regionTrigger);
          }
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) ("OnTitanExitRegion[" + key5 + "]"), (object) str5);
        }
        else if (str1.StartsWith("OnFirstLoad()"))
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnFirstLoad", (object) block);
        else if (str1.StartsWith("OnRoundStart()"))
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnRoundStart", (object) block);
        else if (str1.StartsWith("OnUpdate()"))
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnUpdate", (object) block);
        else if (str1.StartsWith("OnTitanDie"))
        {
          int num25 = str1.IndexOf('(');
          int num26 = str1.LastIndexOf(')');
          string[] strArray2 = str1.Substring(num25 + 1, num26 - num25 - 1).Split(',');
          strArray2[0] = strArray2[0].Substring(1, strArray2[0].Length - 2);
          strArray2[1] = strArray2[1].Substring(1, strArray2[1].Length - 2);
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) "OnTitanDie", (object) strArray2);
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnTitanDie", (object) block);
        }
        else if (str1.StartsWith("OnPlayerDieByTitan"))
        {
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnPlayerDieByTitan", (object) block);
          int num27 = str1.IndexOf('(');
          int num28 = str1.LastIndexOf(')');
          string[] strArray3 = str1.Substring(num27 + 1, num28 - num27 - 1).Split(',');
          strArray3[0] = strArray3[0].Substring(1, strArray3[0].Length - 2);
          strArray3[1] = strArray3[1].Substring(1, strArray3[1].Length - 2);
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) "OnPlayerDieByTitan", (object) strArray3);
        }
        else if (str1.StartsWith("OnPlayerDieByPlayer"))
        {
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnPlayerDieByPlayer", (object) block);
          int num29 = str1.IndexOf('(');
          int num30 = str1.LastIndexOf(')');
          string[] strArray4 = str1.Substring(num29 + 1, num30 - num29 - 1).Split(',');
          strArray4[0] = strArray4[0].Substring(1, strArray4[0].Length - 2);
          strArray4[1] = strArray4[1].Substring(1, strArray4[1].Length - 2);
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) "OnPlayerDieByPlayer", (object) strArray4);
        }
        else if (str1.StartsWith("OnChatInput"))
        {
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Add((object) "OnChatInput", (object) block);
          int num31 = str1.IndexOf('(');
          int num32 = str1.LastIndexOf(')');
          string str6 = str1.Substring(num31 + 1, num32 - num31 - 1);
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Add((object) "OnChatInput", (object) str6.Substring(1, str6.Length - 2));
        }
      }
    }
    catch (UnityException ex)
    {
      this.chatRoom.addLINE(((Exception) ex).Message);
    }
  }

  public int conditionType(string str)
  {
    if (!str.StartsWith("Int"))
    {
      if (str.StartsWith("Bool"))
        return 1;
      if (str.StartsWith("String"))
        return 2;
      if (str.StartsWith("Float"))
        return 3;
      if (str.StartsWith("Titan"))
        return 5;
      if (str.StartsWith("Player"))
        return 4;
    }
    return 0;
  }

  private void core2()
  {
    if ((int) FengGameManagerMKII.settingsOld[64] >= 100)
    {
      this.coreeditor();
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.needChooseSide)
      {
        if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
        {
          if (NGUITools.GetActive(this.ui.GetComponent<UIReferArray>().panels[3]))
          {
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], true);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
            ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = false;
            ((Component) Camera.main).GetComponent<MouseLook>().disable = false;
          }
          else
          {
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], true);
            ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = true;
            ((Component) Camera.main).GetComponent<MouseLook>().disable = true;
          }
        }
        if (SettingsManager.InputSettings.General.Pause.GetKeyDown() && !GameMenu.Paused)
        {
          ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = true;
          ((Component) Camera.main).GetComponent<MouseLook>().disable = true;
          GameMenu.TogglePause(true);
        }
      }
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
        return;
      int num1;
      switch (IN_GAME_MAIN_CAMERA.gametype)
      {
        case GAMETYPE.SINGLE:
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
          {
            if (!this.isLosing)
            {
              Vector3 velocity = ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity;
              this.currentSpeed = ((Vector3) ref velocity).magnitude;
              this.maxSpeed = Mathf.Max(this.maxSpeed, this.currentSpeed);
              this.ShowHUDInfoTopLeft("Current Speed : " + (object) (int) this.currentSpeed + "\nMax Speed:" + (object) this.maxSpeed);
              break;
            }
            break;
          }
          this.ShowHUDInfoTopLeft("Kills:" + (object) this.single_kills + "\nMax Damage:" + (object) this.single_maxDamage + "\nTotal Damage:" + (object) this.single_totalDamage);
          break;
        case GAMETYPE.MULTIPLAYER:
          this.coreadd();
          this.ShowHUDInfoTopLeft(this.playerList);
          if (Object.op_Inequality((Object) Camera.main, (Object) null) && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING && ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
          {
            this.ShowHUDInfoCenter("Press [F7D358]" + SettingsManager.InputSettings.General.SpectateNextPlayer.ToString() + "[-] to spectate the next player. \nPress [F7D358]" + SettingsManager.InputSettings.General.SpectatePreviousPlayer.ToString() + "[-] to spectate the previous player.\nPress [F7D358]" + SettingsManager.InputSettings.Human.AttackSpecial.ToString() + "[-] to enter the spectator mode.\n\n\n\n");
            if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.DEATHMATCH || SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value || (SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0) && SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
            {
              this.myRespawnTime += Time.deltaTime;
              int num2 = 5;
              if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
                num2 = 10;
              if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
                num2 = SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value;
              num1 = num2 - (int) this.myRespawnTime;
              this.ShowHUDInfoCenterADD("Respawn in " + num1.ToString() + "s.");
              if ((double) this.myRespawnTime > (double) num2)
              {
                this.myRespawnTime = 0.0f;
                ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
                  this.SpawnNonAITitan2(this.myLastHero);
                else
                  this.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
                ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                this.ShowHUDInfoCenter(string.Empty);
                break;
              }
              break;
            }
            break;
          }
          break;
      }
      if (this.isLosing && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
            this.ShowHUDInfoCenter("Survive " + (object) this.wave + " Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
          else
            this.ShowHUDInfoCenter("Humanity Fail!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
        }
        else
        {
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
            this.ShowHUDInfoCenter("Survive " + (object) this.wave + " Waves!\nGame Restart in " + (object) (int) this.gameEndCD + "s\n\n");
          else
            this.ShowHUDInfoCenter("Humanity Fail!\nAgain!\nGame Restart in " + ((int) this.gameEndCD).ToString() + "s\n\n");
          if ((double) this.gameEndCD <= 0.0)
          {
            this.gameEndCD = 0.0f;
            if (PhotonNetwork.isMasterClient)
              this.restartRC();
            this.ShowHUDInfoCenter(string.Empty);
          }
          else
            this.gameEndCD -= Time.deltaTime;
        }
      }
      if (this.isWinning)
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          switch (IN_GAME_MAIN_CAMERA.gamemode)
          {
            case GAMEMODE.SURVIVE_MODE:
              this.ShowHUDInfoCenter("Survive All Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
              break;
            case GAMEMODE.RACING:
              this.ShowHUDInfoCenter(((float) ((double) (int) ((double) this.timeTotalServer * 10.0) * 0.10000000149011612 - 5.0)).ToString() + "s !\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
              break;
            default:
              this.ShowHUDInfoCenter("Humanity Win!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
              break;
          }
        }
        else
        {
          switch (IN_GAME_MAIN_CAMERA.gamemode)
          {
            case GAMEMODE.PVP_AHSS:
              if (SettingsManager.LegacyGameSettings.BladePVP.Value == 0 && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
              {
                this.ShowHUDInfoCenter("Team " + (object) this.teamWinner + " Win!\nGame Restart in " + (object) (int) this.gameEndCD + "s\n\n");
                break;
              }
              this.ShowHUDInfoCenter("Round Ended!\nGame Restart in " + (object) (int) this.gameEndCD + "s\n\n");
              break;
            case GAMEMODE.SURVIVE_MODE:
              this.ShowHUDInfoCenter("Survive All Waves!\nGame Restart in " + ((int) this.gameEndCD).ToString() + "s\n\n");
              break;
            case GAMEMODE.RACING:
              this.ShowHUDInfoCenter(this.localRacingResult + "\n\nGame Restart in " + (object) (int) this.gameEndCD + "s");
              break;
            default:
              this.ShowHUDInfoCenter("Humanity Win!\nGame Restart in " + ((int) this.gameEndCD).ToString() + "s\n\n");
              break;
          }
          if ((double) this.gameEndCD <= 0.0)
          {
            this.gameEndCD = 0.0f;
            if (PhotonNetwork.isMasterClient)
              this.restartRC();
            this.ShowHUDInfoCenter(string.Empty);
          }
          else
            this.gameEndCD -= Time.deltaTime;
        }
      }
      this.timeElapse += Time.deltaTime;
      this.roundTime += Time.deltaTime;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
        {
          if (!this.isWinning)
            this.timeTotalServer += Time.deltaTime;
        }
        else if (!this.isLosing && !this.isWinning)
          this.timeTotalServer += Time.deltaTime;
      }
      else
        this.timeTotalServer += Time.deltaTime;
      if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          float num3;
          if (!this.isWinning)
          {
            num3 = this.timeTotalServer - 5f;
            this.ShowHUDInfoTopCenter("Time : " + num3.ToString("0.00"));
          }
          if ((double) this.timeTotalServer < 5.0)
          {
            num3 = 5f - this.timeTotalServer;
            this.ShowHUDInfoCenter("RACE START IN " + num3.ToString("0.00"));
          }
          else if (!this.startRacing)
          {
            this.ShowHUDInfoCenter(string.Empty);
            this.startRacing = true;
            this.endRacing = false;
            GameObject.Find("door").SetActive(false);
          }
        }
        else
        {
          float num4 = SettingsManager.LegacyGameSettings.RacingStartTime.Value;
          float num5;
          string str;
          if ((double) this.roundTime < (double) num4)
          {
            str = "WAITING";
          }
          else
          {
            num5 = this.roundTime - num4;
            str = num5.ToString("0.00");
          }
          this.ShowHUDInfoTopCenter("Time : " + str);
          if ((double) this.roundTime < (double) num4)
          {
            num5 = num4 - this.roundTime;
            this.ShowHUDInfoCenter("RACE START IN " + num5.ToString("0.00") + (!(this.localRacingResult == string.Empty) ? "\nLast Round\n" + this.localRacingResult : "\n\n"));
          }
          else if (!this.startRacing)
          {
            this.ShowHUDInfoCenter(string.Empty);
            this.startRacing = true;
            this.endRacing = false;
            GameObject gameObject = GameObject.Find("door");
            if (Object.op_Inequality((Object) gameObject, (Object) null))
              gameObject.SetActive(false);
            if (this.racingDoors != null && FengGameManagerMKII.customLevelLoaded)
            {
              foreach (GameObject racingDoor in this.racingDoors)
                racingDoor.SetActive(false);
              this.racingDoors = (List<GameObject>) null;
            }
          }
          else if (this.racingDoors != null && FengGameManagerMKII.customLevelLoaded)
          {
            foreach (GameObject racingDoor in this.racingDoors)
              racingDoor.SetActive(false);
            this.racingDoors = (List<GameObject>) null;
          }
          if (this.needChooseSide)
            this.ShowHUDInfoTopCenterADD("\n\nPRESS " + SettingsManager.InputSettings.Human.Flare1.ToString() + " TO ENTER GAME");
        }
        if (((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide && FengGameManagerMKII.customLevelLoaded && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
        {
          this.myRespawnTime += Time.deltaTime;
          if ((double) this.myRespawnTime > 1.5)
          {
            this.myRespawnTime = 0.0f;
            ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            if (Object.op_Inequality((Object) this.checkpoint, (Object) null))
              this.StartCoroutine(this.WaitAndRespawn2(0.1f, this.checkpoint));
            else
              this.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
            ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            this.ShowHUDInfoCenter(string.Empty);
          }
        }
      }
      if ((double) this.timeElapse > 1.0)
      {
        --this.timeElapse;
        string content1 = string.Empty;
        switch (IN_GAME_MAIN_CAMERA.gamemode)
        {
          case GAMEMODE.KILL_TITAN:
          case GAMEMODE.None:
            string str1 = "Titan Left: ";
            num1 = GameObject.FindGameObjectsWithTag("titan").Length;
            string str2 = str1 + num1.ToString() + "  Time : ";
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
              num1 = (int) this.timeTotalServer;
              content1 = str2 + num1.ToString();
              break;
            }
            num1 = this.time - (int) this.timeTotalServer;
            content1 = str2 + num1.ToString();
            break;
          case GAMEMODE.ENDLESS_TITAN:
            num1 = this.time - (int) this.timeTotalServer;
            content1 = content1 + "Time : " + num1.ToString();
            break;
          case GAMEMODE.SURVIVE_MODE:
            object[] objArray = new object[4]
            {
              (object) "Titan Left: ",
              null,
              null,
              null
            };
            num1 = GameObject.FindGameObjectsWithTag("titan").Length;
            objArray[1] = (object) num1.ToString();
            objArray[2] = (object) " Wave : ";
            objArray[3] = (object) this.wave;
            content1 = string.Concat(objArray);
            break;
          case GAMEMODE.BOSS_FIGHT_CT:
            string str3 = "Time : ";
            num1 = this.time - (int) this.timeTotalServer;
            content1 = str3 + num1.ToString() + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
            break;
          case GAMEMODE.PVP_CAPTURE:
            string str4 = "| ";
            for (int index = 0; index < PVPcheckPoint.chkPts.Count; ++index)
              str4 = str4 + (PVPcheckPoint.chkPts[index] as PVPcheckPoint).getStateString() + " ";
            string str5 = str4 + "|";
            num1 = this.time - (int) this.timeTotalServer;
            content1 = (this.PVPtitanScoreMax - this.PVPtitanScore).ToString() + "  " + str5 + "  " + (object) (this.PVPhumanScoreMax - this.PVPhumanScore) + "\n" + "Time : " + num1.ToString();
            break;
        }
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
          content1 = content1 + "\n[00FFFF]Cyan:" + Convert.ToString(this.cyanKills) + "       [FF00FF]Magenta:" + Convert.ToString(this.magentaKills) + "[ffffff]";
        if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
          this.ShowHUDInfoTopCenter(content1);
        string content2 = string.Empty;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
          {
            string str6 = "Time : ";
            num1 = (int) this.timeTotalServer;
            content2 = str6 + num1.ToString();
          }
        }
        else
        {
          switch (IN_GAME_MAIN_CAMERA.gamemode)
          {
            case GAMEMODE.KILL_TITAN:
            case GAMEMODE.BOSS_FIGHT_CT:
            case GAMEMODE.PVP_CAPTURE:
              content2 = "Humanity " + (object) this.humanScore + " : Titan " + (object) this.titanScore + " ";
              break;
            case GAMEMODE.PVP_AHSS:
              for (int index = 0; index < this.teamScores.Length; ++index)
                content2 = content2 + (index == 0 ? (object) string.Empty : (object) " : ") + "Team" + (object) (index + 1) + " " + (object) this.teamScores[index] + string.Empty;
              content2 = content2 + "\nTime : " + (this.time - (int) this.timeTotalServer).ToString();
              break;
            case GAMEMODE.ENDLESS_TITAN:
              content2 = "Humanity " + (object) this.humanScore + " : Titan " + (object) this.titanScore + " ";
              break;
            case GAMEMODE.SURVIVE_MODE:
              string str7 = "Time : ";
              num1 = this.time - (int) this.timeTotalServer;
              content2 = str7 + num1.ToString();
              break;
          }
        }
        this.ShowHUDInfoTopRight(content2);
        string str8;
        if (IN_GAME_MAIN_CAMERA.difficulty < 0)
        {
          str8 = "Trainning";
        }
        else
        {
          switch (IN_GAME_MAIN_CAMERA.difficulty)
          {
            case 0:
              str8 = "Normal";
              break;
            case 1:
              str8 = "Hard";
              break;
            default:
              str8 = "Abnormal";
              break;
          }
        }
        string str9 = str8;
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
          this.ShowHUDInfoTopRightMAPNAME(((int) this.roundTime).ToString() + "s\n" + FengGameManagerMKII.level + " : " + str9);
        else
          this.ShowHUDInfoTopRightMAPNAME("\n" + FengGameManagerMKII.level + " : " + str9);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          string str10 = PhotonNetwork.room.name.Split("`"[0])[0];
          if (str10.Length > 20)
            str10 = str10.Remove(19) + "...";
          this.ShowHUDInfoTopRightMAPNAME("\n" + str10 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
          if (this.needChooseSide && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
            this.ShowHUDInfoTopCenterADD("\n\nPRESS " + SettingsManager.InputSettings.Human.Flare1.ToString() + " TO ENTER GAME");
        }
      }
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.killInfoGO.Count > 0 && this.killInfoGO[0] == null)
        this.killInfoGO.RemoveAt(0);
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !PhotonNetwork.isMasterClient || (double) this.timeTotalServer <= (double) this.time)
        return;
      IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
      this.gameStart = false;
      string str11 = string.Empty;
      string str12 = string.Empty;
      string str13 = string.Empty;
      string str14 = string.Empty;
      string str15 = string.Empty;
      foreach (PhotonPlayer player in PhotonNetwork.playerList)
      {
        if (player != null)
        {
          str11 = str11 + player.customProperties[(object) PhotonPlayerProperty.name]?.ToString() + "\n";
          str12 = str12 + player.customProperties[(object) PhotonPlayerProperty.kills]?.ToString() + "\n";
          str13 = str13 + player.customProperties[(object) PhotonPlayerProperty.deaths]?.ToString() + "\n";
          str14 = str14 + player.customProperties[(object) PhotonPlayerProperty.max_dmg]?.ToString() + "\n";
          str15 = str15 + player.customProperties[(object) PhotonPlayerProperty.total_dmg]?.ToString() + "\n";
        }
      }
      string str16;
      switch (IN_GAME_MAIN_CAMERA.gamemode)
      {
        case GAMEMODE.PVP_AHSS:
          str16 = string.Empty;
          for (int index = 0; index < this.teamScores.Length; ++index)
          {
            string str17 = str16;
            string str18;
            if (index != 0)
              str18 = " : ";
            else
              str18 = "Team" + (object) (index + 1) + " " + (object) this.teamScores[index] + " ";
            str16 = str17 + str18;
          }
          break;
        case GAMEMODE.SURVIVE_MODE:
          str16 = "Highest Wave : " + this.highestwave.ToString();
          break;
        default:
          str16 = "Humanity " + (object) this.humanScore + " : Titan " + (object) this.titanScore;
          break;
      }
      this.photonView.RPC("showResult", PhotonTargets.AllBuffered, (object) str11, (object) str12, (object) str13, (object) str14, (object) str15, (object) str16);
    }
  }

  private void coreadd()
  {
    if (PhotonNetwork.isMasterClient)
    {
      this.OnUpdate();
      if (FengGameManagerMKII.customLevelLoaded)
      {
        for (int index = 0; index < this.titanSpawners.Count; ++index)
        {
          TitanSpawner titanSpawner = this.titanSpawners[index];
          titanSpawner.time -= Time.deltaTime;
          if ((double) titanSpawner.time <= 0.0 && this.titans.Count + this.fT.Count < Math.Min(SettingsManager.LegacyGameSettings.TitanSpawnCap.Value, 80))
          {
            string name = titanSpawner.name;
            if (name == "spawnAnnie")
            {
              PhotonNetwork.Instantiate("FEMALE_TITAN", titanSpawner.location, new Quaternion(0.0f, 0.0f, 0.0f, 1f), 0);
            }
            else
            {
              GameObject gameObject = PhotonNetwork.Instantiate("TITAN_VER3.1", titanSpawner.location, new Quaternion(0.0f, 0.0f, 0.0f, 1f), 0);
              if (name == "spawnAbnormal")
                gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
              else if (name == "spawnJumper")
                gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
              else if (name == "spawnCrawler")
                gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
              else if (name == "spawnPunk")
                gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
            }
            if (titanSpawner.endless)
              titanSpawner.time = titanSpawner.delay;
            else
              this.titanSpawners.Remove(titanSpawner);
          }
        }
      }
    }
    if ((double) Time.timeScale > 0.10000000149011612)
      return;
    if ((double) this.pauseWaitTime <= 3.0)
    {
      this.pauseWaitTime -= Time.deltaTime * 1000000f;
      if ((double) this.pauseWaitTime <= 1.0)
        Camera.main.farClipPlane = 1500f;
      if ((double) this.pauseWaitTime <= 0.0)
      {
        this.pauseWaitTime = 0.0f;
        Time.timeScale = 1f;
      }
    }
    this.justRecompileThePlayerList();
  }

  private void coreeditor()
  {
    if (Input.GetKey((KeyCode) 9))
      GUI.FocusControl((string) null);
    if (Object.op_Inequality((Object) this.selectedObj, (Object) null))
    {
      float num = 0.2f;
      if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
        num = 0.04f;
      else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
        num = 0.6f;
      if (SettingsManager.InputSettings.General.Forward.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(num, new Vector3(((Component) Camera.mainCamera).transform.forward.x, 0.0f, ((Component) Camera.mainCamera).transform.forward.z)));
      }
      else if (SettingsManager.InputSettings.General.Back.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(num, new Vector3(((Component) Camera.mainCamera).transform.forward.x, 0.0f, ((Component) Camera.mainCamera).transform.forward.z)));
      }
      if (SettingsManager.InputSettings.General.Left.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(num, new Vector3(((Component) Camera.mainCamera).transform.right.x, 0.0f, ((Component) Camera.mainCamera).transform.right.z)));
      }
      else if (SettingsManager.InputSettings.General.Right.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(num, new Vector3(((Component) Camera.mainCamera).transform.right.x, 0.0f, ((Component) Camera.mainCamera).transform.right.z)));
      }
      if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(Vector3.up, num));
      }
      else if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
      {
        Transform transform = this.selectedObj.transform;
        transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.up, num));
      }
      if (!((Object) this.selectedObj).name.StartsWith("misc,region"))
      {
        if (SettingsManager.InputSettings.RCEditor.RotateRight.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.up, num));
        else if (SettingsManager.InputSettings.RCEditor.RotateLeft.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.down, num));
        if (SettingsManager.InputSettings.RCEditor.RotateCCW.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.forward, num));
        else if (SettingsManager.InputSettings.RCEditor.RotateCW.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.back, num));
        if (SettingsManager.InputSettings.RCEditor.RotateBack.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.left, num));
        else if (SettingsManager.InputSettings.RCEditor.RotateForward.GetKey())
          this.selectedObj.transform.Rotate(Vector3.op_Multiply(Vector3.right, num));
      }
      if (SettingsManager.InputSettings.RCEditor.Place.GetKeyDown())
      {
        ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Add((object) ((Object) this.selectedObj).GetInstanceID(), (object) (((Object) this.selectedObj).name + "," + Convert.ToString(this.selectedObj.transform.position.x) + "," + Convert.ToString(this.selectedObj.transform.position.y) + "," + Convert.ToString(this.selectedObj.transform.position.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.x) + "," + Convert.ToString(this.selectedObj.transform.rotation.y) + "," + Convert.ToString(this.selectedObj.transform.rotation.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.w)));
        this.selectedObj = (GameObject) null;
        ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = true;
      }
      if (!SettingsManager.InputSettings.RCEditor.Delete.GetKeyDown())
        return;
      Object.Destroy((Object) this.selectedObj);
      this.selectedObj = (GameObject) null;
      ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = true;
      ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Remove((object) ((Object) this.selectedObj).GetInstanceID());
    }
    else
    {
      if (((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled)
      {
        float num = 100f;
        if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
          num = 20f;
        else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
          num = 400f;
        Transform transform1 = ((Component) Camera.main).transform;
        if (SettingsManager.InputSettings.General.Forward.GetKey())
        {
          Transform transform2 = transform1;
          transform2.position = Vector3.op_Addition(transform2.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.forward, num), Time.deltaTime));
        }
        else if (SettingsManager.InputSettings.General.Back.GetKey())
        {
          Transform transform3 = transform1;
          transform3.position = Vector3.op_Subtraction(transform3.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.forward, num), Time.deltaTime));
        }
        if (SettingsManager.InputSettings.General.Left.GetKey())
        {
          Transform transform4 = transform1;
          transform4.position = Vector3.op_Subtraction(transform4.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.right, num), Time.deltaTime));
        }
        else if (SettingsManager.InputSettings.General.Right.GetKey())
        {
          Transform transform5 = transform1;
          transform5.position = Vector3.op_Addition(transform5.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.right, num), Time.deltaTime));
        }
        if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
        {
          Transform transform6 = transform1;
          transform6.position = Vector3.op_Addition(transform6.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.up, num), Time.deltaTime));
        }
        else if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
        {
          Transform transform7 = transform1;
          transform7.position = Vector3.op_Subtraction(transform7.position, Vector3.op_Multiply(Vector3.op_Multiply(transform1.up, num), Time.deltaTime));
        }
      }
      if (SettingsManager.InputSettings.RCEditor.Cursor.GetKeyDown())
      {
        if (((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled)
          ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = false;
        else
          ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = true;
      }
      if (!Input.GetKeyDown((KeyCode) 323) || Screen.lockCursor || GUIUtility.hotControl != 0 || ((double) Input.mousePosition.x <= 300.0 || (double) Input.mousePosition.x >= (double) Screen.width - 300.0) && (double) Screen.height - (double) Input.mousePosition.y <= 600.0)
        return;
      RaycastHit raycastHit = new RaycastHit();
      if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), ref raycastHit))
        return;
      Transform transform = ((RaycastHit) ref raycastHit).transform;
      if (((Object) ((Component) transform).gameObject).name.StartsWith("custom") || ((Object) ((Component) transform).gameObject).name.StartsWith("base") || ((Object) ((Component) transform).gameObject).name.StartsWith("racing") || ((Object) ((Component) transform).gameObject).name.StartsWith("photon") || ((Object) ((Component) transform).gameObject).name.StartsWith("spawnpoint") || ((Object) ((Component) transform).gameObject).name.StartsWith("misc"))
      {
        this.selectedObj = ((Component) transform).gameObject;
        ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = false;
        Screen.lockCursor = true;
        ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Remove((object) ((Object) this.selectedObj).GetInstanceID());
      }
      else
      {
        if (!((Object) ((Component) transform.parent).gameObject).name.StartsWith("custom") && !((Object) ((Component) transform.parent).gameObject).name.StartsWith("base") && !((Object) ((Component) transform.parent).gameObject).name.StartsWith("racing") && !((Object) ((Component) transform.parent).gameObject).name.StartsWith("photon"))
          return;
        this.selectedObj = ((Component) transform.parent).gameObject;
        ((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled = false;
        Screen.lockCursor = true;
        ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Remove((object) ((Object) this.selectedObj).GetInstanceID());
      }
    }
  }

  private IEnumerator customlevelcache()
  {
    for (int i = 0; i < this.levelCache.Count; ++i)
    {
      this.customlevelclientE(this.levelCache[i], false);
      yield return (object) new WaitForEndOfFrame();
    }
  }

  private void customlevelclientE(string[] content, bool renewHash)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (content[content.Length - 1].StartsWith("a"))
    {
      flag1 = true;
      this.customMapMaterials.Clear();
    }
    else if (content[content.Length - 1].StartsWith("z"))
    {
      flag2 = true;
      FengGameManagerMKII.customLevelLoaded = true;
      this.spawnPlayerCustomMap();
      Minimap.TryRecaptureInstance();
      this.unloadAssets();
      ((Behaviour) ((Component) Camera.main).GetComponent<TiltShift>()).enabled = false;
    }
    if (renewHash)
    {
      if (flag1)
      {
        FengGameManagerMKII.currentLevel = string.Empty;
        this.levelCache.Clear();
        this.titanSpawns.Clear();
        this.playerSpawnsC.Clear();
        this.playerSpawnsM.Clear();
        for (int index = 0; index < content.Length; ++index)
        {
          string[] strArray = content[index].Split(',');
          if (strArray[0] == "titan")
            this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
          else if (strArray[0] == "playerC")
            this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
          else if (strArray[0] == "playerM")
            this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
        }
        this.spawnPlayerCustomMap();
      }
      FengGameManagerMKII.currentLevel += content[content.Length - 1];
      this.levelCache.Add(content);
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) FengGameManagerMKII.currentLevel);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    if (flag1 || flag2)
      return;
    for (int index1 = 0; index1 < content.Length; ++index1)
    {
      string[] strArray = content[index1].Split(',');
      GameObject gameObject1;
      float result;
      Color color;
      if (strArray[0].StartsWith("custom"))
      {
        float num1 = 1f;
        gameObject1 = (GameObject) null;
        GameObject gameObject2 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])));
        if (strArray[2] != "default")
        {
          if (strArray[2].StartsWith("transparent"))
          {
            if (float.TryParse(strArray[2].Substring(11), out result))
              num1 = result;
            foreach (Renderer componentsInChild in gameObject2.GetComponentsInChildren<Renderer>())
            {
              componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load("transparent");
              if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
              {
                string materialHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                if (this.customMapMaterials.ContainsKey(materialHash))
                {
                  componentsInChild.material = this.customMapMaterials[materialHash];
                }
                else
                {
                  componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                  this.customMapMaterials.Add(materialHash, componentsInChild.material);
                }
              }
            }
          }
          else
          {
            foreach (Renderer componentsInChild in gameObject2.GetComponentsInChildren<Renderer>())
            {
              componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load(strArray[2]);
              if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
              {
                string materialHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                if (this.customMapMaterials.ContainsKey(materialHash))
                {
                  componentsInChild.material = this.customMapMaterials[materialHash];
                }
                else
                {
                  componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                  this.customMapMaterials.Add(materialHash, componentsInChild.material);
                }
              }
            }
          }
        }
        float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(strArray[3]) - 1f / 1000f;
        float num3 = gameObject2.transform.localScale.y * Convert.ToSingle(strArray[4]);
        float num4 = gameObject2.transform.localScale.z * Convert.ToSingle(strArray[5]);
        gameObject2.transform.localScale = new Vector3(num2, num3, num4);
        if (strArray[6] != "0")
        {
          // ISSUE: explicit constructor call
          ((Color) ref color).\u002Ector(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num1);
          foreach (MeshFilter componentsInChild in gameObject2.GetComponentsInChildren<MeshFilter>())
          {
            Mesh mesh = componentsInChild.mesh;
            Color[] colorArray = new Color[mesh.vertexCount];
            for (int index2 = 0; index2 < mesh.vertexCount; ++index2)
              colorArray[index2] = color;
            mesh.colors = colorArray;
          }
        }
      }
      else if (strArray[0].StartsWith("base"))
      {
        if (strArray.Length < 15)
        {
          Object.Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])));
        }
        else
        {
          float num5 = 1f;
          gameObject1 = (GameObject) null;
          GameObject gameObject3 = (GameObject) Object.Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])));
          if (strArray[2] != "default")
          {
            if (strArray[2].StartsWith("transparent"))
            {
              if (float.TryParse(strArray[2].Substring(11), out result))
                num5 = result;
              foreach (Renderer componentsInChild in gameObject3.GetComponentsInChildren<Renderer>())
              {
                componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load("transparent");
                if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
                {
                  string materialHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                  if (this.customMapMaterials.ContainsKey(materialHash))
                  {
                    componentsInChild.material = this.customMapMaterials[materialHash];
                  }
                  else
                  {
                    componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                    this.customMapMaterials.Add(materialHash, componentsInChild.material);
                  }
                }
              }
            }
            else
            {
              foreach (Renderer componentsInChild in gameObject3.GetComponentsInChildren<Renderer>())
              {
                if (!((Object) componentsInChild).name.Contains("Particle System") || !((Object) gameObject3).name.Contains("aot_supply"))
                {
                  componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load(strArray[2]);
                  if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
                  {
                    string materialHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                    if (this.customMapMaterials.ContainsKey(materialHash))
                    {
                      componentsInChild.material = this.customMapMaterials[materialHash];
                    }
                    else
                    {
                      componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                      this.customMapMaterials.Add(materialHash, componentsInChild.material);
                    }
                  }
                }
              }
            }
          }
          float num6 = gameObject3.transform.localScale.x * Convert.ToSingle(strArray[3]) - 1f / 1000f;
          float num7 = gameObject3.transform.localScale.y * Convert.ToSingle(strArray[4]);
          float num8 = gameObject3.transform.localScale.z * Convert.ToSingle(strArray[5]);
          gameObject3.transform.localScale = new Vector3(num6, num7, num8);
          if (strArray[6] != "0")
          {
            // ISSUE: explicit constructor call
            ((Color) ref color).\u002Ector(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num5);
            foreach (MeshFilter componentsInChild in gameObject3.GetComponentsInChildren<MeshFilter>())
            {
              Mesh mesh = componentsInChild.mesh;
              Color[] colorArray = new Color[mesh.vertexCount];
              for (int index3 = 0; index3 < mesh.vertexCount; ++index3)
                colorArray[index3] = color;
              mesh.colors = colorArray;
            }
          }
        }
      }
      else if (strArray[0].StartsWith("misc"))
      {
        if (strArray[1].StartsWith("barrier"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject4 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num9 = gameObject4.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num10 = gameObject4.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num11 = gameObject4.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject4.transform.localScale = new Vector3(num9, num10, num11);
        }
        else if (strArray[1].StartsWith("racingStart"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject5 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num12 = gameObject5.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num13 = gameObject5.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num14 = gameObject5.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject5.transform.localScale = new Vector3(num12, num13, num14);
          if (this.racingDoors != null)
            this.racingDoors.Add(gameObject5);
        }
        else if (strArray[1].StartsWith("racingEnd"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject6 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num15 = gameObject6.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num16 = gameObject6.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num17 = gameObject6.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject6.transform.localScale = new Vector3(num15, num16, num17);
          gameObject6.AddComponent<LevelTriggerRacingEnd>();
        }
        else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
        {
          Vector3 loc;
          // ISSUE: explicit constructor call
          ((Vector3) ref loc).\u002Ector(Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
          RCRegion rcRegion = new RCRegion(loc, Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]));
          string key = strArray[2];
          if (((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).ContainsKey((object) key))
          {
            GameObject gameObject7 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("region"));
            gameObject7.transform.position = loc;
            gameObject7.AddComponent<RegionTrigger>();
            gameObject7.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger) FengGameManagerMKII.RCRegionTriggers[(object) key]);
            float num18 = gameObject7.transform.localScale.x * Convert.ToSingle(strArray[3]) - 1f / 1000f;
            float num19 = gameObject7.transform.localScale.y * Convert.ToSingle(strArray[4]);
            float num20 = gameObject7.transform.localScale.z * Convert.ToSingle(strArray[5]);
            gameObject7.transform.localScale = new Vector3(num18, num19, num20);
            rcRegion.myBox = gameObject7;
          }
          ((Dictionary<object, object>) FengGameManagerMKII.RCRegions).Add((object) key, (object) rcRegion);
        }
      }
      else if (strArray[0].StartsWith("racing"))
      {
        if (strArray[1].StartsWith("start"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject8 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num21 = gameObject8.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num22 = gameObject8.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num23 = gameObject8.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject8.transform.localScale = new Vector3(num21, num22, num23);
          if (this.racingDoors != null)
            this.racingDoors.Add(gameObject8);
        }
        else if (strArray[1].StartsWith("end"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject9 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num24 = gameObject9.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num25 = gameObject9.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num26 = gameObject9.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject9.transform.localScale = new Vector3(num24, num25, num26);
          ((Component) gameObject9.GetComponentInChildren<Collider>()).gameObject.AddComponent<LevelTriggerRacingEnd>();
        }
        else if (strArray[1].StartsWith("kill"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject10 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num27 = gameObject10.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num28 = gameObject10.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num29 = gameObject10.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject10.transform.localScale = new Vector3(num27, num28, num29);
          ((Component) gameObject10.GetComponentInChildren<Collider>()).gameObject.AddComponent<RacingKillTrigger>();
        }
        else if (strArray[1].StartsWith("checkpoint"))
        {
          gameObject1 = (GameObject) null;
          GameObject gameObject11 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
          float num30 = gameObject11.transform.localScale.x * Convert.ToSingle(strArray[2]) - 1f / 1000f;
          float num31 = gameObject11.transform.localScale.y * Convert.ToSingle(strArray[3]);
          float num32 = gameObject11.transform.localScale.z * Convert.ToSingle(strArray[4]);
          gameObject11.transform.localScale = new Vector3(num30, num31, num32);
          ((Component) gameObject11.GetComponentInChildren<Collider>()).gameObject.AddComponent<RacingCheckpointTrigger>();
        }
      }
      else if (strArray[0].StartsWith("map"))
      {
        if (strArray[1].StartsWith("disablebounds"))
        {
          Object.Destroy((Object) GameObject.Find("gameobjectOutSide"));
          Object.Instantiate(FengGameManagerMKII.RCassets.Load("outside"));
        }
      }
      else if (PhotonNetwork.isMasterClient && strArray[0].StartsWith("photon"))
      {
        if (strArray[1].StartsWith("Cannon"))
        {
          if (strArray.Length > 15)
          {
            GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])), 0);
            go.GetComponent<CannonPropRegion>().settings = content[index1];
            go.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, (object) content[index1]);
          }
          else
            PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = content[index1];
        }
        else
        {
          TitanSpawner titanSpawner = new TitanSpawner();
          float num = 30f;
          if (float.TryParse(strArray[2], out result))
            num = Mathf.Max(Convert.ToSingle(strArray[2]), 1f);
          titanSpawner.time = num;
          titanSpawner.delay = num;
          titanSpawner.name = strArray[1];
          titanSpawner.endless = strArray[3] == "1";
          titanSpawner.location = new Vector3(Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]));
          this.titanSpawners.Add(titanSpawner);
        }
      }
    }
  }

  private IEnumerator customlevelE(List<PhotonPlayer> players)
  {
    FengGameManagerMKII fengGameManagerMkii = this;
    if (!(FengGameManagerMKII.currentLevel == string.Empty))
    {
      for (int i = 0; i < fengGameManagerMkii.levelCache.Count; ++i)
      {
        foreach (PhotonPlayer player in players)
        {
          if (player.customProperties[(object) PhotonPlayerProperty.currentLevel] != null && FengGameManagerMKII.currentLevel != string.Empty && RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.currentLevel]) == FengGameManagerMKII.currentLevel)
          {
            if (i == 0)
            {
              string[] strArray = new string[1]
              {
                "loadcached"
              };
              fengGameManagerMkii.photonView.RPC("customlevelRPC", player, (object) strArray);
            }
          }
          else
            fengGameManagerMkii.photonView.RPC("customlevelRPC", player, (object) fengGameManagerMkii.levelCache[i]);
        }
        if (i > 0)
          yield return (object) new WaitForSeconds(0.75f);
        else
          yield return (object) new WaitForSeconds(0.25f);
      }
    }
    else
    {
      string[] strArray = new string[1]{ "loadempty" };
      foreach (PhotonPlayer player in players)
        fengGameManagerMkii.photonView.RPC("customlevelRPC", player, (object) strArray);
      FengGameManagerMKII.customLevelLoaded = true;
    }
  }

  [RPC]
  private void customlevelRPC(string[] content, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    if (content.Length == 1 && content[0] == "loadcached")
      this.StartCoroutine(this.customlevelcache());
    else if (content.Length == 1 && content[0] == "loadempty")
    {
      FengGameManagerMKII.currentLevel = string.Empty;
      this.levelCache.Clear();
      this.titanSpawns.Clear();
      this.playerSpawnsC.Clear();
      this.playerSpawnsM.Clear();
      this.customMapMaterials.Clear();
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) FengGameManagerMKII.currentLevel);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
      FengGameManagerMKII.customLevelLoaded = true;
      this.spawnPlayerCustomMap();
    }
    else
      this.customlevelclientE(content, true);
  }

  public void debugChat(string str) => this.chatRoom.addLINE(str);

  public void DestroyAllExistingCloths()
  {
    Cloth[] objectsOfType = Object.FindObjectsOfType<Cloth>();
    if (objectsOfType.Length == 0)
      return;
    for (int index = 0; index < objectsOfType.Length; ++index)
      ClothFactory.DisposeObject(((Component) objectsOfType[index]).gameObject);
  }

  private void endGameInfectionRC()
  {
    ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Clear();
    for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
    {
      PhotonPlayer player = PhotonNetwork.playerList[index];
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
      player.SetCustomProperties(propertiesToSet);
    }
    int length = PhotonNetwork.playerList.Length;
    int num = SettingsManager.LegacyGameSettings.InfectionModeAmount.Value;
    for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
    {
      PhotonPlayer player = PhotonNetwork.playerList[index];
      if (length > 0 && (double) Random.Range(0.0f, 1f) <= (double) num / (double) length)
      {
        Hashtable propertiesToSet = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
        player.SetCustomProperties(propertiesToSet);
        ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Add((object) player.ID, (object) 2);
        --num;
      }
      --length;
    }
    this.gameEndCD = 0.0f;
    this.restartGame2();
  }

  private void endGameRC()
  {
    if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
    {
      for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
      {
        PhotonPlayer player = PhotonNetwork.playerList[index];
        Hashtable propertiesToSet = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.kills, (object) 0);
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.deaths, (object) 0);
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.max_dmg, (object) 0);
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.total_dmg, (object) 0);
        player.SetCustomProperties(propertiesToSet);
      }
    }
    this.gameEndCD = 0.0f;
    this.restartGame2();
  }

  public void EnterSpecMode(bool enter)
  {
    if (enter)
    {
      this.spectateSprites = new List<GameObject>();
      foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
      {
        if (Object.op_Inequality((Object) gameObject.GetComponent<UISprite>(), (Object) null) && gameObject.activeInHierarchy)
        {
          string name = ((Object) gameObject).name;
          if (name.Contains("blade") || name.Contains("bullet") || name.Contains("gas") || name.Contains("flare") || name.Contains("skill_cd"))
          {
            if (!this.spectateSprites.Contains(gameObject))
              this.spectateSprites.Add(gameObject);
            gameObject.SetActive(false);
          }
        }
      }
      string[] strArray = new string[2]
      {
        "Flare",
        "LabelInfoBottomRight"
      };
      foreach (string str in strArray)
      {
        GameObject gameObject = GameObject.Find(str);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          if (!this.spectateSprites.Contains(gameObject))
            this.spectateSprites.Add(gameObject);
          gameObject.SetActive(false);
        }
      }
      foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
      {
        if (player.photonView.isMine)
          PhotonNetwork.Destroy(player.photonView);
      }
      if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.dead]))
      {
        foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
        {
          if (titan.photonView.isMine)
            PhotonNetwork.Destroy(titan.photonView);
        }
      }
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
      FengGameManagerMKII.instance.needChooseSide = false;
      ((Behaviour) ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("Player");
      if (Object.op_Inequality((Object) gameObjectWithTag, (Object) null) && Object.op_Inequality((Object) gameObjectWithTag.GetComponent<HERO>(), (Object) null))
        ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObjectWithTag);
      else
        ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
      this.StartCoroutine(this.reloadSky(true));
    }
    else
    {
      if (Object.op_Inequality((Object) GameObject.Find("cross1"), (Object) null))
        GameObject.Find("cross1").transform.localPosition = Vector3.op_Multiply(Vector3.up, 5000f);
      if (this.spectateSprites != null)
      {
        foreach (GameObject spectateSprite in this.spectateSprites)
        {
          if (Object.op_Inequality((Object) spectateSprite, (Object) null))
            spectateSprite.SetActive(true);
        }
      }
      this.spectateSprites = new List<GameObject>();
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
      NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
      FengGameManagerMKII.instance.needChooseSide = true;
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }
  }

  public void gameLose2()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.isMasterClient || this.isWinning || this.isLosing)
      return;
    this.isLosing = true;
    ++this.titanScore;
    this.gameEndCD = this.gameEndTotalCDtime;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
      this.photonView.RPC("netGameLose", PhotonTargets.Others, (object) this.titanScore);
    if (!SettingsManager.UISettings.GameFeed.Value)
      return;
    this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
  }

  public void gameWin2()
  {
    if (this.isLosing || this.isWinning)
      return;
    this.isWinning = true;
    ++this.humanScore;
    switch (IN_GAME_MAIN_CAMERA.gamemode)
    {
      case GAMEMODE.PVP_AHSS:
        this.gameEndCD = this.gameEndTotalCDtime;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          this.photonView.RPC("netGameWin", PhotonTargets.Others, (object) this.teamWinner);
        ++this.teamScores[this.teamWinner - 1];
        break;
      case GAMEMODE.RACING:
        this.gameEndCD = !SettingsManager.LegacyGameSettings.RacingEndless.Value ? 20f : 1000f;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          this.photonView.RPC("netGameWin", PhotonTargets.Others, (object) 0);
          break;
        }
        break;
      default:
        this.gameEndCD = this.gameEndTotalCDtime;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          this.photonView.RPC("netGameWin", PhotonTargets.Others, (object) this.humanScore);
          break;
        }
        break;
    }
    if (!SettingsManager.UISettings.GameFeed.Value)
      return;
    this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
  }

  public ArrayList getPlayers() => this.heroes;

  public ArrayList getErens() => this.eT;

  [RPC]
  private void getRacingResult(string player, float time, PhotonMessageInfo info)
  {
    if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
    {
      if (info == null)
        return;
      this.kickPlayerRCIfMC(info.sender, true, "racing exploit");
    }
    else
    {
      this.racingResult.Add((object) new RacingResult()
      {
        name = player,
        time = time
      });
      this.refreshRacingResult2();
    }
  }

  public ArrayList getTitans() => this.titans;

  private string hairtype(int lol) => lol < 0 ? "Random" : "Male " + lol.ToString();

  [RPC]
  private void ignorePlayer(int ID, PhotonMessageInfo info)
  {
    if (info.sender.isMasterClient)
    {
      PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
      if (photonPlayer != null && !FengGameManagerMKII.ignoreList.Contains(ID))
      {
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          if (PhotonNetwork.playerList[index] == photonPlayer)
          {
            FengGameManagerMKII.ignoreList.Add(ID);
            PhotonNetwork.RaiseEvent((byte) 254, (object) null, true, new RaiseEventOptions()
            {
              TargetActors = new int[1]{ ID }
            });
          }
        }
      }
    }
    this.RecompilePlayerList(0.1f);
  }

  [RPC]
  private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
  {
    if (info.sender.isMasterClient)
    {
      for (int index1 = 0; index1 < IDS.Length; ++index1)
      {
        int ID = IDS[index1];
        PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
        if (photonPlayer != null && !FengGameManagerMKII.ignoreList.Contains(ID))
        {
          for (int index2 = 0; index2 < PhotonNetwork.playerList.Length; ++index2)
          {
            if (PhotonNetwork.playerList[index2] == photonPlayer)
            {
              FengGameManagerMKII.ignoreList.Add(ID);
              PhotonNetwork.RaiseEvent((byte) 254, (object) null, true, new RaiseEventOptions()
              {
                TargetActors = new int[1]{ ID }
              });
            }
          }
        }
      }
    }
    this.RecompilePlayerList(0.1f);
  }

  public static GameObject InstantiateCustomAsset(string key)
  {
    key = key.Substring(8);
    return (GameObject) FengGameManagerMKII.RCassets.Load(key);
  }

  public bool isPlayerAllDead()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (PhotonPlayer player in PhotonNetwork.playerList)
    {
      if ((int) player.customProperties[(object) PhotonPlayerProperty.isTitan] == 1)
      {
        ++num1;
        if ((bool) player.customProperties[(object) PhotonPlayerProperty.dead])
          ++num2;
      }
    }
    return num1 == num2;
  }

  public bool isPlayerAllDead2()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (PhotonPlayer player in PhotonNetwork.playerList)
    {
      if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 1)
      {
        ++num1;
        if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
          ++num2;
      }
    }
    return num1 == num2;
  }

  public bool isTeamAllDead(int team)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (PhotonPlayer player in PhotonNetwork.playerList)
    {
      if ((int) player.customProperties[(object) PhotonPlayerProperty.isTitan] == 1 && (int) player.customProperties[(object) PhotonPlayerProperty.team] == team)
      {
        ++num1;
        if ((bool) player.customProperties[(object) PhotonPlayerProperty.dead])
          ++num2;
      }
    }
    return num1 == num2;
  }

  public bool isTeamAllDead2(int team)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (PhotonPlayer player in PhotonNetwork.playerList)
    {
      if (player.customProperties[(object) PhotonPlayerProperty.isTitan] != null && player.customProperties[(object) PhotonPlayerProperty.team] != null && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 1 && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.team]) == team)
      {
        ++num1;
        if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
          ++num2;
      }
    }
    return num1 == num2;
  }

  public void justRecompileThePlayerList()
  {
    string str1 = string.Empty;
    string empty;
    int num1;
    int num2;
    int num3;
    int num4;
    if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
    {
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      int num9 = 0;
      int num10 = 0;
      int num11 = 0;
      int num12 = 0;
      Dictionary<int, PhotonPlayer> dictionary1 = new Dictionary<int, PhotonPlayer>();
      Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
      Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
      foreach (PhotonPlayer player in PhotonNetwork.playerList)
      {
        if (player.customProperties[(object) PhotonPlayerProperty.dead] != null && !FengGameManagerMKII.ignoreList.Contains(player.ID))
        {
          switch (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]))
          {
            case 0:
              dictionary3.Add(player.ID, player);
              continue;
            case 1:
              dictionary1.Add(player.ID, player);
              num5 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
              num7 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
              num9 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
              num11 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
              continue;
            case 2:
              dictionary2.Add(player.ID, player);
              num6 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
              num8 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
              num10 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
              num12 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
              continue;
            default:
              continue;
          }
        }
      }
      this.cyanKills = num5;
      this.magentaKills = num6;
      if (PhotonNetwork.isMasterClient)
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
        {
          foreach (PhotonPlayer player in PhotonNetwork.playerList)
          {
            int num13 = 0;
            if (dictionary1.Count > dictionary2.Count + 1)
            {
              num13 = 2;
              if (dictionary1.ContainsKey(player.ID))
                dictionary1.Remove(player.ID);
              if (!dictionary2.ContainsKey(player.ID))
                dictionary2.Add(player.ID, player);
            }
            else if (dictionary2.Count > dictionary1.Count + 1)
            {
              num13 = 1;
              if (!dictionary1.ContainsKey(player.ID))
                dictionary1.Add(player.ID, player);
              if (dictionary2.ContainsKey(player.ID))
                dictionary2.Remove(player.ID);
            }
            if (num13 > 0)
              this.photonView.RPC("setTeamRPC", player, (object) num13);
          }
        }
        else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
        {
          foreach (PhotonPlayer player in PhotonNetwork.playerList)
          {
            int num14 = 0;
            int num15 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]);
            if (num15 > 0)
            {
              switch (num15)
              {
                case 1:
                  int num16 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
                  if (num6 + num16 + 7 < num5 - num16)
                  {
                    num14 = 2;
                    num6 += num16;
                    num5 -= num16;
                    break;
                  }
                  break;
                case 2:
                  int num17 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
                  if (num5 + num17 + 7 < num6 - num17)
                  {
                    num14 = 1;
                    num5 += num17;
                    num6 -= num17;
                    break;
                  }
                  break;
              }
              if (num14 > 0)
                this.photonView.RPC("setTeamRPC", player, (object) num14);
            }
          }
        }
      }
      string str2 = str1 + "[00FFFF]TEAM CYAN" + "[ffffff]:" + (object) this.cyanKills + "/" + (object) num7 + "/" + (object) num9 + "/" + (object) num11 + "\n";
      foreach (PhotonPlayer photonPlayer in dictionary1.Values)
      {
        int num18 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num18 == 1)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str2 += "[FF0000][X] ";
          str2 = !photonPlayer.isLocal ? str2 + "[FFCC00]" : str2 + "[00CC00]";
          str2 = str2 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str2 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str2 = str2 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num19 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num19 < 2)
              str2 = str2 + "[" + ColorSet.color_human + "] H ";
            else if (num19 == 2)
              str2 = str2 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str2 = str2 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str3 = str2;
          empty = string.Empty;
          string str4 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num20 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num21 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num22 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num23 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str2 = str3 + string.Empty + str4 + "[ffffff]:" + (object) num20 + "/" + (object) num21 + "/" + (object) num22 + "/" + (object) num23;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str2 += "[-]";
          str2 += "\n";
        }
      }
      string str5 = str2 + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + (object) this.magentaKills + "/" + (object) num8 + "/" + (object) num10 + "/" + (object) num12 + "\n";
      foreach (PhotonPlayer photonPlayer in dictionary2.Values)
      {
        int num24 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num24 == 2)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str5 += "[FF0000][X] ";
          str5 = !photonPlayer.isLocal ? str5 + "[FFCC00]" : str5 + "[00CC00]";
          str5 = str5 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str5 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str5 = str5 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num25 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num25 < 2)
              str5 = str5 + "[" + ColorSet.color_human + "] H ";
            else if (num25 == 2)
              str5 = str5 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str5 = str5 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str6 = str5;
          empty = string.Empty;
          string str7 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num26 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num27 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num28 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num29 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str5 = str6 + string.Empty + str7 + "[ffffff]:" + (object) num26 + "/" + (object) num27 + "/" + (object) num28 + "/" + (object) num29;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str5 += "[-]";
          str5 += "\n";
        }
      }
      str1 = str5 + " \n" + "[00FF00]INDIVIDUAL\n";
      foreach (PhotonPlayer photonPlayer in dictionary3.Values)
      {
        int num30 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num30 == 0)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str1 += "[FF0000][X] ";
          str1 = !photonPlayer.isLocal ? str1 + "[FFCC00]" : str1 + "[00CC00]";
          str1 = str1 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str1 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str1 = str1 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num31 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num31 < 2)
              str1 = str1 + "[" + ColorSet.color_human + "] H ";
            else if (num31 == 2)
              str1 = str1 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str1 = str1 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str8 = str1;
          empty = string.Empty;
          string str9 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num32 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num33 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num34 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num35 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str1 = str8 + string.Empty + str9 + "[ffffff]:" + (object) num32 + "/" + (object) num33 + "/" + (object) num34 + "/" + (object) num35;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str1 += "[-]";
          str1 += "\n";
        }
      }
    }
    else
    {
      foreach (PhotonPlayer player in PhotonNetwork.playerList)
      {
        if (player.customProperties[(object) PhotonPlayerProperty.dead] != null)
        {
          if (FengGameManagerMKII.ignoreList.Contains(player.ID))
            str1 += "[FF0000][X] ";
          string str10 = (!player.isLocal ? str1 + "[FFCC00]" : str1 + "[00CC00]") + "[" + Convert.ToString(player.ID) + "] ";
          if (player.isMasterClient)
            str10 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
            str10 = str10 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num36 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.team]);
            if (num36 < 2)
              str10 = str10 + "[" + ColorSet.color_human + "] H ";
            else if (num36 == 2)
              str10 = str10 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str10 = str10 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str11 = str10;
          empty = string.Empty;
          string str12 = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num37 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num38 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num39 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num40 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          string str13 = str11 + string.Empty + str12 + "[ffffff]:" + (object) num37 + "/" + (object) num38 + "/" + (object) num39 + "/" + (object) num40;
          if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
            str13 += "[-]";
          str1 = str13 + "\n";
        }
      }
    }
    this.playerList = str1;
    if (!PhotonNetwork.isMasterClient || this.isWinning || this.isLosing || (double) this.roundTime < 5.0)
      return;
    if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
    {
      int num41 = 0;
      for (int index1 = 0; index1 < PhotonNetwork.playerList.Length; ++index1)
      {
        PhotonPlayer player = PhotonNetwork.playerList[index1];
        if (!FengGameManagerMKII.ignoreList.Contains(player.ID) && player.customProperties[(object) PhotonPlayerProperty.dead] != null && player.customProperties[(object) PhotonPlayerProperty.isTitan] != null)
        {
          if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 1)
          {
            if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]) > 0)
            {
              if (!((Dictionary<object, object>) FengGameManagerMKII.imatitan).ContainsKey((object) player.ID))
                ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Add((object) player.ID, (object) 2);
              Hashtable propertiesToSet = new Hashtable();
              ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
              player.SetCustomProperties(propertiesToSet);
              this.photonView.RPC("spawnTitanRPC", player);
            }
            else if (((Dictionary<object, object>) FengGameManagerMKII.imatitan).ContainsKey((object) player.ID))
            {
              for (int index2 = 0; index2 < this.heroes.Count; ++index2)
              {
                HERO hero = (HERO) this.heroes[index2];
                if (hero.photonView.owner == player)
                {
                  hero.markDie();
                  hero.photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "no switching in infection");
                }
              }
            }
          }
          else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
            ++num41;
        }
      }
      if (num41 > 0 || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN)
        return;
      this.gameWin2();
    }
    else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
    {
      if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
      {
        if (this.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
        {
          this.photonView.RPC("Chat", PhotonTargets.All, (object) "<color=#00FFFF>Team Cyan wins! </color>", (object) string.Empty);
          this.gameWin2();
        }
        else
        {
          if (this.magentaKills < SettingsManager.LegacyGameSettings.PointModeAmount.Value)
            return;
          this.photonView.RPC("Chat", PhotonTargets.All, (object) "<color=#FF00FF>Team Magenta wins! </color>", (object) string.Empty);
          this.gameWin2();
        }
      }
      else
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
          return;
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
          {
            this.photonView.RPC("Chat", PhotonTargets.All, (object) ("<color=#FFCC00>" + RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]).hexColor() + " wins!</color>"), (object) string.Empty);
            this.gameWin2();
          }
        }
      }
    }
    else
    {
      if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value || !SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BladePVP.Value <= 0)
        return;
      if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
      {
        int num42 = 0;
        int num43 = 0;
        int num44 = 0;
        int num45 = 0;
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          if (!FengGameManagerMKII.ignoreList.Contains(player.ID) && player.customProperties[(object) PhotonPlayerProperty.RCteam] != null && player.customProperties[(object) PhotonPlayerProperty.dead] != null)
          {
            if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
            {
              ++num44;
              if (!RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
                ++num42;
            }
            else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2)
            {
              ++num45;
              if (!RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
                ++num43;
            }
          }
        }
        if (num44 <= 0 || num45 <= 0)
          return;
        if (num42 == 0)
        {
          this.photonView.RPC("Chat", PhotonTargets.All, (object) "<color=#FF00FF>Team Magenta wins! </color>", (object) string.Empty);
          this.gameWin2();
        }
        else
        {
          if (num43 != 0)
            return;
          this.photonView.RPC("Chat", PhotonTargets.All, (object) "<color=#00FFFF>Team Cyan wins! </color>", (object) string.Empty);
          this.gameWin2();
        }
      }
      else
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0 || PhotonNetwork.playerList.Length <= 1)
          return;
        int num46 = 0;
        string text = "Nobody";
        PhotonPlayer player1 = PhotonNetwork.playerList[0];
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player2 = PhotonNetwork.playerList[index];
          if (player2.customProperties[(object) PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(player2.customProperties[(object) PhotonPlayerProperty.dead]))
          {
            text = RCextensions.returnStringFromObject(player2.customProperties[(object) PhotonPlayerProperty.name]).hexColor();
            player1 = player2;
            ++num46;
          }
        }
        if (num46 > 1)
          return;
        string str14 = " 5 points added.";
        if (text == "Nobody")
        {
          str14 = string.Empty;
        }
        else
        {
          for (int index = 0; index < 5; ++index)
            this.playerKillInfoUpdate(player1, 0);
        }
        this.photonView.RPC("Chat", PhotonTargets.All, (object) ("<color=#FFCC00>" + text.hexColor() + " wins." + str14 + "</color>"), (object) string.Empty);
        this.gameWin2();
      }
    }
  }

  private void kickPhotonPlayer(string name)
  {
    MonoBehaviour.print((object) ("KICK " + name + "!!!"));
    foreach (PhotonPlayer player in PhotonNetwork.playerList)
    {
      if (player.ID.ToString() == name && !player.isMasterClient)
      {
        PhotonNetwork.CloseConnection(player);
        break;
      }
    }
  }

  private void kickPlayer(string kickPlayer, string kicker)
  {
    bool flag = false;
    for (int index = 0; index < this.kicklist.Count; ++index)
    {
      if (((KickState) this.kicklist[index]).name == kickPlayer)
      {
        KickState tmp = (KickState) this.kicklist[index];
        tmp.addKicker(kicker);
        this.tryKick(tmp);
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    KickState tmp1 = new KickState();
    tmp1.init(kickPlayer);
    tmp1.addKicker(kicker);
    this.kicklist.Add((object) tmp1);
    this.tryKick(tmp1);
  }

  public void kickPlayerRCIfMC(PhotonPlayer player, bool ban, string reason)
  {
    if (!PhotonNetwork.isMasterClient)
      return;
    this.kickPlayerRC(player, ban, reason);
  }

  public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
  {
    string empty;
    if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
    {
      empty = string.Empty;
      string inGameName = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
      FengGameManagerMKII.ServerCloseConnection(player, ban, inGameName);
    }
    else if (PhotonNetwork.isMasterClient && player == PhotonNetwork.player && reason != string.Empty)
    {
      this.chatRoom.addLINE("Attempting to ban myself for:" + reason + ", please report this to the devs.");
    }
    else
    {
      PhotonNetwork.DestroyPlayerObjects(player);
      PhotonNetwork.CloseConnection(player);
      this.photonView.RPC("ignorePlayer", PhotonTargets.Others, (object) player.ID);
      if (!FengGameManagerMKII.ignoreList.Contains(player.ID))
      {
        FengGameManagerMKII.ignoreList.Add(player.ID);
        PhotonNetwork.RaiseEvent((byte) 254, (object) null, true, new RaiseEventOptions()
        {
          TargetActors = new int[1]{ player.ID }
        });
      }
      if (ban && !((Dictionary<object, object>) FengGameManagerMKII.banHash).ContainsKey((object) player.ID))
      {
        empty = string.Empty;
        string str = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
        ((Dictionary<object, object>) FengGameManagerMKII.banHash).Add((object) player.ID, (object) str);
      }
      if (reason != string.Empty)
        this.chatRoom.addLINE("Player " + player.ID.ToString() + " was autobanned. Reason:" + reason);
      this.RecompilePlayerList(0.1f);
    }
  }

  [RPC]
  private void labelRPC(int setting, PhotonMessageInfo info)
  {
    if (!Object.op_Inequality((Object) PhotonView.Find(setting), (Object) null))
      return;
    PhotonPlayer owner = PhotonView.Find(setting).owner;
    if (owner != info.sender)
      return;
    string str1 = RCextensions.returnStringFromObject(owner.customProperties[(object) PhotonPlayerProperty.guildName]);
    string str2 = RCextensions.returnStringFromObject(owner.customProperties[(object) PhotonPlayerProperty.name]);
    GameObject gameObject = ((Component) PhotonView.Find(setting)).gameObject;
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    HERO component = gameObject.GetComponent<HERO>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    if (str1 != string.Empty)
      component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + str1 + "\n[FFFFFF]" + str2;
    else
      component.myNetWorkName.GetComponent<UILabel>().text = str2;
  }

  private void LateUpdate()
  {
    if (!this.gameStart)
      return;
    foreach (HERO hero in this.heroes)
      hero.lateUpdate2();
    foreach (TITAN_EREN titanEren in this.eT)
      titanEren.lateUpdate();
    foreach (TITAN titan in this.titans)
      titan.lateUpdate2();
    foreach (FEMALE_TITAN femaleTitan in this.fT)
      femaleTitan.lateUpdate2();
    this.core2();
  }

  private void loadconfig()
  {
    object[] objArray = new object[500];
    objArray[31] = (object) 0;
    objArray[64] = (object) 0;
    objArray[68] = (object) 100;
    objArray[69] = (object) "default";
    objArray[70] = (object) "1";
    objArray[71] = (object) "1";
    objArray[72] = (object) "1";
    objArray[73] = (object) 1f;
    objArray[74] = (object) 1f;
    objArray[75] = (object) 1f;
    objArray[76] = (object) 0;
    objArray[77] = (object) string.Empty;
    objArray[78] = (object) 0;
    objArray[79] = (object) "1.0";
    objArray[80] = (object) "1.0";
    objArray[81] = (object) 0;
    objArray[83] = (object) "30";
    objArray[84] = (object) 0;
    objArray[91] = (object) 0;
    objArray[100] = (object) 0;
    objArray[185] = (object) 0;
    objArray[186] = (object) 0;
    objArray[187] = (object) 0;
    objArray[188] = (object) 0;
    objArray[190] = (object) 0;
    objArray[191] = (object) string.Empty;
    objArray[230] = (object) 0;
    objArray[263] = (object) 0;
    FengGameManagerMKII.linkHash = new Hashtable[5]
    {
      new Hashtable(),
      new Hashtable(),
      new Hashtable(),
      new Hashtable(),
      new Hashtable()
    };
    FengGameManagerMKII.settingsOld = objArray;
    this.scroll = Vector2.zero;
    this.scroll2 = Vector2.zero;
    this.transparencySlider = 1f;
    SettingsManager.LegacyGeneralSettings.SetDefault();
    MaterialCache.Clear();
  }

  private void loadskin()
  {
    if ((int) FengGameManagerMKII.settingsOld[64] >= 100)
    {
      string[] strArray = new string[5]
      {
        "Flare",
        "LabelInfoBottomRight",
        "LabelNetworkStatus",
        "skill_cd_bottom",
        "GasUI"
      };
      foreach (GameObject gameObject in (GameObject[]) Object.FindObjectsOfType(typeof (GameObject)))
      {
        if (((Object) gameObject).name.Contains("TREE") || ((Object) gameObject).name.Contains("aot_supply") || ((Object) gameObject).name.Contains("gameobjectOutSide"))
          Object.Destroy((Object) gameObject);
      }
      GameObject.Find("Cube_001").renderer.material.mainTexture = ((Material) FengGameManagerMKII.RCassets.Load("grass")).mainTexture;
      Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0.0f, 0.0f, 0.0f, 1f));
      for (int index = 0; index < strArray.Length; ++index)
      {
        GameObject gameObject = GameObject.Find(strArray[index]);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          Object.Destroy((Object) gameObject);
      }
      ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = true;
    }
    else
    {
      InstantiateTracker.instance.Dispose();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
      {
        this.updateTime = 1f;
        if (FengGameManagerMKII.oldScriptLogic != SettingsManager.LegacyGameSettings.LogicScript.Value)
        {
          ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.globalVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
          ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Clear();
          FengGameManagerMKII.oldScriptLogic = SettingsManager.LegacyGameSettings.LogicScript.Value;
          this.compileScript(SettingsManager.LegacyGameSettings.LogicScript.Value);
          if (((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) "OnFirstLoad"))
            ((RCEvent) FengGameManagerMKII.RCEvents[(object) "OnFirstLoad"]).checkEvent();
        }
        if (((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) "OnRoundStart"))
          ((RCEvent) FengGameManagerMKII.RCEvents[(object) "OnRoundStart"]).checkEvent();
        this.photonView.RPC("setMasterRC", PhotonTargets.All);
      }
      FengGameManagerMKII.logicLoaded = true;
      this.racingSpawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
      this.racingSpawnPointSet = false;
      this.racingDoors = new List<GameObject>();
      this.allowedToCannon = new Dictionary<int, CannonValues>();
      bool flag = false;
      string[] skybox = new string[6]
      {
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty
      };
      if (SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value)
      {
        SkyboxCustomSkinSet selectedSet = (SkyboxCustomSkinSet) SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
        skybox = new string[6]
        {
          selectedSet.Front.Value,
          selectedSet.Back.Value,
          selectedSet.Left.Value,
          selectedSet.Right.Value,
          selectedSet.Up.Value,
          selectedSet.Down.Value
        };
        flag = true;
      }
      if (!FengGameManagerMKII.level.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
      {
        string n = string.Empty;
        string url = string.Empty;
        string url2 = string.Empty;
        if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City") && SettingsManager.CustomSkinSettings.City.SkinsEnabled.Value)
        {
          CityCustomSkinSet selectedSet = (CityCustomSkinSet) SettingsManager.CustomSkinSettings.City.GetSelectedSet();
          List<string> stringList = new List<string>();
          foreach (StringSetting stringSetting in selectedSet.Houses.GetItems())
            stringList.Add(stringSetting.Value);
          url = string.Join(",", stringList.ToArray());
          for (int index = 0; index < 250; ++index)
            n += Convert.ToString((int) Random.Range(0.0f, 8f));
          url2 = string.Join(",", new string[3]
          {
            selectedSet.Ground.Value,
            selectedSet.Wall.Value,
            selectedSet.Gate.Value
          });
          flag = true;
        }
        else if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest") && SettingsManager.CustomSkinSettings.Forest.SkinsEnabled.Value)
        {
          ForestCustomSkinSet selectedSet = (ForestCustomSkinSet) SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
          List<string> stringList1 = new List<string>();
          foreach (StringSetting stringSetting in selectedSet.TreeTrunks.GetItems())
            stringList1.Add(stringSetting.Value);
          url = string.Join(",", stringList1.ToArray());
          List<string> stringList2 = new List<string>();
          foreach (StringSetting stringSetting in selectedSet.TreeLeafs.GetItems())
            stringList2.Add(stringSetting.Value);
          stringList2.Add(selectedSet.Ground.Value);
          url2 = string.Join(",", stringList2.ToArray());
          for (int index = 0; index < 150; ++index)
          {
            string str1 = Convert.ToString((int) Random.Range(0.0f, 8f));
            string str2 = n + str1;
            n = selectedSet.RandomizedPairs.Value ? str2 + Convert.ToString((int) Random.Range(0.0f, 8f)) : str2 + str1;
          }
          flag = true;
        }
        if (!flag)
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          this.StartCoroutine(this.loadskinE(n, url, url2, skybox));
        }
        else
        {
          if (!PhotonNetwork.isMasterClient)
            return;
          this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) n, (object) url, (object) url2, (object) skybox);
        }
      }
      else
      {
        if (!FengGameManagerMKII.level.StartsWith("Custom") || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
          return;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("playerRespawn"))
          gameObject.transform.position = new Vector3(Random.Range(-5f, 5f), 0.0f, Random.Range(-5f, 5f));
        foreach (GameObject gameObject in (GameObject[]) Object.FindObjectsOfType(typeof (GameObject)))
        {
          if (((Object) gameObject).name.Contains("TREE") || ((Object) gameObject).name.Contains("aot_supply"))
            Object.Destroy((Object) gameObject);
          else if (((Object) gameObject).name == "Cube_001" && ((Component) gameObject.transform.parent).gameObject.tag != "player" && Object.op_Inequality((Object) gameObject.renderer, (Object) null))
          {
            this.groundList.Add(gameObject);
            gameObject.renderer.material.mainTexture = ((Material) FengGameManagerMKII.RCassets.Load("grass")).mainTexture;
          }
        }
        if (!PhotonNetwork.isMasterClient)
          return;
        string[] strArray1 = new string[7];
        for (int index = 0; index < 6; ++index)
          strArray1[index] = skybox[index];
        strArray1[6] = ((CustomLevelCustomSkinSet) SettingsManager.CustomSkinSettings.CustomLevel.GetSelectedSet()).Ground.Value;
        SettingsManager.LegacyGameSettings.TitanSpawnCap.Value = Math.Min(100, SettingsManager.LegacyGameSettings.TitanSpawnCap.Value);
        this.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, (object) strArray1, (object) SettingsManager.LegacyGameSettings.GameType.Value);
        ((Dictionary<object, object>) FengGameManagerMKII.RCRegions).Clear();
        if (FengGameManagerMKII.oldScript != SettingsManager.LegacyGameSettings.LevelScript.Value)
        {
          this.levelCache.Clear();
          this.titanSpawns.Clear();
          this.playerSpawnsC.Clear();
          this.playerSpawnsM.Clear();
          this.titanSpawners.Clear();
          FengGameManagerMKII.currentLevel = string.Empty;
          if (SettingsManager.LegacyGameSettings.LevelScript.Value == string.Empty)
          {
            Hashtable propertiesToSet = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) FengGameManagerMKII.currentLevel);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            FengGameManagerMKII.oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
          }
          else
          {
            string[] strArray2 = Regex.Replace(SettingsManager.LegacyGameSettings.LevelScript.Value, "\\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(';');
            for (int index1 = 0; index1 < Mathf.FloorToInt((float) ((strArray2.Length - 1) / 100)) + 1; ++index1)
            {
              if (index1 < Mathf.FloorToInt((float) (strArray2.Length / 100)))
              {
                string[] strArray3 = new string[101];
                int index2 = 0;
                for (int index3 = 100 * index1; index3 < 100 * index1 + 100; ++index3)
                {
                  if (strArray2[index3].StartsWith("spawnpoint"))
                  {
                    string[] strArray4 = strArray2[index3].Split(',');
                    if (strArray4[1] == "titan")
                      this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray4[2]), Convert.ToSingle(strArray4[3]), Convert.ToSingle(strArray4[4])));
                    else if (strArray4[1] == "playerC")
                      this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray4[2]), Convert.ToSingle(strArray4[3]), Convert.ToSingle(strArray4[4])));
                    else if (strArray4[1] == "playerM")
                      this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray4[2]), Convert.ToSingle(strArray4[3]), Convert.ToSingle(strArray4[4])));
                  }
                  strArray3[index2] = strArray2[index3];
                  ++index2;
                }
                string str = Random.Range(10000, 99999).ToString();
                strArray3[100] = str;
                FengGameManagerMKII.currentLevel += str;
                this.levelCache.Add(strArray3);
              }
              else
              {
                string[] strArray5 = new string[strArray2.Length % 100 + 1];
                int index4 = 0;
                for (int index5 = 100 * index1; index5 < 100 * index1 + strArray2.Length % 100; ++index5)
                {
                  if (strArray2[index5].StartsWith("spawnpoint"))
                  {
                    string[] strArray6 = strArray2[index5].Split(',');
                    if (strArray6[1] == "titan")
                      this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                    else if (strArray6[1] == "playerC")
                      this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                    else if (strArray6[1] == "playerM")
                      this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                  }
                  strArray5[index4] = strArray2[index5];
                  ++index4;
                }
                string str = Random.Range(10000, 99999).ToString();
                strArray5[strArray2.Length % 100] = str;
                FengGameManagerMKII.currentLevel += str;
                this.levelCache.Add(strArray5);
              }
            }
            List<string> stringList3 = new List<string>();
            foreach (Vector3 titanSpawn in this.titanSpawns)
            {
              List<string> stringList4 = stringList3;
              string[] strArray7 = new string[6];
              strArray7[0] = "titan,";
              float num = titanSpawn.x;
              strArray7[1] = num.ToString();
              strArray7[2] = ",";
              num = titanSpawn.y;
              strArray7[3] = num.ToString();
              strArray7[4] = ",";
              strArray7[5] = titanSpawn.z.ToString();
              string str = string.Concat(strArray7);
              stringList4.Add(str);
            }
            foreach (Vector3 vector3 in this.playerSpawnsC)
            {
              List<string> stringList5 = stringList3;
              string[] strArray8 = new string[6];
              strArray8[0] = "playerC,";
              float num = vector3.x;
              strArray8[1] = num.ToString();
              strArray8[2] = ",";
              num = vector3.y;
              strArray8[3] = num.ToString();
              strArray8[4] = ",";
              strArray8[5] = vector3.z.ToString();
              string str = string.Concat(strArray8);
              stringList5.Add(str);
            }
            foreach (Vector3 vector3 in this.playerSpawnsM)
            {
              List<string> stringList6 = stringList3;
              string[] strArray9 = new string[6];
              strArray9[0] = "playerM,";
              float num = vector3.x;
              strArray9[1] = num.ToString();
              strArray9[2] = ",";
              num = vector3.y;
              strArray9[3] = num.ToString();
              strArray9[4] = ",";
              strArray9[5] = vector3.z.ToString();
              string str = string.Concat(strArray9);
              stringList6.Add(str);
            }
            int num1 = Random.Range(10000, 99999);
            string str3 = "a" + num1.ToString();
            stringList3.Add(str3);
            FengGameManagerMKII.currentLevel = str3 + FengGameManagerMKII.currentLevel;
            this.levelCache.Insert(0, stringList3.ToArray());
            num1 = Random.Range(10000, 99999);
            string str4 = "z" + num1.ToString();
            this.levelCache.Add(new string[1]{ str4 });
            FengGameManagerMKII.currentLevel += str4;
            Hashtable propertiesToSet = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) FengGameManagerMKII.currentLevel);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            FengGameManagerMKII.oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
          }
        }
        for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index];
          if (!player.isMasterClient)
            this.playersRPC.Add(player);
        }
        this.StartCoroutine(this.customlevelE(this.playersRPC));
        this.StartCoroutine(this.customlevelcache());
      }
    }
  }

  private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
  {
    FengGameManagerMKII fengGameManagerMkii = this;
    if (fengGameManagerMkii.IsValidSkybox(skybox))
      yield return (object) fengGameManagerMkii.StartCoroutine(fengGameManagerMkii._skyboxCustomSkinLoader.LoadSkinsFromRPC((object[]) skybox));
    else
      SkyboxCustomSkinLoader.SkyboxMaterial = (Material) null;
    if (n != string.Empty)
    {
      if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest"))
        yield return (object) fengGameManagerMkii.StartCoroutine(fengGameManagerMkii._forestCustomSkinLoader.LoadSkinsFromRPC(new object[3]
        {
          (object) n,
          (object) url,
          (object) url2
        }));
      else if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City"))
        yield return (object) fengGameManagerMkii.StartCoroutine(fengGameManagerMkii._cityCustomSkinLoader.LoadSkinsFromRPC(new object[3]
        {
          (object) n,
          (object) url,
          (object) url2
        }));
    }
    Minimap.TryRecaptureInstance();
    fengGameManagerMkii.StartCoroutine(fengGameManagerMkii.reloadSky());
    yield return (object) null;
  }

  private bool IsValidSkybox(string[] skybox)
  {
    foreach (string url in skybox)
    {
      if (TextureDownloader.ValidTextureURL(url))
        return true;
    }
    return false;
  }

  [RPC]
  private void loadskinRPC(
    string n,
    string url1,
    string url2,
    string[] skybox,
    PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest"))
    {
      BaseCustomSkinSettings<ForestCustomSkinSet> forest = SettingsManager.CustomSkinSettings.Forest;
      if (!forest.SkinsEnabled.Value || forest.SkinsLocal.Value && !PhotonNetwork.isMasterClient)
        return;
      this.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
    }
    else
    {
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City"))
        return;
      BaseCustomSkinSettings<CityCustomSkinSet> city = SettingsManager.CustomSkinSettings.City;
      if (!city.SkinsEnabled.Value || city.SkinsLocal.Value && !PhotonNetwork.isMasterClient)
        return;
      this.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
    }
  }

  private IEnumerator loginFeng()
  {
    WWWForm wwwForm = new WWWForm();
    wwwForm.AddField("userid", FengGameManagerMKII.usernameField);
    wwwForm.AddField("password", FengGameManagerMKII.passwordField);
    WWW iteratorVariable1 = !Application.isWebPlayer ? new WWW("http://fenglee.com/game/aog/require_user_info.php", wwwForm) : new WWW("http://aotskins.com/version/getinfo.php", wwwForm);
    yield return (object) iteratorVariable1;
    if (iteratorVariable1.error == null && !iteratorVariable1.text.Contains("Error,please sign in again."))
    {
      char[] chArray = new char[1]{ '|' };
      string[] strArray = iteratorVariable1.text.Split(chArray);
      LoginFengKAI.player.name = FengGameManagerMKII.usernameField;
      LoginFengKAI.player.guildname = strArray[0];
      FengGameManagerMKII.loginstate = 3;
    }
    else
      FengGameManagerMKII.loginstate = 2;
  }

  private string mastertexturetype(int lol)
  {
    if (lol == 0)
      return "High";
    return lol == 1 ? "Med" : "Low";
  }

  public void multiplayerRacingFinsih()
  {
    float time = this.roundTime - SettingsManager.LegacyGameSettings.RacingStartTime.Value;
    if (PhotonNetwork.isMasterClient)
      this.getRacingResult(LoginFengKAI.player.name, time, (PhotonMessageInfo) null);
    else
      this.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, (object) LoginFengKAI.player.name, (object) time);
    this.gameWin2();
  }

  [RPC]
  private void netGameLose(int score, PhotonMessageInfo info)
  {
    this.isLosing = true;
    this.titanScore = score;
    this.gameEndCD = this.gameEndTotalCDtime;
    if (SettingsManager.UISettings.GameFeed.Value)
      this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
    if (info.sender == PhotonNetwork.masterClient || info.sender.isLocal || !PhotonNetwork.isMasterClient)
      return;
    this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
  }

  [RPC]
  private void netGameWin(int score, PhotonMessageInfo info)
  {
    this.humanScore = score;
    this.isWinning = true;
    switch (IN_GAME_MAIN_CAMERA.gamemode)
    {
      case GAMEMODE.PVP_AHSS:
        this.teamWinner = score;
        ++this.teamScores[this.teamWinner - 1];
        this.gameEndCD = this.gameEndTotalCDtime;
        break;
      case GAMEMODE.RACING:
        this.gameEndCD = !SettingsManager.LegacyGameSettings.RacingEndless.Value ? 20f : 1000f;
        break;
      default:
        this.gameEndCD = this.gameEndTotalCDtime;
        break;
    }
    if (SettingsManager.UISettings.GameFeed.Value)
      this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
    if (info.sender == PhotonNetwork.masterClient || info.sender.isLocal)
      return;
    this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
  }

  [RPC]
  private void netRefreshRacingResult(string tmp) => this.localRacingResult = tmp;

  [RPC]
  public void netShowDamage(int speed)
  {
    GameObject.Find("Stylish").GetComponent<StylishComponent>().Style(speed);
    GameObject target = GameObject.Find("LabelScore");
    if (!Object.op_Inequality((Object) target, (Object) null))
      return;
    target.GetComponent<UILabel>().text = speed.ToString();
    target.transform.localScale = Vector3.zero;
    speed = (int) ((double) speed * 0.10000000149011612);
    speed = Mathf.Max(40, speed);
    speed = Mathf.Min(150, speed);
    iTween.Stop(target);
    object[] objArray1 = new object[10]
    {
      (object) "x",
      (object) speed,
      (object) "y",
      (object) speed,
      (object) "z",
      (object) speed,
      (object) "easetype",
      (object) iTween.EaseType.easeOutElastic,
      (object) "time",
      (object) 1f
    };
    iTween.ScaleTo(target, iTween.Hash(objArray1));
    object[] objArray2 = new object[12]
    {
      (object) "x",
      (object) 0,
      (object) "y",
      (object) 0,
      (object) "z",
      (object) 0,
      (object) "easetype",
      (object) iTween.EaseType.easeInBounce,
      (object) "time",
      (object) 0.5f,
      (object) "delay",
      (object) 2f
    };
    iTween.ScaleTo(target, iTween.Hash(objArray2));
  }

  public void NOTSpawnNonAITitan(string id)
  {
    this.myLastHero = id.ToUpper();
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
    ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
  }

  public void NOTSpawnNonAITitanRC(string id)
  {
    this.myLastHero = id.ToUpper();
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    this.ShowHUDInfoCenter("Syncing spawn locations...");
    ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
  }

  public void NOTSpawnPlayer(string id)
  {
    this.myLastHero = id.ToUpper();
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
    ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
  }

  public void NOTSpawnPlayerRC(string id)
  {
    this.myLastHero = id.ToUpper();
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
  }

  public void OnConnectedToMaster() => MonoBehaviour.print((object) nameof (OnConnectedToMaster));

  public void OnConnectedToPhoton() => MonoBehaviour.print((object) nameof (OnConnectedToPhoton));

  public void OnConnectionFail(DisconnectCause cause)
  {
    MonoBehaviour.print((object) ("OnConnectionFail : " + cause.ToString()));
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
    this.gameStart = false;
    NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], false);
    NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
    NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
    NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
    NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[4], true);
    GameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail : " + cause.ToString();
  }

  public void OnCreatedRoom()
  {
    this.kicklist = new ArrayList();
    this.racingResult = new ArrayList();
    this.teamScores = new int[2];
    MonoBehaviour.print((object) nameof (OnCreatedRoom));
  }

  public void OnCustomAuthenticationFailed() => MonoBehaviour.print((object) nameof (OnCustomAuthenticationFailed));

  public void OnDisconnectedFromPhoton() => MonoBehaviour.print((object) nameof (OnDisconnectedFromPhoton));

  [RPC]
  public void oneTitanDown(string name1, bool onPlayerLeave)
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !PhotonNetwork.isMasterClient)
      return;
    switch (IN_GAME_MAIN_CAMERA.gamemode)
    {
      case GAMEMODE.KILL_TITAN:
        if (!this.checkIsTitanAllDie())
          break;
        this.gameWin2();
        ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        break;
      case GAMEMODE.CAGE_FIGHT:
        break;
      case GAMEMODE.ENDLESS_TITAN:
        if (onPlayerLeave)
          break;
        ++this.humanScore;
        int abnormal1 = 90;
        if (this.difficulty == 1)
          abnormal1 = 70;
        this.spawnTitanCustom("titanRespawn", abnormal1, 1, false);
        break;
      case GAMEMODE.SURVIVE_MODE:
        if (!this.checkIsTitanAllDie())
          break;
        ++this.wave;
        if ((LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEWROUND || FengGameManagerMKII.level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value == 1) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          foreach (PhotonPlayer player in PhotonNetwork.playerList)
          {
            if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2)
              this.photonView.RPC("respawnHeroInNewRound", player);
          }
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave.ToString() + "</color>");
        if (this.wave > this.highestwave)
          this.highestwave = this.wave;
        if (PhotonNetwork.isMasterClient)
          this.RequireStatus();
        if (!SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && this.wave > 20 || SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && this.wave > SettingsManager.LegacyGameSettings.TitanMaxWaves.Value)
        {
          this.gameWin2();
          break;
        }
        int abnormal2 = 90;
        if (this.difficulty == 1)
          abnormal2 = 70;
        if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
        {
          this.spawnTitanCustom("titanRespawn", abnormal2, this.wave + 2, false);
          break;
        }
        if (this.wave == 5)
        {
          this.spawnTitanCustom("titanRespawn", abnormal2, 1, true);
          break;
        }
        if (this.wave == 10)
        {
          this.spawnTitanCustom("titanRespawn", abnormal2, 2, true);
          break;
        }
        if (this.wave == 15)
        {
          this.spawnTitanCustom("titanRespawn", abnormal2, 3, true);
          break;
        }
        if (this.wave == 20)
        {
          this.spawnTitanCustom("titanRespawn", abnormal2, 4, true);
          break;
        }
        this.spawnTitanCustom("titanRespawn", abnormal2, this.wave + 2, false);
        break;
      case GAMEMODE.PVP_CAPTURE:
        if (name1 != string.Empty)
        {
          switch (name1)
          {
            case "Titan":
              ++this.PVPhumanScore;
              break;
            case "Aberrant":
              this.PVPhumanScore += 2;
              break;
            case "Jumper":
              this.PVPhumanScore += 3;
              break;
            case "Crawler":
              this.PVPhumanScore += 4;
              break;
            case "Female Titan":
              this.PVPhumanScore += 10;
              break;
            default:
              this.PVPhumanScore += 3;
              break;
          }
        }
        this.checkPVPpts();
        this.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, (object) this.PVPhumanScore, (object) this.PVPtitanScore);
        break;
      default:
        int enemyNumber = LevelInfo.getInfo(FengGameManagerMKII.level).enemyNumber;
        break;
    }
  }

  public void OnFailedToConnectToPhoton() => MonoBehaviour.print((object) nameof (OnFailedToConnectToPhoton));

  private void DrawBackgroundIfLoading()
  {
    if (AssetBundleManager.Status != AssetBundleStatus.Loading && AutoUpdateManager.Status != AutoUpdateStatus.Updating)
      return;
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), (Texture) this.textureBackgroundBlue);
  }

  public void OnGUI()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP && Application.loadedLevelName != "characterCreation")
    {
      LegacyPopupTemplate legacyPopupTemplate = new LegacyPopupTemplate(new Color(0.0f, 0.0f, 0.0f, 1f), this.textureBackgroundBlack, new Color(1f, 1f, 1f, 1f), (float) Screen.width / 2f, (float) Screen.height / 2f, 230f, 140f, 2f);
      this.DrawBackgroundIfLoading();
      if (AutoUpdateManager.Status == AutoUpdateStatus.Updating)
        legacyPopupTemplate.DrawPopup("Auto-updating mod...", 130f, 22f);
      else if (AutoUpdateManager.Status == AutoUpdateStatus.NeedRestart && !AutoUpdateManager.CloseFailureBox)
      {
        bool[] flagArray = legacyPopupTemplate.DrawPopupWithTwoButtons("Mod has been updated and requires a restart.", 190f, 44f, "Restart Now", 90f, "Ignore", 90f, 25f);
        if (flagArray[0])
        {
          if (Application.platform == 2)
            Process.Start(Application.dataPath.Replace("_Data", ".exe"));
          else if (Application.platform == 1)
            Process.Start(Application.dataPath + "/MacOS/MacTest");
          Application.Quit();
        }
        else
        {
          if (!flagArray[1])
            return;
          AutoUpdateManager.CloseFailureBox = true;
        }
      }
      else if (AutoUpdateManager.Status == AutoUpdateStatus.LauncherOutdated && !AutoUpdateManager.CloseFailureBox)
      {
        if (!legacyPopupTemplate.DrawPopupWithButton("Game launcher is outdated, visit aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
          return;
        AutoUpdateManager.CloseFailureBox = true;
      }
      else if (AutoUpdateManager.Status == AutoUpdateStatus.FailedUpdate && !AutoUpdateManager.CloseFailureBox)
      {
        if (!legacyPopupTemplate.DrawPopupWithButton("Auto-update failed, check internet connection or aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
          return;
        AutoUpdateManager.CloseFailureBox = true;
      }
      else if (AutoUpdateManager.Status == AutoUpdateStatus.MacTranslocated && !AutoUpdateManager.CloseFailureBox)
      {
        if (!legacyPopupTemplate.DrawPopupWithButton("Your game is not in the Applications folder, cannot auto-update and some bugs may occur.", 190f, 66f, "Continue", 80f, 25f))
          return;
        AutoUpdateManager.CloseFailureBox = true;
      }
      else
      {
        switch (AssetBundleManager.Status)
        {
          case AssetBundleStatus.Loading:
            legacyPopupTemplate.DrawPopup("Downloading asset bundle...", 170f, 22f);
            break;
          case AssetBundleStatus.Failed:
            if (AssetBundleManager.CloseFailureBox || !legacyPopupTemplate.DrawPopupWithButton("Failed to load asset bundle, check your internet connection.", 190f, 44f, "Continue", 80f, 25f))
              break;
            AssetBundleManager.CloseFailureBox = true;
            break;
        }
      }
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
        return;
      if ((int) FengGameManagerMKII.settingsOld[64] >= 100)
      {
        float num1 = (float) Screen.width - 300f;
        GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
        GUI.DrawTexture(new Rect(7f, 7f, 291f, 586f), (Texture) this.textureBackgroundBlue);
        GUI.DrawTexture(new Rect(num1 + 2f, 7f, 291f, 586f), (Texture) this.textureBackgroundBlue);
        bool flag1 = false;
        bool flag2 = false;
        GUI.Box(new Rect(5f, 5f, 295f, 590f), string.Empty);
        GUI.Box(new Rect(num1, 5f, 295f, 590f), string.Empty);
        if (GUI.Button(new Rect(10f, 10f, 60f, 25f), "Script", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[68] = (object) 100;
        if (GUI.Button(new Rect(75f, 10f, 80f, 25f), "Full Screen", GUIStyle.op_Implicit("box")))
          FullscreenHandler.ToggleFullscreen();
        Color color;
        if ((int) FengGameManagerMKII.settingsOld[68] == 100 || (int) FengGameManagerMKII.settingsOld[68] == 102)
        {
          GUI.Label(new Rect(115f, 40f, 100f, 20f), "Level Script:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(115f, 115f, 100f, 20f), "Import Data", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(12f, 535f, 280f, 60f), "Warning: your current level will be lost if you quit or import data. Make sure to save the level to a text document.", GUIStyle.op_Implicit("Label"));
          FengGameManagerMKII.settingsOld[77] = (object) GUI.TextField(new Rect(10f, 140f, 285f, 350f), (string) FengGameManagerMKII.settingsOld[77]);
          if (GUI.Button(new Rect(35f, 500f, 60f, 30f), "Apply"))
          {
            foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
            {
              if (((Object) gameObject).name.StartsWith("custom") || ((Object) gameObject).name.StartsWith("base") || ((Object) gameObject).name.StartsWith("photon") || ((Object) gameObject).name.StartsWith("spawnpoint") || ((Object) gameObject).name.StartsWith("misc") || ((Object) gameObject).name.StartsWith("racing"))
                Object.Destroy((Object) gameObject);
            }
            ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Clear();
            FengGameManagerMKII.settingsOld[186] = (object) 0;
            string[] strArray1 = Regex.Replace((string) FengGameManagerMKII.settingsOld[77], "\\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(';');
            for (int index1 = 0; index1 < strArray1.Length; ++index1)
            {
              string[] strArray2 = strArray1[index1].Split(',');
              if (strArray2[0].StartsWith("custom") || strArray2[0].StartsWith("base") || strArray2[0].StartsWith("photon") || strArray2[0].StartsWith("spawnpoint") || strArray2[0].StartsWith("misc") || strArray2[0].StartsWith("racing"))
              {
                GameObject gameObject1 = (GameObject) null;
                if (strArray2[0].StartsWith("custom"))
                  gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[16]), Convert.ToSingle(strArray2[17]), Convert.ToSingle(strArray2[18])));
                else if (strArray2[0].StartsWith("photon"))
                  gameObject1 = !strArray2[1].StartsWith("Cannon") ? (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[4]), Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6])), new Quaternion(Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]))) : (strArray2.Length >= 15 ? (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1] + "Prop"), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[16]), Convert.ToSingle(strArray2[17]), Convert.ToSingle(strArray2[18]))) : (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1] + "Prop"), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]))));
                else if (strArray2[0].StartsWith("spawnpoint"))
                  gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8])));
                else if (strArray2[0].StartsWith("base"))
                  gameObject1 = strArray2.Length >= 15 ? (GameObject) Object.Instantiate(Resources.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[16]), Convert.ToSingle(strArray2[17]), Convert.ToSingle(strArray2[18]))) : (GameObject) Object.Instantiate(Resources.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8])));
                else if (strArray2[0].StartsWith("misc"))
                {
                  if (strArray2[1].StartsWith("barrier"))
                    gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("barrierEditor"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                  else if (strArray2[1].StartsWith("region"))
                  {
                    gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("regionEditor"));
                    gameObject1.transform.position = new Vector3(Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]));
                    GameObject gameObject2 = (GameObject) Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                    ((Object) gameObject2).name = "RegionLabel";
                    gameObject2.transform.parent = gameObject1.transform;
                    float num2 = 1f;
                    if ((double) Convert.ToSingle(strArray2[4]) > 100.0)
                      num2 = 0.8f;
                    else if ((double) Convert.ToSingle(strArray2[4]) > 1000.0)
                      num2 = 0.5f;
                    gameObject2.transform.localPosition = new Vector3(0.0f, num2, 0.0f);
                    gameObject2.transform.localScale = new Vector3(5f / Convert.ToSingle(strArray2[3]), 5f / Convert.ToSingle(strArray2[4]), 5f / Convert.ToSingle(strArray2[5]));
                    gameObject2.GetComponent<UILabel>().text = strArray2[2];
                    gameObject1.AddComponent<RCRegionLabel>();
                    gameObject1.GetComponent<RCRegionLabel>().myLabel = gameObject2;
                  }
                  else if (strArray2[1].StartsWith("racingStart"))
                    gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("racingStart"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                  else if (strArray2[1].StartsWith("racingEnd"))
                    gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("racingEnd"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                }
                else if (strArray2[0].StartsWith("racing"))
                  gameObject1 = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                if (strArray2[2] != "default" && (strArray2[0].StartsWith("custom") || strArray2[0].StartsWith("base") && strArray2.Length > 15 || strArray2[0].StartsWith("photon") && strArray2.Length > 15))
                {
                  foreach (Renderer componentsInChild in gameObject1.GetComponentsInChildren<Renderer>())
                  {
                    if (!((Object) componentsInChild).name.Contains("Particle System") || !((Object) gameObject1).name.Contains("aot_supply"))
                    {
                      componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load(strArray2[2]);
                      componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray2[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray2[11]));
                    }
                  }
                }
                if (strArray2[0].StartsWith("custom") || strArray2[0].StartsWith("base") && strArray2.Length > 15 || strArray2[0].StartsWith("photon") && strArray2.Length > 15)
                {
                  float num3 = gameObject1.transform.localScale.x * Convert.ToSingle(strArray2[3]) - 1f / 1000f;
                  float num4 = gameObject1.transform.localScale.y * Convert.ToSingle(strArray2[4]);
                  float num5 = gameObject1.transform.localScale.z * Convert.ToSingle(strArray2[5]);
                  gameObject1.transform.localScale = new Vector3(num3, num4, num5);
                  if (strArray2[6] != "0")
                  {
                    // ISSUE: explicit constructor call
                    ((Color) ref color).\u002Ector(Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), 1f);
                    foreach (MeshFilter componentsInChild in gameObject1.GetComponentsInChildren<MeshFilter>())
                    {
                      Mesh mesh = componentsInChild.mesh;
                      Color[] colorArray = new Color[mesh.vertexCount];
                      for (int index2 = 0; index2 < mesh.vertexCount; ++index2)
                        colorArray[index2] = color;
                      mesh.colors = colorArray;
                    }
                  }
                  ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4] + "," + strArray2[5] + "," + strArray2[6] + "," + strArray2[7] + "," + strArray2[8] + "," + strArray2[9] + "," + strArray2[10] + "," + strArray2[11];
                }
                else if (strArray2[0].StartsWith("misc"))
                {
                  if (strArray2[1].StartsWith("barrier") || strArray2[1].StartsWith("racing"))
                  {
                    float num6 = gameObject1.transform.localScale.x * Convert.ToSingle(strArray2[2]) - 1f / 1000f;
                    float num7 = gameObject1.transform.localScale.y * Convert.ToSingle(strArray2[3]);
                    float num8 = gameObject1.transform.localScale.z * Convert.ToSingle(strArray2[4]);
                    gameObject1.transform.localScale = new Vector3(num6, num7, num8);
                    ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4];
                  }
                  else if (strArray2[1].StartsWith("region"))
                  {
                    float num9 = gameObject1.transform.localScale.x * Convert.ToSingle(strArray2[3]) - 1f / 1000f;
                    float num10 = gameObject1.transform.localScale.y * Convert.ToSingle(strArray2[4]);
                    float num11 = gameObject1.transform.localScale.z * Convert.ToSingle(strArray2[5]);
                    gameObject1.transform.localScale = new Vector3(num9, num10, num11);
                    ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4] + "," + strArray2[5];
                  }
                }
                else if (strArray2[0].StartsWith("racing"))
                {
                  float num12 = gameObject1.transform.localScale.x * Convert.ToSingle(strArray2[2]) - 1f / 1000f;
                  float num13 = gameObject1.transform.localScale.y * Convert.ToSingle(strArray2[3]);
                  float num14 = gameObject1.transform.localScale.z * Convert.ToSingle(strArray2[4]);
                  gameObject1.transform.localScale = new Vector3(num12, num13, num14);
                  ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4];
                }
                else if (strArray2[0].StartsWith("photon") && !strArray2[1].StartsWith("Cannon"))
                  ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3];
                else
                  ((Object) gameObject1).name = strArray2[0] + "," + strArray2[1];
                ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Add((object) ((Object) gameObject1).GetInstanceID(), (object) strArray1[index1]);
              }
              else if (strArray2[0].StartsWith("map") && strArray2[1].StartsWith("disablebounds"))
              {
                FengGameManagerMKII.settingsOld[186] = (object) 1;
                if (!((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).ContainsKey((object) "mapbounds"))
                  ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Add((object) "mapbounds", (object) "map,disablebounds");
              }
            }
            this.unloadAssets();
            FengGameManagerMKII.settingsOld[77] = (object) string.Empty;
          }
          else if (GUI.Button(new Rect(205f, 500f, 60f, 30f), "Exit"))
          {
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
            Application.LoadLevel("menu");
          }
          else if (GUI.Button(new Rect(15f, 70f, 115f, 30f), "Copy to Clipboard"))
          {
            string str1 = string.Empty;
            int num15 = 0;
            foreach (string str2 in ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Values)
            {
              ++num15;
              str1 = str1 + str2 + ";\n";
            }
            TextEditor textEditor = new TextEditor()
            {
              content = new GUIContent(str1)
            };
            textEditor.SelectAll();
            textEditor.Copy();
          }
          else if (GUI.Button(new Rect(175f, 70f, 115f, 30f), "View Script"))
            FengGameManagerMKII.settingsOld[68] = (object) 102;
          if ((int) FengGameManagerMKII.settingsOld[68] == 102)
          {
            string str3 = string.Empty;
            int num16 = 0;
            foreach (string str4 in ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Values)
            {
              ++num16;
              str3 = str3 + str4 + ";\n";
            }
            float num17 = (float) (Screen.width / 2) - 110.5f;
            float num18 = (float) (Screen.height / 2) - 250f;
            GUI.DrawTexture(new Rect(num17 + 2f, num18 + 2f, 217f, 496f), (Texture) this.textureBackgroundBlue);
            GUI.Box(new Rect(num17, num18, 221f, 500f), string.Empty);
            if (GUI.Button(new Rect(num17 + 10f, num18 + 460f, 60f, 30f), "Copy"))
            {
              TextEditor textEditor = new TextEditor()
              {
                content = new GUIContent(str3)
              };
              textEditor.SelectAll();
              textEditor.Copy();
            }
            else if (GUI.Button(new Rect(num17 + 151f, num18 + 460f, 60f, 30f), "Done"))
              FengGameManagerMKII.settingsOld[68] = (object) 100;
            GUI.TextArea(new Rect(num17 + 5f, num18 + 5f, 211f, 415f), str3);
            GUI.Label(new Rect(num17 + 10f, num18 + 430f, 150f, 20f), "Object Count: " + Convert.ToString(num16), GUIStyle.op_Implicit("Label"));
          }
        }
        if ((int) FengGameManagerMKII.settingsOld[64] != 105 && (int) FengGameManagerMKII.settingsOld[64] != 106)
        {
          GUI.Label(new Rect(num1 + 13f, 445f, 125f, 20f), "Scale Multipliers:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 13f, 470f, 50f, 22f), "Length:", GUIStyle.op_Implicit("Label"));
          FengGameManagerMKII.settingsOld[72] = (object) GUI.TextField(new Rect(num1 + 58f, 470f, 40f, 20f), (string) FengGameManagerMKII.settingsOld[72]);
          GUI.Label(new Rect(num1 + 13f, 495f, 50f, 20f), "Width:", GUIStyle.op_Implicit("Label"));
          FengGameManagerMKII.settingsOld[70] = (object) GUI.TextField(new Rect(num1 + 58f, 495f, 40f, 20f), (string) FengGameManagerMKII.settingsOld[70]);
          GUI.Label(new Rect(num1 + 13f, 520f, 50f, 22f), "Height:", GUIStyle.op_Implicit("Label"));
          FengGameManagerMKII.settingsOld[71] = (object) GUI.TextField(new Rect(num1 + 58f, 520f, 40f, 20f), (string) FengGameManagerMKII.settingsOld[71]);
          if ((int) FengGameManagerMKII.settingsOld[64] <= 106)
          {
            GUI.Label(new Rect(num1 + 155f, 554f, 50f, 22f), "Tiling:", GUIStyle.op_Implicit("Label"));
            FengGameManagerMKII.settingsOld[79] = (object) GUI.TextField(new Rect(num1 + 200f, 554f, 40f, 20f), (string) FengGameManagerMKII.settingsOld[79]);
            FengGameManagerMKII.settingsOld[80] = (object) GUI.TextField(new Rect(num1 + 245f, 554f, 40f, 20f), (string) FengGameManagerMKII.settingsOld[80]);
            GUI.Label(new Rect(num1 + 219f, 570f, 10f, 22f), "x:", GUIStyle.op_Implicit("Label"));
            GUI.Label(new Rect(num1 + 264f, 570f, 10f, 22f), "y:", GUIStyle.op_Implicit("Label"));
            GUI.Label(new Rect(num1 + 155f, 445f, 50f, 20f), "Color:", GUIStyle.op_Implicit("Label"));
            GUI.Label(new Rect(num1 + 155f, 470f, 10f, 20f), "R:", GUIStyle.op_Implicit("Label"));
            GUI.Label(new Rect(num1 + 155f, 495f, 10f, 20f), "G:", GUIStyle.op_Implicit("Label"));
            GUI.Label(new Rect(num1 + 155f, 520f, 10f, 20f), "B:", GUIStyle.op_Implicit("Label"));
            FengGameManagerMKII.settingsOld[73] = (object) GUI.HorizontalSlider(new Rect(num1 + 170f, 475f, 100f, 20f), (float) FengGameManagerMKII.settingsOld[73], 0.0f, 1f);
            FengGameManagerMKII.settingsOld[74] = (object) GUI.HorizontalSlider(new Rect(num1 + 170f, 500f, 100f, 20f), (float) FengGameManagerMKII.settingsOld[74], 0.0f, 1f);
            FengGameManagerMKII.settingsOld[75] = (object) GUI.HorizontalSlider(new Rect(num1 + 170f, 525f, 100f, 20f), (float) FengGameManagerMKII.settingsOld[75], 0.0f, 1f);
            GUI.Label(new Rect(num1 + 13f, 554f, 57f, 22f), "Material:", GUIStyle.op_Implicit("Label"));
            if (GUI.Button(new Rect(num1 + 66f, 554f, 60f, 20f), (string) FengGameManagerMKII.settingsOld[69]))
              FengGameManagerMKII.settingsOld[78] = (object) 1;
            if ((int) FengGameManagerMKII.settingsOld[78] == 1)
            {
              string[] strArray3 = new string[4]
              {
                "bark",
                "bark2",
                "bark3",
                "bark4"
              };
              string[] strArray4 = new string[4]
              {
                "wood1",
                "wood2",
                "wood3",
                "wood4"
              };
              string[] strArray5 = new string[4]
              {
                "grass",
                "grass2",
                "grass3",
                "grass4"
              };
              string[] strArray6 = new string[4]
              {
                "brick1",
                "brick2",
                "brick3",
                "brick4"
              };
              string[] strArray7 = new string[4]
              {
                "metal1",
                "metal2",
                "metal3",
                "metal4"
              };
              string[] strArray8 = new string[3]
              {
                "rock1",
                "rock2",
                "rock3"
              };
              string[] strArray9 = new string[10]
              {
                "stone1",
                "stone2",
                "stone3",
                "stone4",
                "stone5",
                "stone6",
                "stone7",
                "stone8",
                "stone9",
                "stone10"
              };
              string[] strArray10 = new string[7]
              {
                "earth1",
                "earth2",
                "ice1",
                "lava1",
                "crystal1",
                "crystal2",
                "empty"
              };
              string[] strArray11 = new string[0];
              List<string[]> strArrayList = new List<string[]>()
              {
                strArray3,
                strArray4,
                strArray5,
                strArray6,
                strArray7,
                strArray8,
                strArray9,
                strArray10
              };
              string[] strArray12 = new string[9]
              {
                "bark",
                "wood",
                "grass",
                "brick",
                "metal",
                "rock",
                "stone",
                "misc",
                "transparent"
              };
              int index3 = 78;
              int index4 = 69;
              float num19 = (float) (Screen.width / 2) - 110.5f;
              float num20 = (float) (Screen.height / 2) - 220f;
              int index5 = (int) FengGameManagerMKII.settingsOld[185];
              float num21 = Math.Max((float) (10.0 + 104.0 * (double) (strArrayList[index5].Length / 3 + 1)), 280f);
              GUI.DrawTexture(new Rect(num19 + 2f, num20 + 2f, 208f, 446f), (Texture) this.textureBackgroundBlue);
              GUI.Box(new Rect(num19, num20, 212f, 450f), string.Empty);
              for (int index6 = 0; index6 < strArrayList.Count; ++index6)
              {
                int num22 = index6 / 3;
                int num23 = index6 % 3;
                if (GUI.Button(new Rect((float) ((double) num19 + 5.0 + 69.0 * (double) num23), num20 + 5f + (float) (30 * num22), 64f, 25f), strArray12[index6], GUIStyle.op_Implicit("box")))
                  FengGameManagerMKII.settingsOld[185] = (object) index6;
              }
              this.scroll2 = GUI.BeginScrollView(new Rect(num19, num20 + 110f, 225f, 290f), this.scroll2, new Rect(num19, num20 + 110f, 212f, num21), true, true);
              if (index5 != 8)
              {
                for (int index7 = 0; index7 < strArrayList[index5].Length; ++index7)
                {
                  int num24 = index7 / 3;
                  int num25 = index7 % 3;
                  GUI.DrawTexture(new Rect((float) ((double) num19 + 5.0 + 69.0 * (double) num25), (float) ((double) num20 + 115.0 + 104.0 * (double) num24), 64f, 64f), (Texture) this.RCLoadTexture("p" + strArrayList[index5][index7]));
                  if (GUI.Button(new Rect((float) ((double) num19 + 5.0 + 69.0 * (double) num25), (float) ((double) num20 + 184.0 + 104.0 * (double) num24), 64f, 30f), strArrayList[index5][index7]))
                  {
                    FengGameManagerMKII.settingsOld[index4] = (object) strArrayList[index5][index7];
                    FengGameManagerMKII.settingsOld[index3] = (object) 0;
                  }
                }
              }
              GUI.EndScrollView();
              if (GUI.Button(new Rect(num19 + 24f, num20 + 410f, 70f, 30f), "Default"))
              {
                FengGameManagerMKII.settingsOld[index4] = (object) "default";
                FengGameManagerMKII.settingsOld[index3] = (object) 0;
              }
              else if (GUI.Button(new Rect(num19 + 118f, num20 + 410f, 70f, 30f), "Done"))
                FengGameManagerMKII.settingsOld[index3] = (object) 0;
            }
            bool flag3 = false;
            if ((int) FengGameManagerMKII.settingsOld[76] == 1)
            {
              flag3 = true;
              Texture2D texture2D = new Texture2D(1, 1, (TextureFormat) 5, false);
              texture2D.SetPixel(0, 0, new Color((float) FengGameManagerMKII.settingsOld[73], (float) FengGameManagerMKII.settingsOld[74], (float) FengGameManagerMKII.settingsOld[75], 1f));
              texture2D.Apply();
              GUI.DrawTexture(new Rect(num1 + 235f, 445f, 30f, 20f), (Texture) texture2D, (ScaleMode) 0);
              Object.Destroy((Object) texture2D);
            }
            bool flag4 = GUI.Toggle(new Rect(num1 + 193f, 445f, 40f, 20f), flag3, "On");
            if (flag3 != flag4)
              FengGameManagerMKII.settingsOld[76] = !flag4 ? (object) 0 : (object) 1;
          }
        }
        if (GUI.Button(new Rect(num1 + 5f, 10f, 60f, 25f), "General", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 101;
        else if (GUI.Button(new Rect(num1 + 70f, 10f, 70f, 25f), "Geometry", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 102;
        else if (GUI.Button(new Rect(num1 + 145f, 10f, 65f, 25f), "Buildings", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 103;
        else if (GUI.Button(new Rect(num1 + 215f, 10f, 50f, 25f), "Nature", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 104;
        else if (GUI.Button(new Rect(num1 + 5f, 45f, 70f, 25f), "Spawners", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 105;
        else if (GUI.Button(new Rect(num1 + 80f, 45f, 70f, 25f), "Racing", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 108;
        else if (GUI.Button(new Rect(num1 + 155f, 45f, 40f, 25f), "Misc", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 107;
        else if (GUI.Button(new Rect(num1 + 200f, 45f, 70f, 25f), "Credits", GUIStyle.op_Implicit("box")))
          FengGameManagerMKII.settingsOld[64] = (object) 106;
        float result1;
        if ((int) FengGameManagerMKII.settingsOld[64] == 101)
        {
          this.scroll = GUI.BeginScrollView(new Rect(num1, 80f, 305f, 350f), this.scroll, new Rect(num1, 80f, 300f, 470f), true, true);
          GUI.Label(new Rect(num1 + 100f, 80f, 120f, 20f), "General Objects:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 108f, 245f, 120f, 20f), "Spawn Points:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 7f, 415f, 290f, 60f), "* The above titan spawn points apply only to randomly spawned titans specified by the Random Titan #.", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 7f, 470f, 290f, 60f), "* If team mode is disabled both cyan and magenta spawn points will be randomly chosen for players.", GUIStyle.op_Implicit("Label"));
          GUI.DrawTexture(new Rect(num1 + 27f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("psupply"));
          GUI.DrawTexture(new Rect(num1 + 118f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("pcannonwall"));
          GUI.DrawTexture(new Rect(num1 + 209f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("pcannonground"));
          GUI.DrawTexture(new Rect(num1 + 27f, 275f, 64f, 64f), (Texture) this.RCLoadTexture("pspawnt"));
          GUI.DrawTexture(new Rect(num1 + 118f, 275f, 64f, 64f), (Texture) this.RCLoadTexture("pspawnplayerC"));
          GUI.DrawTexture(new Rect(num1 + 209f, 275f, 64f, 64f), (Texture) this.RCLoadTexture("pspawnplayerM"));
          if (GUI.Button(new Rect(num1 + 27f, 179f, 64f, 60f), "Supply"))
          {
            flag1 = true;
            this.selectedObj = (GameObject) Object.Instantiate(Resources.Load("aot_supply"));
            ((Object) this.selectedObj).name = "base,aot_supply";
          }
          else if (GUI.Button(new Rect(num1 + 118f, 179f, 64f, 60f), "Cannon \nWall"))
          {
            flag1 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("CannonWallProp"));
            ((Object) this.selectedObj).name = "photon,CannonWall";
          }
          else if (GUI.Button(new Rect(num1 + 209f, 179f, 64f, 60f), "Cannon\n Ground"))
          {
            flag1 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("CannonGroundProp"));
            ((Object) this.selectedObj).name = "photon,CannonGround";
          }
          else if (GUI.Button(new Rect(num1 + 27f, 344f, 64f, 60f), "Titan"))
          {
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("titan"));
            ((Object) this.selectedObj).name = "spawnpoint,titan";
          }
          else if (GUI.Button(new Rect(num1 + 118f, 344f, 64f, 60f), "Player \nCyan"))
          {
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("playerC"));
            ((Object) this.selectedObj).name = "spawnpoint,playerC";
          }
          else if (GUI.Button(new Rect(num1 + 209f, 344f, 64f, 60f), "Player \nMagenta"))
          {
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("playerM"));
            ((Object) this.selectedObj).name = "spawnpoint,playerM";
          }
          GUI.EndScrollView();
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 107)
        {
          GUI.DrawTexture(new Rect(num1 + 30f, 90f, 64f, 64f), (Texture) this.RCLoadTexture("pbarrier"));
          GUI.DrawTexture(new Rect(num1 + 30f, 199f, 64f, 64f), (Texture) this.RCLoadTexture("pregion"));
          GUI.Label(new Rect(num1 + 110f, 243f, 200f, 22f), "Region Name:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 110f, 179f, 200f, 22f), "Disable Map Bounds:", GUIStyle.op_Implicit("Label"));
          bool flag5 = false;
          if ((int) FengGameManagerMKII.settingsOld[186] == 1)
          {
            flag5 = true;
            if (!((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).ContainsKey((object) "mapbounds"))
              ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Add((object) "mapbounds", (object) "map,disablebounds");
          }
          else if (((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).ContainsKey((object) "mapbounds"))
            ((Dictionary<object, object>) FengGameManagerMKII.linkHash[3]).Remove((object) "mapbounds");
          if (GUI.Button(new Rect(num1 + 30f, 159f, 64f, 30f), "Barrier"))
          {
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("barrierEditor"));
            ((Object) this.selectedObj).name = "misc,barrier";
          }
          else if (GUI.Button(new Rect(num1 + 30f, 268f, 64f, 30f), "Region"))
          {
            if ((string) FengGameManagerMKII.settingsOld[191] == string.Empty)
              FengGameManagerMKII.settingsOld[191] = (object) ("Region" + Random.Range(10000, 99999).ToString());
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("regionEditor"));
            GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            ((Object) gameObject).name = "RegionLabel";
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[71], out result1))
              FengGameManagerMKII.settingsOld[71] = (object) "1";
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[70], out result1))
              FengGameManagerMKII.settingsOld[70] = (object) "1";
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[72], out result1))
              FengGameManagerMKII.settingsOld[72] = (object) "1";
            gameObject.transform.parent = this.selectedObj.transform;
            float num26 = 1f;
            if ((double) Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]) > 100.0)
              num26 = 0.8f;
            else if ((double) Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]) > 1000.0)
              num26 = 0.5f;
            gameObject.transform.localPosition = new Vector3(0.0f, num26, 0.0f);
            gameObject.transform.localScale = new Vector3(5f / Convert.ToSingle((string) FengGameManagerMKII.settingsOld[70]), 5f / Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]), 5f / Convert.ToSingle((string) FengGameManagerMKII.settingsOld[72]));
            gameObject.GetComponent<UILabel>().text = (string) FengGameManagerMKII.settingsOld[191];
            this.selectedObj.AddComponent<RCRegionLabel>();
            this.selectedObj.GetComponent<RCRegionLabel>().myLabel = gameObject;
            ((Object) this.selectedObj).name = "misc,region," + (string) FengGameManagerMKII.settingsOld[191];
          }
          FengGameManagerMKII.settingsOld[191] = (object) GUI.TextField(new Rect(num1 + 200f, 243f, 75f, 20f), (string) FengGameManagerMKII.settingsOld[191]);
          bool flag6 = GUI.Toggle(new Rect(num1 + 240f, 179f, 40f, 20f), flag5, "On");
          if (flag6 != flag5)
            FengGameManagerMKII.settingsOld[186] = !flag6 ? (object) 0 : (object) 1;
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 105)
        {
          GUI.Label(new Rect(num1 + 95f, 85f, 130f, 20f), "Custom Spawners:", GUIStyle.op_Implicit("Label"));
          GUI.DrawTexture(new Rect(num1 + 7.8f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("ptitan"));
          GUI.DrawTexture(new Rect(num1 + 79.6f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("pabnormal"));
          GUI.DrawTexture(new Rect(num1 + 151.4f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("pjumper"));
          GUI.DrawTexture(new Rect(num1 + 223.2f, 110f, 64f, 64f), (Texture) this.RCLoadTexture("pcrawler"));
          GUI.DrawTexture(new Rect(num1 + 7.8f, 224f, 64f, 64f), (Texture) this.RCLoadTexture("ppunk"));
          GUI.DrawTexture(new Rect(num1 + 79.6f, 224f, 64f, 64f), (Texture) this.RCLoadTexture("pannie"));
          float result2;
          if (GUI.Button(new Rect(num1 + 7.8f, 179f, 64f, 30f), "Titan"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnTitan"));
            int num27 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnTitan," + (string) FengGameManagerMKII.settingsOld[83] + "," + num27.ToString();
          }
          else if (GUI.Button(new Rect(num1 + 79.6f, 179f, 64f, 30f), "Aberrant"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnAbnormal"));
            int num28 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnAbnormal," + (string) FengGameManagerMKII.settingsOld[83] + "," + num28.ToString();
          }
          else if (GUI.Button(new Rect(num1 + 151.4f, 179f, 64f, 30f), "Jumper"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnJumper"));
            int num29 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnJumper," + (string) FengGameManagerMKII.settingsOld[83] + "," + num29.ToString();
          }
          else if (GUI.Button(new Rect(num1 + 223.2f, 179f, 64f, 30f), "Crawler"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnCrawler"));
            int num30 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnCrawler," + (string) FengGameManagerMKII.settingsOld[83] + "," + num30.ToString();
          }
          else if (GUI.Button(new Rect(num1 + 7.8f, 293f, 64f, 30f), "Punk"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnPunk"));
            int num31 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnPunk," + (string) FengGameManagerMKII.settingsOld[83] + "," + num31.ToString();
          }
          else if (GUI.Button(new Rect(num1 + 79.6f, 293f, 64f, 30f), "Annie"))
          {
            if (!float.TryParse((string) FengGameManagerMKII.settingsOld[83], out result2))
              FengGameManagerMKII.settingsOld[83] = (object) "30";
            flag1 = true;
            flag2 = true;
            this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnAnnie"));
            int num32 = (int) FengGameManagerMKII.settingsOld[84];
            ((Object) this.selectedObj).name = "photon,spawnAnnie," + (string) FengGameManagerMKII.settingsOld[83] + "," + num32.ToString();
          }
          GUI.Label(new Rect(num1 + 7f, 379f, 140f, 22f), "Spawn Timer:", GUIStyle.op_Implicit("Label"));
          FengGameManagerMKII.settingsOld[83] = (object) GUI.TextField(new Rect(num1 + 100f, 379f, 50f, 20f), (string) FengGameManagerMKII.settingsOld[83]);
          GUI.Label(new Rect(num1 + 7f, 356f, 140f, 22f), "Endless spawn:", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 7f, 405f, 290f, 80f), "* The above settingsOld apply only to the next placed spawner. You can have unique spawn times and settingsOld for each individual titan spawner.", GUIStyle.op_Implicit("Label"));
          bool flag7 = false;
          if ((int) FengGameManagerMKII.settingsOld[84] == 1)
            flag7 = true;
          bool flag8 = GUI.Toggle(new Rect(num1 + 100f, 356f, 40f, 20f), flag7, "On");
          if (flag7 != flag8)
            FengGameManagerMKII.settingsOld[84] = !flag8 ? (object) 0 : (object) 1;
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 102)
        {
          string[] strArray = new string[12]
          {
            "cuboid",
            "plane",
            "sphere",
            "cylinder",
            "capsule",
            "pyramid",
            "cone",
            "prism",
            "arc90",
            "arc180",
            "torus",
            "tube"
          };
          for (int index = 0; index < strArray.Length; ++index)
          {
            int num33 = index % 4;
            int num34 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num33), (float) (90.0 + 114.0 * (double) num34), 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num33), (float) (159.0 + 114.0 * (double) num34), 64f, 30f), strArray[index]))
            {
              flag1 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[index]));
              ((Object) this.selectedObj).name = "custom," + strArray[index];
            }
          }
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 103)
        {
          List<string> stringList = new List<string>()
          {
            "arch1",
            "house1"
          };
          string[] strArray = new string[44]
          {
            "tower1",
            "tower2",
            "tower3",
            "tower4",
            "tower5",
            "house1",
            "house2",
            "house3",
            "house4",
            "house5",
            "house6",
            "house7",
            "house8",
            "house9",
            "house10",
            "house11",
            "house12",
            "house13",
            "house14",
            "pillar1",
            "pillar2",
            "village1",
            "village2",
            "windmill1",
            "arch1",
            "canal1",
            "castle1",
            "church1",
            "cannon1",
            "statue1",
            "statue2",
            "wagon1",
            "elevator1",
            "bridge1",
            "dummy1",
            "spike1",
            "wall1",
            "wall2",
            "wall3",
            "wall4",
            "arena1",
            "arena2",
            "arena3",
            "arena4"
          };
          float num35 = (float) (110.0 + 114.0 * (double) ((strArray.Length - 1) / 4));
          this.scroll = GUI.BeginScrollView(new Rect(num1, 90f, 303f, 350f), this.scroll, new Rect(num1, 90f, 300f, num35), true, true);
          for (int index = 0; index < strArray.Length; ++index)
          {
            int num36 = index % 4;
            int num37 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num36), (float) (90.0 + 114.0 * (double) num37), 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num36), (float) (159.0 + 114.0 * (double) num37), 64f, 30f), strArray[index]))
            {
              flag1 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[index]));
              if (stringList.Contains(strArray[index]))
                ((Object) this.selectedObj).name = "customb," + strArray[index];
              else
                ((Object) this.selectedObj).name = "custom," + strArray[index];
            }
          }
          GUI.EndScrollView();
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 104)
        {
          List<string> stringList = new List<string>()
          {
            "tree0"
          };
          string[] strArray = new string[23]
          {
            "leaf0",
            "leaf1",
            "leaf2",
            "field1",
            "field2",
            "tree0",
            "tree1",
            "tree2",
            "tree3",
            "tree4",
            "tree5",
            "tree6",
            "tree7",
            "log1",
            "log2",
            "trunk1",
            "boulder1",
            "boulder2",
            "boulder3",
            "boulder4",
            "boulder5",
            "cave1",
            "cave2"
          };
          float num38 = (float) (110.0 + 114.0 * (double) ((strArray.Length - 1) / 4));
          this.scroll = GUI.BeginScrollView(new Rect(num1, 90f, 303f, 350f), this.scroll, new Rect(num1, 90f, 300f, num38), true, true);
          for (int index = 0; index < strArray.Length; ++index)
          {
            int num39 = index % 4;
            int num40 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num39), (float) (90.0 + 114.0 * (double) num40), 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num39), (float) (159.0 + 114.0 * (double) num40), 64f, 30f), strArray[index]))
            {
              flag1 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray[index]));
              if (stringList.Contains(strArray[index]))
                ((Object) this.selectedObj).name = "customb," + strArray[index];
              else
                ((Object) this.selectedObj).name = "custom," + strArray[index];
            }
          }
          GUI.EndScrollView();
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 108)
        {
          string[] strArray13 = new string[12]
          {
            "Cuboid",
            "Plane",
            "Sphere",
            "Cylinder",
            "Capsule",
            "Pyramid",
            "Cone",
            "Prism",
            "Arc90",
            "Arc180",
            "Torus",
            "Tube"
          };
          string[] strArray14 = new string[12];
          for (int index = 0; index < strArray14.Length; ++index)
            strArray14[index] = "start" + strArray13[index];
          float num41 = (float) (110.0 + 114.0 * (double) ((strArray14.Length - 1) / 4)) * 4f + 200f;
          this.scroll = GUI.BeginScrollView(new Rect(num1, 90f, 303f, 350f), this.scroll, new Rect(num1, 90f, 300f, num41), true, true);
          GUI.Label(new Rect(num1 + 90f, 90f, 200f, 22f), "Racing Start Barrier");
          int num42 = 125;
          for (int index = 0; index < strArray14.Length; ++index)
          {
            int num43 = index % 4;
            int num44 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num43), (float) num42 + 114f * (float) num44, 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray14[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num43), (float) ((double) num42 + 69.0 + 114.0 * (double) num44), 64f, 30f), strArray13[index]))
            {
              flag1 = true;
              flag2 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray14[index]));
              ((Object) this.selectedObj).name = "racing," + strArray14[index];
            }
          }
          int num45 = num42 + (114 * (strArray14.Length / 4) + 10);
          GUI.Label(new Rect(num1 + 93f, (float) num45, 200f, 22f), "Racing End Trigger");
          int num46 = num45 + 35;
          for (int index = 0; index < strArray14.Length; ++index)
            strArray14[index] = "end" + strArray13[index];
          for (int index = 0; index < strArray14.Length; ++index)
          {
            int num47 = index % 4;
            int num48 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num47), (float) num46 + 114f * (float) num48, 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray14[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num47), (float) ((double) num46 + 69.0 + 114.0 * (double) num48), 64f, 30f), strArray13[index]))
            {
              flag1 = true;
              flag2 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray14[index]));
              ((Object) this.selectedObj).name = "racing," + strArray14[index];
            }
          }
          int num49 = num46 + (114 * (strArray14.Length / 4) + 10);
          GUI.Label(new Rect(num1 + 113f, (float) num49, 200f, 22f), "Kill Trigger");
          int num50 = num49 + 35;
          for (int index = 0; index < strArray14.Length; ++index)
            strArray14[index] = "kill" + strArray13[index];
          for (int index = 0; index < strArray14.Length; ++index)
          {
            int num51 = index % 4;
            int num52 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num51), (float) num50 + 114f * (float) num52, 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray14[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num51), (float) ((double) num50 + 69.0 + 114.0 * (double) num52), 64f, 30f), strArray13[index]))
            {
              flag1 = true;
              flag2 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray14[index]));
              ((Object) this.selectedObj).name = "racing," + strArray14[index];
            }
          }
          int num53 = num50 + (114 * (strArray14.Length / 4) + 10);
          GUI.Label(new Rect(num1 + 95f, (float) num53, 200f, 22f), "Checkpoint Trigger");
          int num54 = num53 + 35;
          for (int index = 0; index < strArray14.Length; ++index)
            strArray14[index] = "checkpoint" + strArray13[index];
          for (int index = 0; index < strArray14.Length; ++index)
          {
            int num55 = index % 4;
            int num56 = index / 4;
            GUI.DrawTexture(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num55), (float) num54 + 114f * (float) num56, 64f, 64f), (Texture) this.RCLoadTexture("p" + strArray14[index]));
            if (GUI.Button(new Rect((float) ((double) num1 + 7.8000001907348633 + 71.800003051757813 * (double) num55), (float) ((double) num54 + 69.0 + 114.0 * (double) num56), 64f, 30f), strArray13[index]))
            {
              flag1 = true;
              flag2 = true;
              this.selectedObj = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load(strArray14[index]));
              ((Object) this.selectedObj).name = "racing," + strArray14[index];
            }
          }
          GUI.EndScrollView();
        }
        else if ((int) FengGameManagerMKII.settingsOld[64] == 106)
        {
          GUI.Label(new Rect(num1 + 10f, 80f, 200f, 22f), "- Tree 2 designed by Ken P.", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 10f, 105f, 250f, 22f), "- Tower 2, House 5 designed by Matthew Santos", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 10f, 130f, 200f, 22f), "- Cannon retextured by Mika", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 10f, 155f, 200f, 22f), "- Arena 1,2,3 & 4 created by Gun", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 10f, 180f, 250f, 22f), "- Cannon Wall/Ground textured by Bellfox", GUIStyle.op_Implicit("Label"));
          GUI.Label(new Rect(num1 + 10f, 205f, 250f, 120f), "- House 7 - 14, Statue1, Statue2, Wagon1, Wall 1, Wall 2, Wall 3, Wall 4, CannonWall, CannonGround, Tower5, Bridge1, Dummy1, Spike1 created by meecube", GUIStyle.op_Implicit("Label"));
        }
        if (!flag1 || !Object.op_Inequality((Object) this.selectedObj, (Object) null))
          return;
        if (!float.TryParse((string) FengGameManagerMKII.settingsOld[70], out result1))
          FengGameManagerMKII.settingsOld[70] = (object) "1";
        if (!float.TryParse((string) FengGameManagerMKII.settingsOld[71], out result1))
          FengGameManagerMKII.settingsOld[71] = (object) "1";
        if (!float.TryParse((string) FengGameManagerMKII.settingsOld[72], out result1))
          FengGameManagerMKII.settingsOld[72] = (object) "1";
        if (!float.TryParse((string) FengGameManagerMKII.settingsOld[79], out result1))
          FengGameManagerMKII.settingsOld[79] = (object) "1";
        if (!float.TryParse((string) FengGameManagerMKII.settingsOld[80], out result1))
          FengGameManagerMKII.settingsOld[80] = (object) "1";
        if (!flag2)
        {
          float num57 = 1f;
          if ((string) FengGameManagerMKII.settingsOld[69] != "default")
          {
            if (((string) FengGameManagerMKII.settingsOld[69]).StartsWith("transparent"))
            {
              float result3;
              if (float.TryParse(((string) FengGameManagerMKII.settingsOld[69]).Substring(11), out result3))
                num57 = result3;
              foreach (Renderer componentsInChild in this.selectedObj.GetComponentsInChildren<Renderer>())
              {
                componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load("transparent");
                componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[79]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[80]));
              }
            }
            else
            {
              foreach (Renderer componentsInChild in this.selectedObj.GetComponentsInChildren<Renderer>())
              {
                if (!((Object) componentsInChild).name.Contains("Particle System") || !((Object) this.selectedObj).name.Contains("aot_supply"))
                {
                  componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load((string) FengGameManagerMKII.settingsOld[69]);
                  componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[79]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[80]));
                }
              }
            }
          }
          float num58 = 1f;
          foreach (MeshFilter componentsInChild in this.selectedObj.GetComponentsInChildren<MeshFilter>())
          {
            if (((Object) this.selectedObj).name.StartsWith("customb"))
            {
              double num59 = (double) num58;
              Bounds bounds1 = componentsInChild.mesh.bounds;
              double y = (double) ((Bounds) ref bounds1).size.y;
              if (num59 < y)
              {
                Bounds bounds2 = componentsInChild.mesh.bounds;
                num58 = ((Bounds) ref bounds2).size.y;
              }
            }
            else
            {
              double num60 = (double) num58;
              Bounds bounds3 = componentsInChild.mesh.bounds;
              double z = (double) ((Bounds) ref bounds3).size.z;
              if (num60 < z)
              {
                Bounds bounds4 = componentsInChild.mesh.bounds;
                num58 = ((Bounds) ref bounds4).size.z;
              }
            }
          }
          this.selectedObj.transform.localScale = new Vector3(this.selectedObj.transform.localScale.x * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[70]) - 1f / 1000f, this.selectedObj.transform.localScale.y * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]), this.selectedObj.transform.localScale.z * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[72]));
          if ((int) FengGameManagerMKII.settingsOld[76] == 1)
          {
            // ISSUE: explicit constructor call
            ((Color) ref color).\u002Ector((float) FengGameManagerMKII.settingsOld[73], (float) FengGameManagerMKII.settingsOld[74], (float) FengGameManagerMKII.settingsOld[75], num57);
            foreach (MeshFilter componentsInChild in this.selectedObj.GetComponentsInChildren<MeshFilter>())
            {
              Mesh mesh = componentsInChild.mesh;
              Color[] colorArray = new Color[mesh.vertexCount];
              for (int index = 0; index < mesh.vertexCount; ++index)
                colorArray[index] = color;
              mesh.colors = colorArray;
            }
          }
          float z1 = this.selectedObj.transform.localScale.z;
          if (((Object) this.selectedObj).name.Contains("boulder2") || ((Object) this.selectedObj).name.Contains("boulder3") || ((Object) this.selectedObj).name.Contains("field2"))
            z1 *= 0.01f;
          float num61 = (float) (10.0 + (double) z1 * (double) num58 * 1.2000000476837158 / 2.0);
          this.selectedObj.transform.position = new Vector3(((Component) Camera.main).transform.position.x + ((Component) Camera.main).transform.forward.x * num61, ((Component) Camera.main).transform.position.y + ((Component) Camera.main).transform.forward.y * 10f, ((Component) Camera.main).transform.position.z + ((Component) Camera.main).transform.forward.z * num61);
          Transform transform = this.selectedObj.transform;
          Quaternion rotation = ((Component) Camera.main).transform.rotation;
          Quaternion quaternion = Quaternion.Euler(0.0f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
          transform.rotation = quaternion;
          string[] strArray = new string[21]
          {
            ((Object) this.selectedObj).name,
            ",",
            (string) FengGameManagerMKII.settingsOld[69],
            ",",
            (string) FengGameManagerMKII.settingsOld[70],
            ",",
            (string) FengGameManagerMKII.settingsOld[71],
            ",",
            (string) FengGameManagerMKII.settingsOld[72],
            ",",
            FengGameManagerMKII.settingsOld[76].ToString(),
            ",",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
          };
          float num62 = (float) FengGameManagerMKII.settingsOld[73];
          strArray[12] = num62.ToString();
          strArray[13] = ",";
          num62 = (float) FengGameManagerMKII.settingsOld[74];
          strArray[14] = num62.ToString();
          strArray[15] = ",";
          strArray[16] = ((float) FengGameManagerMKII.settingsOld[75]).ToString();
          strArray[17] = ",";
          strArray[18] = (string) FengGameManagerMKII.settingsOld[79];
          strArray[19] = ",";
          strArray[20] = (string) FengGameManagerMKII.settingsOld[80];
          ((Object) this.selectedObj).name = string.Concat(strArray);
          this.unloadAssetsEditor();
        }
        else if (((Object) this.selectedObj).name.StartsWith("misc"))
        {
          if (((Object) this.selectedObj).name.Contains("barrier") || ((Object) this.selectedObj).name.Contains("region") || ((Object) this.selectedObj).name.Contains("racing"))
          {
            float num63 = 1f;
            this.selectedObj.transform.localScale = new Vector3(this.selectedObj.transform.localScale.x * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[70]) - 1f / 1000f, this.selectedObj.transform.localScale.y * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]), this.selectedObj.transform.localScale.z * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[72]));
            float num64 = (float) (10.0 + (double) this.selectedObj.transform.localScale.z * (double) num63 * 1.2000000476837158 / 2.0);
            this.selectedObj.transform.position = new Vector3(((Component) Camera.main).transform.position.x + ((Component) Camera.main).transform.forward.x * num64, ((Component) Camera.main).transform.position.y + ((Component) Camera.main).transform.forward.y * 10f, ((Component) Camera.main).transform.position.z + ((Component) Camera.main).transform.forward.z * num64);
            if (!((Object) this.selectedObj).name.Contains("region"))
            {
              Transform transform = this.selectedObj.transform;
              Quaternion rotation = ((Component) Camera.main).transform.rotation;
              Quaternion quaternion = Quaternion.Euler(0.0f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
              transform.rotation = quaternion;
            }
            ((Object) this.selectedObj).name = ((Object) this.selectedObj).name + "," + (string) FengGameManagerMKII.settingsOld[70] + "," + (string) FengGameManagerMKII.settingsOld[71] + "," + (string) FengGameManagerMKII.settingsOld[72];
          }
        }
        else if (((Object) this.selectedObj).name.StartsWith("racing"))
        {
          float num65 = 1f;
          this.selectedObj.transform.localScale = new Vector3(this.selectedObj.transform.localScale.x * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[70]) - 1f / 1000f, this.selectedObj.transform.localScale.y * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[71]), this.selectedObj.transform.localScale.z * Convert.ToSingle((string) FengGameManagerMKII.settingsOld[72]));
          float num66 = (float) (10.0 + (double) this.selectedObj.transform.localScale.z * (double) num65 * 1.2000000476837158 / 2.0);
          this.selectedObj.transform.position = new Vector3(((Component) Camera.main).transform.position.x + ((Component) Camera.main).transform.forward.x * num66, ((Component) Camera.main).transform.position.y + ((Component) Camera.main).transform.forward.y * 10f, ((Component) Camera.main).transform.position.z + ((Component) Camera.main).transform.forward.z * num66);
          Transform transform = this.selectedObj.transform;
          Quaternion rotation = ((Component) Camera.main).transform.rotation;
          Quaternion quaternion = Quaternion.Euler(0.0f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
          transform.rotation = quaternion;
          ((Object) this.selectedObj).name = ((Object) this.selectedObj).name + "," + (string) FengGameManagerMKII.settingsOld[70] + "," + (string) FengGameManagerMKII.settingsOld[71] + "," + (string) FengGameManagerMKII.settingsOld[72];
        }
        else
        {
          this.selectedObj.transform.position = new Vector3(((Component) Camera.main).transform.position.x + ((Component) Camera.main).transform.forward.x * 10f, ((Component) Camera.main).transform.position.y + ((Component) Camera.main).transform.forward.y * 10f, ((Component) Camera.main).transform.position.z + ((Component) Camera.main).transform.forward.z * 10f);
          Transform transform = this.selectedObj.transform;
          Quaternion rotation = ((Component) Camera.main).transform.rotation;
          Quaternion quaternion = Quaternion.Euler(0.0f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
          transform.rotation = quaternion;
        }
        Screen.lockCursor = true;
        GUI.FocusControl((string) null);
      }
      else
      {
        if (GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
          return;
        if ((double) Time.timeScale <= 0.10000000149011612)
        {
          float num67 = (float) Screen.width / 2f;
          float num68 = (float) Screen.height / 2f;
          GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
          GUI.DrawTexture(new Rect(num67 - 98f, num68 - 48f, 196f, 96f), (Texture) this.textureBackgroundBlue);
          GUI.Box(new Rect(num67 - 100f, num68 - 50f, 200f, 100f), string.Empty);
          if ((double) this.pauseWaitTime <= 3.0)
          {
            GUI.Label(new Rect(num67 - 43f, num68 - 15f, 200f, 22f), "Unpausing in:");
            GUI.Label(new Rect(num67 - 8f, num68 + 5f, 200f, 22f), this.pauseWaitTime.ToString("F1"));
          }
          else
            GUI.Label(new Rect(num67 - 43f, num68 - 10f, 200f, 22f), "Game Paused.");
        }
        else
        {
          if (FengGameManagerMKII.logicLoaded && FengGameManagerMKII.customLevelLoaded)
            return;
          float num69 = (float) Screen.width / 2f;
          float num70 = (float) Screen.height / 2f;
          GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
          GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), (Texture) this.textureBackgroundBlack);
          GUI.DrawTexture(new Rect(num69 - 98f, num70 - 48f, 196f, 146f), (Texture) this.textureBackgroundBlue);
          GUI.Box(new Rect(num69 - 100f, num70 - 50f, 200f, 150f), string.Empty);
          int length1 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.currentLevel]).Length;
          int length2 = RCextensions.returnStringFromObject(PhotonNetwork.masterClient.customProperties[(object) PhotonPlayerProperty.currentLevel]).Length;
          GUI.Label(new Rect(num69 - 60f, num70 - 30f, 200f, 22f), "Loading Level (" + length1.ToString() + "/" + length2.ToString() + ")");
          this.retryTime += Time.deltaTime;
          if (!GUI.Button(new Rect(num69 - 20f, num70 + 50f, 40f, 30f), "Quit"))
            return;
          PhotonNetwork.Disconnect();
          IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
          this.DestroyAllExistingCloths();
          Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
          Application.LoadLevel("menu");
        }
      }
    }
  }

  public void OnJoinedRoom()
  {
    this.maxPlayers = PhotonNetwork.room.maxPlayers;
    this.playerList = string.Empty;
    MonoBehaviour.print((object) ("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split("`"[0])[1]).mapName));
    this.gameTimesUp = false;
    string[] strArray = PhotonNetwork.room.name.Split("`"[0]);
    FengGameManagerMKII.level = strArray[1];
    if (strArray[2] == "normal")
      this.difficulty = 0;
    else if (strArray[2] == "hard")
      this.difficulty = 1;
    else if (strArray[2] == "abnormal")
      this.difficulty = 2;
    IN_GAME_MAIN_CAMERA.difficulty = this.difficulty;
    this.time = int.Parse(strArray[3]);
    this.time *= 60;
    IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
    PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
    this.name = SettingsManager.ProfileSettings.Name.Value;
    LoginFengKAI.player.name = this.name;
    LoginFengKAI.player.guildname = SettingsManager.ProfileSettings.Guild.Value;
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.name, (object) LoginFengKAI.player.name);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.guildName, (object) LoginFengKAI.player.guildname);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.kills, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.max_dmg, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.total_dmg, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.deaths, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.dead, (object) true);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCteam, (object) 0);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) string.Empty);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    this.humanScore = 0;
    this.titanScore = 0;
    this.PVPtitanScore = 0;
    this.PVPhumanScore = 0;
    this.wave = 1;
    this.highestwave = 1;
    this.localRacingResult = string.Empty;
    this.needChooseSide = true;
    this.chatContent = new ArrayList();
    this.killInfoGO = new ArrayList();
    InRoomChat.messages.Clear();
    if (!PhotonNetwork.isMasterClient)
      this.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
    this.assetCacheTextures = new Dictionary<string, Texture2D>();
    this.customMapMaterials = new Dictionary<string, Material>();
    this.isFirstLoad = true;
    if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != MultiplayerServerType.LAN)
      return;
    FengGameManagerMKII.ServerRequestAuthentication(FengGameManagerMKII.PrivateServerAuthPass);
  }

  public void OnLeftLobby() => MonoBehaviour.print((object) nameof (OnLeftLobby));

  public void OnLeftRoom()
  {
    PhotonPlayer.CleanProperties();
    InRoomChat.messages.Clear();
    if (Application.loadedLevel == 0)
      return;
    Time.timeScale = 1f;
    if (PhotonNetwork.connected)
      PhotonNetwork.Disconnect();
    this.resetSettings(true);
    this.loadconfig();
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
    this.gameStart = false;
    this.DestroyAllExistingCloths();
    FengGameManagerMKII.JustLeftRoom = true;
    Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
    Application.LoadLevel("menu");
  }

  private void OnLevelWasLoaded(int level)
  {
    SkyboxCustomSkinLoader.SkyboxMaterial = (Material) null;
    if (level != 0 && Application.loadedLevelName != "characterCreation" && Application.loadedLevelName != "SnapShot")
    {
      UIManager.SetMenu(MenuType.Game);
      foreach (GameObject go in GameObject.FindGameObjectsWithTag("titan"))
      {
        if (!Object.op_Inequality((Object) go.GetPhotonView(), (Object) null) || !go.GetPhotonView().owner.isMasterClient)
          Object.Destroy((Object) go);
      }
      this.isWinning = false;
      this.gameStart = true;
      this.ShowHUDInfoCenter(string.Empty);
      GameObject gameObject1 = (GameObject) Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
      Object.Destroy((Object) GameObject.Find("cameraDefaultPosition"));
      ((Object) gameObject1).name = "MainCamera";
      this.ui = (GameObject) Object.Instantiate(Resources.Load("UI_IN_GAME"));
      ((Object) this.ui).name = "UI_IN_GAME";
      this.ui.SetActive(true);
      NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], true);
      NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
      NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
      NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
      LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
      this.cache();
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
      this.loadskin();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        this.single_kills = 0;
        this.single_maxDamage = 0;
        this.single_totalDamage = 0;
        ((Behaviour) ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
        ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = true;
        ((Component) Camera.main).GetComponent<MouseLook>().disable = true;
        IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
        this.SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper());
        int abnormal = 90;
        if (this.difficulty == 1)
          abnormal = 70;
        this.spawnTitanCustom("titanRespawn", abnormal, info.enemyNumber, false);
      }
      else
      {
        PVPcheckPoint.chkPts = new ArrayList();
        ((Behaviour) ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = false;
        ((Behaviour) ((Component) Camera.main).GetComponent<CameraShake>()).enabled = false;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
        if (info.type == GAMEMODE.TROST)
        {
          GameObject.Find("playerRespawn").SetActive(false);
          Object.Destroy((Object) GameObject.Find("playerRespawn"));
          GameObject.Find("rock").animation["lift"].speed = 0.0f;
          GameObject.Find("door_fine").SetActive(false);
          GameObject.Find("door_broke").SetActive(true);
          Object.Destroy((Object) GameObject.Find("ppl"));
        }
        else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
        {
          GameObject.Find("playerRespawnTrost").SetActive(false);
          Object.Destroy((Object) GameObject.Find("playerRespawnTrost"));
        }
        if (this.needChooseSide)
          this.ShowHUDInfoTopCenterADD("\n\nPRESS " + SettingsManager.InputSettings.Human.Flare1.ToString() + " TO ENTER GAME");
        else if (!SettingsManager.LegacyGeneralSettings.SpecMode.Value)
        {
          Screen.lockCursor = IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS;
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
            this.checkpoint = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2 ? GameObject.Find("PVPchkPtH") : GameObject.Find("PVPchkPtT");
          if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            this.SpawnNonAITitan2(this.myLastHero);
          else
            this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
        }
        if (info.type == GAMEMODE.BOSS_FIGHT_CT)
          Object.Destroy((Object) GameObject.Find("rock"));
        if (PhotonNetwork.isMasterClient)
        {
          if (info.type == GAMEMODE.TROST)
          {
            if (!this.isPlayerAllDead2())
            {
              PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0.0f, -194f), Quaternion.Euler(0.0f, 180f, 0.0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
              int rate = 90;
              if (this.difficulty == 1)
                rate = 70;
              GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn");
              GameObject gameObject2 = GameObject.Find("titanRespawnTrost");
              if (Object.op_Inequality((Object) gameObject2, (Object) null))
              {
                foreach (GameObject gameObject3 in gameObjectsWithTag)
                {
                  if (Object.op_Equality((Object) ((Component) gameObject3.transform.parent).gameObject, (Object) gameObject2))
                    this.spawnTitan(rate, gameObject3.transform.position, gameObject3.transform.rotation);
                }
              }
            }
          }
          else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
          {
            if (!this.isPlayerAllDead2())
              PhotonNetwork.Instantiate("COLOSSAL_TITAN", Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.up), 10000f), Quaternion.Euler(0.0f, 180f, 0.0f), 0);
          }
          else if (info.type == GAMEMODE.KILL_TITAN || info.type == GAMEMODE.ENDLESS_TITAN || info.type == GAMEMODE.SURVIVE_MODE)
          {
            if (info.name == "Annie" || info.name == "Annie II")
            {
              PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
              int abnormal = 90;
              if (this.difficulty == 1)
                abnormal = 70;
              this.spawnTitanCustom("titanRespawn", abnormal, info.enemyNumber, false);
            }
          }
          else if (info.type != GAMEMODE.TROST && info.type == GAMEMODE.PVP_CAPTURE && LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "OutSide")
          {
            GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn");
            if (gameObjectsWithTag.Length == 0)
              return;
            for (int index = 0; index < gameObjectsWithTag.Length; ++index)
              this.spawnTitanRaw(gameObjectsWithTag[index].transform.position, gameObjectsWithTag[index].transform.rotation).GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
          }
        }
        if (!info.supply)
          Object.Destroy((Object) GameObject.Find("aot_supply"));
        if (!PhotonNetwork.isMasterClient)
          this.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
        if (LevelInfo.getInfo(FengGameManagerMKII.level).lavaMode)
        {
          Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0.0f, -29.5f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));
          GameObject.Find("aot_supply").transform.position = GameObject.Find("aot_supply_lava_position").transform.position;
          GameObject.Find("aot_supply").transform.rotation = GameObject.Find("aot_supply_lava_position").transform.rotation;
        }
        if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
          this.EnterSpecMode(true);
      }
    }
    this.unloadAssets(true);
  }

  public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
  {
    if (!FengGameManagerMKII.noRestart)
    {
      if (PhotonNetwork.isMasterClient)
      {
        this.restartingMC = true;
        if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
          this.restartingTitan = true;
        if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
          this.restartingBomb = true;
        if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
          this.restartingHorse = true;
        if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
          this.restartingEren = true;
      }
      this.resetSettings(false);
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan)
      {
        Hashtable propertiesToSet = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
      }
      if (!this.gameTimesUp && PhotonNetwork.isMasterClient)
      {
        this.restartGame2(true);
        this.photonView.RPC("setMasterRC", PhotonTargets.All);
      }
    }
    FengGameManagerMKII.noRestart = false;
  }

  public void OnPhotonCreateRoomFailed() => MonoBehaviour.print((object) nameof (OnPhotonCreateRoomFailed));

  public void OnPhotonCustomRoomPropertiesChanged()
  {
  }

  public void OnPhotonInstantiate() => MonoBehaviour.print((object) nameof (OnPhotonInstantiate));

  public void OnPhotonJoinRoomFailed() => MonoBehaviour.print((object) nameof (OnPhotonJoinRoomFailed));

  public void OnPhotonMaxCccuReached() => MonoBehaviour.print((object) nameof (OnPhotonMaxCccuReached));

  public void OnPhotonPlayerConnected(PhotonPlayer player)
  {
    if (PhotonNetwork.isMasterClient)
    {
      PhotonView photonView = this.photonView;
      if (((Dictionary<object, object>) FengGameManagerMKII.banHash).ContainsValue((object) RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name])))
      {
        this.kickPlayerRC(player, false, "banned.");
      }
      else
      {
        int num1 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statACL]);
        int num2 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statBLA]);
        int num3 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statGAS]);
        int num4 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statSPD]);
        if (num1 > 150 || num2 > 125 || num3 > 150 || num4 > 140)
        {
          this.kickPlayerRC(player, true, "excessive stats.");
          return;
        }
        if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
          this.StartCoroutine(this.WaitAndReloadKDR(player));
        if (FengGameManagerMKII.level.StartsWith("Custom"))
          this.StartCoroutine(this.customlevelE(new List<PhotonPlayer>()
          {
            player
          }));
        Hashtable hashtable = new Hashtable();
        if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "bomb", (object) 1);
        if (SettingsManager.LegacyGameSettings.BombModeCeiling.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "bombCeiling", (object) 1);
        else
          ((Dictionary<object, object>) hashtable).Add((object) "bombCeiling", (object) 0);
        if (SettingsManager.LegacyGameSettings.BombModeInfiniteGas.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "bombInfiniteGas", (object) 1);
        else
          ((Dictionary<object, object>) hashtable).Add((object) "bombInfiniteGas", (object) 0);
        if (SettingsManager.LegacyGameSettings.GlobalHideNames.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "globalHideNames", (object) 1);
        if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "globalDisableMinimap", (object) 1);
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
          ((Dictionary<object, object>) hashtable).Add((object) "team", (object) SettingsManager.LegacyGameSettings.TeamMode.Value);
        if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "point", (object) SettingsManager.LegacyGameSettings.PointModeAmount.Value);
        if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "rock", (object) 1);
        if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "explode", (object) SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value);
        if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
        {
          ((Dictionary<object, object>) hashtable).Add((object) "healthMode", (object) SettingsManager.LegacyGameSettings.TitanHealthMode.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "healthLower", (object) SettingsManager.LegacyGameSettings.TitanHealthMin.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "healthUpper", (object) SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
        }
        if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "infection", (object) SettingsManager.LegacyGameSettings.InfectionModeAmount.Value);
        if (SettingsManager.LegacyGameSettings.KickShifters.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "eren", (object) 1);
        if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "titanc", (object) SettingsManager.LegacyGameSettings.TitanNumber.Value);
        if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "damage", (object) SettingsManager.LegacyGameSettings.TitanArmor.Value);
        if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
        {
          ((Dictionary<object, object>) hashtable).Add((object) "sizeMode", (object) SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "sizeLower", (object) SettingsManager.LegacyGameSettings.TitanSizeMin.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "sizeUpper", (object) SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
        }
        if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
        {
          ((Dictionary<object, object>) hashtable).Add((object) "spawnMode", (object) 1);
          ((Dictionary<object, object>) hashtable).Add((object) "nRate", (object) SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "aRate", (object) SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "jRate", (object) SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "cRate", (object) SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value);
          ((Dictionary<object, object>) hashtable).Add((object) "pRate", (object) SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value);
        }
        if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
        {
          ((Dictionary<object, object>) hashtable).Add((object) "waveModeOn", (object) 1);
          ((Dictionary<object, object>) hashtable).Add((object) "waveModeNum", (object) SettingsManager.LegacyGameSettings.TitanPerWaves.Value);
        }
        if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "friendly", (object) 1);
        if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
          ((Dictionary<object, object>) hashtable).Add((object) "pvp", (object) SettingsManager.LegacyGameSettings.BladePVP.Value);
        if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "maxwave", (object) SettingsManager.LegacyGameSettings.TitanMaxWaves.Value);
        if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "endless", (object) SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value);
        if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
          ((Dictionary<object, object>) hashtable).Add((object) "motd", (object) SettingsManager.LegacyGameSettings.Motd.Value);
        if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "horse", (object) 1);
        if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "ahssReload", (object) 1);
        if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "punkWaves", (object) 1);
        if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "deadlycannons", (object) 1);
        if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
          ((Dictionary<object, object>) hashtable).Add((object) "asoracing", (object) 1);
        ((Dictionary<object, object>) hashtable).Add((object) "racingStartTime", (object) SettingsManager.LegacyGameSettings.RacingStartTime.Value);
        if (FengGameManagerMKII.ignoreList != null && FengGameManagerMKII.ignoreList.Count > 0)
          photonView.RPC("ignorePlayerArray", player, (object) FengGameManagerMKII.ignoreList.ToArray());
        photonView.RPC("settingRPC", player, (object) hashtable);
        photonView.RPC("setMasterRC", player);
        if ((double) Time.timeScale <= 0.10000000149011612 && (double) this.pauseWaitTime > 3.0)
        {
          photonView.RPC("pauseRPC", player, (object) true);
          object[] objArray = new object[2]
          {
            (object) "<color=#FFCC00>MasterClient has paused the game.</color>",
            (object) ""
          };
          photonView.RPC("Chat", player, objArray);
        }
      }
    }
    this.RecompilePlayerList(0.1f);
  }

  public void OnPhotonPlayerDisconnected(PhotonPlayer player)
  {
    if (!this.gameTimesUp)
    {
      this.oneTitanDown(string.Empty, true);
      this.someOneIsDead(0);
    }
    if (FengGameManagerMKII.ignoreList.Contains(player.ID))
      FengGameManagerMKII.ignoreList.Remove(player.ID);
    InstantiateTracker.instance.TryRemovePlayer(player.ID);
    if (PhotonNetwork.isMasterClient)
    {
      this.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, (object) player.ID);
      if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
      {
        string key = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
        if (this.PreservedPlayerKDR.ContainsKey(key))
          this.PreservedPlayerKDR.Remove(key);
        int[] numArray = new int[4]
        {
          RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]),
          RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]),
          RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]),
          RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg])
        };
        this.PreservedPlayerKDR.Add(key, numArray);
      }
    }
    this.RecompilePlayerList(0.1f);
  }

  public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
  {
    this.RecompilePlayerList(0.1f);
    if (playerAndUpdatedProps == null || playerAndUpdatedProps.Length < 2 || (PhotonPlayer) playerAndUpdatedProps[0] != PhotonNetwork.player)
      return;
    Hashtable playerAndUpdatedProp = (Hashtable) playerAndUpdatedProps[1];
    if (((Dictionary<object, object>) playerAndUpdatedProp).ContainsKey((object) "name") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]) != this.name)
    {
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.name, (object) this.name);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    if (!((Dictionary<object, object>) playerAndUpdatedProp).ContainsKey((object) "statACL") && !((Dictionary<object, object>) playerAndUpdatedProp).ContainsKey((object) "statBLA") && !((Dictionary<object, object>) playerAndUpdatedProp).ContainsKey((object) "statGAS") && !((Dictionary<object, object>) playerAndUpdatedProp).ContainsKey((object) "statSPD"))
      return;
    PhotonPlayer player = PhotonNetwork.player;
    int num1 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statACL]);
    int num2 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statBLA]);
    int num3 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statGAS]);
    int num4 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.statSPD]);
    if (num1 > 150)
    {
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statACL, (object) 100);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    if (num2 > 125)
    {
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statBLA, (object) 100);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    if (num3 > 150)
    {
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statGAS, (object) 100);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    if (num4 <= 140)
      return;
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.statSPD, (object) 100);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
  }

  public void OnPhotonRandomJoinFailed() => MonoBehaviour.print((object) nameof (OnPhotonRandomJoinFailed));

  public void OnPhotonSerializeView() => MonoBehaviour.print((object) nameof (OnPhotonSerializeView));

  public void OnReceivedRoomListUpdate()
  {
  }

  public void OnUpdate()
  {
    if (!((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) nameof (OnUpdate)))
      return;
    if ((double) this.updateTime > 0.0)
    {
      this.updateTime -= Time.deltaTime;
    }
    else
    {
      ((RCEvent) FengGameManagerMKII.RCEvents[(object) nameof (OnUpdate)]).checkEvent();
      this.updateTime = 1f;
    }
  }

  public void OnUpdatedFriendList() => MonoBehaviour.print((object) nameof (OnUpdatedFriendList));

  public int operantType(string str, int condition)
  {
    switch (condition)
    {
      case 0:
      case 3:
        if (str.StartsWith("Equals"))
          return 2;
        if (str.StartsWith("NotEquals"))
          return 5;
        if (!str.StartsWith("LessThan"))
        {
          if (str.StartsWith("LessThanOrEquals"))
            return 1;
          if (str.StartsWith("GreaterThanOrEquals"))
            return 3;
          if (str.StartsWith("GreaterThan"))
            return 4;
        }
        return 0;
      case 1:
      case 4:
      case 5:
        if (str.StartsWith("Equals"))
          return 2;
        return str.StartsWith("NotEquals") ? 5 : 0;
      case 2:
        if (str.StartsWith("Equals"))
          return 0;
        if (str.StartsWith("NotEquals"))
          return 1;
        if (str.StartsWith("Contains"))
          return 2;
        if (str.StartsWith("NotContains"))
          return 3;
        if (str.StartsWith("StartsWith"))
          return 4;
        if (str.StartsWith("NotStartsWith"))
          return 5;
        if (str.StartsWith("EndsWith"))
          return 6;
        return str.StartsWith("NotEndsWith") ? 7 : 0;
      default:
        return 0;
    }
  }

  public RCEvent parseBlock(
    string[] stringArray,
    int eventClass,
    int eventType,
    RCCondition condition)
  {
    List<RCAction> sentTrueActions = new List<RCAction>();
    RCEvent rcEvent = new RCEvent((RCCondition) null, (List<RCAction>) null, 0, 0);
    for (int index1 = 0; index1 < stringArray.Length; ++index1)
    {
      if (stringArray[index1].StartsWith("If") && stringArray[index1 + 1] == "{")
      {
        int num1 = index1 + 2;
        int num2 = index1 + 2;
        int num3 = 0;
        for (int index2 = index1 + 2; index2 < stringArray.Length; ++index2)
        {
          if (stringArray[index2] == "{")
            ++num3;
          if (stringArray[index2] == "}")
          {
            if (num3 > 0)
            {
              --num3;
            }
            else
            {
              num2 = index2 - 1;
              index2 = stringArray.Length;
            }
          }
        }
        string[] stringArray1 = new string[num2 - num1 + 1];
        int index3 = 0;
        for (int index4 = num1; index4 <= num2; ++index4)
        {
          stringArray1[index3] = stringArray[index4];
          ++index3;
        }
        int num4 = stringArray[index1].IndexOf("(");
        int num5 = stringArray[index1].LastIndexOf(")");
        string str1 = stringArray[index1].Substring(num4 + 1, num5 - num4 - 1);
        int num6 = this.conditionType(str1);
        int num7 = str1.IndexOf('.');
        string str2 = str1.Substring(num7 + 1);
        int sentOperand = this.operantType(str2, num6);
        int num8 = str2.IndexOf('(');
        int num9 = str2.LastIndexOf(")");
        string[] strArray = str2.Substring(num8 + 1, num9 - num8 - 1).Split(',');
        RCCondition condition1 = new RCCondition(sentOperand, num6, this.returnHelper(strArray[0]), this.returnHelper(strArray[1]));
        RCEvent block = this.parseBlock(stringArray1, 1, 0, condition1);
        RCAction rcAction = new RCAction(0, 0, block, (RCActionHelper[]) null);
        rcEvent = block;
        sentTrueActions.Add(rcAction);
        index1 = num2;
      }
      else if (stringArray[index1].StartsWith("While") && stringArray[index1 + 1] == "{")
      {
        int num10 = index1 + 2;
        int num11 = index1 + 2;
        int num12 = 0;
        for (int index5 = index1 + 2; index5 < stringArray.Length; ++index5)
        {
          if (stringArray[index5] == "{")
            ++num12;
          if (stringArray[index5] == "}")
          {
            if (num12 > 0)
            {
              --num12;
            }
            else
            {
              num11 = index5 - 1;
              index5 = stringArray.Length;
            }
          }
        }
        string[] stringArray2 = new string[num11 - num10 + 1];
        int index6 = 0;
        for (int index7 = num10; index7 <= num11; ++index7)
        {
          stringArray2[index6] = stringArray[index7];
          ++index6;
        }
        int num13 = stringArray[index1].IndexOf("(");
        int num14 = stringArray[index1].LastIndexOf(")");
        string str3 = stringArray[index1].Substring(num13 + 1, num14 - num13 - 1);
        int num15 = this.conditionType(str3);
        int num16 = str3.IndexOf('.');
        string str4 = str3.Substring(num16 + 1);
        int sentOperand = this.operantType(str4, num15);
        int num17 = str4.IndexOf('(');
        int num18 = str4.LastIndexOf(")");
        string[] strArray = str4.Substring(num17 + 1, num18 - num17 - 1).Split(',');
        RCCondition condition2 = new RCCondition(sentOperand, num15, this.returnHelper(strArray[0]), this.returnHelper(strArray[1]));
        RCAction rcAction = new RCAction(0, 0, this.parseBlock(stringArray2, 3, 0, condition2), (RCActionHelper[]) null);
        sentTrueActions.Add(rcAction);
        index1 = num11;
      }
      else if (stringArray[index1].StartsWith("ForeachTitan") && stringArray[index1 + 1] == "{")
      {
        int num19 = index1 + 2;
        int num20 = index1 + 2;
        int num21 = 0;
        for (int index8 = index1 + 2; index8 < stringArray.Length; ++index8)
        {
          if (stringArray[index8] == "{")
            ++num21;
          if (stringArray[index8] == "}")
          {
            if (num21 > 0)
            {
              --num21;
            }
            else
            {
              num20 = index8 - 1;
              index8 = stringArray.Length;
            }
          }
        }
        string[] stringArray3 = new string[num20 - num19 + 1];
        int index9 = 0;
        for (int index10 = num19; index10 <= num20; ++index10)
        {
          stringArray3[index9] = stringArray[index10];
          ++index9;
        }
        int num22 = stringArray[index1].IndexOf("(");
        int num23 = stringArray[index1].LastIndexOf(")");
        string str = stringArray[index1].Substring(num22 + 2, num23 - num22 - 3);
        int eventType1 = 0;
        RCEvent block = this.parseBlock(stringArray3, 2, eventType1, (RCCondition) null);
        block.foreachVariableName = str;
        RCAction rcAction = new RCAction(0, 0, block, (RCActionHelper[]) null);
        sentTrueActions.Add(rcAction);
        index1 = num20;
      }
      else if (stringArray[index1].StartsWith("ForeachPlayer") && stringArray[index1 + 1] == "{")
      {
        int num24 = index1 + 2;
        int num25 = index1 + 2;
        int num26 = 0;
        for (int index11 = index1 + 2; index11 < stringArray.Length; ++index11)
        {
          if (stringArray[index11] == "{")
            ++num26;
          if (stringArray[index11] == "}")
          {
            if (num26 > 0)
            {
              --num26;
            }
            else
            {
              num25 = index11 - 1;
              index11 = stringArray.Length;
            }
          }
        }
        string[] stringArray4 = new string[num25 - num24 + 1];
        int index12 = 0;
        for (int index13 = num24; index13 <= num25; ++index13)
        {
          stringArray4[index12] = stringArray[index13];
          ++index12;
        }
        int num27 = stringArray[index1].IndexOf("(");
        int num28 = stringArray[index1].LastIndexOf(")");
        string str = stringArray[index1].Substring(num27 + 2, num28 - num27 - 3);
        int eventType2 = 1;
        RCEvent block = this.parseBlock(stringArray4, 2, eventType2, (RCCondition) null);
        block.foreachVariableName = str;
        RCAction rcAction = new RCAction(0, 0, block, (RCActionHelper[]) null);
        sentTrueActions.Add(rcAction);
        index1 = num25;
      }
      else if (stringArray[index1].StartsWith("Else") && stringArray[index1 + 1] == "{")
      {
        int num29 = index1 + 2;
        int num30 = index1 + 2;
        int num31 = 0;
        for (int index14 = index1 + 2; index14 < stringArray.Length; ++index14)
        {
          if (stringArray[index14] == "{")
            ++num31;
          if (stringArray[index14] == "}")
          {
            if (num31 > 0)
            {
              --num31;
            }
            else
            {
              num30 = index14 - 1;
              index14 = stringArray.Length;
            }
          }
        }
        string[] stringArray5 = new string[num30 - num29 + 1];
        int index15 = 0;
        for (int index16 = num29; index16 <= num30; ++index16)
        {
          stringArray5[index15] = stringArray[index16];
          ++index15;
        }
        if (stringArray[index1] == "Else")
        {
          RCAction sentElse = new RCAction(0, 0, this.parseBlock(stringArray5, 0, 0, (RCCondition) null), (RCActionHelper[]) null);
          rcEvent.setElse(sentElse);
          index1 = num30;
        }
        else if (stringArray[index1].StartsWith("Else If"))
        {
          int num32 = stringArray[index1].IndexOf("(");
          int num33 = stringArray[index1].LastIndexOf(")");
          string str5 = stringArray[index1].Substring(num32 + 1, num33 - num32 - 1);
          int num34 = this.conditionType(str5);
          int num35 = str5.IndexOf('.');
          string str6 = str5.Substring(num35 + 1);
          int sentOperand = this.operantType(str6, num34);
          int num36 = str6.IndexOf('(');
          int num37 = str6.LastIndexOf(")");
          string[] strArray = str6.Substring(num36 + 1, num37 - num36 - 1).Split(',');
          RCCondition condition3 = new RCCondition(sentOperand, num34, this.returnHelper(strArray[0]), this.returnHelper(strArray[1]));
          RCAction sentElse = new RCAction(0, 0, this.parseBlock(stringArray5, 1, 0, condition3), (RCActionHelper[]) null);
          rcEvent.setElse(sentElse);
          index1 = num30;
        }
      }
      else if (stringArray[index1].StartsWith("VariableInt"))
      {
        int category = 1;
        int num38 = stringArray[index1].IndexOf('.');
        int num39 = stringArray[index1].IndexOf('(');
        int num40 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num38 + 1, num39 - num38 - 1);
        string[] strArray = stringArray[index1].Substring(num39 + 1, num40 - num39 - 1).Split(',');
        if (str.StartsWith("SetRandom"))
        {
          RCActionHelper rcActionHelper1 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper2 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper3 = this.returnHelper(strArray[2]);
          RCAction rcAction = new RCAction(category, 12, (RCEvent) null, new RCActionHelper[3]
          {
            rcActionHelper1,
            rcActionHelper2,
            rcActionHelper3
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper4 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper5 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper4,
            rcActionHelper5
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Add"))
        {
          RCActionHelper rcActionHelper6 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper7 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 1, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper6,
            rcActionHelper7
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Subtract"))
        {
          RCActionHelper rcActionHelper8 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper9 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 2, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper8,
            rcActionHelper9
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Multiply"))
        {
          RCActionHelper rcActionHelper10 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper11 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 3, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper10,
            rcActionHelper11
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Divide"))
        {
          RCActionHelper rcActionHelper12 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper13 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 4, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper12,
            rcActionHelper13
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Modulo"))
        {
          RCActionHelper rcActionHelper14 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper15 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 5, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper14,
            rcActionHelper15
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Power"))
        {
          RCActionHelper rcActionHelper16 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper17 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 6, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper16,
            rcActionHelper17
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("VariableBool"))
      {
        int category = 2;
        int num41 = stringArray[index1].IndexOf('.');
        int num42 = stringArray[index1].IndexOf('(');
        int num43 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num41 + 1, num42 - num41 - 1);
        string[] strArray = stringArray[index1].Substring(num42 + 1, num43 - num42 - 1).Split(',');
        if (str.StartsWith("SetToOpposite"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 11, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetRandom"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 12, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper18 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper19 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper18,
            rcActionHelper19
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("VariableString"))
      {
        int category = 3;
        int num44 = stringArray[index1].IndexOf('.');
        int num45 = stringArray[index1].IndexOf('(');
        int num46 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num44 + 1, num45 - num44 - 1);
        string[] strArray = stringArray[index1].Substring(num45 + 1, num46 - num45 - 1).Split(',');
        if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper20 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper21 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper20,
            rcActionHelper21
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Concat"))
        {
          RCActionHelper[] helpers = new RCActionHelper[strArray.Length];
          for (int index17 = 0; index17 < strArray.Length; ++index17)
            helpers[index17] = this.returnHelper(strArray[index17]);
          RCAction rcAction = new RCAction(category, 7, (RCEvent) null, helpers);
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Append"))
        {
          RCActionHelper rcActionHelper22 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper23 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 8, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper22,
            rcActionHelper23
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Replace"))
        {
          RCActionHelper rcActionHelper24 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper25 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper26 = this.returnHelper(strArray[2]);
          RCAction rcAction = new RCAction(category, 10, (RCEvent) null, new RCActionHelper[3]
          {
            rcActionHelper24,
            rcActionHelper25,
            rcActionHelper26
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Remove"))
        {
          RCActionHelper rcActionHelper27 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper28 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 9, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper27,
            rcActionHelper28
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("VariableFloat"))
      {
        int category = 4;
        int num47 = stringArray[index1].IndexOf('.');
        int num48 = stringArray[index1].IndexOf('(');
        int num49 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num47 + 1, num48 - num47 - 1);
        string[] strArray = stringArray[index1].Substring(num48 + 1, num49 - num48 - 1).Split(',');
        if (str.StartsWith("SetRandom"))
        {
          RCActionHelper rcActionHelper29 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper30 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper31 = this.returnHelper(strArray[2]);
          RCAction rcAction = new RCAction(category, 12, (RCEvent) null, new RCActionHelper[3]
          {
            rcActionHelper29,
            rcActionHelper30,
            rcActionHelper31
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper32 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper33 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper32,
            rcActionHelper33
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Add"))
        {
          RCActionHelper rcActionHelper34 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper35 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 1, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper34,
            rcActionHelper35
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Subtract"))
        {
          RCActionHelper rcActionHelper36 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper37 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 2, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper36,
            rcActionHelper37
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Multiply"))
        {
          RCActionHelper rcActionHelper38 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper39 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 3, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper38,
            rcActionHelper39
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Divide"))
        {
          RCActionHelper rcActionHelper40 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper41 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 4, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper40,
            rcActionHelper41
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Modulo"))
        {
          RCActionHelper rcActionHelper42 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper43 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 5, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper42,
            rcActionHelper43
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Power"))
        {
          RCActionHelper rcActionHelper44 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper45 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 6, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper44,
            rcActionHelper45
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("VariablePlayer"))
      {
        int category = 5;
        int num50 = stringArray[index1].IndexOf('.');
        int num51 = stringArray[index1].IndexOf('(');
        int num52 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num50 + 1, num51 - num50 - 1);
        string[] strArray = stringArray[index1].Substring(num51 + 1, num52 - num51 - 1).Split(',');
        if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper46 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper47 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper46,
            rcActionHelper47
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("VariableTitan"))
      {
        int category = 6;
        int num53 = stringArray[index1].IndexOf('.');
        int num54 = stringArray[index1].IndexOf('(');
        int num55 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num53 + 1, num54 - num53 - 1);
        string[] strArray = stringArray[index1].Substring(num54 + 1, num55 - num54 - 1).Split(',');
        if (str.StartsWith("Set"))
        {
          RCActionHelper rcActionHelper48 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper49 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper48,
            rcActionHelper49
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("Player"))
      {
        int category = 7;
        int num56 = stringArray[index1].IndexOf('.');
        int num57 = stringArray[index1].IndexOf('(');
        int num58 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num56 + 1, num57 - num56 - 1);
        string[] strArray = stringArray[index1].Substring(num57 + 1, num58 - num57 - 1).Split(',');
        if (str.StartsWith("KillPlayer"))
        {
          RCActionHelper rcActionHelper50 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper51 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper50,
            rcActionHelper51
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SpawnPlayerAt"))
        {
          RCActionHelper rcActionHelper52 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper53 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper54 = this.returnHelper(strArray[2]);
          RCActionHelper rcActionHelper55 = this.returnHelper(strArray[3]);
          RCAction rcAction = new RCAction(category, 2, (RCEvent) null, new RCActionHelper[4]
          {
            rcActionHelper52,
            rcActionHelper53,
            rcActionHelper54,
            rcActionHelper55
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SpawnPlayer"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 1, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("MovePlayer"))
        {
          RCActionHelper rcActionHelper56 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper57 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper58 = this.returnHelper(strArray[2]);
          RCActionHelper rcActionHelper59 = this.returnHelper(strArray[3]);
          RCAction rcAction = new RCAction(category, 3, (RCEvent) null, new RCActionHelper[4]
          {
            rcActionHelper56,
            rcActionHelper57,
            rcActionHelper58,
            rcActionHelper59
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetKills"))
        {
          RCActionHelper rcActionHelper60 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper61 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 4, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper60,
            rcActionHelper61
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetDeaths"))
        {
          RCActionHelper rcActionHelper62 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper63 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 5, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper62,
            rcActionHelper63
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetMaxDmg"))
        {
          RCActionHelper rcActionHelper64 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper65 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 6, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper64,
            rcActionHelper65
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetTotalDmg"))
        {
          RCActionHelper rcActionHelper66 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper67 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 7, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper66,
            rcActionHelper67
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetName"))
        {
          RCActionHelper rcActionHelper68 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper69 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 8, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper68,
            rcActionHelper69
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetGuildName"))
        {
          RCActionHelper rcActionHelper70 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper71 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 9, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper70,
            rcActionHelper71
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetTeam"))
        {
          RCActionHelper rcActionHelper72 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper73 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 10, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper72,
            rcActionHelper73
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetCustomInt"))
        {
          RCActionHelper rcActionHelper74 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper75 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 11, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper74,
            rcActionHelper75
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetCustomBool"))
        {
          RCActionHelper rcActionHelper76 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper77 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 12, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper76,
            rcActionHelper77
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetCustomString"))
        {
          RCActionHelper rcActionHelper78 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper79 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 13, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper78,
            rcActionHelper79
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetCustomFloat"))
        {
          RCActionHelper rcActionHelper80 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper81 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 14, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper80,
            rcActionHelper81
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("Titan"))
      {
        int category = 8;
        int num59 = stringArray[index1].IndexOf('.');
        int num60 = stringArray[index1].IndexOf('(');
        int num61 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num59 + 1, num60 - num59 - 1);
        string[] strArray = stringArray[index1].Substring(num60 + 1, num61 - num60 - 1).Split(',');
        if (str.StartsWith("KillTitan"))
        {
          RCActionHelper rcActionHelper82 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper83 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper84 = this.returnHelper(strArray[2]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[3]
          {
            rcActionHelper82,
            rcActionHelper83,
            rcActionHelper84
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SpawnTitanAt"))
        {
          RCActionHelper rcActionHelper85 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper86 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper87 = this.returnHelper(strArray[2]);
          RCActionHelper rcActionHelper88 = this.returnHelper(strArray[3]);
          RCActionHelper rcActionHelper89 = this.returnHelper(strArray[4]);
          RCActionHelper rcActionHelper90 = this.returnHelper(strArray[5]);
          RCActionHelper rcActionHelper91 = this.returnHelper(strArray[6]);
          RCAction rcAction = new RCAction(category, 2, (RCEvent) null, new RCActionHelper[7]
          {
            rcActionHelper85,
            rcActionHelper86,
            rcActionHelper87,
            rcActionHelper88,
            rcActionHelper89,
            rcActionHelper90,
            rcActionHelper91
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SpawnTitan"))
        {
          RCActionHelper rcActionHelper92 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper93 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper94 = this.returnHelper(strArray[2]);
          RCActionHelper rcActionHelper95 = this.returnHelper(strArray[3]);
          RCAction rcAction = new RCAction(category, 1, (RCEvent) null, new RCActionHelper[4]
          {
            rcActionHelper92,
            rcActionHelper93,
            rcActionHelper94,
            rcActionHelper95
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("SetHealth"))
        {
          RCActionHelper rcActionHelper96 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper97 = this.returnHelper(strArray[1]);
          RCAction rcAction = new RCAction(category, 3, (RCEvent) null, new RCActionHelper[2]
          {
            rcActionHelper96,
            rcActionHelper97
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("MoveTitan"))
        {
          RCActionHelper rcActionHelper98 = this.returnHelper(strArray[0]);
          RCActionHelper rcActionHelper99 = this.returnHelper(strArray[1]);
          RCActionHelper rcActionHelper100 = this.returnHelper(strArray[2]);
          RCActionHelper rcActionHelper101 = this.returnHelper(strArray[3]);
          RCAction rcAction = new RCAction(category, 4, (RCEvent) null, new RCActionHelper[4]
          {
            rcActionHelper98,
            rcActionHelper99,
            rcActionHelper100,
            rcActionHelper101
          });
          sentTrueActions.Add(rcAction);
        }
      }
      else if (stringArray[index1].StartsWith("Game"))
      {
        int category = 9;
        int num62 = stringArray[index1].IndexOf('.');
        int num63 = stringArray[index1].IndexOf('(');
        int num64 = stringArray[index1].LastIndexOf(')');
        string str = stringArray[index1].Substring(num62 + 1, num63 - num62 - 1);
        string[] strArray = stringArray[index1].Substring(num63 + 1, num64 - num63 - 1).Split(',');
        if (str.StartsWith("PrintMessage"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 0, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("LoseGame"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 2, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("WinGame"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 1, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
        else if (str.StartsWith("Restart"))
        {
          RCActionHelper rcActionHelper = this.returnHelper(strArray[0]);
          RCAction rcAction = new RCAction(category, 3, (RCEvent) null, new RCActionHelper[1]
          {
            rcActionHelper
          });
          sentTrueActions.Add(rcAction);
        }
      }
    }
    return new RCEvent(condition, sentTrueActions, eventClass, eventType);
  }

  [RPC]
  public void pauseRPC(bool pause, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    if (pause)
    {
      this.pauseWaitTime = 100000f;
      Time.timeScale = 1E-06f;
    }
    else
      this.pauseWaitTime = 3f;
  }

  public void playerKillInfoSingleUpdate(int dmg)
  {
    ++this.single_kills;
    this.single_maxDamage = Mathf.Max(dmg, this.single_maxDamage);
    this.single_totalDamage += dmg;
  }

  public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
  {
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.kills, (object) ((int) player.customProperties[(object) PhotonPlayerProperty.kills] + 1));
    player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.max_dmg, (object) Mathf.Max(dmg, (int) player.customProperties[(object) PhotonPlayerProperty.max_dmg]));
    player.SetCustomProperties(propertiesToSet2);
    Hashtable propertiesToSet3 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.total_dmg, (object) ((int) player.customProperties[(object) PhotonPlayerProperty.total_dmg] + dmg));
    player.SetCustomProperties(propertiesToSet3);
  }

  public GameObject randomSpawnOneTitan(string place, int rate)
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(place);
    int index = Random.Range(0, gameObjectsWithTag.Length);
    GameObject gameObject = gameObjectsWithTag[index];
    return this.spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation);
  }

  public void randomSpawnTitan(string place, int rate, int num, bool punk = false)
  {
    if (num == -1)
      num = 1;
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(place);
    List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
    if (gameObjectsWithTag.Length == 0)
      return;
    for (int index1 = 0; index1 < num; ++index1)
    {
      if (gameObjectList.Count <= 0)
        gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
      int index2 = Random.Range(0, gameObjectList.Count);
      GameObject gameObject = gameObjectList[index2];
      gameObjectList.RemoveAt(index2);
      this.spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk);
    }
  }

  public Texture2D RCLoadTexture(string tex)
  {
    if (this.assetCacheTextures == null)
      this.assetCacheTextures = new Dictionary<string, Texture2D>();
    if (this.assetCacheTextures.ContainsKey(tex))
      return this.assetCacheTextures[tex];
    Texture2D texture2D = (Texture2D) FengGameManagerMKII.RCassets.Load(tex);
    this.assetCacheTextures.Add(tex, texture2D);
    return texture2D;
  }

  public void RecompilePlayerList(float time)
  {
    if (this.isRecompiling)
      return;
    this.isRecompiling = true;
    this.StartCoroutine(this.WaitAndRecompilePlayerList(time));
  }

  [RPC]
  private void refreshPVPStatus(int score1, int score2)
  {
    this.PVPhumanScore = score1;
    this.PVPtitanScore = score2;
  }

  [RPC]
  private void refreshPVPStatus_AHSS(int[] score1) => this.teamScores = score1;

  private void refreshRacingResult()
  {
    this.localRacingResult = "Result\n";
    this.racingResult.Sort((IComparer) new IComparerRacingResult());
    int num = Mathf.Min(this.racingResult.Count, 6);
    for (int index = 0; index < num; ++index)
    {
      this.localRacingResult = this.localRacingResult + "Rank " + (object) (index + 1) + " : ";
      this.localRacingResult += (this.racingResult[index] as RacingResult).name;
      this.localRacingResult = this.localRacingResult + "   " + ((float) (int) ((double) (this.racingResult[index] as RacingResult).time * 100.0) * 0.01f).ToString() + "s";
      this.localRacingResult += "\n";
    }
    this.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, (object) this.localRacingResult);
  }

  private void refreshRacingResult2()
  {
    this.localRacingResult = "Result\n";
    this.racingResult.Sort((IComparer) new IComparerRacingResult());
    int num = Mathf.Min(this.racingResult.Count, 10);
    for (int index = 0; index < num; ++index)
    {
      this.localRacingResult = this.localRacingResult + "Rank " + (object) (index + 1) + " : ";
      this.localRacingResult += (this.racingResult[index] as RacingResult).name;
      this.localRacingResult = this.localRacingResult + "   " + ((float) (int) ((double) (this.racingResult[index] as RacingResult).time * 100.0) * 0.01f).ToString() + "s";
      this.localRacingResult += "\n";
    }
    this.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, (object) this.localRacingResult);
  }

  [RPC]
  private void refreshStatus(
    int score1,
    int score2,
    int wav,
    int highestWav,
    float time1,
    float time2,
    bool startRacin,
    bool endRacin,
    PhotonMessageInfo info)
  {
    if (info.sender != PhotonNetwork.masterClient || PhotonNetwork.isMasterClient)
      return;
    this.humanScore = score1;
    this.titanScore = score2;
    this.wave = wav;
    this.highestwave = highestWav;
    this.roundTime = time1;
    this.timeTotalServer = time2;
    this.startRacing = startRacin;
    this.endRacing = endRacin;
    if (!this.startRacing || !Object.op_Inequality((Object) GameObject.Find("door"), (Object) null))
      return;
    GameObject.Find("door").SetActive(false);
  }

  public IEnumerator reloadSky(bool specmode = false)
  {
    yield return (object) new WaitForSeconds(0.5f);
    Material skyboxMaterial = SkyboxCustomSkinLoader.SkyboxMaterial;
    if (Object.op_Inequality((Object) skyboxMaterial, (Object) null) && Object.op_Inequality((Object) ((Component) Camera.main).GetComponent<Skybox>().material, (Object) skyboxMaterial))
      ((Component) Camera.main).GetComponent<Skybox>().material = skyboxMaterial;
  }

  public void removeCT(COLOSSAL_TITAN titan) => this.cT.Remove((object) titan);

  public void removeET(TITAN_EREN hero) => this.eT.Remove((object) hero);

  public void removeFT(FEMALE_TITAN titan) => this.fT.Remove((object) titan);

  public void removeHero(HERO hero) => this.heroes.Remove((object) hero);

  public void removeHook(Bullet h) => this.hooks.Remove((object) h);

  public void removeTitan(TITAN titan) => this.titans.Remove((object) titan);

  [RPC]
  private void RequireStatus()
  {
    this.photonView.RPC("refreshStatus", PhotonTargets.Others, (object) this.humanScore, (object) this.titanScore, (object) this.wave, (object) this.highestwave, (object) this.roundTime, (object) this.timeTotalServer, (object) this.startRacing, (object) this.endRacing);
    this.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, (object) this.PVPhumanScore, (object) this.PVPtitanScore);
    this.photonView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, (object) this.teamScores);
  }

  private void resetGameSettings() => SettingsManager.LegacyGameSettings.SetDefault();

  private void resetSettings(bool isLeave)
  {
    this.name = LoginFengKAI.player.name;
    FengGameManagerMKII.masterRC = false;
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCteam, (object) 0);
    if (isLeave)
    {
      FengGameManagerMKII.currentLevel = string.Empty;
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.currentLevel, (object) string.Empty);
      this.levelCache = new List<string[]>();
      this.titanSpawns.Clear();
      this.playerSpawnsC.Clear();
      this.playerSpawnsM.Clear();
      this.titanSpawners.Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.globalVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.RCRegions).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.RCEvents).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.RCVariableNames).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
      ((Dictionary<object, object>) FengGameManagerMKII.RCRegionTriggers).Clear();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statACL, (object) 100);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statBLA, (object) 100);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statGAS, (object) 100);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statSPD, (object) 100);
      this.restartingTitan = false;
      this.restartingMC = false;
      this.restartingHorse = false;
      this.restartingEren = false;
      this.restartingBomb = false;
    }
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    this.resetGameSettings();
    FengGameManagerMKII.banHash = new Hashtable();
    FengGameManagerMKII.imatitan = new Hashtable();
    FengGameManagerMKII.oldScript = string.Empty;
    FengGameManagerMKII.ignoreList = new List<int>();
    this.restartCount = new List<float>();
    FengGameManagerMKII.heroHash = new Hashtable();
  }

  private IEnumerator respawnE(float seconds)
  {
    FengGameManagerMKII fengGameManagerMkii = this;
label_1:
    do
    {
      yield return (object) new WaitForSeconds(seconds);
    }
    while (fengGameManagerMkii.isLosing || fengGameManagerMkii.isWinning);
    for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
    {
      PhotonPlayer player = PhotonNetwork.playerList[index];
      if (player.customProperties[(object) PhotonPlayerProperty.RCteam] == null && RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2)
        fengGameManagerMkii.photonView.RPC("respawnHeroInNewRound", player);
    }
    goto label_1;
  }

  [RPC]
  private void respawnHeroInNewRound()
  {
    if (this.needChooseSide || !GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
      return;
    this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
    this.ShowHUDInfoCenter(string.Empty);
  }

  public IEnumerator restartE(float time)
  {
    yield return (object) new WaitForSeconds(time);
    this.restartGame2();
  }

  public void restartGame2(bool masterclientSwitched = false)
  {
    if (this.gameTimesUp)
      return;
    this.PVPtitanScore = 0;
    this.PVPhumanScore = 0;
    this.startRacing = false;
    this.endRacing = false;
    this.checkpoint = (GameObject) null;
    this.timeElapse = 0.0f;
    this.roundTime = 0.0f;
    this.isWinning = false;
    this.isLosing = false;
    this.isPlayer1Winning = false;
    this.isPlayer2Winning = false;
    this.wave = 1;
    this.myRespawnTime = 0.0f;
    this.kicklist = new ArrayList();
    this.killInfoGO = new ArrayList();
    this.racingResult = new ArrayList();
    this.ShowHUDInfoCenter(string.Empty);
    this.isRestarting = true;
    this.DestroyAllExistingCloths();
    PhotonNetwork.DestroyAll();
    Hashtable hash = this.checkGameGUI();
    this.photonView.RPC("settingRPC", PhotonTargets.Others, (object) hash);
    this.photonView.RPC("RPCLoadLevel", PhotonTargets.All);
    this.setGameSettings(hash);
    if (!masterclientSwitched)
      return;
    this.sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]).hexColor());
  }

  [RPC]
  private void restartGameByClient()
  {
  }

  public void restartGameSingle2()
  {
    this.startRacing = false;
    this.endRacing = false;
    this.checkpoint = (GameObject) null;
    this.single_kills = 0;
    this.single_maxDamage = 0;
    this.single_totalDamage = 0;
    this.timeElapse = 0.0f;
    this.roundTime = 0.0f;
    this.timeTotalServer = 0.0f;
    this.isWinning = false;
    this.isLosing = false;
    this.isPlayer1Winning = false;
    this.isPlayer2Winning = false;
    this.wave = 1;
    this.myRespawnTime = 0.0f;
    this.ShowHUDInfoCenter(string.Empty);
    this.DestroyAllExistingCloths();
    Application.LoadLevel(Application.loadedLevel);
  }

  public void restartRC()
  {
    ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
    ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
    ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
    ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
    ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
    ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
    if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
      this.endGameInfectionRC();
    else
      this.endGameRC();
  }

  public RCActionHelper returnHelper(string str)
  {
    string[] strArray = str.Split('.');
    if (float.TryParse(str, out float _))
      strArray = new string[1]{ str };
    List<RCActionHelper> rcActionHelperList = new List<RCActionHelper>();
    int sentType = 0;
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (rcActionHelperList.Count == 0)
      {
        string s = strArray[index];
        if (s.StartsWith("\"") && s.EndsWith("\""))
        {
          RCActionHelper rcActionHelper = new RCActionHelper(0, 0, (object) s.Substring(1, s.Length - 2));
          rcActionHelperList.Add(rcActionHelper);
          sentType = 2;
        }
        else
        {
          int result1;
          if (int.TryParse(s, out result1))
          {
            RCActionHelper rcActionHelper = new RCActionHelper(0, 0, (object) result1);
            rcActionHelperList.Add(rcActionHelper);
            sentType = 0;
          }
          else
          {
            float result2;
            if (float.TryParse(s, out result2))
            {
              RCActionHelper rcActionHelper = new RCActionHelper(0, 0, (object) result2);
              rcActionHelperList.Add(rcActionHelper);
              sentType = 3;
            }
            else if (s.ToLower() == "true" || s.ToLower() == "false")
            {
              RCActionHelper rcActionHelper = new RCActionHelper(0, 0, (object) Convert.ToBoolean(s.ToLower()));
              rcActionHelperList.Add(rcActionHelper);
              sentType = 1;
            }
            else if (s.StartsWith("Variable"))
            {
              int num1 = s.IndexOf('(');
              int num2 = s.LastIndexOf(')');
              if (s.StartsWith("VariableInt"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 0, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
              }
              else if (s.StartsWith("VariableBool"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 1, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 1;
              }
              else if (s.StartsWith("VariableString"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 2, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 2;
              }
              else if (s.StartsWith("VariableFloat"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 3, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
              }
              else if (s.StartsWith("VariablePlayer"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 4, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 4;
              }
              else if (s.StartsWith("VariableTitan"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(1, 5, (object) this.returnHelper(s.Substring(num1 + 1, num2 - num1 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 5;
              }
            }
            else if (s.StartsWith("Region"))
            {
              int num3 = s.IndexOf('(');
              int num4 = s.LastIndexOf(')');
              if (s.StartsWith("RegionRandomX"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(4, 0, (object) this.returnHelper(s.Substring(num3 + 1, num4 - num3 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
              }
              else if (s.StartsWith("RegionRandomY"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(4, 1, (object) this.returnHelper(s.Substring(num3 + 1, num4 - num3 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
              }
              else if (s.StartsWith("RegionRandomZ"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(4, 2, (object) this.returnHelper(s.Substring(num3 + 1, num4 - num3 - 1)));
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
              }
            }
          }
        }
      }
      else if (rcActionHelperList.Count > 0)
      {
        string str1 = strArray[index];
        if (rcActionHelperList[rcActionHelperList.Count - 1].helperClass == 1)
        {
          switch (rcActionHelperList[rcActionHelperList.Count - 1].helperType)
          {
            case 4:
              if (str1.StartsWith("GetTeam()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 1, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetType()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 0, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetIsAlive()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 2, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 1;
                continue;
              }
              if (str1.StartsWith("GetTitan()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 3, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetKills()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 4, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetDeaths()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 5, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetMaxDmg()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 6, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetTotalDmg()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 7, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetCustomInt()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 8, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetCustomBool()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 9, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 1;
                continue;
              }
              if (str1.StartsWith("GetCustomString()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 10, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 2;
                continue;
              }
              if (str1.StartsWith("GetCustomFloat()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 11, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetPositionX()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 14, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetPositionY()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 15, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetPositionZ()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 16, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetName()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 12, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 2;
                continue;
              }
              if (str1.StartsWith("GetGuildName()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 13, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 2;
                continue;
              }
              if (str1.StartsWith("GetSpeed()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(2, 17, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              continue;
            case 5:
              if (str1.StartsWith("GetType()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 0, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetSize()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 1, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetHealth()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 2, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("GetPositionX()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 3, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetPositionY()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 4, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              if (str1.StartsWith("GetPositionZ()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(3, 5, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              continue;
            default:
              if (str1.StartsWith("ConvertToInt()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 0;
                continue;
              }
              if (str1.StartsWith("ConvertToBool()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 1;
                continue;
              }
              if (str1.StartsWith("ConvertToString()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 2;
                continue;
              }
              if (str1.StartsWith("ConvertToFloat()"))
              {
                RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
                rcActionHelperList.Add(rcActionHelper);
                sentType = 3;
                continue;
              }
              continue;
          }
        }
        else if (str1.StartsWith("ConvertToInt()"))
        {
          RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
          rcActionHelperList.Add(rcActionHelper);
          sentType = 0;
        }
        else if (str1.StartsWith("ConvertToBool()"))
        {
          RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
          rcActionHelperList.Add(rcActionHelper);
          sentType = 1;
        }
        else if (str1.StartsWith("ConvertToString()"))
        {
          RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
          rcActionHelperList.Add(rcActionHelper);
          sentType = 2;
        }
        else if (str1.StartsWith("ConvertToFloat()"))
        {
          RCActionHelper rcActionHelper = new RCActionHelper(5, sentType, (object) null);
          rcActionHelperList.Add(rcActionHelper);
          sentType = 3;
        }
      }
    }
    for (int index = rcActionHelperList.Count - 1; index > 0; --index)
      rcActionHelperList[index - 1].setNextHelper(rcActionHelperList[index]);
    return rcActionHelperList[0];
  }

  public static PeerStates returnPeerState(int peerstate)
  {
    switch (peerstate)
    {
      case 0:
        return PeerStates.Authenticated;
      case 1:
        return PeerStates.ConnectedToMaster;
      case 2:
        return PeerStates.DisconnectingFromMasterserver;
      case 3:
        return PeerStates.DisconnectingFromGameserver;
      case 4:
        return PeerStates.DisconnectingFromNameServer;
      default:
        return PeerStates.ConnectingToMasterserver;
    }
  }

  [RPC]
  private void RPCLoadLevel(PhotonMessageInfo info)
  {
    if (info.sender.isMasterClient)
    {
      this.DestroyAllExistingCloths();
      PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
    }
    else if (PhotonNetwork.isMasterClient)
    {
      this.kickPlayerRC(info.sender, true, "false restart.");
    }
    else
    {
      if (FengGameManagerMKII.masterRC)
        return;
      this.restartCount.Add(Time.time);
      foreach (float num in this.restartCount)
      {
        if ((double) Time.time - (double) num > 60.0)
          this.restartCount.Remove(num);
      }
      if (this.restartCount.Count >= 6)
        return;
      this.DestroyAllExistingCloths();
      PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
    }
  }

  public void sendChatContentInfo(string content) => this.photonView.RPC("Chat", PhotonTargets.All, (object) content, (object) string.Empty);

  public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0) => this.photonView.RPC("updateKillInfo", PhotonTargets.All, (object) t1, (object) killer, (object) t2, (object) victim, (object) dmg);

  public static void ServerCloseConnection(
    PhotonPlayer targetPlayer,
    bool requestIpBan,
    string inGameName = null)
  {
    RaiseEventOptions options = new RaiseEventOptions()
    {
      TargetActors = new int[1]{ targetPlayer.ID }
    };
    if (requestIpBan)
    {
      Hashtable eventContent = new Hashtable();
      eventContent[(object) (byte) 0] = (object) true;
      if (inGameName != null && inGameName.Length > 0)
        eventContent[(object) (byte) 1] = (object) inGameName;
      PhotonNetwork.RaiseEvent((byte) 203, (object) eventContent, true, options);
    }
    else
      PhotonNetwork.RaiseEvent((byte) 203, (object) null, true, options);
  }

  public static void ServerRequestAuthentication(string authPassword)
  {
    if (string.IsNullOrEmpty(authPassword))
      return;
    PhotonNetwork.RaiseEvent((byte) 198, (object) new Hashtable()
    {
      [(object) (byte) 0] = (object) authPassword
    }, true, new RaiseEventOptions());
  }

  public static void ServerRequestUnban(string bannedAddress)
  {
    if (string.IsNullOrEmpty(bannedAddress))
      return;
    PhotonNetwork.RaiseEvent((byte) 199, (object) new Hashtable()
    {
      [(object) (byte) 0] = (object) bannedAddress
    }, true, new RaiseEventOptions());
  }

  private void setGameSettings(Hashtable hash)
  {
    this.restartingEren = false;
    this.restartingBomb = false;
    this.restartingHorse = false;
    this.restartingTitan = false;
    LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
    if (((Dictionary<object, object>) hash).ContainsKey((object) "bomb"))
    {
      if (!legacyGameSettings.BombModeEnabled.Value)
      {
        legacyGameSettings.BombModeEnabled.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode enabled.</color>");
      }
    }
    else if (legacyGameSettings.BombModeEnabled.Value)
    {
      legacyGameSettings.BombModeEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode disabled.</color>");
      if (PhotonNetwork.isMasterClient)
        this.restartingBomb = true;
    }
    if (legacyGameSettings.BombModeEnabled.Value && (!((Dictionary<object, object>) hash).ContainsKey((object) "bombCeiling") || (int) hash[(object) "bombCeiling"] == 1))
      MapCeiling.CreateMapCeiling();
    if (!((Dictionary<object, object>) hash).ContainsKey((object) "bombInfiniteGas") || (int) hash[(object) "bombInfiniteGas"] == 1)
      legacyGameSettings.BombModeInfiniteGas.Value = true;
    else
      legacyGameSettings.BombModeInfiniteGas.Value = false;
    legacyGameSettings.GlobalHideNames.Value = ((Dictionary<object, object>) hash).ContainsKey((object) "globalHideNames");
    if (((Dictionary<object, object>) hash).ContainsKey((object) "globalDisableMinimap"))
    {
      if (!legacyGameSettings.GlobalMinimapDisable.Value)
      {
        legacyGameSettings.GlobalMinimapDisable.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
      }
    }
    else if (legacyGameSettings.GlobalMinimapDisable.Value)
    {
      legacyGameSettings.GlobalMinimapDisable.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "globalDisableMinimap"))
    {
      if (!legacyGameSettings.GlobalMinimapDisable.Value)
      {
        legacyGameSettings.GlobalMinimapDisable.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
      }
    }
    else if (legacyGameSettings.GlobalMinimapDisable.Value)
    {
      legacyGameSettings.GlobalMinimapDisable.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "horse"))
    {
      if (!legacyGameSettings.AllowHorses.Value)
      {
        legacyGameSettings.AllowHorses.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Horses enabled.</color>");
      }
    }
    else if (legacyGameSettings.AllowHorses.Value)
    {
      legacyGameSettings.AllowHorses.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Horses disabled.</color>");
      if (PhotonNetwork.isMasterClient)
        this.restartingHorse = true;
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "punkWaves"))
    {
      if (legacyGameSettings.PunksEveryFive.Value)
      {
        legacyGameSettings.PunksEveryFive.Value = false;
        this.chatRoom.addLINE("<color=#FFCC00>Punks every 5 waves disabled.</color>");
      }
    }
    else if (!legacyGameSettings.PunksEveryFive.Value)
    {
      legacyGameSettings.PunksEveryFive.Value = true;
      this.chatRoom.addLINE("<color=#FFCC00>Punks ever 5 waves enabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "ahssReload"))
    {
      if (legacyGameSettings.AHSSAirReload.Value)
      {
        legacyGameSettings.AHSSAirReload.Value = false;
        this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
      }
    }
    else if (!legacyGameSettings.AHSSAirReload.Value)
    {
      legacyGameSettings.AHSSAirReload.Value = true;
      this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload allowed.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "team"))
    {
      if (legacyGameSettings.TeamMode.Value != (int) hash[(object) "team"])
      {
        legacyGameSettings.TeamMode.Value = (int) hash[(object) "team"];
        string str = string.Empty;
        if (legacyGameSettings.TeamMode.Value == 1)
          str = "no sort";
        else if (legacyGameSettings.TeamMode.Value == 2)
          str = "locked by size";
        else if (legacyGameSettings.TeamMode.Value == 3)
          str = "locked by skill";
        this.chatRoom.addLINE("<color=#FFCC00>Team Mode enabled (" + str + ").</color>");
        if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 0)
          this.setTeam(3);
      }
    }
    else if (legacyGameSettings.TeamMode.Value != 0)
    {
      legacyGameSettings.TeamMode.Value = 0;
      this.setTeam(0);
      this.chatRoom.addLINE("<color=#FFCC00>Team mode disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "point"))
    {
      if (!legacyGameSettings.PointModeEnabled.Value || legacyGameSettings.PointModeAmount.Value != (int) hash[(object) "point"])
      {
        legacyGameSettings.PointModeEnabled.Value = true;
        legacyGameSettings.PointModeAmount.Value = (int) hash[(object) "point"];
        this.chatRoom.addLINE("<color=#FFCC00>Point limit enabled (" + Convert.ToString(legacyGameSettings.PointModeAmount.Value) + ").</color>");
      }
    }
    else if (legacyGameSettings.PointModeEnabled.Value)
    {
      legacyGameSettings.PointModeEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Point limit disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "rock"))
    {
      if (legacyGameSettings.RockThrowEnabled.Value)
      {
        legacyGameSettings.RockThrowEnabled.Value = false;
        this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing disabled.</color>");
      }
    }
    else if (!legacyGameSettings.RockThrowEnabled.Value)
    {
      legacyGameSettings.RockThrowEnabled.Value = true;
      this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing enabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "explode"))
    {
      if (!legacyGameSettings.TitanExplodeEnabled.Value || legacyGameSettings.TitanExplodeRadius.Value != (int) hash[(object) "explode"])
      {
        legacyGameSettings.TitanExplodeEnabled.Value = true;
        legacyGameSettings.TitanExplodeRadius.Value = (int) hash[(object) "explode"];
        this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode enabled (Radius " + Convert.ToString(legacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
      }
    }
    else if (legacyGameSettings.TitanExplodeEnabled.Value)
    {
      legacyGameSettings.TitanExplodeEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "healthMode") && ((Dictionary<object, object>) hash).ContainsKey((object) "healthLower") && ((Dictionary<object, object>) hash).ContainsKey((object) "healthUpper"))
    {
      if (legacyGameSettings.TitanHealthMode.Value != (int) hash[(object) "healthMode"] || legacyGameSettings.TitanHealthMin.Value != (int) hash[(object) "healthLower"] || legacyGameSettings.TitanHealthMax.Value != (int) hash[(object) "healthUpper"])
      {
        legacyGameSettings.TitanHealthMode.Value = (int) hash[(object) "healthMode"];
        legacyGameSettings.TitanHealthMin.Value = (int) hash[(object) "healthLower"];
        legacyGameSettings.TitanHealthMax.Value = (int) hash[(object) "healthUpper"];
        string str = "Static";
        if (legacyGameSettings.TitanHealthMode.Value == 2)
          str = "Scaled";
        this.chatRoom.addLINE("<color=#FFCC00>Titan Health (" + str + ", " + legacyGameSettings.TitanHealthMin.Value.ToString() + " to " + legacyGameSettings.TitanHealthMax.Value.ToString() + ") enabled.</color>");
      }
    }
    else if (legacyGameSettings.TitanHealthMode.Value > 0)
    {
      legacyGameSettings.TitanHealthMode.Value = 0;
      this.chatRoom.addLINE("<color=#FFCC00>Titan Health disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "infection"))
    {
      if (!legacyGameSettings.InfectionModeEnabled.Value)
      {
        legacyGameSettings.InfectionModeEnabled.Value = true;
        legacyGameSettings.InfectionModeAmount.Value = (int) hash[(object) "infection"];
        this.name = LoginFengKAI.player.name;
        Hashtable propertiesToSet = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCteam, (object) 0);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.chatRoom.addLINE("<color=#FFCC00>Infection mode (" + Convert.ToString(legacyGameSettings.InfectionModeAmount.Value) + ") enabled. Make sure your first character is human.</color>");
      }
    }
    else if (legacyGameSettings.InfectionModeEnabled.Value)
    {
      legacyGameSettings.InfectionModeEnabled.Value = false;
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
      this.chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
      if (PhotonNetwork.isMasterClient)
        this.restartingTitan = true;
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "eren"))
    {
      if (!legacyGameSettings.KickShifters.Value)
      {
        legacyGameSettings.KickShifters.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren enabled. Using eren transform will get you kicked.</color>");
        if (PhotonNetwork.isMasterClient)
          this.restartingEren = true;
      }
    }
    else if (legacyGameSettings.KickShifters.Value)
    {
      legacyGameSettings.KickShifters.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren disabled. Eren transform is allowed.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "titanc"))
    {
      if (!legacyGameSettings.TitanNumberEnabled.Value || legacyGameSettings.TitanNumber.Value != (int) hash[(object) "titanc"])
      {
        legacyGameSettings.TitanNumberEnabled.Value = true;
        legacyGameSettings.TitanNumber.Value = (int) hash[(object) "titanc"];
        this.chatRoom.addLINE("<color=#FFCC00>" + Convert.ToString(legacyGameSettings.TitanNumber.Value) + " titans will spawn each round.</color>");
      }
    }
    else if (legacyGameSettings.TitanNumberEnabled.Value)
    {
      legacyGameSettings.TitanNumberEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Default titans will spawn each round.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "damage"))
    {
      if (!legacyGameSettings.TitanArmorEnabled.Value || legacyGameSettings.TitanArmor.Value != (int) hash[(object) "damage"])
      {
        legacyGameSettings.TitanArmorEnabled.Value = true;
        legacyGameSettings.TitanArmor.Value = (int) hash[(object) "damage"];
        this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage (" + Convert.ToString(legacyGameSettings.TitanArmor.Value) + ") enabled.</color>");
      }
    }
    else if (legacyGameSettings.TitanArmorEnabled.Value)
    {
      legacyGameSettings.TitanArmorEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "sizeMode") && ((Dictionary<object, object>) hash).ContainsKey((object) "sizeLower") && ((Dictionary<object, object>) hash).ContainsKey((object) "sizeUpper"))
    {
      if (!legacyGameSettings.TitanSizeEnabled.Value || (double) legacyGameSettings.TitanSizeMin.Value != (double) (float) hash[(object) "sizeLower"] || (double) legacyGameSettings.TitanSizeMax.Value != (double) (float) hash[(object) "sizeUpper"])
      {
        legacyGameSettings.TitanSizeEnabled.Value = true;
        legacyGameSettings.TitanSizeMin.Value = (float) hash[(object) "sizeLower"];
        legacyGameSettings.TitanSizeMax.Value = (float) hash[(object) "sizeUpper"];
        this.chatRoom.addLINE("<color=#FFCC00>Custom titan size (" + legacyGameSettings.TitanSizeMin.Value.ToString("F2") + "," + legacyGameSettings.TitanSizeMax.Value.ToString("F2") + ") enabled.</color>");
      }
    }
    else if (legacyGameSettings.TitanSizeEnabled.Value)
    {
      legacyGameSettings.TitanSizeEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Custom titan size disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "spawnMode") && ((Dictionary<object, object>) hash).ContainsKey((object) "nRate") && ((Dictionary<object, object>) hash).ContainsKey((object) "aRate") && ((Dictionary<object, object>) hash).ContainsKey((object) "jRate") && ((Dictionary<object, object>) hash).ContainsKey((object) "cRate") && ((Dictionary<object, object>) hash).ContainsKey((object) "pRate"))
    {
      if (!legacyGameSettings.TitanSpawnEnabled.Value || (double) legacyGameSettings.TitanSpawnNormal.Value != (double) (float) hash[(object) "nRate"] || (double) legacyGameSettings.TitanSpawnAberrant.Value != (double) (float) hash[(object) "aRate"] || (double) legacyGameSettings.TitanSpawnJumper.Value != (double) (float) hash[(object) "jRate"] || (double) legacyGameSettings.TitanSpawnCrawler.Value != (double) (float) hash[(object) "cRate"] || (double) legacyGameSettings.TitanSpawnPunk.Value != (double) (float) hash[(object) "pRate"])
      {
        legacyGameSettings.TitanSpawnEnabled.Value = true;
        legacyGameSettings.TitanSpawnNormal.Value = (float) hash[(object) "nRate"];
        legacyGameSettings.TitanSpawnAberrant.Value = (float) hash[(object) "aRate"];
        legacyGameSettings.TitanSpawnJumper.Value = (float) hash[(object) "jRate"];
        legacyGameSettings.TitanSpawnCrawler.Value = (float) hash[(object) "cRate"];
        legacyGameSettings.TitanSpawnPunk.Value = (float) hash[(object) "pRate"];
        InRoomChat chatRoom = this.chatRoom;
        string[] strArray = new string[11];
        strArray[0] = "<color=#FFCC00>Custom spawn rate enabled (";
        strArray[1] = legacyGameSettings.TitanSpawnNormal.Value.ToString("F2");
        strArray[2] = "% Normal, ";
        strArray[3] = legacyGameSettings.TitanSpawnAberrant.Value.ToString("F2");
        strArray[4] = "% Abnormal, ";
        float num = legacyGameSettings.TitanSpawnJumper.Value;
        strArray[5] = num.ToString("F2");
        strArray[6] = "% Jumper, ";
        num = legacyGameSettings.TitanSpawnCrawler.Value;
        strArray[7] = num.ToString("F2");
        strArray[8] = "% Crawler, ";
        num = legacyGameSettings.TitanSpawnPunk.Value;
        strArray[9] = num.ToString("F2");
        strArray[10] = "% Punk </color>";
        string newLine = string.Concat(strArray);
        chatRoom.addLINE(newLine);
      }
    }
    else if (legacyGameSettings.TitanSpawnEnabled.Value)
    {
      legacyGameSettings.TitanSpawnEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Custom spawn rate disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "waveModeOn") && ((Dictionary<object, object>) hash).ContainsKey((object) "waveModeNum"))
    {
      if (!legacyGameSettings.TitanPerWavesEnabled.Value || legacyGameSettings.TitanPerWaves.Value != (int) hash[(object) "waveModeNum"])
      {
        legacyGameSettings.TitanPerWavesEnabled.Value = true;
        legacyGameSettings.TitanPerWaves.Value = (int) hash[(object) "waveModeNum"];
        this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode (" + legacyGameSettings.TitanPerWaves.Value.ToString() + ") enabled.</color>");
      }
    }
    else if (legacyGameSettings.TitanPerWavesEnabled.Value)
    {
      legacyGameSettings.TitanPerWavesEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "friendly"))
    {
      if (!legacyGameSettings.FriendlyMode.Value)
      {
        legacyGameSettings.FriendlyMode.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>PVP is prohibited.</color>");
      }
    }
    else if (legacyGameSettings.FriendlyMode.Value)
    {
      legacyGameSettings.FriendlyMode.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>PVP is allowed.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "pvp"))
    {
      if (legacyGameSettings.BladePVP.Value != (int) hash[(object) "pvp"])
      {
        legacyGameSettings.BladePVP.Value = (int) hash[(object) "pvp"];
        string str = string.Empty;
        if (legacyGameSettings.BladePVP.Value == 1)
          str = "Team-Based";
        else if (legacyGameSettings.BladePVP.Value == 2)
          str = "FFA";
        this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP enabled (" + str + ").</color>");
      }
    }
    else if (legacyGameSettings.BladePVP.Value != 0)
    {
      legacyGameSettings.BladePVP.Value = 0;
      this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "maxwave"))
    {
      if (!legacyGameSettings.TitanMaxWavesEnabled.Value || legacyGameSettings.TitanMaxWaves.Value != (int) hash[(object) "maxwave"])
      {
        legacyGameSettings.TitanMaxWavesEnabled.Value = true;
        legacyGameSettings.TitanMaxWaves.Value = (int) hash[(object) "maxwave"];
        this.chatRoom.addLINE("<color=#FFCC00>Max wave is " + legacyGameSettings.TitanMaxWaves.Value.ToString() + ".</color>");
      }
    }
    else if (legacyGameSettings.TitanMaxWavesEnabled.Value)
    {
      legacyGameSettings.TitanMaxWavesEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Max wave set to default.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "endless"))
    {
      if (!legacyGameSettings.EndlessRespawnEnabled.Value || legacyGameSettings.EndlessRespawnTime.Value != (int) hash[(object) "endless"])
      {
        legacyGameSettings.EndlessRespawnEnabled.Value = true;
        legacyGameSettings.EndlessRespawnTime.Value = (int) hash[(object) "endless"];
        this.chatRoom.addLINE("<color=#FFCC00>Endless respawn enabled (" + legacyGameSettings.EndlessRespawnTime.Value.ToString() + " seconds).</color>");
      }
    }
    else if (legacyGameSettings.EndlessRespawnEnabled.Value)
    {
      legacyGameSettings.EndlessRespawnEnabled.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Endless respawn disabled.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "motd"))
    {
      if (legacyGameSettings.Motd.Value != (string) hash[(object) "motd"])
      {
        legacyGameSettings.Motd.Value = (string) hash[(object) "motd"];
        this.chatRoom.addLINE("<color=#FFCC00>MOTD:" + legacyGameSettings.Motd.Value + "</color>");
      }
    }
    else if (legacyGameSettings.Motd.Value != string.Empty)
      legacyGameSettings.Motd.Value = string.Empty;
    if (((Dictionary<object, object>) hash).ContainsKey((object) "deadlycannons"))
    {
      if (!legacyGameSettings.CannonsFriendlyFire.Value)
      {
        legacyGameSettings.CannonsFriendlyFire.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Cannons will now kill players.</color>");
      }
    }
    else if (legacyGameSettings.CannonsFriendlyFire.Value)
    {
      legacyGameSettings.CannonsFriendlyFire.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Cannons will no longer kill players.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "asoracing"))
    {
      if (!legacyGameSettings.RacingEndless.Value)
      {
        legacyGameSettings.RacingEndless.Value = true;
        this.chatRoom.addLINE("<color=#FFCC00>Racing will not restart on win.</color>");
      }
    }
    else if (legacyGameSettings.RacingEndless.Value)
    {
      legacyGameSettings.RacingEndless.Value = false;
      this.chatRoom.addLINE("<color=#FFCC00>Racing will restart on win.</color>");
    }
    if (((Dictionary<object, object>) hash).ContainsKey((object) "racingStartTime"))
      legacyGameSettings.RacingStartTime.Value = (float) hash[(object) "racingStartTime"];
    else
      legacyGameSettings.RacingStartTime.Value = 20f;
    foreach (HERO hero in this.heroes)
    {
      if (Object.op_Inequality((Object) hero, (Object) null))
        hero.SetName();
    }
  }

  private IEnumerator setGuildFeng()
  {
    WWWForm wwwForm = new WWWForm();
    wwwForm.AddField("name", LoginFengKAI.player.name);
    wwwForm.AddField("guildname", LoginFengKAI.player.guildname);
    yield return !Application.isWebPlayer ? (object) new WWW("http://fenglee.com/game/aog/change_guild_name.php", wwwForm) : (object) new WWW("http://aotskins.com/version/guild.php", wwwForm);
  }

  [RPC]
  private void setMasterRC(PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    FengGameManagerMKII.masterRC = true;
  }

  private void setTeam(int setting)
  {
    switch (setting)
    {
      case 0:
        this.name = LoginFengKAI.player.name;
        Hashtable propertiesToSet1 = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.RCteam, (object) 0);
        ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.name, (object) this.name);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
        break;
      case 1:
        Hashtable propertiesToSet2 = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.RCteam, (object) 1);
        string str1;
        int startIndex1;
        for (str1 = LoginFengKAI.player.name; str1.Contains("[") && str1.Length >= str1.IndexOf("[") + 8; str1 = str1.Remove(startIndex1, 8))
          startIndex1 = str1.IndexOf("[");
        if (!str1.StartsWith("[00FFFF]"))
          str1 = "[00FFFF]" + str1;
        this.name = str1;
        ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.name, (object) this.name);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
        break;
      case 2:
        Hashtable propertiesToSet3 = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.RCteam, (object) 2);
        string str2;
        int startIndex2;
        for (str2 = LoginFengKAI.player.name; str2.Contains("[") && str2.Length >= str2.IndexOf("[") + 8; str2 = str2.Remove(startIndex2, 8))
          startIndex2 = str2.IndexOf("[");
        if (!str2.StartsWith("[FF00FF]"))
          str2 = "[FF00FF]" + str2;
        this.name = str2;
        ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.name, (object) this.name);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet3);
        break;
      case 3:
        int num1 = 0;
        int num2 = 0;
        int setting1 = 1;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
          int num3 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]);
          if (num3 > 0)
          {
            switch (num3)
            {
              case 1:
                ++num1;
                continue;
              case 2:
                ++num2;
                continue;
              default:
                continue;
            }
          }
        }
        if (num1 > num2)
          setting1 = 2;
        this.setTeam(setting1);
        break;
    }
    if (setting != 0 && setting != 1 && setting != 2)
      return;
    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
    {
      if (go.GetPhotonView().isMine)
        this.photonView.RPC("labelRPC", PhotonTargets.All, (object) go.GetPhotonView().viewID);
    }
  }

  [RPC]
  private void setTeamRPC(int setting, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient && !info.sender.isLocal)
      return;
    this.setTeam(setting);
  }

  [RPC]
  private void settingRPC(Hashtable hash, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    this.setGameSettings(hash);
  }

  private void showChatContent(string content)
  {
    this.chatContent.Add((object) content);
    if (this.chatContent.Count > 10)
      this.chatContent.RemoveAt(0);
    GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
    for (int index = 0; index < this.chatContent.Count; ++index)
      GameObject.Find("LabelChatContent").GetComponent<UILabel>().text += this.chatContent[index]?.ToString();
  }

  public void ShowHUDInfoCenter(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoCenter");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text = content;
  }

  public void ShowHUDInfoCenterADD(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoCenter");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text += content;
  }

  private void ShowHUDInfoTopCenter(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text = content;
  }

  private void ShowHUDInfoTopCenterADD(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text += content;
  }

  private void ShowHUDInfoTopLeft(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text = content;
  }

  private void ShowHUDInfoTopRight(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopRight");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text = content;
  }

  private void ShowHUDInfoTopRightMAPNAME(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopRight");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text += content;
  }

  [RPC]
  private void showResult(
    string text0,
    string text1,
    string text2,
    string text3,
    string text4,
    string text6,
    PhotonMessageInfo t)
  {
    if (!this.gameTimesUp && t.sender.isMasterClient)
    {
      this.gameTimesUp = true;
      GameObject gameObject = GameObject.Find("UI_IN_GAME");
      NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[0], false);
      NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[1], false);
      NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[2], true);
      NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[3], false);
      GameObject.Find("LabelName").GetComponent<UILabel>().text = text0;
      GameObject.Find("LabelKill").GetComponent<UILabel>().text = text1;
      GameObject.Find("LabelDead").GetComponent<UILabel>().text = text2;
      GameObject.Find("LabelMaxDmg").GetComponent<UILabel>().text = text3;
      GameObject.Find("LabelTotalDmg").GetComponent<UILabel>().text = text4;
      GameObject.Find("LabelResultTitle").GetComponent<UILabel>().text = text6;
      IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
      this.gameStart = false;
    }
    else
    {
      if (t.sender.isMasterClient || !PhotonNetwork.player.isMasterClient)
        return;
      this.kickPlayerRC(t.sender, true, "false game end.");
    }
  }

  private void SingleShowHUDInfoTopCenter(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.GetComponent<UILabel>().text = content;
  }

  private void SingleShowHUDInfoTopLeft(string content)
  {
    GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    content = content.Replace("[0]", "[*^_^*]");
    gameObject.GetComponent<UILabel>().text = content;
  }

  [RPC]
  public void someOneIsDead(int id = -1)
  {
    switch (IN_GAME_MAIN_CAMERA.gamemode)
    {
      case GAMEMODE.KILL_TITAN:
      case GAMEMODE.SURVIVE_MODE:
      case GAMEMODE.BOSS_FIGHT_CT:
      case GAMEMODE.TROST:
        if (!this.isPlayerAllDead2())
          break;
        this.gameLose2();
        break;
      case GAMEMODE.PVP_AHSS:
        if (SettingsManager.LegacyGameSettings.BladePVP.Value != 0 || SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
          break;
        if (this.isPlayerAllDead2())
        {
          this.gameLose2();
          this.teamWinner = 0;
        }
        if (this.isTeamAllDead2(1))
        {
          this.teamWinner = 2;
          this.gameWin2();
        }
        if (!this.isTeamAllDead2(2))
          break;
        this.teamWinner = 1;
        this.gameWin2();
        break;
      case GAMEMODE.ENDLESS_TITAN:
        ++this.titanScore;
        break;
      case GAMEMODE.PVP_CAPTURE:
        if (id != 0)
          this.PVPtitanScore += 2;
        this.checkPVPpts();
        this.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, (object) this.PVPhumanScore, (object) this.PVPtitanScore);
        break;
    }
  }

  public void SpawnNonAITitan(string id, string tag = "titanRespawn")
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
    GameObject gameObject1 = gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length)];
    this.myLastHero = id.ToUpper();
    GameObject gameObject2 = IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE ? PhotonNetwork.Instantiate("TITAN_VER3.1", gameObject1.transform.position, gameObject1.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", Vector3.op_Addition(this.checkpoint.transform.position, new Vector3((float) Random.Range(-20, 20), 2f, (float) Random.Range(-20, 20))), this.checkpoint.transform.rotation, 0);
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
    gameObject2.GetComponent<TITAN>().nonAI = true;
    gameObject2.GetComponent<TITAN>().speed = 30f;
    ((Behaviour) gameObject2.GetComponent<TITAN_CONTROLLER>()).enabled = true;
    if (id == "RANDOM" && Random.Range(0, 100) < 7)
      gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
    ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
    GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
    GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) false);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    this.ShowHUDInfoCenter(string.Empty);
  }

  public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
  {
    if (FengGameManagerMKII.logicLoaded && FengGameManagerMKII.customLevelLoaded)
    {
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
      GameObject gameObject1 = gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length)];
      Vector3 position = gameObject1.transform.position;
      if (FengGameManagerMKII.level.StartsWith("Custom") && this.titanSpawns.Count > 0)
        position = this.titanSpawns[Random.Range(0, this.titanSpawns.Count)];
      this.myLastHero = id.ToUpper();
      GameObject gameObject2 = IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE ? PhotonNetwork.Instantiate("TITAN_VER3.1", position, gameObject1.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", Vector3.op_Addition(this.checkpoint.transform.position, new Vector3((float) Random.Range(-20, 20), 2f, (float) Random.Range(-20, 20))), this.checkpoint.transform.rotation, 0);
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
      gameObject2.GetComponent<TITAN>().nonAI = true;
      gameObject2.GetComponent<TITAN>().speed = 30f;
      ((Behaviour) gameObject2.GetComponent<TITAN_CONTROLLER>()).enabled = true;
      if (id == "RANDOM" && Random.Range(0, 100) < 7)
        gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
      ((Behaviour) GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>()).enabled = true;
      GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
      GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
      Hashtable propertiesToSet1 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) false);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
      Hashtable propertiesToSet2 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
      this.ShowHUDInfoCenter(string.Empty);
    }
    else
      this.NOTSpawnNonAITitanRC(id);
  }

  public void SpawnPlayer(string id, string tag = "playerRespawn")
  {
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
    {
      this.SpawnPlayerAt2(id, this.checkpoint);
    }
    else
    {
      this.myLastRespawnTag = tag;
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
      GameObject pos = gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length)];
      this.SpawnPlayerAt2(id, pos);
    }
  }

  public void SpawnPlayerAt2(string id, GameObject pos)
  {
    if (!FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded)
    {
      this.NOTSpawnPlayerRC(id);
    }
    else
    {
      Vector3 position = pos.transform.position;
      if (this.racingSpawnPointSet)
        position = this.racingSpawnPoint;
      else if (FengGameManagerMKII.level.StartsWith("Custom"))
      {
        if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 0)
        {
          List<Vector3> vector3List = new List<Vector3>();
          foreach (Vector3 vector3 in this.playerSpawnsC)
            vector3List.Add(vector3);
          foreach (Vector3 vector3 in this.playerSpawnsM)
            vector3List.Add(vector3);
          if (vector3List.Count > 0)
            position = vector3List[Random.Range(0, vector3List.Count)];
        }
        else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
        {
          if (this.playerSpawnsC.Count > 0)
            position = this.playerSpawnsC[Random.Range(0, this.playerSpawnsC.Count)];
        }
        else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2 && this.playerSpawnsM.Count > 0)
          position = this.playerSpawnsM[Random.Range(0, this.playerSpawnsM.Count)];
      }
      IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
      this.myLastHero = id.ToUpper();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
        {
          component.setMainObject((GameObject) Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation));
        }
        else
        {
          component.setMainObject((GameObject) Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation));
          if (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3")
          {
            HeroCostume heroCostume1 = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
            heroCostume1.checkstat();
            CostumeConeveter.HeroCostumeToLocalData(heroCostume1, IN_GAME_MAIN_CAMERA.singleCharacter);
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
            if (heroCostume1 != null)
            {
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume1;
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = heroCostume1.stat;
            }
            else
            {
              HeroCostume heroCostume2 = HeroCostume.costumeOption[3];
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume2;
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume2.name.ToUpper());
            }
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
            component.main_object.GetComponent<HERO>().setStat2();
            component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
          }
          else
          {
            for (int index1 = 0; index1 < HeroCostume.costume.Length; ++index1)
            {
              if (HeroCostume.costume[index1].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
              {
                int index2 = HeroCostume.costume[index1].id + CheckBoxCostume.costumeSet - 1;
                if (HeroCostume.costume[index2].name != HeroCostume.costume[index1].name)
                  index2 = HeroCostume.costume[index1].id + 1;
                ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
                ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[index2];
                ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[index2].name.ToUpper());
                ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
                component.main_object.GetComponent<HERO>().setStat2();
                component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                break;
              }
            }
          }
        }
      }
      else
      {
        component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0));
        id = id.ToUpper();
        if (id == "SET 1" || id == "SET 2" || id == "SET 3")
        {
          HeroCostume heroCostume3 = CostumeConeveter.LocalDataToHeroCostume(id);
          heroCostume3.checkstat();
          CostumeConeveter.HeroCostumeToLocalData(heroCostume3, id);
          if (heroCostume3.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
            heroCostume3 = HeroCostume.costume[6];
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
          if (heroCostume3 != null)
          {
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume3;
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = heroCostume3.stat;
          }
          else
          {
            HeroCostume heroCostume4 = HeroCostume.costumeOption[3];
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume4;
            ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume4.name.ToUpper());
          }
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
          component.main_object.GetComponent<HERO>().setStat2();
          component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
        }
        else
        {
          for (int index3 = 0; index3 < HeroCostume.costume.Length; ++index3)
          {
            if (HeroCostume.costume[index3].name.ToUpper() == id.ToUpper())
            {
              int index4 = HeroCostume.costume[index3].id;
              if (id.ToUpper() != "AHSS")
                index4 += CheckBoxCostume.costumeSet - 1;
              if (HeroCostume.costume[index4].name != HeroCostume.costume[index3].name)
                index4 = HeroCostume.costume[index3].id + 1;
              if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && id.ToUpper() == "AHSS")
                index4 = 6;
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[index4];
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[index4].name.ToUpper());
              ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
              component.main_object.GetComponent<HERO>().setStat2();
              component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
              break;
            }
          }
        }
        CostumeConeveter.HeroCostumeToPhotonData2(((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
        {
          Transform transform = component.main_object.transform;
          transform.position = Vector3.op_Addition(transform.position, new Vector3((float) Random.Range(-20, 20), 2f, (float) Random.Range(-20, 20)));
        }
        Hashtable propertiesToSet1 = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) false);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
        Hashtable propertiesToSet2 = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
      }
      ((Behaviour) component).enabled = true;
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
      GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
      GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
      component.gameOver = false;
      this.isLosing = false;
      this.ShowHUDInfoCenter(string.Empty);
    }
  }

  [RPC]
  public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient || !FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded || this.needChooseSide || !((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
      return;
    Vector3 position;
    // ISSUE: explicit constructor call
    ((Vector3) ref position).\u002Ector(posX, posY, posZ);
    IN_GAME_MAIN_CAMERA component = ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>();
    component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0.0f, 0.0f, 0.0f, 1f), 0));
    string upper = this.myLastHero.ToUpper();
    if (upper == "SET 1" || upper == "SET 2" || upper == "SET 3")
    {
      HeroCostume heroCostume1 = CostumeConeveter.LocalDataToHeroCostume(upper);
      heroCostume1.checkstat();
      CostumeConeveter.HeroCostumeToLocalData(heroCostume1, upper);
      if (heroCostume1.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
        heroCostume1 = HeroCostume.costume[6];
      ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
      if (heroCostume1 != null)
      {
        ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume1;
        ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = heroCostume1.stat;
      }
      else
      {
        HeroCostume heroCostume2 = HeroCostume.costumeOption[3];
        ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = heroCostume2;
        ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume2.name.ToUpper());
      }
      ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
      component.main_object.GetComponent<HERO>().setStat2();
      component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
    }
    else
    {
      for (int index1 = 0; index1 < HeroCostume.costume.Length; ++index1)
      {
        if (HeroCostume.costume[index1].name.ToUpper() == upper.ToUpper())
        {
          int index2 = HeroCostume.costume[index1].id;
          if (upper.ToUpper() != "AHSS")
            index2 += CheckBoxCostume.costumeSet - 1;
          if (HeroCostume.costume[index2].name != HeroCostume.costume[index1].name)
            index2 = HeroCostume.costume[index1].id + 1;
          if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && upper.ToUpper() == "AHSS")
            index2 = 6;
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().init();
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[index2];
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[index2].name.ToUpper());
          ((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().setCharacterComponent();
          component.main_object.GetComponent<HERO>().setStat2();
          component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
          break;
        }
      }
    }
    CostumeConeveter.HeroCostumeToPhotonData2(((Component) component.main_object.GetComponent<HERO>()).GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
    {
      Transform transform = component.main_object.transform;
      transform.position = Vector3.op_Addition(transform.position, new Vector3((float) Random.Range(-20, 20), 2f, (float) Random.Range(-20, 20)));
    }
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) "dead", (object) false);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.isTitan, (object) 1);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    ((Behaviour) component).enabled = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
    GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
    GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
    component.gameOver = false;
    this.isLosing = false;
    this.ShowHUDInfoCenter(string.Empty);
  }

  private void spawnPlayerCustomMap()
  {
    if (this.needChooseSide || !GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
      return;
    ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
    if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
      this.SpawnNonAITitan2(this.myLastHero);
    else
      this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
    this.ShowHUDInfoCenter(string.Empty);
  }

  public GameObject spawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk = false)
  {
    GameObject gameObject = this.spawnTitanRaw(position, rotation);
    if (punk)
      gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
    else if (Random.Range(0, 100) < rate)
    {
      if (IN_GAME_MAIN_CAMERA.difficulty == 2)
      {
        if ((double) Random.Range(0.0f, 1f) < 0.699999988079071 || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
        else
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
      }
    }
    else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
    {
      if ((double) Random.Range(0.0f, 1f) < 0.699999988079071 || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
        gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
      else
        gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
    }
    else if (Random.Range(0, 100) < rate)
    {
      if ((double) Random.Range(0.0f, 1f) < 0.800000011920929 || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
        gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
      else
        gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
    }
    else if ((double) Random.Range(0.0f, 1f) < 0.800000011920929 || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
      gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
    else
      gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
    (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE ? PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject.transform.position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0) : (GameObject) Object.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject.transform.position, Quaternion.Euler(-90f, 0.0f, 0.0f))).transform.localScale = gameObject.transform.localScale;
    return gameObject;
  }

  public void spawnTitanAction(int type, float size, int health, int number)
  {
    Vector3 position;
    // ISSUE: explicit constructor call
    ((Vector3) ref position).\u002Ector(Random.Range(-400f, 400f), 0.0f, Random.Range(-400f, 400f));
    Quaternion rotation;
    // ISSUE: explicit constructor call
    ((Quaternion) ref rotation).\u002Ector(0.0f, 0.0f, 0.0f, 1f);
    if (this.titanSpawns.Count > 0)
    {
      position = this.titanSpawns[Random.Range(0, this.titanSpawns.Count)];
    }
    else
    {
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn");
      if (gameObjectsWithTag.Length != 0)
      {
        int index = Random.Range(0, gameObjectsWithTag.Length);
        GameObject gameObject = gameObjectsWithTag[index];
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
      }
    }
    for (int index = 0; index < number; ++index)
    {
      GameObject gameObject = this.spawnTitanRaw(position, rotation);
      gameObject.GetComponent<TITAN>().resetLevel(size);
      gameObject.GetComponent<TITAN>().hasSetLevel = true;
      if ((double) health > 0.0)
      {
        gameObject.GetComponent<TITAN>().currentHealth = health;
        gameObject.GetComponent<TITAN>().maxHealth = health;
      }
      switch (type)
      {
        case 0:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
          break;
        case 1:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
          break;
        case 2:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
          break;
        case 3:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
          break;
        case 4:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
          break;
      }
    }
  }

  public void spawnTitanAtAction(
    int type,
    float size,
    int health,
    int number,
    float posX,
    float posY,
    float posZ)
  {
    Vector3 position;
    // ISSUE: explicit constructor call
    ((Vector3) ref position).\u002Ector(posX, posY, posZ);
    Quaternion rotation;
    // ISSUE: explicit constructor call
    ((Quaternion) ref rotation).\u002Ector(0.0f, 0.0f, 0.0f, 1f);
    for (int index = 0; index < number; ++index)
    {
      GameObject gameObject = this.spawnTitanRaw(position, rotation);
      gameObject.GetComponent<TITAN>().resetLevel(size);
      gameObject.GetComponent<TITAN>().hasSetLevel = true;
      if ((double) health > 0.0)
      {
        gameObject.GetComponent<TITAN>().currentHealth = health;
        gameObject.GetComponent<TITAN>().maxHealth = health;
      }
      switch (type)
      {
        case 0:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
          break;
        case 1:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
          break;
        case 2:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
          break;
        case 3:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
          break;
        case 4:
          gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
          break;
      }
    }
  }

  public void spawnTitanCustom(string type, int abnormal, int rate, bool punk)
  {
    int val2 = rate;
    if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
      punk = false;
    if (FengGameManagerMKII.level.StartsWith("Custom"))
    {
      val2 = 5;
      if (SettingsManager.LegacyGameSettings.GameType.Value == 1)
        val2 = 3;
      else if (SettingsManager.LegacyGameSettings.GameType.Value == 2 || SettingsManager.LegacyGameSettings.GameType.Value == 3)
        val2 = 0;
    }
    if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value || !SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value && FengGameManagerMKII.level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value >= 2)
    {
      val2 = SettingsManager.LegacyGameSettings.TitanNumber.Value;
      if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
        val2 = 0;
    }
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
    {
      if (punk)
        val2 = rate;
      else if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
      {
        int num = 1;
        if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
          num = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
        val2 += (this.wave - 1) * (num - 1);
      }
      else if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
      {
        int num = 1;
        if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
          num = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
        val2 += (this.wave - 1) * num;
      }
    }
    int num1 = Math.Min(100, val2);
    if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
    {
      float num2 = SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value;
      float num3 = SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value;
      float num4 = SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value;
      float num5 = SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value;
      float num6 = SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value;
      if (punk)
      {
        num2 = 0.0f;
        num3 = 0.0f;
        num4 = 0.0f;
        num5 = 0.0f;
        num6 = 100f;
        num1 = rate;
      }
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn");
      List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        Vector3 position;
        // ISSUE: explicit constructor call
        ((Vector3) ref position).\u002Ector(Random.Range(-400f, 400f), 0.0f, Random.Range(-400f, 400f));
        Quaternion rotation;
        // ISSUE: explicit constructor call
        ((Quaternion) ref rotation).\u002Ector(0.0f, 0.0f, 0.0f, 1f);
        if (this.titanSpawns.Count > 0)
          position = this.titanSpawns[Random.Range(0, this.titanSpawns.Count)];
        else if (gameObjectsWithTag.Length != 0)
        {
          if (gameObjectList.Count <= 0)
            gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
          int index2 = Random.Range(0, gameObjectList.Count);
          GameObject gameObject = gameObjectList[index2];
          position = gameObject.transform.position;
          rotation = gameObject.transform.rotation;
          gameObjectList.RemoveAt(index2);
        }
        float num7 = Random.Range(0.0f, 100f);
        if ((double) num7 <= (double) num2 + (double) num3 + (double) num4 + (double) num5 + (double) num6)
        {
          GameObject gameObject = this.spawnTitanRaw(position, rotation);
          if ((double) num7 < (double) num2)
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
          else if ((double) num7 >= (double) num2 && (double) num7 < (double) num2 + (double) num3)
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
          else if ((double) num7 >= (double) num2 + (double) num3 && (double) num7 < (double) num2 + (double) num3 + (double) num4)
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
          else if ((double) num7 >= (double) num2 + (double) num3 + (double) num4 && (double) num7 < (double) num2 + (double) num3 + (double) num4 + (double) num5)
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
          else if ((double) num7 >= (double) num2 + (double) num3 + (double) num4 + (double) num5 && (double) num7 < (double) num2 + (double) num3 + (double) num4 + (double) num5 + (double) num6)
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
          else
            gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
        }
        else
          this.spawnTitan(abnormal, position, rotation, punk);
      }
    }
    else if (FengGameManagerMKII.level.StartsWith("Custom"))
    {
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn");
      List<GameObject> gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
      for (int index3 = 0; index3 < num1; ++index3)
      {
        Vector3 position;
        // ISSUE: explicit constructor call
        ((Vector3) ref position).\u002Ector(Random.Range(-400f, 400f), 0.0f, Random.Range(-400f, 400f));
        Quaternion rotation;
        // ISSUE: explicit constructor call
        ((Quaternion) ref rotation).\u002Ector(0.0f, 0.0f, 0.0f, 1f);
        if (this.titanSpawns.Count > 0)
          position = this.titanSpawns[Random.Range(0, this.titanSpawns.Count)];
        else if (gameObjectsWithTag.Length != 0)
        {
          if (gameObjectList.Count <= 0)
            gameObjectList = new List<GameObject>((IEnumerable<GameObject>) gameObjectsWithTag);
          int index4 = Random.Range(0, gameObjectList.Count);
          GameObject gameObject = gameObjectList[index4];
          position = gameObject.transform.position;
          rotation = gameObject.transform.rotation;
          gameObjectList.RemoveAt(index4);
        }
        this.spawnTitan(abnormal, position, rotation, punk);
      }
    }
    else
      this.randomSpawnTitan("titanRespawn", abnormal, num1, punk);
  }

  private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation) => IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE ? (GameObject) Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation) : PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);

  [RPC]
  private void spawnTitanRPC(PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    foreach (TITAN titan in this.titans)
    {
      if (titan.photonView.isMine && (!PhotonNetwork.isMasterClient || titan.nonAI))
        PhotonNetwork.Destroy(((Component) titan).gameObject);
    }
    this.SpawnNonAITitan2(this.myLastHero);
  }

  private void Start()
  {
    FengGameManagerMKII.instance = this;
    ((Object) ((Component) this).gameObject).name = "MultiplayerManager";
    HeroCostume.init2();
    CharacterMaterials.init();
    Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
    this.heroes = new ArrayList();
    this.eT = new ArrayList();
    this.titans = new ArrayList();
    this.fT = new ArrayList();
    this.cT = new ArrayList();
    this.hooks = new ArrayList();
    this.name = string.Empty;
    if (FengGameManagerMKII.nameField == null)
      FengGameManagerMKII.nameField = "GUEST" + Random.Range(0, 100000).ToString();
    if (FengGameManagerMKII.privateServerField == null)
      FengGameManagerMKII.privateServerField = string.Empty;
    if (FengGameManagerMKII.privateLobbyField == null)
      FengGameManagerMKII.privateLobbyField = string.Empty;
    FengGameManagerMKII.usernameField = string.Empty;
    FengGameManagerMKII.passwordField = string.Empty;
    this.resetGameSettings();
    FengGameManagerMKII.banHash = new Hashtable();
    FengGameManagerMKII.imatitan = new Hashtable();
    FengGameManagerMKII.oldScript = string.Empty;
    FengGameManagerMKII.currentLevel = string.Empty;
    this.titanSpawns = new List<Vector3>();
    this.playerSpawnsC = new List<Vector3>();
    this.playerSpawnsM = new List<Vector3>();
    this.playersRPC = new List<PhotonPlayer>();
    this.levelCache = new List<string[]>();
    this.titanSpawners = new List<TitanSpawner>();
    this.restartCount = new List<float>();
    FengGameManagerMKII.ignoreList = new List<int>();
    this.groundList = new List<GameObject>();
    FengGameManagerMKII.noRestart = false;
    FengGameManagerMKII.masterRC = false;
    this.isSpawning = false;
    FengGameManagerMKII.intVariables = new Hashtable();
    FengGameManagerMKII.heroHash = new Hashtable();
    FengGameManagerMKII.boolVariables = new Hashtable();
    FengGameManagerMKII.stringVariables = new Hashtable();
    FengGameManagerMKII.floatVariables = new Hashtable();
    FengGameManagerMKII.globalVariables = new Hashtable();
    FengGameManagerMKII.RCRegions = new Hashtable();
    FengGameManagerMKII.RCEvents = new Hashtable();
    FengGameManagerMKII.RCVariableNames = new Hashtable();
    FengGameManagerMKII.RCRegionTriggers = new Hashtable();
    FengGameManagerMKII.playerVariables = new Hashtable();
    FengGameManagerMKII.titanVariables = new Hashtable();
    FengGameManagerMKII.logicLoaded = false;
    FengGameManagerMKII.customLevelLoaded = false;
    FengGameManagerMKII.oldScriptLogic = string.Empty;
    this.customMapMaterials = new Dictionary<string, Material>();
    this.retryTime = 0.0f;
    this.playerList = string.Empty;
    this.updateTime = 0.0f;
    if (Object.op_Equality((Object) this.textureBackgroundBlack, (Object) null))
    {
      this.textureBackgroundBlack = new Texture2D(1, 1, (TextureFormat) 5, false);
      this.textureBackgroundBlack.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 1f));
      this.textureBackgroundBlack.Apply();
    }
    if (Object.op_Equality((Object) this.textureBackgroundBlue, (Object) null))
    {
      this.textureBackgroundBlue = new Texture2D(1, 1, (TextureFormat) 5, false);
      this.textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
      this.textureBackgroundBlue.Apply();
    }
    this.loadconfig();
    List<string> stringList = new List<string>()
    {
      "PanelLogin",
      "LOGIN",
      "VERSION",
      "LabelNetworkStatus"
    };
    List<string> collection = new List<string>()
    {
      "AOTTG_HERO",
      "Colossal",
      "Icosphere",
      "Cube",
      "colossal",
      "CITY",
      "city",
      "rock"
    };
    if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
      stringList.AddRange((IEnumerable<string>) collection);
    foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
    {
      foreach (string str in stringList)
      {
        if (((Object) gameObject).name.Contains(str))
          Object.Destroy((Object) gameObject);
      }
    }
  }

  public void titanGetKill(PhotonPlayer player, int Damage, string name)
  {
    Damage = Mathf.Max(10, Damage);
    object[] objArray = new object[1]{ (object) Damage };
    this.photonView.RPC("netShowDamage", player, objArray);
    this.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, (object) name, (object) false);
    this.sendKillInfo(false, (string) player.customProperties[(object) PhotonPlayerProperty.name], true, name, Damage);
    this.playerKillInfoUpdate(player, Damage);
  }

  public void titanGetKillbyServer(int Damage, string name)
  {
    Damage = Mathf.Max(10, Damage);
    this.sendKillInfo(false, LoginFengKAI.player.name, true, name, Damage);
    this.netShowDamage(Damage);
    this.oneTitanDown(name, false);
    this.playerKillInfoUpdate(PhotonNetwork.player, Damage);
  }

  private void tryKick(KickState tmp)
  {
    this.sendChatContentInfo("kicking #" + tmp.name + ", " + (object) tmp.getKickCount() + "/" + (object) (int) ((double) PhotonNetwork.playerList.Length * 0.5) + "vote");
    if (tmp.getKickCount() < (int) ((double) PhotonNetwork.playerList.Length * 0.5))
      return;
    this.kickPhotonPlayer(tmp.name.ToString());
  }

  public void unloadAssets(bool immediate = false)
  {
    if (immediate)
    {
      Resources.UnloadUnusedAssets();
    }
    else
    {
      if (this.isUnloading)
        return;
      this.isUnloading = true;
      this.StartCoroutine(this.unloadAssetsE(10f));
    }
  }

  public IEnumerator unloadAssetsE(float time)
  {
    yield return (object) new WaitForSeconds(time);
    Resources.UnloadUnusedAssets();
    this.isUnloading = false;
  }

  public void unloadAssetsEditor()
  {
    if (this.isUnloading)
      return;
    this.isUnloading = true;
    this.StartCoroutine(this.unloadAssetsE(30f));
  }

  private void Update()
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && Object.op_Inequality((Object) GameObject.Find("LabelNetworkStatus"), (Object) null))
    {
      GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionStateDetailed.ToString();
      if (PhotonNetwork.connected)
      {
        UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
        component.text = component.text + " ping:" + PhotonNetwork.GetPing().ToString();
      }
    }
    if (!this.gameStart)
      return;
    foreach (HERO hero in this.heroes)
      hero.update2();
    foreach (Bullet hook in this.hooks)
      hook.update();
    foreach (TITAN_EREN titanEren in this.eT)
      titanEren.update();
    foreach (TITAN titan in this.titans)
      titan.update2();
    foreach (FEMALE_TITAN femaleTitan in this.fT)
      femaleTitan.update();
    foreach (COLOSSAL_TITAN colossalTitan in this.cT)
      colossalTitan.update2();
    if (!Object.op_Inequality((Object) this.mainCamera, (Object) null))
      return;
    this.mainCamera.update2();
  }

  [RPC]
  private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
  {
    GameObject gameObject1 = GameObject.Find("UI_IN_GAME");
    GameObject gameObject2 = (GameObject) Object.Instantiate(Resources.Load("UI/KillInfo"));
    for (int index = 0; index < this.killInfoGO.Count; ++index)
    {
      GameObject gameObject3 = (GameObject) this.killInfoGO[index];
      if (Object.op_Inequality((Object) gameObject3, (Object) null))
        gameObject3.GetComponent<KillInfoComponent>().moveOn();
    }
    if (this.killInfoGO.Count > 4)
    {
      GameObject gameObject4 = (GameObject) this.killInfoGO[0];
      if (Object.op_Inequality((Object) gameObject4, (Object) null))
        gameObject4.GetComponent<KillInfoComponent>().destory();
      this.killInfoGO.RemoveAt(0);
    }
    gameObject2.transform.parent = gameObject1.GetComponent<UIReferArray>().panels[0].transform;
    gameObject2.GetComponent<KillInfoComponent>().show(t1, killer, t2, victim, dmg);
    this.killInfoGO.Add((object) gameObject2);
    this.ReportKillToChatFeed(killer, victim, dmg);
  }

  public void ReportKillToChatFeed(string killer, string victim, int damage)
  {
    if (!SettingsManager.UISettings.GameFeed.Value)
      return;
    this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> " + killer.hexColor() + " killed " + victim.hexColor() + " for " + damage.ToString() + " damage.");
  }

  [RPC]
  public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient || PhotonPlayer.Find(ID) == null)
      return;
    PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
    string empty = string.Empty;
    string str = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
    ((Dictionary<object, object>) FengGameManagerMKII.banHash).Add((object) ID, (object) str);
  }

  public IEnumerator WaitAndRecompilePlayerList(float time)
  {
    FengGameManagerMKII fengGameManagerMkii = this;
    yield return (object) new WaitForSeconds(time);
    string str1 = string.Empty;
    string empty;
    int num1;
    int num2;
    int num3;
    int num4;
    if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
    {
      foreach (PhotonPlayer player in PhotonNetwork.playerList)
      {
        if (player.customProperties[(object) PhotonPlayerProperty.dead] != null)
        {
          if (FengGameManagerMKII.ignoreList.Contains(player.ID))
            str1 += "[FF0000][X] ";
          string str2 = (!player.isLocal ? str1 + "[FFCC00]" : str1 + "[00CC00]") + "[" + Convert.ToString(player.ID) + "] ";
          if (player.isMasterClient)
            str2 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
            str2 = str2 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num5 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.team]);
            if (num5 < 2)
              str2 = str2 + "[" + ColorSet.color_human + "] H ";
            else if (num5 == 2)
              str2 = str2 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str2 = str2 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str3 = str2;
          empty = string.Empty;
          string str4 = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num6 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num7 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num8 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num9 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          string str5 = str3 + string.Empty + str4 + "[ffffff]:" + (object) num6 + "/" + (object) num7 + "/" + (object) num8 + "/" + (object) num9;
          if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
            str5 += "[-]";
          str1 = str5 + "\n";
        }
      }
    }
    else
    {
      int num10 = 0;
      int num11 = 0;
      int num12 = 0;
      int num13 = 0;
      int num14 = 0;
      int num15 = 0;
      int num16 = 0;
      int num17 = 0;
      Dictionary<int, PhotonPlayer> dictionary1 = new Dictionary<int, PhotonPlayer>();
      Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
      Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
      foreach (PhotonPlayer player in PhotonNetwork.playerList)
      {
        if (player.customProperties[(object) PhotonPlayerProperty.dead] != null && !FengGameManagerMKII.ignoreList.Contains(player.ID))
        {
          switch (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]))
          {
            case 0:
              dictionary3.Add(player.ID, player);
              continue;
            case 1:
              dictionary1.Add(player.ID, player);
              num10 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
              num12 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
              num14 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
              num16 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
              continue;
            case 2:
              dictionary2.Add(player.ID, player);
              num11 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
              num13 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]);
              num15 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.max_dmg]);
              num17 += RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.total_dmg]);
              continue;
            default:
              continue;
          }
        }
      }
      fengGameManagerMkii.cyanKills = num10;
      fengGameManagerMkii.magentaKills = num11;
      if (PhotonNetwork.isMasterClient)
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value != 2)
        {
          if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
          {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
              int num18 = 0;
              int num19 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]);
              if (num19 > 0)
              {
                switch (num19)
                {
                  case 1:
                    int num20 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
                    if (num11 + num20 + 7 < num10 - num20)
                    {
                      num18 = 2;
                      num11 += num20;
                      num10 -= num20;
                      break;
                    }
                    break;
                  case 2:
                    int num21 = RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]);
                    if (num10 + num21 + 7 < num11 - num21)
                    {
                      num18 = 1;
                      num10 += num21;
                      num11 -= num21;
                      break;
                    }
                    break;
                }
                if (num18 > 0)
                  fengGameManagerMkii.photonView.RPC("setTeamRPC", player, (object) num18);
              }
            }
          }
        }
        else
        {
          foreach (PhotonPlayer player in PhotonNetwork.playerList)
          {
            int num22 = 0;
            if (dictionary1.Count > dictionary2.Count + 1)
            {
              num22 = 2;
              if (dictionary1.ContainsKey(player.ID))
                dictionary1.Remove(player.ID);
              if (!dictionary2.ContainsKey(player.ID))
                dictionary2.Add(player.ID, player);
            }
            else if (dictionary2.Count > dictionary1.Count + 1)
            {
              num22 = 1;
              if (!dictionary1.ContainsKey(player.ID))
                dictionary1.Add(player.ID, player);
              if (dictionary2.ContainsKey(player.ID))
                dictionary2.Remove(player.ID);
            }
            if (num22 > 0)
              fengGameManagerMkii.photonView.RPC("setTeamRPC", player, (object) num22);
          }
        }
      }
      string str6 = str1 + "[00FFFF]TEAM CYAN" + "[ffffff]:" + (object) fengGameManagerMkii.cyanKills + "/" + (object) num12 + "/" + (object) num14 + "/" + (object) num16 + "\n";
      foreach (PhotonPlayer photonPlayer in dictionary1.Values)
      {
        int num23 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num23 == 1)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str6 += "[FF0000][X] ";
          str6 = !photonPlayer.isLocal ? str6 + "[FFCC00]" : str6 + "[00CC00]";
          str6 = str6 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str6 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str6 = str6 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num24 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num24 < 2)
              str6 = str6 + "[" + ColorSet.color_human + "] H ";
            else if (num24 == 2)
              str6 = str6 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str6 = str6 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str7 = str6;
          empty = string.Empty;
          string str8 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num25 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num26 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num27 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num28 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str6 = str7 + string.Empty + str8 + "[ffffff]:" + (object) num25 + "/" + (object) num26 + "/" + (object) num27 + "/" + (object) num28;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str6 += "[-]";
          str6 += "\n";
        }
      }
      string str9 = str6 + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + (object) fengGameManagerMkii.magentaKills + "/" + (object) num13 + "/" + (object) num15 + "/" + (object) num17 + "\n";
      foreach (PhotonPlayer photonPlayer in dictionary2.Values)
      {
        int num29 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num29 == 2)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str9 += "[FF0000][X] ";
          str9 = !photonPlayer.isLocal ? str9 + "[FFCC00]" : str9 + "[00CC00]";
          str9 = str9 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str9 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str9 = str9 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num30 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num30 < 2)
              str9 = str9 + "[" + ColorSet.color_human + "] H ";
            else if (num30 == 2)
              str9 = str9 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str9 = str9 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str10 = str9;
          empty = string.Empty;
          string str11 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num31 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num32 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num33 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num34 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str9 = str10 + string.Empty + str11 + "[ffffff]:" + (object) num31 + "/" + (object) num32 + "/" + (object) num33 + "/" + (object) num34;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str9 += "[-]";
          str9 += "\n";
        }
      }
      str1 = str9 + " \n" + "[00FF00]INDIVIDUAL\n";
      foreach (PhotonPlayer photonPlayer in dictionary3.Values)
      {
        int num35 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
        if (photonPlayer.customProperties[(object) PhotonPlayerProperty.dead] != null && num35 == 0)
        {
          if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
            str1 += "[FF0000][X] ";
          str1 = !photonPlayer.isLocal ? str1 + "[FFCC00]" : str1 + "[00CC00]";
          str1 = str1 + "[" + Convert.ToString(photonPlayer.ID) + "] ";
          if (photonPlayer.isMasterClient)
            str1 += "[ffffff][M] ";
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str1 = str1 + "[" + ColorSet.color_red + "] *dead* ";
          if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) < 2)
          {
            int num36 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            if (num36 < 2)
              str1 = str1 + "[" + ColorSet.color_human + "] H ";
            else if (num36 == 2)
              str1 = str1 + "[" + ColorSet.color_human_1 + "] A ";
          }
          else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2)
            str1 = str1 + "[" + ColorSet.color_titan_player + "] <T> ";
          string str12 = str1;
          empty = string.Empty;
          string str13 = RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
          num1 = 0;
          int num37 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
          num2 = 0;
          int num38 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
          num3 = 0;
          int num39 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
          num4 = 0;
          int num40 = RCextensions.returnIntFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
          str1 = str12 + string.Empty + str13 + "[ffffff]:" + (object) num37 + "/" + (object) num38 + "/" + (object) num39 + "/" + (object) num40;
          if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]))
            str1 += "[-]";
          str1 += "\n";
        }
      }
    }
    fengGameManagerMkii.playerList = str1;
    if (PhotonNetwork.isMasterClient && !fengGameManagerMkii.isWinning && !fengGameManagerMkii.isLosing && (double) fengGameManagerMkii.roundTime >= 5.0)
    {
      if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
      {
        int num41 = 0;
        for (int index1 = 0; index1 < PhotonNetwork.playerList.Length; ++index1)
        {
          PhotonPlayer player = PhotonNetwork.playerList[index1];
          if (!FengGameManagerMKII.ignoreList.Contains(player.ID) && player.customProperties[(object) PhotonPlayerProperty.dead] != null && player.customProperties[(object) PhotonPlayerProperty.isTitan] != null)
          {
            if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 1)
            {
              if (RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.deaths]) > 0)
              {
                if (!((Dictionary<object, object>) FengGameManagerMKII.imatitan).ContainsKey((object) player.ID))
                  ((Dictionary<object, object>) FengGameManagerMKII.imatitan).Add((object) player.ID, (object) 2);
                Hashtable propertiesToSet = new Hashtable();
                ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.isTitan, (object) 2);
                player.SetCustomProperties(propertiesToSet);
                fengGameManagerMkii.photonView.RPC("spawnTitanRPC", player);
              }
              else if (((Dictionary<object, object>) FengGameManagerMKII.imatitan).ContainsKey((object) player.ID))
              {
                for (int index2 = 0; index2 < fengGameManagerMkii.heroes.Count; ++index2)
                {
                  HERO hero = (HERO) fengGameManagerMkii.heroes[index2];
                  if (hero.photonView.owner == player)
                  {
                    hero.markDie();
                    hero.photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "no switching in infection");
                  }
                }
              }
            }
            else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
              ++num41;
          }
        }
        if (num41 <= 0 && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.KILL_TITAN)
          fengGameManagerMkii.gameWin2();
      }
      else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
        {
          if (fengGameManagerMkii.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
          {
            object[] objArray = new object[2]
            {
              (object) "<color=#00FFFF>Team Cyan wins! </color>",
              (object) string.Empty
            };
            fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
            fengGameManagerMkii.gameWin2();
          }
          else if (fengGameManagerMkii.magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
          {
            object[] objArray = new object[2]
            {
              (object) "<color=#FF00FF>Team Magenta wins! </color>",
              (object) string.Empty
            };
            fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
            fengGameManagerMkii.gameWin2();
          }
        }
        else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
        {
          for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
          {
            PhotonPlayer player = PhotonNetwork.playerList[index];
            if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
            {
              object[] objArray = new object[2]
              {
                (object) ("<color=#FFCC00>" + RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]).hexColor() + " wins!</color>"),
                (object) string.Empty
              };
              fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
              fengGameManagerMkii.gameWin2();
            }
          }
        }
      }
      else if (!SettingsManager.LegacyGameSettings.PointModeEnabled.Value && (SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0))
      {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
        {
          int num42 = 0;
          int num43 = 0;
          int num44 = 0;
          int num45 = 0;
          for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
          {
            PhotonPlayer player = PhotonNetwork.playerList[index];
            if (!FengGameManagerMKII.ignoreList.Contains(player.ID) && player.customProperties[(object) PhotonPlayerProperty.RCteam] != null && player.customProperties[(object) PhotonPlayerProperty.dead] != null)
            {
              if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
              {
                ++num44;
                if (!RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
                  ++num42;
              }
              else if (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2)
              {
                ++num45;
                if (!RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]))
                  ++num43;
              }
            }
          }
          if (num44 > 0 && num45 > 0)
          {
            if (num42 == 0)
            {
              object[] objArray = new object[2]
              {
                (object) "<color=#FF00FF>Team Magenta wins! </color>",
                (object) string.Empty
              };
              fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
              fengGameManagerMkii.gameWin2();
            }
            else if (num43 == 0)
            {
              object[] objArray = new object[2]
              {
                (object) "<color=#00FFFF>Team Cyan wins! </color>",
                (object) string.Empty
              };
              fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
              fengGameManagerMkii.gameWin2();
            }
          }
        }
        else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0 && PhotonNetwork.playerList.Length > 1)
        {
          int num46 = 0;
          string text = "Nobody";
          PhotonPlayer player1 = PhotonNetwork.playerList[0];
          for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
          {
            PhotonPlayer player2 = PhotonNetwork.playerList[index];
            if (player2.customProperties[(object) PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(player2.customProperties[(object) PhotonPlayerProperty.dead]))
            {
              text = RCextensions.returnStringFromObject(player2.customProperties[(object) PhotonPlayerProperty.name]).hexColor();
              player1 = player2;
              ++num46;
            }
          }
          if (num46 <= 1)
          {
            string str14 = " 5 points added.";
            if (text == "Nobody")
            {
              str14 = string.Empty;
            }
            else
            {
              for (int index = 0; index < 5; ++index)
                fengGameManagerMkii.playerKillInfoUpdate(player1, 0);
            }
            object[] objArray = new object[2]
            {
              (object) ("<color=#FFCC00>" + text.hexColor() + " wins." + str14 + "</color>"),
              (object) string.Empty
            };
            fengGameManagerMkii.photonView.RPC("Chat", PhotonTargets.All, objArray);
            fengGameManagerMkii.gameWin2();
          }
        }
      }
    }
    fengGameManagerMkii.isRecompiling = false;
  }

  public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
  {
    yield return (object) new WaitForSeconds(5f);
    string key = RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]);
    if (this.PreservedPlayerKDR.ContainsKey(key))
    {
      int[] numArray = this.PreservedPlayerKDR[key];
      this.PreservedPlayerKDR.Remove(key);
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.kills, (object) numArray[0]);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.deaths, (object) numArray[1]);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.max_dmg, (object) numArray[2]);
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.total_dmg, (object) numArray[3]);
      player.SetCustomProperties(propertiesToSet);
    }
  }

  public IEnumerator WaitAndResetRestarts()
  {
    yield return (object) new WaitForSeconds(10f);
    this.restartingBomb = false;
    this.restartingEren = false;
    this.restartingHorse = false;
    this.restartingMC = false;
    this.restartingTitan = false;
  }

  public IEnumerator WaitAndRespawn1(float time, string str)
  {
    yield return (object) new WaitForSeconds(time);
    this.SpawnPlayer(this.myLastHero, str);
  }

  public IEnumerator WaitAndRespawn2(float time, GameObject pos)
  {
    yield return (object) new WaitForSeconds(time);
    this.SpawnPlayerAt2(this.myLastHero, pos);
  }

  private enum LoginStates
  {
    notlogged,
    loggingin,
    loginfailed,
    loggedin,
  }
}
