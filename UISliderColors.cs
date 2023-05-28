// Decompiled with JetBrains decompiler
// Type: UISliderColors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Slider Colors")]
[RequireComponent(typeof (UISlider))]
[ExecuteInEditMode]
public class UISliderColors : MonoBehaviour
{
  public Color[] colors = new Color[3]
  {
    Color.red,
    Color.yellow,
    Color.green
  };
  private UISlider mSlider;
  public UISprite sprite;

  private void Start()
  {
    this.mSlider = ((Component) this).GetComponent<UISlider>();
    this.Update();
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.sprite, (Object) null) || this.colors.Length == 0)
      return;
    float num1 = this.mSlider.sliderValue * (float) (this.colors.Length - 1);
    int index = Mathf.FloorToInt(num1);
    Color color = this.colors[0];
    if (index >= 0)
    {
      if (index + 1 < this.colors.Length)
      {
        float num2 = num1 - (float) index;
        color = Color.Lerp(this.colors[index], this.colors[index + 1], num2);
      }
      else
        color = index >= this.colors.Length ? this.colors[this.colors.Length - 1] : this.colors[index];
    }
    color.a = this.sprite.color.a;
    this.sprite.color = color;
  }
}
