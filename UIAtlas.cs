// Decompiled with JetBrains decompiler
// Type: UIAtlas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  private Material material;
  [HideInInspector]
  [SerializeField]
  private UIAtlas.Coordinates mCoordinates;
  [HideInInspector]
  [SerializeField]
  private float mPixelSize = 1f;
  private int mPMA = -1;
  [HideInInspector]
  [SerializeField]
  private UIAtlas mReplacement;
  [SerializeField]
  [HideInInspector]
  private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

  public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
  {
    if (Object.op_Equality((Object) a, (Object) null) || Object.op_Equality((Object) b, (Object) null))
      return false;
    return Object.op_Equality((Object) a, (Object) b) || a.References(b) || b.References(a);
  }

  private static int CompareString(string a, string b) => a.CompareTo(b);

  public BetterList<string> GetListOfSprites()
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.GetListOfSprites();
    BetterList<string> listOfSprites = new BetterList<string>();
    int index = 0;
    for (int count = this.sprites.Count; index < count; ++index)
    {
      UIAtlas.Sprite sprite = this.sprites[index];
      if (sprite != null && !string.IsNullOrEmpty(sprite.name))
        listOfSprites.Add(sprite.name);
    }
    return listOfSprites;
  }

  public BetterList<string> GetListOfSprites(string match)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.GetListOfSprites(match);
    if (string.IsNullOrEmpty(match))
      return this.GetListOfSprites();
    BetterList<string> listOfSprites = new BetterList<string>();
    int index1 = 0;
    for (int count = this.sprites.Count; index1 < count; ++index1)
    {
      UIAtlas.Sprite sprite = this.sprites[index1];
      if (sprite != null && !string.IsNullOrEmpty(sprite.name) && string.Equals(match, sprite.name, StringComparison.OrdinalIgnoreCase))
      {
        listOfSprites.Add(sprite.name);
        return listOfSprites;
      }
    }
    char[] separator = new char[1]{ ' ' };
    string[] strArray = match.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    for (int index2 = 0; index2 < strArray.Length; ++index2)
      strArray[index2] = strArray[index2].ToLower();
    int index3 = 0;
    for (int count = this.sprites.Count; index3 < count; ++index3)
    {
      UIAtlas.Sprite sprite = this.sprites[index3];
      if (sprite != null && !string.IsNullOrEmpty(sprite.name))
      {
        string lower = sprite.name.ToLower();
        int num = 0;
        for (int index4 = 0; index4 < strArray.Length; ++index4)
        {
          if (lower.Contains(strArray[index4]))
            ++num;
        }
        if (num == strArray.Length)
          listOfSprites.Add(sprite.name);
      }
    }
    return listOfSprites;
  }

  public UIAtlas.Sprite GetSprite(string name)
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      return this.mReplacement.GetSprite(name);
    if (!string.IsNullOrEmpty(name))
    {
      int index = 0;
      for (int count = this.sprites.Count; index < count; ++index)
      {
        UIAtlas.Sprite sprite = this.sprites[index];
        if (!string.IsNullOrEmpty(sprite.name) && name == sprite.name)
          return sprite;
      }
    }
    return (UIAtlas.Sprite) null;
  }

  public void MarkAsDirty()
  {
    if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      this.mReplacement.MarkAsDirty();
    UISprite[] active1 = NGUITools.FindActive<UISprite>();
    int index1 = 0;
    for (int length = active1.Length; index1 < length; ++index1)
    {
      UISprite uiSprite = active1[index1];
      if (UIAtlas.CheckIfRelated(this, uiSprite.atlas))
      {
        UIAtlas atlas = uiSprite.atlas;
        uiSprite.atlas = (UIAtlas) null;
        uiSprite.atlas = atlas;
      }
    }
    UIFont[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (UIFont)) as UIFont[];
    int index2 = 0;
    for (int length = objectsOfTypeAll.Length; index2 < length; ++index2)
    {
      UIFont uiFont = objectsOfTypeAll[index2];
      if (UIAtlas.CheckIfRelated(this, uiFont.atlas))
      {
        UIAtlas atlas = uiFont.atlas;
        uiFont.atlas = (UIAtlas) null;
        uiFont.atlas = atlas;
      }
    }
    UILabel[] active2 = NGUITools.FindActive<UILabel>();
    int index3 = 0;
    for (int length = active2.Length; index3 < length; ++index3)
    {
      UILabel uiLabel = active2[index3];
      if (Object.op_Inequality((Object) uiLabel.font, (Object) null) && UIAtlas.CheckIfRelated(this, uiLabel.font.atlas))
      {
        UIFont font = uiLabel.font;
        uiLabel.font = (UIFont) null;
        uiLabel.font = font;
      }
    }
  }

  private bool References(UIAtlas atlas)
  {
    if (Object.op_Equality((Object) atlas, (Object) null))
      return false;
    if (Object.op_Equality((Object) atlas, (Object) this))
      return true;
    return Object.op_Inequality((Object) this.mReplacement, (Object) null) && this.mReplacement.References(atlas);
  }

  public UIAtlas.Coordinates coordinates
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.coordinates : this.mCoordinates;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.coordinates = value;
      }
      else
      {
        if (this.mCoordinates == value)
          return;
        if (Object.op_Equality((Object) this.material, (Object) null) || Object.op_Equality((Object) this.material.mainTexture, (Object) null))
        {
          Debug.LogError((object) "Can't switch coordinates until the atlas material has a valid texture");
        }
        else
        {
          this.mCoordinates = value;
          Texture mainTexture = this.material.mainTexture;
          int index = 0;
          for (int count = this.sprites.Count; index < count; ++index)
          {
            UIAtlas.Sprite sprite = this.sprites[index];
            if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
            {
              sprite.outer = NGUIMath.ConvertToTexCoords(sprite.outer, mainTexture.width, mainTexture.height);
              sprite.inner = NGUIMath.ConvertToTexCoords(sprite.inner, mainTexture.width, mainTexture.height);
            }
            else
            {
              sprite.outer = NGUIMath.ConvertToPixels(sprite.outer, mainTexture.width, mainTexture.height, true);
              sprite.inner = NGUIMath.ConvertToPixels(sprite.inner, mainTexture.width, mainTexture.height, true);
            }
          }
        }
      }
    }
  }

  public float pixelSize
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.pixelSize : this.mPixelSize;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
      {
        this.mReplacement.pixelSize = value;
      }
      else
      {
        float num = Mathf.Clamp(value, 0.25f, 4f);
        if ((double) this.mPixelSize == (double) num)
          return;
        this.mPixelSize = num;
        this.MarkAsDirty();
      }
    }
  }

  public bool premultipliedAlpha
  {
    get
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.premultipliedAlpha;
      if (this.mPMA == -1)
      {
        Material spriteMaterial = this.spriteMaterial;
        this.mPMA = Object.op_Equality((Object) spriteMaterial, (Object) null) || Object.op_Equality((Object) spriteMaterial.shader, (Object) null) || !((Object) spriteMaterial.shader).name.Contains("Premultiplied") ? 0 : 1;
      }
      return this.mPMA == 1;
    }
  }

  public UIAtlas replacement
  {
    get => this.mReplacement;
    set
    {
      UIAtlas uiAtlas = value;
      if (Object.op_Equality((Object) uiAtlas, (Object) this))
        uiAtlas = (UIAtlas) null;
      if (!Object.op_Inequality((Object) this.mReplacement, (Object) uiAtlas))
        return;
      if (Object.op_Inequality((Object) uiAtlas, (Object) null) && Object.op_Equality((Object) uiAtlas.replacement, (Object) this))
        uiAtlas.replacement = (UIAtlas) null;
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        this.MarkAsDirty();
      this.mReplacement = uiAtlas;
      this.MarkAsDirty();
    }
  }

  public List<UIAtlas.Sprite> spriteList
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.spriteList : this.sprites;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        this.mReplacement.spriteList = value;
      else
        this.sprites = value;
    }
  }

  public Material spriteMaterial
  {
    get => !Object.op_Equality((Object) this.mReplacement, (Object) null) ? this.mReplacement.spriteMaterial : this.material;
    set
    {
      if (Object.op_Inequality((Object) this.mReplacement, (Object) null))
        this.mReplacement.spriteMaterial = value;
      else if (Object.op_Equality((Object) this.material, (Object) null))
      {
        this.mPMA = 0;
        this.material = value;
      }
      else
      {
        this.MarkAsDirty();
        this.mPMA = -1;
        this.material = value;
        this.MarkAsDirty();
      }
    }
  }

  public Texture texture
  {
    get
    {
      if (!Object.op_Equality((Object) this.mReplacement, (Object) null))
        return this.mReplacement.texture;
      return !Object.op_Equality((Object) this.material, (Object) null) ? this.material.mainTexture : (Texture) null;
    }
  }

  public enum Coordinates
  {
    Pixels,
    TexCoords,
  }

  [Serializable]
  public class Sprite
  {
    public Rect inner = new Rect(0.0f, 0.0f, 1f, 1f);
    public string name = "Unity Bug";
    public Rect outer = new Rect(0.0f, 0.0f, 1f, 1f);
    public float paddingBottom;
    public float paddingLeft;
    public float paddingRight;
    public float paddingTop;
    public bool rotated;

    public bool hasPadding => (double) this.paddingLeft != 0.0 || (double) this.paddingRight != 0.0 || (double) this.paddingTop != 0.0 || (double) this.paddingBottom != 0.0;
  }
}
