// Decompiled with JetBrains decompiler
// Type: RCCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

internal class RCCondition
{
  private int operand;
  private RCActionHelper parameter1;
  private RCActionHelper parameter2;
  private int type;

  public RCCondition(
    int sentOperand,
    int sentType,
    RCActionHelper sentParam1,
    RCActionHelper sentParam2)
  {
    this.operand = sentOperand;
    this.type = sentType;
    this.parameter1 = sentParam1;
    this.parameter2 = sentParam2;
  }

  private bool boolCompare(bool baseBool, bool compareBool)
  {
    switch (this.operand)
    {
      case 2:
        return baseBool == compareBool;
      case 5:
        return baseBool != compareBool;
      default:
        return false;
    }
  }

  public bool checkCondition()
  {
    switch (this.type)
    {
      case 0:
        return this.intCompare(this.parameter1.returnInt((object) null), this.parameter2.returnInt((object) null));
      case 1:
        return this.boolCompare(this.parameter1.returnBool((object) null), this.parameter2.returnBool((object) null));
      case 2:
        return this.stringCompare(this.parameter1.returnString((object) null), this.parameter2.returnString((object) null));
      case 3:
        return this.floatCompare(this.parameter1.returnFloat((object) null), this.parameter2.returnFloat((object) null));
      case 4:
        return this.playerCompare(this.parameter1.returnPlayer((object) null), this.parameter2.returnPlayer((object) null));
      case 5:
        return this.titanCompare(this.parameter1.returnTitan((object) null), this.parameter2.returnTitan((object) null));
      default:
        return false;
    }
  }

  private bool floatCompare(float baseFloat, float compareFloat)
  {
    switch (this.operand)
    {
      case 0:
        return (double) baseFloat < (double) compareFloat;
      case 1:
        return (double) baseFloat <= (double) compareFloat;
      case 2:
        return (double) baseFloat == (double) compareFloat;
      case 3:
        return (double) baseFloat >= (double) compareFloat;
      case 4:
        return (double) baseFloat > (double) compareFloat;
      case 5:
        return (double) baseFloat != (double) compareFloat;
      default:
        return false;
    }
  }

  private bool intCompare(int baseInt, int compareInt)
  {
    switch (this.operand)
    {
      case 0:
        return baseInt < compareInt;
      case 1:
        return baseInt <= compareInt;
      case 2:
        return baseInt == compareInt;
      case 3:
        return baseInt >= compareInt;
      case 4:
        return baseInt > compareInt;
      case 5:
        return baseInt != compareInt;
      default:
        return false;
    }
  }

  private bool playerCompare(PhotonPlayer basePlayer, PhotonPlayer comparePlayer)
  {
    switch (this.operand)
    {
      case 2:
        return basePlayer == comparePlayer;
      case 5:
        return basePlayer != comparePlayer;
      default:
        return false;
    }
  }

  private bool stringCompare(string baseString, string compareString)
  {
    switch (this.operand)
    {
      case 0:
        return baseString == compareString;
      case 1:
        return baseString != compareString;
      case 2:
        return baseString.Contains(compareString);
      case 3:
        return !baseString.Contains(compareString);
      case 4:
        return baseString.StartsWith(compareString);
      case 5:
        return !baseString.StartsWith(compareString);
      case 6:
        return baseString.EndsWith(compareString);
      case 7:
        return !baseString.EndsWith(compareString);
      default:
        return false;
    }
  }

  private bool titanCompare(TITAN baseTitan, TITAN compareTitan)
  {
    switch (this.operand)
    {
      case 2:
        return Object.op_Equality((Object) baseTitan, (Object) compareTitan);
      case 5:
        return Object.op_Inequality((Object) baseTitan, (Object) compareTitan);
      default:
        return false;
    }
  }

  public enum castTypes
  {
    typeInt,
    typeBool,
    typeString,
    typeFloat,
    typePlayer,
    typeTitan,
  }

  public enum operands
  {
    lt,
    lte,
    e,
    gte,
    gt,
    ne,
  }

  public enum stringOperands
  {
    equals,
    notEquals,
    contains,
    notContains,
    startsWith,
    notStartsWith,
    endsWith,
    notEndsWith,
  }
}
