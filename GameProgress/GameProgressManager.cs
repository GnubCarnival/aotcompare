// Decompiled with JetBrains decompiler
// Type: GameProgress.GameProgressManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GameProgress
{
  internal class GameProgressManager : MonoBehaviour
  {
    private static GameProgressManager _instance;
    public static GameProgressContainer GameProgress;
    private static GameStatHandler _gameStatHandler;
    private static AchievmentHandler _achievmentHandler;
    private static QuestHandler _questHandler;
    private static List<BaseGameProgressHandler> _handlers = new List<BaseGameProgressHandler>();

    public static void Init()
    {
      GameProgressManager._instance = SingletonFactory.CreateSingleton<GameProgressManager>(GameProgressManager._instance);
      GameProgressManager.GameProgress = new GameProgressContainer();
      GameProgressManager._instance.StartCoroutine(GameProgressManager._instance.IncrementPlayTime());
    }

    public static void FinishLoadAssets()
    {
      GameProgressManager._gameStatHandler = new GameStatHandler(GameProgressManager.GameProgress.GameStat);
      GameProgressManager._achievmentHandler = new AchievmentHandler(GameProgressManager.GameProgress.Achievment);
      GameProgressManager._questHandler = new QuestHandler(GameProgressManager.GameProgress.Quest);
      GameProgressManager._handlers.Add((BaseGameProgressHandler) GameProgressManager._gameStatHandler);
      GameProgressManager._handlers.Add((BaseGameProgressHandler) GameProgressManager._achievmentHandler);
      GameProgressManager._handlers.Add((BaseGameProgressHandler) GameProgressManager._questHandler);
    }

    private void OnApplicationQuit() => GameProgressManager.Save();

    public static void OnMainMenu()
    {
      GameProgressManager.Save();
      GameProgressManager._achievmentHandler.ReloadAchievments();
      GameProgressManager._questHandler.ReloadQuests();
    }

    private static void Save() => GameProgressManager.GameProgress.Save();

    public static int GetExpToNext() => GameProgressManager._gameStatHandler.GetExpToNext();

    public static void AddExp(int exp) => GameProgressManager._gameStatHandler.AddExp(exp);

    public static void RegisterTitanKill(GameObject character, TITAN victim, KillWeapon weapon)
    {
      foreach (BaseGameProgressHandler handler in GameProgressManager._handlers)
        handler.RegisterTitanKill(character, victim, weapon);
    }

    public static void RegisterHumanKill(GameObject character, HERO victim, KillWeapon weapon)
    {
      foreach (BaseGameProgressHandler handler in GameProgressManager._handlers)
        handler.RegisterHumanKill(character, victim, weapon);
    }

    public static void RegisterDamage(
      GameObject character,
      GameObject victim,
      KillWeapon weapon,
      int damage)
    {
      if (FengGameManagerMKII.level.StartsWith("Custom"))
        return;
      TITAN component = victim.GetComponent<TITAN>();
      if (Object.op_Inequality((Object) component, (Object) null) && (double) component.myLevel > 3.0)
        return;
      foreach (BaseGameProgressHandler handler in GameProgressManager._handlers)
        handler.RegisterDamage(character, victim, weapon, damage);
    }

    public static void RegisterSpeed(GameObject character, float speed)
    {
      if (FengGameManagerMKII.level.StartsWith("Custom"))
        return;
      foreach (BaseGameProgressHandler handler in GameProgressManager._handlers)
        handler.RegisterSpeed(character, speed);
    }

    public static void RegisterInteraction(
      GameObject character,
      GameObject interact,
      InteractionType interactionType)
    {
      foreach (BaseGameProgressHandler handler in GameProgressManager._handlers)
        handler.RegisterInteraction(character, interact, interactionType);
    }

    private IEnumerator IncrementPlayTime()
    {
      while (true)
      {
        yield return (object) new WaitForSeconds(10f);
        GameProgressManager.GameProgress.GameStat.PlayTime.Value += 10f;
      }
    }
  }
}
