// Decompiled with JetBrains decompiler
// Type: UI.QuestAchievmentsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class QuestAchievmentsPanel : QuestCategoryPanel
  {
    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ((LayoutGroup) ((Component) this.SinglePanel).GetComponent<VerticalLayoutGroup>()).padding = new RectOffset(10, 25, this.VerticalPadding, this.VerticalPadding);
      Transform transform = ElementFactory.InstantiateAndBind(this.SinglePanel, "AchievmentHeader").transform;
      ((Component) transform).GetComponent<LayoutElement>().preferredWidth = this.QuestItemWidth;
      ((LayoutGroup) ((Component) transform).GetComponent<HorizontalLayoutGroup>()).padding = new RectOffset(10, 10, 0, 0);
      QuestPopup questPopup = (QuestPopup) parent;
      questPopup.CreateAchievmentDropdowns(transform.Find("LeftPanel"));
      AchievmentCount achievmentCount = GameProgressManager.GameProgress.Achievment.GetAchievmentCount();
      ((Component) transform.Find("RightPanel/TrophyCountBronze/Label")).GetComponent<Text>().text = achievmentCount.FinishedBronze.ToString() + "/" + achievmentCount.TotalBronze.ToString();
      ((Component) transform.Find("RightPanel/TrophyCountSilver/Label")).GetComponent<Text>().text = achievmentCount.FinishedSilver.ToString() + "/" + achievmentCount.TotalSilver.ToString();
      ((Component) transform.Find("RightPanel/TrophyCountGold/Label")).GetComponent<Text>().text = achievmentCount.FinishedGold.ToString() + "/" + achievmentCount.TotalGold.ToString();
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountBronze/Image")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "BronzeColor");
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountSilver/Image")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "SilverColor");
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountGold/Image")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "GoldColor");
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountBronze/Label")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountSilver/Label")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
      ((Graphic) ((Component) transform.Find("RightPanel/TrophyCountGold/Label")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
      List<QuestItem> items = new List<QuestItem>();
      foreach (AchievmentItem achievmentItem in GameProgressManager.GameProgress.Achievment.AchievmentItems.Value)
      {
        if (!(questPopup.TierSelection.Value != achievmentItem.Tier.Value) && (!(questPopup.CompletedSelection.Value == "Completed") || achievmentItem.Finished()) && (!(questPopup.CompletedSelection.Value == "In Progress") || !achievmentItem.Finished()))
          items.Add((QuestItem) achievmentItem);
      }
      this.CreateQuestItems(items);
    }
  }
}
