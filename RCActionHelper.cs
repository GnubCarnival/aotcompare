// Decompiled with JetBrains decompiler
// Type: RCActionHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

internal class RCActionHelper
{
  public int helperClass;
  public int helperType;
  private RCActionHelper nextHelper;
  private object parameters;

  public RCActionHelper(int sentClass, int sentType, object options)
  {
    this.helperClass = sentClass;
    this.helperType = sentType;
    this.parameters = options;
  }

  public void callException(string str) => FengGameManagerMKII.instance.chatRoom.addLINE(str);

  public bool returnBool(object sentObject)
  {
    object obj = sentObject;
    if (this.parameters != null)
      obj = this.parameters;
    switch (this.helperClass)
    {
      case 0:
        return (bool) obj;
      case 1:
        RCActionHelper rcActionHelper1 = (RCActionHelper) obj;
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnBool(FengGameManagerMKII.intVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 1:
            return (bool) FengGameManagerMKII.boolVariables[(object) rcActionHelper1.returnString((object) null)];
          case 2:
            return this.nextHelper.returnBool(FengGameManagerMKII.stringVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 3:
            return this.nextHelper.returnBool(FengGameManagerMKII.floatVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 4:
            return this.nextHelper.returnBool(FengGameManagerMKII.playerVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 5:
            return this.nextHelper.returnBool(FengGameManagerMKII.titanVariables[(object) rcActionHelper1.returnString((object) null)]);
          default:
            return false;
        }
      case 2:
        PhotonPlayer photonPlayer = (PhotonPlayer) obj;
        if (photonPlayer != null)
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            case 1:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
            case 2:
              return !(bool) photonPlayer.customProperties[(object) PhotonPlayerProperty.dead];
            case 3:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]);
            case 4:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
            case 5:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
            case 6:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
            case 7:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
            case 8:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.customInt]);
            case 9:
              return (bool) photonPlayer.customProperties[(object) PhotonPlayerProperty.customBool];
            case 10:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.customString]);
            case 11:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.customFloat]);
            case 12:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
            case 13:
              return this.nextHelper.returnBool(photonPlayer.customProperties[(object) PhotonPlayerProperty.guildName]);
            case 14:
              int id1 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id1) && this.nextHelper.returnBool((object) ((Component) FengGameManagerMKII.heroHash[(object) id1]).transform.position.x);
            case 15:
              int id2 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id2) && this.nextHelper.returnBool((object) ((Component) FengGameManagerMKII.heroHash[(object) id2]).transform.position.y);
            case 16:
              int id3 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id3) && this.nextHelper.returnBool((object) ((Component) FengGameManagerMKII.heroHash[(object) id3]).transform.position.z);
            case 17:
              int id4 = photonPlayer.ID;
              if (!((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id4))
                return false;
              HERO hero = (HERO) FengGameManagerMKII.heroHash[(object) id4];
              RCActionHelper nextHelper = this.nextHelper;
              Vector3 velocity = ((Component) hero).rigidbody.velocity;
              // ISSUE: variable of a boxed type
              __Boxed<float> magnitude = (ValueType) ((Vector3) ref velocity).magnitude;
              return nextHelper.returnBool((object) magnitude);
          }
        }
        return false;
      case 3:
        TITAN titan = (TITAN) obj;
        if (Object.op_Inequality((Object) titan, (Object) null))
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnBool((object) titan.abnormalType);
            case 1:
              return this.nextHelper.returnBool((object) titan.myLevel);
            case 2:
              return this.nextHelper.returnBool((object) titan.currentHealth);
            case 3:
              return this.nextHelper.returnBool((object) ((Component) titan).transform.position.x);
            case 4:
              return this.nextHelper.returnBool((object) ((Component) titan).transform.position.y);
            case 5:
              return this.nextHelper.returnBool((object) ((Component) titan).transform.position.z);
          }
        }
        return false;
      case 4:
        RCActionHelper rcActionHelper2 = (RCActionHelper) obj;
        RCRegion rcRegion = (RCRegion) FengGameManagerMKII.RCRegions[(object) rcActionHelper2.returnString((object) null)];
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnBool((object) rcRegion.GetRandomX());
          case 1:
            return this.nextHelper.returnBool((object) rcRegion.GetRandomY());
          case 2:
            return this.nextHelper.returnBool((object) rcRegion.GetRandomZ());
          default:
            return false;
        }
      case 5:
        switch (this.helperType)
        {
          case 0:
            return Convert.ToBoolean((int) obj);
          case 1:
            return (bool) obj;
          case 2:
            return Convert.ToBoolean((string) obj);
          case 3:
            return Convert.ToBoolean((float) obj);
          default:
            return false;
        }
      default:
        return false;
    }
  }

  public float returnFloat(object sentObject)
  {
    object s = sentObject;
    if (this.parameters != null)
      s = this.parameters;
    switch (this.helperClass)
    {
      case 0:
        return (float) s;
      case 1:
        RCActionHelper rcActionHelper1 = (RCActionHelper) s;
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnFloat(FengGameManagerMKII.intVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 1:
            return this.nextHelper.returnFloat(FengGameManagerMKII.boolVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 2:
            return this.nextHelper.returnFloat(FengGameManagerMKII.stringVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 3:
            return (float) FengGameManagerMKII.floatVariables[(object) rcActionHelper1.returnString((object) null)];
          case 4:
            return this.nextHelper.returnFloat(FengGameManagerMKII.playerVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 5:
            return this.nextHelper.returnFloat(FengGameManagerMKII.titanVariables[(object) rcActionHelper1.returnString((object) null)]);
          default:
            return 0.0f;
        }
      case 2:
        PhotonPlayer photonPlayer = (PhotonPlayer) s;
        if (photonPlayer != null)
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            case 1:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
            case 2:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]);
            case 3:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]);
            case 4:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
            case 5:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
            case 6:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
            case 7:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
            case 8:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.customInt]);
            case 9:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.customBool]);
            case 10:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.customString]);
            case 11:
              return (float) photonPlayer.customProperties[(object) PhotonPlayerProperty.customFloat];
            case 12:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
            case 13:
              return this.nextHelper.returnFloat(photonPlayer.customProperties[(object) PhotonPlayerProperty.guildName]);
            case 14:
              int id1 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id1) ? ((Component) FengGameManagerMKII.heroHash[(object) id1]).transform.position.x : 0.0f;
            case 15:
              int id2 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id2) ? ((Component) FengGameManagerMKII.heroHash[(object) id2]).transform.position.y : 0.0f;
            case 16:
              int id3 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id3) ? ((Component) FengGameManagerMKII.heroHash[(object) id3]).transform.position.z : 0.0f;
            case 17:
              int id4 = photonPlayer.ID;
              if (!((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id4))
                return 0.0f;
              Vector3 velocity = ((Component) FengGameManagerMKII.heroHash[(object) id4]).rigidbody.velocity;
              return ((Vector3) ref velocity).magnitude;
          }
        }
        return 0.0f;
      case 3:
        TITAN titan = (TITAN) s;
        if (Object.op_Inequality((Object) titan, (Object) null))
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnFloat((object) titan.abnormalType);
            case 1:
              return titan.myLevel;
            case 2:
              return this.nextHelper.returnFloat((object) titan.currentHealth);
            case 3:
              return ((Component) titan).transform.position.x;
            case 4:
              return ((Component) titan).transform.position.y;
            case 5:
              return ((Component) titan).transform.position.z;
          }
        }
        return 0.0f;
      case 4:
        RCActionHelper rcActionHelper2 = (RCActionHelper) s;
        RCRegion rcRegion = (RCRegion) FengGameManagerMKII.RCRegions[(object) rcActionHelper2.returnString((object) null)];
        switch (this.helperType)
        {
          case 0:
            return rcRegion.GetRandomX();
          case 1:
            return rcRegion.GetRandomY();
          case 2:
            return rcRegion.GetRandomZ();
          default:
            return 0.0f;
        }
      case 5:
        switch (this.helperType)
        {
          case 0:
            return Convert.ToSingle((int) s);
          case 1:
            return Convert.ToSingle((bool) s);
          case 2:
            float result;
            return float.TryParse((string) s, out result) ? result : 0.0f;
          case 3:
            return (float) s;
          default:
            return (float) s;
        }
      default:
        return 0.0f;
    }
  }

  public int returnInt(object sentObject)
  {
    object s = sentObject;
    if (this.parameters != null)
      s = this.parameters;
    switch (this.helperClass)
    {
      case 0:
        return (int) s;
      case 1:
        RCActionHelper rcActionHelper1 = (RCActionHelper) s;
        switch (this.helperType)
        {
          case 0:
            return (int) FengGameManagerMKII.intVariables[(object) rcActionHelper1.returnString((object) null)];
          case 1:
            return this.nextHelper.returnInt(FengGameManagerMKII.boolVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 2:
            return this.nextHelper.returnInt(FengGameManagerMKII.stringVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 3:
            return this.nextHelper.returnInt(FengGameManagerMKII.floatVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 4:
            return this.nextHelper.returnInt(FengGameManagerMKII.playerVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 5:
            return this.nextHelper.returnInt(FengGameManagerMKII.titanVariables[(object) rcActionHelper1.returnString((object) null)]);
          default:
            return 0;
        }
      case 2:
        PhotonPlayer photonPlayer = (PhotonPlayer) s;
        if (photonPlayer != null)
        {
          switch (this.helperType)
          {
            case 0:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.team];
            case 1:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam];
            case 2:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]);
            case 3:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan];
            case 4:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.kills];
            case 5:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths];
            case 6:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg];
            case 7:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg];
            case 8:
              return (int) photonPlayer.customProperties[(object) PhotonPlayerProperty.customInt];
            case 9:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.customBool]);
            case 10:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.customString]);
            case 11:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.customFloat]);
            case 12:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.name]);
            case 13:
              return this.nextHelper.returnInt(photonPlayer.customProperties[(object) PhotonPlayerProperty.guildName]);
            case 14:
              int id1 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id1) ? this.nextHelper.returnInt((object) ((Component) FengGameManagerMKII.heroHash[(object) id1]).transform.position.x) : 0;
            case 15:
              int id2 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id2) ? this.nextHelper.returnInt((object) ((Component) FengGameManagerMKII.heroHash[(object) id2]).transform.position.y) : 0;
            case 16:
              int id3 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id3) ? this.nextHelper.returnInt((object) ((Component) FengGameManagerMKII.heroHash[(object) id3]).transform.position.z) : 0;
            case 17:
              int id4 = photonPlayer.ID;
              if (!((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id4))
                return 0;
              HERO hero = (HERO) FengGameManagerMKII.heroHash[(object) id4];
              RCActionHelper nextHelper = this.nextHelper;
              Vector3 velocity = ((Component) hero).rigidbody.velocity;
              // ISSUE: variable of a boxed type
              __Boxed<float> magnitude = (ValueType) ((Vector3) ref velocity).magnitude;
              return nextHelper.returnInt((object) magnitude);
          }
        }
        return 0;
      case 3:
        TITAN titan = (TITAN) s;
        if (Object.op_Inequality((Object) titan, (Object) null))
        {
          switch (this.helperType)
          {
            case 0:
              return (int) titan.abnormalType;
            case 1:
              return this.nextHelper.returnInt((object) titan.myLevel);
            case 2:
              return titan.currentHealth;
            case 3:
              return this.nextHelper.returnInt((object) ((Component) titan).transform.position.x);
            case 4:
              return this.nextHelper.returnInt((object) ((Component) titan).transform.position.y);
            case 5:
              return this.nextHelper.returnInt((object) ((Component) titan).transform.position.z);
          }
        }
        return 0;
      case 4:
        RCActionHelper rcActionHelper2 = (RCActionHelper) s;
        RCRegion rcRegion = (RCRegion) FengGameManagerMKII.RCRegions[(object) rcActionHelper2.returnString((object) null)];
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnInt((object) rcRegion.GetRandomX());
          case 1:
            return this.nextHelper.returnInt((object) rcRegion.GetRandomY());
          case 2:
            return this.nextHelper.returnInt((object) rcRegion.GetRandomZ());
          default:
            return 0;
        }
      case 5:
        switch (this.helperType)
        {
          case 0:
            return (int) s;
          case 1:
            return Convert.ToInt32((bool) s);
          case 2:
            int result;
            return int.TryParse((string) s, out result) ? result : 0;
          case 3:
            return Convert.ToInt32((float) s);
          default:
            return (int) s;
        }
      default:
        return 0;
    }
  }

  public PhotonPlayer returnPlayer(object objParameter)
  {
    object obj = objParameter;
    if (this.parameters != null)
      obj = this.parameters;
    switch (this.helperClass)
    {
      case 1:
        RCActionHelper rcActionHelper = (RCActionHelper) obj;
        return (PhotonPlayer) FengGameManagerMKII.playerVariables[(object) rcActionHelper.returnString((object) null)];
      case 2:
        return (PhotonPlayer) obj;
      default:
        return (PhotonPlayer) obj;
    }
  }

  public string returnString(object sentObject)
  {
    object obj = sentObject;
    if (this.parameters != null)
      obj = this.parameters;
    switch (this.helperClass)
    {
      case 0:
        return (string) obj;
      case 1:
        RCActionHelper rcActionHelper1 = (RCActionHelper) obj;
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnString(FengGameManagerMKII.intVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 1:
            return this.nextHelper.returnString(FengGameManagerMKII.boolVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 2:
            return (string) FengGameManagerMKII.stringVariables[(object) rcActionHelper1.returnString((object) null)];
          case 3:
            return this.nextHelper.returnString(FengGameManagerMKII.floatVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 4:
            return this.nextHelper.returnString(FengGameManagerMKII.playerVariables[(object) rcActionHelper1.returnString((object) null)]);
          case 5:
            return this.nextHelper.returnString(FengGameManagerMKII.titanVariables[(object) rcActionHelper1.returnString((object) null)]);
          default:
            return string.Empty;
        }
      case 2:
        PhotonPlayer photonPlayer = (PhotonPlayer) obj;
        if (photonPlayer != null)
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.team]);
            case 1:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]);
            case 2:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.dead]);
            case 3:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.isTitan]);
            case 4:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.kills]);
            case 5:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.deaths]);
            case 6:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.max_dmg]);
            case 7:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.total_dmg]);
            case 8:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.customInt]);
            case 9:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.customBool]);
            case 10:
              return (string) photonPlayer.customProperties[(object) PhotonPlayerProperty.customString];
            case 11:
              return this.nextHelper.returnString(photonPlayer.customProperties[(object) PhotonPlayerProperty.customFloat]);
            case 12:
              return (string) photonPlayer.customProperties[(object) PhotonPlayerProperty.name];
            case 13:
              return (string) photonPlayer.customProperties[(object) PhotonPlayerProperty.guildName];
            case 14:
              int id1 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id1) ? this.nextHelper.returnString((object) ((Component) FengGameManagerMKII.heroHash[(object) id1]).transform.position.x) : string.Empty;
            case 15:
              int id2 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id2) ? this.nextHelper.returnString((object) ((Component) FengGameManagerMKII.heroHash[(object) id2]).transform.position.y) : string.Empty;
            case 16:
              int id3 = photonPlayer.ID;
              return ((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id3) ? this.nextHelper.returnString((object) ((Component) FengGameManagerMKII.heroHash[(object) id3]).transform.position.z) : string.Empty;
            case 17:
              int id4 = photonPlayer.ID;
              if (!((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id4))
                return string.Empty;
              HERO hero = (HERO) FengGameManagerMKII.heroHash[(object) id4];
              RCActionHelper nextHelper = this.nextHelper;
              Vector3 velocity = ((Component) hero).rigidbody.velocity;
              // ISSUE: variable of a boxed type
              __Boxed<float> magnitude = (ValueType) ((Vector3) ref velocity).magnitude;
              return nextHelper.returnString((object) magnitude);
          }
        }
        return string.Empty;
      case 3:
        TITAN titan = (TITAN) obj;
        if (Object.op_Inequality((Object) titan, (Object) null))
        {
          switch (this.helperType)
          {
            case 0:
              return this.nextHelper.returnString((object) titan.abnormalType);
            case 1:
              return this.nextHelper.returnString((object) titan.myLevel);
            case 2:
              return this.nextHelper.returnString((object) titan.currentHealth);
            case 3:
              return this.nextHelper.returnString((object) ((Component) titan).transform.position.x);
            case 4:
              return this.nextHelper.returnString((object) ((Component) titan).transform.position.y);
            case 5:
              return this.nextHelper.returnString((object) ((Component) titan).transform.position.z);
          }
        }
        return string.Empty;
      case 4:
        RCActionHelper rcActionHelper2 = (RCActionHelper) obj;
        RCRegion rcRegion = (RCRegion) FengGameManagerMKII.RCRegions[(object) rcActionHelper2.returnString((object) null)];
        switch (this.helperType)
        {
          case 0:
            return this.nextHelper.returnString((object) rcRegion.GetRandomX());
          case 1:
            return this.nextHelper.returnString((object) rcRegion.GetRandomY());
          case 2:
            return this.nextHelper.returnString((object) rcRegion.GetRandomZ());
          default:
            return string.Empty;
        }
      case 5:
        switch (this.helperType)
        {
          case 0:
            return ((int) obj).ToString();
          case 1:
            return ((bool) obj).ToString();
          case 2:
            return (string) obj;
          case 3:
            return ((float) obj).ToString();
          default:
            return string.Empty;
        }
      default:
        return string.Empty;
    }
  }

  public TITAN returnTitan(object objParameter)
  {
    object obj = objParameter;
    if (this.parameters != null)
      obj = this.parameters;
    switch (this.helperClass)
    {
      case 1:
        RCActionHelper rcActionHelper = (RCActionHelper) obj;
        return (TITAN) FengGameManagerMKII.titanVariables[(object) rcActionHelper.returnString((object) null)];
      case 3:
        return (TITAN) obj;
      default:
        return (TITAN) obj;
    }
  }

  public void setNextHelper(RCActionHelper sentHelper) => this.nextHelper = sentHelper;

  public enum helperClasses
  {
    primitive,
    variable,
    player,
    titan,
    region,
    convert,
  }

  public enum mathTypes
  {
    add,
    subtract,
    multiply,
    divide,
    modulo,
    power,
  }

  public enum other
  {
    regionX,
    regionY,
    regionZ,
  }

  public enum playerTypes
  {
    playerType,
    playerTeam,
    playerAlive,
    playerTitan,
    playerKills,
    playerDeaths,
    playerMaxDamage,
    playerTotalDamage,
    playerCustomInt,
    playerCustomBool,
    playerCustomString,
    playerCustomFloat,
    playerName,
    playerGuildName,
    playerPosX,
    playerPosY,
    playerPosZ,
    playerSpeed,
  }

  public enum titanTypes
  {
    titanType,
    titanSize,
    titanHealth,
    positionX,
    positionY,
    positionZ,
  }

  public enum variableTypes
  {
    typeInt,
    typeBool,
    typeString,
    typeFloat,
    typePlayer,
    typeTitan,
  }
}
