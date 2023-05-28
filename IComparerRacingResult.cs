// Decompiled with JetBrains decompiler
// Type: IComparerRacingResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;

public class IComparerRacingResult : IComparer
{
  int IComparer.Compare(object x, object y)
  {
    float time1 = ((RacingResult) x).time;
    float time2 = ((RacingResult) y).time;
    if ((double) time1 == (double) time2 || (double) Math.Abs(time1 - time2) < 1.4012984643248171E-45)
      return 0;
    return (double) time1 < (double) time2 ? -1 : 1;
  }
}
