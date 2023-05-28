// Decompiled with JetBrains decompiler
// Type: NGUIMath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public static class NGUIMath
{
  public static Vector3 ApplyHalfPixelOffset(Vector3 pos)
  {
    RuntimePlatform platform = Application.platform;
    if (platform <= 5)
    {
      if (platform != 2 && platform != 5)
        goto label_4;
    }
    else if (platform != 7 && platform != 10)
      goto label_4;
    pos.x -= 0.5f;
    pos.y += 0.5f;
label_4:
    return pos;
  }

  public static Vector3 ApplyHalfPixelOffset(Vector3 pos, Vector3 scale)
  {
    RuntimePlatform platform = Application.platform;
    if (platform <= 5)
    {
      if (platform != 2 && platform != 5)
        goto label_7;
    }
    else if (platform != 7 && platform != 10)
      goto label_7;
    if (Mathf.RoundToInt(scale.x) == Mathf.RoundToInt(scale.x * 0.5f) * 2)
      pos.x -= 0.5f;
    if (Mathf.RoundToInt(scale.y) == Mathf.RoundToInt(scale.y * 0.5f) * 2)
      pos.y += 0.5f;
label_7:
    return pos;
  }

  public static Bounds CalculateAbsoluteWidgetBounds(Transform trans)
  {
    UIWidget[] componentsInChildren = ((Component) trans).GetComponentsInChildren<UIWidget>();
    if (componentsInChildren.Length == 0)
      return new Bounds(trans.position, Vector3.zero);
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(float.MaxValue, float.MaxValue, float.MaxValue);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(float.MinValue, float.MinValue, float.MinValue);
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
    {
      UIWidget uiWidget = componentsInChildren[index];
      Vector2 relativeSize = uiWidget.relativeSize;
      Vector2 pivotOffset = uiWidget.pivotOffset;
      float num1 = (pivotOffset.x + 0.5f) * relativeSize.x;
      float num2 = (pivotOffset.y - 0.5f) * relativeSize.y;
      Vector2 vector2 = Vector2.op_Multiply(relativeSize, 0.5f);
      Transform cachedTransform = uiWidget.cachedTransform;
      Vector3 vector3_3 = cachedTransform.TransformPoint(new Vector3(num1 - vector2.x, num2 - vector2.y, 0.0f));
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
      Vector3 vector3_4 = Vector3.Min(vector3_3, vector3_1);
      Vector3 vector3_5 = cachedTransform.TransformPoint(new Vector3(num1 - vector2.x, num2 + vector2.y, 0.0f));
      vector3_2 = Vector3.Max(vector3_5, vector3_2);
      Vector3 vector3_6 = Vector3.Min(vector3_5, vector3_4);
      Vector3 vector3_7 = cachedTransform.TransformPoint(new Vector3(num1 + vector2.x, num2 - vector2.y, 0.0f));
      vector3_2 = Vector3.Max(vector3_7, vector3_2);
      Vector3 vector3_8 = Vector3.Min(vector3_7, vector3_6);
      Vector3 vector3_9 = cachedTransform.TransformPoint(new Vector3(num1 + vector2.x, num2 + vector2.y, 0.0f));
      vector3_2 = Vector3.Max(vector3_9, vector3_2);
      vector3_1 = Vector3.Min(vector3_9, vector3_8);
    }
    Bounds absoluteWidgetBounds;
    // ISSUE: explicit constructor call
    ((Bounds) ref absoluteWidgetBounds).\u002Ector(vector3_1, Vector3.zero);
    ((Bounds) ref absoluteWidgetBounds).Encapsulate(vector3_2);
    return absoluteWidgetBounds;
  }

  public static Bounds CalculateRelativeInnerBounds(Transform root, UISprite sprite)
  {
    if (sprite.type != UISprite.Type.Sliced)
      return NGUIMath.CalculateRelativeWidgetBounds(root, sprite.cachedTransform);
    Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
    Vector2 relativeSize = sprite.relativeSize;
    Vector2 pivotOffset = sprite.pivotOffset;
    Transform cachedTransform = sprite.cachedTransform;
    float num1 = (pivotOffset.x + 0.5f) * relativeSize.x;
    float num2 = (pivotOffset.y - 0.5f) * relativeSize.y;
    Vector2 vector2 = Vector2.op_Multiply(relativeSize, 0.5f);
    float x = cachedTransform.localScale.x;
    float y = cachedTransform.localScale.y;
    Vector4 border = sprite.border;
    if ((double) x != 0.0)
    {
      border.x /= x;
      border.z /= x;
    }
    if ((double) y != 0.0)
    {
      border.y /= y;
      border.w /= y;
    }
    float num3 = num1 - vector2.x + border.x;
    float num4 = num1 + vector2.x - border.z;
    float num5 = num2 - vector2.y + border.y;
    float num6 = num2 + vector2.y - border.w;
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(num3, num5, 0.0f);
    Vector3 vector3_2 = cachedTransform.TransformPoint(vector3_1);
    Vector3 vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_2);
    Bounds relativeInnerBounds;
    // ISSUE: explicit constructor call
    ((Bounds) ref relativeInnerBounds).\u002Ector(vector3_3, Vector3.zero);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_3).\u002Ector(num3, num6, 0.0f);
    Vector3 vector3_4 = cachedTransform.TransformPoint(vector3_3);
    Vector3 vector3_5 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_4);
    ((Bounds) ref relativeInnerBounds).Encapsulate(vector3_5);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_5).\u002Ector(num4, num6, 0.0f);
    Vector3 vector3_6 = cachedTransform.TransformPoint(vector3_5);
    Vector3 vector3_7 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_6);
    ((Bounds) ref relativeInnerBounds).Encapsulate(vector3_7);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_7).\u002Ector(num4, num5, 0.0f);
    Vector3 vector3_8 = cachedTransform.TransformPoint(vector3_7);
    Vector3 vector3_9 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_8);
    ((Bounds) ref relativeInnerBounds).Encapsulate(vector3_9);
    return relativeInnerBounds;
  }

  public static Bounds CalculateRelativeWidgetBounds(Transform trans) => NGUIMath.CalculateRelativeWidgetBounds(trans, trans);

  public static Bounds CalculateRelativeWidgetBounds(Transform root, Transform child)
  {
    UIWidget[] componentsInChildren = ((Component) child).GetComponentsInChildren<UIWidget>();
    if (componentsInChildren.Length == 0)
      return new Bounds(Vector3.zero, Vector3.zero);
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(float.MaxValue, float.MaxValue, float.MaxValue);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(float.MinValue, float.MinValue, float.MinValue);
    Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
    {
      UIWidget uiWidget = componentsInChildren[index];
      Vector2 relativeSize = uiWidget.relativeSize;
      Vector2 pivotOffset = uiWidget.pivotOffset;
      Transform cachedTransform = uiWidget.cachedTransform;
      float num1 = (pivotOffset.x + 0.5f) * relativeSize.x;
      float num2 = (pivotOffset.y - 0.5f) * relativeSize.y;
      Vector2 vector2 = Vector2.op_Multiply(relativeSize, 0.5f);
      Vector3 vector3_3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_3).\u002Ector(num1 - vector2.x, num2 - vector2.y, 0.0f);
      vector3_3 = cachedTransform.TransformPoint(vector3_3);
      vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_3);
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
      Vector3 vector3_4 = Vector3.Min(vector3_3, vector3_1);
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_3).\u002Ector(num1 - vector2.x, num2 + vector2.y, 0.0f);
      vector3_3 = cachedTransform.TransformPoint(vector3_3);
      vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_3);
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
      Vector3 vector3_5 = Vector3.Min(vector3_3, vector3_4);
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_3).\u002Ector(num1 + vector2.x, num2 - vector2.y, 0.0f);
      vector3_3 = cachedTransform.TransformPoint(vector3_3);
      vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_3);
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
      Vector3 vector3_6 = Vector3.Min(vector3_3, vector3_5);
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_3).\u002Ector(num1 + vector2.x, num2 + vector2.y, 0.0f);
      vector3_3 = cachedTransform.TransformPoint(vector3_3);
      vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(vector3_3);
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
      vector3_1 = Vector3.Min(vector3_3, vector3_6);
    }
    Bounds relativeWidgetBounds;
    // ISSUE: explicit constructor call
    ((Bounds) ref relativeWidgetBounds).\u002Ector(vector3_1, Vector3.zero);
    ((Bounds) ref relativeWidgetBounds).Encapsulate(vector3_2);
    return relativeWidgetBounds;
  }

  public static Vector3[] CalculateWidgetCorners(UIWidget w)
  {
    Vector2 relativeSize = w.relativeSize;
    Vector2 pivotOffset = w.pivotOffset;
    Vector4 relativePadding = w.relativePadding;
    float num1 = pivotOffset.x * relativeSize.x - relativePadding.x;
    float num2 = pivotOffset.y * relativeSize.y + relativePadding.y;
    float num3 = num1 + relativeSize.x + relativePadding.x + relativePadding.z;
    float num4 = num2 - relativeSize.y - relativePadding.y - relativePadding.w;
    Transform cachedTransform = w.cachedTransform;
    return new Vector3[4]
    {
      cachedTransform.TransformPoint(num1, num2, 0.0f),
      cachedTransform.TransformPoint(num1, num4, 0.0f),
      cachedTransform.TransformPoint(num3, num4, 0.0f),
      cachedTransform.TransformPoint(num3, num2, 0.0f)
    };
  }

  public static int ClampIndex(int val, int max)
  {
    if (val < 0)
      return 0;
    return val < max ? val : max - 1;
  }

  public static int ColorToInt(Color c) => 0 | Mathf.RoundToInt(c.r * (float) byte.MaxValue) << 24 | Mathf.RoundToInt(c.g * (float) byte.MaxValue) << 16 | Mathf.RoundToInt(c.b * (float) byte.MaxValue) << 8 | Mathf.RoundToInt(c.a * (float) byte.MaxValue);

  public static Vector2 ConstrainRect(
    Vector2 minRect,
    Vector2 maxRect,
    Vector2 minArea,
    Vector2 maxArea)
  {
    Vector2 zero = Vector2.zero;
    float num1 = maxRect.x - minRect.x;
    float num2 = maxRect.y - minRect.y;
    float num3 = maxArea.x - minArea.x;
    float num4 = maxArea.y - minArea.y;
    if ((double) num1 > (double) num3)
    {
      float num5 = num1 - num3;
      minArea.x -= num5;
      maxArea.x += num5;
    }
    if ((double) num2 > (double) num4)
    {
      float num6 = num2 - num4;
      minArea.y -= num6;
      maxArea.y += num6;
    }
    if ((double) minRect.x < (double) minArea.x)
      zero.x += minArea.x - minRect.x;
    if ((double) maxRect.x > (double) maxArea.x)
      zero.x -= maxRect.x - maxArea.x;
    if ((double) minRect.y < (double) minArea.y)
      zero.y += minArea.y - minRect.y;
    if ((double) maxRect.y > (double) maxArea.y)
      zero.y -= maxRect.y - maxArea.y;
    return zero;
  }

  public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
  {
    Rect pixels = rect;
    if (round)
    {
      ((Rect) ref pixels).xMin = (float) Mathf.RoundToInt(((Rect) ref rect).xMin * (float) width);
      ((Rect) ref pixels).xMax = (float) Mathf.RoundToInt(((Rect) ref rect).xMax * (float) width);
      ((Rect) ref pixels).yMin = (float) Mathf.RoundToInt((1f - ((Rect) ref rect).yMax) * (float) height);
      ((Rect) ref pixels).yMax = (float) Mathf.RoundToInt((1f - ((Rect) ref rect).yMin) * (float) height);
      return pixels;
    }
    ((Rect) ref pixels).xMin = ((Rect) ref rect).xMin * (float) width;
    ((Rect) ref pixels).xMax = ((Rect) ref rect).xMax * (float) width;
    ((Rect) ref pixels).yMin = (1f - ((Rect) ref rect).yMax) * (float) height;
    ((Rect) ref pixels).yMax = (1f - ((Rect) ref rect).yMin) * (float) height;
    return pixels;
  }

  public static Rect ConvertToTexCoords(Rect rect, int width, int height)
  {
    Rect texCoords = rect;
    if ((double) width != 0.0 && (double) height != 0.0)
    {
      ((Rect) ref texCoords).xMin = ((Rect) ref rect).xMin / (float) width;
      ((Rect) ref texCoords).xMax = ((Rect) ref rect).xMax / (float) width;
      ((Rect) ref texCoords).yMin = (float) (1.0 - (double) ((Rect) ref rect).yMax / (double) height);
      ((Rect) ref texCoords).yMax = (float) (1.0 - (double) ((Rect) ref rect).yMin / (double) height);
    }
    return texCoords;
  }

  public static string DecimalToHex(int num)
  {
    num &= 16777215;
    return num.ToString("X6");
  }

  public static char DecimalToHexChar(int num)
  {
    if (num > 15)
      return 'F';
    return num < 10 ? (char) (48 + num) : (char) (65 + num - 10);
  }

  private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
  {
    Vector2 vector2_1 = Vector2.op_Subtraction(b, a);
    float sqrMagnitude = ((Vector2) ref vector2_1).sqrMagnitude;
    if ((double) sqrMagnitude == 0.0)
    {
      Vector2 vector2_2 = Vector2.op_Subtraction(point, a);
      return ((Vector2) ref vector2_2).magnitude;
    }
    float num = Vector2.Dot(Vector2.op_Subtraction(point, a), Vector2.op_Subtraction(b, a)) / sqrMagnitude;
    if ((double) num < 0.0)
    {
      Vector2 vector2_3 = Vector2.op_Subtraction(point, a);
      return ((Vector2) ref vector2_3).magnitude;
    }
    if ((double) num > 1.0)
    {
      Vector2 vector2_4 = Vector2.op_Subtraction(point, b);
      return ((Vector2) ref vector2_4).magnitude;
    }
    Vector2 vector2_5 = Vector2.op_Addition(a, Vector2.op_Multiply(num, Vector2.op_Subtraction(b, a)));
    Vector2 vector2_6 = Vector2.op_Subtraction(point, vector2_5);
    return ((Vector2) ref vector2_6).magnitude;
  }

  public static float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
  {
    bool flag = false;
    int val1 = 4;
    for (int val2 = 0; val2 < 5; ++val2)
    {
      Vector3 vector3_1 = Vector2.op_Implicit(screenPoints[NGUIMath.RepeatIndex(val2, 4)]);
      Vector3 vector3_2 = Vector2.op_Implicit(screenPoints[NGUIMath.RepeatIndex(val1, 4)]);
      if ((double) vector3_1.y > (double) mousePos.y != (double) vector3_2.y > (double) mousePos.y && (double) mousePos.x < ((double) vector3_2.x - (double) vector3_1.x) * ((double) mousePos.y - (double) vector3_1.y) / ((double) vector3_2.y - (double) vector3_1.y) + (double) vector3_1.x)
        flag = !flag;
      val1 = val2;
    }
    if (flag)
      return 0.0f;
    float rectangle = -1f;
    for (int index = 0; index < 4; ++index)
    {
      Vector3 vector3_3 = Vector2.op_Implicit(screenPoints[index]);
      Vector3 vector3_4 = Vector2.op_Implicit(screenPoints[NGUIMath.RepeatIndex(index + 1, 4)]);
      float lineSegment = NGUIMath.DistancePointToLineSegment(mousePos, Vector2.op_Implicit(vector3_3), Vector2.op_Implicit(vector3_4));
      if ((double) lineSegment < (double) rectangle || (double) rectangle < 0.0)
        rectangle = lineSegment;
    }
    return rectangle;
  }

  public static float DistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos, Camera cam)
  {
    Vector2[] screenPoints = new Vector2[4];
    for (int index = 0; index < 4; ++index)
      screenPoints[index] = Vector2.op_Implicit(cam.WorldToScreenPoint(worldPoints[index]));
    return NGUIMath.DistanceToRectangle(screenPoints, mousePos);
  }

  public static Color HexToColor(uint val) => NGUIMath.IntToColor((int) val);

  public static int HexToDecimal(char ch)
  {
    char ch1 = ch;
    switch (ch1)
    {
      case '0':
        return 0;
      case '1':
        return 1;
      case '2':
        return 2;
      case '3':
        return 3;
      case '4':
        return 4;
      case '5':
        return 5;
      case '6':
        return 6;
      case '7':
        return 7;
      case '8':
        return 8;
      case '9':
        return 9;
      case 'A':
label_13:
        return 10;
      case 'B':
label_14:
        return 11;
      case 'C':
label_15:
        return 12;
      case 'D':
label_16:
        return 13;
      case 'E':
label_17:
        return 14;
      case 'F':
label_18:
        return 15;
      default:
        switch (ch1)
        {
          case 'a':
            goto label_13;
          case 'b':
            goto label_14;
          case 'c':
            goto label_15;
          case 'd':
            goto label_16;
          case 'e':
            goto label_17;
          case 'f':
            goto label_18;
          default:
            return 15;
        }
    }
  }

  public static string IntToBinary(int val, int bits)
  {
    string empty = string.Empty;
    for (int index = bits; index > 0; empty += ((val & 1 << --index) == 0 ? '0' : '1').ToString())
    {
      if (index == 8 || index == 16 || index == 24)
        empty += " ";
    }
    return empty;
  }

  public static Color IntToColor(int val)
  {
    float num = 0.003921569f;
    Color black = Color.black;
    black.r = num * (float) (val >> 24 & (int) byte.MaxValue);
    black.g = num * (float) (val >> 16 & (int) byte.MaxValue);
    black.b = num * (float) (val >> 8 & (int) byte.MaxValue);
    black.a = num * (float) (val & (int) byte.MaxValue);
    return black;
  }

  public static float Lerp(float from, float to, float factor) => (float) ((double) from * (1.0 - (double) factor) + (double) to * (double) factor);

  public static Rect MakePixelPerfect(Rect rect)
  {
    ((Rect) ref rect).xMin = (float) Mathf.RoundToInt(((Rect) ref rect).xMin);
    ((Rect) ref rect).yMin = (float) Mathf.RoundToInt(((Rect) ref rect).yMin);
    ((Rect) ref rect).xMax = (float) Mathf.RoundToInt(((Rect) ref rect).xMax);
    ((Rect) ref rect).yMax = (float) Mathf.RoundToInt(((Rect) ref rect).yMax);
    return rect;
  }

  public static Rect MakePixelPerfect(Rect rect, int width, int height)
  {
    rect = NGUIMath.ConvertToPixels(rect, width, height, true);
    ((Rect) ref rect).xMin = (float) Mathf.RoundToInt(((Rect) ref rect).xMin);
    ((Rect) ref rect).yMin = (float) Mathf.RoundToInt(((Rect) ref rect).yMin);
    ((Rect) ref rect).xMax = (float) Mathf.RoundToInt(((Rect) ref rect).xMax);
    ((Rect) ref rect).yMax = (float) Mathf.RoundToInt(((Rect) ref rect).yMax);
    return NGUIMath.ConvertToTexCoords(rect, width, height);
  }

  public static int RepeatIndex(int val, int max)
  {
    if (max < 1)
      return 0;
    while (val < 0)
      val += max;
    while (val >= max)
      val -= max;
    return val;
  }

  public static float RotateTowards(float from, float to, float maxAngle)
  {
    float num = NGUIMath.WrapAngle(to - from);
    if ((double) Mathf.Abs(num) > (double) maxAngle)
      num = maxAngle * Mathf.Sign(num);
    return from + num;
  }

  public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
  {
    if ((double) deltaTime > 1.0)
      deltaTime = 1f;
    float num1 = (float) (1.0 - (double) strength * (1.0 / 1000.0));
    int num2 = Mathf.RoundToInt(deltaTime * 1000f);
    Vector2 vector2 = Vector2.zero;
    for (int index = 0; index < num2; ++index)
    {
      vector2 = Vector2.op_Addition(vector2, Vector2.op_Multiply(velocity, 0.06f));
      velocity = Vector2.op_Multiply(velocity, num1);
    }
    return vector2;
  }

  public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
  {
    if ((double) deltaTime > 1.0)
      deltaTime = 1f;
    float num1 = (float) (1.0 - (double) strength * (1.0 / 1000.0));
    int num2 = Mathf.RoundToInt(deltaTime * 1000f);
    Vector3 vector3 = Vector3.zero;
    for (int index = 0; index < num2; ++index)
    {
      vector3 = Vector3.op_Addition(vector3, Vector3.op_Multiply(velocity, 0.06f));
      velocity = Vector3.op_Multiply(velocity, num1);
    }
    return vector3;
  }

  public static float SpringLerp(float strength, float deltaTime)
  {
    if ((double) deltaTime > 1.0)
      deltaTime = 1f;
    int num1 = Mathf.RoundToInt(deltaTime * 1000f);
    deltaTime = 1f / 1000f * strength;
    float num2 = 0.0f;
    for (int index = 0; index < num1; ++index)
      num2 = Mathf.Lerp(num2, 1f, deltaTime);
    return num2;
  }

  public static float SpringLerp(float from, float to, float strength, float deltaTime)
  {
    if ((double) deltaTime > 1.0)
      deltaTime = 1f;
    int num = Mathf.RoundToInt(deltaTime * 1000f);
    deltaTime = 1f / 1000f * strength;
    for (int index = 0; index < num; ++index)
      from = Mathf.Lerp(from, to, deltaTime);
    return from;
  }

  public static Quaternion SpringLerp(
    Quaternion from,
    Quaternion to,
    float strength,
    float deltaTime)
  {
    return Quaternion.Slerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
  }

  public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime) => Vector2.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));

  public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime) => Vector3.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));

  public static float Wrap01(float val) => val - (float) Mathf.FloorToInt(val);

  public static float WrapAngle(float angle)
  {
    while ((double) angle > 180.0)
      angle -= 360f;
    while ((double) angle < -180.0)
      angle += 360f;
    return angle;
  }
}
