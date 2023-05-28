// Decompiled with JetBrains decompiler
// Type: CustomSkins.CustomLevelCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class CustomLevelCustomSkinLoader : LevelCustomSkinLoader
  {
    private List<GameObject> _groundObjects = new List<GameObject>();

    protected override string RendererIdPrefix => "customlevel";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      CustomLevelCustomSkinLoader customSkinLoader = this;
      customSkinLoader.FindAndIndexLevelObjects();
      string groundUrl = (string) data[6];
      foreach (GameObject groundObject in customSkinLoader._groundObjects)
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(0, groundObject);
        if (!customSkinPart.LoadCache(groundUrl))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(groundUrl));
      }
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
    {
      List<Renderer> renderers = new List<Renderer>();
      if (partId != 0)
        return (BaseCustomSkinPart) null;
      this.AddAllRenderers(renderers, levelObject);
      return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
    }

    protected override void FindAndIndexLevelObjects()
    {
      this._groundObjects.Clear();
      foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
      {
        if (Object.op_Inequality((Object) gameObject, (Object) null) && ((Object) gameObject).name.Contains("Cube_001") && ((Component) gameObject.transform.parent).gameObject.tag != "Player" && Object.op_Inequality((Object) gameObject.renderer, (Object) null))
          this._groundObjects.Add(gameObject);
      }
    }
  }
}
