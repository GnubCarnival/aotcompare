// Decompiled with JetBrains decompiler
// Type: InvDatabase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Database")]
[ExecuteInEditMode]
public class InvDatabase : MonoBehaviour
{
  public int databaseID;
  public UIAtlas iconAtlas;
  public List<InvBaseItem> items = new List<InvBaseItem>();
  private static bool mIsDirty = true;
  private static InvDatabase[] mList;

  public static InvBaseItem FindByID(int id32)
  {
    InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
    return !Object.op_Equality((Object) database, (Object) null) ? database.GetItem(id32 & (int) ushort.MaxValue) : (InvBaseItem) null;
  }

  public static InvBaseItem FindByName(string exact)
  {
    int index1 = 0;
    for (int length = InvDatabase.list.Length; index1 < length; ++index1)
    {
      InvDatabase invDatabase = InvDatabase.list[index1];
      int index2 = 0;
      for (int count = invDatabase.items.Count; index2 < count; ++index2)
      {
        InvBaseItem byName = invDatabase.items[index2];
        if (byName.name == exact)
          return byName;
      }
    }
    return (InvBaseItem) null;
  }

  public static int FindItemID(InvBaseItem item)
  {
    int index = 0;
    for (int length = InvDatabase.list.Length; index < length; ++index)
    {
      InvDatabase invDatabase = InvDatabase.list[index];
      if (invDatabase.items.Contains(item))
        return invDatabase.databaseID << 16 | item.id16;
    }
    return -1;
  }

  private static InvDatabase GetDatabase(int dbID)
  {
    int index = 0;
    for (int length = InvDatabase.list.Length; index < length; ++index)
    {
      InvDatabase database = InvDatabase.list[index];
      if (database.databaseID == dbID)
        return database;
    }
    return (InvDatabase) null;
  }

  private InvBaseItem GetItem(int id16)
  {
    int index = 0;
    for (int count = this.items.Count; index < count; ++index)
    {
      InvBaseItem invBaseItem = this.items[index];
      if (invBaseItem.id16 == id16)
        return invBaseItem;
    }
    return (InvBaseItem) null;
  }

  private void OnDisable() => InvDatabase.mIsDirty = true;

  private void OnEnable() => InvDatabase.mIsDirty = true;

  public static InvDatabase[] list
  {
    get
    {
      if (InvDatabase.mIsDirty)
      {
        InvDatabase.mIsDirty = false;
        InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
      }
      return InvDatabase.mList;
    }
  }
}
