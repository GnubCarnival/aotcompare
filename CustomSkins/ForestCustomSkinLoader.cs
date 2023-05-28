// Decompiled with JetBrains decompiler
// Type: CustomSkins.ForestCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
  internal class ForestCustomSkinLoader : LevelCustomSkinLoader
  {
    private List<GameObject> _treeObjects = new List<GameObject>();
    private List<GameObject> _groundObjects = new List<GameObject>();

    protected override string RendererIdPrefix => "forest";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      ForestCustomSkinLoader customSkinLoader = this;
      customSkinLoader.FindAndIndexLevelObjects();
      char[] charArray = ((string) data[0]).ToCharArray();
      int[] trunkRandomIndices = customSkinLoader.SplitRandomIndices(charArray, 0);
      int[] leafRandomIndices = customSkinLoader.SplitRandomIndices(charArray, 1);
      string[] trunkUrls = ((string) data[1]).Split(',');
      string[] leafUrls = ((string) data[2]).Split(',');
      string groundUrl = leafUrls[8];
      for (int i = 0; i < customSkinLoader._treeObjects.Count; ++i)
      {
        int index1 = trunkRandomIndices[i];
        int index2 = leafRandomIndices[i];
        string url = trunkUrls[index1];
        string leafUrl = leafUrls[index2];
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(0, customSkinLoader._treeObjects[i]);
        BaseCustomSkinPart leafPart = customSkinLoader.GetCustomSkinPart(1, customSkinLoader._treeObjects[i]);
        if (!customSkinPart.LoadCache(url))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(url));
        if (!leafPart.LoadCache(leafUrl))
          yield return (object) customSkinLoader.StartCoroutine(leafPart.LoadSkin(leafUrl));
        leafUrl = (string) null;
        leafPart = (BaseCustomSkinPart) null;
      }
      foreach (GameObject groundObject in customSkinLoader._groundObjects)
      {
        BaseCustomSkinPart customSkinPart = customSkinLoader.GetCustomSkinPart(2, groundObject);
        if (!customSkinPart.LoadCache(groundUrl))
          yield return (object) customSkinLoader.StartCoroutine(customSkinPart.LoadSkin(groundUrl));
      }
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
    {
      List<Renderer> renderers = new List<Renderer>();
      switch (partId)
      {
        case 0:
          this.AddRenderersContainingName(renderers, levelObject, "Cube");
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 2000000);
        case 1:
          this.AddRenderersContainingName(renderers, levelObject, "Plane_031");
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 2:
          this.AddAllRenderers(renderers, levelObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        default:
          return (BaseCustomSkinPart) null;
      }
    }

    protected override void FindAndIndexLevelObjects()
    {
      this._treeObjects.Clear();
      this._groundObjects.Clear();
      foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
      {
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          if (((Object) gameObject).name.Contains("TREE"))
            this._treeObjects.Add(gameObject);
          else if (((Object) gameObject).name.Contains("Cube_001") && ((Component) gameObject.transform.parent).gameObject.tag != "Player")
            this._groundObjects.Add(gameObject);
        }
      }
    }

    private int[] SplitRandomIndices(char[] randomIndices, int offset)
    {
      List<int> source = new List<int>();
      for (int index = offset; index < randomIndices.Length; index += 2)
      {
        if (index < randomIndices.Length)
          source.Add(int.Parse(randomIndices[index].ToString()));
      }
      return source.ToArray<int>();
    }
  }
}
