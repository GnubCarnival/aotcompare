// Decompiled with JetBrains decompiler
// Type: EffectLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using UnityEngine;

public class EffectLayer : MonoBehaviour
{
  public EffectNode[] ActiveENodes;
  public bool AlongVelocity;
  public int AngleAroundAxis;
  public bool AttractionAffectorEnable;
  public AnimationCurve AttractionCurve;
  public Vector3 AttractionPosition;
  public float AttractMag = 0.1f;
  public EffectNode[] AvailableENodes;
  public int AvailableNodeCount;
  public Vector3 BoxSize;
  public float ChanceToEmit = 100f;
  public Vector3 CircleDir;
  public Transform ClientTransform;
  public Color Color1 = Color.white;
  public Color Color2;
  public Color Color3;
  public Color Color4;
  public bool ColorAffectorEnable;
  public int ColorAffectType;
  public float ColorGradualTimeLength = 1f;
  public COLOR_GRADUAL_TYPE ColorGradualType;
  public int Cols = 1;
  public float DeltaRot;
  public float DeltaScaleX;
  public float DeltaScaleY;
  public float DiffDistance = 0.1f;
  public int EanIndex;
  public string EanPath = "none";
  public float EmitDelay;
  public float EmitDuration = 10f;
  public int EmitLoop = 1;
  public Vector3 EmitPoint;
  public int EmitRate = 20;
  protected Emitter emitter;
  public int EmitType;
  public bool IsEmitByDistance;
  public bool IsNodeLifeLoop = true;
  public bool IsRandomDir;
  public bool JetAffectorEnable;
  public float JetMax;
  public float JetMin;
  public Vector3 LastClientPos;
  public Vector3 LinearForce;
  public bool LinearForceAffectorEnable;
  public float LinearMagnitude = 1f;
  public float LineLengthLeft = -1f;
  public float LineLengthRight = 1f;
  public int LoopCircles = -1;
  protected Camera MainCamera;
  public Material Material;
  public int MaxENodes = 1;
  public float MaxFps = 60f;
  public int MaxRibbonElements = 6;
  public float NodeLifeMax = 1f;
  public float NodeLifeMin = 1f;
  public Vector2 OriLowerLeftUV = Vector2.zero;
  public int OriPoint;
  public int OriRotationMax;
  public int OriRotationMin;
  public float OriScaleXMax = 1f;
  public float OriScaleXMin = 1f;
  public float OriScaleYMax = 1f;
  public float OriScaleYMin = 1f;
  public float OriSpeed;
  public Vector2 OriUVDimensions = Vector2.one;
  public Vector3 OriVelocityAxis;
  public float Radius;
  public bool RandomOriRot;
  public bool RandomOriScale;
  public int RenderType;
  public float RibbonLen = 1f;
  public float RibbonWidth = 0.5f;
  public bool RotAffectorEnable;
  public AnimationCurve RotateCurve;
  public RSTYPE RotateType;
  public int Rows = 1;
  public bool ScaleAffectorEnable;
  public RSTYPE ScaleType;
  public AnimationCurve ScaleXCurve;
  public AnimationCurve ScaleYCurve;
  public float SpriteHeight = 1f;
  public int SpriteType;
  public int SpriteUVStretch;
  public float SpriteWidth = 1f;
  public float StartTime;
  public int StretchType;
  public bool SyncClient;
  public float TailDistance;
  public bool UseAttractCurve;
  public bool UseVortexCurve;
  public bool UVAffectorEnable;
  public float UVTime = 30f;
  public int UVType;
  public VertexPool Vertexpool;
  public bool VortexAffectorEnable;
  public AnimationCurve VortexCurve;
  public Vector3 VortexDirection;
  public float VortexMag = 0.1f;

  public void AddActiveNode(EffectNode node)
  {
    if (this.AvailableNodeCount == 0)
      Debug.LogError((object) "out index!");
    if (this.AvailableENodes[node.Index] == null)
      return;
    this.ActiveENodes[node.Index] = node;
    this.AvailableENodes[node.Index] = (EffectNode) null;
    --this.AvailableNodeCount;
  }

  protected void AddNodes(int num)
  {
    int num1 = 0;
    for (int index = 0; index < this.MaxENodes && num1 != num; ++index)
    {
      EffectNode availableEnode = this.AvailableENodes[index];
      if (availableEnode != null)
      {
        this.AddActiveNode(availableEnode);
        ++num1;
        this.emitter.SetEmitPosition(availableEnode);
        float life = !this.IsNodeLifeLoop ? Random.Range(this.NodeLifeMin, this.NodeLifeMax) : -1f;
        Vector3 emitRotation = this.emitter.GetEmitRotation(availableEnode);
        availableEnode.Init(((Vector3) ref emitRotation).normalized, this.OriSpeed, life, Random.Range(this.OriRotationMin, this.OriRotationMax), Random.Range(this.OriScaleXMin, this.OriScaleXMax), Random.Range(this.OriScaleYMin, this.OriScaleYMax), this.Color1, this.OriLowerLeftUV, this.OriUVDimensions);
      }
    }
  }

  public void FixedUpdateCustom()
  {
    this.AddNodes(this.emitter.GetNodes());
    for (int index = 0; index < this.MaxENodes; ++index)
      this.ActiveENodes[index]?.Update();
  }

  public RibbonTrail GetRibbonTrail() => !(this.ActiveENodes == null | this.ActiveENodes.Length != 1) && this.MaxENodes == 1 && this.RenderType == 1 ? this.ActiveENodes[0].Ribbon : (RibbonTrail) null;

  public VertexPool GetVertexPool() => this.Vertexpool;

  protected void Init()
  {
    this.AvailableENodes = new EffectNode[this.MaxENodes];
    this.ActiveENodes = new EffectNode[this.MaxENodes];
    for (int index = 0; index < this.MaxENodes; ++index)
    {
      EffectNode node = new EffectNode(index, this.ClientTransform, this.SyncClient, this);
      ArrayList afts = this.InitAffectors(node);
      node.SetAffectorList(afts);
      if (this.RenderType == 0)
        node.SetType(this.SpriteWidth, this.SpriteHeight, (STYPE) this.SpriteType, (ORIPOINT) this.OriPoint, this.SpriteUVStretch, this.MaxFps);
      else
        node.SetType(this.RibbonWidth, this.MaxRibbonElements, this.RibbonLen, this.ClientTransform.position, this.StretchType, this.MaxFps);
      this.AvailableENodes[index] = node;
    }
    this.AvailableNodeCount = this.MaxENodes;
    this.emitter = new Emitter(this);
  }

  protected ArrayList InitAffectors(EffectNode node)
  {
    ArrayList arrayList = new ArrayList();
    if (this.UVAffectorEnable)
    {
      UVAnimation frame = new UVAnimation();
      Texture texture = this.Vertexpool.GetMaterial().GetTexture("_MainTex");
      if (this.UVType == 2)
      {
        frame.BuildFromFile(this.EanPath, this.EanIndex, this.UVTime, texture);
        this.OriLowerLeftUV = frame.frames[0];
        this.OriUVDimensions = frame.UVDimensions[0];
      }
      else if (this.UVType == 1)
      {
        float num1 = (float) (texture.width / this.Cols);
        float num2 = (float) (texture.height / this.Rows);
        Vector2 cellSize;
        // ISSUE: explicit constructor call
        ((Vector2) ref cellSize).\u002Ector(num1 / (float) texture.width, num2 / (float) texture.height);
        Vector2 start;
        // ISSUE: explicit constructor call
        ((Vector2) ref start).\u002Ector(0.0f, 1f);
        frame.BuildUVAnim(start, cellSize, this.Cols, this.Rows, this.Cols * this.Rows);
        this.OriLowerLeftUV = start;
        this.OriUVDimensions = cellSize;
        this.OriUVDimensions.y = -this.OriUVDimensions.y;
      }
      if (frame.frames.Length == 1)
      {
        this.OriLowerLeftUV = frame.frames[0];
        this.OriUVDimensions = frame.UVDimensions[0];
      }
      else
      {
        frame.loopCycles = this.LoopCircles;
        Affector affector = (Affector) new UVAffector(frame, this.UVTime, node);
        arrayList.Add((object) affector);
      }
    }
    if (this.RotAffectorEnable && this.RotateType != RSTYPE.NONE)
    {
      Affector affector = this.RotateType != RSTYPE.CURVE ? (Affector) new RotateAffector(this.DeltaRot, node) : (Affector) new RotateAffector(this.RotateCurve, node);
      arrayList.Add((object) affector);
    }
    if (this.ScaleAffectorEnable && this.ScaleType != RSTYPE.NONE)
    {
      Affector affector = this.ScaleType != RSTYPE.CURVE ? (Affector) new ScaleAffector(this.DeltaScaleX, this.DeltaScaleY, node) : (Affector) new ScaleAffector(this.ScaleXCurve, this.ScaleYCurve, node);
      arrayList.Add((object) affector);
    }
    if (this.ColorAffectorEnable && this.ColorAffectType != 0)
    {
      ColorAffector colorAffector;
      if (this.ColorAffectType == 2)
        colorAffector = new ColorAffector(new Color[4]
        {
          this.Color1,
          this.Color2,
          this.Color3,
          this.Color4
        }, this.ColorGradualTimeLength, this.ColorGradualType, node);
      else
        colorAffector = new ColorAffector(new Color[2]
        {
          this.Color1,
          this.Color2
        }, this.ColorGradualTimeLength, this.ColorGradualType, node);
      arrayList.Add((object) colorAffector);
    }
    if (this.LinearForceAffectorEnable)
    {
      Affector affector = (Affector) new LinearForceAffector(Vector3.op_Multiply(((Vector3) ref this.LinearForce).normalized, this.LinearMagnitude), node);
      arrayList.Add((object) affector);
    }
    if (this.JetAffectorEnable)
    {
      Affector affector = (Affector) new JetAffector(this.JetMin, this.JetMax, node);
      arrayList.Add((object) affector);
    }
    if (this.VortexAffectorEnable)
    {
      Affector affector = !this.UseVortexCurve ? (Affector) new VortexAffector(this.VortexMag, this.VortexDirection, node) : (Affector) new VortexAffector(this.VortexCurve, this.VortexDirection, node);
      arrayList.Add((object) affector);
    }
    if (this.AttractionAffectorEnable)
    {
      Affector affector = !this.UseVortexCurve ? (Affector) new AttractionForceAffector(this.AttractMag, this.AttractionPosition, node) : (Affector) new AttractionForceAffector(this.AttractionCurve, this.AttractionPosition, node);
      arrayList.Add((object) affector);
    }
    return arrayList;
  }

  private void OnDrawGizmosSelected()
  {
  }

  public void RemoveActiveNode(EffectNode node)
  {
    if (this.AvailableNodeCount == this.MaxENodes)
      Debug.LogError((object) "out index!");
    if (this.ActiveENodes[node.Index] == null)
      return;
    this.ActiveENodes[node.Index] = (EffectNode) null;
    this.AvailableENodes[node.Index] = node;
    ++this.AvailableNodeCount;
  }

  public void Reset()
  {
    for (int index = 0; index < this.MaxENodes; ++index)
    {
      if (this.ActiveENodes == null)
        return;
      EffectNode activeEnode = this.ActiveENodes[index];
      if (activeEnode != null)
      {
        activeEnode.Reset();
        this.RemoveActiveNode(activeEnode);
      }
    }
    this.emitter.Reset();
  }

  public void StartCustom()
  {
    if (Object.op_Equality((Object) this.MainCamera, (Object) null))
      this.MainCamera = Camera.main;
    this.Init();
    this.LastClientPos = this.ClientTransform.position;
  }
}
