// Decompiled with JetBrains decompiler
// Type: Localization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
  public TextAsset[] languages;
  private Dictionary<string, string> mDictionary = new Dictionary<string, string>();
  private static Localization mInstance;
  private string mLanguage;
  public string startingLanguage = "English";

  private void Awake()
  {
    if (Object.op_Equality((Object) Localization.mInstance, (Object) null))
    {
      Localization.mInstance = this;
      Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
      this.currentLanguage = PlayerPrefs.GetString("Language", this.startingLanguage);
      if (!string.IsNullOrEmpty(this.mLanguage) || this.languages == null || this.languages.Length == 0)
        return;
      this.currentLanguage = ((Object) this.languages[0]).name;
    }
    else
      Object.Destroy((Object) ((Component) this).gameObject);
  }

  public string Get(string key)
  {
    string str;
    return this.mDictionary.TryGetValue(key, out str) ? str : key;
  }

  private void Load(TextAsset asset)
  {
    this.mLanguage = ((Object) asset).name;
    PlayerPrefs.SetString("Language", this.mLanguage);
    this.mDictionary = new ByteReader(asset).ReadDictionary();
    UIRoot.Broadcast("OnLocalize", (object) this);
  }

  public static string Localize(string key) => !Object.op_Equality((Object) Localization.instance, (Object) null) ? Localization.instance.Get(key) : key;

  private void OnDestroy()
  {
    if (!Object.op_Equality((Object) Localization.mInstance, (Object) this))
      return;
    Localization.mInstance = (Localization) null;
  }

  private void OnEnable()
  {
    if (!Object.op_Equality((Object) Localization.mInstance, (Object) null))
      return;
    Localization.mInstance = this;
  }

  public string currentLanguage
  {
    get => this.mLanguage;
    set
    {
      if (!(this.mLanguage != value))
        return;
      this.startingLanguage = value;
      if (!string.IsNullOrEmpty(value))
      {
        if (this.languages != null)
        {
          int index = 0;
          for (int length = this.languages.Length; index < length; ++index)
          {
            TextAsset language = this.languages[index];
            if (Object.op_Inequality((Object) language, (Object) null) && ((Object) language).name == value)
            {
              this.Load(language);
              return;
            }
          }
        }
        TextAsset asset = Resources.Load(value, typeof (TextAsset)) as TextAsset;
        if (Object.op_Inequality((Object) asset, (Object) null))
        {
          this.Load(asset);
          return;
        }
      }
      this.mDictionary.Clear();
      PlayerPrefs.DeleteKey("Language");
    }
  }

  public static Localization instance
  {
    get
    {
      if (Object.op_Equality((Object) Localization.mInstance, (Object) null))
      {
        Localization.mInstance = Object.FindObjectOfType(typeof (Localization)) as Localization;
        if (Object.op_Equality((Object) Localization.mInstance, (Object) null))
        {
          GameObject gameObject = new GameObject("_Localization");
          Object.DontDestroyOnLoad((Object) gameObject);
          Localization.mInstance = gameObject.AddComponent<Localization>();
        }
      }
      return Localization.mInstance;
    }
  }

  public static bool isActive => Object.op_Inequality((Object) Localization.mInstance, (Object) null);
}
