// Decompiled with JetBrains decompiler
// Type: Language
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class Language
{
  public static string[] abnormal = new string[25];
  public static string[] btn_back = new string[25];
  public static string[] btn_continue = new string[25];
  public static string[] btn_create_game = new string[25];
  public static string[] btn_credits = new string[25];
  public static string[] btn_default = new string[25];
  public static string[] btn_join = new string[25];
  public static string[] btn_LAN = new string[25];
  public static string[] btn_multiplayer = new string[25];
  public static string[] btn_option = new string[25];
  public static string[] btn_QUICK_MATCH = new string[25];
  public static string[] btn_quit = new string[25];
  public static string[] btn_ready = new string[25];
  public static string[] btn_refresh = new string[25];
  public static string[] btn_server_ASIA = new string[25];
  public static string[] btn_server_EU = new string[25];
  public static string[] btn_server_JAPAN = new string[25];
  public static string[] btn_server_US = new string[25];
  public static string[] btn_single = new string[25];
  public static string[] btn_start = new string[25];
  public static string[] camera_info = new string[25];
  public static string[] camera_original = new string[25];
  public static string[] camera_tilt = new string[25];
  public static string[] camera_tps = new string[25];
  public static string[] camera_type = new string[25];
  public static string[] camera_wow = new string[25];
  public static string[] change_quality = new string[25];
  public static string[] choose_character = new string[25];
  public static string[] choose_map = new string[25];
  public static string[] choose_region_server = new string[25];
  public static string[] difficulty = new string[25];
  public static string[] game_time = new string[25];
  public static string[] hard = new string[25];
  public static string[] invert_mouse = new string[25];
  public static string[] key_set_info_1 = new string[25];
  public static string[] key_set_info_2 = new string[25];
  public static string[] max_player = new string[25];
  public static string[] max_Time = new string[25];
  public static string[] mouse_sensitivity = new string[25];
  public static string[] normal = new string[25];
  public static string[] port = new string[25];
  public static string[] select_titan = new string[25];
  public static string[] server_ip = new string[25];
  public static string[] server_name = new string[25];
  public static string[] soldier = new string[25];
  public static string[] titan = new string[25];
  public static int type = -1;
  public static string[] waiting_for_input = new string[25];

  public static string GetLang(int id)
  {
    switch (id)
    {
      case 1:
        return "简体中文";
      case 2:
        return "SPANISH";
      case 3:
        return "POLSKI";
      case 4:
        return "ITALIANO";
      case 5:
        return "NORWEGIAN";
      case 6:
        return "PORTUGUESE";
      case 7:
        return "PORTUGUESE_BR";
      case 8:
        return "繁體中文_台";
      case 9:
        return "繁體中文_港";
      case 10:
        return "SLOVAK";
      case 11:
        return "GERMAN";
      case 12:
        return "FRANCAIS";
      case 13:
        return "TÜRKÇE";
      case 14:
        return "ARABIC";
      case 15:
        return "Thai";
      case 16:
        return "Русский";
      case 17:
        return "NEDERLANDS";
      case 18:
        return "Hebrew";
      case 19:
        return "DANSK";
      default:
        return "ENGLISH";
    }
  }

  public static int GetLangIndex(string txt)
  {
    if (txt != "ENGLISH")
    {
      switch (txt)
      {
        case "SPANISH":
          return 2;
        case "POLSKI":
          return 3;
        case "ITALIANO":
          return 4;
        case "NORWEGIAN":
          return 5;
        case "PORTUGUESE":
          return 6;
        case "PORTUGUESE_BR":
          return 7;
        case "SLOVAK":
          return 10;
        case "GERMAN":
          return 11;
        case "FRANCAIS":
          return 12;
        case "TÜRKÇE":
          return 13;
        case "ARABIC":
          return 14;
        case "Thai":
          return 15;
        case "Русский":
          return 16;
        case "NEDERLANDS":
          return 17;
        case "Hebrew":
          return 18;
        case "DANSK":
          return 19;
        case "简体中文":
          return 1;
        case "繁體中文_台":
          return 8;
        case "繁體中文_港":
          return 9;
      }
    }
    return 0;
  }

  public static void init()
  {
    char[] chArray1 = new char[1]{ "\n"[0] };
    string[] strArray = ((TextAsset) Resources.Load("lang")).text.Split(chArray1);
    string empty1 = string.Empty;
    int index1 = 0;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    for (int index2 = 0; index2 < strArray.Length; ++index2)
    {
      string str1 = strArray[index2];
      if (!str1.Contains("//"))
      {
        if (str1.Contains("#START"))
        {
          char[] chArray2 = new char[1]{ "@"[0] };
          empty1 = str1.Split(chArray2)[1];
          index1 = Language.GetLangIndex(empty1);
        }
        else if (str1.Contains("#END"))
          empty1 = string.Empty;
        else if (empty1 != string.Empty && str1.Contains("@"))
        {
          char[] chArray3 = new char[1]{ "@"[0] };
          string str2 = str1.Split(chArray3)[0];
          char[] chArray4 = new char[1]{ "@"[0] };
          string str3 = str1.Split(chArray4)[1];
          switch (str2)
          {
            case "abnormal":
              Language.abnormal[index1] = str3;
              continue;
            case "btn_LAN":
              Language.btn_LAN[index1] = str3;
              continue;
            case "btn_QUICK_MATCH":
              Language.btn_QUICK_MATCH[index1] = str3;
              continue;
            case "btn_back":
              Language.btn_back[index1] = str3;
              continue;
            case "btn_continue":
              Language.btn_continue[index1] = str3;
              continue;
            case "btn_create_game":
              Language.btn_create_game[index1] = str3;
              continue;
            case "btn_credits":
              Language.btn_credits[index1] = str3;
              continue;
            case "btn_default":
              Language.btn_default[index1] = str3;
              continue;
            case "btn_join":
              Language.btn_join[index1] = str3;
              continue;
            case "btn_multiplayer":
              Language.btn_multiplayer[index1] = str3;
              continue;
            case "btn_option":
              Language.btn_option[index1] = str3;
              continue;
            case "btn_quit":
              Language.btn_quit[index1] = str3;
              continue;
            case "btn_ready":
              Language.btn_ready[index1] = str3;
              continue;
            case "btn_refresh":
              Language.btn_refresh[index1] = str3;
              continue;
            case "btn_server_ASIA":
              Language.btn_server_ASIA[index1] = str3;
              continue;
            case "btn_server_EU":
              Language.btn_server_EU[index1] = str3;
              continue;
            case "btn_server_JAPAN":
              Language.btn_server_JAPAN[index1] = str3;
              continue;
            case "btn_server_US":
              Language.btn_server_US[index1] = str3;
              continue;
            case "btn_single":
              Language.btn_single[index1] = str3;
              continue;
            case "btn_start":
              Language.btn_start[index1] = str3;
              continue;
            case "camera_info":
              Language.camera_info[index1] = str3;
              continue;
            case "camera_original":
              Language.camera_original[index1] = str3;
              continue;
            case "camera_tilt":
              Language.camera_tilt[index1] = str3;
              continue;
            case "camera_tps":
              Language.camera_tps[index1] = str3;
              continue;
            case "camera_type":
              Language.camera_type[index1] = str3;
              continue;
            case "camera_wow":
              Language.camera_wow[index1] = str3;
              continue;
            case "change_quality":
              Language.change_quality[index1] = str3;
              continue;
            case "choose_character":
              Language.choose_character[index1] = str3;
              continue;
            case "choose_map":
              Language.choose_map[index1] = str3;
              continue;
            case "choose_region_server":
              Language.choose_region_server[index1] = str3;
              continue;
            case "difficulty":
              Language.difficulty[index1] = str3;
              continue;
            case "game_time":
              Language.game_time[index1] = str3;
              continue;
            case "hard":
              Language.hard[index1] = str3;
              continue;
            case "invert_mouse":
              Language.invert_mouse[index1] = str3;
              continue;
            case "key_set_info_1":
              Language.key_set_info_1[index1] = str3;
              continue;
            case "key_set_info_2":
              Language.key_set_info_2[index1] = str3;
              continue;
            case "max_Time":
              Language.max_Time[index1] = str3;
              continue;
            case "max_player":
              Language.max_player[index1] = str3;
              continue;
            case "mouse_sensitivity":
              Language.mouse_sensitivity[index1] = str3;
              continue;
            case "normal":
              Language.normal[index1] = str3;
              continue;
            case "port":
              Language.port[index1] = str3;
              continue;
            case "select_titan":
              Language.select_titan[index1] = str3;
              continue;
            case "server_ip":
              Language.server_ip[index1] = str3;
              continue;
            case "server_name":
              Language.server_name[index1] = str3;
              continue;
            case "soldier":
              Language.soldier[index1] = str3;
              continue;
            case "titan":
              Language.titan[index1] = str3;
              continue;
            case "waiting_for_input":
              Language.waiting_for_input[index1] = str3;
              continue;
            default:
              continue;
          }
        }
      }
    }
  }
}
