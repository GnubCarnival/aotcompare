// Decompiled with JetBrains decompiler
// Type: UILocalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Localize")]
[RequireComponent(typeof (UIWidget))]
public class UILocalize : MonoBehaviour
{
  public string key;
  private string mLanguage;
  private bool mStarted;

  public void Localize()
  {
    Localization instance = Localization.instance;
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    UILabel uiLabel = component as UILabel;
    UISprite uiSprite = component as UISprite;
    if (string.IsNullOrEmpty(this.mLanguage) && string.IsNullOrEmpty(this.key) && Object.op_Inequality((Object) uiLabel, (Object) null))
      this.key = uiLabel.text;
    string str = !string.IsNullOrEmpty(this.key) ? instance.Get(this.key) : string.Empty;
    if (Object.op_Inequality((Object) uiLabel, (Object) null))
    {
      UIInput inParents = NGUITools.FindInParents<UIInput>(((Component) uiLabel).gameObject);
      if (Object.op_Inequality((Object) inParents, (Object) null) && Object.op_Equality((Object) inParents.label, (Object) uiLabel))
        inParents.defaultText = str;
      else
        uiLabel.text = str;
    }
    else if (Object.op_Inequality((Object) uiSprite, (Object) null))
    {
      uiSprite.spriteName = str;
      uiSprite.MakePixelPerfect();
    }
    this.mLanguage = instance.currentLanguage;
  }

  private void OnEnable()
  {
    if (!this.mStarted || !Object.op_Inequality((Object) Localization.instance, (Object) null))
      return;
    this.Localize();
  }

  private void OnLocalize(Localization loc)
  {
    if (!(this.mLanguage != loc.currentLanguage))
      return;
    this.Localize();
  }

  private void Start()
  {
    this.mStarted = true;
    if (!Object.op_Inequality((Object) Localization.instance, (Object) null))
      return;
    this.Localize();
  }
}
