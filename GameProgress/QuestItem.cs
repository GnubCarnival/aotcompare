// Decompiled with JetBrains decompiler
// Type: GameProgress.QuestItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;

namespace GameProgress
{
  internal class QuestItem : BaseSettingsContainer
  {
    public StringSetting Category = new StringSetting(string.Empty);
    public ListSetting<StringSetting> Conditions = new ListSetting<StringSetting>();
    public IntSetting Amount = new IntSetting(0);
    public StringSetting RewardType = new StringSetting(string.Empty);
    public StringSetting RewardValue = new StringSetting(string.Empty);
    public StringSetting Icon = new StringSetting(string.Empty);
    public IntSetting Progress = new IntSetting(0);
    public BoolSetting Daily = new BoolSetting(true);
    public IntSetting DayCreated = new IntSetting(0);
    public BoolSetting Collected = new BoolSetting(false);

    public string GetQuestName() => this.Category.Value + this.GetConditionsHash() + this.Amount.Value.ToString();

    public string GetConditionsHash()
    {
      string conditionsHash = "";
      foreach (StringSetting stringSetting in this.Conditions.Value)
        conditionsHash += stringSetting.Value;
      return conditionsHash;
    }

    public bool Finished() => this.Progress.Value >= this.Amount.Value;

    public void AddProgress(int count = 1)
    {
      this.Progress.Value += count;
      this.Progress.Value = Math.Min(this.Progress.Value, this.Amount.Value);
    }

    public void CollectReward()
    {
      if (this.Collected.Value || this.Progress.Value < this.Amount.Value)
        return;
      this.Collected.Value = true;
      if (!(this.RewardType.Value == "Exp"))
        return;
      GameProgressManager.AddExp(int.Parse(this.RewardValue.Value));
    }
  }
}
