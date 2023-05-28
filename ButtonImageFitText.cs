// Decompiled with JetBrains decompiler
// Type: ButtonImageFitText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

public class ButtonImageFitText : MonoBehaviour
{
  public Image image;
  public Text text;

  private void Start() => MonoBehaviour.print((object) (this.text.flexibleWidth.ToString() + " " + (object) this.text.minWidth + " " + (object) this.text.preferredWidth));

  private void Update()
  {
  }
}
