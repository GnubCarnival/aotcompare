// Decompiled with JetBrains decompiler
// Type: ScoreExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ExitGames.Client.Photon;
using System.Collections.Generic;

internal static class ScoreExtensions
{
  public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
  {
    int num = player.GetScore() + scoreToAddToCurrent;
    player.SetCustomProperties(new Hashtable()
    {
      [(object) "score"] = (object) num
    });
  }

  public static int GetScore(this PhotonPlayer player)
  {
    object obj;
    return ((Dictionary<object, object>) player.customProperties).TryGetValue((object) "score", out obj) ? (int) obj : 0;
  }

  public static void SetScore(this PhotonPlayer player, int newScore) => player.SetCustomProperties(new Hashtable()
  {
    [(object) "score"] = (object) newScore
  });
}
