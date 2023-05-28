// Decompiled with JetBrains decompiler
// Type: Utility.RateLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Utility
{
  internal class RateLimit
  {
    private int _currentUsage;
    private int _maxUsage;
    private float _resetDelay;
    private float _lastResetTime;

    public RateLimit(int maxUsage, float resetDelay)
    {
      this._currentUsage = 0;
      this._lastResetTime = Time.time;
      this._maxUsage = maxUsage;
      this._resetDelay = resetDelay;
    }

    public bool Check(int usage = 1)
    {
      this.TryReset();
      if (this._currentUsage + usage > this._maxUsage)
        return false;
      this._currentUsage += usage;
      return true;
    }

    private void TryReset()
    {
      if ((double) Time.time < (double) this._lastResetTime + (double) this._resetDelay)
        return;
      this._currentUsage = 0;
      this._lastResetTime = Time.time;
    }
  }
}
