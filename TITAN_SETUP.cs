// Decompiled with JetBrains decompiler
// Type: TITAN_SETUP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using CustomSkins;
using Photon;
using Settings;
using System.Collections;
using UnityEngine;

public class TITAN_SETUP : MonoBehaviour
{
  public GameObject eye;
  private CostumeHair hair;
  private GameObject hair_go_ref;
  private int hairType;
  public bool haseye;
  public GameObject part_hair;
  public int skin;
  private TitanCustomSkinLoader _customSkinLoader;

  private void Awake()
  {
    CostumeHair.init();
    CharacterMaterials.init();
    HeroCostume.init2();
    this.hair_go_ref = new GameObject();
    this.eye.transform.parent = ((Component) ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head")).transform;
    this.hair_go_ref.transform.position = Vector3.op_Addition(Vector3.op_Addition(this.eye.transform.position, Vector3.op_Multiply(Vector3.up, 3.5f)), Vector3.op_Multiply(((Component) this).transform.forward, 5.2f));
    this.hair_go_ref.transform.rotation = this.eye.transform.rotation;
    this.hair_go_ref.transform.RotateAround(this.eye.transform.position, ((Component) this).transform.right, -20f);
    this.hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
    this.hair_go_ref.transform.parent = ((Component) ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head")).transform;
    this._customSkinLoader = ((Component) this).gameObject.AddComponent<TitanCustomSkinLoader>();
  }

  public IEnumerator loadskinE(int hair, int eye, string hairlink)
  {
    TITAN_SETUP titanSetup = this;
    Object.Destroy((Object) titanSetup.part_hair);
    titanSetup.hair = CostumeHair.hairsM[hair];
    titanSetup.hairType = hair;
    if (titanSetup.hair.hair != string.Empty)
    {
      GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("Character/" + titanSetup.hair.hair));
      gameObject.transform.parent = titanSetup.hair_go_ref.transform.parent;
      gameObject.transform.position = titanSetup.hair_go_ref.transform.position;
      gameObject.transform.rotation = titanSetup.hair_go_ref.transform.rotation;
      gameObject.transform.localScale = titanSetup.hair_go_ref.transform.localScale;
      gameObject.renderer.material = CharacterMaterials.materials[titanSetup.hair.texture];
      titanSetup.part_hair = gameObject;
      yield return (object) titanSetup.StartCoroutine(titanSetup._customSkinLoader.LoadSkinsFromRPC(new object[2]
      {
        (object) true,
        (object) hairlink
      }));
    }
    if (eye >= 0)
      titanSetup.setFacialTexture(titanSetup.eye, eye);
    yield return (object) null;
  }

  public void setFacialTexture(GameObject go, int id)
  {
    if (id < 0)
      return;
    float num1 = 0.25f;
    float num2 = 0.125f * (float) (int) ((double) id / 8.0);
    float num3 = -num1 * (float) (id % 4);
    go.renderer.material.mainTextureOffset = new Vector2(num2, num3);
  }

  public void setHair2()
  {
    BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
    if (titan.SkinsEnabled.Value && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine))
    {
      TitanCustomSkinSet selectedSet = (TitanCustomSkinSet) titan.GetSelectedSet();
      int num1 = Random.Range(0, 9);
      if (num1 == 3)
        num1 = 9;
      int index = this.skin;
      if (selectedSet.RandomizedPairs.Value)
        index = Random.Range(0, 5);
      int num2 = ((TypedSetting<int>) selectedSet.HairModels.GetItemAt(index)).Value - 1;
      if (num2 >= 0)
        num1 = num2;
      string hairlink = ((TypedSetting<string>) selectedSet.Hairs.GetItemAt(index)).Value;
      int num3 = Random.Range(1, 8);
      if (this.haseye)
        num3 = 0;
      bool flag = false;
      if (hairlink.EndsWith(".jpg") || hairlink.EndsWith(".png") || hairlink.EndsWith(".jpeg"))
        flag = true;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
      {
        if (flag)
        {
          this.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, (object) num1, (object) num3, (object) hairlink);
        }
        else
        {
          Color hairColor = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
          this.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, (object) num1, (object) num3, (object) hairColor.r, (object) hairColor.g, (object) hairColor.b);
        }
      }
      else
      {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
          return;
        if (flag)
        {
          this.StartCoroutine(this.loadskinE(num1, num3, hairlink));
        }
        else
        {
          Color hairColor = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
          this.setHairPRC(num1, num3, hairColor.r, hairColor.g, hairColor.b);
        }
      }
    }
    else
    {
      int index = Random.Range(0, CostumeHair.hairsM.Length);
      if (index == 3)
        index = 9;
      Object.Destroy((Object) this.part_hair);
      this.hairType = index;
      this.hair = CostumeHair.hairsM[index];
      if (this.hair.hair == string.Empty)
      {
        this.hair = CostumeHair.hairsM[9];
        this.hairType = 9;
      }
      this.part_hair = (GameObject) Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
      this.part_hair.transform.parent = this.hair_go_ref.transform.parent;
      this.part_hair.transform.position = this.hair_go_ref.transform.position;
      this.part_hair.transform.rotation = this.hair_go_ref.transform.rotation;
      this.part_hair.transform.localScale = this.hair_go_ref.transform.localScale;
      this.part_hair.renderer.material = CharacterMaterials.materials[this.hair.texture];
      this.part_hair.renderer.material.color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
      int id = Random.Range(1, 8);
      this.setFacialTexture(this.eye, id);
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine)
        return;
      this.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, (object) this.hairType, (object) id, (object) this.part_hair.renderer.material.color.r, (object) this.part_hair.renderer.material.color.g, (object) this.part_hair.renderer.material.color.b);
    }
  }

  [RPC]
  private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
  {
    Object.Destroy((Object) this.part_hair);
    this.hair = CostumeHair.hairsM[type];
    this.hairType = type;
    if (this.hair.hair != string.Empty)
    {
      GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
      gameObject.transform.parent = this.hair_go_ref.transform.parent;
      gameObject.transform.position = this.hair_go_ref.transform.position;
      gameObject.transform.rotation = this.hair_go_ref.transform.rotation;
      gameObject.transform.localScale = this.hair_go_ref.transform.localScale;
      gameObject.renderer.material = CharacterMaterials.materials[this.hair.texture];
      gameObject.renderer.material.color = new Color(c1, c2, c3);
      this.part_hair = gameObject;
    }
    this.setFacialTexture(this.eye, eye_type);
  }

  [RPC]
  public void setHairRPC2(int hair, int eye, string hairlink, PhotonMessageInfo info)
  {
    BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
    if (info.sender != this.photonView.owner || !titan.SkinsEnabled.Value || titan.SkinsLocal.Value && !this.photonView.isMine)
      return;
    this.StartCoroutine(this.loadskinE(hair, eye, hairlink));
  }

  public void setPunkHair()
  {
    Object.Destroy((Object) this.part_hair);
    this.hair = CostumeHair.hairsM[3];
    this.hairType = 3;
    GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
    gameObject.transform.parent = this.hair_go_ref.transform.parent;
    gameObject.transform.position = this.hair_go_ref.transform.position;
    gameObject.transform.rotation = this.hair_go_ref.transform.rotation;
    gameObject.transform.localScale = this.hair_go_ref.transform.localScale;
    gameObject.renderer.material = CharacterMaterials.materials[this.hair.texture];
    switch (Random.Range(1, 4))
    {
      case 1:
        gameObject.renderer.material.color = FengColor.hairPunk1;
        break;
      case 2:
        gameObject.renderer.material.color = FengColor.hairPunk2;
        break;
      case 3:
        gameObject.renderer.material.color = FengColor.hairPunk3;
        break;
    }
    this.part_hair = gameObject;
    this.setFacialTexture(this.eye, 0);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine)
      return;
    this.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, (object) this.hairType, (object) 0, (object) this.part_hair.renderer.material.color.r, (object) this.part_hair.renderer.material.color.g, (object) this.part_hair.renderer.material.color.b);
  }

  public void setVar(int skin, bool haseye)
  {
    this.skin = skin;
    this.haseye = haseye;
  }
}
