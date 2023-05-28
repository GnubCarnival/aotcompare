// Decompiled with JetBrains decompiler
// Type: StylishComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UI;
using UnityEngine;

public class StylishComponent : MonoBehaviour
{
  public GameObject bar;
  private int chainKillRank;
  private float[] chainRankMultiplier;
  private float chainTime;
  private float duration;
  private Vector3 exitPosition;
  private bool flip;
  private bool hasLostRank;
  public GameObject labelChain;
  public GameObject labelHits;
  public GameObject labelS;
  public GameObject labelS1;
  public GameObject labelS2;
  public GameObject labelsub;
  public GameObject labelTotal;
  private Vector3 originalPosition;
  private float R;
  private int styleHits;
  private float stylePoints;
  private int styleRank;
  private int[] styleRankDepletions;
  private int[] styleRankPoints;
  private string[,] styleRankText;
  private int styleTotalDamage;

  public StylishComponent()
  {
    this.styleRankText = new string[8, 2]
    {
      {
        "D",
        "eja Vu"
      },
      {
        "C",
        "asual"
      },
      {
        "B",
        "oppin!"
      },
      {
        "A",
        "mazing!"
      },
      {
        "S",
        "ensational!"
      },
      {
        "S",
        "pectacular!!"
      },
      {
        "S",
        "tylish!!!"
      },
      {
        "X",
        "TREEME!!!"
      }
    };
    this.chainRankMultiplier = new float[9]
    {
      1f,
      1.1f,
      1.2f,
      1.3f,
      1.5f,
      1.7f,
      2f,
      2.3f,
      2.5f
    };
    this.styleRankPoints = new int[7]
    {
      350,
      950,
      2450,
      4550,
      7000,
      15000,
      100000
    };
    this.styleRankDepletions = new int[8]
    {
      1,
      2,
      5,
      10,
      15,
      20,
      25,
      25
    };
  }

  private int GetRankPercentage()
  {
    if (this.styleRank > 0 && this.styleRank < this.styleRankPoints.Length)
      return (int) (((double) this.stylePoints - (double) this.styleRankPoints[this.styleRank - 1]) * 100.0 / (double) (this.styleRankPoints[this.styleRank] - this.styleRankPoints[this.styleRank - 1]));
    return this.styleRank == 0 ? (int) ((double) this.stylePoints * 100.0) / this.styleRankPoints[this.styleRank] : 100;
  }

  private int GetStyleDepletionRate() => this.styleRankDepletions[this.styleRank];

  public void reset()
  {
    this.styleTotalDamage = 0;
    this.chainKillRank = 0;
    this.chainTime = 0.0f;
    this.styleRank = 0;
    this.stylePoints = 0.0f;
    this.styleHits = 0;
  }

  public void OnResolutionChange()
  {
    this.setPosition();
    if ((double) this.stylePoints > 0.0)
      ((Component) this).transform.localPosition = this.originalPosition;
    else
      ((Component) this).transform.localPosition = this.exitPosition;
  }

  private void setPosition()
  {
    this.originalPosition = new Vector3((float) (int) ((double) Screen.width * 0.5 - 2.0), 0.0f, 0.0f);
    this.exitPosition = new Vector3((float) Screen.width, this.originalPosition.y, this.originalPosition.z);
  }

  private void SetRank()
  {
    int styleRank = this.styleRank;
    int index = 0;
    while (index < this.styleRankPoints.Length && (double) this.stylePoints > (double) this.styleRankPoints[index])
      ++index;
    this.styleRank = index >= this.styleRankPoints.Length ? this.styleRankPoints.Length : index;
    if (this.styleRank < styleRank)
    {
      if (this.hasLostRank)
      {
        this.stylePoints = 0.0f;
        this.styleHits = 0;
        this.styleTotalDamage = 0;
        this.styleRank = 0;
      }
      else
        this.hasLostRank = true;
    }
    else
    {
      if (this.styleRank <= styleRank)
        return;
      this.hasLostRank = false;
    }
  }

  private void setRankText()
  {
    this.labelS.GetComponent<UILabel>().text = this.styleRankText[this.styleRank, 0];
    if (this.styleRank == 5)
      this.labelS2.GetComponent<UILabel>().text = "[" + ColorSet.color_SS + "]S";
    else
      this.labelS2.GetComponent<UILabel>().text = string.Empty;
    if (this.styleRank == 6)
    {
      this.labelS2.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
      this.labelS1.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
    }
    else
      this.labelS1.GetComponent<UILabel>().text = string.Empty;
    if (this.styleRank == 0)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_D + "]D";
    if (this.styleRank == 1)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_C + "]C";
    if (this.styleRank == 2)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_B + "]B";
    if (this.styleRank == 3)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_A + "]A";
    if (this.styleRank == 4)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_S + "]S";
    if (this.styleRank == 5)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_SS + "]S";
    if (this.styleRank == 6)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
    if (this.styleRank == 7)
      this.labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_X + "]X";
    this.labelsub.GetComponent<UILabel>().text = this.styleRankText[this.styleRank, 1];
  }

  private void shakeUpdate()
  {
    if ((double) this.duration <= 0.0)
      return;
    this.duration -= Time.deltaTime;
    if (this.flip)
      ((Component) this).gameObject.transform.localPosition = Vector3.op_Addition(this.originalPosition, Vector3.op_Multiply(Vector3.up, this.R));
    else
      ((Component) this).gameObject.transform.localPosition = Vector3.op_Subtraction(this.originalPosition, Vector3.op_Multiply(Vector3.up, this.R));
    this.flip = !this.flip;
    if ((double) this.duration > 0.0)
      return;
    ((Component) this).gameObject.transform.localPosition = this.originalPosition;
  }

  private void Start()
  {
    this.setPosition();
    ((Component) this).transform.localPosition = this.exitPosition;
  }

  public void startShake(int R, float duration)
  {
    if ((double) this.duration >= (double) duration)
      return;
    this.R = (float) R;
    this.duration = duration;
  }

  public void Style(int damage)
  {
    if (damage != -1)
    {
      this.stylePoints += (float) (int) ((double) (damage + 200) * (double) this.chainRankMultiplier[this.chainKillRank]);
      this.styleTotalDamage += damage;
      this.chainKillRank = this.chainKillRank >= this.chainRankMultiplier.Length - 1 ? this.chainKillRank : this.chainKillRank + 1;
      this.chainTime = 5f;
      ++this.styleHits;
      this.SetRank();
    }
    else if ((double) this.stylePoints == 0.0)
    {
      ++this.stylePoints;
      this.SetRank();
    }
    this.startShake(5, 0.3f);
    this.setPosition();
    this.labelTotal.GetComponent<UILabel>().text = ((int) this.stylePoints).ToString();
    this.labelHits.GetComponent<UILabel>().text = this.styleHits.ToString() + (this.styleHits <= 1 ? "Hit" : "Hits");
    if (this.chainKillRank == 0)
      this.labelChain.GetComponent<UILabel>().text = string.Empty;
    else
      this.labelChain.GetComponent<UILabel>().text = "x" + this.chainRankMultiplier[this.chainKillRank].ToString() + "!";
  }

  private void Update()
  {
    if (GameMenu.Paused)
      return;
    if ((double) this.stylePoints > 0.0)
    {
      this.setRankText();
      this.bar.GetComponent<UISprite>().fillAmount = (float) this.GetRankPercentage() * 0.01f;
      this.stylePoints -= (float) ((double) this.GetStyleDepletionRate() * (double) Time.deltaTime * 10.0);
      this.SetRank();
    }
    else
      ((Component) this).transform.localPosition = Vector3.Lerp(((Component) this).transform.localPosition, this.exitPosition, Time.deltaTime * 3f);
    if ((double) this.chainTime > 0.0)
    {
      this.chainTime -= Time.deltaTime;
    }
    else
    {
      this.chainTime = 0.0f;
      this.chainKillRank = 0;
    }
    this.shakeUpdate();
  }
}
