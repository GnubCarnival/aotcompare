// Decompiled with JetBrains decompiler
// Type: Replay.ReplayScriptEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using Utility;

namespace Replay
{
  internal class ReplayScriptEvent : BaseCSVRow
  {
    public float Time;
    public string Category;
    public string Action;
    public List<string> Parameters = new List<string>();
  }
}
