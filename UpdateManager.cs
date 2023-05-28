// Decompiled with JetBrains decompiler
// Type: UpdateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Update Manager")]
[ExecuteInEditMode]
public class UpdateManager : MonoBehaviour
{
  private BetterList<UpdateManager.DestroyEntry> mDest = new BetterList<UpdateManager.DestroyEntry>();
  private static UpdateManager mInst;
  private List<UpdateManager.UpdateEntry> mOnCoro = new List<UpdateManager.UpdateEntry>();
  private List<UpdateManager.UpdateEntry> mOnLate = new List<UpdateManager.UpdateEntry>();
  private List<UpdateManager.UpdateEntry> mOnUpdate = new List<UpdateManager.UpdateEntry>();
  private float mTime;

  private void Add(
    MonoBehaviour mb,
    int updateOrder,
    UpdateManager.OnUpdate func,
    List<UpdateManager.UpdateEntry> list)
  {
    int index = 0;
    for (int count = list.Count; index < count; ++index)
    {
      if (list[index].func == func)
        return;
    }
    UpdateManager.UpdateEntry updateEntry = new UpdateManager.UpdateEntry()
    {
      index = updateOrder,
      func = func,
      mb = mb,
      isMonoBehaviour = Object.op_Inequality((Object) mb, (Object) null)
    };
    list.Add(updateEntry);
    if (updateOrder == 0)
      return;
    list.Sort(new Comparison<UpdateManager.UpdateEntry>(UpdateManager.Compare));
  }

  public static void AddCoroutine(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
  {
    UpdateManager.CreateInstance();
    UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnCoro);
  }

  public static void AddDestroy(Object obj, float delay)
  {
    if (!Object.op_Inequality(obj, (Object) null))
      return;
    if (Application.isPlaying)
    {
      if ((double) delay > 0.0)
      {
        UpdateManager.CreateInstance();
        UpdateManager.DestroyEntry destroyEntry = new UpdateManager.DestroyEntry()
        {
          obj = obj,
          time = Time.realtimeSinceStartup + delay
        };
        UpdateManager.mInst.mDest.Add(destroyEntry);
      }
      else
        Object.Destroy(obj);
    }
    else
      Object.DestroyImmediate(obj);
  }

  public static void AddLateUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
  {
    UpdateManager.CreateInstance();
    UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnLate);
  }

  public static void AddUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
  {
    UpdateManager.CreateInstance();
    UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnUpdate);
  }

  private static int Compare(UpdateManager.UpdateEntry a, UpdateManager.UpdateEntry b)
  {
    if (a.index < b.index)
      return 1;
    return a.index > b.index ? -1 : 0;
  }

  [DebuggerHidden]
  private IEnumerator CoroutineFunction() => (IEnumerator) new UpdateManager.CoroutineFunctioncIterator8()
  {
    fthis = this
  };

  private bool CoroutineUpdate()
  {
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    float delta = realtimeSinceStartup - this.mTime;
    if ((double) delta >= 1.0 / 1000.0)
    {
      this.mTime = realtimeSinceStartup;
      this.UpdateList(this.mOnCoro, delta);
      bool isPlaying = Application.isPlaying;
      int size = this.mDest.size;
      while (size > 0)
      {
        UpdateManager.DestroyEntry destroyEntry = this.mDest.buffer[--size];
        if (!isPlaying || (double) destroyEntry.time < (double) this.mTime)
        {
          if (Object.op_Inequality(destroyEntry.obj, (Object) null))
          {
            NGUITools.Destroy(destroyEntry.obj);
            destroyEntry.obj = (Object) null;
          }
          this.mDest.RemoveAt(size);
        }
      }
      if (this.mOnUpdate.Count == 0 && this.mOnLate.Count == 0 && this.mOnCoro.Count == 0 && this.mDest.size == 0)
      {
        NGUITools.Destroy((Object) ((Component) this).gameObject);
        return false;
      }
    }
    return true;
  }

  private static void CreateInstance()
  {
    if (!Object.op_Equality((Object) UpdateManager.mInst, (Object) null))
      return;
    UpdateManager.mInst = Object.FindObjectOfType(typeof (UpdateManager)) as UpdateManager;
    if (!Object.op_Equality((Object) UpdateManager.mInst, (Object) null) || !Application.isPlaying)
      return;
    GameObject gameObject = new GameObject("_UpdateManager");
    Object.DontDestroyOnLoad((Object) gameObject);
    UpdateManager.mInst = gameObject.AddComponent<UpdateManager>();
  }

  private void LateUpdate()
  {
    this.UpdateList(this.mOnLate, Time.deltaTime);
    if (Application.isPlaying)
      return;
    this.CoroutineUpdate();
  }

  private void OnApplicationQuit() => Object.DestroyImmediate((Object) ((Component) this).gameObject);

  private void Start()
  {
    if (!Application.isPlaying)
      return;
    this.mTime = Time.realtimeSinceStartup;
    this.StartCoroutine(this.CoroutineFunction());
  }

  private void Update()
  {
    if (Object.op_Inequality((Object) UpdateManager.mInst, (Object) this))
      NGUITools.Destroy((Object) ((Component) this).gameObject);
    else
      this.UpdateList(this.mOnUpdate, Time.deltaTime);
  }

  private void UpdateList(List<UpdateManager.UpdateEntry> list, float delta)
  {
    int count = list.Count;
    while (count > 0)
    {
      UpdateManager.UpdateEntry updateEntry = list[--count];
      if (updateEntry.isMonoBehaviour)
      {
        if (Object.op_Equality((Object) updateEntry.mb, (Object) null))
        {
          list.RemoveAt(count);
          continue;
        }
        if (!((Behaviour) updateEntry.mb).enabled || !NGUITools.GetActive(((Component) updateEntry.mb).gameObject))
          continue;
      }
      updateEntry.func(delta);
    }
  }

  public class DestroyEntry
  {
    public Object obj;
    public float time;
  }

  public delegate void OnUpdate(float delta);

  public class UpdateEntry
  {
    public UpdateManager.OnUpdate func;
    public int index;
    public bool isMonoBehaviour;
    public MonoBehaviour mb;
  }
}
