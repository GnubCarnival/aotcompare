// Decompiled with JetBrains decompiler
// Type: CustomSkins.ColossalCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class ColossalCustomSkinLoader : BaseCustomSkinLoader
  {
    protected override string RendererIdPrefix => "colossal";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      ColossalCustomSkinLoader customSkinLoader = this;
      string url = (string) data[0];
      BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(0);
      if (!customSkinPart.LoadCache(url))
        yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(url));
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
    {
      List<Renderer> renderers = new List<Renderer>();
      if (partId != 0)
        return (BaseCustomSkinPart) null;
      this.AddRenderersContainingName(renderers, this._owner, "hair");
      return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 2000000);
    }
  }
}
