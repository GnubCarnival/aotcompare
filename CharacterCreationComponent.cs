// Decompiled with JetBrains decompiler
// Type: CharacterCreationComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class CharacterCreationComponent : MonoBehaviour
{
  public GameObject manager;
  public CreatePart part;

  public void nextOption() => this.manager.GetComponent<CustomCharacterManager>().nextOption(this.part);

  public void prevOption() => this.manager.GetComponent<CustomCharacterManager>().prevOption(this.part);
}
