// Decompiled with JetBrains decompiler
// Type: ApplicationManagers.FullscreenHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UI;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
  internal class FullscreenHandler : MonoBehaviour
  {
    private static FullscreenHandler _instance;
    private static bool _exclusiveFullscreen;
    private static bool _fullscreen;
    private static int WindowedWidth;
    private static int WindowedHeight;
    private static int FullscreenWidth;
    private static int FullscreenHeight;
    private static readonly string RootPath = Application.dataPath + "/FullscreenFix";
    private static readonly string BorderlessPath = FullscreenHandler.RootPath + "/mainDataBorderless";
    private static readonly string ExclusivePath = FullscreenHandler.RootPath + "/mainDataExclusive";
    private static readonly string MainDataPath = Application.dataPath + "/mainData";

    [DllImport("user32.dll")]
    private static extern int GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(int hWnd, int nCmdShow);

    public static void Init()
    {
      FullscreenHandler._instance = SingletonFactory.CreateSingleton<FullscreenHandler>(FullscreenHandler._instance);
      FullscreenHandler._fullscreen = Screen.fullScreen;
      FullscreenHandler._exclusiveFullscreen = SettingsManager.GraphicsSettings.ExclusiveFullscreen.Value;
      if (FullscreenHandler._fullscreen)
      {
        FullscreenHandler.WindowedWidth = 960;
        FullscreenHandler.WindowedHeight = 600;
        FullscreenHandler.FullscreenWidth = Screen.width;
        FullscreenHandler.FullscreenHeight = Screen.height;
      }
      else
      {
        FullscreenHandler.WindowedWidth = Screen.width;
        FullscreenHandler.WindowedHeight = Screen.height;
        Resolution currentResolution1 = Screen.currentResolution;
        FullscreenHandler.FullscreenWidth = ((Resolution) ref currentResolution1).width;
        Resolution currentResolution2 = Screen.currentResolution;
        FullscreenHandler.FullscreenHeight = ((Resolution) ref currentResolution2).height;
      }
    }

    public static void ToggleFullscreen()
    {
      FullscreenHandler.SetFullscreen(!FullscreenHandler._fullscreen);
      FullscreenHandler._fullscreen = !FullscreenHandler._fullscreen;
    }

    private static void SetFullscreen(bool fullscreen)
    {
      int num = fullscreen != Screen.fullScreen ? 1 : 0;
      if (fullscreen && !Screen.fullScreen)
        Screen.SetResolution(FullscreenHandler.FullscreenWidth, FullscreenHandler.FullscreenHeight, true);
      else if (!fullscreen && Screen.fullScreen)
        Screen.SetResolution(FullscreenHandler.WindowedWidth, FullscreenHandler.WindowedHeight, false);
      if (num == 0)
        return;
      FullscreenHandler._instance.StartCoroutine(FullscreenHandler._instance.WaitAndRefreshHUD());
      CursorManager.RefreshCursorLock();
      if (!Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null))
        return;
      UIManager.CurrentMenu.ApplyScale();
    }

    public void OnApplicationFocus(bool hasFocus)
    {
      if (!FullscreenHandler.Supported())
        return;
      if (FullscreenHandler._exclusiveFullscreen)
      {
        if (hasFocus)
        {
          FullscreenHandler.SetFullscreen(FullscreenHandler._fullscreen);
        }
        else
        {
          FullscreenHandler.SetFullscreen(false);
          FullscreenHandler.ShowWindow(FullscreenHandler.GetActiveWindow(), 2);
        }
      }
      else if (hasFocus)
        FullscreenHandler._instance.StartCoroutine(this.WaitAndRefreshMinimap());
      CursorManager.RefreshCursorLock();
    }

    private IEnumerator WaitAndRefreshHUD()
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) new WaitForEndOfFrame();
      IN_GAME_MAIN_CAMERA.needSetHUD = true;
      Minimap.OnScreenResolutionChanged();
      GameObject gameObject = GameObject.Find("Stylish");
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        gameObject.GetComponent<StylishComponent>().OnResolutionChange();
    }

    private IEnumerator WaitAndRefreshMinimap()
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) new WaitForEndOfFrame();
      Minimap.OnScreenResolutionChanged();
    }

    public static void SetMainData(bool trueFullscreen)
    {
      if (!FullscreenHandler.Supported())
        return;
      try
      {
        if (trueFullscreen)
          File.Copy(FullscreenHandler.ExclusivePath, FullscreenHandler.MainDataPath, true);
        else
          File.Copy(FullscreenHandler.BorderlessPath, FullscreenHandler.MainDataPath, true);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("FullscreenHandler error setting main data: " + ex.Message));
      }
    }

    private static bool Supported() => Application.platform == 2;
  }
}
