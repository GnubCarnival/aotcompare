// Decompiled with JetBrains decompiler
// Type: BTN_choose_titan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class BTN_choose_titan : MonoBehaviour
{
  private void OnClick()
  {
    switch (IN_GAME_MAIN_CAMERA.gamemode)
    {
      case GAMEMODE.PVP_AHSS:
        string id = "AHSS";
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], true);
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
        if (!PhotonNetwork.isMasterClient && (double) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60.0)
        {
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(id);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
        }
        else
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(id, "playerRespawn2");
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
        IN_GAME_MAIN_CAMERA.usingTitan = false;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
        Hashtable propertiesToSet = new Hashtable();
        ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.character, (object) id);
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        return;
      case GAMEMODE.PVP_CAPTURE:
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = GameObject.Find("PVPchkPtT");
        break;
    }
    string selection = GameObject.Find("PopupListCharacterTITAN").GetComponent<UIPopupList>().selection;
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], true);
    if (!PhotonNetwork.isMasterClient && (double) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60.0 || GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().justSuicide)
    {
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().justSuicide = false;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnNonAITitan(selection);
    }
    else
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnNonAITitan2(selection);
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
    NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
    IN_GAME_MAIN_CAMERA.usingTitan = true;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
  }

  private void Start()
  {
    if (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan)
      return;
    ((Component) this).gameObject.GetComponent<UIButton>().isEnabled = false;
  }
}
