// Decompiled with JetBrains decompiler
// Type: TeamExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

internal static class TeamExtensions
{
  public static PunTeams.Team GetTeam(this PhotonPlayer player)
  {
    object obj;
    return ((Dictionary<object, object>) player.customProperties).TryGetValue((object) "team", out obj) ? (PunTeams.Team) obj : PunTeams.Team.none;
  }

  public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
  {
    if (!PhotonNetwork.connectedAndReady)
      Debug.LogWarning((object) ("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed.ToString() + ". Not connectedAndReady."));
    if (PhotonNetwork.player.GetTeam() == team)
      return;
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) nameof (team), (object) (byte) team);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
  }
}
