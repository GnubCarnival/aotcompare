// Decompiled with JetBrains decompiler
// Type: UI.BasePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class BasePanel : MonoBehaviour
  {
    protected Transform SinglePanel;
    protected Transform DoublePanelLeft;
    protected Transform DoublePanelRight;
    protected List<BasePopup> _popups = new List<BasePopup>();
    protected GameObject _currentCategoryPanel;
    protected StringSetting _currentCategoryPanelName = new StringSetting(string.Empty);
    protected Dictionary<string, Type> _categoryPanelTypes = new Dictionary<string, Type>();
    public BasePanel Parent;

    protected virtual string ThemePanel => "DefaultPanel";

    protected virtual float Width => 800f;

    protected virtual float Height => 600f;

    protected virtual float BorderVerticalPadding => 0.0f;

    protected virtual float BorderHorizontalPadding => 0.0f;

    protected virtual int VerticalPadding => 30;

    protected virtual int HorizontalPadding => 40;

    protected virtual float VerticalSpacing => 30f;

    protected virtual TextAnchor PanelAlignment => (TextAnchor) 0;

    protected virtual bool DoublePanel => false;

    protected virtual bool DoublePanelDivider => true;

    protected virtual bool ScrollBar => false;

    protected virtual bool CategoryPanel => false;

    protected virtual bool UseLastCategory => true;

    protected virtual bool HasPremadeContent => false;

    protected virtual string DefaultCategoryPanel => string.Empty;

    protected void OnEnable()
    {
      if (Object.op_Inequality((Object) ((Component) this).transform.Find("Border"), (Object) null))
        ((Component) ((Component) this).transform.Find("Border")).GetComponent<CanvasGroup>().blocksRaycasts = false;
      if (!Object.op_Inequality((Object) this._currentCategoryPanel, (Object) null))
        return;
      this._currentCategoryPanel.SetActive(true);
    }

    public virtual void Setup(BasePanel parent = null)
    {
      this.Parent = parent;
      ((Component) this).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.Width, this.Height);
      if (!this.CategoryPanel && !this.HasPremadeContent)
      {
        if (this.DoublePanel)
        {
          GameObject doublePanel = this.CreateDoublePanel(this.ScrollBar, this.DoublePanelDivider);
          this.DoublePanelLeft = this.GetDoublePanelLeftTransform(doublePanel);
          this.DoublePanelRight = this.GetDoublePanelRightTransform(doublePanel);
        }
        else
          this.SinglePanel = this.GetSinglePanelTransform(this.CreateSinglePanel(this.ScrollBar));
      }
      else if (this.HasPremadeContent)
        this.SetupPremadePanel();
      this.SetupPopups();
      if (!this.CategoryPanel)
        return;
      this.RegisterCategoryPanels();
      string lastcategory = UIManager.GetLastcategory(((object) this).GetType());
      if (this.UseLastCategory && lastcategory != string.Empty)
        this.SetCategoryPanel(lastcategory);
      else
        this.SetCategoryPanel(this.DefaultCategoryPanel);
    }

    public virtual void Show() => ((Component) this).gameObject.SetActive(true);

    public virtual void Hide()
    {
      this.HideAllPopups();
      ((Component) this).gameObject.SetActive(false);
    }

    public virtual void SyncSettingElements()
    {
      foreach (BaseSettingElement componentsInChild in ((Component) this).GetComponentsInChildren<BaseSettingElement>())
        componentsInChild.SyncElement();
    }

    protected virtual void SetupPremadePanel()
    {
      if (this.DoublePanel)
      {
        GameObject gameObject = ((Component) ((Component) this).transform.Find("DoublePanelContent")).gameObject;
        this.DoublePanelLeft = this.GetDoublePanelLeftTransform(gameObject);
        this.DoublePanelRight = this.GetDoublePanelRightTransform(gameObject);
        this.BindPanel(gameObject, this.ScrollBar);
        this.SetPanelPadding(((Component) this.GetDoublePanelLeftTransform(gameObject)).gameObject);
        this.SetPanelPadding(((Component) this.GetDoublePanelRightTransform(gameObject)).gameObject);
      }
      else
      {
        GameObject gameObject = ((Component) ((Component) this).transform.Find("SinglePanelContent")).gameObject;
        this.SinglePanel = this.GetSinglePanelTransform(gameObject);
        this.BindPanel(gameObject, this.ScrollBar);
        this.SetPanelPadding(((Component) this.GetSinglePanelTransform(gameObject)).gameObject);
      }
    }

    protected virtual void SetupPopups()
    {
    }

    protected virtual void HideAllPopups()
    {
      foreach (BasePanel popup in this._popups)
        popup.Hide();
    }

    protected virtual void RegisterCategoryPanels()
    {
    }

    public virtual void SetCategoryPanel(string name)
    {
      this.HideAllPopups();
      if (Object.op_Inequality((Object) this._currentCategoryPanel, (Object) null))
        Object.Destroy((Object) this._currentCategoryPanel);
      Type categoryPanelType = this._categoryPanelTypes[name];
      this._currentCategoryPanelName.Value = name;
      this._currentCategoryPanel = ElementFactory.CreateDefaultPanel(((Component) this).transform, categoryPanelType, true);
      this._currentCategoryPanel.SetActive(false);
      this.StartCoroutine(this.WaitAndEnableCategoryPanel());
      UIManager.SetLastCategory(((object) this).GetType(), name);
    }

    private IEnumerator WaitAndEnableCategoryPanel()
    {
      yield return (object) new WaitForEndOfFrame();
      this._currentCategoryPanel.SetActive(true);
    }

    public string GetCurrentCategoryName() => this._currentCategoryPanelName.Value;

    public void RebuildCategoryPanel() => this.SetCategoryPanel(this._currentCategoryPanelName);

    public void SetCategoryPanel(StringSetting setting) => this.SetCategoryPanel(setting.Value);

    protected GameObject CreateHorizontalDivider(Transform parent, float height = 1f)
    {
      float width = !this.DoublePanel ? this.GetPanelWidth() - (float) this.HorizontalPadding * 2f : (float) ((double) this.GetPanelWidth() * 0.5 - (double) this.HorizontalPadding * 2.0);
      return ElementFactory.CreateHorizontalLine(parent, new ElementStyle(themePanel: this.ThemePanel), width, height);
    }

    protected Transform GetSinglePanelTransform(GameObject singlePanel) => singlePanel.transform.Find("ScrollView/Panel");

    protected Transform GetDoublePanelLeftTransform(GameObject doublePanel) => doublePanel.transform.Find("ScrollView/LeftPanel");

    protected Transform GetDoublePanelRightTransform(GameObject doublePanel) => doublePanel.transform.Find("ScrollView/RightPanel");

    protected GameObject CreateSinglePanel(bool scrollBar)
    {
      GameObject singlePanel = AssetBundleManager.InstantiateAsset<GameObject>("SinglePanelContent");
      ((Component) this.GetSinglePanelTransform(singlePanel)).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth();
      this.BindPanel(singlePanel, scrollBar);
      this.SetPanelPadding(((Component) this.GetSinglePanelTransform(singlePanel)).gameObject);
      return singlePanel;
    }

    protected GameObject CreateDoublePanel(bool scrollBar, bool divider)
    {
      GameObject doublePanel = AssetBundleManager.InstantiateAsset<GameObject>("DoublePanelContent");
      ((Component) this.GetDoublePanelLeftTransform(doublePanel)).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth() * 0.5f;
      ((Component) this.GetDoublePanelRightTransform(doublePanel)).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth() * 0.5f;
      if (divider)
        ((Component) doublePanel.transform.Find("ScrollView/VerticalLine")).gameObject.AddComponent<VerticalLineScaler>();
      else
        ((Component) doublePanel.transform.Find("ScrollView/VerticalLine")).gameObject.SetActive(false);
      this.BindPanel(doublePanel, scrollBar);
      this.SetPanelPadding(((Component) this.GetDoublePanelLeftTransform(doublePanel)).gameObject);
      this.SetPanelPadding(((Component) this.GetDoublePanelRightTransform(doublePanel)).gameObject);
      return doublePanel;
    }

    protected virtual void BindPanel(GameObject panel, bool scrollBar)
    {
      panel.transform.SetParent(((Component) this).gameObject.transform, false);
      panel.transform.localPosition = Vector3.zero;
      float panelHeight = this.GetPanelHeight();
      panel.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetPanelWidth(), panelHeight);
      ((Component) panel.transform.Find("ScrollView")).GetComponent<LayoutElement>().minHeight = panelHeight;
      Scrollbar component = ((Component) panel.transform.Find("Scrollbar")).GetComponent<Scrollbar>();
      component.value = 1f;
      if (!scrollBar)
        ((Component) component).gameObject.SetActive(false);
      ((Graphic) panel.GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BackgroundColor");
      ((Selectable) component).colors = UIManager.GetThemeColorBlock(this.ThemePanel, "MainBody", "Scrollbar");
      ((Graphic) ((Component) component).GetComponent<Image>()).color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "ScrollbarBackgroundColor");
    }

    protected void SetPanelPadding(GameObject panel)
    {
      ((LayoutGroup) panel.GetComponent<VerticalLayoutGroup>()).padding = new RectOffset(this.HorizontalPadding, this.HorizontalPadding, this.VerticalPadding, this.VerticalPadding);
      ((HorizontalOrVerticalLayoutGroup) panel.GetComponent<VerticalLayoutGroup>()).spacing = this.VerticalSpacing;
      ((LayoutGroup) panel.GetComponent<VerticalLayoutGroup>()).childAlignment = this.PanelAlignment;
    }

    protected virtual float GetPanelWidth() => this.Width - this.BorderHorizontalPadding * 2f;

    protected virtual float GetPanelHeight() => this.Height - this.BorderVerticalPadding * 2f;
  }
}
