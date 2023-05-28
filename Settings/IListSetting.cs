// Decompiled with JetBrains decompiler
// Type: Settings.IListSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;

namespace Settings
{
  internal interface IListSetting
  {
    int GetCount();

    BaseSetting GetItemAt(int index);

    List<BaseSetting> GetItems();

    void AddItem(BaseSetting item);

    void Clear();
  }
}
