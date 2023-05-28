// Decompiled with JetBrains decompiler
// Type: BTN_Server_ASIA
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class BTN_Server_ASIA : MonoBehaviour
{
  private void OnClick() => SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
}
