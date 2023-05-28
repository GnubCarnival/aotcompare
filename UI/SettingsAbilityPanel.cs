// Decompiled with JetBrains decompiler
// Type: UI.SettingsAbilityPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
  internal class SettingsAbilityPanel : SettingsCategoryPanel
  {
    protected Text _pointsLeftLabel;

    public override void Setup(BasePanel parent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SettingsAbilityPanel.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10 = new SettingsAbilityPanel.\u003C\u003Ec__DisplayClass1_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass10.\u003C\u003E4__this = this;
      base.Setup(parent);
      SettingsPopup settingsPopup = (SettingsPopup) parent;
      string localeCategory = settingsPopup.LocaleCategory;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass10.settings = SettingsManager.AbilitySettings;
      ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: this.ThemePanel);
      // ISSUE: reference to a compiler-generated field
      ElementFactory.CreateColorSetting(this.DoublePanelRight, style, (BaseSetting) cDisplayClass10.settings.BombColor, "Bomb color", settingsPopup.ColorPickPopup);
      // ISSUE: reference to a compiler-generated field
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) cDisplayClass10.settings.ShowBombColors, "Show bomb colors");
      // ISSUE: reference to a compiler-generated field
      ElementFactory.CreateToggleSetting(this.DoublePanelRight, style, (BaseSetting) cDisplayClass10.settings.UseOldEffect, "Use old effect");
      this._pointsLeftLabel = ElementFactory.CreateDefaultLabel(this.DoublePanelLeft, style, "Points Left").GetComponent<Text>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ElementFactory.CreateIncrementSetting(this.DoublePanelLeft, style, (BaseSetting) cDisplayClass10.settings.BombRadius, "Bomb radius (0-10)", onValueChanged: new UnityAction((object) cDisplayClass10, __methodptr(\u003CSetup\u003Eb__0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ElementFactory.CreateIncrementSetting(this.DoublePanelLeft, style, (BaseSetting) cDisplayClass10.settings.BombRange, "Bomb range (0-3)", onValueChanged: new UnityAction((object) cDisplayClass10, __methodptr(\u003CSetup\u003Eb__1)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ElementFactory.CreateIncrementSetting(this.DoublePanelLeft, style, (BaseSetting) cDisplayClass10.settings.BombSpeed, "Bomb speed (0-10)", onValueChanged: new UnityAction((object) cDisplayClass10, __methodptr(\u003CSetup\u003Eb__2)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ElementFactory.CreateIncrementSetting(this.DoublePanelLeft, style, (BaseSetting) cDisplayClass10.settings.BombCooldown, "Bomb cooldown (0-6)", onValueChanged: new UnityAction((object) cDisplayClass10, __methodptr(\u003CSetup\u003Eb__3)));
      // ISSUE: reference to a compiler-generated field
      this.OnStatChanged(cDisplayClass10.settings.BombRadius);
    }

    protected void OnStatChanged(IntSetting setting)
    {
      int num1 = 16;
      AbilitySettings abilitySettings = SettingsManager.AbilitySettings;
      int num2 = abilitySettings.BombRadius.Value + abilitySettings.BombRange.Value + abilitySettings.BombSpeed.Value + abilitySettings.BombCooldown.Value;
      if (num2 > num1)
      {
        int num3 = num2 - num1;
        setting.Value -= num3;
        if (setting.Value < 0)
        {
          abilitySettings.BombRadius.SetDefault();
          abilitySettings.BombRange.SetDefault();
          abilitySettings.BombSpeed.SetDefault();
          abilitySettings.BombCooldown.SetDefault();
        }
        this.SyncSettingElements();
      }
      int num4 = abilitySettings.BombRadius.Value + abilitySettings.BombRange.Value + abilitySettings.BombSpeed.Value + abilitySettings.BombCooldown.Value;
      this._pointsLeftLabel.text = "Points left: " + Math.Max(0, num1 - num4).ToString();
    }
  }
}
