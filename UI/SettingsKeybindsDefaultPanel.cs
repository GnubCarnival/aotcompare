// Decompiled with JetBrains decompiler
// Type: UI.SettingsKeybindsDefaultPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;

namespace UI
{
  internal class SettingsKeybindsDefaultPanel : SettingsCategoryPanel
  {
    protected override bool ScrollBar => true;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      SettingsKeybindsPanel settingsKeybindsPanel = (SettingsKeybindsPanel) parent;
      SettingsPopup parent1 = (SettingsPopup) settingsKeybindsPanel.Parent;
      settingsKeybindsPanel.CreateGategoryDropdown(this.DoublePanelLeft);
      string localeCategory = parent1.LocaleCategory;
      ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: this.ThemePanel);
      string key = settingsKeybindsPanel.GetCurrentCategoryName().Replace(" ", "");
      this.CreateKeybindSettings((BaseSettingsContainer) SettingsManager.InputSettings.Settings[(object) key], parent1.KeybindPopup, localeCategory, "Keybinds." + key, style);
      if (!(key == "Human"))
        return;
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.InputSettings.Human.DashDoubleTap, UIManager.GetLocale(localeCategory, "Keybinds.Human", "DashDoubleTap"));
      ElementFactory.CreateSliderSetting(this.DoublePanelRight, style, (BaseSetting) SettingsManager.InputSettings.Human.ReelOutScrollSmoothing, UIManager.GetLocale(localeCategory, "Keybinds.Human", "ReelOutScrollSmoothing"), UIManager.GetLocale(localeCategory, "Keybinds.Human", "ReelOutScrollSmoothingTooltip"), 130f);
    }

    private void CreateKeybindSettings(
      BaseSettingsContainer container,
      KeybindPopup popup,
      string cat,
      string sub,
      ElementStyle style)
    {
      int num = 0;
      foreach (DictionaryEntry setting1 in container.Settings)
      {
        BaseSetting setting2 = (BaseSetting) setting1.Value;
        string key = (string) setting1.Key;
        if (setting2.GetType() == typeof (KeybindSetting))
        {
          ElementFactory.CreateKeybindSetting(num < container.Settings.Count / 2 ? this.DoublePanelLeft : this.DoublePanelRight, style, setting2, UIManager.GetLocale(cat, sub, key), popup);
          ++num;
        }
      }
    }
  }
}
