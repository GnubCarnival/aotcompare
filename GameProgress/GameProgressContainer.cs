﻿// Decompiled with JetBrains decompiler
// Type: GameProgress.GameProgressContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

namespace GameProgress
{
  internal class GameProgressContainer : SaveableSettingsContainer
  {
    public AchievmentContainer Achievment = new AchievmentContainer();
    public QuestContainer Quest = new QuestContainer();
    public GameStatContainer GameStat = new GameStatContainer();

    protected override string FolderPath => Application.dataPath + "/UserData/GameProgress";

    protected override string FileName => "GameProgress";

    protected override bool Encrypted => true;

    public override void Save()
    {
      this.Quest.CollectRewards();
      base.Save();
    }
  }
}
