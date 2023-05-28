// Decompiled with JetBrains decompiler
// Type: StyledComboBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof (RectTransform))]
public class StyledComboBox : StyledItem
{
  public StyledComboBoxPrefab containerPrefab;
  private bool isToggled;
  public StyledItem itemMenuPrefab;
  public StyledItem itemPrefab;
  [HideInInspector]
  [SerializeField]
  private List<StyledItem> items = new List<StyledItem>();
  public StyledComboBox.SelectionChangedHandler OnSelectionChanged;
  [HideInInspector]
  [SerializeField]
  private StyledComboBoxPrefab root;
  [SerializeField]
  private int selectedIndex;

  private void AddItem(object data)
  {
    if (!Object.op_Inequality((Object) this.itemPrefab, (Object) null))
      return;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    StyledComboBox.AddItemcAnonStoreyF itemcAnonStoreyF = new StyledComboBox.AddItemcAnonStoreyF()
    {
      fthis = this
    };
    Vector3[] vector3Array = new Vector3[4];
    ((Component) this.itemPrefab).GetComponent<RectTransform>().GetLocalCorners(vector3Array);
    Vector3 vector3 = vector3Array[0];
    float num = vector3.y - vector3Array[2].y;
    vector3.y = (float) this.items.Count * num;
    // ISSUE: reference to a compiler-generated field
    itemcAnonStoreyF.styledItem = Object.Instantiate((Object) this.itemPrefab, vector3, ((Transform) this.root.itemRoot).rotation) as StyledItem;
    // ISSUE: reference to a compiler-generated field
    RectTransform component = ((Component) itemcAnonStoreyF.styledItem).GetComponent<RectTransform>();
    // ISSUE: reference to a compiler-generated field
    itemcAnonStoreyF.styledItem.Populate(data);
    ((Transform) component).SetParent(((Component) this.root.itemRoot).transform, false);
    component.pivot = new Vector2(0.0f, 1f);
    component.anchorMin = new Vector2(0.0f, 1f);
    component.anchorMax = Vector2.one;
    component.anchoredPosition = new Vector2(0.0f, vector3.y);
    // ISSUE: reference to a compiler-generated field
    this.items.Add(itemcAnonStoreyF.styledItem);
    component.offsetMin = new Vector2(0.0f, vector3.y + num);
    component.offsetMax = new Vector2(0.0f, vector3.y);
    this.root.itemRoot.offsetMin = new Vector2(this.root.itemRoot.offsetMin.x, (float) (this.items.Count + 2) * num);
    // ISSUE: reference to a compiler-generated field
    Button button = itemcAnonStoreyF.styledItem.GetButton();
    // ISSUE: reference to a compiler-generated field
    itemcAnonStoreyF.curIndex = this.items.Count - 1;
    if (!Object.op_Inequality((Object) button, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) itemcAnonStoreyF, __methodptr(m0)));
  }

  public void AddItems(params object[] list)
  {
    this.ClearItems();
    for (int index = 0; index < list.Length; ++index)
      this.AddItem(list[index]);
    this.SelectedIndex = 0;
  }

  private void Awake() => this.InitControl();

  public void ClearItems()
  {
    for (int index = this.items.Count - 1; index >= 0; --index)
      Object.DestroyObject((Object) ((Component) this.items[index]).gameObject);
  }

  private void CreateMenuButton(object data)
  {
    if (((Component) this.root.menuItem).transform.childCount > 0)
    {
      for (int index = ((Component) this.root.menuItem).transform.childCount - 1; index >= 0; --index)
        Object.DestroyObject((Object) ((Component) ((Component) this.root.menuItem).transform.GetChild(index)).gameObject);
    }
    if (!Object.op_Inequality((Object) this.itemMenuPrefab, (Object) null) || !Object.op_Inequality((Object) this.root.menuItem, (Object) null))
      return;
    StyledItem styledItem = Object.Instantiate((Object) this.itemMenuPrefab) as StyledItem;
    styledItem.Populate(data);
    ((Component) styledItem).transform.SetParent(((Component) this.root.menuItem).transform, false);
    RectTransform component = ((Component) styledItem).GetComponent<RectTransform>();
    component.pivot = new Vector2(0.5f, 0.5f);
    component.anchorMin = Vector2.zero;
    component.anchorMax = Vector2.one;
    component.offsetMin = Vector2.zero;
    component.offsetMax = Vector2.zero;
    ((Object) ((Component) this.root).gameObject).hideFlags = (HideFlags) 1;
    Button button = styledItem.GetButton();
    if (!Object.op_Inequality((Object) button, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(TogglePanelState)));
  }

  public void InitControl()
  {
    if (Object.op_Inequality((Object) this.root, (Object) null))
      Object.DestroyImmediate((Object) ((Component) this.root).gameObject);
    if (!Object.op_Inequality((Object) this.containerPrefab, (Object) null))
      return;
    RectTransform component1 = ((Component) this).GetComponent<RectTransform>();
    this.root = Object.Instantiate((Object) this.containerPrefab, ((Transform) component1).position, ((Transform) component1).rotation) as StyledComboBoxPrefab;
    ((Component) this.root).transform.SetParent(((Component) this).transform, false);
    RectTransform component2 = ((Component) this.root).GetComponent<RectTransform>();
    component2.pivot = new Vector2(0.5f, 0.5f);
    component2.anchorMin = Vector2.zero;
    component2.anchorMax = Vector2.one;
    component2.offsetMax = Vector2.zero;
    component2.offsetMin = Vector2.zero;
    ((Object) ((Component) this.root).gameObject).hideFlags = (HideFlags) 1;
    ((Component) this.root.itemPanel).gameObject.SetActive(this.isToggled);
  }

  public void OnItemClicked(StyledItem item, int index)
  {
    this.SelectedIndex = index;
    this.TogglePanelState();
    if (this.OnSelectionChanged == null)
      return;
    this.OnSelectionChanged(item);
  }

  public void TogglePanelState()
  {
    this.isToggled = !this.isToggled;
    ((Component) this.root.itemPanel).gameObject.SetActive(this.isToggled);
  }

  public int SelectedIndex
  {
    get => this.selectedIndex;
    set
    {
      if (value < 0 || value > this.items.Count)
        return;
      this.selectedIndex = value;
      this.CreateMenuButton((object) this.items[this.selectedIndex].GetText().text);
    }
  }

  public StyledItem SelectedItem => this.selectedIndex >= 0 && this.selectedIndex <= this.items.Count ? this.items[this.selectedIndex] : (StyledItem) null;

  public delegate void SelectionChangedHandler(StyledItem item);
}
