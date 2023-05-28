// Decompiled with JetBrains decompiler
// Type: Xffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Xffect")]
public class Xffect : MonoBehaviour
{
  private List<EffectLayer> EflList = new List<EffectLayer>();
  protected float ElapsedTime;
  public float LifeTime = -1f;
  private Dictionary<string, VertexPool> MatDic = new Dictionary<string, VertexPool>();

  public void Active()
  {
    foreach (Component component in ((Component) this).transform)
      component.gameObject.SetActive(true);
    ((Component) this).gameObject.SetActive(true);
    this.ElapsedTime = 0.0f;
  }

  private void Awake() => this.Initialize();

  public void DeActive()
  {
    foreach (Component component in ((Component) this).transform)
      component.gameObject.SetActive(false);
    ((Component) this).gameObject.SetActive(false);
  }

  public void Initialize()
  {
    if (this.EflList.Count > 0)
      return;
    foreach (Component component1 in ((Component) this).transform)
    {
      EffectLayer component2 = (EffectLayer) component1.GetComponent(typeof (EffectLayer));
      if (Object.op_Inequality((Object) component2, (Object) null) && Object.op_Inequality((Object) component2.Material, (Object) null))
      {
        Material material = component2.Material;
        this.EflList.Add(component2);
        Transform transform = ((Component) this).transform.Find("mesh " + ((Object) material).name);
        if (Object.op_Inequality((Object) transform, (Object) null))
        {
          MeshFilter component3 = (MeshFilter) ((Component) transform).GetComponent(typeof (MeshFilter));
          MeshRenderer component4 = (MeshRenderer) ((Component) transform).GetComponent(typeof (MeshRenderer));
          component3.mesh.Clear();
          this.MatDic[((Object) material).name] = new VertexPool(component3.mesh, material);
        }
        if (!this.MatDic.ContainsKey(((Object) material).name))
        {
          GameObject gameObject = new GameObject("mesh " + ((Object) material).name)
          {
            transform = {
              parent = ((Component) this).transform
            }
          };
          gameObject.AddComponent("MeshFilter");
          gameObject.AddComponent("MeshRenderer");
          MeshFilter component5 = (MeshFilter) gameObject.GetComponent(typeof (MeshFilter));
          MeshRenderer component6 = (MeshRenderer) gameObject.GetComponent(typeof (MeshRenderer));
          ((Renderer) component6).castShadows = false;
          ((Renderer) component6).receiveShadows = false;
          ((Component) component6).renderer.material = material;
          this.MatDic[((Object) material).name] = new VertexPool(component5.mesh, material);
        }
      }
    }
    foreach (EffectLayer efl in this.EflList)
      efl.Vertexpool = this.MatDic[((Object) efl.Material).name];
  }

  private void LateUpdate()
  {
    foreach (KeyValuePair<string, VertexPool> keyValuePair in this.MatDic)
      keyValuePair.Value.LateUpdate();
    if ((double) this.ElapsedTime <= (double) this.LifeTime || (double) this.LifeTime < 0.0)
      return;
    foreach (EffectLayer efl in this.EflList)
      efl.Reset();
    this.DeActive();
    this.ElapsedTime = 0.0f;
  }

  private void OnDrawGizmosSelected()
  {
  }

  public void SetClient(Transform client)
  {
    foreach (EffectLayer efl in this.EflList)
      efl.ClientTransform = client;
  }

  public void SetDirectionAxis(Vector3 axis)
  {
    foreach (EffectLayer efl in this.EflList)
      efl.OriVelocityAxis = axis;
  }

  public void SetEmitPosition(Vector3 pos)
  {
    foreach (EffectLayer efl in this.EflList)
      efl.EmitPoint = pos;
  }

  private void Start()
  {
    ((Component) this).transform.position = Vector3.zero;
    ((Component) this).transform.rotation = Quaternion.identity;
    ((Component) this).transform.localScale = Vector3.one;
    foreach (Transform transform in ((Component) this).transform)
    {
      ((Component) transform).transform.position = Vector3.zero;
      ((Component) transform).transform.rotation = Quaternion.identity;
      ((Component) transform).transform.localScale = Vector3.one;
    }
    foreach (EffectLayer efl in this.EflList)
      efl.StartCustom();
  }

  private void Update()
  {
    this.ElapsedTime += Time.deltaTime;
    foreach (EffectLayer efl in this.EflList)
    {
      if ((double) this.ElapsedTime > (double) efl.StartTime)
        efl.FixedUpdateCustom();
    }
  }
}
