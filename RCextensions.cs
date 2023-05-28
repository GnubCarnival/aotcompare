// Decompiled with JetBrains decompiler
// Type: RCextensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

internal static class RCextensions
{
  private static readonly string HexPattern = "(\\[)[\\w]{6}(\\])";
  private static readonly Regex HexRegex = new Regex(RCextensions.HexPattern);

  public static void Add<T>(ref T[] source, T value)
  {
    T[] objArray = new T[source.Length + 1];
    for (int index = 0; index < source.Length; ++index)
      objArray[index] = source[index];
    objArray[objArray.Length - 1] = value;
    source = objArray;
  }

  public static bool IsNullOrEmpty(this string value) => value == null || value.Length == 0;

  public static string UpperFirstLetter(this string text)
  {
    if (text == string.Empty)
      return text;
    return text.Length > 1 ? char.ToUpper(text[0]).ToString() + text.Substring(1) : text.ToUpper();
  }

  public static string StripHex(this string text) => RCextensions.HexRegex.Replace(text, "");

  public static string hexColor(this string text)
  {
    if (text.Contains("]"))
      text = text.Replace("]", ">");
    bool flag = false;
    while (text.Contains("[") && !flag)
    {
      int startIndex1 = text.IndexOf("[");
      if (text.Length >= startIndex1 + 7)
      {
        string str = text.Substring(startIndex1 + 1, 6);
        text = text.Remove(startIndex1, 7).Insert(startIndex1, "<color=#" + str);
        int startIndex2 = text.Length;
        if (text.Contains("["))
          startIndex2 = text.IndexOf("[");
        text = text.Insert(startIndex2, "</color>");
      }
      else
        flag = true;
    }
    return flag ? string.Empty : text;
  }

  public static bool isLowestID(this PhotonPlayer player)
  {
    foreach (PhotonPlayer player1 in PhotonNetwork.playerList)
    {
      if (player1.ID < player.ID)
        return false;
    }
    return true;
  }

  public static void RemoveAt<T>(ref T[] source, int index)
  {
    if (source.Length == 1)
    {
      source = new T[0];
    }
    else
    {
      if (source.Length <= 1)
        return;
      T[] objArray = new T[source.Length - 1];
      int index1 = 0;
      int index2 = 0;
      for (; index1 < source.Length; ++index1)
      {
        if (index1 != index)
        {
          objArray[index2] = source[index1];
          ++index2;
        }
      }
      source = objArray;
    }
  }

  public static bool returnBoolFromObject(object obj) => obj != null && obj is bool flag && flag;

  public static float returnFloatFromObject(object obj) => obj != null && obj is float num ? num : 0.0f;

  public static int returnIntFromObject(object obj) => obj != null && obj is int num ? num : 0;

  public static string returnStringFromObject(object obj) => obj != null && obj is string str ? str : string.Empty;

  public static T ToEnum<T>(this string value, bool ignoreCase = true) => Enum.IsDefined(typeof (T), (object) value) ? (T) Enum.Parse(typeof (T), value, ignoreCase) : default (T);

  public static string[] EnumToStringArray<T>() => Enum.GetNames(typeof (T));

  public static string[] EnumToStringArrayExceptNone<T>()
  {
    List<string> stringList = new List<string>();
    foreach (string str in RCextensions.EnumToStringArray<T>())
    {
      if (str != "None")
        stringList.Add(str);
    }
    return stringList.ToArray();
  }

  public static List<T> EnumToList<T>() => Enum.GetValues(typeof (T)).Cast<T>().ToList<T>();

  public static Dictionary<string, T> EnumToDict<T>()
  {
    Dictionary<string, T> dict = new Dictionary<string, T>();
    foreach (T obj in RCextensions.EnumToList<T>())
      dict.Add(obj.ToString(), obj);
    return dict;
  }

  public static float ParseFloat(string str) => float.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);

  public static bool IsGray(this Color color) => (double) color.r == (double) color.g && (double) color.r == (double) color.b && (double) color.a == 1.0;

  public static HERO GetMyHero()
  {
    foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
    {
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || player.photonView.isMine)
        return player;
    }
    return (HERO) null;
  }
}
