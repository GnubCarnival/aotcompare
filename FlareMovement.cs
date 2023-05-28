// Decompiled with JetBrains decompiler
// Type: FlareMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class FlareMovement : MonoBehaviour
{
  public string color;
  private GameObject hero;
  private GameObject hint;
  private bool nohint;
  private Vector3 offY;
  private float timer;

  public void dontShowHint()
  {
    Object.Destroy((Object) this.hint);
    this.nohint = true;
  }

  private void Start()
  {
    this.hero = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
    if (this.nohint || !Object.op_Inequality((Object) this.hero, (Object) null))
      return;
    this.hint = (GameObject) Object.Instantiate(Resources.Load("UI/" + this.color + "FlareHint"));
    this.offY = !(this.color == "Black") ? Vector3.op_Multiply(Vector3.up, 0.5f) : Vector3.op_Multiply(Vector3.up, 0.4f);
    this.hint.transform.parent = ((Component) this).transform.root;
    this.hint.transform.position = Vector3.op_Addition(this.hero.transform.position, this.offY);
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, this.hint.transform.position);
    this.hint.transform.rotation = Quaternion.Euler(-90f, Mathf.Atan2(-vector3.z, vector3.x) * 57.29578f + 180f, 0.0f);
    this.hint.transform.localScale = Vector3.zero;
    iTween.ScaleTo(this.hint, iTween.Hash((object) "x", (object) 1f, (object) "y", (object) 1f, (object) "z", (object) 1f, (object) "easetype", (object) iTween.EaseType.easeOutElastic, (object) "time", (object) 1f));
    iTween.ScaleTo(this.hint, iTween.Hash((object) "x", (object) 0, (object) "y", (object) 0, (object) "z", (object) 0, (object) "easetype", (object) iTween.EaseType.easeInBounce, (object) "time", (object) 0.5f, (object) "delay", (object) 2.5f));
  }

  private void Update()
  {
    this.timer += Time.deltaTime;
    if (Object.op_Inequality((Object) this.hint, (Object) null))
    {
      if ((double) this.timer < 3.0)
      {
        this.hint.transform.position = Vector3.op_Addition(this.hero.transform.position, this.offY);
        Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, this.hint.transform.position);
        this.hint.transform.rotation = Quaternion.Euler(-90f, Mathf.Atan2(-vector3.z, vector3.x) * 57.29578f + 180f, 0.0f);
      }
      else if (Object.op_Inequality((Object) this.hint, (Object) null))
        Object.Destroy((Object) this.hint);
    }
    if ((double) this.timer < 4.0)
      ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Addition(((Component) this).transform.forward, Vector3.op_Multiply(((Component) this).transform.up, 5f)), Time.deltaTime), 5f), (ForceMode) 2);
    else
      ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this).transform.up), Time.deltaTime), 7f), (ForceMode) 5);
  }
}
