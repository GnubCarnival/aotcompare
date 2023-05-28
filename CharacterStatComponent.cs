// Decompiled with JetBrains decompiler
// Type: CharacterStatComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CharacterStatComponent : MonoBehaviour
{
  public GameObject manager;
  public CreateStat type;

  public void nextOption() => this.manager.GetComponent<CustomCharacterManager>().nextStatOption(this.type);

  public void prevOption() => this.manager.GetComponent<CustomCharacterManager>().prevStatOption(this.type);
}
