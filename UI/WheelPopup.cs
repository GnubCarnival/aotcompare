// Decompiled with JetBrains decompiler
// Type: UI.WheelPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class WheelPopup : BasePopup
  {
    private Text _centerText;
    private List<GameObject> _buttons = new List<GameObject>();
    public int SelectedItem;
    private UnityAction _callback;

    protected override float AnimationTime => 0.2f;

    protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

    public override void Setup(BasePanel parent = null)
    {
      this._centerText = ((Component) ((Component) this).transform.Find("Panel/Center/Label")).GetComponent<Text>();
      for (int index = 0; index < 8; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WheelPopup.\u003C\u003Ec__DisplayClass8_0 cDisplayClass80 = new WheelPopup.\u003C\u003Ec__DisplayClass8_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass80.\u003C\u003E4__this = this;
        this._buttons.Add(ElementFactory.InstantiateAndBind(((Component) this).transform.Find("Panel/Buttons"), "WheelButton").gameObject);
        // ISSUE: reference to a compiler-generated field
        cDisplayClass80.index = index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) this._buttons[cDisplayClass80.index].GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass80, __methodptr(\u003CSetup\u003Eb__0)));
      }
      ElementFactory.SetAnchor(this._buttons[0], (TextAnchor) 4, (TextAnchor) 7, new Vector2(0.0f, 180f));
      ElementFactory.SetAnchor(this._buttons[1], (TextAnchor) 4, (TextAnchor) 6, new Vector2(135f, 90f));
      ElementFactory.SetAnchor(this._buttons[2], (TextAnchor) 4, (TextAnchor) 3, new Vector2(180f, 0.0f));
      ElementFactory.SetAnchor(this._buttons[3], (TextAnchor) 4, (TextAnchor) 0, new Vector2(135f, -90f));
      ElementFactory.SetAnchor(this._buttons[4], (TextAnchor) 4, (TextAnchor) 1, new Vector2(0.0f, -180f));
      ElementFactory.SetAnchor(this._buttons[5], (TextAnchor) 4, (TextAnchor) 2, new Vector2(-135f, -90f));
      ElementFactory.SetAnchor(this._buttons[6], (TextAnchor) 4, (TextAnchor) 5, new Vector2(-180f, 0.0f));
      ElementFactory.SetAnchor(this._buttons[7], (TextAnchor) 4, (TextAnchor) 8, new Vector2(-135f, 90f));
    }

    public void Show(string openKey, List<string> options, UnityAction callback)
    {
      if (((Component) this).gameObject.activeSelf)
      {
        this.StopAllCoroutines();
        this.SetTransformAlpha(this.MaxFadeAlpha);
      }
      this.SetCenterText(openKey);
      this._callback = callback;
      for (int index = 0; index < options.Count; ++index)
      {
        this._buttons[index].SetActive(true);
        KeybindSetting setting = (KeybindSetting) SettingsManager.InputSettings.Interaction.Settings[(object) ("QuickSelect" + (index + 1).ToString())];
        ((Component) this._buttons[index].transform.Find("Text")).GetComponent<Text>().text = setting.ToString() + " - " + options[index];
      }
      for (int count = options.Count; count < this._buttons.Count; ++count)
        this._buttons[count].SetActive(false);
      this.Show();
    }

    private void SetCenterText(string openKey)
    {
      this._centerText.text = SettingsManager.InputSettings.Interaction.MenuNext.ToString() + " - " + UIManager.GetLocaleCommon("Next") + "\n";
      Text centerText = this._centerText;
      centerText.text = centerText.text + openKey + " - " + UIManager.GetLocaleCommon("Cancel");
    }

    private void OnButtonClick(int index)
    {
      this.SelectedItem = index;
      this._callback.Invoke();
    }

    private void Update()
    {
      for (int index = 0; index < 8; ++index)
      {
        if (((KeybindSetting) SettingsManager.InputSettings.Interaction.Settings[(object) ("QuickSelect" + (index + 1).ToString())]).GetKeyDown())
          this.OnButtonClick(index);
      }
    }
  }
}
