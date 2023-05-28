// Decompiled with JetBrains decompiler
// Type: RCAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

internal class RCAction
{
  private int actionClass;
  private int actionType;
  private RCEvent nextEvent;
  private RCActionHelper[] parameters;

  public RCAction(int category, int type, RCEvent next, RCActionHelper[] helpers)
  {
    this.actionClass = category;
    this.actionType = type;
    this.nextEvent = next;
    this.parameters = helpers;
  }

  public void callException(string str) => FengGameManagerMKII.instance.chatRoom.addLINE(str);

  public void doAction()
  {
    switch (this.actionClass)
    {
      case 0:
        this.nextEvent.checkEvent();
        break;
      case 1:
        string key1 = this.parameters[0].returnString((object) null);
        int y1 = this.parameters[1].returnInt((object) null);
        switch (this.actionType)
        {
          case 0:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Add((object) key1, (object) y1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) y1;
            return;
          case 1:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) ((int) FengGameManagerMKII.intVariables[(object) key1] + y1);
            return;
          case 2:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) ((int) FengGameManagerMKII.intVariables[(object) key1] - y1);
            return;
          case 3:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) ((int) FengGameManagerMKII.intVariables[(object) key1] * y1);
            return;
          case 4:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) ((int) FengGameManagerMKII.intVariables[(object) key1] / y1);
            return;
          case 5:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) ((int) FengGameManagerMKII.intVariables[(object) key1] % y1);
            return;
          case 6:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              this.callException("Variable not found: " + key1);
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) (int) Math.Pow((double) (int) FengGameManagerMKII.intVariables[(object) key1], (double) y1);
            return;
          case 7:
            return;
          case 8:
            return;
          case 9:
            return;
          case 10:
            return;
          case 11:
            return;
          case 12:
            if (!((Dictionary<object, object>) FengGameManagerMKII.intVariables).ContainsKey((object) key1))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Add((object) key1, (object) Random.Range(y1, this.parameters[2].returnInt((object) null)));
              return;
            }
            FengGameManagerMKII.intVariables[(object) key1] = (object) Random.Range(y1, this.parameters[2].returnInt((object) null));
            return;
          default:
            return;
        }
      case 2:
        string key2 = this.parameters[0].returnString((object) null);
        bool flag = this.parameters[1].returnBool((object) null);
        switch (this.actionType)
        {
          case 0:
            if (!((Dictionary<object, object>) FengGameManagerMKII.boolVariables).ContainsKey((object) key2))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Add((object) key2, (object) flag);
              return;
            }
            FengGameManagerMKII.boolVariables[(object) key2] = (object) flag;
            return;
          case 11:
            if (!((Dictionary<object, object>) FengGameManagerMKII.boolVariables).ContainsKey((object) key2))
            {
              this.callException("Variable not found: " + key2);
              return;
            }
            FengGameManagerMKII.boolVariables[(object) key2] = (object) !(bool) FengGameManagerMKII.boolVariables[(object) key2];
            return;
          case 12:
            if (!((Dictionary<object, object>) FengGameManagerMKII.boolVariables).ContainsKey((object) key2))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Add((object) key2, (object) Convert.ToBoolean(Random.Range(0, 2)));
              return;
            }
            FengGameManagerMKII.boolVariables[(object) key2] = (object) Convert.ToBoolean(Random.Range(0, 2));
            return;
          default:
            return;
        }
      case 3:
        string key3 = this.parameters[0].returnString((object) null);
        switch (this.actionType)
        {
          case 0:
            string str1 = this.parameters[1].returnString((object) null);
            if (!((Dictionary<object, object>) FengGameManagerMKII.stringVariables).ContainsKey((object) key3))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Add((object) key3, (object) str1);
              return;
            }
            FengGameManagerMKII.stringVariables[(object) key3] = (object) str1;
            return;
          case 7:
            string empty = string.Empty;
            for (int index = 1; index < this.parameters.Length; ++index)
              empty += this.parameters[index].returnString((object) null);
            if (!((Dictionary<object, object>) FengGameManagerMKII.stringVariables).ContainsKey((object) key3))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Add((object) key3, (object) empty);
              return;
            }
            FengGameManagerMKII.stringVariables[(object) key3] = (object) empty;
            return;
          case 8:
            string str2 = this.parameters[1].returnString((object) null);
            if (!((Dictionary<object, object>) FengGameManagerMKII.stringVariables).ContainsKey((object) key3))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.stringVariables[(object) key3] = (object) ((string) FengGameManagerMKII.stringVariables[(object) key3] + str2);
            return;
          case 9:
            this.parameters[1].returnString((object) null);
            if (!((Dictionary<object, object>) FengGameManagerMKII.stringVariables).ContainsKey((object) key3))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.stringVariables[(object) key3] = (object) ((string) FengGameManagerMKII.stringVariables[(object) key3]).Replace(this.parameters[1].returnString((object) null), this.parameters[2].returnString((object) null));
            return;
          default:
            return;
        }
      case 4:
        string key4 = this.parameters[0].returnString((object) null);
        float y2 = this.parameters[1].returnFloat((object) null);
        switch (this.actionType)
        {
          case 0:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Add((object) key4, (object) y2);
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) y2;
            return;
          case 1:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) ((double) (float) FengGameManagerMKII.floatVariables[(object) key4] + (double) y2);
            return;
          case 2:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) ((double) (float) FengGameManagerMKII.floatVariables[(object) key4] - (double) y2);
            return;
          case 3:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) ((double) (float) FengGameManagerMKII.floatVariables[(object) key4] * (double) y2);
            return;
          case 4:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) ((double) (float) FengGameManagerMKII.floatVariables[(object) key4] / (double) y2);
            return;
          case 5:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) ((double) (float) FengGameManagerMKII.floatVariables[(object) key4] % (double) y2);
            return;
          case 6:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              this.callException("No Variable");
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) (float) Math.Pow((double) (int) FengGameManagerMKII.floatVariables[(object) key4], (double) y2);
            return;
          case 7:
            return;
          case 8:
            return;
          case 9:
            return;
          case 10:
            return;
          case 11:
            return;
          case 12:
            if (!((Dictionary<object, object>) FengGameManagerMKII.floatVariables).ContainsKey((object) key4))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Add((object) key4, (object) Random.Range(y2, this.parameters[2].returnFloat((object) null)));
              return;
            }
            FengGameManagerMKII.floatVariables[(object) key4] = (object) Random.Range(y2, this.parameters[2].returnFloat((object) null));
            return;
          default:
            return;
        }
      case 5:
        string key5 = this.parameters[0].returnString((object) null);
        PhotonPlayer photonPlayer = this.parameters[1].returnPlayer((object) null);
        if (this.actionType != 0)
          break;
        if (!((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) key5))
        {
          ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) key5, (object) photonPlayer);
          break;
        }
        FengGameManagerMKII.playerVariables[(object) key5] = (object) photonPlayer;
        break;
      case 6:
        string key6 = this.parameters[0].returnString((object) null);
        TITAN titan1 = this.parameters[1].returnTitan((object) null);
        if (this.actionType != 0)
          break;
        if (!((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) key6))
        {
          ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) key6, (object) titan1);
          break;
        }
        FengGameManagerMKII.titanVariables[(object) key6] = (object) titan1;
        break;
      case 7:
        PhotonPlayer targetPlayer = this.parameters[0].returnPlayer((object) null);
        switch (this.actionType)
        {
          case 0:
            int id1 = targetPlayer.ID;
            if (((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id1))
            {
              HERO hero = (HERO) FengGameManagerMKII.heroHash[(object) id1];
              hero.markDie();
              hero.photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) (this.parameters[1].returnString((object) null) + " "));
              return;
            }
            this.callException("Player Not Alive");
            return;
          case 1:
            FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", targetPlayer);
            return;
          case 2:
            FengGameManagerMKII.instance.photonView.RPC("spawnPlayerAtRPC", targetPlayer, (object) this.parameters[1].returnFloat((object) null), (object) this.parameters[2].returnFloat((object) null), (object) this.parameters[3].returnFloat((object) null));
            return;
          case 3:
            int id2 = targetPlayer.ID;
            if (((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id2))
            {
              ((MonoBehaviour) FengGameManagerMKII.heroHash[(object) id2]).photonView.RPC("moveToRPC", targetPlayer, (object) this.parameters[1].returnFloat((object) null), (object) this.parameters[2].returnFloat((object) null), (object) this.parameters[3].returnFloat((object) null));
              return;
            }
            this.callException("Player Not Alive");
            return;
          case 4:
            Hashtable propertiesToSet1 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.kills, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet1);
            return;
          case 5:
            Hashtable propertiesToSet2 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet2);
            return;
          case 6:
            Hashtable propertiesToSet3 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.max_dmg, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet3);
            return;
          case 7:
            Hashtable propertiesToSet4 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet4).Add((object) PhotonPlayerProperty.total_dmg, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet4);
            return;
          case 8:
            Hashtable propertiesToSet5 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet5).Add((object) PhotonPlayerProperty.name, (object) this.parameters[1].returnString((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet5);
            return;
          case 9:
            Hashtable propertiesToSet6 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet6).Add((object) PhotonPlayerProperty.guildName, (object) this.parameters[1].returnString((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet6);
            return;
          case 10:
            Hashtable propertiesToSet7 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet7).Add((object) PhotonPlayerProperty.RCteam, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet7);
            return;
          case 11:
            Hashtable propertiesToSet8 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet8).Add((object) PhotonPlayerProperty.customInt, (object) this.parameters[1].returnInt((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet8);
            return;
          case 12:
            Hashtable propertiesToSet9 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet9).Add((object) PhotonPlayerProperty.customBool, (object) this.parameters[1].returnBool((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet9);
            return;
          case 13:
            Hashtable propertiesToSet10 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet10).Add((object) PhotonPlayerProperty.customString, (object) this.parameters[1].returnString((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet10);
            return;
          case 14:
            Hashtable propertiesToSet11 = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet11).Add((object) PhotonPlayerProperty.RCteam, (object) this.parameters[1].returnFloat((object) null));
            targetPlayer.SetCustomProperties(propertiesToSet11);
            return;
          default:
            return;
        }
      case 8:
        switch (this.actionType)
        {
          case 0:
            TITAN titan2 = this.parameters[0].returnTitan((object) null);
            object[] objArray = new object[2]
            {
              (object) this.parameters[1].returnPlayer((object) null).ID,
              (object) this.parameters[2].returnInt((object) null)
            };
            titan2.photonView.RPC("titanGetHit", titan2.photonView.owner, objArray);
            return;
          case 1:
            FengGameManagerMKII.instance.spawnTitanAction(this.parameters[0].returnInt((object) null), this.parameters[1].returnFloat((object) null), this.parameters[2].returnInt((object) null), this.parameters[3].returnInt((object) null));
            return;
          case 2:
            FengGameManagerMKII.instance.spawnTitanAtAction(this.parameters[0].returnInt((object) null), this.parameters[1].returnFloat((object) null), this.parameters[2].returnInt((object) null), this.parameters[3].returnInt((object) null), this.parameters[4].returnFloat((object) null), this.parameters[5].returnFloat((object) null), this.parameters[6].returnFloat((object) null));
            return;
          case 3:
            TITAN titan3 = this.parameters[0].returnTitan((object) null);
            int num = this.parameters[1].returnInt((object) null);
            titan3.currentHealth = num;
            if (titan3.maxHealth == 0)
              titan3.maxHealth = titan3.currentHealth;
            titan3.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, (object) titan3.currentHealth, (object) titan3.maxHealth);
            return;
          case 4:
            TITAN titan4 = this.parameters[0].returnTitan((object) null);
            if (titan4.photonView.isMine)
            {
              titan4.moveTo(this.parameters[1].returnFloat((object) null), this.parameters[2].returnFloat((object) null), this.parameters[3].returnFloat((object) null));
              return;
            }
            titan4.photonView.RPC("moveToRPC", titan4.photonView.owner, (object) this.parameters[1].returnFloat((object) null), (object) this.parameters[2].returnFloat((object) null), (object) this.parameters[3].returnFloat((object) null));
            return;
          default:
            return;
        }
      case 9:
        switch (this.actionType)
        {
          case 0:
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, (object) this.parameters[0].returnString((object) null), (object) string.Empty);
            return;
          case 1:
            FengGameManagerMKII.instance.gameWin2();
            if (!this.parameters[0].returnBool((object) null))
              return;
            ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
            return;
          case 2:
            FengGameManagerMKII.instance.gameLose2();
            if (!this.parameters[0].returnBool((object) null))
              return;
            ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
            ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
            return;
          case 3:
            if (this.parameters[0].returnBool((object) null))
            {
              ((Dictionary<object, object>) FengGameManagerMKII.intVariables).Clear();
              ((Dictionary<object, object>) FengGameManagerMKII.boolVariables).Clear();
              ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Clear();
              ((Dictionary<object, object>) FengGameManagerMKII.floatVariables).Clear();
              ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Clear();
              ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Clear();
            }
            FengGameManagerMKII.instance.restartGame2();
            return;
          default:
            return;
        }
    }
  }

  public enum actionClasses
  {
    typeVoid,
    typeVariableInt,
    typeVariableBool,
    typeVariableString,
    typeVariableFloat,
    typeVariablePlayer,
    typeVariableTitan,
    typePlayer,
    typeTitan,
    typeGame,
  }

  public enum gameTypes
  {
    printMessage,
    winGame,
    loseGame,
    restartGame,
  }

  public enum playerTypes
  {
    killPlayer,
    spawnPlayer,
    spawnPlayerAt,
    movePlayer,
    setKills,
    setDeaths,
    setMaxDmg,
    setTotalDmg,
    setName,
    setGuildName,
    setTeam,
    setCustomInt,
    setCustomBool,
    setCustomString,
    setCustomFloat,
  }

  public enum titanTypes
  {
    killTitan,
    spawnTitan,
    spawnTitanAt,
    setHealth,
    moveTitan,
  }

  public enum varTypes
  {
    set,
    add,
    subtract,
    multiply,
    divide,
    modulo,
    power,
    concat,
    append,
    remove,
    replace,
    toOpposite,
    setRandom,
  }
}
