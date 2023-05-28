// Decompiled with JetBrains decompiler
// Type: UI.DropdownSettingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
  internal class DropdownSettingElement : BaseSettingElement
  {
    protected GameObject _optionsPanel;
    protected GameObject _selectedButton;
    protected GameObject _selectedButtonLabel;
    protected string[] _options;
    protected float _currentScrollValue = 1f;
    protected Scrollbar _scrollBar;
    private Vector3 _optionsOffset;
    private UnityAction _onDropdownOptionSelect;
    private Vector3 _lastKnownPosition = Vector3.zero;

    protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
    {
      SettingType.String,
      SettingType.Int
    };

    public void Setup(
      BaseSetting setting,
      ElementStyle style,
      string title,
      string[] options,
      string tooltip,
      float elementWidth,
      float elementHeight,
      float optionsWidth,
      float maxScrollHeight,
      UnityAction onDropdownOptionSelect)
    {
      if (options.Length == 0)
        throw new ArgumentException("Dropdown cannot have 0 options.");
      this._onDropdownOptionSelect = onDropdownOptionSelect;
      this._options = options;
      this._optionsPanel = ((Component) ((Component) this).transform.Find("Dropdown/Mask")).gameObject;
      this._selectedButton = ((Component) ((Component) this).transform.Find("Dropdown/SelectedButton")).gameObject;
      this._selectedButtonLabel = ((Component) this._selectedButton.transform.Find("Label")).gameObject;
      this.SetupLabel(this._selectedButtonLabel, options[0], style.FontSize);
      // ISSUE: method pointer
      ((UnityEvent) this._selectedButton.GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__11_0)));
      this._selectedButton.GetComponent<LayoutElement>().preferredWidth = elementWidth;
      this._selectedButton.GetComponent<LayoutElement>().preferredHeight = elementHeight;
      for (int index = 0; index < options.Length; ++index)
        this.CreateOptionButton(options[index], index, optionsWidth, elementHeight, style.FontSize, style.ThemePanel);
      ((Selectable) this._selectedButton.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Dropdown");
      ((Graphic) this._selectedButtonLabel.GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
      ((Graphic) ((Component) this._selectedButton.transform.Find("Image")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
      ((Graphic) ((Component) this._optionsPanel.transform.Find("Options")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownBorderColor");
      Canvas.ForceUpdateCanvases();
      float num = ((Component) this._optionsPanel.transform.Find("Options")).GetComponent<RectTransform>().sizeDelta.y;
      if ((double) num > (double) maxScrollHeight)
        num = maxScrollHeight;
      else
        ((Component) this._optionsPanel.transform.Find("Scrollbar")).gameObject.SetActive(false);
      this._scrollBar = ((Component) this._optionsPanel.transform.Find("Scrollbar")).GetComponent<Scrollbar>();
      ((Selectable) this._scrollBar).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "DropdownScrollbar");
      ((Graphic) ((Component) this._scrollBar).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownScrollbarBackgroundColor");
      this._optionsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(optionsWidth, num);
      ((Component) ((Component) this).transform.Find("Label")).GetComponent<LayoutElement>().preferredHeight = elementHeight;
      this._optionsOffset = new Vector3((float) (((double) optionsWidth - (double) elementWidth) * 0.5), (float) (-((double) elementHeight + (double) num) * 0.5 + 2.0), 0.0f);
      this._optionsPanel.transform.SetParent(((Component) this).transform.root, true);
      this._optionsPanel.SetActive(false);
      this.Setup(setting, style, title, tooltip);
    }

    protected void SetOptionsPosition()
    {
      Vector3 vector3 = Vector3.op_Addition(this._selectedButton.transform.position, Vector3.op_Multiply(this._optionsOffset, UIManager.CurrentCanvasScale));
      ((Transform) ((Component) this._optionsPanel.transform).GetComponent<RectTransform>()).position = vector3;
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this._optionsPanel, (Object) null))
        return;
      this._optionsPanel.SetActive(false);
    }

    private void OnDestroy()
    {
      if (!Object.op_Inequality((Object) this._optionsPanel, (Object) null))
        return;
      Object.Destroy((Object) this._optionsPanel);
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this._optionsPanel, (Object) null) || !this._optionsPanel.activeSelf || (!Input.GetKeyUp((KeyCode) 323) || !Object.op_Inequality((Object) EventSystem.current.currentSelectedGameObject, (Object) ((Component) this._scrollBar).gameObject)) && !Vector3.op_Inequality(((Component) this).transform.position, this._lastKnownPosition))
        return;
      this.StartCoroutine(this.WaitAndCloseOptions());
    }

    protected void CreateOptionButton(
      string option,
      int index,
      float width,
      float height,
      int fontSize,
      string themePanel)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DropdownSettingElement.\u003C\u003Ec__DisplayClass16_0 cDisplayClass160 = new DropdownSettingElement.\u003C\u003Ec__DisplayClass16_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.option = option;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.index = index;
      GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("DropdownOption");
      gameObject.transform.SetParent(this._optionsPanel.transform.Find("Options"), false);
      // ISSUE: reference to a compiler-generated field
      this.SetupLabel(((Component) gameObject.transform.Find("Label")).gameObject, cDisplayClass160.option, fontSize);
      // ISSUE: method pointer
      ((UnityEvent) gameObject.GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass160, __methodptr(\u003CCreateOptionButton\u003Eb__0)));
      gameObject.GetComponent<LayoutElement>().preferredWidth = width;
      gameObject.GetComponent<LayoutElement>().preferredHeight = height;
      ((Selectable) gameObject.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(themePanel, "DefaultSetting", "Dropdown");
    }

    protected void OnDropdownSelectedButtonClick()
    {
      if (!this._optionsPanel.activeSelf)
        this.StartCoroutine(this.WaitAndEnableOptions());
      else
        this.CloseOptions();
    }

    private IEnumerator WaitAndEnableOptions()
    {
      DropdownSettingElement dropdownSettingElement = this;
      yield return (object) new WaitForEndOfFrame();
      dropdownSettingElement.SetOptionsPosition();
      dropdownSettingElement._optionsPanel.transform.SetAsLastSibling();
      dropdownSettingElement._lastKnownPosition = ((Component) dropdownSettingElement).transform.position;
      dropdownSettingElement._optionsPanel.SetActive(true);
      yield return (object) new WaitForEndOfFrame();
      dropdownSettingElement._scrollBar.value = dropdownSettingElement._currentScrollValue;
    }

    private IEnumerator WaitAndCloseOptions()
    {
      yield return (object) new WaitForEndOfFrame();
      this.CloseOptions();
    }

    protected void OnDropdownOptionClick(string option, int index)
    {
      this.SetupLabel(this._selectedButtonLabel, option);
      this.CloseOptions();
      if (this._settingType == SettingType.String)
        ((TypedSetting<string>) this._setting).Value = option;
      else if (this._settingType == SettingType.Int)
        ((TypedSetting<int>) this._setting).Value = index;
      this._onDropdownOptionSelect?.Invoke();
    }

    protected void CloseOptions()
    {
      this._currentScrollValue = this._scrollBar.value;
      this._optionsPanel.SetActive(false);
    }

    public override void SyncElement()
    {
      if (this._settingType == SettingType.String)
      {
        this.SetupLabel(this._selectedButtonLabel, ((TypedSetting<string>) this._setting).Value);
      }
      else
      {
        if (this._settingType != SettingType.Int)
          return;
        IntSetting setting = (IntSetting) this._setting;
        if (setting.Value >= this._options.Length)
          setting.Value = 0;
        this.SetupLabel(this._selectedButtonLabel, this._options[setting.Value]);
      }
    }
  }
}
