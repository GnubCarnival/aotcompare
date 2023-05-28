﻿// Decompiled with JetBrains decompiler
// Type: BombUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class BombUtil
{
  public static Color GetBombColor(PhotonPlayer player, float minAlpha = 0.5f)
  {
    if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
    {
      switch (RCextensions.returnIntFromObject(player.customProperties[(object) PhotonPlayerProperty.RCteam]))
      {
        case 1:
          return Color.cyan;
        case 2:
          return Color.magenta;
      }
    }
    return BombUtil.GetBombColorIndividual(player, minAlpha);
  }

  private static Color GetBombColorIndividual(PhotonPlayer player, float minAlpha)
  {
    double num1 = (double) RCextensions.returnFloatFromObject(player.customProperties[(object) PhotonPlayerProperty.RCBombR]);
    float num2 = RCextensions.returnFloatFromObject(player.customProperties[(object) PhotonPlayerProperty.RCBombG]);
    float num3 = RCextensions.returnFloatFromObject(player.customProperties[(object) PhotonPlayerProperty.RCBombB]);
    float num4 = RCextensions.returnFloatFromObject(player.customProperties[(object) PhotonPlayerProperty.RCBombA]);
    float num5 = Mathf.Max(minAlpha, num4);
    double num6 = (double) num2;
    double num7 = (double) num3;
    double num8 = (double) num5;
    return new Color((float) num1, (float) num6, (float) num7, (float) num8);
  }
}
