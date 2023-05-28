// Decompiled with JetBrains decompiler
// Type: CharacterCreateAnimationControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

public class CharacterCreateAnimationControl : MonoBehaviour
{
  private string currentAnimation;
  private float interval = 10f;
  private HERO_SETUP setup;
  private float timeElapsed;

  private void play(string id)
  {
    this.currentAnimation = id;
    ((Component) this).animation.Play(id);
  }

  public void playAttack(string id)
  {
    string key = id;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (CharacterCreateAnimationControl.fswitchSmap0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CharacterCreateAnimationControl.fswitchSmap0 = new Dictionary<string, int>(7)
        {
          {
            "mikasa",
            0
          },
          {
            "levi",
            1
          },
          {
            "sasha",
            2
          },
          {
            "jean",
            3
          },
          {
            "marco",
            4
          },
          {
            "armin",
            5
          },
          {
            "petra",
            6
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (CharacterCreateAnimationControl.fswitchSmap0.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.currentAnimation = "attack3_1";
            break;
          case 1:
            this.currentAnimation = "attack5";
            break;
          case 2:
            this.currentAnimation = "special_sasha";
            break;
          case 3:
            this.currentAnimation = "grabbed_jean";
            break;
          case 4:
            this.currentAnimation = "special_marco_0";
            break;
          case 5:
            this.currentAnimation = "special_armin";
            break;
          case 6:
            this.currentAnimation = "special_petra";
            break;
        }
      }
    }
    ((Component) this).animation.Play(this.currentAnimation);
  }

  private void Start()
  {
    this.setup = ((Component) this).gameObject.GetComponent<HERO_SETUP>();
    this.currentAnimation = "stand_levi";
    this.play(this.currentAnimation);
  }

  public void toStand()
  {
    this.currentAnimation = this.setup.myCostume.sex != SEX.FEMALE ? "stand_levi" : "stand";
    ((Component) this).animation.CrossFade(this.currentAnimation, 0.1f);
    this.timeElapsed = 0.0f;
  }

  private void Update()
  {
    if (this.currentAnimation == "stand" || this.currentAnimation == "stand_levi")
    {
      this.timeElapsed += Time.deltaTime;
      if ((double) this.timeElapsed <= (double) this.interval)
        return;
      this.timeElapsed = 0.0f;
      if (Random.Range(1, 1000) < 350)
        this.play("salute");
      else if (Random.Range(1, 1000) < 350)
        this.play("supply");
      else
        this.play("dodge");
    }
    else
    {
      if ((double) ((Component) this).animation[this.currentAnimation].normalizedTime < 1.0)
        return;
      if (this.currentAnimation == "attack3_1")
        this.play("attack3_2");
      else if (this.currentAnimation == "special_sasha")
        this.play("run_sasha");
      else
        this.toStand();
    }
  }
}
