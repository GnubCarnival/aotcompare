// Decompiled with JetBrains decompiler
// Type: InRoomChat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Anticheat;
using ExitGames.Client.Photon;
using Photon;
using Settings;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class InRoomChat : MonoBehaviour
{
  private bool AlignBottom = true;
  public static readonly string ChatRPC = "Chat";
  public static Rect GuiRect = new Rect(0.0f, 100f, 300f, 470f);
  public static Rect GuiRect2 = new Rect(30f, 575f, 300f, 25f);
  private string inputLine = string.Empty;
  public bool IsVisible = true;
  public static LinkedList<string> messages = new LinkedList<string>();
  private float deltaTime;
  private int _maxLines = 15;

  private void ShowFPS()
  {
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) ((double) Screen.width / 4.0 - 75.0), 10f, 150f, 30f);
    int num = (int) Math.Round(1.0 / (double) this.deltaTime);
    GUI.Label(rect, string.Format("FPS: {0}", (object) num));
  }

  private void ShowMessageWindow()
  {
    GUI.SetNextControlName(string.Empty);
    GUILayout.BeginArea(InRoomChat.GuiRect);
    GUILayout.FlexibleSpace();
    string str = string.Empty;
    foreach (string message in InRoomChat.messages)
      str = str + message + "\n";
    GUILayout.Label(str, new GUILayoutOption[0]);
    GUILayout.EndArea();
  }

  public void Update() => this.deltaTime += (float) (((double) Time.unscaledDeltaTime - (double) this.deltaTime) * 0.10000000149011612);

  public void addLINE(string newLine)
  {
    newLine = newLine.FilterSizeTag();
    InRoomChat.messages.AddLast(newLine);
    while (InRoomChat.messages.Count > this._maxLines)
      InRoomChat.messages.RemoveFirst();
  }

  public void OnGUI()
  {
    if (SettingsManager.GraphicsSettings.ShowFPS.Value)
      this.ShowFPS();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && SettingsManager.UISettings.GameFeed.Value)
      this.ShowMessageWindow();
    if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerStates.Joined)
      return;
    if (Event.current.type == 4)
    {
      if ((Event.current.keyCode == 9 || Event.current.character == '\t') && !GameMenu.Paused && !SettingsManager.InputSettings.General.Chat.Contains((KeyCode) 9))
      {
        Event.current.Use();
        goto label_249;
      }
    }
    else if (Event.current.type == 5 && Event.current.keyCode != null && SettingsManager.InputSettings.General.Chat.Contains(Event.current.keyCode) && GUI.GetNameOfFocusedControl() != "ChatInput" && Event.current.keyCode != 271 && Event.current.keyCode != 13)
    {
      this.inputLine = string.Empty;
      GUI.FocusControl("ChatInput");
      goto label_249;
    }
    if (Event.current.type == 4 && (Event.current.keyCode == 271 || Event.current.keyCode == 13))
    {
      if (!string.IsNullOrEmpty(this.inputLine))
      {
        if (this.inputLine == "\t")
        {
          this.inputLine = string.Empty;
          GUI.FocusControl(string.Empty);
          return;
        }
        if (((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) "OnChatInput"))
        {
          string rcVariableName = (string) FengGameManagerMKII.RCVariableNames[(object) "OnChatInput"];
          if (((Dictionary<object, object>) FengGameManagerMKII.stringVariables).ContainsKey((object) rcVariableName))
            FengGameManagerMKII.stringVariables[(object) rcVariableName] = (object) this.inputLine;
          else
            ((Dictionary<object, object>) FengGameManagerMKII.stringVariables).Add((object) rcVariableName, (object) this.inputLine);
          ((RCEvent) FengGameManagerMKII.RCEvents[(object) "OnChatInput"]).checkEvent();
        }
        if (!this.inputLine.StartsWith("/"))
        {
          string str = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]).hexColor();
          if (str == string.Empty)
          {
            str = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]);
            if (PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
            {
              if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
                str = "<color=#00FFFF>" + str + "</color>";
              else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2)
                str = "<color=#FF00FF>" + str + "</color>";
            }
          }
          object[] objArray = new object[2]
          {
            (object) this.inputLine,
            (object) str
          };
          FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
        }
        else if (this.inputLine.StartsWith("/aso"))
        {
          if (PhotonNetwork.isMasterClient)
          {
            LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
            LegacyGameSettings legacyGameSettingsUi = SettingsManager.LegacyGameSettingsUI;
            switch (this.inputLine.Substring(5))
            {
              case "kdr":
                if (!legacyGameSettings.PreserveKDR.Value)
                {
                  legacyGameSettings.PreserveKDR.Value = true;
                  legacyGameSettingsUi.PreserveKDR.Value = true;
                  this.addLINE("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
                  break;
                }
                legacyGameSettings.PreserveKDR.Value = false;
                legacyGameSettingsUi.PreserveKDR.Value = false;
                this.addLINE("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
                break;
              case "racing":
                if (!legacyGameSettings.RacingEndless.Value)
                {
                  legacyGameSettings.RacingEndless.Value = legacyGameSettingsUi.RacingEndless.Value = true;
                  this.addLINE("<color=#FFCC00>Endless racing enabled.</color>");
                  break;
                }
                legacyGameSettings.RacingEndless.Value = legacyGameSettingsUi.RacingEndless.Value = false;
                this.addLINE("<color=#FFCC00>Endless racing disabled.</color>");
                break;
            }
          }
        }
        else if (this.inputLine == "/pause")
        {
          if (PhotonNetwork.isMasterClient)
          {
            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, (object) true);
            object[] objArray = new object[2]
            {
              (object) "<color=#FFCC00>MasterClient has paused the game.</color>",
              (object) ""
            };
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
          }
          else
            this.addLINE("<color=#FFCC00>error: not master client</color>");
        }
        else if (this.inputLine == "/unpause")
        {
          if (PhotonNetwork.isMasterClient)
          {
            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, (object) false);
            object[] objArray = new object[2]
            {
              (object) "<color=#FFCC00>MasterClient has unpaused the game.</color>",
              (object) ""
            };
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
          }
          else
            this.addLINE("<color=#FFCC00>error: not master client</color>");
        }
        else if (this.inputLine == "/checklevel")
        {
          foreach (PhotonPlayer player in PhotonNetwork.playerList)
            this.addLINE(RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.currentLevel]));
        }
        else if (this.inputLine == "/isrc")
        {
          if (FengGameManagerMKII.masterRC)
            this.addLINE("is RC");
          else
            this.addLINE("not RC");
        }
        else if (this.inputLine == "/ignorelist")
        {
          foreach (int ignore in FengGameManagerMKII.ignoreList)
            this.addLINE(ignore.ToString());
        }
        else if (this.inputLine.StartsWith("/room"))
        {
          if (PhotonNetwork.isMasterClient)
          {
            if (this.inputLine.Substring(6).StartsWith("max"))
            {
              int int32 = Convert.ToInt32(this.inputLine.Substring(10));
              FengGameManagerMKII.instance.maxPlayers = int32;
              PhotonNetwork.room.maxPlayers = int32;
              object[] objArray = new object[2]
              {
                (object) ("<color=#FFCC00>Max players changed to " + this.inputLine.Substring(10) + "!</color>"),
                (object) ""
              };
              FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            }
            else if (this.inputLine.Substring(6).StartsWith("time"))
            {
              FengGameManagerMKII.instance.addTime(Convert.ToSingle(this.inputLine.Substring(11)));
              object[] objArray = new object[2]
              {
                (object) ("<color=#FFCC00>" + this.inputLine.Substring(11) + " seconds added to the clock.</color>"),
                (object) ""
              };
              FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            }
          }
          else
            this.addLINE("<color=#FFCC00>error: not master client</color>");
        }
        else if (this.inputLine.StartsWith("/resetkd"))
        {
          if (this.inputLine == "/resetkdall")
          {
            if (PhotonNetwork.isMasterClient)
            {
              foreach (PhotonPlayer player in PhotonNetwork.playerList)
              {
                Hashtable propertiesToSet = new Hashtable();
                ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.kills, (object) 0);
                ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.deaths, (object) 0);
                ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.max_dmg, (object) 0);
                ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.total_dmg, (object) 0);
                player.SetCustomProperties(propertiesToSet);
              }
              object[] objArray = new object[2]
              {
                (object) "<color=#FFCC00>All stats have been reset.</color>",
                (object) ""
              };
              FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            }
            else
              this.addLINE("<color=#FFCC00>error: not master client</color>");
          }
          else
          {
            Hashtable propertiesToSet = new Hashtable();
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.kills, (object) 0);
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.deaths, (object) 0);
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.max_dmg, (object) 0);
            ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.total_dmg, (object) 0);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            this.addLINE("<color=#FFCC00>Your stats have been reset. </color>");
          }
        }
        else if (this.inputLine.StartsWith("/pm"))
        {
          string[] strArray = this.inputLine.Split(' ');
          PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(strArray[1]));
          string str1 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]).hexColor();
          if (str1 == string.Empty)
          {
            str1 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]);
            if (PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
            {
              if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
                str1 = "<color=#00FFFF>" + str1 + "</color>";
              else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2)
                str1 = "<color=#FF00FF>" + str1 + "</color>";
            }
          }
          string str2 = RCextensions.returnStringFromObject(targetPlayer.customProperties[(object) PhotonPlayerProperty.name]).hexColor();
          if (str2 == string.Empty)
          {
            str2 = RCextensions.returnStringFromObject(targetPlayer.customProperties[(object) PhotonPlayerProperty.name]);
            if (targetPlayer.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
            {
              if (RCextensions.returnIntFromObject(targetPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]) == 1)
                str2 = "<color=#00FFFF>" + str2 + "</color>";
              else if (RCextensions.returnIntFromObject(targetPlayer.customProperties[(object) PhotonPlayerProperty.RCteam]) == 2)
                str2 = "<color=#FF00FF>" + str2 + "</color>";
            }
          }
          string str3 = string.Empty;
          for (int index = 2; index < strArray.Length; ++index)
            str3 = str3 + strArray[index] + " ";
          FengGameManagerMKII.instance.photonView.RPC("ChatPM", targetPlayer, (object) str1, (object) str3);
          this.addLINE("<color=#FFC000>TO [" + targetPlayer.ID.ToString() + "]</color> " + str2 + ":" + str3);
        }
        else if (this.inputLine.StartsWith("/team"))
        {
          if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
          {
            if (this.inputLine.Substring(6) == "1" || this.inputLine.Substring(6) == "cyan")
            {
              FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, (object) 1);
              this.addLINE("<color=#00FFFF>You have joined team cyan.</color>");
              foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
              {
                if (go.GetPhotonView().isMine)
                {
                  go.GetComponent<HERO>().markDie();
                  go.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "Team Switch");
                }
              }
            }
            else if (this.inputLine.Substring(6) == "2" || this.inputLine.Substring(6) == "magenta")
            {
              FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, (object) 2);
              this.addLINE("<color=#FF00FF>You have joined team magenta.</color>");
              foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
              {
                if (go.GetPhotonView().isMine)
                {
                  go.GetComponent<HERO>().markDie();
                  go.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "Team Switch");
                }
              }
            }
            else if (this.inputLine.Substring(6) == "0" || this.inputLine.Substring(6) == "individual")
            {
              FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, (object) 0);
              this.addLINE("<color=#00FF00>You have joined individuals.</color>");
              foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
              {
                if (go.GetPhotonView().isMine)
                {
                  go.GetComponent<HERO>().markDie();
                  go.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "Team Switch");
                }
              }
            }
            else
              this.addLINE("<color=#FFCC00>error: invalid team code. Accepted values are 0,1, and 2.</color>");
          }
          else
            this.addLINE("<color=#FFCC00>error: teams are locked or disabled. </color>");
        }
        else if (this.inputLine == "/restart")
        {
          if (PhotonNetwork.isMasterClient)
          {
            object[] objArray = new object[2]
            {
              (object) "<color=#FFCC00>MasterClient has restarted the game!</color>",
              (object) ""
            };
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            FengGameManagerMKII.instance.restartRC();
          }
          else
            this.addLINE("<color=#FFCC00>error: not master client</color>");
        }
        else if (this.inputLine.StartsWith("/specmode"))
        {
          SettingsManager.LegacyGeneralSettings.SpecMode.Value = !SettingsManager.LegacyGeneralSettings.SpecMode.Value;
          if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
          {
            FengGameManagerMKII.instance.EnterSpecMode(true);
            this.addLINE("<color=#FFCC00>You have entered spectator mode.</color>");
          }
          else
          {
            FengGameManagerMKII.instance.EnterSpecMode(false);
            this.addLINE("<color=#FFCC00>You have exited spectator mode.</color>");
          }
        }
        else if (this.inputLine.StartsWith("/fov"))
        {
          int int32 = Convert.ToInt32(this.inputLine.Substring(5));
          Camera.main.fieldOfView = (float) int32;
          this.addLINE("<color=#FFCC00>Field of vision set to " + int32.ToString() + ".</color>");
        }
        else if (this.inputLine == "/clear")
          InRoomChat.messages.Clear();
        else if (this.inputLine.StartsWith("/spectate"))
        {
          int int32 = Convert.ToInt32(this.inputLine.Substring(10));
          foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
          {
            if (go.GetPhotonView().owner.ID == int32)
            {
              ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(go);
              ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            }
          }
        }
        else if (!this.inputLine.StartsWith("/kill"))
        {
          if (this.inputLine.StartsWith("/revive"))
          {
            if (PhotonNetwork.isMasterClient)
            {
              if (this.inputLine == "/reviveall")
              {
                object[] objArray = new object[2]
                {
                  (object) "<color=#FFCC00>All players have been revived.</color>",
                  (object) string.Empty
                };
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                  if (player.customProperties[(object) PhotonPlayerProperty.dead] != null && RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2)
                    FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                }
              }
              else
              {
                int int32 = Convert.ToInt32(this.inputLine.Substring(8));
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                  if (player.ID == int32)
                  {
                    this.addLINE("<color=#FFCC00>Player " + int32.ToString() + " has been revived.</color>");
                    if (player.customProperties[(object) PhotonPlayerProperty.dead] != null && RCextensions.returnBoolFromObject(player.customProperties[(object) PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2)
                    {
                      object[] objArray = new object[2]
                      {
                        (object) "<color=#FFCC00>You have been revived by the master client.</color>",
                        (object) string.Empty
                      };
                      FengGameManagerMKII.instance.photonView.RPC("Chat", player, objArray);
                      FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                    }
                  }
                }
              }
            }
            else
              this.addLINE("<color=#FFCC00>error: not master client</color>");
          }
          else if (this.inputLine.StartsWith("/unban"))
          {
            if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
              FengGameManagerMKII.ServerRequestUnban(this.inputLine.Substring(7));
            else if (PhotonNetwork.isMasterClient)
            {
              int int32 = Convert.ToInt32(this.inputLine.Substring(7));
              if (((Dictionary<object, object>) FengGameManagerMKII.banHash).ContainsKey((object) int32))
              {
                object[] objArray = new object[2]
                {
                  (object) ("<color=#FFCC00>" + (string) FengGameManagerMKII.banHash[(object) int32] + " has been unbanned from the server. </color>"),
                  (object) string.Empty
                };
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
                ((Dictionary<object, object>) FengGameManagerMKII.banHash).Remove((object) int32);
              }
              else
                this.addLINE("error: no such player");
            }
            else
              this.addLINE("<color=#FFCC00>error: not master client</color>");
          }
          else if (this.inputLine.StartsWith("/rules"))
          {
            this.addLINE("<color=#FFCC00>Currently activated gamemodes:</color>");
            if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
              this.addLINE("<color=#FFCC00>Bomb mode is on.</color>");
            if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
            {
              if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
                this.addLINE("<color=#FFCC00>Team mode is on (no sort).</color>");
              else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
                this.addLINE("<color=#FFCC00>Team mode is on (sort by size).</color>");
              else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
                this.addLINE("<color=#FFCC00>Team mode is on (sort by skill).</color>");
            }
            if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
              this.addLINE("<color=#FFCC00>Point mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.PointModeAmount.Value) + ").</color>");
            if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
              this.addLINE("<color=#FFCC00>Punk Rock-Throwing is disabled.</color>");
            float num;
            if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
            {
              string[] strArray = new string[11];
              strArray[0] = "<color=#FFCC00>Custom spawn rate is on (";
              num = SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value;
              strArray[1] = num.ToString("F2");
              strArray[2] = "% Normal, ";
              num = SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value;
              strArray[3] = num.ToString("F2");
              strArray[4] = "% Abnormal, ";
              num = SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value;
              strArray[5] = num.ToString("F2");
              strArray[6] = "% Jumper, ";
              num = SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value;
              strArray[7] = num.ToString("F2");
              strArray[8] = "% Crawler, ";
              num = SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value;
              strArray[9] = num.ToString("F2");
              strArray[10] = "% Punk </color>";
              this.addLINE(string.Concat(strArray));
            }
            if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
              this.addLINE("<color=#FFCC00>Titan explode mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
              this.addLINE("<color=#FFCC00>Titan health mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMin.Value) + "-" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMax.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
              this.addLINE("<color=#FFCC00>Infection mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.InfectionModeAmount.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
              this.addLINE("<color=#FFCC00>Nape armor is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanArmor.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
              this.addLINE("<color=#FFCC00>Custom titan # is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanNumber.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
            {
              string[] strArray = new string[5]
              {
                "<color=#FFCC00>Custom titan size is on (",
                null,
                null,
                null,
                null
              };
              num = SettingsManager.LegacyGameSettings.TitanSizeMin.Value;
              strArray[1] = num.ToString("F2");
              strArray[2] = ",";
              num = SettingsManager.LegacyGameSettings.TitanSizeMax.Value;
              strArray[3] = num.ToString("F2");
              strArray[4] = ").</color>";
              this.addLINE(string.Concat(strArray));
            }
            if (SettingsManager.LegacyGameSettings.KickShifters.Value)
              this.addLINE("<color=#FFCC00>Anti-shifter is on. Using shifters will get you kicked.</color>");
            if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
              this.addLINE("<color=#FFCC00>Custom wave mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanMaxWaves.Value) + ").</color>");
            if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
              this.addLINE("<color=#FFCC00>Friendly-Fire disabled. PVP is prohibited.</color>");
            if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
            {
              if (SettingsManager.LegacyGameSettings.BladePVP.Value == 1)
                this.addLINE("<color=#FFCC00>AHSS/Blade PVP is on (team-based).</color>");
              else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 2)
                this.addLINE("<color=#FFCC00>AHSS/Blade PVP is on (FFA).</color>");
            }
            if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
              this.addLINE("<color=#FFCC00>Max Wave set to " + SettingsManager.LegacyGameSettings.TitanMaxWaves.Value.ToString() + "</color>");
            if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
              this.addLINE("<color=#FFCC00>Horses are enabled.</color>");
            if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
              this.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
            if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
              this.addLINE("<color=#FFCC00>Punks will not spawn every five waves.</color>");
            if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
              this.addLINE("<color=#FFCC00>Endless Respawn is enabled (" + SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value.ToString() + " seconds).</color>");
            if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
              this.addLINE("<color=#FFCC00>Minimaps are disabled.</color>");
            if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
              this.addLINE("<color=#FFCC00>MOTD:" + SettingsManager.LegacyGameSettings.Motd.Value + "</color>");
            if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
              this.addLINE("<color=#FFCC00>Cannons will kill humans.</color>");
          }
          else if (this.inputLine.StartsWith("/kick"))
          {
            int int32 = Convert.ToInt32(this.inputLine.Substring(6));
            if (int32 == PhotonNetwork.player.ID)
              this.addLINE("error:can't kick yourself.");
            else if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != MultiplayerServerType.LAN && !PhotonNetwork.isMasterClient)
            {
              object[] objArray = new object[2]
              {
                (object) ("/kick #" + Convert.ToString(int32)),
                (object) LoginFengKAI.player.name
              };
              FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            }
            else
            {
              bool flag = false;
              foreach (PhotonPlayer player in PhotonNetwork.playerList)
              {
                if (int32 == player.ID)
                {
                  flag = true;
                  if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
                    FengGameManagerMKII.instance.kickPlayerRC(player, false, "");
                  else if (PhotonNetwork.isMasterClient)
                  {
                    FengGameManagerMKII.instance.kickPlayerRC(player, false, "");
                    object[] objArray = new object[2]
                    {
                      (object) ("<color=#FFCC00>" + RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]) + " has been kicked from the server!</color>"),
                      (object) string.Empty
                    };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
                  }
                }
              }
              if (!flag)
                this.addLINE("error:no such player.");
            }
          }
          else if (this.inputLine.StartsWith("/ban"))
          {
            if (this.inputLine == "/banlist")
            {
              this.addLINE("<color=#FFCC00>List of banned players:</color>");
              foreach (int key in ((Dictionary<object, object>) FengGameManagerMKII.banHash).Keys)
                this.addLINE("<color=#FFCC00>" + Convert.ToString(key) + ":" + (string) FengGameManagerMKII.banHash[(object) key] + "</color>");
            }
            else
            {
              int int32 = Convert.ToInt32(this.inputLine.Substring(5));
              if (int32 == PhotonNetwork.player.ID)
                this.addLINE("error:can't kick yourself.");
              else if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != MultiplayerServerType.LAN && !PhotonNetwork.isMasterClient)
              {
                object[] objArray = new object[2]
                {
                  (object) ("/kick #" + Convert.ToString(int32)),
                  (object) LoginFengKAI.player.name
                };
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
              }
              else
              {
                bool flag = false;
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                  if (int32 == player.ID)
                  {
                    flag = true;
                    if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
                      FengGameManagerMKII.instance.kickPlayerRC(player, true, "");
                    else if (PhotonNetwork.isMasterClient)
                    {
                      FengGameManagerMKII.instance.kickPlayerRC(player, true, "");
                      object[] objArray = new object[2]
                      {
                        (object) ("<color=#FFCC00>" + RCextensions.returnStringFromObject(player.customProperties[(object) PhotonPlayerProperty.name]) + " has been banned from the server!</color>"),
                        (object) string.Empty
                      };
                      FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
                    }
                  }
                }
                if (!flag)
                  this.addLINE("error:no such player.");
              }
            }
          }
        }
        this.inputLine = string.Empty;
        GUI.FocusControl(string.Empty);
        return;
      }
      this.inputLine = "\t";
      GUI.FocusControl("ChatInput");
    }
label_249:
    this.ShowMessageWindow();
    GUILayout.BeginArea(InRoomChat.GuiRect2);
    GUILayout.BeginHorizontal(new GUILayoutOption[0]);
    GUI.SetNextControlName("ChatInput");
    this.inputLine = GUILayout.TextField(this.inputLine, new GUILayoutOption[0]);
    GUILayout.EndHorizontal();
    GUILayout.EndArea();
  }

  public void setPosition()
  {
    if (!this.AlignBottom)
      return;
    InRoomChat.GuiRect = new Rect(0.0f, (float) (Screen.height - 500), 300f, 470f);
    InRoomChat.GuiRect2 = new Rect(30f, (float) (Screen.height - 300 + 275), 300f, 25f);
  }

  public void Start() => this.setPosition();
}
