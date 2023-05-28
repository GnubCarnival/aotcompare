// Decompiled with JetBrains decompiler
// Type: InstantiateTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using Settings;
using UnityEngine;

public class InstantiateTracker
{
  public static readonly InstantiateTracker instance = new InstantiateTracker();
  private InstantiateTracker.Player[] players = new InstantiateTracker.Player[0];

  public bool checkObj(string key, PhotonPlayer photonPlayer, int[] viewIDS)
  {
    if (photonPlayer.isMasterClient || photonPlayer.isLocal)
      return true;
    int num1 = photonPlayer.ID * PhotonNetwork.MAX_VIEW_IDS;
    int num2 = num1 + PhotonNetwork.MAX_VIEW_IDS;
    foreach (int num3 in viewIDS)
    {
      if (num3 <= num1 || num3 >= num2)
      {
        if (PhotonNetwork.isMasterClient)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning invalid photon view.");
        return false;
      }
    }
    key = key.ToLower();
    switch (key)
    {
      case "aot_supply":
      case "colossal_titan":
      case "female_titan":
      case "monsterprefab":
      case "titan_eren_trost":
      case "titan_new_1":
      case "titan_new_2":
        if (!PhotonNetwork.isMasterClient)
          return !FengGameManagerMKII.masterRC && this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
        FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning MC item (" + key + ").");
        return false;
      case "aottg_hero 1":
      case "hook":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
      case "bloodexplore":
      case "bloodsplatter":
      case "fx/justSmoke":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.effect);
      case "fx/bite":
      case "fx/boom1":
      case "fx/boom3":
      case "fx/boom5":
      case "fx/fxtitanspawn":
      case "fx/rockthrow":
        if (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
          return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.effect);
        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingTitan)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning titan effects.");
        return false;
      case "fx/boom1_ct_kick":
      case "fx/colossal_steam":
      case "fx/colossal_steam_dmg":
        if (!PhotonNetwork.isMasterClient || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
          return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.effect);
        FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning colossal effect (" + key + ").");
        return false;
      case "fx/boom2":
      case "fx/boom4":
      case "fx/boost_smoke":
      case "fx/fxtitandie":
      case "fx/fxtitandie1":
      case "fx/thunder":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.effect);
      case "fx/flarebullet1":
      case "fx/flarebullet2":
      case "fx/flarebullet3":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.flare);
      case "fx/shotgun":
      case "fx/shotgun 1":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.shotGun);
      case "hitmeat":
      case "redcross":
      case "redcross1":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.bladeHit);
      case "hitmeat2":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.bloodEffect);
      case "hitmeatbig":
        if (!(RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.character]).ToUpper() != "EREN"))
        {
          if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
            return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.effect);
          if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingEren)
            FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning eren effect (" + key + ").");
          return false;
        }
        if (PhotonNetwork.isMasterClient)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning eren effect (" + key + ").");
        return false;
      case "horse":
        if (LevelInfo.getInfo(FengGameManagerMKII.level).horse || SettingsManager.LegacyGameSettings.AllowHorses.Value)
          return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingHorse)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning horse (" + key + ").");
        return false;
      case "rcasset/bombexplodemain":
      case "rcasset/bombmain":
        if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
          return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.bomb);
        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingBomb)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning bomb item (" + key + ").");
        return false;
      case "rcasset/cannonballobject":
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.bomb);
      case "rcasset/cannonground":
      case "rcasset/cannonwall":
        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.allowedToCannon.ContainsKey(photonPlayer.ID) && !FengGameManagerMKII.instance.restartingMC)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning cannon item (" + key + ").");
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
      case "rcasset/cannongroundprop":
      case "rcasset/cannonwallprop":
        if (PhotonNetwork.isMasterClient)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning MC item (" + key + ").");
        return false;
      case "rock":
        if (!PhotonNetwork.isMasterClient || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
          return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
        FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning MC item (" + key + ").");
        return false;
      case "titan_eren":
        if (!(RCextensions.returnStringFromObject(photonPlayer.customProperties[(object) PhotonPlayerProperty.character]).ToUpper() != "EREN"))
        {
          if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
            return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
          if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingEren)
            FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning titan eren (" + key + ").");
          return false;
        }
        if (PhotonNetwork.isMasterClient)
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, true, "spawning titan eren (" + key + ").");
        return false;
      case "titan_ver3.1":
        if (!PhotonNetwork.isMasterClient)
        {
          if (FengGameManagerMKII.masterRC && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
          {
            int num4 = 0;
            foreach (MonoBehaviour titan in FengGameManagerMKII.instance.getTitans())
            {
              if (titan.photonView.owner == photonPlayer)
                ++num4;
            }
            if (num4 > 1)
              return false;
          }
        }
        else if (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || FengGameManagerMKII.instance.restartingTitan)
        {
          if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
          {
            int num5 = 0;
            foreach (MonoBehaviour titan in FengGameManagerMKII.instance.getTitans())
            {
              if (titan.photonView.owner == photonPlayer)
                ++num5;
            }
            if (num5 > 1)
            {
              FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning titan (" + key + ").");
              return false;
            }
          }
        }
        else
        {
          FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, false, "spawning titan (" + key + ").");
          return false;
        }
        return this.Instantiated(photonPlayer, InstantiateTracker.GameResource.general);
      default:
        return false;
    }
  }

  public void Dispose()
  {
    this.players = (InstantiateTracker.Player[]) null;
    this.players = new InstantiateTracker.Player[0];
  }

  public bool Instantiated(PhotonPlayer owner, InstantiateTracker.GameResource type)
  {
    int result;
    if (this.TryGetPlayer(owner.ID, out result))
    {
      if (this.players[result].IsThingExcessive(type))
      {
        PhotonPlayer player = owner;
        if (player != null && PhotonNetwork.isMasterClient)
          FengGameManagerMKII.instance.kickPlayerRC(player, true, "spamming instantiate (" + type.ToString() + ").");
        RCextensions.RemoveAt<InstantiateTracker.Player>(ref this.players, result);
        return false;
      }
    }
    else
    {
      RCextensions.Add<InstantiateTracker.Player>(ref this.players, new InstantiateTracker.Player(owner.ID));
      this.players[this.players.Length - 1].IsThingExcessive(type);
    }
    return true;
  }

  public bool PropertiesChanged(PhotonPlayer owner)
  {
    int result;
    if (this.TryGetPlayer(owner.ID, out result))
    {
      if (this.players[result].IsThingExcessive(InstantiateTracker.GameResource.name))
        return false;
    }
    else
    {
      RCextensions.Add<InstantiateTracker.Player>(ref this.players, new InstantiateTracker.Player(owner.ID));
      this.players[this.players.Length - 1].IsThingExcessive(InstantiateTracker.GameResource.name);
    }
    return true;
  }

  public void resetPropertyTracking(int ID)
  {
    int result;
    if (!this.TryGetPlayer(ID, out result))
      return;
    this.players[result].resetNameTracking();
  }

  private bool TryGetPlayer(int id, out int result)
  {
    for (int index = 0; index < this.players.Length; ++index)
    {
      if (this.players[index].id == id)
      {
        result = index;
        return true;
      }
    }
    result = -1;
    return false;
  }

  public void TryRemovePlayer(int playerId)
  {
    for (int index = 0; index < this.players.Length; ++index)
    {
      if (this.players[index].id == playerId)
      {
        RCextensions.RemoveAt<InstantiateTracker.Player>(ref this.players, index);
        break;
      }
    }
  }

  private class AhssShots : InstantiateTracker.ThingToCheck
  {
    private float lastShot = Time.time;
    private int shots = 1;

    public AhssShots() => this.type = InstantiateTracker.GameResource.shotGun;

    public override bool KickWorthy()
    {
      if ((double) Time.time - (double) this.lastShot < 1.0)
      {
        ++this.shots;
        if (this.shots > 2)
          return true;
      }
      else
        this.shots = 0;
      this.lastShot = Time.time;
      return false;
    }

    public override void reset()
    {
    }
  }

  private class BladeHitEffect : InstantiateTracker.ThingToCheck
  {
    private float accumTime;
    private float lastHit = Time.time;

    public BladeHitEffect() => this.type = InstantiateTracker.GameResource.bladeHit;

    public override bool KickWorthy()
    {
      float num = Time.time - this.lastHit;
      this.lastHit = Time.time;
      if ((double) num <= 0.30000001192092896)
      {
        this.accumTime += num;
        return (double) this.accumTime >= 1.25;
      }
      this.accumTime = 0.0f;
      return false;
    }

    public override void reset()
    {
    }
  }

  private class BloodEffect : InstantiateTracker.ThingToCheck
  {
    private float accumTime;
    private float lastHit = Time.time;

    public BloodEffect() => this.type = InstantiateTracker.GameResource.bloodEffect;

    public override bool KickWorthy()
    {
      float num = Time.time - this.lastHit;
      this.lastHit = Time.time;
      if ((double) num <= 0.30000001192092896)
      {
        this.accumTime += num;
        return (double) this.accumTime >= 2.0;
      }
      this.accumTime = 0.0f;
      return false;
    }

    public override void reset()
    {
    }
  }

  private class ExcessiveBombs : InstantiateTracker.ThingToCheck
  {
    private int count = 1;
    private float lastClear = Time.time;

    public ExcessiveBombs() => this.type = InstantiateTracker.GameResource.bomb;

    public override bool KickWorthy()
    {
      if ((double) Time.time - (double) this.lastClear > 5.0)
      {
        this.count = 0;
        this.lastClear = Time.time;
      }
      ++this.count;
      return this.count > 20;
    }

    public override void reset()
    {
    }
  }

  private class ExcessiveEffect : InstantiateTracker.ThingToCheck
  {
    private int effectCounter = 1;
    private float lastEffectTime = Time.time;

    public ExcessiveEffect() => this.type = InstantiateTracker.GameResource.effect;

    public override bool KickWorthy()
    {
      if ((double) Time.time - (double) this.lastEffectTime >= 2.0)
      {
        this.effectCounter = 0;
        this.lastEffectTime = Time.time;
      }
      ++this.effectCounter;
      return this.effectCounter > 8;
    }

    public override void reset()
    {
    }
  }

  private class ExcessiveFlares : InstantiateTracker.ThingToCheck
  {
    private int flares = 1;
    private float lastFlare = Time.time;

    public ExcessiveFlares() => this.type = InstantiateTracker.GameResource.flare;

    public override bool KickWorthy()
    {
      if ((double) Time.time - (double) this.lastFlare >= 10.0)
      {
        this.flares = 0;
        this.lastFlare = Time.time;
      }
      ++this.flares;
      return this.flares > 4;
    }

    public override void reset()
    {
    }
  }

  private class ExcessiveNameChange : InstantiateTracker.ThingToCheck
  {
    private float lastNameChange = Time.time;
    private int nameChanges = 1;

    public ExcessiveNameChange() => this.type = InstantiateTracker.GameResource.name;

    public override bool KickWorthy()
    {
      double num = (double) Time.time - (double) this.lastNameChange;
      this.lastNameChange = Time.time;
      if (num >= 5.0)
        this.nameChanges = 0;
      ++this.nameChanges;
      return this.nameChanges > 5;
    }

    public override void reset()
    {
      this.nameChanges = 0;
      this.lastNameChange = Time.time;
    }
  }

  public enum GameResource
  {
    none,
    shotGun,
    effect,
    flare,
    bladeHit,
    bloodEffect,
    general,
    name,
    bomb,
  }

  private class GenerallyExcessive : InstantiateTracker.ThingToCheck
  {
    private int count = 1;
    private float lastClear = Time.time;

    public GenerallyExcessive() => this.type = InstantiateTracker.GameResource.general;

    public override bool KickWorthy()
    {
      if ((double) Time.time - (double) this.lastClear > 5.0)
      {
        this.count = 0;
        this.lastClear = Time.time;
      }
      ++this.count;
      return this.count > 35;
    }

    public override void reset()
    {
    }
  }

  private class Player
  {
    public int id;
    private InstantiateTracker.ThingToCheck[] thingsToCheck;

    public Player(int id)
    {
      this.id = id;
      this.thingsToCheck = new InstantiateTracker.ThingToCheck[0];
    }

    private InstantiateTracker.ThingToCheck GameResourceToThing(InstantiateTracker.GameResource gr)
    {
      switch (gr)
      {
        case InstantiateTracker.GameResource.shotGun:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.AhssShots();
        case InstantiateTracker.GameResource.effect:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.ExcessiveEffect();
        case InstantiateTracker.GameResource.flare:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.ExcessiveFlares();
        case InstantiateTracker.GameResource.bladeHit:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.BladeHitEffect();
        case InstantiateTracker.GameResource.bloodEffect:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.BloodEffect();
        case InstantiateTracker.GameResource.general:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.GenerallyExcessive();
        case InstantiateTracker.GameResource.name:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.ExcessiveNameChange();
        case InstantiateTracker.GameResource.bomb:
          return (InstantiateTracker.ThingToCheck) new InstantiateTracker.ExcessiveBombs();
        default:
          return (InstantiateTracker.ThingToCheck) null;
      }
    }

    private int GetThingToCheck(InstantiateTracker.GameResource type)
    {
      for (int thingToCheck = 0; thingToCheck < this.thingsToCheck.Length; ++thingToCheck)
      {
        if (this.thingsToCheck[thingToCheck].type == type)
          return thingToCheck;
      }
      return -1;
    }

    public bool IsThingExcessive(InstantiateTracker.GameResource gr)
    {
      int thingToCheck = this.GetThingToCheck(gr);
      if (thingToCheck > -1)
        return this.thingsToCheck[thingToCheck].KickWorthy();
      RCextensions.Add<InstantiateTracker.ThingToCheck>(ref this.thingsToCheck, this.GameResourceToThing(gr));
      return false;
    }

    public void resetNameTracking()
    {
      int thingToCheck = this.GetThingToCheck(InstantiateTracker.GameResource.name);
      if (thingToCheck <= -1)
        return;
      this.thingsToCheck[thingToCheck].reset();
    }
  }

  private abstract class ThingToCheck
  {
    public InstantiateTracker.GameResource type;

    public abstract bool KickWorthy();

    public abstract void reset();
  }
}
