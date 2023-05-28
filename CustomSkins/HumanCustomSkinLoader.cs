// Decompiled with JetBrains decompiler
// Type: CustomSkins.HumanCustomSkinLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xft;

namespace CustomSkins
{
  internal class HumanCustomSkinLoader : BaseCustomSkinLoader
  {
    private int _horseViewId;
    public HookCustomSkinPart HookL;
    public HookCustomSkinPart HookR;
    public float HookLTiling = 1f;
    public float HookRTiling = 1f;

    protected override string RendererIdPrefix => "human";

    public override IEnumerator LoadSkinsFromRPC(object[] data)
    {
      HumanCustomSkinLoader customSkinLoader = this;
      customSkinLoader._horseViewId = (int) data[0];
      string[] skinUrls = ((string) data[1]).Split(',');
      foreach (int partId in customSkinLoader.GetCustomSkinPartIds(typeof (HumanCustomSkinPartId)))
      {
        if ((partId != 0 || customSkinLoader._horseViewId >= 0) && (partId != 12 || customSkinLoader._owner.GetComponent<HERO>().IsMine()) && (partId != 10 || SettingsManager.CustomSkinSettings.Human.GasEnabled.Value))
        {
          if (partId == 16 && skinUrls.Length > partId)
            float.TryParse(skinUrls[partId], out customSkinLoader.HookLTiling);
          else if (partId == 18 && skinUrls.Length > partId)
            float.TryParse(skinUrls[partId], out customSkinLoader.HookRTiling);
          else if ((partId != 15 || SettingsManager.CustomSkinSettings.Human.HookEnabled.Value) && (partId != 17 || SettingsManager.CustomSkinSettings.Human.HookEnabled.Value))
          {
            BaseCustomSkinPart part = customSkinLoader.GetCustomSkinPart(partId);
            if (skinUrls.Length > partId && !part.LoadCache(skinUrls[partId]))
              yield return (object) customSkinLoader.StartCoroutine(part.LoadSkin(skinUrls[partId]));
            switch (partId)
            {
              case 15:
                customSkinLoader.HookL = (HookCustomSkinPart) part;
                break;
              case 17:
                customSkinLoader.HookR = (HookCustomSkinPart) part;
                break;
            }
            part = (BaseCustomSkinPart) null;
          }
        }
      }
      FengGameManagerMKII.instance.unloadAssets();
    }

    protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
    {
      HERO component = this._owner.GetComponent<HERO>();
      List<Renderer> renderers = new List<Renderer>();
      switch (partId)
      {
        case 0:
          this.AddRenderersMatchingName(renderers, ((Component) PhotonView.Find(this._horseViewId)).gameObject, "HORSE");
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 1:
          this.AddRendererIfExists(renderers, component.setup.part_hair);
          this.AddRendererIfExists(renderers, component.setup.part_hair_1);
          return (BaseCustomSkinPart) new HumanHairCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000, component.setup.myCostume.hairInfo);
        case 2:
          this.AddRendererIfExists(renderers, component.setup.part_eye);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000, new Vector2?(new Vector2(8f, 8f)));
        case 3:
          this.AddRendererIfExists(renderers, component.setup.part_glass);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000, new Vector2?(new Vector2(8f, 8f)));
        case 4:
          this.AddRendererIfExists(renderers, component.setup.part_face);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000, new Vector2?(new Vector2(8f, 8f)));
        case 5:
          this.AddRendererIfExists(renderers, component.setup.part_hand_l);
          this.AddRendererIfExists(renderers, component.setup.part_hand_r);
          this.AddRendererIfExists(renderers, component.setup.part_head);
          this.AddRendererIfExists(renderers, component.setup.part_chest);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 6:
          this.AddRendererIfExists(renderers, component.setup.part_arm_l);
          this.AddRendererIfExists(renderers, component.setup.part_arm_r);
          this.AddRendererIfExists(renderers, component.setup.part_leg);
          this.AddRendererIfExists(renderers, component.setup.part_chest_2);
          this.AddRendererIfExists(renderers, component.setup.part_chest_3);
          this.AddRendererIfExists(renderers, component.setup.part_upper_body);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 2000000, useTransparentMaterial: true);
        case 7:
          this.AddRendererIfExists(renderers, component.setup.part_cape);
          this.AddRendererIfExists(renderers, component.setup.part_brand_1);
          this.AddRendererIfExists(renderers, component.setup.part_brand_2);
          this.AddRendererIfExists(renderers, component.setup.part_brand_3);
          this.AddRendererIfExists(renderers, component.setup.part_brand_4);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 8:
          this.AddRendererIfExists(renderers, component.setup.part_3dmg);
          this.AddRendererIfExists(renderers, component.setup.part_3dmg_belt);
          this.AddRendererIfExists(renderers, component.setup.part_3dmg_gas_l);
          this.AddRendererIfExists(renderers, component.setup.part_blade_l);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 9:
          this.AddRendererIfExists(renderers, component.setup.part_3dmg_gas_r);
          this.AddRendererIfExists(renderers, component.setup.part_blade_r);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 10:
          this.AddRendererIfExists(renderers, ((Component) ((Component) component).transform.Find("3dmg_smoke")).gameObject);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 11:
          if (Object.op_Inequality((Object) component.setup.part_chest_1, (Object) null) && ((Object) component.setup.part_chest_1).name.Contains("character_cap"))
            this.AddRendererIfExists(renderers, component.setup.part_chest_1);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 500000);
        case 12:
          return (BaseCustomSkinPart) new WeaponTrailCustomSkinPart((BaseCustomSkinLoader) this, new List<XWeaponTrail>()
          {
            component.leftbladetrail,
            component.leftbladetrail2,
            component.rightbladetrail,
            component.rightbladetrail2
          }, this.GetRendererId(partId), 500000);
        case 13:
          if (Object.op_Inequality((Object) component.ThunderSpearLModel, (Object) null))
            this.AddRendererIfExists(renderers, component.ThunderSpearLModel);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 14:
          if (Object.op_Inequality((Object) component.ThunderSpearRModel, (Object) null))
            this.AddRendererIfExists(renderers, component.ThunderSpearRModel);
          return new BaseCustomSkinPart((BaseCustomSkinLoader) this, renderers, this.GetRendererId(partId), 1000000);
        case 15:
        case 17:
          return (BaseCustomSkinPart) new HookCustomSkinPart((BaseCustomSkinLoader) this, this.GetRendererId(partId), 500000);
        default:
          return (BaseCustomSkinPart) null;
      }
    }
  }
}
