// Decompiled with JetBrains decompiler
// Type: EffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class EffectController : MonoBehaviour
{
  protected XffectCache EffectCache;
  public Transform ObjectCache;

  protected Vector3 GetFaceDirection() => ((Component) this).transform.TransformDirection(Vector3.forward);

  private void OnEffect(string eftname)
  {
    switch (eftname)
    {
      case "lightning":
        for (int index = 0; index < 9; ++index)
        {
          Xffect component = ((Component) this.EffectCache.GetObject(eftname)).GetComponent<Xffect>();
          Vector3 zero = Vector3.zero;
          zero.x = Random.Range(-2.2f, 2.3f);
          zero.z = Random.Range(-2.1f, 2.1f);
          component.SetEmitPosition(zero);
          component.Active();
        }
        break;
      case "cyclone":
        Xffect component1 = ((Component) this.EffectCache.GetObject(eftname)).GetComponent<Xffect>();
        Xffect xffect = component1;
        Vector3 faceDirection = this.GetFaceDirection();
        Vector3 normalized = ((Vector3) ref faceDirection).normalized;
        xffect.SetDirectionAxis(normalized);
        component1.Active();
        break;
      case "crystal":
        ((Component) this.EffectCache.GetObject("crystal_surround")).GetComponent<Xffect>().Active();
        Xffect component2 = ((Component) this.EffectCache.GetObject("crystal")).GetComponent<Xffect>();
        component2.SetEmitPosition(new Vector3(0.0f, 1.9f, 1.4f));
        component2.Active();
        Xffect component3 = ((Component) this.EffectCache.GetObject("crystal_lightn")).GetComponent<Xffect>();
        component3.SetDirectionAxis(new Vector3(-1.5f, 1.8f, 0.0f));
        component3.Active();
        Xffect component4 = ((Component) this.EffectCache.GetObject("crystal")).GetComponent<Xffect>();
        component4.SetEmitPosition(new Vector3(0.0f, 1.5f, -1.2f));
        component4.Active();
        Xffect component5 = ((Component) this.EffectCache.GetObject("crystal_lightn")).GetComponent<Xffect>();
        component5.SetDirectionAxis(new Vector3(1.4f, 1.4f, 0.0f));
        component5.Active();
        break;
      default:
        ((Component) this.EffectCache.GetObject(eftname)).GetComponent<Xffect>().Active();
        break;
    }
  }

  private void OnGUI()
  {
    GUI.Box(new Rect(0.0f, 0.0f, 100f, 225f), "Effect List");
    GUI.Label(new Rect(150f, 0.0f, 350f, 25f), "alt+left mouse button to rotation.  mouse wheel to zoom.");
    if (GUI.Button(new Rect(10f, 20f, 80f, 20f), "Effect1"))
      this.OnEffect("crystal");
    if (GUI.Button(new Rect(10f, 45f, 80f, 20f), "Effect2"))
      this.OnEffect("rage_explode");
    if (GUI.Button(new Rect(10f, 70f, 80f, 20f), "Effect3"))
      this.OnEffect("cyclone");
    if (GUI.Button(new Rect(10f, 95f, 80f, 20f), "Effect4"))
      this.OnEffect("lightning");
    if (GUI.Button(new Rect(10f, 120f, 80f, 20f), "Effect5"))
      this.OnEffect("hit");
    if (GUI.Button(new Rect(10f, 145f, 80f, 20f), "Effect6"))
      this.OnEffect("firebody");
    if (GUI.Button(new Rect(10f, 170f, 80f, 20f), "Effect7"))
      this.OnEffect("explode");
    if (!GUI.Button(new Rect(10f, 195f, 80f, 20f), "Effect8"))
      return;
    this.OnEffect("rain");
  }

  private void Start() => this.EffectCache = ((Component) this.ObjectCache).GetComponent<XffectCache>();
}
