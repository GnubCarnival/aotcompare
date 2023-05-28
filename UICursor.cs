// Decompiled with JetBrains decompiler
// Type: UICursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[RequireComponent(typeof (UISprite))]
[AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
  private UIAtlas mAtlas;
  private static UICursor mInstance;
  private UISprite mSprite;
  private string mSpriteName;
  private Transform mTrans;
  public Camera uiCamera;

  private void Awake() => UICursor.mInstance = this;

  public static void Clear() => UICursor.Set(UICursor.mInstance.mAtlas, UICursor.mInstance.mSpriteName);

  private void OnDestroy() => UICursor.mInstance = (UICursor) null;

  public static void Set(UIAtlas atlas, string sprite)
  {
    if (!Object.op_Inequality((Object) UICursor.mInstance, (Object) null))
      return;
    UICursor.mInstance.mSprite.atlas = atlas;
    UICursor.mInstance.mSprite.spriteName = sprite;
    UICursor.mInstance.mSprite.MakePixelPerfect();
    UICursor.mInstance.Update();
  }

  private void Start()
  {
    this.mTrans = ((Component) this).transform;
    this.mSprite = ((Component) this).GetComponentInChildren<UISprite>();
    this.mAtlas = this.mSprite.atlas;
    this.mSpriteName = this.mSprite.spriteName;
    this.mSprite.depth = 100;
    if (!Object.op_Equality((Object) this.uiCamera, (Object) null))
      return;
    this.uiCamera = NGUITools.FindCameraForLayer(((Component) this).gameObject.layer);
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.mSprite.atlas, (Object) null))
      return;
    Vector3 mousePosition = Input.mousePosition;
    if (Object.op_Inequality((Object) this.uiCamera, (Object) null))
    {
      mousePosition.x = Mathf.Clamp01(mousePosition.x / (float) Screen.width);
      mousePosition.y = Mathf.Clamp01(mousePosition.y / (float) Screen.height);
      this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
      if (!this.uiCamera.isOrthoGraphic)
        return;
      this.mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(this.mTrans.localPosition, this.mTrans.localScale);
    }
    else
    {
      mousePosition.x -= (float) Screen.width * 0.5f;
      mousePosition.y -= (float) Screen.height * 0.5f;
      this.mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(mousePosition, this.mTrans.localScale);
    }
  }
}
