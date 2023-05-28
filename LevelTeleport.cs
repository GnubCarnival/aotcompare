// Decompiled with JetBrains decompiler
// Type: LevelTeleport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
  public string levelname = string.Empty;
  public GameObject link;

  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "Player"))
      return;
    if (this.levelname != string.Empty)
      Application.LoadLevel(this.levelname);
    else
      ((Component) other).gameObject.transform.position = this.link.transform.position;
  }

  private void Start()
  {
  }

  private void Update()
  {
  }
}
