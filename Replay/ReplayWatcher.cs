// Decompiled with JetBrains decompiler
// Type: Replay.ReplayWatcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Replay
{
  internal class ReplayWatcher : MonoBehaviour
  {
    public bool Playing;
    public float Speed;
    public float CurrentTime;
    public float MaxTime;
    private ReplayScript _script;
    private int _currentEvent;

    public void LoadScript(ReplayScript script)
    {
      this._script = script;
      this._currentEvent = 0;
      this.Playing = false;
      this.CurrentTime = script.Events[0].Time;
      this.MaxTime = script.Events[script.Events.Count - 1].Time;
      this.HandleEvent(script.Events[0]);
    }

    private void FixedUpdate()
    {
      if (!this.Playing)
        return;
      this.CurrentTime += Time.fixedDeltaTime * this.Speed;
      while (this._currentEvent < this._script.Events.Count - 1)
      {
        ReplayScriptEvent currentEvent = this._script.Events[this._currentEvent + 1];
        if ((double) this.CurrentTime >= (double) currentEvent.Time)
        {
          ++this._currentEvent;
          this.HandleEvent(currentEvent);
        }
        else
          break;
      }
      if ((double) this.CurrentTime < (double) this.MaxTime)
        return;
      this.CurrentTime = this.MaxTime;
      this.Playing = false;
    }

    private void HandleEvent(ReplayScriptEvent currentEvent)
    {
      if (currentEvent.Category == ReplayEventCategory.Map.ToString())
        this.HandleMapEvent(currentEvent);
      else if (currentEvent.Category == ReplayEventCategory.Human.ToString())
        this.HandleHumanEvent(currentEvent);
      else if (currentEvent.Category == ReplayEventCategory.Titan.ToString())
        this.HandleTitanEvent(currentEvent);
      else if (currentEvent.Category == ReplayEventCategory.Camera.ToString())
      {
        this.HandleCameraEvent(currentEvent);
      }
      else
      {
        if (!(currentEvent.Category == ReplayEventCategory.Chat.ToString()))
          return;
        this.HandleChatEvent(currentEvent);
      }
    }

    private void HandleMapEvent(ReplayScriptEvent currentEvent)
    {
      if (!(currentEvent.Action == ReplayEventMapAction.SetMap.ToString()))
        return;
      string parameter = currentEvent.Parameters[0];
    }

    private void HandleHumanEvent(ReplayScriptEvent currentEvent)
    {
    }

    private void HandleTitanEvent(ReplayScriptEvent currentEvent)
    {
    }

    private void HandleCameraEvent(ReplayScriptEvent currentEvent)
    {
    }

    private void HandleChatEvent(ReplayScriptEvent currentEvent)
    {
    }
  }
}
