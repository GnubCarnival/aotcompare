// Decompiled with JetBrains decompiler
// Type: CustomSkins.CityCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class CityCustomSkinLoader : LevelCustomSkinLoader
  {
    private List<GameObject> _houseObjects = new List<GameObject>();
    private List<GameObject> _groundObjects = new List<GameObject>();
    private List<GameObject> _wallObjects = new List<GameObject>();
    private List<GameObject> _gateObjects = new List<GameObject>();

    protected override string RendererIdPrefix => "city";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      CityCustomSkinLoader customSkinLoader = this;
      customSkinLoader.FindAndIndexLevelObjects();
      char[] randomIndices = ((string) data[0]).ToCharArray();
      string[] houseUrls = ((string) data[1]).Split(',');
      string[] miscUrls = ((string) data[2]).Split(',');
      for (int i = 0; i < customSkinLoader._houseObjects.Count; ++i)
      {
        int index = int.Parse(randomIndices[i].ToString());
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(0, customSkinLoader._houseObjects[i]);
        if (!customSkinPart.LoadCache(houseUrls[index]))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(houseUrls[index]));
      }
      foreach (GameObject groundObject in customSkinLoader._groundObjects)
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(1, groundObject);
        if (!customSkinPart.LoadCache(miscUrls[0]))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(miscUrls[0]));
      }
      foreach (GameObject wallObject in customSkinLoader._wallObjects)
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(2, wallObject);
        if (!customSkinPart.LoadCache(miscUrls[1]))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(miscUrls[1]));
      }
      foreach (GameObject gateObject in customSkinLoader._gateObjects)
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(3, gateObject);
        if (!customSkinPart.LoadCache(miscUrls[2]))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(miscUrls[2]));
      }
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
    {
      List<Renderer> renderers = new List<Renderer>();
      switch (partId)
      {
        case 0:
          this.AddAllRenderers(renderers, levelObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 2000000);
        case 1:
          this.AddAllRenderers(renderers, levelObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 2:
          this.AddAllRenderers(renderers, levelObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 3:
          this.AddAllRenderers(renderers, levelObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 2000000);
        default:
          return (BaseCustomSkinPart) null;
      }
    }

    protected override void FindAndIndexLevelObjects()
    {
      this._houseObjects.Clear();
      this._groundObjects.Clear();
      this._wallObjects.Clear();
      this._gateObjects.Clear();
      foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
      {
        string name = ((Object) gameObject).name;
        if (Object.op_Inequality((Object) gameObject, (Object) null) && name.Contains("Cube_") && ((Component) gameObject.transform.parent).gameObject.tag != "Player")
        {
          if (name.EndsWith("001"))
            this._groundObjects.Add(gameObject);
          else if (name.EndsWith("006") || name.EndsWith("007") || name.EndsWith("015") || name.EndsWith("000"))
            this._wallObjects.Add(gameObject);
          else if (name.EndsWith("002") && Vector3.op_Equality(gameObject.transform.position, Vector3.zero))
            this._wallObjects.Add(gameObject);
          else if (name.EndsWith("005") || name.EndsWith("003"))
            this._houseObjects.Add(gameObject);
          else if (name.EndsWith("002") && Vector3.op_Inequality(gameObject.transform.position, Vector3.zero))
            this._houseObjects.Add(gameObject);
          else if (name.EndsWith("019") || name.EndsWith("020"))
            this._gateObjects.Add(gameObject);
        }
      }
    }
  }
}
