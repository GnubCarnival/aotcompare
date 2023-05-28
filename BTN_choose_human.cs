// Decompiled with JetBrains decompiler
// Type: BTN_choose_human
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class BTN_choose_human : MonoBehaviour
{
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

  private void OnClick()
  {
    string selection = GameObject.Find("PopupListCharacterHUMAN").GetComponent<UIPopupList>().selection;
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], true);
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = GameObject.Find("PVPchkPtH");
    if (!PhotonNetwork.isMasterClient && (double) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60.0)
    {
      if (!this.isPlayerAllDead2())
      {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
      }
      else
      {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
      }
    }
    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
    {
      if (this.isPlayerAllDead2())
      {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
      }
      else
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection);
    }
    else
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
    IN_GAME_MAIN_CAMERA.usingTitan = false;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.character, (object) selection);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
  }
}
