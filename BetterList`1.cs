// Decompiled with JetBrains decompiler
// Type: BetterList`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BetterList<T>
{
  public T[] buffer;
  public int size;

  public void Add(T item)
  {
    if (this.buffer == null || this.size == this.buffer.Length)
      this.AllocateMore();
    this.buffer[this.size++] = item;
  }

  private void AllocateMore()
  {
    T[] objArray = this.buffer == null ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
    if (this.buffer != null && this.size > 0)
      this.buffer.CopyTo((Array) objArray, 0);
    this.buffer = objArray;
  }

  public void Clear() => this.size = 0;

  public bool Contains(T item)
  {
    if (this.buffer != null)
    {
      for (int index = 0; index < this.size; ++index)
      {
        if (this.buffer[index].Equals((object) item))
          return true;
      }
    }
    return false;
  }

  [DebuggerHidden]
  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) new BetterList<T>.GetEnumeratorcIterator9<T>()
  {
    fthis = this
  };

  public void Insert(int index, T item)
  {
    if (this.buffer == null || this.size == this.buffer.Length)
      this.AllocateMore();
    if (index < this.size)
    {
      for (int size = this.size; size > index; --size)
        this.buffer[size] = this.buffer[size - 1];
      this.buffer[index] = item;
      ++this.size;
    }
    else
      this.Add(item);
  }

  public T Pop()
  {
    if (this.buffer == null || this.size == 0)
      return default (T);
    T obj = this.buffer[--this.size];
    this.buffer[this.size] = default (T);
    return obj;
  }

  public void Release()
  {
    this.size = 0;
    this.buffer = (T[]) null;
  }

  public bool Remove(T item)
  {
    if (this.buffer != null)
    {
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index1 = 0; index1 < this.size; ++index1)
      {
        if (equalityComparer.Equals(this.buffer[index1], item))
        {
          --this.size;
          this.buffer[index1] = default (T);
          for (int index2 = index1; index2 < this.size; ++index2)
            this.buffer[index2] = this.buffer[index2 + 1];
          return true;
        }
      }
    }
    return false;
  }

  public void RemoveAt(int index)
  {
    if (this.buffer == null || index >= this.size)
      return;
    --this.size;
    this.buffer[index] = default (T);
    for (int index1 = index; index1 < this.size; ++index1)
      this.buffer[index1] = this.buffer[index1 + 1];
  }

  public void Sort(Comparison<T> comparer)
  {
    bool flag = true;
    while (flag)
    {
      flag = false;
      for (int index = 1; index < this.size; ++index)
      {
        if (comparer(this.buffer[index - 1], this.buffer[index]) > 0)
        {
          T obj = this.buffer[index];
          this.buffer[index] = this.buffer[index - 1];
          this.buffer[index - 1] = obj;
          flag = true;
        }
      }
    }
  }

  public T[] ToArray()
  {
    this.Trim();
    return this.buffer;
  }

  private void Trim()
  {
    if (this.size > 0)
    {
      if (this.size >= this.buffer.Length)
        return;
      T[] objArray = new T[this.size];
      for (int index = 0; index < this.size; ++index)
        objArray[index] = this.buffer[index];
      this.buffer = objArray;
    }
    else
      this.buffer = (T[]) null;
  }

  public T this[int i]
  {
    get => this.buffer[i];
    set => this.buffer[i] = value;
  }
}
