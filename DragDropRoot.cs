// Decompiled with JetBrains decompiler
// Type: DragDropRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Root")]
public class DragDropRoot : MonoBehaviour
{
  public static Transform root;

  private void Awake() => DragDropRoot.root = ((Component) this).transform;
}
