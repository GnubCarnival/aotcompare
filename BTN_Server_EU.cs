// Decompiled with JetBrains decompiler
// Type: BTN_Server_EU
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class BTN_Server_EU : MonoBehaviour
{
  private void OnClick() => SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.EU);
}
