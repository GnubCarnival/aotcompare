// Decompiled with JetBrains decompiler
// Type: Utility.BaseCSVObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
  internal class BaseCSVObject
  {
    private static Dictionary<Type, FieldInfo[]> _fields = new Dictionary<Type, FieldInfo[]>();

    protected virtual char Delimiter => ',';

    protected virtual char ParamDelimiter => ':';

    protected virtual bool NamedParams => false;

    public virtual string Serialize()
    {
      List<string> stringList = new List<string>();
      foreach (FieldInfo field in this.GetFields())
      {
        string str = this.SerializeField(field, (object) this);
        stringList.Add(str);
      }
      return string.Join(this.Delimiter.ToString(), stringList.ToArray());
    }

    public virtual void Deserialize(string csv)
    {
      string[] strArray1 = csv.Split(this.Delimiter);
      FieldInfo[] fields = this.GetFields();
      for (int index = 0; index < strArray1.Length; ++index)
        strArray1[index] = strArray1[index].Trim();
      if (this.NamedParams)
      {
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]
          {
            this.ParamDelimiter
          };
          string[] strArray2 = str.Split(chArray);
          FieldInfo field = this.FindField(strArray2[0]);
          if (field != null)
            this.DeserializeField(field, (object) this, strArray2[1]);
        }
      }
      else
      {
        for (int index1 = 0; index1 < fields.Length; ++index1)
        {
          if (this.IsList(fields[index1]))
          {
            Type genericArgument = fields[index1].FieldType.GetGenericArguments()[0];
            List<object> objectList = (List<object>) fields[index1].GetValue((object) this);
            objectList.Clear();
            int index2 = index1;
            while (index2 < strArray1.Length)
            {
              objectList.Add(this.DeserializeValue(genericArgument, strArray1[index2]));
              ++index1;
            }
            break;
          }
          this.DeserializeField(fields[index1], (object) this, strArray1[index1]);
        }
      }
    }

    protected virtual FieldInfo[] GetFields()
    {
      Type type = this.GetType();
      if (!BaseCSVObject._fields.ContainsKey(type))
        BaseCSVObject._fields.Add(type, type.GetFields());
      return BaseCSVObject._fields[type];
    }

    protected virtual FieldInfo FindField(string name)
    {
      foreach (FieldInfo field in BaseCSVObject._fields[this.GetType()])
      {
        if (field.Name == name)
          return field;
      }
      return (FieldInfo) null;
    }

    protected virtual bool IsList(FieldInfo field) => field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof (IList<>);

    protected virtual string SerializeField(FieldInfo info, object instance)
    {
      string str1 = string.Empty;
      if (this.NamedParams)
        str1 = info.Name + this.ParamDelimiter.ToString();
      string str2;
      if (this.IsList(info))
      {
        List<string> stringList = new List<string>();
        Type genericArgument = info.FieldType.GetGenericArguments()[0];
        foreach (object obj in (List<object>) info.GetValue(instance))
          stringList.Add(this.SerializeValue(genericArgument, obj));
        str2 = str1 + string.Join(this.Delimiter.ToString(), stringList.ToArray());
      }
      else
        str2 = str1 + this.SerializeValue(info.FieldType, info.GetValue(instance));
      return str2;
    }

    protected virtual void DeserializeField(FieldInfo info, object instance, string value) => info.SetValue(instance, this.DeserializeValue(info.FieldType, value));

    protected virtual string SerializeValue(Type t, object value)
    {
      if (t == typeof (string))
        return (string) value;
      if (t == typeof (int) || t == typeof (float))
        return value.ToString();
      if (t == typeof (bool))
        return Convert.ToInt32(value).ToString();
      return typeof (BaseCSVObject).IsAssignableFrom(t) ? ((BaseCSVObject) value).Serialize() : string.Empty;
    }

    protected virtual object DeserializeValue(Type t, string value)
    {
      if (t == typeof (string))
        return (object) value;
      if (t == typeof (int))
        return (object) int.Parse(value);
      if (t == typeof (float))
        return (object) float.Parse(value);
      if (t == typeof (bool))
        return (object) Convert.ToBoolean(int.Parse(value));
      if (!typeof (BaseCSVObject).IsAssignableFrom(t))
        return (object) null;
      BaseCSVObject instance = (BaseCSVObject) Activator.CreateInstance(t);
      instance.Deserialize(value);
      return (object) instance;
    }
  }
}
