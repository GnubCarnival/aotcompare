// Decompiled with JetBrains decompiler
// Type: LevelBottom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelBottom : MonoBehaviour
{
  public GameObject link;
  public BottomType type;

  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "Player"))
      return;
    if (this.type == BottomType.Die)
    {
      if (!Object.op_Inequality((Object) ((Component) other).gameObject.GetComponent<HERO>(), (Object) null))
        return;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
      {
        if (!((Component) other).gameObject.GetPhotonView().isMine)
          return;
        ((Component) other).gameObject.GetComponent<HERO>().netDieLocal(Vector3.op_Multiply(((Component) this).rigidbody.velocity, 50f), false, titanName: string.Empty);
      }
      else
        ((Component) other).gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(((Component) other).gameObject.rigidbody.velocity, 50f), false);
    }
    else
    {
      if (this.type != BottomType.Teleport)
        return;
      if (Object.op_Inequality((Object) this.link, (Object) null))
        ((Component) other).gameObject.transform.position = this.link.transform.position;
      else
        ((Component) other).gameObject.transform.position = Vector3.zero;
    }
  }

  private void Start()
  {
  }

  private void Update()
  {
  }
}
