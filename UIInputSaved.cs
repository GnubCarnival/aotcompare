// Decompiled with JetBrains decompiler
// Type: UIInputSaved
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Saved)")]
public class UIInputSaved : UIInput
{
  public string playerPrefsField;

  private void Awake()
  {
    this.onSubmit = new UIInput.OnSubmit(this.SaveToPlayerPrefs);
    if (string.IsNullOrEmpty(this.playerPrefsField) || !PlayerPrefs.HasKey(this.playerPrefsField))
      return;
    this.text = PlayerPrefs.GetString(this.playerPrefsField);
  }

  private void OnApplicationQuit() => this.SaveToPlayerPrefs(this.text);

  private void SaveToPlayerPrefs(string val)
  {
    if (string.IsNullOrEmpty(this.playerPrefsField))
      return;
    PlayerPrefs.SetString(this.playerPrefsField, val);
  }

  public override string text
  {
    get => base.text;
    set
    {
      base.text = value;
      this.SaveToPlayerPrefs(value);
    }
  }
}
