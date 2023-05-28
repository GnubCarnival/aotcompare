// Decompiled with JetBrains decompiler
// Type: GameProgress.AchievmentHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameProgress
{
  internal class AchievmentHandler : QuestHandler
  {
    private AchievmentContainer _achievment;

    public AchievmentHandler(AchievmentContainer achievment)
      : base((QuestContainer) null)
    {
      this._achievment = achievment;
      this.ReloadAchievments();
    }

    public void ReloadAchievments()
    {
      this.LoadAchievments();
      this.CacheActiveAchievments();
    }

    private void LoadAchievments()
    {
      ListSetting<AchievmentItem> other = new ListSetting<AchievmentItem>();
      Dictionary<string, AchievmentItem> dictionary = new Dictionary<string, AchievmentItem>();
      foreach (AchievmentItem achievmentItem in this._achievment.AchievmentItems.Value)
        dictionary.Add(achievmentItem.GetQuestName(), achievmentItem);
      AchievmentContainer achievmentContainer = new AchievmentContainer();
      achievmentContainer.DeserializeFromJsonString(((TextAsset) AssetBundleManager.MainAssetBundle.Load("AchievmentList")).text);
      foreach (AchievmentItem achievmentItem1 in achievmentContainer.AchievmentItems.Value)
      {
        if (dictionary.ContainsKey(achievmentItem1.GetQuestName()))
        {
          AchievmentItem achievmentItem2 = dictionary[achievmentItem1.GetQuestName()];
          achievmentItem1.Progress.Value = achievmentItem2.Progress.Value;
        }
        achievmentItem1.Active.Value = false;
        other.Value.Add(achievmentItem1);
      }
      this._achievment.AchievmentItems.Copy((BaseSetting) other);
    }

    private void CacheActiveAchievments()
    {
      this._activeQuests.Clear();
      Dictionary<string, List<AchievmentItem>> dictionary = new Dictionary<string, List<AchievmentItem>>();
      foreach (AchievmentItem achievmentItem in this._achievment.AchievmentItems.Value)
      {
        string key = achievmentItem.Category.Value + achievmentItem.GetConditionsHash();
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, new List<AchievmentItem>());
        dictionary[key].Add(achievmentItem);
      }
      foreach (string key in dictionary.Keys)
      {
        List<AchievmentItem> list = dictionary[key].OrderBy<AchievmentItem, string>((Func<AchievmentItem, string>) (x => x.GetQuestName())).ToList<AchievmentItem>();
        AchievmentItem achievmentItem1 = (AchievmentItem) null;
        foreach (AchievmentItem achievmentItem2 in list)
        {
          if (achievmentItem2.Progress.Value < achievmentItem2.Amount.Value)
          {
            achievmentItem1 = achievmentItem2;
            break;
          }
        }
        if (achievmentItem1 != null)
        {
          achievmentItem1.Active.Value = true;
          this.AddActiveQuest((QuestItem) achievmentItem1);
        }
      }
    }
  }
}
