// Decompiled with JetBrains decompiler
// Type: SimpleJSONFixed.JSONBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.Text;

namespace SimpleJSONFixed
{
  public class JSONBool : JSONNode
  {
    private bool m_Data;

    public override JSONNodeType Tag => JSONNodeType.Boolean;

    public override bool IsBoolean => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data.ToString();
      set
      {
        bool result;
        if (!bool.TryParse(value, out result))
          return;
        this.m_Data = result;
      }
    }

    public override bool AsBool
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public JSONBool(bool aData) => this.m_Data = aData;

    public JSONBool(string aData) => this.Value = aData;

    public override JSONNode Clone() => (JSONNode) new JSONBool(this.m_Data);

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append(this.m_Data ? "true" : "false");
    }

    public override bool Equals(object obj) => obj != null && obj is bool flag && this.m_Data == flag;

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void Clear() => this.m_Data = false;
  }
}
