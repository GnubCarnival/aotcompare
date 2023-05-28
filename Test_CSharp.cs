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
    jsonNode1["array"][1]["Foo"] = JSONNode.op_Implicit("Bar");
    this.P("'nice formatted' string representation of the JSON tree:");
    this.P(jsonNode1.ToString(string.Empty));
    this.P(string.Empty);
    this.P("'normal' string representation of the JSON tree:");
    this.P(jsonNode1.ToString());
    this.P(string.Empty);
    this.P("content of member 'name':");
    this.P(JSONNode.op_Implicit(jsonNode1["name"]));
    this.P(string.Empty);
    this.P("content of member 'array':");
    this.P(jsonNode1["array"].ToString(string.Empty));
    this.P(string.Empty);
    this.P("first element of member 'array': " + JSONNode.op_Implicit(jsonNode1["array"][0]));
    this.P(string.Empty);
    jsonNode1["array"][0].AsInt = 10;
    this.P("value of the first element set to: " + JSONNode.op_Implicit(jsonNode1["array"][0]));
    this.P("The value of the first element as integer: " + jsonNode1["array"][0].AsInt.ToString());
    this.P(string.Empty);
    this.P("N[\"array\"][1][\"data\"] == " + JSONNode.op_Implicit(jsonNode1["array"][1]["data"]));
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
    this.P(jsonNode2.ToString());
    this.P(string.Empty);
    JSONClass jsonClass = new JSONClass();
    ((JSONNode) jsonClass)["version"].AsInt = 5;
    ((JSONNode) jsonClass)["author"]["name"] = JSONNode.op_Implicit("Bunny83");
    ((JSONNode) jsonClass)["author"]["phone"] = JSONNode.op_Implicit("0123456789");
    ((JSONNode) jsonClass)["data"][-1] = JSONNode.op_Implicit("First item\twith tab");
    ((JSONNode) jsonClass)["data"][-1] = JSONNode.op_Implicit("Second item");
    ((JSONNode) jsonClass)["data"][-1]["value"] = JSONNode.op_Implicit("class item");
    ((JSONNode) jsonClass)["data"].Add(JSONNode.op_Implicit("Forth item"));
    ((JSONNode) jsonClass)["data"][1] = JSONNode.op_Implicit(JSONNode.op_Implicit(((JSONNode) jsonClass)["data"][1]) + " 'addition to the second item'");
    ((JSONNode) jsonClass).Add("version", JSONNode.op_Implicit("1.0"));
    this.P("Second example:");
    this.P(jsonClass.ToString());
    this.P(string.Empty);
    this.P("I[\"data\"][0]            : " + JSONNode.op_Implicit(((JSONNode) jsonClass)["data"][0]));
    this.P("I[\"data\"][0].ToString() : " + ((JSONNode) jsonClass)["data"][0].ToString());
    this.P("I[\"data\"][0].Value      : " + ((JSONNode) jsonClass)["data"][0].Value);
    this.P(jsonClass.ToString());
  }
}
