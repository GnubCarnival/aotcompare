// Decompiled with JetBrains decompiler
// Type: UIPopupList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : MonoBehaviour
{
  private const float animSpeed = 0.15f;
  public UIAtlas atlas;
  public Color backgroundColor = Color.white;
  public string backgroundSprite;
  public static UIPopupList current;
  public GameObject eventReceiver;
  public UIFont font;
  public string functionName = "OnSelectionChange";
  public Color highlightColor = new Color(0.5960785f, 1f, 0.2f, 1f);
  public string highlightSprite;
  public bool isAnimated = true;
  public bool isLocalized;
  public List<string> items = new List<string>();
  private UISprite mBackground;
  private float mBgBorder;
  private GameObject mChild;
  private UISprite mHighlight;
  private UILabel mHighlightedLabel;
  private List<UILabel> mLabelList = new List<UILabel>();
  private UIPanel mPanel;
  [HideInInspector]
  [SerializeField]
  private string mSelectedItem;
  public UIPopupList.OnSelectionChange onSelectionChange;
  public Vector2 padding = Vector2.op_Implicit(new Vector3(4f, 4f));
  public UIPopupList.Position position;
  public Color textColor = Color.white;
  public UILabel textLabel;
  public float textScale = 1f;

  private void Animate(UIWidget widget, bool placeAbove, float bottom)
  {
    this.AnimateColor(widget);
    this.AnimatePosition(widget, placeAbove, bottom);
  }

  private void AnimateColor(UIWidget widget)
  {
    Color color = widget.color;
    widget.color = new Color(color.r, color.g, color.b, 0.0f);
    TweenColor.Begin(((Component) widget).gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
  }

  private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
  {
    Vector3 localPosition = widget.cachedTransform.localPosition;
    Vector3 vector3 = !placeAbove ? new Vector3(localPosition.x, 0.0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
    widget.cachedTransform.localPosition = vector3;
    TweenPosition.Begin(((Component) widget).gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
  }

  private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
  {
    GameObject gameObject = ((Component) widget).gameObject;
    Transform cachedTransform = widget.cachedTransform;
    float num = (float) ((double) this.font.size * (double) this.textScale + (double) this.mBgBorder * 2.0);
    Vector3 localScale = cachedTransform.localScale;
    cachedTransform.localScale = new Vector3(localScale.x, num, localScale.z);
    TweenScale.Begin(gameObject, 0.15f, localScale).method = UITweener.Method.EaseOut;
    if (!placeAbove)
      return;
    Vector3 localPosition = cachedTransform.localPosition;
    cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - localScale.y + num, localPosition.z);
    TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
  }

  private void Highlight(UILabel lbl, bool instant)
  {
    if (!Object.op_Inequality((Object) this.mHighlight, (Object) null))
      return;
    TweenPosition component = ((Component) lbl).GetComponent<TweenPosition>();
    if (!Object.op_Equality((Object) component, (Object) null) && ((Behaviour) component).enabled)
      return;
    this.mHighlightedLabel = lbl;
    UIAtlas.Sprite atlasSprite = this.mHighlight.GetAtlasSprite();
    if (atlasSprite == null)
      return;
    float num1 = ((Rect) ref atlasSprite.inner).xMin - ((Rect) ref atlasSprite.outer).xMin;
    float num2 = ((Rect) ref atlasSprite.inner).yMin - ((Rect) ref atlasSprite.outer).yMin;
    Vector3 pos = Vector3.op_Addition(lbl.cachedTransform.localPosition, new Vector3(-num1, num2, 1f));
    if (instant || !this.isAnimated)
      this.mHighlight.cachedTransform.localPosition = pos;
    else
      TweenPosition.Begin(((Component) this.mHighlight).gameObject, 0.1f, pos).method = UITweener.Method.EaseOut;
  }

  private void OnClick()
  {
    if (Object.op_Equality((Object) this.mChild, (Object) null) && Object.op_Inequality((Object) this.atlas, (Object) null) && Object.op_Inequality((Object) this.font, (Object) null) && this.items.Count > 0)
    {
      this.mLabelList.Clear();
      this.handleEvents = true;
      if (Object.op_Equality((Object) this.mPanel, (Object) null))
        this.mPanel = UIPanel.Find(((Component) this).transform, true);
      Transform transform1 = ((Component) this).transform;
      Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(transform1.parent, transform1);
      this.mChild = new GameObject("Drop-down List");
      this.mChild.layer = ((Component) this).gameObject.layer;
      Transform transform2 = this.mChild.transform;
      transform2.parent = transform1.parent;
      transform2.localPosition = ((Bounds) ref relativeWidgetBounds).min;
      transform2.localRotation = Quaternion.identity;
      transform2.localScale = Vector3.one;
      this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
      this.mBackground.pivot = UIWidget.Pivot.TopLeft;
      this.mBackground.depth = NGUITools.CalculateNextDepth(((Component) this.mPanel).gameObject);
      this.mBackground.color = this.backgroundColor;
      Vector4 border = this.mBackground.border;
      this.mBgBorder = border.y;
      this.mBackground.cachedTransform.localPosition = new Vector3(0.0f, border.y, 0.0f);
      this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
      this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
      this.mHighlight.color = this.highlightColor;
      UIAtlas.Sprite atlasSprite = this.mHighlight.GetAtlasSprite();
      if (atlasSprite == null)
        return;
      float num1 = ((Rect) ref atlasSprite.inner).yMin - ((Rect) ref atlasSprite.outer).yMin;
      float num2 = (float) this.font.size * this.font.pixelSize * this.textScale;
      float num3 = 0.0f;
      float num4 = -this.padding.y;
      List<UILabel> uiLabelList = new List<UILabel>();
      int index1 = 0;
      for (int count = this.items.Count; index1 < count; ++index1)
      {
        string key = this.items[index1];
        UILabel lbl = NGUITools.AddWidget<UILabel>(this.mChild);
        lbl.pivot = UIWidget.Pivot.TopLeft;
        lbl.font = this.font;
        lbl.text = !this.isLocalized || Object.op_Equality((Object) Localization.instance, (Object) null) ? key : Localization.instance.Get(key);
        lbl.color = this.textColor;
        lbl.cachedTransform.localPosition = new Vector3(border.x + this.padding.x, num4, -1f);
        lbl.MakePixelPerfect();
        if ((double) this.textScale != 1.0)
        {
          Vector3 localScale = lbl.cachedTransform.localScale;
          lbl.cachedTransform.localScale = Vector3.op_Multiply(localScale, this.textScale);
        }
        uiLabelList.Add(lbl);
        num4 = num4 - num2 - this.padding.y;
        num3 = Mathf.Max(num3, lbl.relativeSize.x * num2);
        UIEventListener uiEventListener = UIEventListener.Get(((Component) lbl).gameObject);
        uiEventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
        uiEventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
        uiEventListener.parameter = (object) key;
        if (this.mSelectedItem == key)
          this.Highlight(lbl, true);
        this.mLabelList.Add(lbl);
      }
      float num5 = Mathf.Max(num3, ((Bounds) ref relativeWidgetBounds).size.x - (float) (((double) border.x + (double) this.padding.x) * 2.0));
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector(num5 * 0.5f / num2, -0.5f, 0.0f);
      Vector3 vector3_2;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_2).\u002Ector(num5 / num2, (num2 + this.padding.y) / num2, 1f);
      int index2 = 0;
      for (int count = uiLabelList.Count; index2 < count; ++index2)
      {
        BoxCollider boxCollider = NGUITools.AddWidgetCollider(((Component) uiLabelList[index2]).gameObject);
        vector3_1.z = boxCollider.center.z;
        boxCollider.center = vector3_1;
        boxCollider.size = vector3_2;
      }
      float num6 = num5 + (float) (((double) border.x + (double) this.padding.x) * 2.0);
      float num7 = num4 - border.y;
      this.mBackground.cachedTransform.localScale = new Vector3(num6, -num7 + border.y, 1f);
      this.mHighlight.cachedTransform.localScale = new Vector3((float) ((double) num6 - ((double) border.x + (double) this.padding.x) * 2.0 + ((double) ((Rect) ref atlasSprite.inner).xMin - (double) ((Rect) ref atlasSprite.outer).xMin) * 2.0), num2 + num1 * 2f, 1f);
      bool placeAbove = this.position == UIPopupList.Position.Above;
      if (this.position == UIPopupList.Position.Auto)
      {
        UICamera cameraForLayer = UICamera.FindCameraForLayer(((Component) this).gameObject.layer);
        if (Object.op_Inequality((Object) cameraForLayer, (Object) null))
          placeAbove = (double) cameraForLayer.cachedCamera.WorldToViewportPoint(transform1.position).y < 0.5;
      }
      if (this.isAnimated)
      {
        float bottom = num7 + num2;
        this.Animate((UIWidget) this.mHighlight, placeAbove, bottom);
        int index3 = 0;
        for (int count = uiLabelList.Count; index3 < count; ++index3)
          this.Animate((UIWidget) uiLabelList[index3], placeAbove, bottom);
        this.AnimateColor((UIWidget) this.mBackground);
        this.AnimateScale((UIWidget) this.mBackground, placeAbove, bottom);
      }
      if (!placeAbove)
        return;
      transform2.localPosition = new Vector3(((Bounds) ref relativeWidgetBounds).min.x, ((Bounds) ref relativeWidgetBounds).max.y - num7 - border.y, ((Bounds) ref relativeWidgetBounds).min.z);
    }
    else
      this.OnSelect(false);
  }

  private void OnItemHover(GameObject go, bool isOver)
  {
    if (!isOver)
      return;
    this.Highlight(go.GetComponent<UILabel>(), false);
  }

  private void OnItemPress(GameObject go, bool isPressed)
  {
    if (!isPressed)
      return;
    this.Select(go.GetComponent<UILabel>(), true);
  }

  private void OnKey(KeyCode key)
  {
    if (!((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || !this.handleEvents)
      return;
    int num1 = this.mLabelList.IndexOf(this.mHighlightedLabel);
    int num2;
    if (key == 273)
    {
      if (num1 <= 0)
        return;
      this.Select(this.mLabelList[num2 = num1 - 1], false);
    }
    else if (key == 274)
    {
      if (num1 + 1 >= this.mLabelList.Count)
        return;
      this.Select(this.mLabelList[num2 = num1 + 1], false);
    }
    else
    {
      if (key != 27)
        return;
      this.OnSelect(false);
    }
  }

  private void OnLocalize(Localization loc)
  {
    if (!this.isLocalized || !Object.op_Inequality((Object) this.textLabel, (Object) null))
      return;
    this.textLabel.text = loc.Get(this.mSelectedItem);
  }

  private void OnSelect(bool isSelected)
  {
    if (isSelected || !Object.op_Inequality((Object) this.mChild, (Object) null))
      return;
    this.mLabelList.Clear();
    this.handleEvents = false;
    if (this.isAnimated)
    {
      UIWidget[] componentsInChildren1 = this.mChild.GetComponentsInChildren<UIWidget>();
      int index1 = 0;
      for (int length = componentsInChildren1.Length; index1 < length; ++index1)
      {
        UIWidget uiWidget = componentsInChildren1[index1];
        Color color = uiWidget.color;
        color.a = 0.0f;
        TweenColor.Begin(((Component) uiWidget).gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
      }
      Collider[] componentsInChildren2 = this.mChild.GetComponentsInChildren<Collider>();
      int index2 = 0;
      for (int length = componentsInChildren2.Length; index2 < length; ++index2)
        componentsInChildren2[index2].enabled = false;
      Object.Destroy((Object) this.mChild, 0.15f);
    }
    else
      Object.Destroy((Object) this.mChild);
    this.mBackground = (UISprite) null;
    this.mHighlight = (UISprite) null;
    this.mChild = (GameObject) null;
  }

  private void Select(UILabel lbl, bool instant)
  {
    this.Highlight(lbl, instant);
    this.selection = ((Component) lbl).gameObject.GetComponent<UIEventListener>().parameter as string;
    UIButtonSound[] components = ((Component) this).GetComponents<UIButtonSound>();
    int index = 0;
    for (int length = components.Length; index < length; ++index)
    {
      UIButtonSound uiButtonSound = components[index];
      if (uiButtonSound.trigger == UIButtonSound.Trigger.OnClick)
        NGUITools.PlaySound(uiButtonSound.audioClip, uiButtonSound.volume, 1f);
    }
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) this.textLabel, (Object) null))
      return;
    if (string.IsNullOrEmpty(this.mSelectedItem))
    {
      if (this.items.Count <= 0)
        return;
      this.selection = this.items[0];
    }
    else
    {
      string mSelectedItem = this.mSelectedItem;
      this.mSelectedItem = (string) null;
      this.selection = mSelectedItem;
    }
  }

  private bool handleEvents
  {
    get
    {
      UIButtonKeys component = ((Component) this).GetComponent<UIButtonKeys>();
      return Object.op_Equality((Object) component, (Object) null) || !((Behaviour) component).enabled;
    }
    set
    {
      UIButtonKeys component = ((Component) this).GetComponent<UIButtonKeys>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      ((Behaviour) component).enabled = !value;
    }
  }

  public bool isOpen => Object.op_Inequality((Object) this.mChild, (Object) null);

  public string selection
  {
    get => this.mSelectedItem;
    set
    {
      if (!(this.mSelectedItem != value))
        return;
      this.mSelectedItem = value;
      if (Object.op_Inequality((Object) this.textLabel, (Object) null))
        this.textLabel.text = !this.isLocalized ? value : Localization.Localize(value);
      UIPopupList.current = this;
      if (this.onSelectionChange != null)
        this.onSelectionChange(this.mSelectedItem);
      if (Object.op_Inequality((Object) this.eventReceiver, (Object) null) && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
        this.eventReceiver.SendMessage(this.functionName, (object) this.mSelectedItem, (SendMessageOptions) 1);
      UIPopupList.current = (UIPopupList) null;
      if (!Object.op_Equality((Object) this.textLabel, (Object) null))
        return;
      this.mSelectedItem = (string) null;
    }
  }

  public delegate void OnSelectionChange(string item);

  public enum Position
  {
    Auto,
    Above,
    Below,
  }
}
