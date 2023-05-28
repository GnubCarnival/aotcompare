// Decompiled with JetBrains decompiler
// Type: CustomSkins.BaseCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
  internal abstract class BaseCustomSkinLoader : MonoBehaviour
  {
    public static readonly string TransparentURL = "transparent";
    protected GameObject _owner;
    protected const int BytesPerKb = 1000;
    protected const int MaxSizeLarge = 2000000;
    protected const int MaxSizeMedium = 1000000;
    protected const int MaxSizeSmall = 500000;

    protected abstract string RendererIdPrefix { get; }

    protected void Awake() => this._owner = ((Component) this).gameObject;

    protected virtual BaseCustomSkinPart GetCustomSkinPart(int partId) => throw new NotImplementedException();

    public abstract IEnumerator LoadSkinsFromRPC(object[] data);

    protected string GetRendererId(int partId) => this.RendererIdPrefix + partId.ToString();

    protected void AddRendererIfExists(List<Renderer> renderers, GameObject obj)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null))
        return;
      renderers.Add(obj.renderer);
    }

    protected void AddAllRenderers(List<Renderer> renderers, GameObject obj)
    {
      foreach (Renderer componentsInChild in obj.GetComponentsInChildren<Renderer>())
        renderers.Add(componentsInChild);
    }

    protected void AddRenderersContainingName(
      List<Renderer> renderers,
      GameObject obj,
      string name)
    {
      foreach (Renderer componentsInChild in obj.GetComponentsInChildren<Renderer>())
      {
        if (((Object) componentsInChild).name.Contains(name))
          renderers.Add(componentsInChild);
      }
    }

    protected void AddRenderersMatchingName(List<Renderer> renderers, GameObject obj, string name)
    {
      foreach (Renderer componentsInChild in obj.GetComponentsInChildren<Renderer>())
      {
        if (((Object) componentsInChild).name == name)
          renderers.Add(componentsInChild);
      }
    }

    protected List<int> GetCustomSkinPartIds(Type t) => Enum.GetValues(t).Cast<int>().ToList<int>();

    private void OnDestroy() => TextureDownloader.ResetConcurrentDownloads();
  }
}
