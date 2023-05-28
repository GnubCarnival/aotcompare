// Decompiled with JetBrains decompiler
// Type: UI.QuestCategoryPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using GameProgress;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class QuestCategoryPanel : BasePanel
  {
    protected float QuestItemWidth = 940f;
    protected float QuestItemHeight = 100f;

    protected override string ThemePanel => "QuestPopup";

    protected override float Width => 980f;

    protected override float Height => 600f;

    protected override float VerticalSpacing => 20f;

    protected override int HorizontalPadding => 20;

    protected override int VerticalPadding => 20;

    protected override TextAnchor PanelAlignment => (TextAnchor) 1;

    public override void Setup(BasePanel parent = null) => base.Setup(parent);

    protected void CreateQuestItems(List<QuestItem> items)
    {
      foreach (QuestItem questItem in items)
      {
        Transform transform = ElementFactory.InstantiateAndBind(this.SinglePanel, "QuestItemPanel").transform;
        ((Component) transform).GetComponent<LayoutElement>().preferredWidth = this.QuestItemWidth;
        ((Component) transform).GetComponent<LayoutElement>().preferredHeight = this.QuestItemHeight;
        ((Component) transform.Find("Panel/Icon")).GetComponent<RawImage>().texture = (Texture) AssetBundleManager.LoadAsset(questItem.Icon.Value + "Icon", true);
        this.SetTitle(questItem, transform);
        this.SetRewardLabel(questItem, transform);
        this.SetProgress(questItem, transform);
        ((Graphic) ((Component) transform.Find("Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "BackgroundColor");
        ((Component) transform.Find("Panel/CheckIcon")).gameObject.SetActive(questItem.Finished());
        ((Graphic) ((Component) transform.Find("Border")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "BorderColor");
        ((Graphic) ((Component) transform.Find("Panel/Icon")).GetComponent<RawImage>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "IconColor");
        ((Graphic) ((Component) transform.Find("Panel/CheckIcon")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "CheckColor");
        ((Graphic) ((Component) transform.Find("Panel/Title")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "TextColor");
        ((Graphic) ((Component) transform.Find("Panel/ProgressLabel")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "TextColor");
        ((Graphic) ((Component) transform.Find("Panel/RewardLabel")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "TextColor");
        ((Graphic) ((Component) transform.Find("Panel/CheckIcon")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "IconColor");
        ((Graphic) ((Component) transform.Find("Panel/ProgressBar/Background")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "ProgressBarBackgroundColor");
        ((Graphic) ((Component) transform.Find("Panel/ProgressBar/Fill Area/Fill")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "QuestItem", "ProgressBarFillColor");
      }
    }

    protected void SetRewardLabel(QuestItem item, Transform panel)
    {
      if (item is AchievmentItem)
      {
        ((Component) panel.Find("Panel/RewardLabel")).gameObject.SetActive(false);
        ((Component) panel.Find("Panel/AchievmentIcon")).gameObject.SetActive(true);
        ((Graphic) ((Component) panel.Find("Panel/AchievmentIcon")).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", ((AchievmentItem) item).Tier.Value + "Color");
      }
      else
      {
        ((Component) panel.Find("Panel/RewardLabel")).gameObject.SetActive(true);
        ((Component) panel.Find("Panel/AchievmentIcon")).gameObject.SetActive(false);
        if (!(item.RewardType.Value == "Exp"))
          return;
        ((Component) panel.Find("Panel/RewardLabel")).GetComponent<Text>().text = "+" + item.RewardValue.Value + " exp";
      }
    }

    protected void SetTitle(QuestItem item, Transform panel)
    {
      string locale = UIManager.GetLocale("QuestItems", item.Category.Value);
      Dictionary<string, string> conditionToValue = new Dictionary<string, string>();
      foreach (TypedSetting<string> typedSetting in item.Conditions.Value)
      {
        string[] strArray = typedSetting.Value.Split(':');
        conditionToValue.Add(strArray[0], strArray[1]);
      }
      string str1 = "";
      for (int index1 = 0; index1 < locale.Length; ++index1)
      {
        if (locale[index1] == '{')
        {
          str1 += this.HandleConditionVariable(locale, index1, conditionToValue);
          index1 = locale.IndexOf('}', index1);
        }
        else if (locale[index1] == '[')
        {
          int num1 = locale.IndexOf(']', index1);
          int index2 = locale.IndexOf('{', index1);
          int num2 = locale.IndexOf('}', index1);
          string str2 = this.HandleConditionVariable(locale, index2, conditionToValue);
          if (str2 != string.Empty)
            str1 = str1 + locale.Substring(index1 + 1, index2 - index1 - 1) + str2 + locale.Substring(num2 + 1, num1 - num2 - 1);
          index1 = num1;
        }
        else
          str1 += locale[index1].ToString();
      }
      ((Component) panel.Find("Panel/Title")).GetComponent<Text>().text = str1;
    }

    private string HandleConditionVariable(
      string locale,
      int index,
      Dictionary<string, string> conditionToValue)
    {
      int num = locale.IndexOf('}', index);
      string key = locale.Substring(index + 1, num - index - 1);
      if (!conditionToValue.ContainsKey(key))
        return string.Empty;
      string locale1 = UIManager.GetLocale("QuestItems", key + "." + conditionToValue[key], defaultValue: "Error");
      return locale1 == "Error" ? conditionToValue[key] : locale1;
    }

    protected void SetProgress(QuestItem item, Transform panel)
    {
      ((Component) panel.Find("Panel/ProgressBar")).GetComponent<Slider>().value = (float) item.Progress.Value / (float) item.Amount.Value;
      Text component = ((Component) panel.Find("Panel/ProgressLabel")).GetComponent<Text>();
      int num = item.Progress.Value;
      string str1 = num.ToString();
      num = item.Amount.Value;
      string str2 = num.ToString();
      string str3 = str1 + " / " + str2;
      component.text = str3;
    }
  }
}
