// Decompiled with JetBrains decompiler
// Type: TitanSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class TitanSpawner
{
  public TitanSpawner()
  {
    this.name = string.Empty;
    this.location = new Vector3(0.0f, 0.0f, 0.0f);
    this.time = 30f;
    this.endless = false;
    this.delay = 30f;
  }

  public void resetTime() => this.time = this.delay;

  public float delay { get; set; }

  public bool endless { get; set; }

  public Vector3 location { get; set; }

  public string name { get; set; }

  public float time { get; set; }
}
