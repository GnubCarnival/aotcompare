// Decompiled with JetBrains decompiler
// Type: SetColorOnSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Set Color on Selection")]
[ExecuteInEditMode]
[RequireComponent(typeof (UIWidget))]
public class SetColorOnSelection : MonoBehaviour
{
  private UIWidget mWidget;

  private void OnSelectionChange(string val)
  {
    if (Object.op_Equality((Object) this.mWidget, (Object) null))
      this.mWidget = ((Component) this).GetComponent<UIWidget>();
    string key = val;
    if (key == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (SetColorOnSelection.fswitchSmap4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SetColorOnSelection.fswitchSmap4 = new Dictionary<string, int>(7)
      {
        {
          "White",
          0
        },
        {
          "Red",
          1
        },
        {
          "Green",
          2
        },
        {
          "Blue",
          3
        },
        {
          "Yellow",
          4
        },
        {
          "Cyan",
          5
        },
        {
          "Magenta",
          6
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!SetColorOnSelection.fswitchSmap4.TryGetValue(key, out num))
      return;
    switch (num)
    {
      case 0:
        this.mWidget.color = Color.white;
        break;
      case 1:
        this.mWidget.color = Color.red;
        break;
      case 2:
        this.mWidget.color = Color.green;
        break;
      case 3:
        this.mWidget.color = Color.blue;
        break;
      case 4:
        this.mWidget.color = Color.yellow;
        break;
      case 5:
        this.mWidget.color = Color.cyan;
        break;
      case 6:
        this.mWidget.color = Color.magenta;
        break;
    }
  }
}
