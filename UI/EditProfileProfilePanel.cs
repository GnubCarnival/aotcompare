// Decompiled with JetBrains decompiler
// Type: UI.EditProfileProfilePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class EditProfileProfilePanel : BasePanel
  {
    protected override float Width => 720f;

    protected override float Height => 520f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      ProfileSettings profileSettings = SettingsManager.ProfileSettings;
      ElementStyle style = new ElementStyle(themePanel: this.ThemePanel);
      ElementFactory.CreateInputSetting(this.SinglePanel, style, (BaseSetting) profileSettings.Name, UIManager.GetLocaleCommon("Name"), elementWidth: 200f);
      ElementFactory.CreateInputSetting(this.SinglePanel, style, (BaseSetting) profileSettings.Guild, UIManager.GetLocaleCommon("Guild"), elementWidth: 200f);
    }
  }
}
