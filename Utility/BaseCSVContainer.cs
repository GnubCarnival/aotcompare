// Decompiled with JetBrains decompiler
// Type: Utility.BaseCSVContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


namespace Utility
{
  internal class BaseCSVContainer : BaseCSVObject
  {
    protected override char Delimiter => ';';

    protected virtual bool UseNewlines => true;

    public override string Serialize()
    {
      string str = base.Serialize();
      if (this.UseNewlines)
        str = this.InsertNewlines(str);
      return str;
    }

    public virtual string InsertNewlines(string str)
    {
      string[] strArray = str.Split(this.Delimiter);
      return string.Join(this.Delimiter.ToString() + "\n", strArray);
    }
  }
}
