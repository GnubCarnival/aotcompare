// Decompiled with JetBrains decompiler
// Type: InvGameItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InvGameItem
{
  public int itemLevel;
  private InvBaseItem mBaseItem;
  [SerializeField]
  private int mBaseItemID;
  public InvGameItem.Quality quality;

  public InvGameItem(int id)
  {
    this.quality = InvGameItem.Quality.Sturdy;
    this.itemLevel = 1;
    this.mBaseItemID = id;
  }

  public InvGameItem(int id, InvBaseItem bi)
  {
    this.quality = InvGameItem.Quality.Sturdy;
    this.itemLevel = 1;
    this.mBaseItemID = id;
    this.mBaseItem = bi;
  }

  public List<InvStat> CalculateStats()
  {
    List<InvStat> stats1 = new List<InvStat>();
    if (this.baseItem != null)
    {
      float statMultiplier = this.statMultiplier;
      List<InvStat> stats2 = this.baseItem.stats;
      int index1 = 0;
      for (int count1 = stats2.Count; index1 < count1; ++index1)
      {
        InvStat invStat1 = stats2[index1];
        int num = Mathf.RoundToInt(statMultiplier * (float) invStat1.amount);
        if (num != 0)
        {
          bool flag = false;
          int index2 = 0;
          for (int count2 = stats1.Count; index2 < count2; ++index2)
          {
            InvStat invStat2 = stats1[index2];
            if (invStat2.id == invStat1.id && invStat2.modifier == invStat1.modifier)
            {
              invStat2.amount += num;
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            InvStat invStat3 = new InvStat()
            {
              id = invStat1.id,
              amount = num,
              modifier = invStat1.modifier
            };
            stats1.Add(invStat3);
          }
        }
      }
      stats1.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
    }
    return stats1;
  }

  public InvBaseItem baseItem
  {
    get
    {
      if (this.mBaseItem == null)
        this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
      return this.mBaseItem;
    }
  }

  public int baseItemID => this.mBaseItemID;

  public Color color
  {
    get
    {
      Color white = Color.white;
      switch (this.quality)
      {
        case InvGameItem.Quality.Broken:
          return new Color(0.4f, 0.2f, 0.2f);
        case InvGameItem.Quality.Cursed:
          return Color.red;
        case InvGameItem.Quality.Damaged:
          return new Color(0.4f, 0.4f, 0.4f);
        case InvGameItem.Quality.Worn:
          return new Color(0.7f, 0.7f, 0.7f);
        case InvGameItem.Quality.Sturdy:
          return new Color(1f, 1f, 1f);
        case InvGameItem.Quality.Polished:
          return NGUIMath.HexToColor(3774856959U);
        case InvGameItem.Quality.Improved:
          return NGUIMath.HexToColor(2480359935U);
        case InvGameItem.Quality.Crafted:
          return NGUIMath.HexToColor(1325334783U);
        case InvGameItem.Quality.Superior:
          return NGUIMath.HexToColor(12255231U);
        case InvGameItem.Quality.Enchanted:
          return NGUIMath.HexToColor(1937178111U);
        case InvGameItem.Quality.Epic:
          return NGUIMath.HexToColor(2516647935U);
        case InvGameItem.Quality.Legendary:
          return NGUIMath.HexToColor(4287627519U);
        default:
          return white;
      }
    }
  }

  public string name => this.baseItem == null ? (string) null : this.quality.ToString() + " " + this.baseItem.name;

  public float statMultiplier
  {
    get
    {
      float num1 = 0.0f;
      switch (this.quality)
      {
        case InvGameItem.Quality.Broken:
          num1 = 0.0f;
          break;
        case InvGameItem.Quality.Cursed:
          num1 = -1f;
          break;
        case InvGameItem.Quality.Damaged:
          num1 = 0.25f;
          break;
        case InvGameItem.Quality.Worn:
          num1 = 0.9f;
          break;
        case InvGameItem.Quality.Sturdy:
          num1 = 1f;
          break;
        case InvGameItem.Quality.Polished:
          num1 = 1.1f;
          break;
        case InvGameItem.Quality.Improved:
          num1 = 1.25f;
          break;
        case InvGameItem.Quality.Crafted:
          num1 = 1.5f;
          break;
        case InvGameItem.Quality.Superior:
          num1 = 1.75f;
          break;
        case InvGameItem.Quality.Enchanted:
          num1 = 2f;
          break;
        case InvGameItem.Quality.Epic:
          num1 = 2.5f;
          break;
        case InvGameItem.Quality.Legendary:
          num1 = 3f;
          break;
      }
      float num2 = (float) this.itemLevel / 50f;
      return num1 * Mathf.Lerp(num2, num2 * num2, 0.5f);
    }
  }

  public enum Quality
  {
    Broken,
    Cursed,
    Damaged,
    Worn,
    Sturdy,
    Polished,
    Improved,
    Crafted,
    Superior,
    Enchanted,
    Epic,
    Legendary,
    _LastDoNotUse,
  }
}
