// Decompiled with JetBrains decompiler
// Type: OnClickLoadSomething
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class OnClickLoadSomething : MonoBehaviour
{
  public string ResourceToLoad;
  public OnClickLoadSomething.ResourceTypeOption ResourceTypeToLoad;

  public void OnClick()
  {
    switch (this.ResourceTypeToLoad)
    {
      case OnClickLoadSomething.ResourceTypeOption.Scene:
        Application.LoadLevel(this.ResourceToLoad);
        break;
      case OnClickLoadSomething.ResourceTypeOption.Web:
        Application.OpenURL(this.ResourceToLoad);
        break;
    }
  }

  public enum ResourceTypeOption : byte
  {
    Scene,
    Web,
  }
}
