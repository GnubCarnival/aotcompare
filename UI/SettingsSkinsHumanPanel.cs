// Decompiled with JetBrains decompiler
// Type: UI.SettingsSkinsHumanPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;

namespace UI
{
  internal class SettingsSkinsHumanPanel : SettingsCategoryPanel
  {
    protected override float VerticalSpacing => 20f;

    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsSkinsPanel.Parent;
      HumanCustomSkinSettings human = SettingsManager.CustomSkinSettings.Human;
      settingsSkinsPanel.CreateCommonSettings(this.DoublePanelLeft, this.DoublePanelRight);
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel), (BaseSetting) human.GasEnabled, UIManager.GetLocale(parent1.LocaleCategory, "Skins.Human", "GasEnabled"));
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel), (BaseSetting) human.HookEnabled, UIManager.GetLocale(parent1.LocaleCategory, "Skins.Human", "HookEnabled"));
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateHorizontalDivider(this.DoublePanelRight);
      settingsSkinsPanel.CreateSkinStringSettings(this.DoublePanelLeft, this.DoublePanelRight, 200f, 200f, 9);
    }
  }
}
