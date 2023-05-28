// Decompiled with JetBrains decompiler
// Type: custom_inputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class custom_inputs : MonoBehaviour
{
  public bool allowDuplicates;
  public KeyCode[] alt_default_inputKeys;
  private float AltInputBox_X = 120f;
  private bool altInputson;
  [HideInInspector]
  public float analogFeel_down;
  public float analogFeel_gravity = 0.2f;
  [HideInInspector]
  public float analogFeel_jump;
  [HideInInspector]
  public float analogFeel_left;
  [HideInInspector]
  public float analogFeel_right;
  public float analogFeel_sensitivity = 0.8f;
  [HideInInspector]
  public float analogFeel_up;
  public float Boxes_Y = 300f;
  public float BoxesMargin_Y = 30f;
  private float buttonHeight = 20f;
  public int buttonSize = 200;
  public KeyCode[] default_inputKeys;
  private float DescBox_X = -320f;
  private float DescriptionBox_X;
  public int DescriptionSize = 200;
  public string[] DescriptionString;
  private bool[] inputBool;
  private bool[] inputBool2;
  private float InputBox_X = -100f;
  private float InputBox1_X;
  private float InputBox2_X;
  private KeyCode[] inputKey;
  private KeyCode[] inputKey2;
  private string[] inputString;
  private string[] inputString2;
  [HideInInspector]
  public bool[] isInput;
  [HideInInspector]
  public bool[] isInputDown;
  [HideInInspector]
  public bool[] isInputUp;
  [HideInInspector]
  public bool[] joystickActive;
  [HideInInspector]
  public bool[] joystickActive2;
  [HideInInspector]
  public string[] joystickString;
  [HideInInspector]
  public string[] joystickString2;
  private float lastInterval;
  public bool menuOn;
  public bool mouseAxisOn;
  public bool mouseButtonsOn = true;
  public GUISkin OurSkin;
  private float resetbuttonLocX = -100f;
  public float resetbuttonLocY = 600f;
  public string resetbuttonText = "Reset to defaults";
  private float resetbuttonX;
  private bool tempbool;
  private bool[] tempjoy1;
  private bool[] tempjoy2;
  private string tempkeyPressed;
  private int tempLength;

  private void checDoubleAxis(string testAxisString, int o, int p)
  {
    if (this.allowDuplicates)
      return;
    for (int index = 0; index < this.DescriptionString.Length; ++index)
    {
      if (testAxisString == this.joystickString[index] && (index != o || p == 2))
      {
        this.inputKey[index] = (KeyCode) 0;
        this.inputBool[index] = false;
        this.inputString[index] = this.inputKey[index].ToString();
        this.joystickActive[index] = false;
        this.joystickString[index] = "#";
        this.saveInputs();
      }
      if (testAxisString == this.joystickString2[index] && (index != o || p == 1))
      {
        this.inputKey2[index] = (KeyCode) 0;
        this.inputBool2[index] = false;
        this.inputString2[index] = this.inputKey2[index].ToString();
        this.joystickActive2[index] = false;
        this.joystickString2[index] = "#";
        this.saveInputs();
      }
    }
  }

  private void checDoubles(KeyCode testkey, int o, int p)
  {
    if (this.allowDuplicates)
      return;
    for (int index = 0; index < this.DescriptionString.Length; ++index)
    {
      if (testkey == (int) this.inputKey[index] && (index != o || p == 2))
      {
        this.inputKey[index] = (KeyCode) 0;
        this.inputBool[index] = false;
        this.inputString[index] = this.inputKey[index].ToString();
        this.joystickActive[index] = false;
        this.joystickString[index] = "#";
        this.saveInputs();
      }
      if (testkey == (int) this.inputKey2[index] && (index != o || p == 1))
      {
        this.inputKey2[index] = (KeyCode) 0;
        this.inputBool2[index] = false;
        this.inputString2[index] = this.inputKey2[index].ToString();
        this.joystickActive2[index] = false;
        this.joystickString2[index] = "#";
        this.saveInputs();
      }
    }
  }

  private void drawButtons1()
  {
    float boxesY = this.Boxes_Y;
    float x = Input.mousePosition.x;
    float y = Input.mousePosition.y;
    Matrix4x4 matrix4x4 = GUI.matrix;
    matrix4x4 = ((Matrix4x4) ref matrix4x4).inverse;
    Vector3 vector3 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(new Vector3(x, (float) Screen.height - y, 1f));
    GUI.skin = this.OurSkin;
    GUI.Box(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), string.Empty);
    GUI.Box(new Rect(60f, 60f, (float) (Screen.width - 120), (float) (Screen.height - 120)), string.Empty, GUIStyle.op_Implicit("window"));
    GUI.Label(new Rect(this.DescriptionBox_X, boxesY - 10f, (float) this.DescriptionSize, this.buttonHeight), "name", GUIStyle.op_Implicit("textfield"));
    GUI.Label(new Rect(this.InputBox1_X, boxesY - 10f, (float) this.DescriptionSize, this.buttonHeight), "input", GUIStyle.op_Implicit("textfield"));
    GUI.Label(new Rect(this.InputBox2_X, boxesY - 10f, (float) this.DescriptionSize, this.buttonHeight), "alt input", GUIStyle.op_Implicit("textfield"));
    for (int o = 0; o < this.DescriptionString.Length; ++o)
    {
      boxesY += this.BoxesMargin_Y;
      GUI.Label(new Rect(this.DescriptionBox_X, boxesY, (float) this.DescriptionSize, this.buttonHeight), this.DescriptionString[o], GUIStyle.op_Implicit("box"));
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(this.InputBox1_X, boxesY, (float) this.buttonSize, this.buttonHeight);
      GUI.Button(rect, this.inputString[o]);
      if (!this.joystickActive[o] && (int) this.inputKey[o] == 0)
        this.joystickString[o] = "#";
      if (this.inputBool[o])
        GUI.Toggle(rect, true, string.Empty, this.OurSkin.button);
      if (((Rect) ref rect).Contains(vector3) && Input.GetMouseButtonUp(0) && !this.tempbool)
      {
        this.tempbool = true;
        this.inputBool[o] = true;
        this.lastInterval = Time.realtimeSinceStartup;
      }
      if (GUI.Button(new Rect(this.resetbuttonX, this.resetbuttonLocY, (float) this.buttonSize, this.buttonHeight), this.resetbuttonText) && Input.GetMouseButtonUp(0))
      {
        PlayerPrefs.DeleteAll();
        this.reset2defaults();
        this.loadConfig();
        this.saveInputs();
      }
      if (Event.current.type == 4 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = Event.current.keyCode;
        this.inputBool[o] = false;
        this.inputString[o] = this.inputKey[o].ToString();
        this.tempbool = false;
        this.joystickActive[o] = false;
        this.joystickString[o] = "#";
        this.saveInputs();
        this.checDoubles((KeyCode) (int) this.inputKey[o], o, 1);
      }
      if (this.mouseButtonsOn)
      {
        int num = 323;
        for (int index = 0; index < 6; ++index)
        {
          if (Input.GetMouseButton(index) && this.inputBool[o] && Event.current.keyCode != 27)
          {
            num += index;
            this.inputKey[o] = (KeyCode) num;
            this.inputBool[o] = false;
            this.inputString[o] = this.inputKey[o].ToString();
            this.joystickActive[o] = false;
            this.joystickString[o] = "#";
            this.saveInputs();
            this.checDoubles((KeyCode) (int) this.inputKey[o], o, 1);
          }
        }
      }
      for (int index = 350; index < 409; ++index)
      {
        if (Input.GetKey((KeyCode) index) && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) index;
          this.inputBool[o] = false;
          this.inputString[o] = this.inputKey[o].ToString();
          this.tempbool = false;
          this.joystickActive[o] = false;
          this.joystickString[o] = "#";
          this.saveInputs();
          this.checDoubles((KeyCode) (int) this.inputKey[o], o, 1);
        }
      }
      if (this.mouseAxisOn)
      {
        if ((double) Input.GetAxis("MouseUp") == 1.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseUp";
          this.inputString[o] = "Mouse Up";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
        if ((double) Input.GetAxis("MouseDown") == 1.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseDown";
          this.inputString[o] = "Mouse Down";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
        if ((double) Input.GetAxis("MouseLeft") == 1.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseLeft";
          this.inputBool[o] = false;
          this.inputString[o] = "Mouse Left";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
        if ((double) Input.GetAxis("MouseRight") == 1.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseRight";
          this.inputString[o] = "Mouse Right";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
      }
      if (this.mouseButtonsOn)
      {
        if ((double) Input.GetAxis("MouseScrollUp") > 0.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseScrollUp";
          this.inputBool[o] = false;
          this.inputString[o] = "Mouse scroll Up";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
        if ((double) Input.GetAxis("MouseScrollDown") > 0.0 && this.inputBool[o] && Event.current.keyCode != 27)
        {
          this.inputKey[o] = (KeyCode) 0;
          this.inputBool[o] = false;
          this.joystickActive[o] = true;
          this.joystickString[o] = "MouseScrollDown";
          this.inputBool[o] = false;
          this.inputString[o] = "Mouse scroll Down";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString[o], o, 1);
        }
      }
      if ((double) Input.GetAxis("JoystickUp") > 0.5 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "JoystickUp";
        this.inputString[o] = "Joystick Up";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("JoystickDown") > 0.5 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "JoystickDown";
        this.inputString[o] = "Joystick Down";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("JoystickLeft") > 0.5 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "JoystickLeft";
        this.inputString[o] = "Joystick Left";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("JoystickRight") > 0.5 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "JoystickRight";
        this.inputString[o] = "Joystick Right";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_3a") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_3a";
        this.inputString[o] = "Joystick Axis 3 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_3b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_3b";
        this.inputString[o] = "Joystick Axis 3 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_4a") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_4a";
        this.inputString[o] = "Joystick Axis 4 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_4b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_4b";
        this.inputString[o] = "Joystick Axis 4 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_5b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_5b";
        this.inputString[o] = "Joystick Axis 5 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_6b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_6b";
        this.inputString[o] = "Joystick Axis 6 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_7a") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_7a";
        this.inputString[o] = "Joystick Axis 7 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_7b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_7b";
        this.inputString[o] = "Joystick Axis 7 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_8a") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_8a";
        this.inputString[o] = "Joystick Axis 8 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
      if ((double) Input.GetAxis("Joystick_8b") > 0.800000011920929 && this.inputBool[o] && Event.current.keyCode != 27)
      {
        this.inputKey[o] = (KeyCode) 0;
        this.inputBool[o] = false;
        this.joystickActive[o] = true;
        this.joystickString[o] = "Joystick_8b";
        this.inputString[o] = "Joystick Axis 8 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString[o], o, 1);
      }
    }
  }

  private void drawButtons2()
  {
    float boxesY = this.Boxes_Y;
    float x = Input.mousePosition.x;
    float y = Input.mousePosition.y;
    Matrix4x4 matrix4x4 = GUI.matrix;
    matrix4x4 = ((Matrix4x4) ref matrix4x4).inverse;
    Vector3 vector3 = ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(new Vector3(x, (float) Screen.height - y, 1f));
    GUI.skin = this.OurSkin;
    for (int o = 0; o < this.DescriptionString.Length; ++o)
    {
      boxesY += this.BoxesMargin_Y;
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(this.InputBox2_X, boxesY, (float) this.buttonSize, this.buttonHeight);
      GUI.Button(rect, this.inputString2[o]);
      if (!this.joystickActive2[o] && (int) this.inputKey2[o] == 0)
        this.joystickString2[o] = "#";
      if (this.inputBool2[o])
        GUI.Toggle(rect, true, string.Empty, this.OurSkin.button);
      if (((Rect) ref rect).Contains(vector3) && Input.GetMouseButtonUp(0) && !this.tempbool)
      {
        this.tempbool = true;
        this.inputBool2[o] = true;
        this.lastInterval = Time.realtimeSinceStartup;
      }
      if (Event.current.type == 4 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = Event.current.keyCode;
        this.inputBool2[o] = false;
        this.inputString2[o] = this.inputKey2[o].ToString();
        this.tempbool = false;
        this.joystickActive2[o] = false;
        this.joystickString2[o] = "#";
        this.saveInputs();
        this.checDoubles((KeyCode) (int) this.inputKey2[o], o, 2);
      }
      if (this.mouseButtonsOn)
      {
        int num = 323;
        for (int index = 0; index < 6; ++index)
        {
          if (Input.GetMouseButton(index) && this.inputBool2[o] && Event.current.keyCode != 27)
          {
            num += index;
            this.inputKey2[o] = (KeyCode) num;
            this.inputBool2[o] = false;
            this.inputString2[o] = this.inputKey2[o].ToString();
            this.joystickActive2[o] = false;
            this.joystickString2[o] = "#";
            this.saveInputs();
            this.checDoubles((KeyCode) (int) this.inputKey2[o], o, 2);
          }
        }
      }
      for (int index = 350; index < 409; ++index)
      {
        if (Input.GetKey((KeyCode) index) && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) index;
          this.inputBool2[o] = false;
          this.inputString2[o] = this.inputKey2[o].ToString();
          this.tempbool = false;
          this.joystickActive2[o] = false;
          this.joystickString2[o] = "#";
          this.saveInputs();
          this.checDoubles((KeyCode) (int) this.inputKey2[o], o, 2);
        }
      }
      if (this.mouseAxisOn)
      {
        if ((double) Input.GetAxis("MouseUp") == 1.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseUp";
          this.inputString2[o] = "Mouse Up";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
        if ((double) Input.GetAxis("MouseDown") == 1.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseDown";
          this.inputString2[o] = "Mouse Down";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
        if ((double) Input.GetAxis("MouseLeft") == 1.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseLeft";
          this.inputBool2[o] = false;
          this.inputString2[o] = "Mouse Left";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
        if ((double) Input.GetAxis("MouseRight") == 1.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseRight";
          this.inputString2[o] = "Mouse Right";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
      }
      if (this.mouseButtonsOn)
      {
        if ((double) Input.GetAxis("MouseScrollUp") > 0.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseScrollUp";
          this.inputBool2[o] = false;
          this.inputString2[o] = "Mouse scroll Up";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
        if ((double) Input.GetAxis("MouseScrollDown") > 0.0 && this.inputBool2[o] && Event.current.keyCode != 27)
        {
          this.inputKey2[o] = (KeyCode) 0;
          this.inputBool2[o] = false;
          this.joystickActive2[o] = true;
          this.joystickString2[o] = "MouseScrollDown";
          this.inputBool2[o] = false;
          this.inputString2[o] = "Mouse scroll Down";
          this.tempbool = false;
          this.saveInputs();
          this.checDoubleAxis(this.joystickString2[o], o, 2);
        }
      }
      if ((double) Input.GetAxis("JoystickUp") > 0.5 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "JoystickUp";
        this.inputString2[o] = "Joystick Up";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("JoystickDown") > 0.5 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "JoystickDown";
        this.inputString2[o] = "Joystick Down";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("JoystickLeft") > 0.5 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "JoystickLeft";
        this.inputBool2[o] = false;
        this.inputString2[o] = "Joystick Left";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("JoystickRight") > 0.5 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "JoystickRight";
        this.inputString2[o] = "Joystick Right";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_3a") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_3a";
        this.inputString2[o] = "Joystick Axis 3 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_3b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_3b";
        this.inputString2[o] = "Joystick Axis 3 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_4a") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_4a";
        this.inputString2[o] = "Joystick Axis 4 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_4b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_4b";
        this.inputString2[o] = "Joystick Axis 4 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_5b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_5b";
        this.inputString2[o] = "Joystick Axis 5 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_6b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_6b";
        this.inputString2[o] = "Joystick Axis 6 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_7a") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_7a";
        this.inputString2[o] = "Joystick Axis 7 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_7b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_7b";
        this.inputString2[o] = "Joystick Axis 7 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_8a") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_8a";
        this.inputString2[o] = "Joystick Axis 8 +";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
      if ((double) Input.GetAxis("Joystick_8b") > 0.800000011920929 && this.inputBool2[o] && Event.current.keyCode != 27)
      {
        this.inputKey2[o] = (KeyCode) 0;
        this.inputBool2[o] = false;
        this.joystickActive2[o] = true;
        this.joystickString2[o] = "Joystick_8b";
        this.inputString2[o] = "Joystick Axis 8 -";
        this.tempbool = false;
        this.saveInputs();
        this.checDoubleAxis(this.joystickString2[o], o, 2);
      }
    }
  }

  private void inputSetBools()
  {
    for (int index = 0; index < this.DescriptionString.Length; ++index)
    {
      this.isInput[index] = Input.GetKey((KeyCode) (int) this.inputKey[index]) || this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) > 0.949999988079071 || Input.GetKey((KeyCode) (int) this.inputKey2[index]) || this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) > 0.949999988079071;
      this.isInputDown[index] = Input.GetKeyDown((KeyCode) (int) this.inputKey[index]) || Input.GetKeyDown((KeyCode) (int) this.inputKey2[index]);
      if (this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) > 0.949999988079071 || this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) > 0.949999988079071)
      {
        if (!this.tempjoy1[index])
          this.isInputDown[index] = false;
        if (this.tempjoy1[index])
        {
          this.isInputDown[index] = true;
          this.tempjoy1[index] = false;
        }
      }
      if (!this.tempjoy1[index] && this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) < 0.10000000149011612 && this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) < 0.10000000149011612)
      {
        this.isInputDown[index] = false;
        this.tempjoy1[index] = true;
      }
      if (!this.tempjoy1[index] && !this.joystickActive[index] && this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) < 0.10000000149011612)
      {
        this.isInputDown[index] = false;
        this.tempjoy1[index] = true;
      }
      if (!this.tempjoy1[index] && !this.joystickActive2[index] && this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) < 0.10000000149011612)
      {
        this.isInputDown[index] = false;
        this.tempjoy1[index] = true;
      }
      this.isInputUp[index] = Input.GetKeyUp((KeyCode) (int) this.inputKey[index]) || Input.GetKeyUp((KeyCode) (int) this.inputKey2[index]);
      if (this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) > 0.949999988079071 || this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) > 0.949999988079071)
      {
        if (this.tempjoy2[index])
          this.isInputUp[index] = false;
        if (!this.tempjoy2[index])
        {
          this.isInputUp[index] = false;
          this.tempjoy2[index] = true;
        }
      }
      if (this.tempjoy2[index] && this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) < 0.10000000149011612 && this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) < 0.10000000149011612)
      {
        this.isInputUp[index] = true;
        this.tempjoy2[index] = false;
      }
      if (this.tempjoy2[index] && !this.joystickActive[index] && this.joystickActive2[index] && (double) Input.GetAxis(this.joystickString2[index]) < 0.10000000149011612)
      {
        this.isInputUp[index] = true;
        this.tempjoy2[index] = false;
      }
      if (this.tempjoy2[index] && !this.joystickActive2[index] && this.joystickActive[index] && (double) Input.GetAxis(this.joystickString[index]) < 0.10000000149011612)
      {
        this.isInputUp[index] = true;
        this.tempjoy2[index] = false;
      }
    }
  }

  private void loadConfig()
  {
    string str1 = PlayerPrefs.GetString("KeyCodes");
    string str2 = PlayerPrefs.GetString("Joystick_input");
    string str3 = PlayerPrefs.GetString("Names_input");
    string str4 = PlayerPrefs.GetString("KeyCodes2");
    string str5 = PlayerPrefs.GetString("Joystick_input2");
    string str6 = PlayerPrefs.GetString("Names_input2");
    char[] chArray1 = new char[1]{ '*' };
    string[] strArray1 = str1.Split(chArray1);
    char[] chArray2 = new char[1]{ '*' };
    this.joystickString = str2.Split(chArray2);
    char[] chArray3 = new char[1]{ '*' };
    this.inputString = str3.Split(chArray3);
    char[] chArray4 = new char[1]{ '*' };
    string[] strArray2 = str4.Split(chArray4);
    char[] chArray5 = new char[1]{ '*' };
    this.joystickString2 = str5.Split(chArray5);
    char[] chArray6 = new char[1]{ '*' };
    this.inputString2 = str6.Split(chArray6);
    for (int index = 0; index < this.DescriptionString.Length; ++index)
    {
      int result1;
      int.TryParse(strArray1[index], out result1);
      this.inputKey[index] = (KeyCode) result1;
      int result2;
      int.TryParse(strArray2[index], out result2);
      this.inputKey2[index] = (KeyCode) result2;
      this.joystickActive[index] = !(this.joystickString[index] == "#");
      this.joystickActive2[index] = !(this.joystickString2[index] == "#");
    }
  }

  private void OnGUI()
  {
    if ((double) Time.realtimeSinceStartup > (double) this.lastInterval + 3.0)
      this.tempbool = false;
    if (!this.menuOn)
      return;
    this.drawButtons1();
    if (!this.altInputson)
      return;
    this.drawButtons2();
  }

  private void reset2defaults()
  {
    if (this.default_inputKeys.Length != this.DescriptionString.Length)
      this.default_inputKeys = new KeyCode[this.DescriptionString.Length];
    if (this.alt_default_inputKeys.Length != this.default_inputKeys.Length)
      this.alt_default_inputKeys = new KeyCode[this.default_inputKeys.Length];
    string str1 = string.Empty;
    string empty1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    string empty2 = string.Empty;
    string str4 = string.Empty;
    for (int index = this.DescriptionString.Length - 1; index > -1; --index)
    {
      int num = (int) this.default_inputKeys[index];
      str1 = num.ToString() + "*" + str1;
      empty1 += "#*";
      str2 = this.default_inputKeys[index].ToString() + "*" + str2;
      PlayerPrefs.SetString("KeyCodes", str1);
      PlayerPrefs.SetString("Joystick_input", empty1);
      PlayerPrefs.SetString("Names_input", str2);
      num = (int) this.alt_default_inputKeys[index];
      str3 = num.ToString() + "*" + str3;
      empty2 += "#*";
      str4 = this.alt_default_inputKeys[index].ToString() + "*" + str4;
      PlayerPrefs.SetString("KeyCodes2", str3);
      PlayerPrefs.SetString("Joystick_input2", empty2);
      PlayerPrefs.SetString("Names_input2", str4);
      PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
    }
  }

  private void saveInputs()
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    string str4 = string.Empty;
    string str5 = string.Empty;
    string str6 = string.Empty;
    for (int index = this.DescriptionString.Length - 1; index > -1; --index)
    {
      int num = (int) this.inputKey[index];
      str1 = num.ToString() + "*" + str1;
      str2 = this.joystickString[index] + "*" + str2;
      str3 = this.inputString[index] + "*" + str3;
      num = (int) this.inputKey2[index];
      str4 = num.ToString() + "*" + str4;
      str5 = this.joystickString2[index] + "*" + str5;
      str6 = this.inputString2[index] + "*" + str6;
    }
    PlayerPrefs.SetString("KeyCodes", str1);
    PlayerPrefs.SetString("Joystick_input", str2);
    PlayerPrefs.SetString("Names_input", str3);
    PlayerPrefs.SetString("KeyCodes2", str4);
    PlayerPrefs.SetString("Joystick_input2", str5);
    PlayerPrefs.SetString("Names_input2", str6);
    PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
  }

  private void Start()
  {
    if (this.alt_default_inputKeys.Length == this.default_inputKeys.Length)
      this.altInputson = true;
    this.inputBool = new bool[this.DescriptionString.Length];
    this.inputString = new string[this.DescriptionString.Length];
    this.inputKey = new KeyCode[this.DescriptionString.Length];
    this.joystickActive = new bool[this.DescriptionString.Length];
    this.joystickString = new string[this.DescriptionString.Length];
    this.inputBool2 = new bool[this.DescriptionString.Length];
    this.inputString2 = new string[this.DescriptionString.Length];
    this.inputKey2 = new KeyCode[this.DescriptionString.Length];
    this.joystickActive2 = new bool[this.DescriptionString.Length];
    this.joystickString2 = new string[this.DescriptionString.Length];
    this.isInput = new bool[this.DescriptionString.Length];
    this.isInputDown = new bool[this.DescriptionString.Length];
    this.isInputUp = new bool[this.DescriptionString.Length];
    this.tempLength = PlayerPrefs.GetInt("KeyLength");
    this.tempjoy1 = new bool[this.DescriptionString.Length];
    this.tempjoy2 = new bool[this.DescriptionString.Length];
    if (!PlayerPrefs.HasKey("KeyCodes") || !PlayerPrefs.HasKey("KeyCodes2"))
      this.reset2defaults();
    this.tempLength = PlayerPrefs.GetInt("KeyLength");
    if (PlayerPrefs.HasKey("KeyCodes") && this.tempLength == this.DescriptionString.Length)
    {
      this.loadConfig();
    }
    else
    {
      PlayerPrefs.DeleteAll();
      this.reset2defaults();
      this.loadConfig();
      this.saveInputs();
    }
    for (int index = 0; index < this.DescriptionString.Length; ++index)
    {
      this.isInput[index] = false;
      this.isInputDown[index] = false;
      this.isInputUp[index] = false;
      this.tempjoy1[index] = true;
      this.tempjoy2[index] = false;
    }
  }

  private void Update()
  {
    this.DescriptionBox_X = (float) (Screen.width / 2) + this.DescBox_X;
    this.InputBox1_X = (float) (Screen.width / 2) + this.InputBox_X;
    this.InputBox2_X = (float) (Screen.width / 2) + this.AltInputBox_X;
    this.resetbuttonX = (float) (Screen.width / 2) + this.resetbuttonLocX;
    if (!this.menuOn)
      this.inputSetBools();
    if (!Input.GetKeyDown("escape"))
      return;
    if (this.menuOn)
    {
      Time.timeScale = 1f;
      this.tempbool = false;
      this.menuOn = false;
      this.saveInputs();
    }
    else
    {
      Time.timeScale = 0.0f;
      this.menuOn = true;
      Screen.showCursor = true;
      Screen.lockCursor = false;
    }
  }
}
