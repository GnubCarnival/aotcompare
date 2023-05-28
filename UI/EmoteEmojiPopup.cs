// Decompiled with JetBrains decompiler
// Type: UI.EmoteEmojiPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class EmoteEmojiPopup : EmoteTextPopup
  {
    protected RawImage _emojiImage;

    protected override Vector3 offset => Vector3.op_Multiply(Vector3.up, 3f);

    public override void Setup(BasePanel parent = null)
    {
      this._emojiImage = ((Component) ((Component) this).transform.Find("Panel/Emoji")).GetComponent<RawImage>();
      this._transform = ((Component) this).transform;
    }

    protected override void SetEmote(string text) => this._emojiImage.texture = (Texture) GameMenu.EmojiTextures[text];
  }
}
