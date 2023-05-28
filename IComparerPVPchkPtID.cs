// Decompiled with JetBrains decompiler
// Type: IComparerPVPchkPtID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;

public class IComparerPVPchkPtID : IComparer
{
  int IComparer.Compare(object x, object y)
  {
    float id1 = (float) ((PVPcheckPoint) x).id;
    float id2 = (float) ((PVPcheckPoint) y).id;
    if ((double) id1 == (double) id2 || (double) Math.Abs(id1 - id2) < 1.4012984643248171E-45)
      return 0;
    return (double) id1 < (double) id2 ? -1 : 1;
  }
}
