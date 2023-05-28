// Decompiled with JetBrains decompiler
// Type: UI.QuestWeeklyPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class QuestWeeklyPanel : QuestCategoryPanel
  {
    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ((Graphic) ElementFactory.CreateDefaultLabel(this.SinglePanel, new ElementStyle(themePanel: this.ThemePanel), QuestHandler.GetTimeToQuestReset(false), alignment: (TextAnchor) 3).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestHeader", "ResetTextColor");
      this.CreateQuestItems(GameProgressManager.GameProgress.Quest.WeeklyQuestItems.Value);
    }
  }
}
