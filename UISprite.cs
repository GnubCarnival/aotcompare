// Decompiled with JetBrains decompiler
// Type: UISprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Sprite")]
public class UISprite : UIWidget
{
  [SerializeField]
  [HideInInspector]
  private UIAtlas mAtlas;
  [SerializeField]
  [HideInInspector]
  private float mFillAmount = 1f;
  [HideInInspector]
  [SerializeField]
  private bool mFillCenter = true;
  [SerializeField]
  [HideInInspector]
  private UISprite.FillDirection mFillDirection = UISprite.FillDirection.Radial360;
  protected Rect mInner;
  protected Rect mInnerUV;
  [SerializeField]
  [HideInInspector]
  private bool mInvert;
  protected Rect mOuter;
  protected Rect mOuterUV;
  protected Vector3 mScale = Vector3.one;
  protected UIAtlas.Sprite mSprite;
  [SerializeField]
  [HideInInspector]
  private string mSpriteName;
  private bool mSpriteSet;
  [HideInInspector]
  [SerializeField]
  private UISprite.Type mType;

  protected bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
  {
    if ((double) fill < 1.0 / 1000.0)
      return false;
    if (invert || (double) fill <= 0.99900001287460327)
    {
      float num1 = Mathf.Clamp01(fill);
      if (!invert)
        num1 = 1f - num1;
      float num2 = num1 * 1.570796f;
      float num3 = Mathf.Sin(num2);
      float num4 = Mathf.Cos(num2);
      float num5;
      float num6;
      if ((double) num3 > (double) num4)
      {
        num5 = num4 * (1f / num3);
        num6 = 1f;
        if (!invert)
        {
          xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num5);
          xy[3].y = xy[0].y;
          uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num5);
          uv[3].y = uv[0].y;
        }
      }
      else if ((double) num4 > (double) num3)
      {
        num6 = num3 * (1f / num4);
        num5 = 1f;
        if (invert)
        {
          xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, num6);
          xy[1].x = xy[0].x;
          uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, num6);
          uv[1].x = uv[0].x;
        }
      }
      else
      {
        num6 = 1f;
        num5 = 1f;
      }
      if (invert)
      {
        xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num5);
        uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num5);
      }
      else
      {
        xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, num6);
        uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, num6);
      }
    }
    return true;
  }

  protected void FilledFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 1f;
    float num4 = -1f;
    float num5 = ((Rect) ref this.mOuterUV).xMin;
    float num6 = ((Rect) ref this.mOuterUV).yMin;
    float num7 = ((Rect) ref this.mOuterUV).xMax;
    float num8 = ((Rect) ref this.mOuterUV).yMax;
    if (this.mFillDirection == UISprite.FillDirection.Horizontal || this.mFillDirection == UISprite.FillDirection.Vertical)
    {
      float num9 = (num7 - num5) * this.mFillAmount;
      float num10 = (num8 - num6) * this.mFillAmount;
      if (this.fillDirection == UISprite.FillDirection.Horizontal)
      {
        if (this.mInvert)
        {
          num1 = 1f - this.mFillAmount;
          num5 = num7 - num9;
        }
        else
        {
          num3 *= this.mFillAmount;
          num7 = num5 + num9;
        }
      }
      else if (this.fillDirection == UISprite.FillDirection.Vertical)
      {
        if (this.mInvert)
        {
          num4 *= this.mFillAmount;
          num6 = num8 - num10;
        }
        else
        {
          num2 = (float) -(1.0 - (double) this.mFillAmount);
          num8 = num6 + num10;
        }
      }
    }
    Vector2[] xy = new Vector2[4];
    Vector2[] uv = new Vector2[4];
    xy[0] = new Vector2(num3, num2);
    xy[1] = new Vector2(num3, num4);
    xy[2] = new Vector2(num1, num4);
    xy[3] = new Vector2(num1, num2);
    uv[0] = new Vector2(num7, num8);
    uv[1] = new Vector2(num7, num6);
    uv[2] = new Vector2(num5, num6);
    uv[3] = new Vector2(num5, num8);
    Color color = this.color;
    color.a *= this.mPanel.alpha;
    Color32 color32 = Color32.op_Implicit(!this.atlas.premultipliedAlpha ? color : NGUITools.ApplyPMA(color));
    if (this.fillDirection == UISprite.FillDirection.Radial90)
    {
      if (!this.AdjustRadial(xy, uv, this.mFillAmount, this.mInvert))
        return;
    }
    else
    {
      if (this.fillDirection == UISprite.FillDirection.Radial180)
      {
        Vector2[] vector2Array1 = new Vector2[4];
        Vector2[] vector2Array2 = new Vector2[4];
        for (int offset = 0; offset < 2; ++offset)
        {
          vector2Array1[0] = new Vector2(0.0f, 0.0f);
          vector2Array1[1] = new Vector2(0.0f, 1f);
          vector2Array1[2] = new Vector2(1f, 1f);
          vector2Array1[3] = new Vector2(1f, 0.0f);
          vector2Array2[0] = new Vector2(0.0f, 0.0f);
          vector2Array2[1] = new Vector2(0.0f, 1f);
          vector2Array2[2] = new Vector2(1f, 1f);
          vector2Array2[3] = new Vector2(1f, 0.0f);
          if (this.mInvert)
          {
            if (offset > 0)
            {
              this.Rotate(vector2Array1, offset);
              this.Rotate(vector2Array2, offset);
            }
          }
          else if (offset < 1)
          {
            this.Rotate(vector2Array1, 1 - offset);
            this.Rotate(vector2Array2, 1 - offset);
          }
          float num11;
          float num12;
          if (offset == 1)
          {
            num11 = !this.mInvert ? 1f : 0.5f;
            num12 = !this.mInvert ? 0.5f : 1f;
          }
          else
          {
            num11 = !this.mInvert ? 0.5f : 1f;
            num12 = !this.mInvert ? 1f : 0.5f;
          }
          vector2Array1[1].y = Mathf.Lerp(num11, num12, vector2Array1[1].y);
          vector2Array1[2].y = Mathf.Lerp(num11, num12, vector2Array1[2].y);
          vector2Array2[1].y = Mathf.Lerp(num11, num12, vector2Array2[1].y);
          vector2Array2[2].y = Mathf.Lerp(num11, num12, vector2Array2[2].y);
          float fill = this.mFillAmount * 2f - (float) offset;
          bool flag = offset % 2 == 1;
          if (this.AdjustRadial(vector2Array1, vector2Array2, fill, !flag))
          {
            if (this.mInvert)
              flag = !flag;
            if (flag)
            {
              for (int index = 0; index < 4; ++index)
              {
                float num13 = Mathf.Lerp(xy[0].x, xy[2].x, vector2Array1[index].x);
                float num14 = Mathf.Lerp(xy[0].y, xy[2].y, vector2Array1[index].y);
                float num15 = Mathf.Lerp(uv[0].x, uv[2].x, vector2Array2[index].x);
                float num16 = Mathf.Lerp(uv[0].y, uv[2].y, vector2Array2[index].y);
                verts.Add(new Vector3(num13, num14, 0.0f));
                uvs.Add(new Vector2(num15, num16));
                cols.Add(color32);
              }
            }
            else
            {
              for (int index = 3; index > -1; --index)
              {
                float num17 = Mathf.Lerp(xy[0].x, xy[2].x, vector2Array1[index].x);
                float num18 = Mathf.Lerp(xy[0].y, xy[2].y, vector2Array1[index].y);
                float num19 = Mathf.Lerp(uv[0].x, uv[2].x, vector2Array2[index].x);
                float num20 = Mathf.Lerp(uv[0].y, uv[2].y, vector2Array2[index].y);
                verts.Add(new Vector3(num17, num18, 0.0f));
                uvs.Add(new Vector2(num19, num20));
                cols.Add(color32);
              }
            }
          }
        }
        return;
      }
      if (this.fillDirection == UISprite.FillDirection.Radial360)
      {
        float[] numArray = new float[16]
        {
          0.5f,
          1f,
          0.0f,
          0.5f,
          0.5f,
          1f,
          0.5f,
          1f,
          0.0f,
          0.5f,
          0.5f,
          1f,
          0.0f,
          0.5f,
          0.0f,
          0.5f
        };
        Vector2[] vector2Array3 = new Vector2[4];
        Vector2[] vector2Array4 = new Vector2[4];
        for (int offset = 0; offset < 4; ++offset)
        {
          vector2Array3[0] = new Vector2(0.0f, 0.0f);
          vector2Array3[1] = new Vector2(0.0f, 1f);
          vector2Array3[2] = new Vector2(1f, 1f);
          vector2Array3[3] = new Vector2(1f, 0.0f);
          vector2Array4[0] = new Vector2(0.0f, 0.0f);
          vector2Array4[1] = new Vector2(0.0f, 1f);
          vector2Array4[2] = new Vector2(1f, 1f);
          vector2Array4[3] = new Vector2(1f, 0.0f);
          if (this.mInvert)
          {
            if (offset > 0)
            {
              this.Rotate(vector2Array3, offset);
              this.Rotate(vector2Array4, offset);
            }
          }
          else if (offset < 3)
          {
            this.Rotate(vector2Array3, 3 - offset);
            this.Rotate(vector2Array4, 3 - offset);
          }
          for (int index1 = 0; index1 < 4; ++index1)
          {
            int index2 = !this.mInvert ? offset * 4 : (3 - offset) * 4;
            float num21 = numArray[index2];
            float num22 = numArray[index2 + 1];
            float num23 = numArray[index2 + 2];
            float num24 = numArray[index2 + 3];
            vector2Array3[index1].x = Mathf.Lerp(num21, num22, vector2Array3[index1].x);
            vector2Array3[index1].y = Mathf.Lerp(num23, num24, vector2Array3[index1].y);
            vector2Array4[index1].x = Mathf.Lerp(num21, num22, vector2Array4[index1].x);
            vector2Array4[index1].y = Mathf.Lerp(num23, num24, vector2Array4[index1].y);
          }
          float fill = this.mFillAmount * 4f - (float) offset;
          bool flag = offset % 2 == 1;
          if (this.AdjustRadial(vector2Array3, vector2Array4, fill, !flag))
          {
            if (this.mInvert)
              flag = !flag;
            if (flag)
            {
              for (int index = 0; index < 4; ++index)
              {
                float num25 = Mathf.Lerp(xy[0].x, xy[2].x, vector2Array3[index].x);
                float num26 = Mathf.Lerp(xy[0].y, xy[2].y, vector2Array3[index].y);
                float num27 = Mathf.Lerp(uv[0].x, uv[2].x, vector2Array4[index].x);
                float num28 = Mathf.Lerp(uv[0].y, uv[2].y, vector2Array4[index].y);
                verts.Add(new Vector3(num25, num26, 0.0f));
                uvs.Add(new Vector2(num27, num28));
                cols.Add(color32);
              }
            }
            else
            {
              for (int index = 3; index > -1; --index)
              {
                float num29 = Mathf.Lerp(xy[0].x, xy[2].x, vector2Array3[index].x);
                float num30 = Mathf.Lerp(xy[0].y, xy[2].y, vector2Array3[index].y);
                float num31 = Mathf.Lerp(uv[0].x, uv[2].x, vector2Array4[index].x);
                float num32 = Mathf.Lerp(uv[0].y, uv[2].y, vector2Array4[index].y);
                verts.Add(new Vector3(num29, num30, 0.0f));
                uvs.Add(new Vector2(num31, num32));
                cols.Add(color32);
              }
            }
          }
        }
        return;
      }
    }
    for (int index = 0; index < 4; ++index)
    {
      verts.Add(Vector2.op_Implicit(xy[index]));
      uvs.Add(uv[index]);
      cols.Add(color32);
    }
  }

  public UIAtlas.Sprite GetAtlasSprite()
  {
    if (!this.mSpriteSet)
      this.mSprite = (UIAtlas.Sprite) null;
    if (this.mSprite == null && Object.op_Inequality((Object) this.mAtlas, (Object) null))
    {
      if (!string.IsNullOrEmpty(this.mSpriteName))
      {
        UIAtlas.Sprite sprite = this.mAtlas.GetSprite(this.mSpriteName);
        if (sprite == null)
          return (UIAtlas.Sprite) null;
        this.SetAtlasSprite(sprite);
      }
      if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
      {
        UIAtlas.Sprite sprite = this.mAtlas.spriteList[0];
        if (sprite == null)
          return (UIAtlas.Sprite) null;
        this.SetAtlasSprite(sprite);
        if (this.mSprite == null)
        {
          Debug.LogError((object) (((Object) this.mAtlas).name + " seems to have a null sprite!"));
          return (UIAtlas.Sprite) null;
        }
        this.mSpriteName = this.mSprite.name;
      }
      if (this.mSprite != null)
      {
        this.material = this.mAtlas.spriteMaterial;
        this.UpdateUVs(true);
      }
    }
    return this.mSprite;
  }

  public override void MakePixelPerfect()
  {
    if (!this.isValid)
      return;
    this.UpdateUVs(false);
    switch (this.type)
    {
      case UISprite.Type.Sliced:
        Vector3 localPosition1 = this.cachedTransform.localPosition;
        localPosition1.x = (float) Mathf.RoundToInt(localPosition1.x);
        localPosition1.y = (float) Mathf.RoundToInt(localPosition1.y);
        localPosition1.z = (float) Mathf.RoundToInt(localPosition1.z);
        this.cachedTransform.localPosition = localPosition1;
        Vector3 localScale1 = this.cachedTransform.localScale;
        localScale1.x = (float) (Mathf.RoundToInt(localScale1.x * 0.5f) << 1);
        localScale1.y = (float) (Mathf.RoundToInt(localScale1.y * 0.5f) << 1);
        localScale1.z = 1f;
        this.cachedTransform.localScale = localScale1;
        break;
      case UISprite.Type.Tiled:
        Vector3 localPosition2 = this.cachedTransform.localPosition;
        localPosition2.x = (float) Mathf.RoundToInt(localPosition2.x);
        localPosition2.y = (float) Mathf.RoundToInt(localPosition2.y);
        localPosition2.z = (float) Mathf.RoundToInt(localPosition2.z);
        this.cachedTransform.localPosition = localPosition2;
        Vector3 localScale2 = this.cachedTransform.localScale;
        localScale2.x = (float) Mathf.RoundToInt(localScale2.x);
        localScale2.y = (float) Mathf.RoundToInt(localScale2.y);
        localScale2.z = 1f;
        this.cachedTransform.localScale = localScale2;
        break;
      default:
        Texture mainTexture = this.mainTexture;
        Vector3 localScale3 = this.cachedTransform.localScale;
        if (Object.op_Inequality((Object) mainTexture, (Object) null))
        {
          Rect pixels = NGUIMath.ConvertToPixels(this.outerUV, mainTexture.width, mainTexture.height, true);
          float pixelSize = this.atlas.pixelSize;
          localScale3.x = (float) Mathf.RoundToInt(((Rect) ref pixels).width * pixelSize) * Mathf.Sign(localScale3.x);
          localScale3.y = (float) Mathf.RoundToInt(((Rect) ref pixels).height * pixelSize) * Mathf.Sign(localScale3.y);
          localScale3.z = 1f;
          this.cachedTransform.localScale = localScale3;
        }
        int num1 = Mathf.RoundToInt(Mathf.Abs(localScale3.x) * (1f + this.mSprite.paddingLeft + this.mSprite.paddingRight));
        int num2 = Mathf.RoundToInt(Mathf.Abs(localScale3.y) * (1f + this.mSprite.paddingTop + this.mSprite.paddingBottom));
        Vector3 localPosition3 = this.cachedTransform.localPosition;
        localPosition3.x = (float) (Mathf.CeilToInt(localPosition3.x * 4f) >> 2);
        localPosition3.y = (float) (Mathf.CeilToInt(localPosition3.y * 4f) >> 2);
        localPosition3.z = (float) Mathf.RoundToInt(localPosition3.z);
        if (num1 % 2 == 1 && (this.pivot == UIWidget.Pivot.Top || this.pivot == UIWidget.Pivot.Center || this.pivot == UIWidget.Pivot.Bottom))
          localPosition3.x += 0.5f;
        if (num2 % 2 == 1 && (this.pivot == UIWidget.Pivot.Left || this.pivot == UIWidget.Pivot.Center || this.pivot == UIWidget.Pivot.Right))
          localPosition3.y += 0.5f;
        this.cachedTransform.localPosition = localPosition3;
        break;
    }
  }

  public override void OnFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    switch (this.type)
    {
      case UISprite.Type.Simple:
        this.SimpleFill(verts, uvs, cols);
        break;
      case UISprite.Type.Sliced:
        this.SlicedFill(verts, uvs, cols);
        break;
      case UISprite.Type.Tiled:
        this.TiledFill(verts, uvs, cols);
        break;
      case UISprite.Type.Filled:
        this.FilledFill(verts, uvs, cols);
        break;
    }
  }

  protected override void OnStart()
  {
    if (!Object.op_Inequality((Object) this.mAtlas, (Object) null))
      return;
    this.UpdateUVs(true);
  }

  protected void Rotate(Vector2[] v, int offset)
  {
    for (int index = 0; index < offset; ++index)
    {
      Vector2 vector2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2).\u002Ector(v[3].x, v[3].y);
      v[3].x = v[2].y;
      v[3].y = v[2].x;
      v[2].x = v[1].y;
      v[2].y = v[1].x;
      v[1].x = v[0].y;
      v[1].y = v[0].x;
      v[0].x = vector2.y;
      v[0].y = vector2.x;
    }
  }

  protected void SetAtlasSprite(UIAtlas.Sprite sp)
  {
    this.mChanged = true;
    this.mSpriteSet = true;
    if (sp != null)
    {
      this.mSprite = sp;
      this.mSpriteName = this.mSprite.name;
    }
    else
    {
      this.mSpriteName = this.mSprite == null ? string.Empty : this.mSprite.name;
      this.mSprite = sp;
    }
  }

  protected void SimpleFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(((Rect) ref this.mOuterUV).xMin, ((Rect) ref this.mOuterUV).yMin);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(((Rect) ref this.mOuterUV).xMax, ((Rect) ref this.mOuterUV).yMax);
    verts.Add(new Vector3(1f, 0.0f, 0.0f));
    verts.Add(new Vector3(1f, -1f, 0.0f));
    verts.Add(new Vector3(0.0f, -1f, 0.0f));
    verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
    uvs.Add(vector2_2);
    uvs.Add(new Vector2(vector2_2.x, vector2_1.y));
    uvs.Add(vector2_1);
    uvs.Add(new Vector2(vector2_1.x, vector2_2.y));
    Color color = this.color;
    color.a *= this.mPanel.alpha;
    Color32 color32 = Color32.op_Implicit(!this.atlas.premultipliedAlpha ? color : NGUITools.ApplyPMA(color));
    cols.Add(color32);
    cols.Add(color32);
    cols.Add(color32);
    cols.Add(color32);
  }

  protected void SlicedFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    if (Rect.op_Equality(this.mOuterUV, this.mInnerUV))
    {
      this.SimpleFill(verts, uvs, cols);
    }
    else
    {
      Vector2[] vector2Array1 = new Vector2[4];
      Vector2[] vector2Array2 = new Vector2[4];
      Texture mainTexture = this.mainTexture;
      vector2Array1[0] = Vector2.zero;
      vector2Array1[1] = Vector2.zero;
      vector2Array1[2] = new Vector2(1f, -1f);
      vector2Array1[3] = new Vector2(1f, -1f);
      if (Object.op_Equality((Object) mainTexture, (Object) null))
      {
        for (int index = 0; index < 4; ++index)
          vector2Array2[index] = Vector2.zero;
      }
      else
      {
        float pixelSize = this.atlas.pixelSize;
        float num1 = (((Rect) ref this.mInnerUV).xMin - ((Rect) ref this.mOuterUV).xMin) * pixelSize;
        float num2 = (((Rect) ref this.mOuterUV).xMax - ((Rect) ref this.mInnerUV).xMax) * pixelSize;
        float num3 = (((Rect) ref this.mInnerUV).yMax - ((Rect) ref this.mOuterUV).yMax) * pixelSize;
        float num4 = (((Rect) ref this.mOuterUV).yMin - ((Rect) ref this.mInnerUV).yMin) * pixelSize;
        Vector3 localScale = this.cachedTransform.localScale;
        localScale.x = Mathf.Max(0.0f, localScale.x);
        localScale.y = Mathf.Max(0.0f, localScale.y);
        Vector2 vector2_1;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_1).\u002Ector(localScale.x / (float) mainTexture.width, localScale.y / (float) mainTexture.height);
        Vector2 vector2_2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_2).\u002Ector(num1 / vector2_1.x, num3 / vector2_1.y);
        Vector2 vector2_3;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_3).\u002Ector(num2 / vector2_1.x, num4 / vector2_1.y);
        UIWidget.Pivot pivot = this.pivot;
        switch (pivot)
        {
          case UIWidget.Pivot.TopRight:
          case UIWidget.Pivot.Right:
          case UIWidget.Pivot.BottomRight:
            vector2Array1[0].x = Mathf.Min(0.0f, (float) (1.0 - ((double) vector2_3.x + (double) vector2_2.x)));
            vector2Array1[1].x = vector2Array1[0].x + vector2_2.x;
            vector2Array1[2].x = vector2Array1[0].x + Mathf.Max(vector2_2.x, 1f - vector2_3.x);
            vector2Array1[3].x = vector2Array1[0].x + Mathf.Max(vector2_2.x + vector2_3.x, 1f);
            break;
          default:
            vector2Array1[1].x = vector2_2.x;
            vector2Array1[2].x = Mathf.Max(vector2_2.x, 1f - vector2_3.x);
            vector2Array1[3].x = Mathf.Max(vector2_2.x + vector2_3.x, 1f);
            break;
        }
        switch (pivot)
        {
          case UIWidget.Pivot.BottomLeft:
          case UIWidget.Pivot.Bottom:
          case UIWidget.Pivot.BottomRight:
            vector2Array1[0].y = Mathf.Max(0.0f, (float) (-1.0 - ((double) vector2_3.y + (double) vector2_2.y)));
            vector2Array1[1].y = vector2Array1[0].y + vector2_2.y;
            vector2Array1[2].y = vector2Array1[0].y + Mathf.Min(vector2_2.y, -1f - vector2_3.y);
            vector2Array1[3].y = vector2Array1[0].y + Mathf.Min(vector2_2.y + vector2_3.y, -1f);
            break;
          default:
            vector2Array1[1].y = vector2_2.y;
            vector2Array1[2].y = Mathf.Min(vector2_2.y, -1f - vector2_3.y);
            vector2Array1[3].y = Mathf.Min(vector2_2.y + vector2_3.y, -1f);
            break;
        }
        vector2Array2[0] = new Vector2(((Rect) ref this.mOuterUV).xMin, ((Rect) ref this.mOuterUV).yMax);
        vector2Array2[1] = new Vector2(((Rect) ref this.mInnerUV).xMin, ((Rect) ref this.mInnerUV).yMax);
        vector2Array2[2] = new Vector2(((Rect) ref this.mInnerUV).xMax, ((Rect) ref this.mInnerUV).yMin);
        vector2Array2[3] = new Vector2(((Rect) ref this.mOuterUV).xMax, ((Rect) ref this.mOuterUV).yMin);
      }
      Color color = this.color;
      color.a *= this.mPanel.alpha;
      Color32 color32 = Color32.op_Implicit(!this.atlas.premultipliedAlpha ? color : NGUITools.ApplyPMA(color));
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = index1 + 1;
        for (int index3 = 0; index3 < 3; ++index3)
        {
          if (this.mFillCenter || index1 != 1 || index3 != 1)
          {
            int index4 = index3 + 1;
            verts.Add(new Vector3(vector2Array1[index2].x, vector2Array1[index3].y, 0.0f));
            verts.Add(new Vector3(vector2Array1[index2].x, vector2Array1[index4].y, 0.0f));
            verts.Add(new Vector3(vector2Array1[index1].x, vector2Array1[index4].y, 0.0f));
            verts.Add(new Vector3(vector2Array1[index1].x, vector2Array1[index3].y, 0.0f));
            uvs.Add(new Vector2(vector2Array2[index2].x, vector2Array2[index3].y));
            uvs.Add(new Vector2(vector2Array2[index2].x, vector2Array2[index4].y));
            uvs.Add(new Vector2(vector2Array2[index1].x, vector2Array2[index4].y));
            uvs.Add(new Vector2(vector2Array2[index1].x, vector2Array2[index3].y));
            cols.Add(color32);
            cols.Add(color32);
            cols.Add(color32);
            cols.Add(color32);
          }
        }
      }
    }
  }

  protected void TiledFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    Texture mainTexture = this.material.mainTexture;
    if (!Object.op_Inequality((Object) mainTexture, (Object) null))
      return;
    Rect rect = this.mInner;
    if (this.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
      rect = NGUIMath.ConvertToPixels(rect, mainTexture.width, mainTexture.height, true);
    Vector2 vector2_1 = Vector2.op_Implicit(this.cachedTransform.localScale);
    float pixelSize = this.atlas.pixelSize;
    float num1 = Mathf.Abs(((Rect) ref rect).width / vector2_1.x) * pixelSize;
    float num2 = Mathf.Abs(((Rect) ref rect).height / vector2_1.y) * pixelSize;
    if ((double) num1 < 0.0099999997764825821 || (double) num2 < 0.0099999997764825821)
    {
      Debug.LogWarning((object) ("The tiled sprite (" + NGUITools.GetHierarchy(((Component) this).gameObject) + ") is too small.\nConsider using a bigger one."));
      num1 = 0.01f;
      num2 = 0.01f;
    }
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(((Rect) ref rect).xMin / (float) mainTexture.width, ((Rect) ref rect).yMin / (float) mainTexture.height);
    Vector2 vector2_3;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_3).\u002Ector(((Rect) ref rect).xMax / (float) mainTexture.width, ((Rect) ref rect).yMax / (float) mainTexture.height);
    Vector2 vector2_4 = vector2_3;
    Color color = this.color;
    color.a *= this.mPanel.alpha;
    Color32 color32 = Color32.op_Implicit(!this.atlas.premultipliedAlpha ? color : NGUITools.ApplyPMA(color));
    for (float num3 = 0.0f; (double) num3 < 1.0; num3 += num2)
    {
      float num4 = 0.0f;
      vector2_4.x = vector2_3.x;
      float num5 = num3 + num2;
      if ((double) num5 > 1.0)
      {
        vector2_4.y = vector2_2.y + (float) (((double) vector2_3.y - (double) vector2_2.y) * (1.0 - (double) num3) / ((double) num5 - (double) num3));
        num5 = 1f;
      }
      for (; (double) num4 < 1.0; num4 += num1)
      {
        float num6 = num4 + num1;
        if ((double) num6 > 1.0)
        {
          vector2_4.x = vector2_2.x + (float) (((double) vector2_3.x - (double) vector2_2.x) * (1.0 - (double) num4) / ((double) num6 - (double) num4));
          num6 = 1f;
        }
        verts.Add(new Vector3(num6, -num3, 0.0f));
        verts.Add(new Vector3(num6, -num5, 0.0f));
        verts.Add(new Vector3(num4, -num5, 0.0f));
        verts.Add(new Vector3(num4, -num3, 0.0f));
        uvs.Add(new Vector2(vector2_4.x, 1f - vector2_2.y));
        uvs.Add(new Vector2(vector2_4.x, 1f - vector2_4.y));
        uvs.Add(new Vector2(vector2_2.x, 1f - vector2_4.y));
        uvs.Add(new Vector2(vector2_2.x, 1f - vector2_2.y));
        cols.Add(color32);
        cols.Add(color32);
        cols.Add(color32);
        cols.Add(color32);
      }
    }
  }

  public override void Update()
  {
    base.Update();
    if (this.mChanged || !this.mSpriteSet)
    {
      this.mSpriteSet = true;
      this.mSprite = (UIAtlas.Sprite) null;
      this.mChanged = true;
      this.UpdateUVs(true);
    }
    else
      this.UpdateUVs(false);
  }

  public virtual void UpdateUVs(bool force)
  {
    if ((this.type == UISprite.Type.Sliced || this.type == UISprite.Type.Tiled) && Vector3.op_Inequality(this.cachedTransform.localScale, this.mScale))
    {
      this.mScale = this.cachedTransform.localScale;
      this.mChanged = true;
    }
    if (!(this.isValid & force))
      return;
    Texture mainTexture = this.mainTexture;
    if (!Object.op_Inequality((Object) mainTexture, (Object) null))
      return;
    this.mInner = this.mSprite.inner;
    this.mOuter = this.mSprite.outer;
    this.mInnerUV = this.mInner;
    this.mOuterUV = this.mOuter;
    if (this.atlas.coordinates != UIAtlas.Coordinates.Pixels)
      return;
    this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, mainTexture.width, mainTexture.height);
    this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, mainTexture.width, mainTexture.height);
  }

  public UIAtlas atlas
  {
    get => this.mAtlas;
    set
    {
      if (!Object.op_Inequality((Object) this.mAtlas, (Object) value))
        return;
      this.mAtlas = value;
      this.mSpriteSet = false;
      this.mSprite = (UIAtlas.Sprite) null;
      this.material = Object.op_Equality((Object) this.mAtlas, (Object) null) ? (Material) null : this.mAtlas.spriteMaterial;
      if (string.IsNullOrEmpty(this.mSpriteName) && Object.op_Inequality((Object) this.mAtlas, (Object) null) && this.mAtlas.spriteList.Count > 0)
      {
        this.SetAtlasSprite(this.mAtlas.spriteList[0]);
        this.mSpriteName = this.mSprite.name;
      }
      if (string.IsNullOrEmpty(this.mSpriteName))
        return;
      string mSpriteName = this.mSpriteName;
      this.mSpriteName = string.Empty;
      this.spriteName = mSpriteName;
      this.mChanged = true;
      this.UpdateUVs(true);
    }
  }

  public override Vector4 border
  {
    get
    {
      if (this.type != UISprite.Type.Sliced)
        return base.border;
      UIAtlas.Sprite atlasSprite = this.GetAtlasSprite();
      if (atlasSprite == null)
        return Vector4.op_Implicit(Vector2.zero);
      Rect rect1 = atlasSprite.outer;
      Rect rect2 = atlasSprite.inner;
      Texture mainTexture = this.mainTexture;
      if (this.atlas.coordinates == UIAtlas.Coordinates.TexCoords && Object.op_Inequality((Object) mainTexture, (Object) null))
      {
        rect1 = NGUIMath.ConvertToPixels(rect1, mainTexture.width, mainTexture.height, true);
        rect2 = NGUIMath.ConvertToPixels(rect2, mainTexture.width, mainTexture.height, true);
      }
      return Vector4.op_Multiply(new Vector4(((Rect) ref rect2).xMin - ((Rect) ref rect1).xMin, ((Rect) ref rect2).yMin - ((Rect) ref rect1).yMin, ((Rect) ref rect1).xMax - ((Rect) ref rect2).xMax, ((Rect) ref rect1).yMax - ((Rect) ref rect2).yMax), this.atlas.pixelSize);
    }
  }

  public float fillAmount
  {
    get => this.mFillAmount;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mFillAmount == (double) num)
        return;
      this.mFillAmount = num;
      this.mChanged = true;
    }
  }

  public bool fillCenter
  {
    get => this.mFillCenter;
    set
    {
      if (this.mFillCenter == value)
        return;
      this.mFillCenter = value;
      this.MarkAsChanged();
    }
  }

  public UISprite.FillDirection fillDirection
  {
    get => this.mFillDirection;
    set
    {
      if (this.mFillDirection == value)
        return;
      this.mFillDirection = value;
      this.mChanged = true;
    }
  }

  public Rect innerUV
  {
    get
    {
      this.UpdateUVs(false);
      return this.mInnerUV;
    }
  }

  public bool invert
  {
    get => this.mInvert;
    set
    {
      if (this.mInvert == value)
        return;
      this.mInvert = value;
      this.mChanged = true;
    }
  }

  public bool isValid => this.GetAtlasSprite() != null;

  public override Material material
  {
    get
    {
      Material material = base.material;
      if (Object.op_Equality((Object) material, (Object) null))
      {
        material = Object.op_Equality((Object) this.mAtlas, (Object) null) ? (Material) null : this.mAtlas.spriteMaterial;
        this.mSprite = (UIAtlas.Sprite) null;
        this.material = material;
        if (Object.op_Inequality((Object) material, (Object) null))
          this.UpdateUVs(true);
      }
      return material;
    }
  }

  public Rect outerUV
  {
    get
    {
      this.UpdateUVs(false);
      return this.mOuterUV;
    }
  }

  public override bool pixelPerfectAfterResize => this.type == UISprite.Type.Sliced;

  public override Vector4 relativePadding => this.isValid && this.type == UISprite.Type.Simple ? new Vector4(this.mSprite.paddingLeft, this.mSprite.paddingTop, this.mSprite.paddingRight, this.mSprite.paddingBottom) : base.relativePadding;

  public string spriteName
  {
    get => this.mSpriteName;
    set
    {
      if (string.IsNullOrEmpty(value))
      {
        if (string.IsNullOrEmpty(this.mSpriteName))
          return;
        this.mSpriteName = string.Empty;
        this.mSprite = (UIAtlas.Sprite) null;
        this.mChanged = true;
        this.mSpriteSet = false;
      }
      else
      {
        if (!(this.mSpriteName != value))
          return;
        this.mSpriteName = value;
        this.mSprite = (UIAtlas.Sprite) null;
        this.mChanged = true;
        this.mSpriteSet = false;
        if (!this.isValid)
          return;
        this.UpdateUVs(true);
      }
    }
  }

  public virtual UISprite.Type type
  {
    get => this.mType;
    set
    {
      if (this.mType == value)
        return;
      this.mType = value;
      this.MarkAsChanged();
    }
  }

  public enum FillDirection
  {
    Horizontal,
    Vertical,
    Radial90,
    Radial180,
    Radial360,
  }

  public enum Type
  {
    Simple,
    Sliced,
    Tiled,
    Filled,
  }
}
