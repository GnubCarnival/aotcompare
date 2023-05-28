// Decompiled with JetBrains decompiler
// Type: RCEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using System.Collections.Generic;

internal class RCEvent
{
  private RCCondition condition;
  private RCAction elseAction;
  private int eventClass;
  private int eventType;
  public string foreachVariableName;
  public List<RCAction> trueActions;

  public RCEvent(
    RCCondition sentCondition,
    List<RCAction> sentTrueActions,
    int sentClass,
    int sentType)
  {
    this.condition = sentCondition;
    this.trueActions = sentTrueActions;
    this.eventClass = sentClass;
    this.eventType = sentType;
  }

  public void checkEvent()
  {
    switch (this.eventClass)
    {
      case 0:
        for (int index = 0; index < this.trueActions.Count; ++index)
          this.trueActions[index].doAction();
        break;
      case 1:
        if (!this.condition.checkCondition())
        {
          if (this.elseAction == null)
            break;
          this.elseAction.doAction();
          break;
        }
        for (int index = 0; index < this.trueActions.Count; ++index)
          this.trueActions[index].doAction();
        break;
      case 2:
        switch (this.eventType)
        {
          case 0:
            IEnumerator enumerator = FengGameManagerMKII.instance.getTitans().GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                TITAN current = (TITAN) enumerator.Current;
                if (((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) this.foreachVariableName))
                  FengGameManagerMKII.titanVariables[(object) this.foreachVariableName] = (object) current;
                else
                  ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) this.foreachVariableName, (object) current);
                foreach (RCAction trueAction in this.trueActions)
                  trueAction.doAction();
              }
              return;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          case 1:
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
              if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) this.foreachVariableName))
                FengGameManagerMKII.playerVariables[(object) this.foreachVariableName] = (object) player;
              else
                ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) this.foreachVariableName, (object) player);
              foreach (RCAction trueAction in this.trueActions)
                trueAction.doAction();
            }
            return;
          default:
            return;
        }
      case 3:
        while (this.condition.checkCondition())
        {
          foreach (RCAction trueAction in this.trueActions)
            trueAction.doAction();
        }
        break;
    }
  }

  public void setElse(RCAction sentElse) => this.elseAction = sentElse;

  public enum foreachType
  {
    titan,
    player,
  }

  public enum loopType
  {
    noLoop,
    ifLoop,
    foreachLoop,
    whileLoop,
  }
}
