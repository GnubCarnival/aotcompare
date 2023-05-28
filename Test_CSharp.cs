// Decompiled with JetBrains decompiler
// Type: Test_CSharp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using SimpleJSON;
using UnityEngine;

internal class Test_CSharp : MonoBehaviour
{
  private string m_InGameLog = string.Empty;
  private Vector2 m_Position = Vector2.zero;

  private void OnGUI()
  {
    this.m_Position = GUILayout.BeginScrollView(this.m_Position, new GUILayoutOption[0]);
    GUILayout.Label(this.m_InGameLog, new GUILayoutOption[0]);
    GUILayout.EndScrollView();
  }

  private void P(string aText) => this.m_InGameLog = this.m_InGameLog + aText + "\n";

  private void Start()
  {
    this.Test();
    Debug.Log((object) ("Test results:\n" + this.m_InGameLog));
  }

  private void Test()
  {
    JSONNode jsonNode1 = JSONNode.Parse("{\"name\":\"test\", \"array\":[1,{\"data\":\"value\"}]}");
    jsonNode1["array"][1]["Foo"] = (JSONNode) "Bar";
    this.P("'nice formatted' string representation of the JSON tree:");
    this.P(jsonNode1.ToString(string.Empty));
    this.P(string.Empty);
    this.P("'normal' string representation of the JSON tree:");
    this.P(((object) jsonNode1).ToString());
    this.P(string.Empty);
    this.P("content of member 'name':");
    this.P((string) jsonNode1["name"]);
    this.P(string.Empty);
    this.P("content of member 'array':");
    this.P(jsonNode1["array"].ToString(string.Empty));
    this.P(string.Empty);
    this.P("first element of member 'array': " + (string) jsonNode1["array"][0]);
    this.P(string.Empty);
    jsonNode1["array"][0].AsInt = 10;
    this.P("value of the first element set to: " + (string) jsonNode1["array"][0]);
    this.P("The value of the first element as integer: " + jsonNode1["array"][0].AsInt.ToString());
    this.P(string.Empty);
    this.P("N[\"array\"][1][\"data\"] == " + (string) jsonNode1["array"][1]["data"]);
    this.P(string.Empty);
    string base64 = jsonNode1.SaveToBase64();
    string compressedBase64 = jsonNode1.SaveToCompressedBase64();
    this.P("Serialized to Base64 string:");
    this.P(base64);
    this.P("Serialized to Base64 string (compressed):");
    this.P(compressedBase64);
    this.P(string.Empty);
    JSONNode jsonNode2 = JSONNode.LoadFromBase64(base64);
    this.P("Deserialized from Base64 string:");
    this.P(((object) jsonNode2).ToString());
    this.P(string.Empty);
    JSONClass jsonClass = new JSONClass();
    jsonClass["version"].AsInt = 5;
    jsonClass["author"]["name"] = (JSONNode) "Bunny83";
    jsonClass["author"]["phone"] = (JSONNode) "0123456789";
    jsonClass["data"][-1] = (JSONNode) "First item\twith tab";
    jsonClass["data"][-1] = (JSONNode) "Second item";
    jsonClass["data"][-1]["value"] = (JSONNode) "class item";
    jsonClass["data"].Add((JSONNode) "Forth item");
    jsonClass["data"][1] = (JSONNode) ((string) jsonClass["data"][1] + " 'addition to the second item'");
    jsonClass.Add("version", (JSONNode) "1.0");
    this.P("Second example:");
    this.P(((object) jsonClass).ToString());
    this.P(string.Empty);
    this.P("I[\"data\"][0]            : " + (string) jsonClass["data"][0]);
    this.P("I[\"data\"][0].ToString() : " + ((object) jsonClass["data"][0]).ToString());
    this.P("I[\"data\"][0].Value      : " + jsonClass["data"][0].Value);
    this.P(((object) jsonClass).ToString());
  }
}
