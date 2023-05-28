// Decompiled with JetBrains decompiler
// Type: UI.ElementFactory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class ElementFactory
  {
    public static ElementStyle CurrentElementStyle = new ElementStyle();

    public static GameObject CreateDefaultMenu<T>() where T : BaseMenu
    {
      GameObject defaultMenu = AssetBundleManager.InstantiateAsset<GameObject>("DefaultMenu");
      defaultMenu.transform.position = Vector3.zero;
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) defaultMenu.AddComponent<T>();
      local.Setup();
      local.ApplyScale();
      return defaultMenu;
    }

    public static GameObject CreateDefaultPanel<T>(Transform parent, bool enabled = false) where T : BasePanel => ElementFactory.InstantiateAndSetupPanel<T>(parent, "DefaultPanel", enabled);

    public static GameObject CreateDefaultPanel(Transform parent, Type t, bool enabled = false)
    {
      GameObject defaultPanel = ElementFactory.InstantiateAndBind(parent, "DefaultPanel");
      ((BasePanel) defaultPanel.AddComponent(t)).Setup(((Component) parent).GetComponent<BasePanel>());
      defaultPanel.SetActive(enabled);
      return defaultPanel;
    }

    public static GameObject CreateHeadedPanel<T>(Transform parent, bool enabled = false) where T : HeadedPanel => ElementFactory.InstantiateAndSetupPanel<T>(parent, "HeadedPanel", enabled);

    public static GameObject CreateTooltipPopup<T>(Transform parent, bool enabled = false) where T : TooltipPopup => ElementFactory.InstantiateAndSetupPanel<T>(parent, "TooltipPopup", enabled);

    public static GameObject CreateDefaultButton(
      Transform parent,
      ElementStyle style,
      string title,
      float elementWidth = 0.0f,
      float elementHeight = 0.0f,
      UnityAction onClick = null)
    {
      GameObject defaultButton = ElementFactory.InstantiateAndBind(parent, "DefaultButton");
      Text component1 = ((Component) defaultButton.transform.Find("Text")).GetComponent<Text>();
      component1.text = title;
      component1.fontSize = style.FontSize;
      LayoutElement component2 = defaultButton.GetComponent<LayoutElement>();
      if ((double) elementWidth > 0.0)
        component2.preferredWidth = elementWidth;
      if ((double) elementHeight > 0.0)
        component2.preferredHeight = elementHeight;
      if (onClick != null)
        ((UnityEvent) defaultButton.GetComponent<Button>().onClick).AddListener(onClick);
      ((Selectable) defaultButton.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
      ((Graphic) component1).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
      return defaultButton;
    }

    public static GameObject CreateCategoryButton(
      Transform parent,
      ElementStyle style,
      string title,
      UnityAction onClick = null)
    {
      GameObject categoryButton = ElementFactory.InstantiateAndBind(parent, "CategoryButton");
      Text component = categoryButton.GetComponent<Text>();
      component.text = title;
      component.fontSize = style.FontSize;
      if (onClick != null)
        ((UnityEvent) categoryButton.GetComponent<Button>().onClick).AddListener(onClick);
      ((Selectable) categoryButton.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "CategoryButton", "");
      return categoryButton;
    }

    public static GameObject CreateDropdownSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string[] options,
      string tooltip = "",
      float elementWidth = 140f,
      float elementHeight = 40f,
      float maxScrollHeight = 300f,
      float? optionsWidth = null,
      UnityAction onDropdownOptionSelect = null)
    {
      GameObject dropdownSetting = ElementFactory.InstantiateAndBind(parent, "DropdownSetting");
      DropdownSettingElement dropdownSettingElement = dropdownSetting.AddComponent<DropdownSettingElement>();
      if (!optionsWidth.HasValue)
        optionsWidth = new float?(elementWidth);
      BaseSetting setting1 = setting;
      ElementStyle style1 = style;
      string title1 = title;
      string[] options1 = options;
      string tooltip1 = tooltip;
      double elementWidth1 = (double) elementWidth;
      double elementHeight1 = (double) elementHeight;
      double optionsWidth1 = (double) optionsWidth.Value;
      double maxScrollHeight1 = (double) maxScrollHeight;
      UnityAction onDropdownOptionSelect1 = onDropdownOptionSelect;
      dropdownSettingElement.Setup(setting1, style1, title1, options1, tooltip1, (float) elementWidth1, (float) elementHeight1, (float) optionsWidth1, (float) maxScrollHeight1, onDropdownOptionSelect1);
      return dropdownSetting;
    }

    public static GameObject CreateIncrementSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string tooltip = "",
      float elementWidth = 33f,
      float elementHeight = 30f,
      string[] options = null,
      UnityAction onValueChanged = null)
    {
      GameObject incrementSetting = ElementFactory.InstantiateAndBind(parent, "IncrementSetting");
      incrementSetting.AddComponent<IncrementSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, options, onValueChanged);
      return incrementSetting;
    }

    public static GameObject CreateToggleSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string tooltip = "",
      float elementWidth = 30f,
      float elementHeight = 30f)
    {
      GameObject toggleSetting = ElementFactory.InstantiateAndBind(parent, "ToggleSetting");
      toggleSetting.AddComponent<ToggleSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight);
      return toggleSetting;
    }

    public static GameObject CreateToggleGroupSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string[] options,
      string tooltip = "",
      float elementWidth = 30f,
      float elementHeight = 30f)
    {
      GameObject toggleGroupSetting = ElementFactory.InstantiateAndBind(parent, "ToggleGroupSetting");
      toggleGroupSetting.AddComponent<ToggleGroupSettingElement>().Setup(setting, style, title, options, tooltip, elementWidth, elementHeight);
      return toggleGroupSetting;
    }

    public static GameObject CreateSliderSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string tooltip = "",
      float elementWidth = 150f,
      float elementHeight = 16f,
      int decimalPlaces = 2)
    {
      GameObject sliderSetting = ElementFactory.InstantiateAndBind(parent, "SliderSetting");
      sliderSetting.AddComponent<SliderSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, decimalPlaces);
      return sliderSetting;
    }

    public static GameObject CreateInputSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string tooltip = "",
      float elementWidth = 140f,
      float elementHeight = 40f,
      bool multiLine = false,
      UnityAction onValueChanged = null,
      UnityAction onEndEdit = null)
    {
      GameObject inputSetting = ElementFactory.InstantiateAndBind(parent, "InputSetting");
      inputSetting.AddComponent<InputSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, multiLine, onValueChanged, onEndEdit);
      return inputSetting;
    }

    public static GameObject CreateSliderInputSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      string tooltip = "",
      float sliderWidth = 150f,
      float sliderHeight = 16f,
      float inputWidth = 70f,
      float inputHeight = 40f,
      int decimalPlaces = 2)
    {
      GameObject sliderInputSetting = ElementFactory.InstantiateAndBind(parent, "SliderInputSetting");
      sliderInputSetting.AddComponent<SliderInputSettingElement>().Setup(setting, style, title, tooltip, sliderWidth, sliderHeight, inputWidth, inputHeight, decimalPlaces);
      return sliderInputSetting;
    }

    public static GameObject CreateDefaultLabel(
      Transform parent,
      ElementStyle style,
      string title,
      FontStyle fontStyle = 0,
      TextAnchor alignment = 4)
    {
      GameObject defaultLabel = ElementFactory.InstantiateAndBind(parent, "DefaultLabel");
      Text component = defaultLabel.GetComponent<Text>();
      component.fontSize = style.FontSize;
      component.text = title;
      component.fontStyle = fontStyle;
      ((Graphic) component).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
      component.alignment = alignment;
      if (Object.op_Inequality((Object) ((Component) parent).GetComponent<VerticalLayoutGroup>(), (Object) null))
      {
        ((Component) component).GetComponent<ContentSizeFitter>().horizontalFit = (ContentSizeFitter.FitMode) 0;
        return defaultLabel;
      }
      ((Component) component).GetComponent<ContentSizeFitter>().horizontalFit = (ContentSizeFitter.FitMode) 2;
      return defaultLabel;
    }

    public static GameObject CreateKeybindSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      KeybindPopup keybindPopup,
      string tooltip = "",
      float elementWidth = 120f,
      float elementHeight = 35f,
      int bindCount = 2)
    {
      GameObject keybindSetting = ElementFactory.InstantiateAndBind(parent, "KeybindSetting");
      keybindSetting.AddComponent<KeybindSettingElement>().Setup(setting, style, title, keybindPopup, tooltip, elementWidth, elementHeight, bindCount);
      return keybindSetting;
    }

    public static GameObject CreateColorSetting(
      Transform parent,
      ElementStyle style,
      BaseSetting setting,
      string title,
      ColorPickPopup colorPickPopup,
      string tooltip = "",
      float elementWidth = 90f,
      float elementHeight = 30f)
    {
      GameObject colorSetting = ElementFactory.InstantiateAndBind(parent, "ColorSetting");
      colorSetting.AddComponent<ColorSettingElement>().Setup(setting, style, title, colorPickPopup, tooltip, elementWidth, elementHeight);
      return colorSetting;
    }

    public static GameObject CreateHorizontalLine(
      Transform parent,
      ElementStyle style,
      float width,
      float height = 1f)
    {
      GameObject horizontalLine = ElementFactory.InstantiateAndBind(parent, "HorizontalLine");
      ((Component) horizontalLine.transform.Find("LineImage")).GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
      ((Component) horizontalLine.transform.Find("LineImage")).gameObject.AddComponent<HorizontalLineScaler>();
      ((Graphic) ((Component) horizontalLine.transform.Find("LineImage")).GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "HorizontalLineColor");
      return horizontalLine;
    }

    public static GameObject CreateHorizontalGroup(
      Transform parent,
      float spacing,
      TextAnchor alignment = 0)
    {
      GameObject horizontalGroup = ElementFactory.InstantiateAndBind(parent, "HorizontalGroup");
      ((HorizontalOrVerticalLayoutGroup) horizontalGroup.GetComponent<HorizontalLayoutGroup>()).spacing = spacing;
      ((LayoutGroup) horizontalGroup.GetComponent<HorizontalLayoutGroup>()).childAlignment = alignment;
      return horizontalGroup;
    }

    public static GameObject InstantiateAndSetupPanel<T>(
      Transform parent,
      string asset,
      bool enabled = false)
      where T : BasePanel
    {
      GameObject gameObject = ElementFactory.InstantiateAndBind(parent, asset);
      gameObject.AddComponent<T>().Setup(((Component) parent).GetComponent<BasePanel>());
      gameObject.SetActive(enabled);
      return gameObject;
    }

    public static GameObject InstantiateAndBind(Transform parent, string asset)
    {
      GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>(asset);
      gameObject.transform.SetParent(parent, false);
      gameObject.transform.localPosition = Vector3.zero;
      return gameObject;
    }

    public static void SetAnchor(
      GameObject obj,
      TextAnchor anchor,
      TextAnchor pivot,
      Vector2 offset)
    {
      RectTransform component = obj.GetComponent<RectTransform>();
      Vector2 anchorVector;
      Vector2 vector2 = anchorVector = ElementFactory.GetAnchorVector(anchor);
      component.anchorMax = anchorVector;
      component.anchorMin = vector2;
      component.pivot = ElementFactory.GetAnchorVector(pivot);
      component.anchoredPosition = offset;
    }

    public static Vector2 GetAnchorVector(TextAnchor anchor)
    {
      Vector2 anchorVector;
      // ISSUE: explicit constructor call
      ((Vector2) ref anchorVector).\u002Ector(0.0f, 0.0f);
      switch ((int) anchor)
      {
        case 0:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.0f, 1f);
          break;
        case 1:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.5f, 1f);
          break;
        case 2:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(1f, 1f);
          break;
        case 3:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.0f, 0.5f);
          break;
        case 4:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.5f, 0.5f);
          break;
        case 5:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(1f, 0.5f);
          break;
        case 6:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.0f, 0.0f);
          break;
        case 7:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(0.5f, 0.0f);
          break;
        case 8:
          // ISSUE: explicit constructor call
          ((Vector2) ref anchorVector).\u002Ector(1f, 0.0f);
          break;
      }
      return anchorVector;
    }
  }
}
