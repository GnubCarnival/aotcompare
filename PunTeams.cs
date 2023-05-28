// Decompiled with JetBrains decompiler
// Type: PunTeams
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

public class PunTeams : MonoBehaviour
{
  public static Dictionary<PunTeams.Team, List<PhotonPlayer>> PlayersPerTeam;
  public const string TeamPlayerProp = "team";

  public void OnJoinedRoom() => this.UpdateTeams();

  public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) => this.UpdateTeams();

  public void Start()
  {
    PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
    foreach (object key in Enum.GetValues(typeof (PunTeams.Team)))
      PunTeams.PlayersPerTeam[(PunTeams.Team) key] = new List<PhotonPlayer>();
  }

  public void UpdateTeams()
  {
    foreach (object key in Enum.GetValues(typeof (PunTeams.Team)))
      PunTeams.PlayersPerTeam[(PunTeams.Team) key].Clear();
    for (int index = 0; index < PhotonNetwork.playerList.Length; ++index)
    {
      PhotonPlayer player = PhotonNetwork.playerList[index];
      PunTeams.Team team = player.GetTeam();
      PunTeams.PlayersPerTeam[team].Add(player);
    }
  }

  public enum Team : byte
  {
    none,
    red,
    blue,
  }
}
