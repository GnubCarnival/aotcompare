// Decompiled with JetBrains decompiler
// Type: UI.GameMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class GameMenu : BaseMenu
  {
    public static Dictionary<string, Texture2D> EmojiTextures = new Dictionary<string, Texture2D>();
    public static List<string> AvailableEmojis = new List<string>()
    {
      "Smile",
      "ThumbsUp",
      "Cool",
      "Love",
      "Shocked",
      "Crying",
      "Annoyed",
      "Angry"
    };
    public static List<string> AvailableText = new List<string>()
    {
      "Help",
      "Thanks",
      "Sorry",
      "Titan here",
      "Good game",
      "Nice hit",
      "Oops",
      "Welcome"
    };
    public static List<string> AvailableActions = new List<string>()
    {
      "Salute",
      "Dance",
      "Flip",
      "Wave1",
      "Wave2",
      "Eat"
    };
    private const float EmoteCooldown = 4f;
    public static bool Paused;
    public static bool WheelMenu;
    public static bool HideCrosshair;
    public List<BasePopup> _emoteTextPopups = new List<BasePopup>();
    public List<BasePopup> _emoteEmojiPopups = new List<BasePopup>();
    public BasePopup _settingsPopup;
    public BasePopup _emoteWheelPopup;
    public BasePopup _itemWheelPopup;
    public RawImage _crosshairImageWhite;
    public RawImage _crosshairImageRed;
    public Text _crosshairLabelWhite;
    public Text _crosshairLabelRed;
    private float _currentEmoteCooldown;
    private EmoteWheelState _currentEmoteWheelState;

    public override void Setup()
    {
      base.Setup();
      GameMenu.HideCrosshair = false;
      GameMenu.TogglePause(false);
      GameMenu.WheelMenu = false;
      this.SetupCrosshairs();
    }

    public static bool InMenu() => GameMenu.Paused || GameMenu.WheelMenu;

    public static void TogglePause(bool pause)
    {
      GameMenu.Paused = pause;
      if (Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null) && Object.op_Inequality((Object) ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>(), (Object) null))
      {
        GameMenu component = ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>();
        if (GameMenu.Paused && !((Component) component._settingsPopup).gameObject.activeSelf)
        {
          component._settingsPopup.Show();
          component._emoteWheelPopup.Hide();
          GameMenu.WheelMenu = false;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            Time.timeScale = 0.0f;
        }
        else
        {
          GameMenu.Paused = false;
          component._settingsPopup.Hide();
          if (!((Behaviour) ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>()).enabled)
          {
            ((Component) Camera.main).GetComponent<SpectatorMovement>().disable = false;
            ((Component) Camera.main).GetComponent<MouseLook>().disable = false;
          }
        }
      }
      if (GameMenu.Paused || (double) FengGameManagerMKII.instance.pauseWaitTime > 0.0)
        return;
      Time.timeScale = 1f;
    }

    public static void OnEmoteTextRPC(int viewId, string text, PhotonMessageInfo info)
    {
      if (Object.op_Equality((Object) UIManager.CurrentMenu, (Object) null) || !SettingsManager.UISettings.ShowEmotes.Value)
        return;
      GameMenu component = ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>();
      Transform transformFromViewId = GameMenu.GetTransformFromViewId(viewId, info);
      if (!Object.op_Inequality((Object) transformFromViewId, (Object) null) || !Object.op_Inequality((Object) component, (Object) null))
        return;
      component.ShowEmoteText(text, transformFromViewId);
    }

    public static void OnEmoteEmojiRPC(int viewId, string emoji, PhotonMessageInfo info)
    {
      if (Object.op_Equality((Object) UIManager.CurrentMenu, (Object) null) || !SettingsManager.UISettings.ShowEmotes.Value)
        return;
      GameMenu component = ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>();
      Transform transformFromViewId = GameMenu.GetTransformFromViewId(viewId, info);
      if (!Object.op_Inequality((Object) transformFromViewId, (Object) null) || !Object.op_Inequality((Object) component, (Object) null))
        return;
      component.ShowEmoteEmoji(emoji, transformFromViewId);
    }

    public static void ToggleEmoteWheel(bool enable)
    {
      if (!Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null) || !Object.op_Inequality((Object) ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>(), (Object) null))
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameMenu.\u003C\u003Ec__DisplayClass24_0 cDisplayClass240 = new GameMenu.\u003C\u003Ec__DisplayClass24_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass240.menu = ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>();
      if (enable)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((WheelPopup) cDisplayClass240.menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(), GameMenu.GetEmoteWheelOptions(cDisplayClass240.menu._currentEmoteWheelState), new UnityAction((object) cDisplayClass240, __methodptr(\u003CToggleEmoteWheel\u003Eb__0)));
        GameMenu.WheelMenu = true;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass240.menu._emoteWheelPopup.Hide();
        GameMenu.WheelMenu = false;
      }
    }

    public static void NextEmoteWheel()
    {
      if (!Object.op_Inequality((Object) UIManager.CurrentMenu, (Object) null) || !Object.op_Inequality((Object) ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>(), (Object) null))
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameMenu.\u003C\u003Ec__DisplayClass25_0 cDisplayClass250 = new GameMenu.\u003C\u003Ec__DisplayClass25_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass250.menu = ((Component) UIManager.CurrentMenu).GetComponent<GameMenu>();
      // ISSUE: reference to a compiler-generated field
      if (!((Component) cDisplayClass250.menu._emoteWheelPopup).gameObject.activeSelf || !GameMenu.WheelMenu)
        return;
      // ISSUE: reference to a compiler-generated field
      ++cDisplayClass250.menu._currentEmoteWheelState;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass250.menu._currentEmoteWheelState > EmoteWheelState.Action)
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass250.menu._currentEmoteWheelState = EmoteWheelState.Text;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((WheelPopup) cDisplayClass250.menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(), GameMenu.GetEmoteWheelOptions(cDisplayClass250.menu._currentEmoteWheelState), new UnityAction((object) cDisplayClass250, __methodptr(\u003CNextEmoteWheel\u003Eb__0)));
    }

    public void ShowEmoteText(string text, Transform parent)
    {
      EmoteTextPopup availablePopup = (EmoteTextPopup) this.GetAvailablePopup(this._emoteTextPopups);
      if (text.Length > 20)
        text = text.Substring(0, 20);
      availablePopup.Show(text, parent);
    }

    public void ShowEmoteEmoji(string emoji, Transform parent) => ((EmoteTextPopup) this.GetAvailablePopup(this._emoteEmojiPopups)).Show(emoji, parent);

    private void OnEmoteWheelSelect()
    {
      if (this._currentEmoteWheelState != EmoteWheelState.Action)
      {
        if ((double) this._currentEmoteCooldown > 0.0)
          return;
        this._currentEmoteCooldown = 4f;
      }
      HERO myHero = RCextensions.GetMyHero();
      if (Object.op_Equality((Object) myHero, (Object) null))
        return;
      if (this._currentEmoteWheelState == EmoteWheelState.Text)
      {
        string text = GameMenu.AvailableText[((WheelPopup) this._emoteWheelPopup).SelectedItem];
        switch (IN_GAME_MAIN_CAMERA.gametype)
        {
          case GAMETYPE.SINGLE:
            this.ShowEmoteText(text, ((Component) myHero).transform);
            break;
          case GAMETYPE.MULTIPLAYER:
            CustomRPCManager.PhotonView.RPC("EmoteTextRPC", PhotonTargets.All, (object) myHero.photonView.viewID, (object) text);
            break;
        }
      }
      else if (this._currentEmoteWheelState == EmoteWheelState.Emoji)
      {
        string availableEmoji = GameMenu.AvailableEmojis[((WheelPopup) this._emoteWheelPopup).SelectedItem];
        switch (IN_GAME_MAIN_CAMERA.gametype)
        {
          case GAMETYPE.SINGLE:
            this.ShowEmoteEmoji(availableEmoji, ((Component) myHero).transform);
            break;
          case GAMETYPE.MULTIPLAYER:
            CustomRPCManager.PhotonView.RPC("EmoteEmojiRPC", PhotonTargets.All, (object) myHero.photonView.viewID, (object) availableEmoji);
            break;
        }
      }
      else if (this._currentEmoteWheelState == EmoteWheelState.Action)
      {
        switch (GameMenu.AvailableActions[((WheelPopup) this._emoteWheelPopup).SelectedItem])
        {
          case "Salute":
            myHero.EmoteAction("salute");
            break;
          case "Dance":
            myHero.EmoteAction("special_armin");
            break;
          case "Flip":
            myHero.EmoteAction("dodge");
            break;
          case "Wave1":
            myHero.EmoteAction("special_marco_0");
            break;
          case "Wave2":
            myHero.EmoteAction("special_marco_1");
            break;
          case "Eat":
            myHero.EmoteAction("special_sasha");
            break;
        }
      }
      myHero._flareDelayAfterEmote = 2f;
      this._emoteWheelPopup.Hide();
      GameMenu.WheelMenu = false;
    }

    private static Transform GetTransformFromViewId(int viewId, PhotonMessageInfo info)
    {
      PhotonView photonView = PhotonView.Find(viewId);
      return Object.op_Inequality((Object) photonView, (Object) null) && photonView.owner == info.sender ? ((Component) photonView).transform : (Transform) null;
    }

    private static List<string> GetEmoteWheelOptions(EmoteWheelState state)
    {
      if (state == EmoteWheelState.Text)
        return GameMenu.AvailableText;
      return state == EmoteWheelState.Emoji ? GameMenu.AvailableEmojis : GameMenu.AvailableActions;
    }

    private BasePopup GetAvailablePopup(List<BasePopup> popups)
    {
      foreach (BasePopup popup in popups)
      {
        if (!((Component) popup).gameObject.activeSelf)
          return popup;
      }
      return popups[0];
    }

    protected void SetupCrosshairs()
    {
      this._crosshairImageWhite = ElementFactory.InstantiateAndBind(((Component) this).transform, "CrosshairImage").GetComponent<RawImage>();
      this._crosshairImageRed = ElementFactory.InstantiateAndBind(((Component) this).transform, "CrosshairImage").GetComponent<RawImage>();
      ((Graphic) this._crosshairImageRed).color = Color.red;
      this._crosshairLabelWhite = ((Component) ((Component) this._crosshairImageWhite).transform.Find("DefaultLabel")).GetComponent<Text>();
      this._crosshairLabelRed = ((Component) ((Component) this._crosshairImageRed).transform.Find("DefaultLabel")).GetComponent<Text>();
      ElementFactory.SetAnchor(((Component) this._crosshairImageWhite).gameObject, (TextAnchor) 4, (TextAnchor) 4, Vector2.zero);
      ElementFactory.SetAnchor(((Component) this._crosshairImageRed).gameObject, (TextAnchor) 4, (TextAnchor) 4, Vector2.zero);
      ((Component) this._crosshairImageWhite).gameObject.AddComponent<CrosshairScaler>();
      ((Component) this._crosshairImageRed).gameObject.AddComponent<CrosshairScaler>();
      CursorManager.UpdateCrosshair(this._crosshairImageWhite, this._crosshairImageRed, this._crosshairLabelWhite, this._crosshairLabelRed, true);
    }

    protected override void SetupPopups()
    {
      base.SetupPopups();
      this._settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(((Component) this).transform).GetComponent<BasePopup>();
      this._emoteWheelPopup = ElementFactory.InstantiateAndSetupPanel<WheelPopup>(((Component) this).transform, "WheelMenu").GetComponent<BasePopup>();
      for (int index = 0; index < 5; ++index)
      {
        this._emoteTextPopups.Add(ElementFactory.InstantiateAndSetupPanel<EmoteTextPopup>(((Component) this).transform, "EmoteTextPopup").GetComponent<BasePopup>());
        this._emoteEmojiPopups.Add(ElementFactory.InstantiateAndSetupPanel<EmoteEmojiPopup>(((Component) this).transform, "EmoteEmojiPopup").GetComponent<BasePopup>());
      }
      this._popups.Add(this._settingsPopup);
      this._popups.Add(this._emoteWheelPopup);
    }

    private void Update()
    {
      CursorManager.UpdateCrosshair(this._crosshairImageWhite, this._crosshairImageRed, this._crosshairLabelWhite, this._crosshairLabelRed);
      this._currentEmoteCooldown -= Time.deltaTime;
    }
  }
}
