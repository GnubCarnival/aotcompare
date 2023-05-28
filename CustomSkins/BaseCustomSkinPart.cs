// Decompiled with JetBrains decompiler
// Type: CustomSkins.BaseCustomSkinPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomSkins
{
  internal class BaseCustomSkinPart
  {
    protected BaseCustomSkinLoader _loader;
    protected List<Renderer> _renderers;
    protected string _rendererId;
    protected int _maxSize;
    protected Vector2 _textureScale;
    protected readonly Vector2 _defaultTextureScale = new Vector2(1f, 1f);
    protected bool _useTransparentMaterial;

    public BaseCustomSkinPart(
      BaseCustomSkinLoader loader,
      List<Renderer> renderers,
      string rendererId,
      int maxSize,
      Vector2? textureScale = null,
      bool useTransparentMaterial = false)
    {
      this._loader = loader;
      this._renderers = renderers;
      this._rendererId = rendererId;
      this._maxSize = maxSize;
      this._textureScale = textureScale.HasValue ? textureScale.Value : this._defaultTextureScale;
      this._useTransparentMaterial = useTransparentMaterial;
    }

    public bool LoadCache(string url)
    {
      if (url.ToLower() == BaseCustomSkinLoader.TransparentURL)
      {
        this.DisableRenderers();
        return true;
      }
      if (!this.IsValidPart() || !TextureDownloader.ValidTextureURL(url))
        return true;
      if (!MaterialCache.ContainsKey(this._rendererId, url))
        return false;
      this.SetMaterial(MaterialCache.GetMaterial(this._rendererId, url));
      return true;
    }

    public IEnumerator LoadSkin(string url)
    {
      url = url.Trim();
      if (this.IsValidPart() && TextureDownloader.ValidTextureURL(url))
      {
        bool mipmap = SettingsManager.GraphicsSettings.MipmapEnabled.Value;
        CoroutineWithData cwd = new CoroutineWithData((MonoBehaviour) this._loader, TextureDownloader.DownloadTexture(this._loader, url, mipmap, this._maxSize));
        yield return (object) cwd.Coroutine;
        if (this.IsValidPart())
        {
          Material material = this.SetNewTexture((Texture2D) cwd.Result);
          MaterialCache.SetMaterial(this._rendererId, url, material);
        }
      }
    }

    protected virtual bool IsValidPart() => this._renderers.Count > 0 && Object.op_Inequality((Object) this._renderers[0], (Object) null);

    protected virtual void DisableRenderers()
    {
      if (this._useTransparentMaterial)
      {
        this.SetMaterial(MaterialCache.TransparentMaterial);
      }
      else
      {
        foreach (Renderer renderer in this._renderers)
          renderer.enabled = false;
      }
    }

    protected virtual void SetMaterial(Material material)
    {
      foreach (Renderer renderer in this._renderers)
        renderer.material = material;
    }

    protected virtual Material SetNewTexture(Texture2D texture)
    {
      this._renderers[0].material.mainTexture = (Texture) texture;
      if (Vector2.op_Inequality(this._textureScale, this._defaultTextureScale))
      {
        Vector2 mainTextureScale = this._renderers[0].material.mainTextureScale;
        this._renderers[0].material.mainTextureScale = new Vector2(mainTextureScale.x * this._textureScale.x, mainTextureScale.y * this._textureScale.y);
        this._renderers[0].material.mainTextureOffset = new Vector2(0.0f, 0.0f);
      }
      this.SetMaterial(this._renderers[0].material);
      return this._renderers[0].material;
    }
  }
}
