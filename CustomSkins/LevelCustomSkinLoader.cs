﻿// Decompiled with JetBrains decompiler
// Type: CustomSkins.LevelCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;

namespace CustomSkins
{
  internal abstract class LevelCustomSkinLoader : BaseCustomSkinLoader
  {
    protected virtual BaseCustomSkinPart GetCustomSkinPart(int partId, int randomIndex) => throw new NotImplementedException();

    protected virtual void FindAndIndexLevelObjects() => throw new NotImplementedException();
  }
}
