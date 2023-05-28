// Decompiled with JetBrains decompiler
// Type: LevelTriggerHint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using UnityEngine;

public class LevelTriggerHint : MonoBehaviour
{
  public string content;
  public HintType myhint;
  private bool on;

  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "Player"))
      return;
    this.on = true;
  }

  private void Start()
  {
    if (!LevelInfo.getInfo(FengGameManagerMKII.level).hint)
      ((Behaviour) this).enabled = false;
    if (!(this.content == string.Empty))
      return;
    switch (this.myhint)
    {
      case HintType.MOVE:
        this.content = "Hello soldier!\nWelcome to Attack On Titan Tribute Game!\n Press [F7D358]" + SettingsManager.InputSettings.General.Forward.ToString() + SettingsManager.InputSettings.General.Left.ToString() + SettingsManager.InputSettings.General.Back.ToString() + SettingsManager.InputSettings.General.Right.ToString() + "[-] to Move.";
        break;
      case HintType.TELE:
        this.content = "Move to [82FA58]green warp point[-] to proceed.";
        break;
      case HintType.CAMA:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.General.ChangeCamera.ToString() + "[-] to change camera mode\nPress [F7D358]" + SettingsManager.InputSettings.General.HideUI.ToString() + "[-] to hide or show the cursor.";
        break;
      case HintType.JUMP:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Jump.ToString() + "[-] to Jump.";
        break;
      case HintType.JUMP2:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.General.Forward.ToString() + "[-] towards a wall to perform a wall-run.";
        break;
      case HintType.HOOK:
        this.content = "Press and Hold[F7D358] " + SettingsManager.InputSettings.Human.HookLeft.ToString() + "[-] or [F7D358]" + SettingsManager.InputSettings.Human.HookRight.ToString() + "[-] to launch your grapple.\nNow Try hooking to the [>3<] box. ";
        break;
      case HintType.HOOK2:
        this.content = "Press and Hold[F7D358] " + SettingsManager.InputSettings.Human.HookBoth.ToString() + "[-] to launch both of your grapples at the same Time.\n\nNow aim between the two black blocks. \nYou will see the mark '<' and '>' appearing on the blocks. \nThen press " + SettingsManager.InputSettings.Human.HookBoth.ToString() + " to hook the blocks.";
        break;
      case HintType.SUPPLY:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Reload.ToString() + "[-] to reload your blades.\n Move to the supply station to refill your gas and blades.";
        break;
      case HintType.DODGE:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Dodge.ToString() + "[-] to Dodge.";
        break;
      case HintType.ATTACK:
        this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.AttackDefault.ToString() + "[-] to Attack. \nPress [F7D358]" + SettingsManager.InputSettings.Human.AttackSpecial.ToString() + "[-] to use special attack.\n***You can only kill a titan by slashing his [FA5858]NAPE[-].***\n\n";
        break;
    }
  }

  private void Update()
  {
    if (!this.on)
      return;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoCenter(this.content + "\n\n\n\n\n");
    this.on = false;
  }
}
