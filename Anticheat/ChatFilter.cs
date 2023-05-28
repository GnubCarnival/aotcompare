// Decompiled with JetBrains decompiler
// Type: Anticheat.ChatFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Anticheat
{
  internal static class ChatFilter
  {
    public static string FilterSizeTag(this string text)
    {
      MatchCollection matchCollection = Regex.Matches(text.ToLower(), "(<size=(.*?>))");
      List<KeyValuePair<int, string>> source = new List<KeyValuePair<int, string>>();
      foreach (Match match1 in matchCollection)
      {
        Match match = match1;
        if (!source.Any<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (p => p.Key == match.Index)))
          source.Add(new KeyValuePair<int, string>(match.Index, match.Value));
      }
      foreach (KeyValuePair<int, string> keyValuePair in source)
      {
        if (keyValuePair.Value.StartsWith("<size=") && keyValuePair.Value.Length > 9)
        {
          text = text.Remove(keyValuePair.Key, keyValuePair.Value.Length);
          text = text.Substring(0, keyValuePair.Key) + "<size=20>" + text.Substring(keyValuePair.Key, text.Length - keyValuePair.Key);
        }
      }
      return text;
    }
  }
}
