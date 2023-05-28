// Decompiled with JetBrains decompiler
// Type: NGUITools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

public static class NGUITools
{
  private static float mGlobalVolume = 1f;
  private static Color mInvisible = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  private static AudioListener mListener;
  private static bool mLoaded = false;

  private static void Activate(Transform t)
  {
    NGUITools.SetActiveSelf(((Component) t).gameObject, true);
    int num1 = 0;
    for (int childCount = t.childCount; num1 < childCount; ++num1)
    {
      if (((Component) t.GetChild(num1)).gameObject.activeSelf)
        return;
    }
    int num2 = 0;
    for (int childCount = t.childCount; num2 < childCount; ++num2)
      NGUITools.Activate(t.GetChild(num2));
  }

  public static GameObject AddChild(GameObject parent)
  {
    GameObject gameObject = new GameObject();
    if (Object.op_Inequality((Object) parent, (Object) null))
    {
      Transform transform = gameObject.transform;
      transform.parent = parent.transform;
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      transform.localScale = Vector3.one;
      gameObject.layer = parent.layer;
    }
    return gameObject;
  }

  public static T AddChild<T>(GameObject parent) where T : Component
  {
    GameObject gameObject = NGUITools.AddChild(parent);
    ((Object) gameObject).name = NGUITools.GetName<T>();
    return gameObject.AddComponent<T>();
  }

  public static GameObject AddChild(GameObject parent, GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate((Object) prefab) as GameObject;
    if (Object.op_Inequality((Object) gameObject, (Object) null) && Object.op_Inequality((Object) parent, (Object) null))
    {
      Transform transform = gameObject.transform;
      transform.parent = parent.transform;
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      transform.localScale = Vector3.one;
      gameObject.layer = parent.layer;
    }
    return gameObject;
  }

  public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
  {
    UIAtlas.Sprite sprite = Object.op_Equality((Object) atlas, (Object) null) ? (UIAtlas.Sprite) null : atlas.GetSprite(spriteName);
    UISprite uiSprite = NGUITools.AddWidget<UISprite>(go);
    uiSprite.type = sprite == null || Rect.op_Equality(sprite.inner, sprite.outer) ? UISprite.Type.Simple : UISprite.Type.Sliced;
    uiSprite.atlas = atlas;
    uiSprite.spriteName = spriteName;
    return uiSprite;
  }

  public static T AddWidget<T>(GameObject go) where T : UIWidget
  {
    int nextDepth = NGUITools.CalculateNextDepth(go);
    T obj = NGUITools.AddChild<T>(go);
    obj.depth = nextDepth;
    Transform transform = ((Component) (object) obj).transform;
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
    transform.localScale = new Vector3(100f, 100f, 1f);
    ((Component) (object) obj).gameObject.layer = go.layer;
    return obj;
  }

  public static BoxCollider AddWidgetCollider(GameObject go)
  {
    if (Object.op_Equality((Object) go, (Object) null))
      return (BoxCollider) null;
    Collider component = go.GetComponent<Collider>();
    BoxCollider boxCollider = component as BoxCollider;
    if (Object.op_Equality((Object) boxCollider, (Object) null))
    {
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        if (Application.isPlaying)
          Object.Destroy((Object) component);
        else
          Object.DestroyImmediate((Object) component);
      }
      boxCollider = go.AddComponent<BoxCollider>();
    }
    int nextDepth = NGUITools.CalculateNextDepth(go);
    Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
    ((Collider) boxCollider).isTrigger = true;
    boxCollider.center = Vector3.op_Addition(((Bounds) ref relativeWidgetBounds).center, Vector3.op_Multiply(Vector3.back, (float) nextDepth * 0.25f));
    boxCollider.size = new Vector3(((Bounds) ref relativeWidgetBounds).size.x, ((Bounds) ref relativeWidgetBounds).size.y, 0.0f);
    return boxCollider;
  }

  public static Color ApplyPMA(Color c)
  {
    if ((double) c.a != 1.0)
    {
      c.r *= c.a;
      c.g *= c.a;
      c.b *= c.a;
    }
    return c;
  }

  public static void Broadcast(string funcName)
  {
    GameObject[] objectsOfType = Object.FindObjectsOfType(typeof (GameObject)) as GameObject[];
    int index = 0;
    for (int length = objectsOfType.Length; index < length; ++index)
      objectsOfType[index].SendMessage(funcName, (SendMessageOptions) 1);
  }

  public static void Broadcast(string funcName, object param)
  {
    GameObject[] objectsOfType = Object.FindObjectsOfType(typeof (GameObject)) as GameObject[];
    int index = 0;
    for (int length = objectsOfType.Length; index < length; ++index)
      objectsOfType[index].SendMessage(funcName, param, (SendMessageOptions) 1);
  }

  public static int CalculateNextDepth(GameObject go)
  {
    int num = -1;
    UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
      num = Mathf.Max(num, componentsInChildren[index].depth);
    return num + 1;
  }

  private static void Deactivate(Transform t) => NGUITools.SetActiveSelf(((Component) t).gameObject, false);

  public static void Destroy(Object obj)
  {
    if (!Object.op_Inequality(obj, (Object) null))
      return;
    if (Application.isPlaying)
    {
      if (obj is GameObject)
        (obj as GameObject).transform.parent = (Transform) null;
      Object.Destroy(obj);
    }
    else
      Object.DestroyImmediate(obj);
  }

  public static void DestroyImmediate(Object obj)
  {
    if (!Object.op_Inequality(obj, (Object) null))
      return;
    if (Application.isEditor)
      Object.DestroyImmediate(obj);
    else
      Object.Destroy(obj);
  }

  public static string EncodeColor(Color c) => NGUIMath.DecimalToHex(16777215 & NGUIMath.ColorToInt(c) >> 8);

  public static T[] FindActive<T>() where T : Component => Object.FindObjectsOfType(typeof (T)) as T[];

  public static Camera FindCameraForLayer(int layer)
  {
    int num = 1 << layer;
    Camera[] active = NGUITools.FindActive<Camera>();
    int index = 0;
    for (int length = active.Length; index < length; ++index)
    {
      Camera cameraForLayer = active[index];
      if ((cameraForLayer.cullingMask & num) != 0)
        return cameraForLayer;
    }
    return (Camera) null;
  }

  public static T FindInParents<T>(GameObject go) where T : Component
  {
    if (Object.op_Equality((Object) go, (Object) null))
      return default (T);
    object component = (object) go.GetComponent<T>();
    if (component == null)
    {
      for (Transform parent = go.transform.parent; Object.op_Inequality((Object) parent, (Object) null) && component == null; parent = parent.parent)
        component = (object) ((Component) parent).gameObject.GetComponent<T>();
    }
    return (T) component;
  }

  public static bool GetActive(GameObject go) => Object.op_Inequality((Object) go, (Object) null) && go.activeInHierarchy;

  public static string GetHierarchy(GameObject obj)
  {
    string str = ((Object) obj).name;
    while (Object.op_Inequality((Object) obj.transform.parent, (Object) null))
    {
      obj = ((Component) obj.transform.parent).gameObject;
      str = ((Object) obj).name + "/" + str;
    }
    return "\"" + str + "\"";
  }

  public static string GetName<T>() where T : Component
  {
    string name = typeof (T).ToString();
    if (name.StartsWith("UI"))
      return name.Substring(2);
    if (name.StartsWith("UnityEngine."))
      name = name.Substring(12);
    return name;
  }

  public static GameObject GetRoot(GameObject go)
  {
    Transform transform = go.transform;
    while (true)
    {
      Transform parent = transform.parent;
      if (!Object.op_Equality((Object) parent, (Object) null))
        transform = parent;
      else
        break;
    }
    return ((Component) transform).gameObject;
  }

  public static bool IsChild(Transform parent, Transform child)
  {
    if (Object.op_Inequality((Object) parent, (Object) null) && Object.op_Inequality((Object) child, (Object) null))
    {
      for (; Object.op_Inequality((Object) child, (Object) null); child = child.parent)
      {
        if (Object.op_Equality((Object) child, (Object) parent))
          return true;
      }
    }
    return false;
  }

  public static byte[] Load(string fileName) => (byte[]) null;

  public static void MakePixelPerfect(Transform t)
  {
    UIWidget component = ((Component) t).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      component.MakePixelPerfect();
    }
    else
    {
      t.localPosition = NGUITools.Round(t.localPosition);
      t.localScale = NGUITools.Round(t.localScale);
      int num = 0;
      for (int childCount = t.childCount; num < childCount; ++num)
        NGUITools.MakePixelPerfect(t.GetChild(num));
    }
  }

  public static void MarkParentAsChanged(GameObject go)
  {
    UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
    int index = 0;
    for (int length = componentsInChildren.Length; index < length; ++index)
      componentsInChildren[index].ParentHasChanged();
  }

  public static WWW OpenURL(string url)
  {
    WWW www = (WWW) null;
    try
    {
      www = new WWW(url);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.Message);
    }
    return www;
  }

  public static WWW OpenURL(string url, WWWForm form)
  {
    if (form == null)
      return NGUITools.OpenURL(url);
    WWW www = (WWW) null;
    try
    {
      www = new WWW(url, form);
    }
    catch (Exception ex)
    {
      Debug.LogError(ex == null ? (object) "null" : (object) ex.Message);
    }
    return www;
  }

  public static Color ParseColor(string text, int offset)
  {
    int num1 = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
    int num2 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
    int num3 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
    float num4 = 0.003921569f;
    return new Color(num4 * (float) num1, num4 * (float) num2, num4 * (float) num3);
  }

  public static int ParseSymbol(string text, int index, List<Color> colors, bool premultiply)
  {
    int length = text.Length;
    if (index + 2 < length)
    {
      if (text[index + 1] == '-')
      {
        if (text[index + 2] == ']')
        {
          if (colors != null && colors.Count > 1)
            colors.RemoveAt(colors.Count - 1);
          return 3;
        }
      }
      else if (index + 7 < length && text[index + 7] == ']')
      {
        if (colors != null)
        {
          Color c = NGUITools.ParseColor(text, index + 1);
          if (NGUITools.EncodeColor(c) != text.Substring(index + 1, 6).ToUpper())
            return 0;
          Color color = colors[colors.Count - 1];
          c.a = color.a;
          if (premultiply && (double) c.a != 1.0)
            c = Color.Lerp(NGUITools.mInvisible, c, c.a);
          colors.Add(c);
        }
        return 8;
      }
    }
    return 0;
  }

  public static AudioSource PlaySound(AudioClip clip) => NGUITools.PlaySound(clip, 1f, 1f);

  public static AudioSource PlaySound(AudioClip clip, float volume) => NGUITools.PlaySound(clip, volume, 1f);

  public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
  {
    volume *= NGUITools.soundVolume;
    if (Object.op_Inequality((Object) clip, (Object) null) && (double) volume > 0.0099999997764825821)
    {
      if (Object.op_Equality((Object) NGUITools.mListener, (Object) null))
      {
        NGUITools.mListener = Object.FindObjectOfType(typeof (AudioListener)) as AudioListener;
        if (Object.op_Equality((Object) NGUITools.mListener, (Object) null))
        {
          Camera camera = Camera.main;
          if (Object.op_Equality((Object) camera, (Object) null))
            camera = Object.FindObjectOfType(typeof (Camera)) as Camera;
          if (Object.op_Inequality((Object) camera, (Object) null))
            NGUITools.mListener = ((Component) camera).gameObject.AddComponent<AudioListener>();
        }
      }
      if (Object.op_Inequality((Object) NGUITools.mListener, (Object) null) && ((Behaviour) NGUITools.mListener).enabled && NGUITools.GetActive(((Component) NGUITools.mListener).gameObject))
      {
        AudioSource audioSource = ((Component) NGUITools.mListener).audio;
        if (Object.op_Equality((Object) audioSource, (Object) null))
          audioSource = ((Component) NGUITools.mListener).gameObject.AddComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
        return audioSource;
      }
    }
    return (AudioSource) null;
  }

  public static int RandomRange(int min, int max) => min == max ? min : Random.Range(min, max + 1);

  public static Vector3 Round(Vector3 v)
  {
    v.x = Mathf.Round(v.x);
    v.y = Mathf.Round(v.y);
    v.z = Mathf.Round(v.z);
    return v;
  }

  public static bool Save(string fileName, byte[] bytes) => false;

  public static void SetActive(GameObject go, bool state)
  {
    if (state)
      NGUITools.Activate(go.transform);
    else
      NGUITools.Deactivate(go.transform);
  }

  public static void SetActiveChildren(GameObject go, bool state)
  {
    Transform transform = go.transform;
    if (state)
    {
      int num = 0;
      for (int childCount = transform.childCount; num < childCount; ++num)
        NGUITools.Activate(transform.GetChild(num));
    }
    else
    {
      int num = 0;
      for (int childCount = transform.childCount; num < childCount; ++num)
        NGUITools.Deactivate(transform.GetChild(num));
    }
  }

  public static void SetActiveSelf(GameObject go, bool state) => go.SetActive(state);

  public static void SetLayer(GameObject go, int layer)
  {
    go.layer = layer;
    Transform transform = go.transform;
    int num = 0;
    for (int childCount = transform.childCount; num < childCount; ++num)
      NGUITools.SetLayer(((Component) transform.GetChild(num)).gameObject, layer);
  }

  public static string StripSymbols(string text)
  {
    if (text != null)
    {
      int num = 0;
      int length = text.Length;
      while (num < length)
      {
        if (text[num] == '[')
        {
          int symbol = NGUITools.ParseSymbol(text, num, (List<Color>) null, false);
          if (symbol > 0)
          {
            text = text.Remove(num, symbol);
            length = text.Length;
            continue;
          }
        }
        ++num;
      }
    }
    return text;
  }

  public static string clipboard
  {
    get => (string) null;
    set
    {
    }
  }

  public static bool fileAccess => Application.platform != 5 && Application.platform != 3;

  public static float soundVolume
  {
    get
    {
      if (!NGUITools.mLoaded)
      {
        NGUITools.mLoaded = true;
        NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
      }
      return NGUITools.mGlobalVolume;
    }
    set
    {
      if ((double) NGUITools.mGlobalVolume == (double) value)
        return;
      NGUITools.mLoaded = true;
      NGUITools.mGlobalVolume = value;
      PlayerPrefs.SetFloat("Sound", value);
    }
  }
}
