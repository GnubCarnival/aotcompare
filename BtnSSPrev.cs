// Decompiled with JetBrains decompiler
// Type: BtnSSPrev
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BtnSSPrev : MonoBehaviour
{
  private void OnClick()
  {
    if (Object.op_Inequality((Object) ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterCreationComponent>(), (Object) null))
      ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterCreationComponent>().prevOption();
    else
      ((Component) ((Component) this).gameObject.transform.parent).gameObject.GetComponent<CharacterStatComponent>().prevOption();
  }
}
