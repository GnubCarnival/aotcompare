// Decompiled with JetBrains decompiler
// Type: CostumeConeveter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class CostumeConeveter
{
  private static int DivisionToInt(DIVISION id)
  {
    switch (id)
    {
      case DIVISION.TraineesSquad:
        return 3;
      case DIVISION.TheGarrison:
        return 0;
      case DIVISION.TheMilitaryPolice:
        return 1;
      default:
        return 2;
    }
  }

  public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
  {
    slot = slot.ToUpper();
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.sex, CostumeConeveter.SexToInt(costume.sex));
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.costumeId, costume.costumeId);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.heroCostumeId, costume.id);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.cape, !costume.cape ? 0 : 1);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.skin_color, costume.skin_color);
    PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color1, costume.hair_color.r);
    PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color2, costume.hair_color.g);
    PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color3, costume.hair_color.b);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.division, CostumeConeveter.DivisionToInt(costume.division));
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statSPD, costume.stat.SPD);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statGAS, costume.stat.GAS);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statBLA, costume.stat.BLA);
    PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statACL, costume.stat.ACL);
    PlayerPrefs.SetString(slot + PhotonPlayerProperty.statSKILL, costume.stat.skillId);
  }

  public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
  {
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.sex, (object) CostumeConeveter.SexToInt(costume.sex));
    int num = costume.costumeId;
    if (num == 26)
      num = 25;
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.costumeId, (object) num);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.heroCostumeId, (object) costume.id);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.cape, (object) costume.cape);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.hairInfo, (object) costume.hairInfo.id);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.eye_texture_id, (object) costume.eye_texture_id);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.beard_texture_id, (object) costume.beard_texture_id);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.glass_texture_id, (object) costume.glass_texture_id);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.skin_color, (object) costume.skin_color);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.hair_color1, (object) costume.hair_color.r);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.hair_color2, (object) costume.hair_color.g);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.hair_color3, (object) costume.hair_color.b);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.division, (object) CostumeConeveter.DivisionToInt(costume.division));
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statSPD, (object) costume.stat.SPD);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statGAS, (object) costume.stat.GAS);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statBLA, (object) costume.stat.BLA);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statACL, (object) costume.stat.ACL);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.statSKILL, (object) costume.stat.skillId);
    player.SetCustomProperties(propertiesToSet);
  }

  private static DIVISION IntToDivision(int id)
  {
    switch (id)
    {
      case 0:
        return DIVISION.TheGarrison;
      case 1:
        return DIVISION.TheMilitaryPolice;
      case 3:
        return DIVISION.TraineesSquad;
      default:
        return DIVISION.TheSurveryCorps;
    }
  }

  private static SEX IntToSex(int id)
  {
    if (id == 0)
      return SEX.FEMALE;
    return SEX.MALE;
  }

  private static UNIFORM_TYPE IntToUniformType(int id)
  {
    switch (id)
    {
      case 0:
        return UNIFORM_TYPE.CasualA;
      case 1:
        return UNIFORM_TYPE.CasualB;
      case 3:
        return UNIFORM_TYPE.UniformB;
      case 4:
        return UNIFORM_TYPE.CasualAHSS;
      default:
        return UNIFORM_TYPE.UniformA;
    }
  }

  public static HeroCostume LocalDataToHeroCostume(string slot)
  {
    slot = slot.ToUpper();
    if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.sex))
      return HeroCostume.costume[0];
    HeroCostume heroCostume1 = new HeroCostume();
    HeroCostume heroCostume2 = new HeroCostume()
    {
      sex = CostumeConeveter.IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)),
      id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.heroCostumeId),
      costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.costumeId),
      cape = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.cape) == 1,
      hairInfo = CostumeConeveter.IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)) == SEX.FEMALE ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)],
      eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.eye_texture_id),
      beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.beard_texture_id),
      glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.glass_texture_id),
      skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.skin_color),
      hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color3)),
      division = CostumeConeveter.IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.division)),
      stat = new HeroStat()
    };
    heroCostume2.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statSPD);
    heroCostume2.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statGAS);
    heroCostume2.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statBLA);
    heroCostume2.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statACL);
    heroCostume2.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.statSKILL);
    heroCostume2.setBodyByCostumeId();
    heroCostume2.setMesh2();
    heroCostume2.setTexture();
    return heroCostume2;
  }

  public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
  {
    HeroCostume heroCostume = new HeroCostume();
    SEX sex = CostumeConeveter.IntToSex((int) player.customProperties[(object) PhotonPlayerProperty.sex]);
    HeroCostume heroCostume2 = new HeroCostume()
    {
      sex = sex,
      costumeId = (int) player.customProperties[(object) PhotonPlayerProperty.costumeId],
      id = (int) player.customProperties[(object) PhotonPlayerProperty.heroCostumeId],
      cape = (bool) player.customProperties[(object) PhotonPlayerProperty.cape],
      hairInfo = sex != SEX.MALE ? CostumeHair.hairsF[(int) player.customProperties[(object) PhotonPlayerProperty.hairInfo]] : CostumeHair.hairsM[(int) player.customProperties[(object) PhotonPlayerProperty.hairInfo]],
      eye_texture_id = (int) player.customProperties[(object) PhotonPlayerProperty.eye_texture_id],
      beard_texture_id = (int) player.customProperties[(object) PhotonPlayerProperty.beard_texture_id],
      glass_texture_id = (int) player.customProperties[(object) PhotonPlayerProperty.glass_texture_id],
      skin_color = (int) player.customProperties[(object) PhotonPlayerProperty.skin_color],
      hair_color = new Color((float) player.customProperties[(object) PhotonPlayerProperty.hair_color1], (float) player.customProperties[(object) PhotonPlayerProperty.hair_color2], (float) player.customProperties[(object) PhotonPlayerProperty.hair_color3]),
      division = CostumeConeveter.IntToDivision((int) player.customProperties[(object) PhotonPlayerProperty.division]),
      stat = new HeroStat()
    };
    heroCostume2.stat.SPD = (int) player.customProperties[(object) PhotonPlayerProperty.statSPD];
    heroCostume2.stat.GAS = (int) player.customProperties[(object) PhotonPlayerProperty.statGAS];
    heroCostume2.stat.BLA = (int) player.customProperties[(object) PhotonPlayerProperty.statBLA];
    heroCostume2.stat.ACL = (int) player.customProperties[(object) PhotonPlayerProperty.statACL];
    heroCostume2.stat.skillId = (string) player.customProperties[(object) PhotonPlayerProperty.statSKILL];
    if (heroCostume2.costumeId == 25 && heroCostume2.sex == SEX.FEMALE)
      heroCostume2.costumeId = 26;
    heroCostume2.setBodyByCostumeId();
    heroCostume2.setMesh2();
    heroCostume2.setTexture();
    return heroCostume2;
  }

  private static int SexToInt(SEX id)
  {
    if (id == SEX.FEMALE)
      return 0;
    return 1;
  }

  private static int UniformTypeToInt(UNIFORM_TYPE id)
  {
    switch (id)
    {
      case UNIFORM_TYPE.UniformB:
        return 3;
      case UNIFORM_TYPE.CasualA:
        return 0;
      case UNIFORM_TYPE.CasualB:
        return 1;
      case UNIFORM_TYPE.CasualAHSS:
        return 4;
      default:
        return 2;
    }
  }
}
