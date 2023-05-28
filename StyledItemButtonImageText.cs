// Decompiled with JetBrains decompiler
// Type: StyledItemButtonImageText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

public class StyledItemButtonImageText : StyledItem
{
  public Button buttonCtrl;
  public RawImage rawImageCtrl;
  public Text textCtrl;

  public override Button GetButton() => this.buttonCtrl;

  public override RawImage GetRawImage() => this.rawImageCtrl;

  public override Text GetText() => this.textCtrl;

  public override void Populate(object o)
  {
    Texture2D texture2D = o as Texture2D;
    if (Object.op_Inequality((Object) texture2D, (Object) null))
    {
      if (!Object.op_Inequality((Object) this.rawImageCtrl, (Object) null))
        return;
      this.rawImageCtrl.texture = (Texture) texture2D;
    }
    else if (!(o is StyledItemButtonImageText.Data data))
    {
      if (!Object.op_Inequality((Object) this.textCtrl, (Object) null))
        return;
      this.textCtrl.text = o.ToString();
    }
    else
    {
      if (Object.op_Inequality((Object) this.rawImageCtrl, (Object) null))
        this.rawImageCtrl.texture = (Texture) data.image;
      if (!Object.op_Inequality((Object) this.textCtrl, (Object) null))
        return;
      this.textCtrl.text = data.text;
    }
  }

  public class Data
  {
    public Texture2D image;
    public string text;

    public Data(string t, Texture2D tex)
    {
      this.text = t;
      this.image = tex;
    }
  }
}
