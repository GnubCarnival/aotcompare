// Decompiled with JetBrains decompiler
// Type: UI.QuestDailyPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class QuestDailyPanel : QuestCategoryPanel
  {
    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ((Graphic) ElementFactory.CreateDefaultLabel(this.SinglePanel, new ElementStyle(themePanel: this.ThemePanel), QuestHandler.GetTimeToQuestReset(true), alignment: (TextAnchor) 3).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestHeader", "ResetTextColor");
      this.CreateQuestItems(GameProgressManager.GameProgress.Quest.DailyQuestItems.Value);
    }
  }
}
