// Decompiled with JetBrains decompiler
// Type: XffectCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XffectCache : MonoBehaviour
{
  private Dictionary<string, ArrayList> ObjectDic = new Dictionary<string, ArrayList>();

  protected Transform AddObject(string name)
  {
    Transform transform1 = ((Component) this).transform.Find(name);
    if (Object.op_Equality((Object) transform1, (Object) null))
    {
      Debug.Log((object) ("object:" + name + "doesn't exist!"));
      return (Transform) null;
    }
    Transform transform2 = Object.Instantiate((Object) transform1, Vector3.zero, Quaternion.identity) as Transform;
    this.ObjectDic[name].Add((object) transform2);
    ((Component) transform2).gameObject.SetActive(false);
    Xffect component = ((Component) transform2).GetComponent<Xffect>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.Initialize();
    return transform2;
  }

  private void Awake()
  {
    foreach (Transform transform in ((Component) this).transform)
    {
      this.ObjectDic[((Object) transform).name] = new ArrayList();
      this.ObjectDic[((Object) transform).name].Add((object) transform);
      Xffect component = ((Component) transform).GetComponent<Xffect>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.Initialize();
      ((Component) transform).gameObject.SetActive(false);
    }
  }

  public Transform GetObject(string name)
  {
    ArrayList arrayList = this.ObjectDic[name];
    if (arrayList == null)
    {
      Debug.LogError((object) (name + ": cache doesnt exist!"));
      return (Transform) null;
    }
    foreach (Transform transform in arrayList)
    {
      if (!((Component) transform).gameObject.active)
      {
        ((Component) transform).gameObject.SetActive(true);
        return transform;
      }
    }
    return this.AddObject(name);
  }

  public ArrayList GetObjectCache(string name)
  {
    ArrayList objectCache = this.ObjectDic[name];
    if (objectCache != null)
      return objectCache;
    Debug.LogError((object) (name + ": cache doesnt exist!"));
    return (ArrayList) null;
  }

  private void Start()
  {
  }
}
