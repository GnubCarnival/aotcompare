// Decompiled with JetBrains decompiler
// Type: BombExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using Settings;
using System.Collections;
using UnityEngine;

public class BombExplode : MonoBehaviour
{
  public static float _sizeMultiplier = 1.1f;

  public void Awake()
  {
    if (!Object.op_Inequality((Object) this.photonView, (Object) null))
      return;
    PhotonPlayer owner = this.photonView.owner;
    float num = Mathf.Clamp(RCextensions.returnFloatFromObject(owner.customProperties[(object) PhotonPlayerProperty.RCBombRadius]), 20f, 60f) * 2f * BombExplode._sizeMultiplier;
    ParticleSystem component = ((Component) this).GetComponent<ParticleSystem>();
    if (SettingsManager.AbilitySettings.UseOldEffect.Value)
    {
      component.Stop();
      component.Clear();
      component = ((Component) ((Component) this).transform.Find("OldExplodeEffect")).GetComponent<ParticleSystem>();
      ((Component) component).gameObject.SetActive(true);
      num /= BombExplode._sizeMultiplier;
    }
    if (SettingsManager.AbilitySettings.ShowBombColors.Value)
      component.startColor = BombUtil.GetBombColor(owner);
    component.startSize = num;
    if (!this.photonView.isMine)
      return;
    this.StartCoroutine(this.WaitAndDestroyCoroutine(1.5f));
  }

  private IEnumerator WaitAndDestroyCoroutine(float time)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BombExplode bombExplode = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      PhotonNetwork.Destroy(((Component) bombExplode).gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(time);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
