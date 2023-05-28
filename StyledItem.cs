// Decompiled with JetBrains decompiler
// Type: StyledItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

public class StyledItem : MonoBehaviour
{
  public virtual Button GetButton() => (Button) null;

  public virtual Image GetImage() => (Image) null;

  public virtual RawImage GetRawImage() => (RawImage) null;

  public virtual Selectable GetSelectable() => (Selectable) null;

  public virtual Text GetText() => (Text) null;

  public virtual void Populate(object o)
  {
  }
}
