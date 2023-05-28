// Decompiled with JetBrains decompiler
// Type: CustomSkins.WeaponTrailCustomSkinPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;
using Xft;

namespace CustomSkins
{
  internal class WeaponTrailCustomSkinPart : BaseCustomSkinPart
  {
    private List<XWeaponTrail> _weaponTrails;

    public WeaponTrailCustomSkinPart(
      BaseCustomSkinLoader loader,
      List<XWeaponTrail> weaponTrails,
      string rendererId,
      int maxSize,
      Vector2? textureScale = null)
      : base(loader, (List<Renderer>) null, rendererId, maxSize, textureScale)
    {
      this._weaponTrails = weaponTrails;
    }

    protected override bool IsValidPart() => this._weaponTrails.Count > 0 && Object.op_Inequality((Object) this._weaponTrails[0], (Object) null);

    protected override void DisableRenderers()
    {
      foreach (Behaviour weaponTrail in this._weaponTrails)
        weaponTrail.enabled = false;
    }

    protected override void SetMaterial(Material material)
    {
      foreach (XWeaponTrail weaponTrail in this._weaponTrails)
        weaponTrail.MyMaterial = material;
    }

    protected override Material SetNewTexture(Texture2D texture)
    {
      this._weaponTrails[0].MyMaterial.mainTexture = (Texture) texture;
      if (Vector2.op_Inequality(this._textureScale, this._defaultTextureScale))
      {
        Vector2 mainTextureScale = this._weaponTrails[0].MyMaterial.mainTextureScale;
        this._weaponTrails[0].MyMaterial.mainTextureScale = new Vector2(mainTextureScale.x * this._textureScale.x, mainTextureScale.y * this._textureScale.y);
      }
      this.SetMaterial(this._weaponTrails[0].MyMaterial);
      return this._weaponTrails[0].MyMaterial;
    }
  }
}
