// Decompiled with JetBrains decompiler
// Type: supplyCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class supplyCheck : MonoBehaviour
{
  private float elapsedTime;
  private float stepTime = 1f;

  private void Start()
  {
    if (!Object.op_Inequality((Object) Minimap.instance, (Object) null))
      return;
    Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.white, false, true, Minimap.IconStyle.SUPPLY);
  }

  private void Update()
  {
    this.elapsedTime += Time.deltaTime;
    if ((double) this.elapsedTime <= (double) this.stepTime)
      return;
    this.elapsedTime -= this.stepTime;
    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
    {
      if (Object.op_Inequality((Object) go.GetComponent<HERO>(), (Object) null))
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if ((double) Vector3.Distance(go.transform.position, ((Component) this).transform.position) < 1.5)
            go.GetComponent<HERO>().getSupply();
        }
        else if (go.GetPhotonView().isMine && (double) Vector3.Distance(go.transform.position, ((Component) this).transform.position) < 1.5)
          go.GetComponent<HERO>().getSupply();
      }
    }
  }
}
