// Decompiled with JetBrains decompiler
// Type: GameProgress.AchievmentItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace GameProgress
{
  internal class AchievmentItem : QuestItem
  {
    public StringSetting Tier = new StringSetting(string.Empty);
    public BoolSetting Active = new BoolSetting(false);
  }
}
