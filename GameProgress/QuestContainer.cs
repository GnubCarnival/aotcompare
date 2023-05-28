// Decompiled with JetBrains decompiler
// Type: GameProgress.QuestContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace GameProgress
{
  internal class QuestContainer : BaseSettingsContainer
  {
    public ListSetting<QuestItem> DailyQuestItems = new ListSetting<QuestItem>();
    public ListSetting<QuestItem> WeeklyQuestItems = new ListSetting<QuestItem>();

    public void CollectRewards()
    {
      foreach (QuestItem questItem in this.DailyQuestItems.Value)
        questItem.CollectReward();
      foreach (QuestItem questItem in this.WeeklyQuestItems.Value)
        questItem.CollectReward();
    }
  }
}
