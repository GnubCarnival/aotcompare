// Decompiled with JetBrains decompiler
// Type: UI.EmoteTextPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class EmoteTextPopup : BasePopup
  {
    private const float ShowTime = 3f;
    private Text _text;
    protected Transform _parent;
    protected float _currentShowTime;
    protected bool _isHiding;
    protected Transform _transform;
    protected Camera _camera;

    protected override float AnimationTime => 0.25f;

    protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

    protected virtual Vector3 offset => Vector3.op_Multiply(Vector3.up, 2.5f);

    public override void Setup(BasePanel parent = null)
    {
      this._text = ((Component) ((Component) this).transform.Find("Panel/Text/Label")).GetComponent<Text>();
      this._transform = ((Component) this).transform;
    }

    public void Show(string text, Transform parent)
    {
      this._parent = parent;
      this._currentShowTime = 3f;
      this._isHiding = false;
      this._camera = Camera.main;
      this.SetEmote(text);
      this.SetPosition();
      this.Show();
    }

    protected virtual void SetEmote(string text) => this._text.text = text;

    protected void SetPosition()
    {
      if (!Object.op_Inequality((Object) this._parent, (Object) null))
        return;
      this._transform.position = this._camera.WorldToScreenPoint(Vector3.op_Addition(this._parent.position, this.offset));
    }

    protected void LateUpdate()
    {
      this.SetPosition();
      this._currentShowTime -= Time.deltaTime;
      if ((double) this._currentShowTime > 0.0 || this._isHiding)
        return;
      this._isHiding = true;
      this.Hide();
    }
  }
}
