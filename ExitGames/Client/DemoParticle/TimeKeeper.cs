// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.DemoParticle.TimeKeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;

namespace ExitGames.Client.DemoParticle
{
  public class TimeKeeper
  {
    private int lastExecutionTime = Environment.TickCount;
    private bool shouldExecute;

    public TimeKeeper(int interval)
    {
      this.IsEnabled = true;
      this.Interval = interval;
    }

    public void Reset()
    {
      this.shouldExecute = false;
      this.lastExecutionTime = Environment.TickCount;
    }

    public int Interval { get; set; }

    public bool IsEnabled { get; set; }

    public bool ShouldExecute
    {
      get
      {
        if (!this.IsEnabled)
          return false;
        return this.shouldExecute || Environment.TickCount - this.lastExecutionTime > this.Interval;
      }
      set => this.shouldExecute = value;
    }
  }
}
