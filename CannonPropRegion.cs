// Decompiled with JetBrains decompiler
// Type: CannonPropRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using System;
using System.Collections;
using UnityEngine;

internal class CannonPropRegion : MonoBehaviour
{
  public bool destroyed;
  public bool disabled;
  public string settings;
  public HERO storedHero;

  public void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.storedHero, (Object) null))
      return;
    this.storedHero.myCannonRegion = (CannonPropRegion) null;
    this.storedHero.ClearPopup();
  }

  public void OnTriggerEnter(Collider collider)
  {
    GameObject gameObject = ((Component) ((Component) collider).transform.root).gameObject;
    if (gameObject.layer != 8 || !gameObject.GetPhotonView().isMine)
      return;
    HERO component = gameObject.GetComponent<HERO>();
    if (!Object.op_Inequality((Object) component, (Object) null) || component.isCannon)
      return;
    if (Object.op_Inequality((Object) component.myCannonRegion, (Object) null))
      component.myCannonRegion.storedHero = (HERO) null;
    component.myCannonRegion = this;
    this.storedHero = component;
  }

  public void OnTriggerExit(Collider collider)
  {
    GameObject gameObject = ((Component) ((Component) collider).transform.root).gameObject;
    if (gameObject.layer != 8 || !gameObject.GetPhotonView().isMine)
      return;
    HERO component = gameObject.GetComponent<HERO>();
    if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) this.storedHero, (Object) null) || !Object.op_Equality((Object) component, (Object) this.storedHero))
      return;
    component.myCannonRegion = (CannonPropRegion) null;
    component.ClearPopup();
    this.storedHero = (HERO) null;
  }

  [RPC]
  public void RequestControlRPC(int viewID, PhotonMessageInfo info)
  {
    if (!this.photonView.isMine || !PhotonNetwork.isMasterClient || this.disabled)
      return;
    HERO component = ((Component) PhotonView.Find(viewID)).gameObject.GetComponent<HERO>();
    if (!Object.op_Inequality((Object) component, (Object) null) || component.photonView.owner != info.sender || FengGameManagerMKII.instance.allowedToCannon.ContainsKey(info.sender.ID))
      return;
    this.disabled = true;
    this.StartCoroutine(this.WaitAndEnable());
    FengGameManagerMKII.instance.allowedToCannon.Add(info.sender.ID, new CannonValues(this.photonView.viewID, this.settings));
    component.photonView.RPC("SpawnCannonRPC", info.sender, (object) this.settings);
  }

  [RPC]
  public void SetSize(string settings, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    string[] strArray = settings.Split(',');
    if (strArray.Length <= 15)
      return;
    float num1 = 1f;
    GameObject gameObject = ((Component) this).gameObject;
    if (strArray[2] != "default")
    {
      if (strArray[2].StartsWith("transparent"))
      {
        float result;
        if (float.TryParse(strArray[2].Substring(11), out result))
          num1 = result;
        foreach (Renderer componentsInChild in gameObject.GetComponentsInChildren<Renderer>())
        {
          componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load("transparent");
          if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
            componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
        }
      }
      else
      {
        foreach (Renderer componentsInChild in gameObject.GetComponentsInChildren<Renderer>())
        {
          componentsInChild.material = (Material) FengGameManagerMKII.RCassets.Load(strArray[2]);
          if ((double) Convert.ToSingle(strArray[10]) != 1.0 || (double) Convert.ToSingle(strArray[11]) != 1.0)
            componentsInChild.material.mainTextureScale = new Vector2(componentsInChild.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), componentsInChild.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
        }
      }
    }
    float num2 = gameObject.transform.localScale.x * Convert.ToSingle(strArray[3]) - 1f / 1000f;
    float num3 = gameObject.transform.localScale.y * Convert.ToSingle(strArray[4]);
    float num4 = gameObject.transform.localScale.z * Convert.ToSingle(strArray[5]);
    gameObject.transform.localScale = new Vector3(num2, num3, num4);
    if (!(strArray[6] != "0"))
      return;
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num1);
    foreach (MeshFilter componentsInChild in gameObject.GetComponentsInChildren<MeshFilter>())
    {
      Mesh mesh = componentsInChild.mesh;
      Color[] colorArray = new Color[mesh.vertexCount];
      for (int index = 0; index < mesh.vertexCount; ++index)
        colorArray[index] = color;
      mesh.colors = colorArray;
    }
  }

  public void Start()
  {
    if ((int) FengGameManagerMKII.settingsOld[64] < 100)
      return;
    ((Component) this).GetComponent<Collider>().enabled = false;
  }

  public IEnumerator WaitAndEnable()
  {
    yield return (object) new WaitForSeconds(5f);
    if (!this.destroyed)
      this.disabled = false;
  }
}
