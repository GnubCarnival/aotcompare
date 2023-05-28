// Decompiled with JetBrains decompiler
// Type: UICamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("NGUI/UI/Camera")]
[ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
  public bool allowMultiTouch = true;
  public KeyCode cancelKey0 = (KeyCode) 27;
  public KeyCode cancelKey1 = (KeyCode) 331;
  public bool clipRaycasts = true;
  public static UICamera current = (UICamera) null;
  public static Camera currentCamera = (Camera) null;
  public static UICamera.MouseOrTouch currentTouch = (UICamera.MouseOrTouch) null;
  public static int currentTouchID = -1;
  public bool debug;
  public LayerMask eventReceiverMask = LayerMask.op_Implicit(-1);
  public static GameObject fallThrough;
  public static GameObject genericEventHandler;
  public string horizontalAxisName = "Horizontal";
  public static GameObject hoveredObject;
  public static bool inputHasFocus = false;
  public static bool isDragging = false;
  public static RaycastHit lastHit;
  public static Vector2 lastTouchPosition = Vector2.zero;
  private Camera mCam;
  private static UICamera.MouseOrTouch mController = new UICamera.MouseOrTouch();
  private static List<UICamera.Highlighted> mHighlighted = new List<UICamera.Highlighted>();
  private static GameObject mHover;
  private bool mIsEditor;
  private LayerMask mLayerMask;
  private static List<UICamera> mList = new List<UICamera>();
  private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[3]
  {
    new UICamera.MouseOrTouch(),
    new UICamera.MouseOrTouch(),
    new UICamera.MouseOrTouch()
  };
  private static float mNextEvent = 0.0f;
  public float mouseClickThreshold = 10f;
  public float mouseDragThreshold = 4f;
  private static GameObject mSel = (GameObject) null;
  private GameObject mTooltip;
  private float mTooltipTime;
  private static Dictionary<int, UICamera.MouseOrTouch> mTouches = new Dictionary<int, UICamera.MouseOrTouch>();
  public static UICamera.OnCustomInput onCustomInput;
  public float rangeDistance = -1f;
  public string scrollAxisName = "Mouse ScrollWheel";
  public static bool showTooltips = true;
  public bool stickyPress = true;
  public bool stickyTooltip = true;
  public KeyCode submitKey0 = (KeyCode) 13;
  public KeyCode submitKey1 = (KeyCode) 330;
  public float tooltipDelay = 1f;
  public float touchClickThreshold = 40f;
  public float touchDragThreshold = 40f;
  public bool useController = true;
  public bool useKeyboard = true;
  public bool useMouse = true;
  public bool useTouch = true;
  public string verticalAxisName = "Vertical";

  private void Awake()
  {
    this.cachedCamera.eventMask = 0;
    if (Application.platform == 11 || Application.platform == 8)
    {
      this.useMouse = false;
      this.useTouch = true;
      if (Application.platform == 8)
      {
        this.useKeyboard = false;
        this.useController = false;
      }
    }
    else if (Application.platform == 9 || Application.platform == 10)
    {
      this.useMouse = false;
      this.useTouch = false;
      this.useKeyboard = false;
      this.useController = true;
    }
    else if (Application.platform == 7 || Application.platform == null)
      this.mIsEditor = true;
    UICamera.mMouse[0].pos.x = Input.mousePosition.x;
    UICamera.mMouse[0].pos.y = Input.mousePosition.y;
    UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
    if (LayerMask.op_Implicit(this.eventReceiverMask) != -1)
      return;
    this.eventReceiverMask = LayerMask.op_Implicit(this.cachedCamera.cullingMask);
  }

  private static int CompareFunc(UICamera a, UICamera b)
  {
    if ((double) a.cachedCamera.depth < (double) b.cachedCamera.depth)
      return 1;
    return (double) a.cachedCamera.depth > (double) b.cachedCamera.depth ? -1 : 0;
  }

  public static UICamera FindCameraForLayer(int layer)
  {
    int num = 1 << layer;
    for (int index = 0; index < UICamera.mList.Count; ++index)
    {
      UICamera m = UICamera.mList[index];
      Camera cachedCamera = m.cachedCamera;
      if (Object.op_Inequality((Object) cachedCamera, (Object) null) && (cachedCamera.cullingMask & num) != 0)
        return m;
    }
    return (UICamera) null;
  }

  private void FixedUpdate()
  {
    if (!this.useMouse || !Application.isPlaying || !this.handlesEvents)
      return;
    UICamera.hoveredObject = !UICamera.Raycast(Input.mousePosition, ref UICamera.lastHit) ? UICamera.fallThrough : ((Component) ((RaycastHit) ref UICamera.lastHit).collider).gameObject;
    if (Object.op_Equality((Object) UICamera.hoveredObject, (Object) null))
      UICamera.hoveredObject = UICamera.genericEventHandler;
    for (int index = 0; index < 3; ++index)
      UICamera.mMouse[index].current = UICamera.hoveredObject;
  }

  private static int GetDirection(string axis)
  {
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    if ((double) UICamera.mNextEvent < (double) realtimeSinceStartup)
    {
      float axis1 = Input.GetAxis(axis);
      if ((double) axis1 > 0.75)
      {
        UICamera.mNextEvent = realtimeSinceStartup + 0.25f;
        return 1;
      }
      if ((double) axis1 < -0.75)
      {
        UICamera.mNextEvent = realtimeSinceStartup + 0.25f;
        return -1;
      }
    }
    return 0;
  }

  private static int GetDirection(KeyCode up, KeyCode down)
  {
    if (Input.GetKeyDown(up))
      return 1;
    return Input.GetKeyDown(down) ? -1 : 0;
  }

  private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
  {
    if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
      return 1;
    return !Input.GetKeyDown(down0) && !Input.GetKeyDown(down1) ? 0 : -1;
  }

  public static UICamera.MouseOrTouch GetTouch(int id)
  {
    UICamera.MouseOrTouch touch = (UICamera.MouseOrTouch) null;
    if (!UICamera.mTouches.TryGetValue(id, out touch))
    {
      touch = new UICamera.MouseOrTouch()
      {
        touchBegan = true
      };
      UICamera.mTouches.Add(id, touch);
    }
    return touch;
  }

  private static void Highlight(GameObject go, bool highlighted)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    int count = UICamera.mHighlighted.Count;
    while (count > 0)
    {
      UICamera.Highlighted highlighted1 = UICamera.mHighlighted[--count];
      if (highlighted1 == null || Object.op_Equality((Object) highlighted1.go, (Object) null))
        UICamera.mHighlighted.RemoveAt(count);
      else if (Object.op_Equality((Object) highlighted1.go, (Object) go))
      {
        if (highlighted)
        {
          ++highlighted1.counter;
          return;
        }
        if (--highlighted1.counter >= 1)
          return;
        UICamera.mHighlighted.Remove(highlighted1);
        UICamera.Notify(go, "OnHover", (object) false);
        return;
      }
    }
    if (!highlighted)
      return;
    UICamera.Highlighted highlighted2 = new UICamera.Highlighted()
    {
      go = go,
      counter = 1
    };
    UICamera.mHighlighted.Add(highlighted2);
    UICamera.Notify(go, "OnHover", (object) true);
  }

  public static bool IsHighlighted(GameObject go)
  {
    int count = UICamera.mHighlighted.Count;
    while (count > 0)
    {
      if (Object.op_Equality((Object) UICamera.mHighlighted[--count].go, (Object) go))
        return true;
    }
    return false;
  }

  private static bool IsVisible(ref RaycastHit hit)
  {
    UIPanel inParents = NGUITools.FindInParents<UIPanel>(((Component) ((RaycastHit) ref hit).collider).gameObject);
    return !Object.op_Inequality((Object) inParents, (Object) null) || inParents.IsVisible(((RaycastHit) ref hit).point);
  }

  public static void Notify(GameObject go, string funcName, object obj)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    go.SendMessage(funcName, obj, (SendMessageOptions) 1);
    if (!Object.op_Inequality((Object) UICamera.genericEventHandler, (Object) null) || !Object.op_Inequality((Object) UICamera.genericEventHandler, (Object) go))
      return;
    UICamera.genericEventHandler.SendMessage(funcName, obj, (SendMessageOptions) 1);
  }

  private void OnApplicationQuit() => UICamera.mHighlighted.Clear();

  private void OnDestroy() => UICamera.mList.Remove(this);

  public void ProcessMouse()
  {
    bool flag1 = this.useMouse && (double) Time.timeScale < 0.89999997615814209;
    if (!flag1)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (Input.GetMouseButton(index) || Input.GetMouseButtonUp(index))
        {
          flag1 = true;
          break;
        }
      }
    }
    UICamera.mMouse[0].pos = Vector2.op_Implicit(Input.mousePosition);
    UICamera.mMouse[0].delta = Vector2.op_Subtraction(UICamera.mMouse[0].pos, UICamera.lastTouchPosition);
    bool flag2 = Vector2.op_Inequality(UICamera.mMouse[0].pos, UICamera.lastTouchPosition);
    UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
    if (flag1)
    {
      UICamera.hoveredObject = !UICamera.Raycast(Input.mousePosition, ref UICamera.lastHit) ? UICamera.fallThrough : ((Component) ((RaycastHit) ref UICamera.lastHit).collider).gameObject;
      if (Object.op_Equality((Object) UICamera.hoveredObject, (Object) null))
        UICamera.hoveredObject = UICamera.genericEventHandler;
      UICamera.mMouse[0].current = UICamera.hoveredObject;
    }
    for (int index = 1; index < 3; ++index)
    {
      UICamera.mMouse[index].pos = UICamera.mMouse[0].pos;
      UICamera.mMouse[index].delta = UICamera.mMouse[0].delta;
      UICamera.mMouse[index].current = UICamera.mMouse[0].current;
    }
    bool flag3 = false;
    for (int index = 0; index < 3; ++index)
    {
      if (Input.GetMouseButton(index))
      {
        flag3 = true;
        break;
      }
    }
    if (flag3)
      this.mTooltipTime = 0.0f;
    else if (this.useMouse & flag2 && (!this.stickyTooltip || Object.op_Inequality((Object) UICamera.mHover, (Object) UICamera.mMouse[0].current)))
    {
      if ((double) this.mTooltipTime != 0.0)
        this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
      else if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
        this.ShowTooltip(false);
    }
    if (this.useMouse && !flag3 && Object.op_Inequality((Object) UICamera.mHover, (Object) null) && Object.op_Inequality((Object) UICamera.mHover, (Object) UICamera.mMouse[0].current))
    {
      if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
        this.ShowTooltip(false);
      UICamera.Highlight(UICamera.mHover, false);
      UICamera.mHover = (GameObject) null;
    }
    if (this.useMouse)
    {
      for (int index = 0; index < 3; ++index)
      {
        bool mouseButtonDown = Input.GetMouseButtonDown(index);
        bool mouseButtonUp = Input.GetMouseButtonUp(index);
        UICamera.currentTouch = UICamera.mMouse[index];
        UICamera.currentTouchID = -1 - index;
        if (mouseButtonDown)
          UICamera.currentTouch.pressedCam = UICamera.currentCamera;
        else if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) null))
          UICamera.currentCamera = UICamera.currentTouch.pressedCam;
        this.ProcessTouch(mouseButtonDown, mouseButtonUp);
      }
      UICamera.currentTouch = (UICamera.MouseOrTouch) null;
    }
    if (!this.useMouse || flag3 || !Object.op_Inequality((Object) UICamera.mHover, (Object) UICamera.mMouse[0].current))
      return;
    this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
    UICamera.mHover = UICamera.mMouse[0].current;
    UICamera.Highlight(UICamera.mHover, true);
  }

  public void ProcessOthers()
  {
    UICamera.currentTouchID = -100;
    UICamera.currentTouch = UICamera.mController;
    UICamera.inputHasFocus = Object.op_Inequality((Object) UICamera.mSel, (Object) null) && Object.op_Inequality((Object) UICamera.mSel.GetComponent<UIInput>(), (Object) null);
    bool pressed = this.submitKey0 != null && Input.GetKeyDown(this.submitKey0) || this.submitKey1 != null && Input.GetKeyDown(this.submitKey1);
    bool unpressed = this.submitKey0 != null && Input.GetKeyUp(this.submitKey0) || this.submitKey1 != null && Input.GetKeyUp(this.submitKey1);
    if (pressed | unpressed)
    {
      UICamera.currentTouch.current = UICamera.mSel;
      this.ProcessTouch(pressed, unpressed);
      UICamera.currentTouch.current = (GameObject) null;
    }
    int num1 = 0;
    int num2 = 0;
    if (this.useKeyboard)
    {
      if (UICamera.inputHasFocus)
      {
        num1 += UICamera.GetDirection((KeyCode) 273, (KeyCode) 274);
        num2 += UICamera.GetDirection((KeyCode) 275, (KeyCode) 276);
      }
      else
      {
        num1 += UICamera.GetDirection((KeyCode) 119, (KeyCode) 273, (KeyCode) 115, (KeyCode) 274);
        num2 += UICamera.GetDirection((KeyCode) 100, (KeyCode) 275, (KeyCode) 97, (KeyCode) 276);
      }
    }
    if (this.useController)
    {
      if (!string.IsNullOrEmpty(this.verticalAxisName))
        num1 += UICamera.GetDirection(this.verticalAxisName);
      if (!string.IsNullOrEmpty(this.horizontalAxisName))
        num2 += UICamera.GetDirection(this.horizontalAxisName);
    }
    if (num1 != 0)
      UICamera.Notify(UICamera.mSel, "OnKey", (object) (KeyCode) (num1 <= 0 ? 274 : 273));
    if (num2 != 0)
      UICamera.Notify(UICamera.mSel, "OnKey", (object) (KeyCode) (num2 <= 0 ? 276 : 275));
    if (this.useKeyboard && Input.GetKeyDown((KeyCode) 9))
      UICamera.Notify(UICamera.mSel, "OnKey", (object) (KeyCode) 9);
    if (this.cancelKey0 != null && Input.GetKeyDown(this.cancelKey0))
      UICamera.Notify(UICamera.mSel, "OnKey", (object) (KeyCode) 27);
    if (this.cancelKey1 != null && Input.GetKeyDown(this.cancelKey1))
      UICamera.Notify(UICamera.mSel, "OnKey", (object) (KeyCode) 27);
    UICamera.currentTouch = (UICamera.MouseOrTouch) null;
  }

  public void ProcessTouch(bool pressed, bool unpressed)
  {
    bool flag1 = UICamera.currentTouch == UICamera.mMouse[0] || UICamera.currentTouch == UICamera.mMouse[1] || UICamera.currentTouch == UICamera.mMouse[2];
    float num1 = !flag1 ? this.touchDragThreshold : this.mouseDragThreshold;
    float num2 = !flag1 ? this.touchClickThreshold : this.mouseClickThreshold;
    if (pressed)
    {
      if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
        this.ShowTooltip(false);
      UICamera.currentTouch.pressStarted = true;
      UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", (object) false);
      UICamera.currentTouch.pressed = UICamera.currentTouch.current;
      UICamera.currentTouch.dragged = UICamera.currentTouch.current;
      UICamera.currentTouch.clickNotification = !flag1 ? UICamera.ClickNotification.Always : UICamera.ClickNotification.BasedOnDelta;
      UICamera.currentTouch.totalDelta = Vector2.zero;
      UICamera.currentTouch.dragStarted = false;
      UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", (object) true);
      if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) UICamera.mSel))
      {
        if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
          this.ShowTooltip(false);
        UICamera.selectedObject = (GameObject) null;
      }
    }
    else
    {
      if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && !this.stickyPress && !unpressed && UICamera.currentTouch.pressStarted && Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) UICamera.hoveredObject))
      {
        UICamera.isDragging = true;
        UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", (object) false);
        UICamera.currentTouch.pressed = UICamera.hoveredObject;
        UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", (object) true);
        UICamera.isDragging = false;
      }
      if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) null) && (double) ((Vector2) ref UICamera.currentTouch.delta).magnitude != 0.0)
      {
        UICamera.MouseOrTouch currentTouch = UICamera.currentTouch;
        currentTouch.totalDelta = Vector2.op_Addition(currentTouch.totalDelta, UICamera.currentTouch.delta);
        float magnitude = ((Vector2) ref UICamera.currentTouch.totalDelta).magnitude;
        if (!UICamera.currentTouch.dragStarted && (double) num1 < (double) magnitude)
        {
          UICamera.currentTouch.dragStarted = true;
          UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
        }
        if (UICamera.currentTouch.dragStarted)
        {
          if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
            this.ShowTooltip(false);
          UICamera.isDragging = true;
          bool flag2 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
          UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", (object) UICamera.currentTouch.delta);
          UICamera.isDragging = false;
          if (flag2)
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
          else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && (double) num2 < (double) magnitude)
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
        }
      }
    }
    if (!unpressed)
      return;
    UICamera.currentTouch.pressStarted = false;
    if (Object.op_Inequality((Object) this.mTooltip, (Object) null))
      this.ShowTooltip(false);
    if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) null))
    {
      UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", (object) false);
      if (this.useMouse && Object.op_Equality((Object) UICamera.currentTouch.pressed, (Object) UICamera.mHover))
        UICamera.Notify(UICamera.currentTouch.pressed, "OnHover", (object) true);
      if (Object.op_Equality((Object) UICamera.currentTouch.dragged, (Object) UICamera.currentTouch.current) || UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && (double) ((Vector2) ref UICamera.currentTouch.totalDelta).magnitude < (double) num1)
      {
        if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) UICamera.mSel))
        {
          UICamera.mSel = UICamera.currentTouch.pressed;
          UICamera.Notify(UICamera.currentTouch.pressed, "OnSelect", (object) true);
        }
        else
          UICamera.mSel = UICamera.currentTouch.pressed;
        if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None)
        {
          float realtimeSinceStartup = Time.realtimeSinceStartup;
          UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", (object) null);
          if ((double) UICamera.currentTouch.clickTime + 0.34999999403953552 > (double) realtimeSinceStartup)
            UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", (object) null);
          UICamera.currentTouch.clickTime = realtimeSinceStartup;
        }
      }
      else
        UICamera.Notify(UICamera.currentTouch.current, "OnDrop", (object) UICamera.currentTouch.dragged);
    }
    UICamera.currentTouch.dragStarted = false;
    UICamera.currentTouch.pressed = (GameObject) null;
    UICamera.currentTouch.dragged = (GameObject) null;
  }

  public void ProcessTouches()
  {
    for (int index = 0; index < Input.touchCount; ++index)
    {
      Touch touch = Input.GetTouch(index);
      UICamera.currentTouchID = !this.allowMultiTouch ? 1 : ((Touch) ref touch).fingerId;
      UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
      bool pressed = ((Touch) ref touch).phase == null || UICamera.currentTouch.touchBegan;
      bool unpressed = ((Touch) ref touch).phase == 4 || ((Touch) ref touch).phase == 3;
      UICamera.currentTouch.touchBegan = false;
      UICamera.currentTouch.delta = !pressed ? Vector2.op_Subtraction(((Touch) ref touch).position, UICamera.currentTouch.pos) : Vector2.zero;
      UICamera.currentTouch.pos = ((Touch) ref touch).position;
      UICamera.hoveredObject = !UICamera.Raycast(Vector2.op_Implicit(UICamera.currentTouch.pos), ref UICamera.lastHit) ? UICamera.fallThrough : ((Component) ((RaycastHit) ref UICamera.lastHit).collider).gameObject;
      if (Object.op_Equality((Object) UICamera.hoveredObject, (Object) null))
        UICamera.hoveredObject = UICamera.genericEventHandler;
      UICamera.currentTouch.current = UICamera.hoveredObject;
      UICamera.lastTouchPosition = UICamera.currentTouch.pos;
      if (pressed)
        UICamera.currentTouch.pressedCam = UICamera.currentCamera;
      else if (Object.op_Inequality((Object) UICamera.currentTouch.pressed, (Object) null))
        UICamera.currentCamera = UICamera.currentTouch.pressedCam;
      if (((Touch) ref touch).tapCount > 1)
        UICamera.currentTouch.clickTime = Time.realtimeSinceStartup;
      this.ProcessTouch(pressed, unpressed);
      if (unpressed)
        UICamera.RemoveTouch(UICamera.currentTouchID);
      UICamera.currentTouch = (UICamera.MouseOrTouch) null;
      if (!this.allowMultiTouch)
        break;
    }
  }

  public static bool Raycast(Vector3 inPos, ref RaycastHit hit)
  {
    for (int index1 = 0; index1 < UICamera.mList.Count; ++index1)
    {
      UICamera m = UICamera.mList[index1];
      if (((Behaviour) m).enabled && NGUITools.GetActive(((Component) m).gameObject))
      {
        UICamera.currentCamera = m.cachedCamera;
        Vector3 viewportPoint = UICamera.currentCamera.ScreenToViewportPoint(inPos);
        if ((double) viewportPoint.x >= 0.0 && (double) viewportPoint.x <= 1.0 && (double) viewportPoint.y >= 0.0 && (double) viewportPoint.y <= 1.0)
        {
          Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
          int num1 = UICamera.currentCamera.cullingMask & LayerMask.op_Implicit(m.eventReceiverMask);
          float num2 = (double) m.rangeDistance <= 0.0 ? UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane : m.rangeDistance;
          if (m.clipRaycasts)
          {
            RaycastHit[] array = Physics.RaycastAll(ray, num2, num1);
            if (array.Length <= 1)
            {
              if (array.Length == 1 && UICamera.IsVisible(ref array[0]))
              {
                hit = array[0];
                return true;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (UICamera.famScache31 == null)
              {
                // ISSUE: reference to a compiler-generated field
                UICamera.famScache31 = (Comparison<RaycastHit>) ((r1, r2) => ((RaycastHit) ref r1).distance.CompareTo(((RaycastHit) ref r2).distance));
              }
              // ISSUE: reference to a compiler-generated field
              Array.Sort<RaycastHit>(array, UICamera.famScache31);
              int index2 = 0;
              for (int length = array.Length; index2 < length; ++index2)
              {
                if (UICamera.IsVisible(ref array[index2]))
                {
                  hit = array[index2];
                  return true;
                }
              }
            }
          }
          else if (Physics.Raycast(ray, ref hit, num2, num1))
            return true;
        }
      }
    }
    return false;
  }

  public static void RemoveTouch(int id) => UICamera.mTouches.Remove(id);

  public void ShowTooltip(bool val)
  {
    this.mTooltipTime = 0.0f;
    UICamera.Notify(this.mTooltip, "OnTooltip", (object) val);
    if (val)
      return;
    this.mTooltip = (GameObject) null;
  }

  private void Start()
  {
    UICamera.mList.Add(this);
    UICamera.mList.Sort(new Comparison<UICamera>(UICamera.CompareFunc));
  }

  private void Update()
  {
    if (!Application.isPlaying || !this.handlesEvents)
      return;
    UICamera.current = this;
    if (this.useMouse || this.useTouch && this.mIsEditor)
      this.ProcessMouse();
    if (this.useTouch)
      this.ProcessTouches();
    if (UICamera.onCustomInput != null)
      UICamera.onCustomInput();
    if (this.useMouse && Object.op_Inequality((Object) UICamera.mSel, (Object) null) && (this.cancelKey0 != null && Input.GetKeyDown(this.cancelKey0) || this.cancelKey1 != null && Input.GetKeyDown(this.cancelKey1)))
      UICamera.selectedObject = (GameObject) null;
    if (Object.op_Inequality((Object) UICamera.mSel, (Object) null))
    {
      string inputString = Input.inputString;
      if (this.useKeyboard && Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue))
        inputString += "\b";
      if (inputString.Length > 0)
      {
        if (!this.stickyTooltip && Object.op_Inequality((Object) this.mTooltip, (Object) null))
          this.ShowTooltip(false);
        UICamera.Notify(UICamera.mSel, "OnInput", (object) inputString);
      }
    }
    else
      UICamera.inputHasFocus = false;
    if (Object.op_Inequality((Object) UICamera.mSel, (Object) null))
      this.ProcessOthers();
    if (this.useMouse && Object.op_Inequality((Object) UICamera.mHover, (Object) null))
    {
      float axis = Input.GetAxis(this.scrollAxisName);
      if ((double) axis != 0.0)
        UICamera.Notify(UICamera.mHover, "OnScroll", (object) axis);
      if (UICamera.showTooltips && (double) this.mTooltipTime != 0.0 && ((double) this.mTooltipTime < (double) Time.realtimeSinceStartup || Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303)))
      {
        this.mTooltip = UICamera.mHover;
        this.ShowTooltip(true);
      }
    }
    UICamera.current = (UICamera) null;
  }

  public Camera cachedCamera
  {
    get
    {
      if (Object.op_Equality((Object) this.mCam, (Object) null))
        this.mCam = ((Component) this).camera;
      return this.mCam;
    }
  }

  public static int dragCount
  {
    get
    {
      int dragCount = 0;
      for (int key = 0; key < UICamera.mTouches.Count; ++key)
      {
        if (Object.op_Inequality((Object) UICamera.mTouches[key].dragged, (Object) null))
          ++dragCount;
      }
      for (int index = 0; index < UICamera.mMouse.Length; ++index)
      {
        if (Object.op_Inequality((Object) UICamera.mMouse[index].dragged, (Object) null))
          ++dragCount;
      }
      if (Object.op_Inequality((Object) UICamera.mController.dragged, (Object) null))
        ++dragCount;
      return dragCount;
    }
  }

  public static UICamera eventHandler
  {
    get
    {
      for (int index = 0; index < UICamera.mList.Count; ++index)
      {
        UICamera m = UICamera.mList[index];
        if (Object.op_Inequality((Object) m, (Object) null) && ((Behaviour) m).enabled && NGUITools.GetActive(((Component) m).gameObject))
          return m;
      }
      return (UICamera) null;
    }
  }

  private bool handlesEvents => Object.op_Equality((Object) UICamera.eventHandler, (Object) this);

  public static Camera mainCamera
  {
    get
    {
      UICamera eventHandler = UICamera.eventHandler;
      return !Object.op_Equality((Object) eventHandler, (Object) null) ? eventHandler.cachedCamera : (Camera) null;
    }
  }

  public static GameObject selectedObject
  {
    get => UICamera.mSel;
    set
    {
      if (!Object.op_Inequality((Object) UICamera.mSel, (Object) value))
        return;
      if (Object.op_Inequality((Object) UICamera.mSel, (Object) null))
      {
        UICamera cameraForLayer = UICamera.FindCameraForLayer(UICamera.mSel.layer);
        if (Object.op_Inequality((Object) cameraForLayer, (Object) null))
        {
          UICamera.current = cameraForLayer;
          UICamera.currentCamera = cameraForLayer.mCam;
          UICamera.Notify(UICamera.mSel, "OnSelect", (object) false);
          if (cameraForLayer.useController || cameraForLayer.useKeyboard)
            UICamera.Highlight(UICamera.mSel, false);
          UICamera.current = (UICamera) null;
        }
      }
      UICamera.mSel = value;
      if (!Object.op_Inequality((Object) UICamera.mSel, (Object) null))
        return;
      UICamera cameraForLayer1 = UICamera.FindCameraForLayer(UICamera.mSel.layer);
      if (!Object.op_Inequality((Object) cameraForLayer1, (Object) null))
        return;
      UICamera.current = cameraForLayer1;
      UICamera.currentCamera = cameraForLayer1.mCam;
      if (cameraForLayer1.useController || cameraForLayer1.useKeyboard)
        UICamera.Highlight(UICamera.mSel, true);
      UICamera.Notify(UICamera.mSel, "OnSelect", (object) true);
      UICamera.current = (UICamera) null;
    }
  }

  public static int touchCount
  {
    get
    {
      int touchCount = 0;
      for (int key = 0; key < UICamera.mTouches.Count; ++key)
      {
        if (Object.op_Inequality((Object) UICamera.mTouches[key].pressed, (Object) null))
          ++touchCount;
      }
      for (int index = 0; index < UICamera.mMouse.Length; ++index)
      {
        if (Object.op_Inequality((Object) UICamera.mMouse[index].pressed, (Object) null))
          ++touchCount;
      }
      if (Object.op_Inequality((Object) UICamera.mController.pressed, (Object) null))
        ++touchCount;
      return touchCount;
    }
  }

  public enum ClickNotification
  {
    None,
    Always,
    BasedOnDelta,
  }

  private class Highlighted
  {
    public int counter;
    public GameObject go;
  }

  public class MouseOrTouch
  {
    public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;
    public float clickTime;
    public GameObject current;
    public Vector2 delta;
    public GameObject dragged;
    public bool dragStarted;
    public Vector2 pos;
    public GameObject pressed;
    public Camera pressedCam;
    public bool pressStarted;
    public Vector2 totalDelta;
    public bool touchBegan = true;
  }

  public delegate void OnCustomInput();
}
