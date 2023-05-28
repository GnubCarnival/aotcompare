// Decompiled with JetBrains decompiler
// Type: UI.KeybindSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class KeybindSettingElement : BaseSettingElement
  {
    private List<Text> _buttonLabels = new List<Text>();
    private KeybindPopup _keybindPopup;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.Keybind
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      KeybindPopup keybindPopup,
      string tooltip,
      float elementWidth,
      float elementHeight,
      int bindCount)
    {
      this._keybindPopup = keybindPopup;
      for (int index = 0; index < bindCount; ++index)
        this.CreateKeybindButton(index, style, elementWidth, elementHeight);
      this.Setup(setting, style, title, tooltip);
    }

    private void CreateKeybindButton(int index, ElementStyle style, float width, float height)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      KeybindSettingElement.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new KeybindSettingElement.\u003C\u003Ec__DisplayClass5_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass50.index = index;
      GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("KeybindButton");
      Text component = ((Component) gameObject.transform.Find("Text")).GetComponent<Text>();
      ((Graphic) component).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "KeybindTextColor");
      component.fontSize = style.FontSize;
      gameObject.GetComponent<LayoutElement>().preferredWidth = width;
      gameObject.GetComponent<LayoutElement>().preferredHeight = height;
      gameObject.transform.SetParent(((Component) this).transform, false);
      // ISSUE: method pointer
      ((UnityEvent) gameObject.GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass50, __methodptr(\u003CCreateKeybindButton\u003Eb__0)));
      ((Selectable) gameObject.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Keybind");
      this._buttonLabels.Add(component);
    }

    protected void OnButtonClicked(int index) => this._keybindPopup.Show(((KeybindSetting) this._setting).InputKeys[index], this._buttonLabels[index]);

    public override void SyncElement()
    {
      for (int index = 0; index < this._buttonLabels.Count; ++index)
        this._buttonLabels[index].text = ((KeybindSetting) this._setting).InputKeys[index].ToString();
    }
  }
}
