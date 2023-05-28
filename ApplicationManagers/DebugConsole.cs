// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.DebugConsole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
  internal class DebugConsole : MonoBehaviour
  {
    private static DebugConsole _instance;
    private static bool _enabled;
    private static LinkedList<string> _messages = new LinkedList<string>();
    private static int _currentCharCount = 0;
    private static Vector2 _scrollPosition = Vector2.zero;
    private static string _inputLine = string.Empty;
    private static bool _needResetScroll;
    private const int MaxMessages = 100;
    private const int MaxChars = 5000;
    private const int PositionX = 20;
    private const int PositionY = 20;
    private const int Width = 400;
    private const int Height = 300;
    private const int InputHeight = 25;
    private const int Padding = 10;
    private const string InputControlName = "DebugInput";

    public static void Init()
    {
      DebugConsole._instance = SingletonFactory.CreateSingleton<DebugConsole>(DebugConsole._instance);
      Application.RegisterLogCallback(new Application.LogCallback(DebugConsole.OnUnityDebugLog));
    }

    private static void OnUnityDebugLog(string log, string stackTrace, LogType type)
    {
      DebugConsole.AddMessage(stackTrace);
      DebugConsole.AddMessage(log);
    }

    private static void AddMessage(string message)
    {
      DebugConsole._messages.AddLast(message);
      DebugConsole._currentCharCount += message.Length;
      while (DebugConsole._messages.Count > 100 || DebugConsole._currentCharCount > 5000)
      {
        DebugConsole._currentCharCount -= DebugConsole._messages.First.Value.Length;
        DebugConsole._messages.RemoveFirst();
      }
      DebugConsole._needResetScroll = true;
    }

    private void Update()
    {
      if (!Input.GetKeyDown((KeyCode) 292))
        return;
      DebugConsole._enabled = !DebugConsole._enabled;
    }

    private void OnGUI()
    {
      if (!DebugConsole._enabled)
        return;
      GUI.depth = 1;
      GUI.Box(new Rect(20f, 20f, 400f, 300f), "");
      DebugConsole.DrawMessageWindow();
      DebugConsole.DrawInputWindow();
      DebugConsole.HandleInput();
      GUI.depth = 0;
    }

    private static void DrawMessageWindow()
    {
      int num1 = 30;
      int num2 = 30;
      int num3 = 380;
      GUI.Label(new Rect((float) num1, (float) num2, (float) num3, 25f), "Debug Console (Press F11 to hide)");
      int num4 = num2 + 35;
      int num5 = 210;
      GUIStyle guiStyle = new GUIStyle(GUI.skin.box);
      string str = "";
      foreach (string message in DebugConsole._messages)
        str = str + message + "\n";
      int num6 = num3 - 20;
      int num7 = (int) guiStyle.CalcHeight(new GUIContent(str), (float) num6) + 10;
      DebugConsole._scrollPosition = GUI.BeginScrollView(new Rect((float) num1, (float) num4, (float) num3, (float) num5), DebugConsole._scrollPosition, new Rect((float) num1, (float) num4, (float) num6, (float) num7));
      GUI.Label(new Rect((float) num1, (float) num4, (float) num6, (float) num7), str);
      if (DebugConsole._needResetScroll)
      {
        DebugConsole._needResetScroll = false;
        DebugConsole._scrollPosition = new Vector2(0.0f, (float) num7);
      }
      GUI.EndScrollView();
    }

    private static void DrawInputWindow()
    {
      int num = 285;
      GUI.SetNextControlName("DebugInput");
      DebugConsole._inputLine = GUI.TextField(new Rect(30f, (float) num, 380f, 25f), DebugConsole._inputLine);
    }

    private static void HandleInput()
    {
      if (GUI.GetNameOfFocusedControl() == "DebugInput")
      {
        if (!DebugConsole.IsEnterUp())
          return;
        if (DebugConsole._inputLine != string.Empty)
        {
          Debug.Log((object) DebugConsole._inputLine);
          if (DebugConsole._inputLine.StartsWith("/"))
            DebugTesting.RunDebugCommand(DebugConsole._inputLine.Substring(1));
          else
            Debug.Log((object) "Invalid debug command.");
          DebugConsole._inputLine = string.Empty;
        }
        GUI.FocusControl(string.Empty);
      }
      else
      {
        if (!DebugConsole.IsEnterUp())
          return;
        GUI.FocusControl("DebugInput");
      }
    }

    private static bool IsEnterUp()
    {
      if (Event.current.type != 5)
        return false;
      return Event.current.keyCode == 13 || Event.current.keyCode == 271;
    }
  }
}
