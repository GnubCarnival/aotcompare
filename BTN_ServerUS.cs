// Decompiled with JetBrains decompiler
// Type: BTN_ServerUS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class BTN_ServerUS : MonoBehaviour
{
  private void OnClick() => SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.US);
}
