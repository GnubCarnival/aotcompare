// Decompiled with JetBrains decompiler
// Type: ChatInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/Examples/Chat Input")]
[RequireComponent(typeof (UIInput))]
public class ChatInput : MonoBehaviour
{
  public bool fillWithDummyData;
  private bool mIgnoreNextEnter;
  private UIInput mInput;
  public UITextList textList;

  private void OnSubmit()
  {
    if (Object.op_Inequality((Object) this.textList, (Object) null))
    {
      string text = NGUITools.StripSymbols(this.mInput.text);
      if (!string.IsNullOrEmpty(text))
      {
        this.textList.Add(text);
        this.mInput.text = string.Empty;
        this.mInput.selected = false;
      }
    }
    this.mIgnoreNextEnter = true;
  }

  private void Start()
  {
    this.mInput = ((Component) this).GetComponent<UIInput>();
    if (!this.fillWithDummyData || !Object.op_Inequality((Object) this.textList, (Object) null))
      return;
    for (int index = 0; index < 30; ++index)
      this.textList.Add((index % 2 != 0 ? (object) "[AAAAAA]" : (object) "[FFFFFF]").ToString() + "This is an example paragraph for the text list, testing line " + (object) index + "[-]");
  }

  private void Update()
  {
    if (!Input.GetKeyUp((KeyCode) 13))
      return;
    if (!this.mIgnoreNextEnter && !this.mInput.selected)
      this.mInput.selected = true;
    this.mIgnoreNextEnter = false;
  }
}
