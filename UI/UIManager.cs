// Decompiled with JetBrains decompiler
// Type: UI.UIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
  internal class UIManager : MonoBehaviour
  {
    private static readonly string LanguageFolderPath = Application.dataPath + "/Resources/Languages";
    private static readonly string UIThemeFolderPath = Application.dataPath + "/Resources/UIThemes";
    private static Dictionary<string, JSONObject> _languages = new Dictionary<string, JSONObject>();
    private static Dictionary<string, JSONObject> _uiThemes = new Dictionary<string, JSONObject>();
    private static Dictionary<System.Type, string> _lastCategories = new Dictionary<System.Type, string>();
    private static string _currentUITheme;
    private static UIManager _instance;
    public static BaseMenu CurrentMenu;
    public static float CurrentCanvasScale = 1f;

    public static void Init()
    {
      UIManager._instance = SingletonFactory.CreateSingleton<UIManager>(UIManager._instance);
      UIManager.LoadLanguages();
      UIManager.LoadUIThemes();
    }

    public static void FinishLoadAssets()
    {
      UIManager.LoadEmojis();
      UIManager.SetMenu(MenuType.Main);
    }

    public static void SetLastCategory(System.Type t, string category)
    {
      if (UIManager._lastCategories.ContainsKey(t))
        UIManager._lastCategories[t] = category;
      else
        UIManager._lastCategories.Add(t, category);
    }

    public static string GetLastcategory(System.Type t) => UIManager._lastCategories.ContainsKey(t) ? UIManager._lastCategories[t] : string.Empty;

    private static void LoadEmojis()
    {
      foreach (string availableEmoji in GameMenu.AvailableEmojis)
        GameMenu.EmojiTextures.Add(availableEmoji, AssetBundleManager.MainAssetBundle.Load("Emoji" + availableEmoji) as Texture2D);
    }

    public static void SetMenu(MenuType menu)
    {
      UIManager._currentUITheme = SettingsManager.UISettings.UITheme.Value;
      if (Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null))
        Object.Destroy((Object) UIManager.CurrentMenu);
      if (menu != MenuType.Main)
      {
        if (menu != MenuType.Game)
          return;
        UIManager.CurrentMenu = (BaseMenu) ElementFactory.CreateDefaultMenu<GameMenu>().GetComponent<GameMenu>();
      }
      else
      {
        UIManager._lastCategories.Clear();
        UIManager.CurrentMenu = ElementFactory.CreateDefaultMenu<MainMenu>().GetComponent<BaseMenu>();
      }
    }

    public static string GetLocale(
      string category,
      string subCategory,
      string item = "",
      string forcedLanguage = "",
      string defaultValue = "")
    {
      JSONObject jsonObject = (JSONObject) null;
      string key = forcedLanguage != string.Empty ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value;
      if (UIManager._languages.ContainsKey(key))
        jsonObject = UIManager._languages[key];
      string aKey = subCategory;
      if (item != string.Empty)
        aKey = aKey + "." + item;
      if (!((JSONNode) jsonObject == (object) null) && !(jsonObject[category] == (object) null) && !(jsonObject[category][aKey] == (object) null))
        return jsonObject[category][aKey].Value;
      if (!(key == "English"))
        return UIManager.GetLocale(category, subCategory, item, "English");
      return defaultValue != string.Empty ? defaultValue : string.Format("{0} locale error.", (object) aKey);
    }

    public static string[] GetLocaleArray(
      string category,
      string subCategory,
      string item = "",
      string forcedLanguage = "")
    {
      JSONObject jsonObject = (JSONObject) null;
      string key = forcedLanguage != string.Empty ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value;
      if (UIManager._languages.ContainsKey(key))
        jsonObject = UIManager._languages[key];
      string aKey = subCategory;
      if (item != string.Empty)
        aKey = aKey + "." + item;
      if ((JSONNode) jsonObject == (object) null || jsonObject[category] == (object) null || jsonObject[category][aKey] == (object) null)
      {
        if (!(key == "English"))
          return UIManager.GetLocaleArray(category, subCategory, item, "English");
        return new string[1]
        {
          string.Format("{0} locale error.", (object) aKey)
        };
      }
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonObject[category][aKey])
      {
        JSONString jsonString = (JSONString) (JSONNode) keyValuePair;
        stringList.Add(jsonString.Value);
      }
      return stringList.ToArray();
    }

    public static string GetLocaleCommon(string item) => UIManager.GetLocale("Common", item);

    public static string[] GetLocaleCommonArray(string item) => UIManager.GetLocaleArray("Common", item);

    public static string[] GetLanguages()
    {
      List<string> stringList = new List<string>();
      foreach (string key in UIManager._languages.Keys)
      {
        if (key == "English")
          stringList.Insert(0, key);
        else
          stringList.Add(key);
      }
      return stringList.ToArray();
    }

    private static void LoadLanguages()
    {
      if (!Directory.Exists(UIManager.LanguageFolderPath))
      {
        Directory.CreateDirectory(UIManager.LanguageFolderPath);
        Debug.Log((object) "No language folder found, creating it.");
      }
      else
      {
        foreach (string file in Directory.GetFiles(UIManager.LanguageFolderPath, "*.json"))
        {
          JSONObject jsonObject = (JSONObject) JSON.Parse(File.ReadAllText(file));
          if (!UIManager._languages.ContainsKey((string) jsonObject["Name"]))
            UIManager._languages.Add(jsonObject["Name"].Value, jsonObject);
        }
        if (UIManager._languages.ContainsKey(SettingsManager.GeneralSettings.Language.Value))
          return;
        SettingsManager.GeneralSettings.Language.Value = "English";
        SettingsManager.GeneralSettings.Save();
      }
    }

    public static Color GetThemeColor(
      string panel,
      string category,
      string item,
      string fallbackPanel = "DefaultPanel")
    {
      JSONObject jsonObject = (JSONObject) null;
      if (UIManager._uiThemes.ContainsKey(UIManager._currentUITheme))
        jsonObject = UIManager._uiThemes[UIManager._currentUITheme];
      if (!((JSONNode) jsonObject == (object) null) && !(jsonObject[panel] == (object) null) && !(jsonObject[panel][category] == (object) null))
      {
        if (!(jsonObject[panel][category][item] == (object) null))
        {
          try
          {
            List<float> floatList = new List<float>();
            JSONNode.Enumerator enumerator = jsonObject[panel][category][item].GetEnumerator();
            while (enumerator.MoveNext())
            {
              JSONNumber current = (JSONNumber) (JSONNode) enumerator.Current;
              floatList.Add(float.Parse(current.Value) / (float) byte.MaxValue);
            }
            return new Color(floatList[0], floatList[1], floatList[2], floatList[3]);
          }
          catch
          {
            Debug.Log((object) string.Format("{0} {1} {2} theme error.", (object) panel, (object) category, (object) item));
            return Color.white;
          }
        }
      }
      if (panel != fallbackPanel)
        return UIManager.GetThemeColor(fallbackPanel, category, item, fallbackPanel);
      Debug.Log((object) string.Format("{0} {1} {2} theme error.", (object) panel, (object) category, (object) item));
      return Color.white;
    }

    public static ColorBlock GetThemeColorBlock(
      string panel,
      string category,
      string item,
      string fallbackPanel = "DefaultPanel")
    {
      Color themeColor1 = UIManager.GetThemeColor(panel, category, item + "NormalColor", fallbackPanel);
      Color themeColor2 = UIManager.GetThemeColor(panel, category, item + "HighlightedColor", fallbackPanel);
      Color themeColor3 = UIManager.GetThemeColor(panel, category, item + "PressedColor", fallbackPanel);
      ColorBlock themeColorBlock = new ColorBlock();
      ((ColorBlock) ref themeColorBlock).colorMultiplier = 1f;
      ((ColorBlock) ref themeColorBlock).fadeDuration = 0.1f;
      ((ColorBlock) ref themeColorBlock).normalColor = themeColor1;
      ((ColorBlock) ref themeColorBlock).highlightedColor = themeColor2;
      ((ColorBlock) ref themeColorBlock).pressedColor = themeColor3;
      ((ColorBlock) ref themeColorBlock).disabledColor = themeColor3;
      return themeColorBlock;
    }

    public static string[] GetUIThemes()
    {
      List<string> stringList = new List<string>();
      bool flag1 = false;
      bool flag2 = false;
      foreach (string key in UIManager._uiThemes.Keys)
      {
        switch (key)
        {
          case "Light":
            flag1 = true;
            continue;
          case "Dark":
            flag2 = true;
            continue;
          default:
            stringList.Add(key);
            continue;
        }
      }
      if (flag2)
        stringList.Insert(0, "Dark");
      if (flag1)
        stringList.Insert(0, "Light");
      return stringList.ToArray();
    }

    private static void LoadUIThemes()
    {
      if (!Directory.Exists(UIManager.UIThemeFolderPath))
      {
        Directory.CreateDirectory(UIManager.UIThemeFolderPath);
        Debug.Log((object) "No UI theme folder found, creating it.");
      }
      else
      {
        foreach (string file in Directory.GetFiles(UIManager.UIThemeFolderPath, "*.json"))
        {
          JSONObject jsonObject = (JSONObject) JSON.Parse(File.ReadAllText(file));
          if (!UIManager._uiThemes.ContainsKey((string) jsonObject["Name"]))
            UIManager._uiThemes.Add(jsonObject["Name"].Value, jsonObject);
        }
        if (UIManager._uiThemes.ContainsKey(SettingsManager.UISettings.UITheme.Value))
          return;
        SettingsManager.UISettings.UITheme.Value = "Dark";
        SettingsManager.UISettings.Save();
      }
    }
  }
}
