// Decompiled with JetBrains decompiler
// Type: EffectNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using UnityEngine;

public class EffectNode
{
  public float Acceleration;
  protected ArrayList AffectorList;
  public Transform ClientTrans;
  public Color Color;
  protected Vector3 CurDirection;
  protected Vector3 CurWorldPos;
  protected float ElapsedTime;
  public int Index;
  protected Vector3 LastWorldPos = Vector3.zero;
  protected float LifeTime;
  public Vector2 LowerLeftUV;
  protected Vector3 OriDirection;
  protected int OriRotateAngle;
  protected float OriScaleX;
  protected float OriScaleY;
  public EffectLayer Owner;
  public Vector3 Position;
  public RibbonTrail Ribbon;
  public float RotateAngle;
  public Vector2 Scale;
  public Sprite Sprite;
  public bool SyncClient;
  protected int Type;
  public Vector2 UVDimensions;
  public Vector3 Velocity;

  public EffectNode(int index, Transform clienttrans, bool sync, EffectLayer owner)
  {
    this.Index = index;
    this.ClientTrans = clienttrans;
    this.SyncClient = sync;
    this.Owner = owner;
    this.LowerLeftUV = Vector2.zero;
    this.UVDimensions = Vector2.one;
    this.Scale = Vector2.one;
    this.RotateAngle = 0.0f;
    this.Color = Color.white;
  }

  public float GetElapsedTime() => this.ElapsedTime;

  public float GetLifeTime() => this.LifeTime;

  public Vector3 GetLocalPosition() => this.Position;

  public void Init(
    Vector3 oriDir,
    float speed,
    float life,
    int oriRot,
    float oriScaleX,
    float oriScaleY,
    Color oriColor,
    Vector2 oriLowerUv,
    Vector2 oriUVDimension)
  {
    this.OriDirection = oriDir;
    this.LifeTime = life;
    this.OriRotateAngle = oriRot;
    this.OriScaleX = oriScaleX;
    this.OriScaleY = oriScaleY;
    this.Color = oriColor;
    this.ElapsedTime = 0.0f;
    this.Velocity = Vector3.op_Multiply(this.OriDirection, speed);
    this.Acceleration = 0.0f;
    this.LowerLeftUV = oriLowerUv;
    this.UVDimensions = oriUVDimension;
    if (this.Type == 1)
    {
      this.Sprite.SetUVCoord(this.LowerLeftUV, this.UVDimensions);
      this.Sprite.SetColor(oriColor);
    }
    else if (this.Type == 2)
    {
      this.Ribbon.SetUVCoord(this.LowerLeftUV, this.UVDimensions);
      this.Ribbon.SetColor(oriColor);
      this.Ribbon.SetHeadPosition(Vector3.op_Addition(Vector3.op_Addition(this.ClientTrans.position, this.Position), Vector3.op_Multiply(((Vector3) ref this.OriDirection).normalized, this.Owner.TailDistance)));
      this.Ribbon.ResetElementsPos();
    }
    if (this.Type != 1)
      return;
    this.Sprite.SetRotationTo(this.OriDirection);
  }

  public void Remove() => this.Owner.RemoveActiveNode(this);

  public void Reset()
  {
    this.Position = Vector3.op_Multiply(Vector3.up, 9999f);
    this.Velocity = Vector3.zero;
    this.Acceleration = 0.0f;
    this.ElapsedTime = 0.0f;
    this.LastWorldPos = this.CurWorldPos = Vector3.zero;
    foreach (Affector affector in this.AffectorList)
      affector.Reset();
    if (this.Type == 1)
    {
      this.Sprite.SetRotation((float) this.OriRotateAngle);
      this.Sprite.SetPosition(this.Position);
      this.Sprite.SetColor(Color.clear);
      this.Sprite.Update(true);
      this.Scale = Vector2.one;
    }
    else
    {
      if (this.Type != 2)
        return;
      this.Ribbon.SetHeadPosition(Vector3.op_Addition(this.ClientTrans.position, Vector3.op_Multiply(((Vector3) ref this.OriDirection).normalized, this.Owner.TailDistance)));
      this.Ribbon.Reset();
      this.Ribbon.SetColor(Color.clear);
      this.Ribbon.UpdateVertices(Vector3.zero);
    }
  }

  public void SetAffectorList(ArrayList afts) => this.AffectorList = afts;

  public void SetLocalPosition(Vector3 pos) => this.Position = pos;

  public void SetType(
    float width,
    int maxelemnt,
    float len,
    Vector3 pos,
    int stretchType,
    float maxFps)
  {
    this.Type = 2;
    this.Ribbon = this.Owner.GetVertexPool().AddRibbonTrail(width, maxelemnt, len, pos, stretchType, maxFps);
  }

  public void SetType(
    float width,
    float height,
    STYPE type,
    ORIPOINT orip,
    int uvStretch,
    float maxFps)
  {
    this.Type = 1;
    this.Sprite = this.Owner.GetVertexPool().AddSprite(width, height, type, orip, Camera.main, uvStretch, maxFps);
  }

  public void Update()
  {
    this.ElapsedTime += Time.deltaTime;
    foreach (Affector affector in this.AffectorList)
      affector.Update();
    this.Position = Vector3.op_Addition(this.Position, Vector3.op_Multiply(this.Velocity, Time.deltaTime));
    if ((double) Mathf.Abs(this.Acceleration) > 0.0001)
      this.Velocity = Vector3.op_Addition(this.Velocity, Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref this.Velocity).normalized, this.Acceleration), Time.deltaTime));
    this.CurWorldPos = !this.SyncClient ? this.Position : this.ClientTrans.TransformPoint(this.Position);
    if (this.Type == 1)
      this.UpdateSprite();
    else if (this.Type == 2)
      this.UpdateRibbonTrail();
    this.LastWorldPos = this.CurWorldPos;
    if ((double) this.ElapsedTime <= (double) this.LifeTime || (double) this.LifeTime <= 0.0)
      return;
    this.Reset();
    this.Remove();
  }

  public void UpdateRibbonTrail()
  {
    this.Ribbon.SetHeadPosition(this.CurWorldPos);
    if (this.Owner.UVAffectorEnable)
      this.Ribbon.SetUVCoord(this.LowerLeftUV, this.UVDimensions);
    this.Ribbon.SetColor(this.Color);
    this.Ribbon.Update();
  }

  public void UpdateSprite()
  {
    if (this.Owner.AlongVelocity)
    {
      Vector3 zero = Vector3.zero;
      if (!Vector3.op_Inequality(this.LastWorldPos, Vector3.zero))
        return;
      Vector3 vector3 = Vector3.op_Subtraction(this.CurWorldPos, this.LastWorldPos);
      if (Vector3.op_Inequality(vector3, Vector3.zero))
      {
        this.CurDirection = vector3;
        this.Sprite.SetRotationTo(this.CurDirection);
      }
    }
    this.Sprite.SetScale(this.Scale.x * this.OriScaleX, this.Scale.y * this.OriScaleY);
    if (this.Owner.ColorAffectorEnable)
      this.Sprite.SetColor(this.Color);
    if (this.Owner.UVAffectorEnable)
      this.Sprite.SetUVCoord(this.LowerLeftUV, this.UVDimensions);
    this.Sprite.SetRotation((float) this.OriRotateAngle + this.RotateAngle);
    this.Sprite.SetPosition(this.CurWorldPos);
    this.Sprite.Update(false);
  }
}
