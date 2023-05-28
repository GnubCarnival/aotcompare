// Decompiled with JetBrains decompiler
// Type: BTN_ToJoin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_ToJoin : MonoBehaviour
{
  public static GameObject CreateInput(
    GameObject parent,
    GameObject toClone,
    Vector3 position,
    Quaternion rotation,
    string name,
    string hint,
    uint width = 100,
    int maxChars = 100,
    bool isPassword = false)
  {
    GameObject prefab = (GameObject) Object.Instantiate((Object) toClone);
    GameObject input = NGUITools.AddChild(parent, prefab);
    ((Object) input).name = name;
    input.transform.localPosition = position;
    input.transform.localRotation = rotation;
    ((Component) input.transform.Find("Label")).gameObject.GetComponent<UILabel>().text = hint;
    input.GetComponent<UIInput>().isPassword = isPassword;
    input.GetComponent<UIInput>().maxChars = maxChars;
    Vector3 size = input.GetComponent<BoxCollider>().size;
    float x = size.x;
    size.x = (float) width;
    input.GetComponent<BoxCollider>().size = size;
    input.GetComponent<UIInput>().label.lineWidth = (int) width;
    Vector3 localScale = input.transform.Find("Background").localScale;
    localScale.x *= (float) width / x;
    input.transform.Find("Background").localScale = localScale;
    input.transform.Find("Background").position = ((Component) input.GetComponent<UIInput>().label).transform.position;
    return input;
  }

  public static GameObject CreateLabel(
    GameObject parent,
    GameObject toClone,
    Vector3 position,
    Quaternion rotation,
    string name,
    string text,
    int fontsize,
    int lineWidth = 130)
  {
    GameObject prefab = (GameObject) Object.Instantiate((Object) toClone);
    GameObject label = NGUITools.AddChild(parent, prefab);
    ((Object) label).name = name;
    label.transform.localPosition = position;
    label.transform.localRotation = rotation;
    label.GetComponent<UILabel>().text = text;
    label.GetComponent<UILabel>().font.dynamicFontSize = fontsize;
    label.GetComponent<UILabel>().lineWidth = lineWidth;
    return label;
  }

  private void OnClick()
  {
    NGUITools.SetActive(((Component) ((Component) this).transform.parent).gameObject, false);
    NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate, true);
    Transform transform1 = GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate.transform;
    Transform transform2 = transform1.Find("ButtonJOIN");
    if (Object.op_Equality((Object) ((Component) transform2).GetComponent<BTN_Join_LAN>(), (Object) null))
      ((Component) transform2).gameObject.AddComponent<BTN_Join_LAN>();
    Transform transform3 = transform1.Find("InputIP");
    Transform transform4 = transform1.Find("InputPort");
    string str1 = PlayerPrefs.GetString("lastIP", "127.0.0.1");
    string str2 = PlayerPrefs.GetString("lastPort", "5055");
    ((Component) transform3).GetComponent<UIInput>().text = str1;
    ((Component) transform3).GetComponent<UIInput>().label.text = str1;
    ((Component) transform4).GetComponent<UIInput>().text = str2;
    ((Component) transform4).GetComponent<UIInput>().label.text = str2;
    ((Component) transform3).GetComponent<UIInput>().label.shrinkToFit = true;
    ((Component) transform4).GetComponent<UIInput>().label.shrinkToFit = true;
    Transform transform5 = transform1.Find("LabelAuthPass");
    Transform transform6 = transform1.Find("InputAuthPass");
    if (Object.op_Equality((Object) transform6, (Object) null))
    {
      uint x = (uint) ((Component) transform3).transform.Find("Background").localScale.x;
      Vector3 position = Vector3.op_Addition(transform2.localPosition, new Vector3(0.0f, 61f, 0.0f));
      transform6 = BTN_ToJoin.CreateInput(((Component) transform1).gameObject, ((Component) transform3).gameObject, position, transform2.rotation, "InputAuthPass", string.Empty, x).transform;
      ((Component) transform6).GetComponent<UIInput>().label.shrinkToFit = true;
    }
    if (Object.op_Equality((Object) transform5, (Object) null))
    {
      Vector3 position = Vector3.op_Addition(transform6.localPosition, new Vector3(0.0f, 35f, 0.0f));
      GameObject gameObject = ((Component) transform1.Find("LabelIP")).gameObject;
      Transform transform7 = BTN_ToJoin.CreateLabel(((Component) transform1).gameObject, gameObject, position, transform6.rotation, "LabelAuthPass", "Admin Password (Optional)", gameObject.GetComponent<UILabel>().font.dynamicFontSize, gameObject.GetComponent<UILabel>().lineWidth).transform;
      transform7.localScale = gameObject.transform.localScale;
      ((Component) transform7).GetComponent<UILabel>().color = gameObject.GetComponent<UILabel>().color;
    }
    string str3 = PlayerPrefs.GetString("lastAuthPass", string.Empty);
    ((Component) transform6).GetComponent<UIInput>().text = str3;
    ((Component) transform6).GetComponent<UIInput>().label.text = str3;
  }

  private void Start()
  {
  }
}
