// Decompiled with JetBrains decompiler
// Type: CustomSkins.SkyboxCustomSkinPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class SkyboxCustomSkinPart : BaseCustomSkinPart
  {
    private Material _skyboxMaterial;
    private string _textureName;

    public SkyboxCustomSkinPart(
      BaseCustomSkinLoader loader,
      Material skyboxMaterial,
      string textureName,
      string rendererId,
      int maxSize,
      Vector2? textureScale = null)
      : base(loader, (List<Renderer>) null, rendererId, maxSize, textureScale)
    {
      this._skyboxMaterial = skyboxMaterial;
      this._textureName = textureName;
    }

    protected override bool IsValidPart() => Object.op_Inequality((Object) this._skyboxMaterial, (Object) null);

    protected override void DisableRenderers()
    {
    }

    protected override void SetMaterial(Material material) => this._skyboxMaterial.SetTexture(this._textureName, material.GetTexture(this._textureName));

    protected override Material SetNewTexture(Texture2D texture)
    {
      ((Texture) texture).wrapMode = (TextureWrapMode) 1;
      Material material = new Material(Shader.Find("RenderFX/Skybox"));
      material.CopyPropertiesFromMaterial(this._skyboxMaterial);
      material.SetTexture(this._textureName, (Texture) texture);
      this.SetMaterial(material);
      return material;
    }
  }
}
