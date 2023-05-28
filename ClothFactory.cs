// Decompiled with JetBrains decompiler
// Type: ClothFactory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

public static class ClothFactory
{
  private static Dictionary<string, List<GameObject>> clothCache = new Dictionary<string, List<GameObject>>(CostumeHair.hairsF.Length);

  public static void ClearClothCache() => ClothFactory.clothCache.Clear();

  public static void DisposeObject(GameObject cachedObject)
  {
    if (!Object.op_Inequality((Object) cachedObject, (Object) null))
      return;
    ParentFollow component = cachedObject.GetComponent<ParentFollow>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      if (!component.isActiveInScene)
        return;
      ((Renderer) cachedObject.GetComponent<SkinnedMeshRenderer>()).enabled = false;
      cachedObject.GetComponent<Cloth>().enabled = false;
      component.isActiveInScene = false;
      cachedObject.transform.position = new Vector3(0.0f, -99999f, 0.0f);
      cachedObject.GetComponent<ParentFollow>().RemoveParent();
    }
    else
      Object.Destroy((Object) cachedObject);
  }

  private static GameObject GenerateCloth(GameObject go, string res)
  {
    if (Object.op_Equality((Object) go.GetComponent<SkinnedMeshRenderer>(), (Object) null))
      go.AddComponent<SkinnedMeshRenderer>();
    Transform[] bones = go.GetComponent<SkinnedMeshRenderer>().bones;
    SkinnedMeshRenderer component = ((GameObject) Object.Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
    ((Component) component).transform.localScale = Vector3.one;
    component.bones = bones;
    component.quality = (SkinQuality) 4;
    return ((Component) component).gameObject;
  }

  public static GameObject GetCape(GameObject reference, string name, Material material)
  {
    List<GameObject> gameObjectList1;
    if (ClothFactory.clothCache.TryGetValue(name, out gameObjectList1))
    {
      for (int index = 0; index < gameObjectList1.Count; ++index)
      {
        GameObject clothObject = gameObjectList1[index];
        if (Object.op_Equality((Object) clothObject, (Object) null))
        {
          gameObjectList1.RemoveAt(index);
          index = Mathf.Max(index - 1, 0);
        }
        else
        {
          ParentFollow component = clothObject.GetComponent<ParentFollow>();
          if (!component.isActiveInScene)
          {
            component.isActiveInScene = true;
            clothObject.renderer.material = material;
            clothObject.GetComponent<Cloth>().enabled = true;
            ((Renderer) clothObject.GetComponent<SkinnedMeshRenderer>()).enabled = true;
            clothObject.GetComponent<ParentFollow>().SetParent(reference.transform);
            ClothFactory.ReapplyClothBones(reference, clothObject);
            return clothObject;
          }
        }
      }
      GameObject cloth = ClothFactory.GenerateCloth(reference, name);
      cloth.renderer.material = material;
      cloth.AddComponent<ParentFollow>().SetParent(reference.transform);
      gameObjectList1.Add(cloth);
      ClothFactory.clothCache[name] = gameObjectList1;
      return cloth;
    }
    GameObject cloth1 = ClothFactory.GenerateCloth(reference, name);
    cloth1.renderer.material = material;
    cloth1.AddComponent<ParentFollow>().SetParent(reference.transform);
    List<GameObject> gameObjectList2 = new List<GameObject>()
    {
      cloth1
    };
    ClothFactory.clothCache.Add(name, gameObjectList2);
    return cloth1;
  }

  public static string GetDebugInfo()
  {
    int num1 = 0;
    foreach (KeyValuePair<string, List<GameObject>> keyValuePair in ClothFactory.clothCache)
      num1 += ClothFactory.clothCache[keyValuePair.Key].Count;
    int num2 = 0;
    foreach (Cloth cloth in Object.FindObjectsOfType<Cloth>())
    {
      if (cloth.enabled)
        ++num2;
    }
    return string.Format("{0} cached cloths, {1} active cloths, {2} types cached", (object) num1, (object) num2, (object) ClothFactory.clothCache.Keys.Count);
  }

  public static GameObject GetHair(
    GameObject reference,
    string name,
    Material material,
    Color color)
  {
    List<GameObject> gameObjectList1;
    if (ClothFactory.clothCache.TryGetValue(name, out gameObjectList1))
    {
      for (int index = 0; index < gameObjectList1.Count; ++index)
      {
        GameObject clothObject = gameObjectList1[index];
        if (Object.op_Equality((Object) clothObject, (Object) null))
        {
          gameObjectList1.RemoveAt(index);
          index = Mathf.Max(index - 1, 0);
        }
        else
        {
          ParentFollow component = clothObject.GetComponent<ParentFollow>();
          if (!component.isActiveInScene)
          {
            component.isActiveInScene = true;
            clothObject.renderer.material = material;
            clothObject.renderer.material.color = color;
            clothObject.GetComponent<Cloth>().enabled = true;
            ((Renderer) clothObject.GetComponent<SkinnedMeshRenderer>()).enabled = true;
            clothObject.GetComponent<ParentFollow>().SetParent(reference.transform);
            ClothFactory.ReapplyClothBones(reference, clothObject);
            return clothObject;
          }
        }
      }
      GameObject cloth = ClothFactory.GenerateCloth(reference, name);
      cloth.renderer.material = material;
      cloth.renderer.material.color = color;
      cloth.AddComponent<ParentFollow>().SetParent(reference.transform);
      gameObjectList1.Add(cloth);
      ClothFactory.clothCache[name] = gameObjectList1;
      return cloth;
    }
    GameObject cloth1 = ClothFactory.GenerateCloth(reference, name);
    cloth1.renderer.material = material;
    cloth1.renderer.material.color = color;
    cloth1.AddComponent<ParentFollow>().SetParent(reference.transform);
    List<GameObject> gameObjectList2 = new List<GameObject>()
    {
      cloth1
    };
    ClothFactory.clothCache.Add(name, gameObjectList2);
    return cloth1;
  }

  private static void ReapplyClothBones(GameObject reference, GameObject clothObject)
  {
    SkinnedMeshRenderer component1 = reference.GetComponent<SkinnedMeshRenderer>();
    SkinnedMeshRenderer component2 = clothObject.GetComponent<SkinnedMeshRenderer>();
    component2.bones = component1.bones;
    ((Component) component2).transform.localScale = Vector3.one;
  }
}
