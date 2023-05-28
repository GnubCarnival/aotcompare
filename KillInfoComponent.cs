// Decompiled with JetBrains decompiler
// Type: KillInfoComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class KillInfoComponent : MonoBehaviour
{
  private float alpha = 1f;
  private int col;
  public GameObject groupBig;
  public GameObject groupSmall;
  public GameObject labelNameLeft;
  public GameObject labelNameRight;
  public GameObject labelScore;
  public GameObject leftTitan;
  private float lifeTime = 8f;
  private float maxScale = 1.5f;
  private int offset = 24;
  public GameObject rightTitan;
  public GameObject slabelNameLeft;
  public GameObject slabelNameRight;
  public GameObject slabelScore;
  public GameObject sleftTitan;
  public GameObject spriteSkeleton;
  public GameObject spriteSword;
  public GameObject srightTitan;
  public GameObject sspriteSkeleton;
  public GameObject sspriteSword;
  private bool start;
  private float timeElapsed;

  public void destory() => this.timeElapsed = this.lifeTime;

  public void moveOn()
  {
    ++this.col;
    if (this.col > 4)
      this.timeElapsed = this.lifeTime;
    this.groupBig.SetActive(false);
    this.groupSmall.SetActive(true);
  }

  private void setAlpha(float alpha)
  {
    if (this.groupBig.activeInHierarchy)
    {
      this.labelScore.GetComponent<UILabel>().color = new Color(this.labelScore.GetComponent<UILabel>().color.r, this.labelScore.GetComponent<UILabel>().color.g, this.labelScore.GetComponent<UILabel>().color.b, alpha);
      this.leftTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
      this.rightTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
      this.labelNameLeft.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
      this.labelNameRight.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
      this.spriteSkeleton.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
      this.spriteSword.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
    }
    if (!this.groupSmall.activeInHierarchy)
      return;
    this.slabelScore.GetComponent<UILabel>().color = new Color(this.labelScore.GetComponent<UILabel>().color.r, this.labelScore.GetComponent<UILabel>().color.g, this.labelScore.GetComponent<UILabel>().color.b, alpha);
    this.sleftTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
    this.srightTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
    this.slabelNameLeft.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
    this.slabelNameRight.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
    this.sspriteSkeleton.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
    this.sspriteSword.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
  }

  public void show(bool isTitan1, string name1, bool isTitan2, string name2, int dmg = 0)
  {
    this.groupBig.SetActive(true);
    this.groupSmall.SetActive(true);
    if (!isTitan1)
    {
      this.leftTitan.SetActive(false);
      this.spriteSkeleton.SetActive(false);
      this.sleftTitan.SetActive(false);
      this.sspriteSkeleton.SetActive(false);
      Transform transform1 = this.labelNameLeft.transform;
      transform1.position = Vector3.op_Addition(transform1.position, new Vector3(18f, 0.0f, 0.0f));
      Transform transform2 = this.slabelNameLeft.transform;
      transform2.position = Vector3.op_Addition(transform2.position, new Vector3(16f, 0.0f, 0.0f));
    }
    else
    {
      this.spriteSword.SetActive(false);
      this.sspriteSword.SetActive(false);
      Transform transform3 = this.labelNameRight.transform;
      transform3.position = Vector3.op_Subtraction(transform3.position, new Vector3(18f, 0.0f, 0.0f));
      Transform transform4 = this.slabelNameRight.transform;
      transform4.position = Vector3.op_Subtraction(transform4.position, new Vector3(16f, 0.0f, 0.0f));
    }
    if (!isTitan2)
    {
      this.rightTitan.SetActive(false);
      this.srightTitan.SetActive(false);
    }
    this.labelNameLeft.GetComponent<UILabel>().text = name1;
    this.labelNameRight.GetComponent<UILabel>().text = name2;
    this.slabelNameLeft.GetComponent<UILabel>().text = name1;
    this.slabelNameRight.GetComponent<UILabel>().text = name2;
    if (dmg == 0)
    {
      this.labelScore.GetComponent<UILabel>().text = string.Empty;
      this.slabelScore.GetComponent<UILabel>().text = string.Empty;
    }
    else
    {
      this.labelScore.GetComponent<UILabel>().text = dmg.ToString();
      this.slabelScore.GetComponent<UILabel>().text = dmg.ToString();
      if (dmg > 1000)
      {
        this.labelScore.GetComponent<UILabel>().color = Color.red;
        this.slabelScore.GetComponent<UILabel>().color = Color.red;
      }
    }
    this.groupSmall.SetActive(false);
  }

  private void Start()
  {
    this.start = true;
    ((Component) this).transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
    ((Component) this).transform.localPosition = new Vector3(0.0f, (float) ((double) Screen.height * 0.5 - 100.0), 0.0f);
  }

  private void Update()
  {
    if (!this.start)
      return;
    this.timeElapsed += Time.deltaTime;
    if ((double) this.timeElapsed < 0.20000000298023224)
      ((Component) this).transform.localScale = Vector3.Lerp(((Component) this).transform.localScale, Vector3.op_Multiply(Vector3.one, this.maxScale), Time.deltaTime * 10f);
    else if ((double) this.timeElapsed < 1.0)
      ((Component) this).transform.localScale = Vector3.Lerp(((Component) this).transform.localScale, Vector3.one, Time.deltaTime * 10f);
    if ((double) this.timeElapsed > (double) this.lifeTime)
    {
      Transform transform = ((Component) this).transform;
      transform.position = Vector3.op_Addition(transform.position, new Vector3(0.0f, Time.deltaTime * 0.15f, 0.0f));
      this.alpha = (float) (1.0 - (double) Time.deltaTime * 45.0) + this.lifeTime - this.timeElapsed;
      this.setAlpha(this.alpha);
    }
    else
      ((Component) this).transform.localPosition = Vector3.Lerp(((Component) this).transform.localPosition, new Vector3(0.0f, -(float) ((int) (100.0 - (double) Screen.height * 0.5) + this.col * this.offset), 0.0f), Time.deltaTime * 10f);
    if ((double) this.timeElapsed <= (double) this.lifeTime + 0.5)
      return;
    Object.Destroy((Object) ((Component) this).gameObject);
  }
}
