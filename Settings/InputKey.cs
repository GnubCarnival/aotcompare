// Decompiled with JetBrains decompiler
// Type: Settings.InputKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
  internal class InputKey
  {
    protected KeyCode _key;
    protected bool _isSpecial;
    protected SpecialKey _special;
    protected bool _isModifier;
    protected KeyCode _modifier;
    protected HashSet<KeyCode> ModifierKeys = new HashSet<KeyCode>()
    {
      (KeyCode) 304,
      (KeyCode) 308,
      (KeyCode) 306,
      (KeyCode) 303,
      (KeyCode) 307,
      (KeyCode) 305
    };
    protected HashSet<string> AlphaDigits = new HashSet<string>()
    {
      "0",
      "1",
      "2",
      "3",
      "4",
      "5",
      "6",
      "7",
      "8",
      "9"
    };

    public InputKey()
    {
    }

    public InputKey(string keyStr) => this.LoadFromString(keyStr);

    public bool MatchesKeyCode(KeyCode key) => !this._isSpecial && !this._isModifier && this._key == key;

    public bool ReadNextInput()
    {
      this._isModifier = false;
      foreach (KeyCode modifierKey in this.ModifierKeys)
      {
        if (Input.GetKey(modifierKey))
        {
          this._modifier = modifierKey;
          this._isModifier = true;
        }
      }
      foreach (KeyCode keyCode in Enum.GetValues(typeof (KeyCode)))
      {
        if (this.ModifierKeys.Contains(keyCode) && Input.GetKeyUp(keyCode))
        {
          this._isModifier = false;
          this._key = keyCode;
          this._isSpecial = false;
          return true;
        }
        if (!this.ModifierKeys.Contains(keyCode) && keyCode == 323 && Input.GetKeyUp(keyCode))
        {
          this._key = keyCode;
          this._isSpecial = false;
          return true;
        }
        if (!this.ModifierKeys.Contains(keyCode) && keyCode != 323 && Input.GetKeyDown(keyCode))
        {
          this._key = keyCode;
          this._isSpecial = false;
          return true;
        }
      }
      foreach (SpecialKey specialKey in Enum.GetValues(typeof (SpecialKey)))
      {
        if (this.GetSpecial(specialKey))
        {
          this._special = specialKey;
          this._isSpecial = true;
          return true;
        }
      }
      return false;
    }

    public bool GetKeyDown() => this._isSpecial ? this.GetModifier() && this.GetSpecial(this._special) : this.GetModifier() && Input.GetKeyDown(this._key);

    public bool GetKey() => this._isSpecial ? this.GetModifier() && this.GetSpecial(this._special) : this.GetModifier() && Input.GetKey(this._key);

    public bool GetKeyUp() => this._isSpecial ? this.GetModifier() && this.GetSpecial(this._special) : this.GetModifier() && Input.GetKeyUp(this._key);

    public bool IsWheel()
    {
      if (!this._isSpecial)
        return false;
      return this._special == SpecialKey.WheelDown || this._special == SpecialKey.WheelUp;
    }

    public bool IsNone() => this._isSpecial && this._special == SpecialKey.None;

    public override string ToString()
    {
      string str = this._isSpecial ? this._special.ToString() : this._key.ToString();
      if (str.StartsWith("Alpha"))
        str = str.Substring(5);
      if (this._isModifier)
        str = this._modifier.ToString() + "+" + str;
      return str;
    }

    public override bool Equals(object obj) => this.ToString() == obj.ToString();

    public void LoadFromString(string serializedKey)
    {
      this._isModifier = false;
      string[] strArray = serializedKey.Split('+');
      string str = strArray[0];
      if (strArray.Length > 1)
      {
        this._modifier = str.ToEnum<KeyCode>();
        this._isModifier = true;
        str = strArray[1];
      }
      if (str.Length == 1 && this.AlphaDigits.Contains(str))
        str = "Alpha" + str;
      KeyCode keyCode = str.ToEnum<KeyCode>();
      if (keyCode != null)
      {
        this._key = keyCode;
        this._isSpecial = false;
      }
      else
      {
        this._special = str.ToEnum<SpecialKey>();
        this._isSpecial = true;
      }
    }

    protected bool GetModifier() => !this._isModifier || Input.GetKey(this._modifier);

    protected bool GetSpecial(SpecialKey specialKey)
    {
      if (specialKey == SpecialKey.WheelUp)
        return (double) Input.GetAxis("Mouse ScrollWheel") > 0.0;
      return specialKey == SpecialKey.WheelDown && (double) Input.GetAxis("Mouse ScrollWheel") < 0.0;
    }
  }
}
