// Decompiled with JetBrains decompiler
// Type: UITexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Texture")]
public class UITexture : UIWidget
{
  private bool mCreatingMat;
  private Material mDynamicMat;
  private int mPMA = -1;
  [HideInInspector]
  [SerializeField]
  private Rect mRect = new Rect(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  [HideInInspector]
  private Shader mShader;
  [SerializeField]
  [HideInInspector]
  private Texture mTexture;

  public override void MakePixelPerfect()
  {
    Texture mainTexture = this.mainTexture;
    if (Object.op_Inequality((Object) mainTexture, (Object) null))
    {
      Vector3 localScale = this.cachedTransform.localScale;
      ref Vector3 local1 = ref localScale;
      double width1 = (double) mainTexture.width;
      Rect uvRect = this.uvRect;
      double width2 = (double) ((Rect) ref uvRect).width;
      double num1 = width1 * width2;
      local1.x = (float) num1;
      ref Vector3 local2 = ref localScale;
      double height1 = (double) mainTexture.height;
      uvRect = this.uvRect;
      double height2 = (double) ((Rect) ref uvRect).height;
      double num2 = height1 * height2;
      local2.y = (float) num2;
      localScale.z = 1f;
      this.cachedTransform.localScale = localScale;
    }
    base.MakePixelPerfect();
  }

  private void OnDestroy() => NGUITools.Destroy((Object) this.mDynamicMat);

  public override void OnFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    Color color = this.color;
    color.a *= this.mPanel.alpha;
    Color32 color32 = Color32.op_Implicit(!this.premultipliedAlpha ? color : NGUITools.ApplyPMA(color));
    verts.Add(new Vector3(1f, 0.0f, 0.0f));
    verts.Add(new Vector3(1f, -1f, 0.0f));
    verts.Add(new Vector3(0.0f, -1f, 0.0f));
    verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
    uvs.Add(new Vector2(((Rect) ref this.mRect).xMax, ((Rect) ref this.mRect).yMax));
    uvs.Add(new Vector2(((Rect) ref this.mRect).xMax, ((Rect) ref this.mRect).yMin));
    uvs.Add(new Vector2(((Rect) ref this.mRect).xMin, ((Rect) ref this.mRect).yMin));
    uvs.Add(new Vector2(((Rect) ref this.mRect).xMin, ((Rect) ref this.mRect).yMax));
    cols.Add(color32);
    cols.Add(color32);
    cols.Add(color32);
    cols.Add(color32);
  }

  public bool hasDynamicMaterial => Object.op_Inequality((Object) this.mDynamicMat, (Object) null);

  public override bool keepMaterial => true;

  public override Texture mainTexture
  {
    get => !Object.op_Equality((Object) this.mTexture, (Object) null) ? this.mTexture : base.mainTexture;
    set
    {
      if (Object.op_Inequality((Object) this.mPanel, (Object) null) && Object.op_Inequality((Object) this.mMat, (Object) null))
        this.mPanel.RemoveWidget((UIWidget) this);
      if (Object.op_Equality((Object) this.mMat, (Object) null))
      {
        this.mDynamicMat = new Material(this.shader);
        ((Object) this.mDynamicMat).hideFlags = (HideFlags) 4;
        this.mMat = this.mDynamicMat;
      }
      this.mPanel = (UIPanel) null;
      this.mTex = value;
      this.mTexture = value;
      this.mMat.mainTexture = value;
      if (!((Behaviour) this).enabled)
        return;
      this.CreatePanel();
    }
  }

  public override Material material
  {
    get
    {
      if (!this.mCreatingMat && Object.op_Equality((Object) this.mMat, (Object) null))
      {
        this.mCreatingMat = true;
        if (Object.op_Inequality((Object) this.mainTexture, (Object) null))
        {
          if (Object.op_Equality((Object) this.mShader, (Object) null))
            this.mShader = Shader.Find("Unlit/Texture");
          this.mDynamicMat = new Material(this.mShader);
          ((Object) this.mDynamicMat).hideFlags = (HideFlags) 4;
          this.mDynamicMat.mainTexture = this.mainTexture;
          base.material = this.mDynamicMat;
          this.mPMA = 0;
        }
        this.mCreatingMat = false;
      }
      return this.mMat;
    }
    set
    {
      if (Object.op_Inequality((Object) this.mDynamicMat, (Object) value) && Object.op_Inequality((Object) this.mDynamicMat, (Object) null))
      {
        NGUITools.Destroy((Object) this.mDynamicMat);
        this.mDynamicMat = (Material) null;
      }
      base.material = value;
      this.mPMA = -1;
    }
  }

  public bool premultipliedAlpha
  {
    get
    {
      if (this.mPMA == -1)
      {
        Material material = this.material;
        this.mPMA = Object.op_Equality((Object) material, (Object) null) || Object.op_Equality((Object) material.shader, (Object) null) || !((Object) material.shader).name.Contains("Premultiplied") ? 0 : 1;
      }
      return this.mPMA == 1;
    }
  }

  public Shader shader
  {
    get
    {
      if (Object.op_Equality((Object) this.mShader, (Object) null))
      {
        Material material = this.material;
        if (Object.op_Inequality((Object) material, (Object) null))
          this.mShader = material.shader;
        if (Object.op_Equality((Object) this.mShader, (Object) null))
          this.mShader = Shader.Find("Unlit/Texture");
      }
      return this.mShader;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.mShader, (Object) value))
        return;
      this.mShader = value;
      Material material = this.material;
      if (Object.op_Inequality((Object) material, (Object) null))
        material.shader = value;
      this.mPMA = -1;
    }
  }

  public Rect uvRect
  {
    get => this.mRect;
    set
    {
      if (!Rect.op_Inequality(this.mRect, value))
        return;
      this.mRect = value;
      this.MarkAsChanged();
    }
  }
}
