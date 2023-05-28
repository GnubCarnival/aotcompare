// Decompiled with JetBrains decompiler
// Type: SnapShotReview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using System;
using UnityEngine;

public class SnapShotReview : MonoBehaviour
{
  public GameObject labelDMG;
  public GameObject labelInfo;
  public GameObject labelPage;
  private UILabel page;
  public GameObject texture;
  private float textureH = 600f;
  private float textureW = 960f;
  private int _currentIndex;

  private void freshInfo()
  {
    int num;
    if (SnapshotManager.GetLength() == 0)
    {
      this.page.text = "0/0";
    }
    else
    {
      UILabel page = this.page;
      num = this._currentIndex + 1;
      string str1 = num.ToString();
      num = SnapshotManager.GetLength();
      string str2 = num.ToString();
      string str3 = str1 + "/" + str2;
      page.text = str3;
    }
    if (SnapshotManager.GetDamage(this._currentIndex) > 0)
    {
      UILabel component = this.labelDMG.GetComponent<UILabel>();
      num = SnapshotManager.GetDamage(this._currentIndex);
      string str = num.ToString();
      component.text = str;
    }
    else
      this.labelDMG.GetComponent<UILabel>().text = string.Empty;
  }

  private void setTextureWH()
  {
    if (SnapshotManager.GetLength() == 0)
      return;
    float num1 = 1.6f;
    float num2 = (float) this.texture.GetComponent<UITexture>().mainTexture.width / (float) this.texture.GetComponent<UITexture>().mainTexture.height;
    if ((double) num2 > (double) num1)
    {
      this.texture.transform.localScale = new Vector3(this.textureW, this.textureW / num2, 0.0f);
      this.labelDMG.transform.localPosition = new Vector3((float) (int) ((double) this.textureW * 0.5 - 20.0), (float) (int) (0.0 + (double) this.textureW * 0.5 / (double) num2 - 20.0), -20f);
      this.labelInfo.transform.localPosition = new Vector3((float) (int) ((double) this.textureW * 0.5 - 20.0), (float) (int) (0.0 - (double) this.textureW * 0.5 / (double) num2 + 20.0), -20f);
    }
    else
    {
      this.texture.transform.localScale = new Vector3(this.textureH * num2, this.textureH, 0.0f);
      this.labelDMG.transform.localPosition = new Vector3((float) (int) ((double) this.textureH * (double) num2 * 0.5 - 20.0), (float) (int) (0.0 + (double) this.textureH * 0.5 - 20.0), -20f);
      this.labelInfo.transform.localPosition = new Vector3((float) (int) ((double) this.textureH * (double) num2 * 0.5 - 20.0), (float) (int) (0.0 - (double) this.textureH * 0.5 + 20.0), -20f);
    }
  }

  public void ShowNextIMG()
  {
    if (this._currentIndex >= SnapshotManager.GetLength() - 1)
      return;
    ++this._currentIndex;
    this.texture.GetComponent<UITexture>().mainTexture = (Texture) SnapshotManager.GetSnapshot(this._currentIndex);
    this.setTextureWH();
    this.freshInfo();
  }

  public void ShowPrevIMG()
  {
    if (this._currentIndex <= 0)
      return;
    --this._currentIndex;
    this.texture.GetComponent<UITexture>().mainTexture = (Texture) SnapshotManager.GetSnapshot(this._currentIndex);
    this.setTextureWH();
    this.freshInfo();
  }

  private void Start()
  {
    this.page = this.labelPage.GetComponent<UILabel>();
    this._currentIndex = 0;
    if (SnapshotManager.GetLength() > 0)
      this.texture.GetComponent<UITexture>().mainTexture = (Texture) SnapshotManager.GetSnapshot(this._currentIndex);
    this.labelInfo.GetComponent<UILabel>().text = LoginFengKAI.player.name + " " + DateTime.Today.ToShortDateString();
    this.freshInfo();
    this.setTextureWH();
  }
}
