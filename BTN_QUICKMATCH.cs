// Decompiled with JetBrains decompiler
// Type: BTN_QUICKMATCH
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
  private void OnClick() => SettingsManager.MultiplayerSettings.ConnectOffline();
}
