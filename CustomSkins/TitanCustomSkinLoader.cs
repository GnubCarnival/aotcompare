// Decompiled with JetBrains decompiler
// Type: CustomSkins.TitanCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class TitanCustomSkinLoader : BaseCustomSkinLoader
  {
    protected override string RendererIdPrefix => "titan";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      TitanCustomSkinLoader customSkinLoader = this;
      if ((bool) data[0])
      {
        string url = (string) data[1];
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(0);
        if (!customSkinPart.LoadCache(url))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(url));
      }
      else
      {
        string url = (string) data[1];
        string eyeUrl = (string) data[2];
        BaseCustomSkinPart customSkinPart1 = customSkinLoader.GetCustomSkinPart(1);
        if (!customSkinPart1.LoadCache(url))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart1.LoadSkin(url));
        BaseCustomSkinPart customSkinPart2 = customSkinLoader.GetCustomSkinPart(2);
        if (!customSkinPart2.LoadCache(eyeUrl))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart2.LoadSkin(eyeUrl));
        eyeUrl = (string) null;
      }
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
    {
      TITAN component = this._owner.GetComponent<TITAN>();
      List<Renderer> renderers = new List<Renderer>();
      switch (partId)
      {
        case 0:
          this.AddRendererIfExists(renderers, ((Component) component).GetComponent<TITAN_SETUP>().part_hair);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 1:
          this.AddRenderersMatchingName(renderers, this._owner, "hair");
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 2:
          this.AddRenderersContainingName(renderers, this._owner, "eye");
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000, new Vector2?(new Vector2(4f, 8f)));
        default:
          return (BaseCustomSkinPart) null;
      }
    }
  }
}
