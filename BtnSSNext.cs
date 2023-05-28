// Decompiled with JetBrains decompiler
// Type: BtnSSNext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BtnSSNext : MonoBehaviour
{
  private void OnClick()
  {
    if (Object.op_Inequality((Object) ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterCreationComponent>(), (Object) null))
      ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterCreationComponent>().nextOption();
    else
      ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterStatComponent>().nextOption();
  }
}
