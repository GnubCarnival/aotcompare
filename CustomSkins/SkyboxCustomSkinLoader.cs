// Decompiled with JetBrains decompiler
// Type: CustomSkins.SkyboxCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using UnityEngine;

namespace CustomSkins
{
  internal class SkyboxCustomSkinLoader : BaseCustomSkinLoader
  {
    public static Material SkyboxMaterial;

    protected override string RendererIdPrefix => "skybox";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      SkyboxCustomSkinLoader customSkinLoader = this;
      SkyboxCustomSkinLoader.SkyboxMaterial = new Material(Shader.Find("RenderFX/Skybox"));
      SkyboxCustomSkinLoader.SkyboxMaterial.CopyPropertiesFromMaterial(((Component) Camera.main).GetComponent<Skybox>().material);
      foreach (int customSkinPartId in customSkinLoader.GetCustomSkinPartIds(typeof (SkyboxCustomSkinPartId)))
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(customSkinPartId);
        string url = (string) data[customSkinPartId];
        if (!customSkinPart.LoadCache(url))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(url));
      }
    }

    protected override BaseCustomSkinPart GetCustomSkinPart(int partId) => (BaseCustomSkinPart) new SkyboxCustomSkinPart((BaseCustomSkinLoader) this, SkyboxCustomSkinLoader.SkyboxMaterial, this.PartIdToTextureName((SkyboxCustomSkinPartId) partId), this.GetRendererId(partId), 2000000);

    public string PartIdToTextureName(SkyboxCustomSkinPartId partId) => "_" + partId.ToString() + "Tex";
  }
}
