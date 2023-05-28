// Decompiled with JetBrains decompiler
// Type: CustomSkins.HookCustomSkinPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
  internal class HookCustomSkinPart : BaseCustomSkinPart
  {
    public Material HookMaterial;
    public bool Transparent;

    public HookCustomSkinPart(
      BaseCustomSkinLoader loader,
      string rendererId,
      int maxSize,
      Vector2? textureScale = null)
      : base(loader, (List<Renderer>) null, rendererId, maxSize, textureScale)
    {
    }

    protected override bool IsValidPart() => true;

    protected override void DisableRenderers() => this.Transparent = true;

    protected override void SetMaterial(Material material) => this.HookMaterial = material;

    protected override Material SetNewTexture(Texture2D texture)
    {
      Material material = new Material(Shader.Find("Transparent/Diffuse"));
      material.mainTexture = (Texture) texture;
      this.SetMaterial(material);
      return material;
    }
  }
}
