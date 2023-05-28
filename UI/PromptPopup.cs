// Decompiled with JetBrains decompiler
// Type: UI.PromptPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace UI
{
  internal class PromptPopup : BasePopup
  {
    protected override float TopBarHeight => 55f;

    protected override float BottomBarHeight => 55f;

    protected override int TitleFontSize => 26;

    protected override int ButtonFontSize => 22;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      GameObject gameObject = ElementFactory.InstantiateAndBind(((Component) this).transform, "BackgroundDim");
      gameObject.AddComponent<IgnoreScaler>();
      gameObject.transform.SetSiblingIndex(0);
      this._staticTransforms.Add(gameObject.transform);
    }
  }
}
