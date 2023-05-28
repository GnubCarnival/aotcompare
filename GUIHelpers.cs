// Decompiled with JetBrains decompiler
// Type: GUIHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public static class GUIHelpers
{
  public static Rect AlignRect(float width, float height, GUIHelpers.Alignment alignment) => GUIHelpers.AlignRect(width, height, new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), alignment, 0.0f, 0.0f);

  public static Rect AlignRect(
    float width,
    float height,
    Rect parentRect,
    GUIHelpers.Alignment alignment)
  {
    return GUIHelpers.AlignRect(width, height, parentRect, alignment, 0.0f, 0.0f);
  }

  public static Rect AlignRect(
    float width,
    float height,
    GUIHelpers.Alignment alignment,
    float xOffset,
    float yOffset)
  {
    return GUIHelpers.AlignRect(width, height, new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), alignment, xOffset, yOffset);
  }

  public static Rect AlignRect(
    float width,
    float height,
    Rect parentRect,
    GUIHelpers.Alignment alignment,
    float xOffset,
    float yOffset)
  {
    Rect rect;
    switch (alignment)
    {
      case GUIHelpers.Alignment.TOPLEFT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(0.0f, 0.0f, width, height);
        break;
      case GUIHelpers.Alignment.TOPCENTER:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector((float) ((double) ((Rect) ref parentRect).width * 0.5 - (double) width * 0.5), 0.0f, width, height);
        break;
      case GUIHelpers.Alignment.TOPRIGHT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(((Rect) ref parentRect).width - width, 0.0f, width, height);
        break;
      case GUIHelpers.Alignment.RIGHT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(((Rect) ref parentRect).width - width, (float) ((double) ((Rect) ref parentRect).height * 0.5 - (double) height * 0.5), width, height);
        break;
      case GUIHelpers.Alignment.BOTTOMRIGHT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(((Rect) ref parentRect).width - width, ((Rect) ref parentRect).height - height, width, height);
        break;
      case GUIHelpers.Alignment.BOTTOMCENTER:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector((float) ((double) ((Rect) ref parentRect).width * 0.5 - (double) width * 0.5), ((Rect) ref parentRect).height - height, width, height);
        break;
      case GUIHelpers.Alignment.BOTTOMLEFT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(0.0f, ((Rect) ref parentRect).y + ((Rect) ref parentRect).height - height, width, height);
        break;
      case GUIHelpers.Alignment.LEFT:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(0.0f, (float) ((double) ((Rect) ref parentRect).height * 0.5 - (double) height * 0.5), width, height);
        break;
      case GUIHelpers.Alignment.CENTER:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector((float) ((double) ((Rect) ref parentRect).width * 0.5 - (double) width * 0.5), (float) ((double) ((Rect) ref parentRect).height * 0.5 - (double) height * 0.5), width, height);
        break;
      default:
        // ISSUE: explicit constructor call
        ((Rect) ref rect).\u002Ector(0.0f, 0.0f, width, height);
        break;
    }
    ref Rect local1 = ref rect;
    ((Rect) ref local1).x = ((Rect) ref local1).x + (((Rect) ref parentRect).x + xOffset);
    ref Rect local2 = ref rect;
    ((Rect) ref local2).y = ((Rect) ref local2).y + (((Rect) ref parentRect).y + yOffset);
    return rect;
  }

  public static Rect ClampPosition(this Rect r, Rect borderRect) => new Rect(Mathf.Clamp(((Rect) ref r).x, ((Rect) ref borderRect).x, ((Rect) ref borderRect).x + ((Rect) ref borderRect).width - ((Rect) ref r).width), Mathf.Clamp(((Rect) ref r).y, ((Rect) ref borderRect).y, ((Rect) ref borderRect).y + ((Rect) ref borderRect).height - ((Rect) ref r).height), ((Rect) ref r).width, ((Rect) ref r).height);

  public static Vector2 FixedTouchDelta(this Touch aTouch)
  {
    float f = Time.deltaTime / ((Touch) ref aTouch).deltaTime;
    if ((double) f == 0.0 || float.IsNaN(f) || float.IsInfinity(f))
      f = 1f;
    return Vector2.op_Multiply(((Touch) ref aTouch).deltaPosition, f);
  }

  public static Vector2 FlipY(Vector2 inPos)
  {
    inPos.y = (float) Screen.height - inPos.y;
    return inPos;
  }

  public static Vector2 GetGUIPosition(this Touch aTouch)
  {
    Vector2 position = ((Touch) ref aTouch).position;
    position.y = (float) Screen.height - position.y;
    return position;
  }

  public static bool GetKeyDown(this Event aEvent, KeyCode aKey) => aEvent.type == 4 && aEvent.keyCode == aKey;

  public static bool GetKeyUp(this Event aEvent, KeyCode aKey) => aEvent.type == 5 && aEvent.keyCode == aKey;

  public static bool GetMouseDown(this Event aEvent, int aButton) => aEvent.type == null && aEvent.button == aButton;

  public static bool GetMouseDown(this Event aEvent, int aButton, Rect aRect) => aEvent.type == null && aEvent.button == aButton && ((Rect) ref aRect).Contains(aEvent.mousePosition);

  public static bool GetMouseUp(this Event aEvent, int aButton) => aEvent.type == 1 && aEvent.button == aButton;

  public static bool GetMouseUp(this Event aEvent, int aButton, Rect aRect) => aEvent.type == 1 && aEvent.button == aButton && ((Rect) ref aRect).Contains(aEvent.mousePosition);

  public static Rect Grow(this Rect r, float nbPixels) => r.Shrink(-nbPixels, -nbPixels);

  public static Rect Grow(this Rect r, float nbPixelX, float nbPixelY) => r.Shrink(-nbPixelX, -nbPixelY);

  public static Rect InverseTransform(this Rect r, Rect from) => new Rect(((Rect) ref r).x + ((Rect) ref from).x, ((Rect) ref r).y + ((Rect) ref from).y, ((Rect) ref r).width, ((Rect) ref r).height);

  public static Vector2 InverseTransformPoint(Rect rect, Vector3 inPos) => new Vector2(((Rect) ref rect).x + inPos.x, ((Rect) ref rect).y + inPos.y);

  public static Vector2 MouseRelativePos(Rect rect) => GUIHelpers.RelativePos(rect, GUIHelpers.mousePos.x, GUIHelpers.mousePos.y);

  public static Rect Move(this Rect r, Vector2 movement) => r.Move(movement.x, movement.y);

  public static Rect Move(this Rect r, float xMovement, float yMovement) => new Rect(((Rect) ref r).x + xMovement, ((Rect) ref r).y + yMovement, ((Rect) ref r).width, ((Rect) ref r).height);

  public static Rect MoveX(this Rect r, float xMovement) => r.Move(xMovement, 0.0f);

  public static Rect MoveY(this Rect r, float yMovement) => r.Move(0.0f, yMovement);

  public static Vector2 RelativePos(Rect rect, Vector2 inPos) => GUIHelpers.RelativePos(rect, inPos.x, inPos.y);

  public static Vector2 RelativePos(Rect rect, Vector3 inPos) => GUIHelpers.RelativePos(rect, inPos.x, inPos.y);

  public static Vector2 RelativePos(Rect rect, float x, float y) => new Vector2(x - ((Rect) ref rect).x, y - ((Rect) ref rect).y);

  public static Rect RelativeTo(this Rect r, Rect to) => new Rect(((Rect) ref r).x - ((Rect) ref to).x, ((Rect) ref r).y - ((Rect) ref to).y, ((Rect) ref r).width, ((Rect) ref r).height);

  public static Rect Shrink(this Rect r, float nbPixels) => r.Shrink(nbPixels, nbPixels);

  public static Rect Shrink(this Rect r, float nbPixelX, float nbPixelY) => new Rect(((Rect) ref r).x + nbPixelX, ((Rect) ref r).y + nbPixelY, ((Rect) ref r).width - nbPixelX * 2f, ((Rect) ref r).height - nbPixelY * 2f);

  public static Vector2 mousePos => Event.current.mousePosition;

  public static Vector2 mousePosInvertY => GUIHelpers.FlipY(GUIHelpers.mousePos);

  public static Rect screenRect => new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height);

  public enum Alignment
  {
    TOPLEFT,
    TOPCENTER,
    TOPRIGHT,
    RIGHT,
    BOTTOMRIGHT,
    BOTTOMCENTER,
    BOTTOMLEFT,
    LEFT,
    CENTER,
  }
}
