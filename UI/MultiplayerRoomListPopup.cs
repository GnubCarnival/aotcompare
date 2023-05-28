// Decompiled with JetBrains decompiler
// Type: UI.MultiplayerRoomListPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class MultiplayerRoomListPopup : BasePopup
  {
    protected MultiplayerPasswordPopup _multiplayerPasswordPopup;
    protected BasePopup _multiplayerCreatePopup;
    protected MultiplayerFilterPopup _multiplayerFilterPopup;
    protected Text _pageLabel;
    protected Text _playersOnlineLabel;
    protected GameObject _roomList;
    protected GameObject _noRoomsLabel;
    protected List<GameObject> _roomButtons = new List<GameObject>();
    public StringSetting _filterQuery = new StringSetting(string.Empty);
    public BoolSetting _filterShowFull = new BoolSetting(true);
    public BoolSetting _filterShowPassword = new BoolSetting(true);
    protected IntSetting _currentPage = new IntSetting(0, 0);
    private float _maxUpdateDelay = 5f;
    private float _currentUpdateDelay = 5f;
    private int _roomsPerPage = 10;
    private RoomInfo[] _rooms;
    private char[] _roomSeperator = new char[1]{ "`"[0] };
    private int _lastPageCount;

    protected override string ThemePanel => nameof (MultiplayerRoomListPopup);

    protected override bool HasPremadeContent => true;

    protected override int HorizontalPadding => 0;

    protected override int VerticalPadding => 0;

    protected override float Width => 1000f;

    protected override float Height => 660f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      string category = "MainMenu";
      string subCategory = nameof (MultiplayerRoomListPopup);
      ElementStyle style = new ElementStyle(this.ButtonFontSize, themePanel: this.ThemePanel);
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Create"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_0)));
      // ISSUE: method pointer
      ElementFactory.CreateDefaultButton(this.BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_1)));
      // ISSUE: method pointer
      ((Component) this.TopBar.Find("SearchInputSetting")).gameObject.AddComponent<InputSettingElement>().Setup((BaseSetting) this._filterQuery, new ElementStyle(titleWidth: 0.0f), UIManager.GetLocaleCommon("Search"), string.Empty, 160f, 40f, false, (UnityAction) null, new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_2)));
      // ISSUE: method pointer
      ((UnityEvent) ((Component) this.TopBar.Find("FilterButton")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_3)));
      // ISSUE: method pointer
      ((UnityEvent) ((Component) this.TopBar.Find("RefreshButton")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_4)));
      // ISSUE: method pointer
      ((UnityEvent) ((Component) this.TopBar.Find("Page/LeftButton")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_5)));
      // ISSUE: method pointer
      ((UnityEvent) ((Component) this.TopBar.Find("Page/RightButton")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CSetup\u003Eb__30_6)));
      this._pageLabel = ((Component) this.TopBar.Find("Page/PageLabel")).GetComponent<Text>();
      this._roomList = ((Component) this.SinglePanel.Find("RoomList")).gameObject;
      this._noRoomsLabel = ((Component) this._roomList.transform.Find("NoRoomsLabel")).gameObject;
      this._noRoomsLabel.GetComponent<Text>().text = UIManager.GetLocale(category, subCategory, "NoRooms");
      this._playersOnlineLabel = ((Component) this.TopBar.Find("PlayersOnlineLabel")).GetComponent<Text>();
      this._playersOnlineLabel.text = "0 " + UIManager.GetLocale(category, subCategory, "PlayersOnline");
      ((Component) this.TopBar.Find("FilterButton").Find("Text")).GetComponent<Text>().text = UIManager.GetLocaleCommon("Filters");
      foreach (Button componentsInChild in ((Component) this.TopBar).GetComponentsInChildren<Button>())
      {
        ((Selectable) componentsInChild).colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
        if (Object.op_Inequality((Object) ((Component) componentsInChild).transform.Find("Text"), (Object) null))
          ((Graphic) ((Component) ((Component) componentsInChild).transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
      }
      ((Graphic) ((Component) this.TopBar.Find("Page/PageLabel")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
      ((Graphic) ((Component) this.TopBar.Find("PlayersOnlineLabel")).GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
      ((Graphic) this._noRoomsLabel.GetComponent<Text>()).color = UIManager.GetThemeColor(style.ThemePanel, "RoomButton", "TextColor");
      ((Graphic) this._roomList.GetComponent<Image>()).color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "BackgroundColor");
    }

    public override void Show()
    {
      base.Show();
      this._currentPage.Value = 0;
      this.RefreshList();
      this._currentUpdateDelay = 0.5f;
    }

    public override void Hide()
    {
      if (((Component) this).gameObject.activeSelf)
        PhotonNetwork.Disconnect();
      base.Hide();
    }

    protected void Update()
    {
      this._currentUpdateDelay -= Time.deltaTime;
      if ((double) this._currentUpdateDelay > 0.0)
        return;
      this.RefreshList();
      this._currentUpdateDelay = this._maxUpdateDelay;
    }

    protected override void SetupPopups()
    {
      base.SetupPopups();
      this._multiplayerPasswordPopup = ElementFactory.CreateHeadedPanel<MultiplayerPasswordPopup>(((Component) this).transform).GetComponent<MultiplayerPasswordPopup>();
      this._multiplayerFilterPopup = ElementFactory.CreateHeadedPanel<MultiplayerFilterPopup>(((Component) this).transform).GetComponent<MultiplayerFilterPopup>();
      this._multiplayerCreatePopup = (BasePopup) ElementFactory.CreateHeadedPanel<MultiplayerCreatePopup>(((Component) this).transform).GetComponent<MultiplayerCreatePopup>();
      this._popups.Add((BasePopup) this._multiplayerPasswordPopup);
      this._popups.Add((BasePopup) this._multiplayerFilterPopup);
      this._popups.Add(this._multiplayerCreatePopup);
    }

    public void RefreshList(bool refetch = true)
    {
      this._currentUpdateDelay = this._maxUpdateDelay;
      if (refetch)
      {
        this._rooms = PhotonNetwork.GetRoomList();
        this._playersOnlineLabel.text = PhotonNetwork.countOfPlayers.ToString() + " " + UIManager.GetLocale("MainMenu", nameof (MultiplayerRoomListPopup), "PlayersOnline");
      }
      this.ClearRoomButtons();
      List<RoomInfo> filteredRooms = this.GetFilteredRooms();
      if (filteredRooms.Count == 0)
      {
        this._noRoomsLabel.SetActive(true);
        this._pageLabel.text = "0/0";
      }
      else
      {
        this._noRoomsLabel.SetActive(false);
        this._lastPageCount = this.GetPageCount(filteredRooms);
        this._currentPage.Value = Math.Min(this._currentPage.Value, this._lastPageCount - 1);
        this._pageLabel.text = (this._currentPage.Value + 1).ToString() + "/" + this._lastPageCount.ToString();
        foreach (RoomInfo currentPageRoom in this.GetCurrentPageRooms(filteredRooms))
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MultiplayerRoomListPopup.\u003C\u003Ec__DisplayClass35_0 cDisplayClass350 = new MultiplayerRoomListPopup.\u003C\u003Ec__DisplayClass35_0();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass350.\u003C\u003E4__this = this;
          // ISSUE: reference to a compiler-generated field
          cDisplayClass350.room = currentPageRoom;
          GameObject gameObject = ElementFactory.InstantiateAndBind(this._roomList.transform, "MultiplayerRoomButton");
          this._roomButtons.Add(gameObject);
          // ISSUE: method pointer
          ((UnityEvent) gameObject.GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass350, __methodptr(\u003CRefreshList\u003Eb__0)));
          // ISSUE: reference to a compiler-generated field
          ((Component) gameObject.transform.Find("Text")).GetComponent<Text>().text = this.GetRoomFormattedName(cDisplayClass350.room);
          // ISSUE: reference to a compiler-generated field
          if (this.GetRoomPassword(cDisplayClass350.room.name) == string.Empty)
            ((Component) gameObject.transform.Find("PasswordIcon")).gameObject.SetActive(false);
          ((Selectable) gameObject.GetComponent<Button>()).colors = UIManager.GetThemeColorBlock(this.ThemePanel, "RoomButton", "");
          ((Graphic) ((Component) gameObject.transform.Find("Text")).GetComponent<Text>()).color = UIManager.GetThemeColor(this.ThemePanel, "RoomButton", "TextColor");
        }
      }
    }

    protected List<RoomInfo> GetCurrentPageRooms(List<RoomInfo> rooms)
    {
      if (rooms.Count <= this._roomsPerPage)
        return rooms;
      List<RoomInfo> currentPageRooms = new List<RoomInfo>();
      int num1 = this._currentPage.Value * this._roomsPerPage;
      int num2 = Math.Min(num1 + this._roomsPerPage, rooms.Count);
      for (int index = num1; index < num2; ++index)
        currentPageRooms.Add(rooms[index]);
      return currentPageRooms;
    }

    protected List<RoomInfo> GetFilteredRooms()
    {
      List<RoomInfo> filteredRooms = new List<RoomInfo>();
      foreach (RoomInfo room in this._rooms)
      {
        if (this.IsValidRoom(room) && (!(this._filterQuery.Value != string.Empty) || room.name.ToLower().Contains(this._filterQuery.Value.ToLower())) && (this._filterShowFull.Value || room.playerCount < (int) room.maxPlayers) && (this._filterShowPassword.Value || !(this.GetRoomPassword(room.name) != string.Empty)))
          filteredRooms.Add(room);
      }
      return filteredRooms;
    }

    protected int GetPageCount(List<RoomInfo> rooms) => rooms.Count == 0 ? 0 : (rooms.Count - 1) / this._roomsPerPage + 1;

    protected void ClearRoomButtons()
    {
      foreach (Object roomButton in this._roomButtons)
        Object.Destroy(roomButton);
      this._roomButtons.Clear();
    }

    protected bool IsValidRoom(RoomInfo info) => info.name.Split(this._roomSeperator).Length > 5;

    protected string GetRoomPassword(string name)
    {
      string[] strArray = name.Split(this._roomSeperator);
      return strArray.Length > 5 ? strArray[5] : string.Empty;
    }

    protected string GetRoomFormattedName(RoomInfo room)
    {
      char[] chArray = new char[1]{ "`"[0] };
      string[] strArray = room.name.Split(chArray);
      return (strArray[0] + " / " + strArray[1] + " / " + strArray[2].UpperFirstLetter() + " / " + strArray[4].UpperFirstLetter() + "   " + (object) room.playerCount + "/" + (object) room.maxPlayers).hexColor();
    }

    private void OnRoomClick(string name)
    {
      string roomPassword = this.GetRoomPassword(name);
      if (roomPassword != string.Empty)
      {
        this.HideAllPopups();
        this._multiplayerPasswordPopup.Show(roomPassword, name);
      }
      else
        PhotonNetwork.JoinRoom(name);
    }

    private void OnButtonClick(string name)
    {
      this.HideAllPopups();
      switch (name)
      {
        case "Back":
          ((MainMenu) UIManager.CurrentMenu).ShowMultiplayerMapPopup();
          break;
        case "Create":
          this._multiplayerCreatePopup.Show();
          break;
        case "Filter":
          this._multiplayerFilterPopup.Show();
          break;
        case "Refresh":
          this.RefreshList();
          break;
        case "LeftPage":
          if (this._currentPage.Value <= 0)
            this._currentPage.Value = this._lastPageCount - 1;
          else
            --this._currentPage.Value;
          this.RefreshList(false);
          break;
        case "RightPage":
          if (this._currentPage.Value >= this._lastPageCount - 1)
            this._currentPage.Value = 0;
          else
            ++this._currentPage.Value;
          this.RefreshList(false);
          break;
      }
    }
  }
}
