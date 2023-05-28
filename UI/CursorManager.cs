// Decompiled with JetBrains decompiler
// Type: UI.CursorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
  internal class CursorManager : MonoBehaviour
  {
    public static CursorState State;
    private static CursorManager _instance;
    private static Texture2D _cursorPointer;
    private static Dictionary<CrosshairStyle, Texture2D> _crosshairs = new Dictionary<CrosshairStyle, Texture2D>();
    private bool _ready;
    private bool _crosshairWhite = true;
    private bool _lastCrosshairWhite;
    private string _crosshairText = string.Empty;
    private bool _forceNextCrosshairUpdate;
    private CrosshairStyle _lastCrosshairStyle;

    public static void Init() => CursorManager._instance = SingletonFactory.CreateSingleton<CursorManager>(CursorManager._instance);

    public static void FinishLoadAssets()
    {
      CursorManager._cursorPointer = (Texture2D) AssetBundleManager.MainAssetBundle.Load("CursorPointer");
      foreach (CrosshairStyle key in Enum.GetValues(typeof (CrosshairStyle)))
      {
        Texture2D texture2D = (Texture2D) AssetBundleManager.MainAssetBundle.Load("Cursor" + key.ToString());
        CursorManager._crosshairs.Add(key, texture2D);
      }
      CursorManager._instance._ready = true;
      CursorManager.SetPointer(true);
    }

    private void Update()
    {
      if (Application.loadedLevel == 0 || Application.loadedLevelName == "characterCreation" || Application.loadedLevelName == "Snapshot")
        CursorManager.SetPointer();
      else if (Application.loadedLevel == 2 && (int) FengGameManagerMKII.settingsOld[64] >= 100)
      {
        if (((Behaviour) ((Component) Camera.main).GetComponent<MouseLook>()).enabled)
          CursorManager.SetHidden();
        else
          CursorManager.SetPointer();
      }
      else if (GameMenu.InMenu() || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
        CursorManager.SetPointer();
      else if (!FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded)
        CursorManager.SetPointer();
      else if (FengGameManagerMKII.instance.needChooseSide && NGUITools.GetActive(FengGameManagerMKII.instance.ui.GetComponent<UIReferArray>().panels[3]))
        CursorManager.SetPointer();
      else if (Object.op_Inequality((Object) IN_GAME_MAIN_CAMERA.Instance.main_object, (Object) null))
      {
        HERO component = IN_GAME_MAIN_CAMERA.Instance.main_object.GetComponent<HERO>();
        if (SettingsManager.LegacyGeneralSettings.SpecMode.Value || Object.op_Equality((Object) component, (Object) null) || !component.IsMine())
          CursorManager.SetHidden();
        else
          CursorManager.SetCrosshair();
      }
      else
        CursorManager.SetHidden();
    }

    public static void RefreshCursorLock()
    {
      if (!Screen.lockCursor)
        return;
      Screen.lockCursor = !Screen.lockCursor;
      Screen.lockCursor = !Screen.lockCursor;
    }

    public static void SetPointer(bool force = false)
    {
      if (!force && CursorManager.State == CursorState.Pointer)
        return;
      Screen.showCursor = true;
      Screen.lockCursor = false;
      CursorManager.State = CursorState.Pointer;
    }

    public static void SetHidden(bool force = false)
    {
      if (force || CursorManager.State != CursorState.Hidden)
      {
        Screen.showCursor = false;
        CursorManager.State = CursorState.Hidden;
      }
      if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
      {
        if (Screen.lockCursor)
          return;
        Screen.lockCursor = true;
      }
      else
      {
        if (!Screen.lockCursor)
          return;
        Screen.lockCursor = false;
      }
    }

    public static void SetCrosshair(bool force = false)
    {
      if (force || CursorManager.State != CursorState.Crosshair)
      {
        Screen.showCursor = false;
        CursorManager.State = CursorState.Crosshair;
      }
      if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
      {
        if (Screen.lockCursor)
          return;
        Screen.lockCursor = true;
      }
      else
      {
        if (!Screen.lockCursor)
          return;
        Screen.lockCursor = false;
      }
    }

    public static void SetCrosshairColor(bool white)
    {
      if (CursorManager._instance._crosshairWhite == white)
        return;
      CursorManager._instance._crosshairWhite = white;
    }

    public static void SetCrosshairText(string text) => CursorManager._instance._crosshairText = text;

    public static void UpdateCrosshair(
      RawImage crosshairImageWhite,
      RawImage crosshairImageRed,
      Text crosshairLabelWhite,
      Text crosshairLabelRed,
      bool force = false)
    {
      if (!CursorManager._instance._ready)
        return;
      if (CursorManager.State != CursorState.Crosshair || GameMenu.HideCrosshair)
      {
        if (((Component) crosshairImageRed).gameObject.activeSelf)
          ((Component) crosshairImageRed).gameObject.SetActive(false);
        if (((Component) crosshairImageWhite).gameObject.activeSelf)
          ((Component) crosshairImageWhite).gameObject.SetActive(false);
        CursorManager._instance._forceNextCrosshairUpdate = true;
      }
      else
      {
        CrosshairStyle key = (CrosshairStyle) SettingsManager.UISettings.CrosshairStyle.Value;
        if (CursorManager._instance._lastCrosshairStyle != key | force || CursorManager._instance._forceNextCrosshairUpdate)
        {
          crosshairImageWhite.texture = (Texture) CursorManager._crosshairs[key];
          crosshairImageRed.texture = (Texture) CursorManager._crosshairs[key];
          CursorManager._instance._lastCrosshairStyle = key;
        }
        if (CursorManager._instance._crosshairWhite != CursorManager._instance._lastCrosshairWhite | force || CursorManager._instance._forceNextCrosshairUpdate)
        {
          ((Component) crosshairImageWhite).gameObject.SetActive(CursorManager._instance._crosshairWhite);
          ((Component) crosshairImageRed).gameObject.SetActive(!CursorManager._instance._crosshairWhite);
          CursorManager._instance._lastCrosshairWhite = CursorManager._instance._crosshairWhite;
        }
        Text text = crosshairLabelWhite;
        RawImage rawImage = crosshairImageWhite;
        if (!CursorManager._instance._crosshairWhite)
        {
          text = crosshairLabelRed;
          rawImage = crosshairImageRed;
        }
        text.text = CursorManager._instance._crosshairText;
        Vector3 mousePosition = Input.mousePosition;
        Transform transform = ((Component) rawImage).transform;
        if (Vector3.op_Inequality(transform.position, mousePosition))
        {
          if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
          {
            if ((double) Math.Abs(transform.position.x - mousePosition.x) > 1.0 || (double) Math.Abs(transform.position.y - mousePosition.y) > 1.0)
              transform.position = mousePosition;
          }
          else
            transform.position = mousePosition;
        }
        CursorManager._instance._forceNextCrosshairUpdate = false;
      }
    }
  }
}
