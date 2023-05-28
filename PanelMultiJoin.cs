// Decompiled with JetBrains decompiler
// Type: PanelMultiJoin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections;
using UnityEngine;

public class PanelMultiJoin : MonoBehaviour
{
  private int currentPage = 1;
  private float elapsedTime = 10f;
  private string filter = string.Empty;
  private ArrayList filterRoom;
  public GameObject[] items;
  private int totalPage = 1;

  public void connectToIndex(int index, string roomName)
  {
    int num = 0;
    for (int index1 = 0; index1 < 10; ++index1)
      this.items[index1].SetActive(false);
    num = 10 * (this.currentPage - 1) + index;
    char[] chArray = new char[1]{ "`"[0] };
    string[] strArray = roomName.Split(chArray);
    if (strArray[5] != string.Empty)
    {
      PanelMultiJoinPWD.Password = strArray[5];
      PanelMultiJoinPWD.roomName = roomName;
      NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, true);
      NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, false);
    }
    else
      PhotonNetwork.JoinRoom(roomName);
  }

  private string getServerDataString(RoomInfo room)
  {
    char[] chArray = new char[1]{ "`"[0] };
    string[] strArray = room.name.Split(chArray);
    return (!(strArray[5] == string.Empty) ? (object) "[PWD]" : (object) string.Empty).ToString() + strArray[0] + "/" + strArray[1] + "/" + strArray[2] + "/" + strArray[4] + " " + (object) room.playerCount + "/" + (object) room.maxPlayers;
  }

  private void OnDisable()
  {
  }

  private void OnEnable()
  {
    this.currentPage = 1;
    this.totalPage = 0;
    this.refresh();
  }

  private void OnFilterSubmit(string content)
  {
    this.filter = content;
    this.updateFilterRooms();
    this.showlist();
  }

  public void pageDown()
  {
    ++this.currentPage;
    if (this.currentPage > this.totalPage)
      this.currentPage = 1;
    this.showServerList();
  }

  public void pageUp()
  {
    --this.currentPage;
    if (this.currentPage < 1)
      this.currentPage = this.totalPage;
    this.showServerList();
  }

  public void refresh() => this.showlist();

  private void showlist()
  {
    if (this.filter == string.Empty)
    {
      this.totalPage = PhotonNetwork.GetRoomList().Length == 0 ? 1 : (PhotonNetwork.GetRoomList().Length - 1) / 10 + 1;
    }
    else
    {
      this.updateFilterRooms();
      this.totalPage = this.filterRoom.Count <= 0 ? 1 : (this.filterRoom.Count - 1) / 10 + 1;
    }
    if (this.currentPage < 1)
      this.currentPage = this.totalPage;
    if (this.currentPage > this.totalPage)
      this.currentPage = 1;
    this.showServerList();
  }

  private void showServerList()
  {
    if (PhotonNetwork.GetRoomList().Length != 0)
    {
      if (this.filter == string.Empty)
      {
        for (int index1 = 0; index1 < 10; ++index1)
        {
          int index2 = 10 * (this.currentPage - 1) + index1;
          if (index2 < PhotonNetwork.GetRoomList().Length)
          {
            this.items[index1].SetActive(true);
            this.items[index1].GetComponentInChildren<UILabel>().text = this.getServerDataString(PhotonNetwork.GetRoomList()[index2]);
            this.items[index1].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = PhotonNetwork.GetRoomList()[index2].name;
          }
          else
            this.items[index1].SetActive(false);
        }
      }
      else
      {
        for (int index3 = 0; index3 < 10; ++index3)
        {
          int index4 = 10 * (this.currentPage - 1) + index3;
          if (index4 < this.filterRoom.Count)
          {
            RoomInfo room = (RoomInfo) this.filterRoom[index4];
            this.items[index3].SetActive(true);
            this.items[index3].GetComponentInChildren<UILabel>().text = this.getServerDataString(room);
            this.items[index3].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = room.name;
          }
          else
            this.items[index3].SetActive(false);
        }
      }
      GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage.ToString() + "/" + this.totalPage.ToString();
    }
    else
    {
      for (int index = 0; index < this.items.Length; ++index)
        this.items[index].SetActive(false);
      GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage.ToString() + "/" + this.totalPage.ToString();
    }
  }

  private void Start()
  {
    for (int index = 0; index < 10; ++index)
    {
      this.items[index].SetActive(true);
      this.items[index].GetComponentInChildren<UILabel>().text = string.Empty;
      this.items[index].SetActive(false);
    }
  }

  private void Update()
  {
    this.elapsedTime += Time.deltaTime;
    if ((double) this.elapsedTime <= 1.0)
      return;
    this.elapsedTime = 0.0f;
    this.showlist();
  }

  private void updateFilterRooms()
  {
    this.filterRoom = new ArrayList();
    if (!(this.filter != string.Empty))
      return;
    foreach (RoomInfo room in PhotonNetwork.GetRoomList())
    {
      if (room.name.ToUpper().Contains(this.filter.ToUpper()))
        this.filterRoom.Add((object) room);
    }
  }
}
