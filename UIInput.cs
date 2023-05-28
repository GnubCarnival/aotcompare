// Decompiled with JetBrains decompiler
// Type: UIInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
  public Color activeColor = Color.white;
  public bool autoCorrect;
  public string caratChar = "|";
  public static UIInput current;
  public GameObject eventReceiver;
  public string functionName = "OnSubmit";
  public bool isPassword;
  public UILabel label;
  public int maxChars;
  private Color mDefaultColor = Color.white;
  private string mDefaultText = string.Empty;
  private bool mDoInit = true;
  private string mLastIME = string.Empty;
  private UIWidget.Pivot mPivot = UIWidget.Pivot.Left;
  private float mPosition;
  private string mText = string.Empty;
  public UIInput.OnSubmit onSubmit;
  public GameObject selectOnTab;
  public UIInput.KeyboardType type;
  public bool useLabelTextAtStart;
  public UIInput.Validator validator;

  private void Append(string input)
  {
    int index = 0;
    for (int length = input.Length; index < length; ++index)
    {
      char nextChar = input[index];
      if (nextChar != '\b')
      {
        if (nextChar == '\n' || nextChar == '\r')
        {
          if ((UICamera.current.submitKey0 == 13 || UICamera.current.submitKey1 == 13) && (!this.label.multiLine || !Input.GetKey((KeyCode) 306) && !Input.GetKey((KeyCode) 305)))
          {
            UIInput.current = this;
            if (this.onSubmit != null)
              this.onSubmit(this.mText);
            if (Object.op_Equality((Object) this.eventReceiver, (Object) null))
              this.eventReceiver = ((Component) this).gameObject;
            this.eventReceiver.SendMessage(this.functionName, (object) this.mText, (SendMessageOptions) 1);
            UIInput.current = (UIInput) null;
            this.selected = false;
            return;
          }
          if (this.validator != null)
            nextChar = this.validator(this.mText, nextChar);
          switch (nextChar)
          {
            case char.MinValue:
              continue;
            case '\n':
            case '\r':
              if (this.label.multiLine)
              {
                this.mText += "\n";
                break;
              }
              break;
            default:
              this.mText += nextChar.ToString();
              break;
          }
          ((Component) this).SendMessage("OnInputChanged", (object) this, (SendMessageOptions) 1);
        }
        else if (nextChar >= ' ')
        {
          if (this.validator != null)
            nextChar = this.validator(this.mText, nextChar);
          if (nextChar != char.MinValue)
          {
            this.mText += nextChar.ToString();
            ((Component) this).SendMessage("OnInputChanged", (object) this, (SendMessageOptions) 1);
          }
        }
      }
      else if (this.mText.Length > 0)
      {
        this.mText = this.mText.Substring(0, this.mText.Length - 1);
        ((Component) this).SendMessage("OnInputChanged", (object) this, (SendMessageOptions) 1);
      }
    }
    this.UpdateLabel();
  }

  protected void Init()
  {
    this.maxChars = 100;
    this.initMain();
  }

  protected void initMain()
  {
    this.maxChars = 100;
    if (!this.mDoInit)
      return;
    this.mDoInit = false;
    if (Object.op_Equality((Object) this.label, (Object) null))
      this.label = ((Component) this).GetComponentInChildren<UILabel>();
    if (Object.op_Inequality((Object) this.label, (Object) null))
    {
      if (this.useLabelTextAtStart)
        this.mText = this.label.text;
      this.mDefaultText = this.label.text;
      this.mDefaultColor = this.label.color;
      this.label.supportEncoding = false;
      this.label.password = this.isPassword;
      this.mPivot = this.label.pivot;
      this.mPosition = this.label.cachedTransform.localPosition.x;
    }
    else
      ((Behaviour) this).enabled = false;
  }

  private void OnDisable()
  {
    if (!UICamera.IsHighlighted(((Component) this).gameObject))
      return;
    this.OnSelect(false);
  }

  private void OnEnable()
  {
    if (!UICamera.IsHighlighted(((Component) this).gameObject))
      return;
    this.OnSelect(true);
  }

  private void OnInput(string input)
  {
    if (this.mDoInit)
      this.initMain();
    if (!this.selected || !((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject) || Application.platform == 11 || Application.platform == 8)
      return;
    this.Append(input);
  }

  private void OnSelect(bool isSelected)
  {
    if (this.mDoInit)
      this.initMain();
    if (!Object.op_Inequality((Object) this.label, (Object) null) || !((Behaviour) this).enabled || !NGUITools.GetActive(((Component) this).gameObject))
      return;
    if (isSelected)
    {
      this.mText = this.useLabelTextAtStart || !(this.label.text == this.mDefaultText) ? this.label.text : string.Empty;
      this.label.color = this.activeColor;
      if (this.isPassword)
        this.label.password = true;
      Input.imeCompositionMode = (IMECompositionMode) 1;
      Transform cachedTransform = this.label.cachedTransform;
      Vector3 vector3_1 = Vector2.op_Implicit(this.label.pivotOffset);
      vector3_1.y += this.label.relativeSize.y;
      Vector3 vector3_2 = cachedTransform.TransformPoint(vector3_1);
      Input.compositionCursorPos = Vector2.op_Implicit(UICamera.currentCamera.WorldToScreenPoint(vector3_2));
      this.UpdateLabel();
    }
    else
    {
      if (string.IsNullOrEmpty(this.mText))
      {
        this.label.text = this.mDefaultText;
        this.label.color = this.mDefaultColor;
        if (this.isPassword)
          this.label.password = false;
      }
      else
        this.label.text = this.mText;
      this.label.showLastPasswordChar = false;
      Input.imeCompositionMode = (IMECompositionMode) 2;
      this.RestoreLabel();
    }
  }

  private void RestoreLabel()
  {
    if (!Object.op_Inequality((Object) this.label, (Object) null))
      return;
    this.label.pivot = this.mPivot;
    Vector3 localPosition = this.label.cachedTransform.localPosition;
    localPosition.x = this.mPosition;
    this.label.cachedTransform.localPosition = localPosition;
  }

  private void Update()
  {
    if (!this.selected)
      return;
    if (Object.op_Inequality((Object) this.selectOnTab, (Object) null) && Input.GetKeyDown((KeyCode) 9))
      UICamera.selectedObject = this.selectOnTab;
    if (Input.GetKeyDown((KeyCode) 118) && (Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305)))
      this.Append(NGUITools.clipboard);
    if (!(this.mLastIME != Input.compositionString))
      return;
    this.mLastIME = Input.compositionString;
    this.UpdateLabel();
  }

  private void UpdateLabel()
  {
    if (this.mDoInit)
      this.initMain();
    if (this.maxChars > 0 && this.mText.Length > this.maxChars)
      this.mText = this.mText.Substring(0, this.maxChars);
    if (!Object.op_Inequality((Object) this.label.font, (Object) null))
      return;
    string text;
    if (this.isPassword && this.selected)
    {
      string empty = string.Empty;
      int num = 0;
      for (int length = this.mText.Length; num < length; ++num)
        empty += "*";
      text = empty + Input.compositionString + this.caratChar;
    }
    else
      text = !this.selected ? this.mText : this.mText + Input.compositionString + this.caratChar;
    this.label.supportEncoding = false;
    if (!this.label.shrinkToFit)
    {
      if (this.label.multiLine)
      {
        text = this.label.font.WrapText(text, (float) this.label.lineWidth / this.label.cachedTransform.localScale.x, 0, false, UIFont.SymbolStyle.None);
      }
      else
      {
        string endOfLineThatFits = this.label.font.GetEndOfLineThatFits(text, (float) this.label.lineWidth / this.label.cachedTransform.localScale.x, false, UIFont.SymbolStyle.None);
        if (endOfLineThatFits != text)
        {
          text = endOfLineThatFits;
          Vector3 localPosition = this.label.cachedTransform.localPosition;
          localPosition.x = this.mPosition + (float) this.label.lineWidth;
          if (this.mPivot == UIWidget.Pivot.Left)
            this.label.pivot = UIWidget.Pivot.Right;
          else if (this.mPivot == UIWidget.Pivot.TopLeft)
            this.label.pivot = UIWidget.Pivot.TopRight;
          else if (this.mPivot == UIWidget.Pivot.BottomLeft)
            this.label.pivot = UIWidget.Pivot.BottomRight;
          this.label.cachedTransform.localPosition = localPosition;
        }
        else
          this.RestoreLabel();
      }
    }
    this.label.text = text;
    this.label.showLastPasswordChar = this.selected;
  }

  public string defaultText
  {
    get => this.mDefaultText;
    set
    {
      if (this.label.text == this.mDefaultText)
        this.label.text = value;
      this.mDefaultText = value;
    }
  }

  public bool selected
  {
    get => Object.op_Equality((Object) UICamera.selectedObject, (Object) ((Component) this).gameObject);
    set
    {
      if (!value && Object.op_Equality((Object) UICamera.selectedObject, (Object) ((Component) this).gameObject))
      {
        UICamera.selectedObject = (GameObject) null;
      }
      else
      {
        if (!value)
          return;
        UICamera.selectedObject = ((Component) this).gameObject;
      }
    }
  }

  public virtual string text
  {
    get
    {
      if (this.mDoInit)
        this.initMain();
      return this.mText;
    }
    set
    {
      if (this.mDoInit)
        this.initMain();
      this.mText = value;
      if (!Object.op_Inequality((Object) this.label, (Object) null))
        return;
      if (string.IsNullOrEmpty(value))
        value = this.mDefaultText;
      this.label.supportEncoding = false;
      this.label.text = !this.selected ? value : value + this.caratChar;
      this.label.showLastPasswordChar = this.selected;
      this.label.color = this.selected || value != this.mDefaultText ? this.activeColor : this.mDefaultColor;
    }
  }

  public enum KeyboardType
  {
    Default,
    ASCIICapable,
    NumbersAndPunctuation,
    URL,
    NumberPad,
    PhonePad,
    NamePhonePad,
    EmailAddress,
  }

  public delegate void OnSubmit(string inputString);

  public delegate char Validator(string currentText, char nextChar);
}
