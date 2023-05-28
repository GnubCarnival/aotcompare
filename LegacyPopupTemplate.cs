// Decompiled with JetBrains decompiler
// Type: LegacyPopupTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

internal class LegacyPopupTemplate
{
  private Color BorderColor;
  private Texture2D BackgroundTexture;
  private float PositionX;
  private float PositionY;
  private float Width;
  private float Height;
  private float BorderThickness;
  private float Padding;
  private Color ButtonColor;

  public LegacyPopupTemplate(
    Color borderColor,
    Texture2D bgTexture,
    Color buttonColor,
    float x,
    float y,
    float w,
    float h,
    float borderThickness)
  {
    this.BorderColor = borderColor;
    this.BackgroundTexture = bgTexture;
    this.ButtonColor = buttonColor;
    this.PositionX = x - w / 2f;
    this.PositionY = y - h / 2f;
    this.Width = w;
    this.Height = h;
    this.BorderThickness = borderThickness;
  }

  public void DrawPopup(string message, float messageWidth, float messageHeight)
  {
    this.DrawPopupBackground();
    GUI.Label(new Rect(this.PositionX + (float) (((double) this.Width - (double) messageWidth) * 0.5), this.PositionY + (float) (((double) this.Height - (double) messageHeight) * 0.5), messageWidth, messageHeight), message);
  }

  public bool DrawPopupWithButton(
    string message,
    float messageWidth,
    float messageHeight,
    string buttonMessage,
    float buttonWidth,
    float buttonHeight)
  {
    this.DrawPopupBackground();
    float num1 = (float) (((double) this.Width - (double) messageWidth) * 0.5);
    float num2 = (float) (((double) this.Width - (double) buttonWidth) * 0.5);
    float num3 = (float) (((double) this.Height - (double) messageHeight - (double) buttonHeight) / 3.0);
    GUI.Label(new Rect(this.PositionX + num1, this.PositionY + num3, messageWidth, messageHeight), message);
    float num4 = this.PositionX + num2;
    float num5 = this.PositionY + this.Height - buttonHeight - num3;
    GUI.backgroundColor = this.ButtonColor;
    return GUI.Button(new Rect(num4, num5, buttonWidth, buttonHeight), buttonMessage);
  }

  public bool[] DrawPopupWithTwoButtons(
    string message,
    float messageWidth,
    float messageHeight,
    string button1Message,
    float button1Width,
    string button2Message,
    float button2Width,
    float buttonHeight)
  {
    this.DrawPopupBackground();
    float num1 = (float) (((double) this.Width - (double) messageWidth) * 0.5);
    float num2 = (float) (((double) this.Width - (double) button1Width - (double) button2Width) / 3.0);
    float num3 = (float) (((double) this.Height - (double) messageHeight - (double) buttonHeight) / 3.0);
    GUI.Label(new Rect(this.PositionX + num1, this.PositionY + num3, messageWidth, messageHeight), message);
    float num4 = this.PositionX + num2;
    float num5 = num4 + button1Width + num2;
    float num6 = this.PositionY + this.Height - buttonHeight - num3;
    GUI.backgroundColor = this.ButtonColor;
    return new bool[2]
    {
      GUI.Button(new Rect(num4, num6, button1Width, buttonHeight), button1Message),
      GUI.Button(new Rect(num5, num6, button2Width, buttonHeight), button2Message)
    };
  }

  private void DrawPopupBackground()
  {
    GUI.backgroundColor = this.BorderColor;
    GUI.Box(new Rect(this.PositionX, this.PositionY, this.Width, this.Height), string.Empty);
    GUI.DrawTexture(new Rect(this.PositionX + this.BorderThickness, this.PositionY + this.BorderThickness, this.Width - 2f * this.BorderThickness, this.Height - 2f * this.BorderThickness), (Texture) this.BackgroundTexture);
  }
}
