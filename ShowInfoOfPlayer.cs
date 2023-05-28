// Decompiled with JetBrains decompiler
// Type: ShowInfoOfPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class ShowInfoOfPlayer : MonoBehaviour
{
  public bool DisableOnOwnObjects;
  public Font font;
  private const int FontSize3D = 0;
  private GameObject textGo;
  private TextMesh tm;

  private void OnDisable()
  {
    if (!Object.op_Inequality((Object) this.textGo, (Object) null))
      return;
    this.textGo.SetActive(false);
  }

  private void OnEnable()
  {
    if (!Object.op_Inequality((Object) this.textGo, (Object) null))
      return;
    this.textGo.SetActive(true);
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.font, (Object) null))
    {
      this.font = (Font) Resources.FindObjectsOfTypeAll(typeof (Font))[0];
      Debug.LogWarning((object) ("No font defined. Found font: " + this.font?.ToString()));
    }
    if (Object.op_Equality((Object) this.tm, (Object) null))
    {
      this.textGo = new GameObject("3d text");
      this.textGo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
      this.textGo.transform.parent = ((Component) this).gameObject.transform;
      this.textGo.transform.localPosition = Vector3.zero;
      ((Renderer) this.textGo.AddComponent<MeshRenderer>()).material = this.font.material;
      this.tm = this.textGo.AddComponent<TextMesh>();
      this.tm.font = this.font;
      this.tm.fontSize = 0;
      this.tm.anchor = (TextAnchor) 4;
    }
    if (this.DisableOnOwnObjects || !this.photonView.isMine)
      return;
    ((Behaviour) this).enabled = false;
  }

  private void Update()
  {
    if (this.DisableOnOwnObjects)
    {
      ((Behaviour) this).enabled = false;
      if (!Object.op_Inequality((Object) this.textGo, (Object) null))
        return;
      this.textGo.SetActive(false);
    }
    else
    {
      PhotonPlayer owner = this.photonView.owner;
      if (owner != null)
        this.tm.text = !string.IsNullOrEmpty(owner.name) ? owner.name : "n/a";
      else if (this.photonView.isSceneView)
      {
        if (!this.DisableOnOwnObjects && this.photonView.isMine)
        {
          ((Behaviour) this).enabled = false;
          this.textGo.SetActive(false);
        }
        else
          this.tm.text = "scn";
      }
      else
        this.tm.text = "n/a";
    }
  }
}
