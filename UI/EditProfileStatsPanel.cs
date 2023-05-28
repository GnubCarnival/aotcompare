// Decompiled with JetBrains decompiler
// Type: UI.EditProfileStatsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  internal class EditProfileStatsPanel : BasePanel
  {
    protected override float Width => 720f;

    protected override float Height => 520f;

    protected override bool DoublePanel => true;

    protected override bool DoublePanelDivider => true;

    protected override bool ScrollBar => true;

    protected override float VerticalSpacing => 10f;

    public override void Setup(BasePanel parent = null)
    {
      base.Setup(parent);
      GameStatContainer gameStat = GameProgressManager.GameProgress.GameStat;
      AchievmentCount achievmentCount = GameProgressManager.GameProgress.Achievment.GetAchievmentCount();
      ElementStyle style1 = new ElementStyle(titleWidth: 100f, themePanel: this.ThemePanel);
      this.CreateTitleLabel(this.DoublePanelLeft, style1, "General");
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Level", gameStat.Level.Value.ToString());
      Transform doublePanelLeft = this.DoublePanelLeft;
      ElementStyle style2 = style1;
      int expToNext = gameStat.Exp.Value;
      string str1 = expToNext.ToString();
      expToNext = GameProgressManager.GetExpToNext();
      string str2 = expToNext.ToString();
      string str3 = str1 + "/" + str2;
      this.CreateStatLabel(doublePanelLeft, style2, "Exp", str3);
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Hours played", (gameStat.PlayTime.Value / 3600f).ToString("0.00"));
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Highest speed", ((int) gameStat.HighestSpeed.Value).ToString());
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateTitleLabel(this.DoublePanelLeft, style1, "Achievments");
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Bronze", achievmentCount.FinishedBronze.ToString() + "/" + achievmentCount.TotalBronze.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Silver", achievmentCount.FinishedSilver.ToString() + "/" + achievmentCount.TotalSilver.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Gold", achievmentCount.FinishedGold.ToString() + "/" + achievmentCount.TotalGold.ToString());
      this.CreateHorizontalDivider(this.DoublePanelLeft);
      this.CreateTitleLabel(this.DoublePanelLeft, style1, "Damage");
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Highest overall", gameStat.DamageHighestOverall.Value.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Highest blade", gameStat.DamageHighestBlade.Value.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Highest gun", gameStat.DamageHighestGun.Value.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Total overall", gameStat.DamageTotalOverall.Value.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Total blade", gameStat.DamageTotalBlade.Value.ToString());
      this.CreateStatLabel(this.DoublePanelLeft, style1, "Total gun", gameStat.DamageTotalGun.Value.ToString());
      this.CreateTitleLabel(this.DoublePanelRight, style1, "Titans Killed");
      this.CreateStatLabel(this.DoublePanelRight, style1, "Total", gameStat.TitansKilledTotal.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Blade", gameStat.TitansKilledBlade.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Gun", gameStat.TitansKilledGun.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Thunder spear", gameStat.TitansKilledThunderSpear.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Other", gameStat.TitansKilledOther.Value.ToString());
      this.CreateHorizontalDivider(this.DoublePanelRight);
      this.CreateTitleLabel(this.DoublePanelRight, style1, "Humans Killed");
      this.CreateStatLabel(this.DoublePanelRight, style1, "Total", gameStat.HumansKilledTotal.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Blade", gameStat.HumansKilledBlade.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Gun", gameStat.HumansKilledGun.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Thunder spear", gameStat.HumansKilledThunderSpear.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Titan", gameStat.HumansKilledTitan.Value.ToString());
      this.CreateStatLabel(this.DoublePanelRight, style1, "Other", gameStat.TitansKilledOther.Value.ToString());
    }

    protected void CreateStatLabel(
      Transform panel,
      ElementStyle style,
      string title,
      string value)
    {
      ElementFactory.CreateDefaultLabel(panel, style, title + ": " + value, alignment: (TextAnchor) 3).GetComponent<Text>();
    }

    protected void CreateTitleLabel(Transform panel, ElementStyle style, string title) => ElementFactory.CreateDefaultLabel(panel, style, title, (FontStyle) 1, (TextAnchor) 3).GetComponent<Text>();
  }
}
