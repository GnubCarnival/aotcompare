// Decompiled with JetBrains decompiler
// Type: GameProgress.AchievmentContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace GameProgress
{
  internal class AchievmentContainer : BaseSettingsContainer
  {
    public ListSetting<AchievmentItem> AchievmentItems = new ListSetting<AchievmentItem>();

    public AchievmentCount GetAchievmentCount()
    {
      AchievmentCount achievmentCount = new AchievmentCount();
      foreach (AchievmentItem achievmentItem in this.AchievmentItems.Value)
      {
        if (achievmentItem.Tier.Value == "Bronze")
        {
          ++achievmentCount.TotalBronze;
          if (achievmentItem.Finished())
            ++achievmentCount.FinishedBronze;
        }
        else if (achievmentItem.Tier.Value == "Silver")
        {
          ++achievmentCount.TotalSilver;
          if (achievmentItem.Finished())
            ++achievmentCount.FinishedSilver;
        }
        else if (achievmentItem.Tier.Value == "Gold")
        {
          ++achievmentCount.TotalGold;
          if (achievmentItem.Finished())
            ++achievmentCount.FinishedGold;
        }
      }
      achievmentCount.TotalAll = achievmentCount.TotalBronze + achievmentCount.TotalSilver + achievmentCount.TotalGold;
      achievmentCount.FinishedAll = achievmentCount.FinishedBronze + achievmentCount.FinishedSilver + achievmentCount.FinishedGold;
      return achievmentCount;
    }
  }
}
