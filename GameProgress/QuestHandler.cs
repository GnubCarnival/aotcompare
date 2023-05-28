// Decompiled with JetBrains decompiler
// Type: GameProgress.QuestHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameProgress
{
  internal class QuestHandler : BaseGameProgressHandler
  {
    protected QuestContainer _quest;
    protected Dictionary<string, List<QuestItem>> _activeQuests = new Dictionary<string, List<QuestItem>>();
    private const int DailyQuestCount = 3;
    private const int WeeklyQuestCount = 3;
    protected string[] TitanKillCategories = new string[1]
    {
      "KillTitan"
    };
    protected string[] HumanKillCategories = new string[1]
    {
      "KillHuman"
    };
    protected string[] DamageCategories = new string[2]
    {
      "DealDamage",
      "HitDamage"
    };
    protected string[] SpeedCategories = new string[1]
    {
      "ReachSpeed"
    };
    protected string[] InteractionCategories = new string[2]
    {
      "ShareGas",
      "CarryPlayer"
    };
    private static Dictionary<string, KillWeapon> NameToKillWeapon = RCextensions.EnumToDict<KillWeapon>();

    public QuestHandler(QuestContainer quest)
    {
      if (quest == null)
        return;
      this._quest = quest;
      this.ReloadQuests();
    }

    public void ReloadQuests()
    {
      this.LoadQuests();
      this.CacheActiveQuests();
    }

    public static string GetTimeToQuestReset(bool daily)
    {
      TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1);
      if (daily)
      {
        int num = 24 - timeSpan.Hours;
        return string.Format("Resets in: {0} {1}", (object) num, num == 1 ? (object) "hour" : (object) "hours");
      }
      int num1 = 6 - (timeSpan.Days - 1) % 7;
      int num2 = 24 - timeSpan.Hours;
      return string.Format("Resets in: {0} {1}, {2} {3}", (object) num1, num1 == 1 ? (object) "day" : (object) "days", (object) num2, num2 == 1 ? (object) "hour" : (object) "hours");
    }

    private void LoadQuests()
    {
      int days = (DateTime.UtcNow - new DateTime(1970, 1, 1)).Days;
      int num = (days - 1) / 7;
      QuestContainer defaultQuest = new QuestContainer();
      defaultQuest.DeserializeFromJsonString(((TextAsset) AssetBundleManager.MainAssetBundle.Load("QuestList")).text);
      ListSetting<QuestItem> other1 = new ListSetting<QuestItem>();
      foreach (QuestItem questItem in this._quest.DailyQuestItems.Value)
      {
        if (questItem.DayCreated.Value == days)
          other1.Value.Add(questItem);
      }
      other1.Value.AddRange((IEnumerable<QuestItem>) this.CreateQuests(defaultQuest, days, true, 3 - other1.Value.Count));
      ListSetting<QuestItem> other2 = new ListSetting<QuestItem>();
      foreach (QuestItem questItem in this._quest.WeeklyQuestItems.Value)
      {
        if ((questItem.DayCreated.Value - 1) / 7 == num)
          other2.Value.Add(questItem);
      }
      other2.Value.AddRange((IEnumerable<QuestItem>) this.CreateQuests(defaultQuest, days, false, 3 - other2.Value.Count));
      this._quest.DailyQuestItems.Copy((BaseSetting) other1);
      this._quest.WeeklyQuestItems.Copy((BaseSetting) other2);
    }

    private List<QuestItem> CreateQuests(
      QuestContainer defaultQuest,
      int currentDay,
      bool daily,
      int count)
    {
      List<QuestItem> questItemList = daily ? defaultQuest.DailyQuestItems.Value : defaultQuest.WeeklyQuestItems.Value;
      List<QuestItem> quests = new List<QuestItem>();
      HashSet<string> stringSet = new HashSet<string>();
      for (int index1 = 0; index1 < count; ++index1)
      {
        for (int index2 = 0; index2 < 10; ++index2)
        {
          int index3 = Random.Range(0, questItemList.Count);
          if (!stringSet.Contains(questItemList[index3].GetQuestName()))
          {
            QuestItem questItem = new QuestItem();
            questItem.Copy((BaseSetting) questItemList[index3]);
            questItem.DayCreated.Value = currentDay;
            quests.Add(questItem);
            stringSet.Add(questItem.GetQuestName());
            break;
          }
        }
      }
      return quests;
    }

    private void CacheActiveQuests()
    {
      this._activeQuests.Clear();
      foreach (QuestItem questItem in this._quest.DailyQuestItems.Value)
      {
        if (questItem.Progress.Value < questItem.Amount.Value)
          this.AddActiveQuest(questItem);
      }
      foreach (QuestItem questItem in this._quest.WeeklyQuestItems.Value)
      {
        if (questItem.Progress.Value < questItem.Amount.Value)
          this.AddActiveQuest(questItem);
      }
    }

    protected void AddActiveQuest(QuestItem item)
    {
      string key = item.Category.Value;
      if (!this._activeQuests.ContainsKey(key))
        this._activeQuests.Add(key, new List<QuestItem>());
      this._activeQuests[key].Add(item);
    }

    protected virtual bool CheckKillConditions(List<StringSetting> conditions, KillWeapon weapon)
    {
      foreach (TypedSetting<string> condition in conditions)
      {
        string[] strArray = condition.Value.Split(':');
        string str = strArray[0];
        string key = strArray[1];
        if (str == "Weapon" && QuestHandler.NameToKillWeapon[key] != weapon)
          return false;
      }
      return true;
    }

    protected virtual bool CheckDamageConditions(
      List<StringSetting> conditions,
      KillWeapon weapon,
      int damage)
    {
      foreach (TypedSetting<string> condition in conditions)
      {
        string[] strArray = condition.Value.Split(':');
        string str1 = strArray[0];
        string str2 = strArray[1];
        if (str1 == "Weapon" && QuestHandler.NameToKillWeapon[str2] != weapon || str1 == "Damage" && damage < int.Parse(str2))
          return false;
      }
      return true;
    }

    protected virtual bool CheckSpeedConditions(
      List<StringSetting> conditions,
      GameObject character,
      float speed)
    {
      foreach (TypedSetting<string> condition in conditions)
      {
        string[] strArray = condition.Value.Split(':');
        string str = strArray[0];
        string s = strArray[1];
        if (str == "Speed" && (double) speed < (double) int.Parse(s))
          return false;
      }
      return true;
    }

    public override void RegisterTitanKill(GameObject character, TITAN victim, KillWeapon weapon)
    {
      foreach (string titanKillCategory in this.TitanKillCategories)
      {
        if (this._activeQuests.ContainsKey(titanKillCategory))
        {
          foreach (QuestItem questItem in this._activeQuests[titanKillCategory])
          {
            if (this.CheckKillConditions(questItem.Conditions.Value, weapon) && titanKillCategory == "KillTitan")
              questItem.AddProgress();
          }
        }
      }
    }

    public override void RegisterHumanKill(GameObject character, HERO victim, KillWeapon weapon)
    {
      foreach (string humanKillCategory in this.HumanKillCategories)
      {
        if (this._activeQuests.ContainsKey(humanKillCategory))
        {
          foreach (QuestItem questItem in this._activeQuests[humanKillCategory])
          {
            if (this.CheckKillConditions(questItem.Conditions.Value, weapon) && humanKillCategory == "KillHuman")
              questItem.AddProgress();
          }
        }
      }
    }

    public override void RegisterDamage(
      GameObject character,
      GameObject victim,
      KillWeapon weapon,
      int damage)
    {
      foreach (string damageCategory in this.DamageCategories)
      {
        if (this._activeQuests.ContainsKey(damageCategory))
        {
          foreach (QuestItem questItem in this._activeQuests[damageCategory])
          {
            if (this.CheckDamageConditions(questItem.Conditions.Value, weapon, damage))
            {
              switch (damageCategory)
              {
                case "HitDamage":
                  questItem.AddProgress();
                  continue;
                case "DealDamage":
                  questItem.AddProgress(damage);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }

    public override void RegisterSpeed(GameObject character, float speed)
    {
      foreach (string speedCategory in this.SpeedCategories)
      {
        if (this._activeQuests.ContainsKey(speedCategory))
        {
          foreach (QuestItem questItem in this._activeQuests[speedCategory])
          {
            if (this.CheckSpeedConditions(questItem.Conditions.Value, character, speed) && speedCategory == "ReachSpeed")
              questItem.AddProgress();
          }
        }
      }
    }

    public override void RegisterInteraction(
      GameObject character,
      GameObject interact,
      InteractionType interactionType)
    {
      foreach (string interactionCategory in this.InteractionCategories)
      {
        if (this._activeQuests.ContainsKey(interactionCategory))
        {
          foreach (QuestItem questItem in this._activeQuests[interactionCategory])
          {
            switch (interactionCategory)
            {
              case "ShareGas":
                questItem.AddProgress();
                continue;
              case "CarryHuman":
                questItem.AddProgress();
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
  }
}
