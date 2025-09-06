using System.Collections.Generic;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public class CIVaniilaTooltipsTweak : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public static Player LocalPlayer => Main.LocalPlayer;
        public override bool InstancePerEntity => true;
        //灾厄是硬编码，考虑到和汉化补丁的问题，这里所有的写法都是全部干掉原本的tooltip重写
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            if (!CIServerConfig.Instance.VanillaUnnerf)
                return;
            string General = CIFunction.GetTextValue("Content.Items.VanillaTweaks.");
            switch (item.type)
            {
                case ItemID.MechanicalGlove:
                    tooltips.FuckThisTooltipAndReplace(General + "MechanicalGlove", "10%", "12%");
                    break;
                case ItemID.BerserkerGlove:
                    tooltips.FuckThisTooltipAndReplace(General + "BerserkerGlove", "10%", "12%");
                    break;
                case ItemID.FeralClaws:
                    tooltips.FuckThisTooltipAndReplace(General + "FeralClaws", "12%");
                    break;
                case ItemID.FireGauntlet:
                    tooltips.FuckThisTooltipAndReplace(General + "FireGauntlet", "10%", "14%");
                    break;
                //天界壳等是没有具体数值的
                case ItemID.CelestialStone:
                    tooltips.FuckThisTooltipAndReplace(General + "CelestialStone");
                    break;
                case ItemID.CelestialShell:
                    tooltips.FuckThisTooltipAndReplace(General + "CelestialShell");
                    break;
                case ItemID.MoonCharm:
                    tooltips.FuckThisTooltipAndReplace(General + "MoonCharm");
                    break;
                case ItemID.MoonStone:
                    tooltips.FuckThisTooltipAndReplace(General + "MoonStone");
                    break;
                case ItemID.SunStone:
                    tooltips.FuckThisTooltipAndReplace(General + "SunStone");
                    break;
                case ItemID.SniperScope:
                    tooltips.FuckThisTooltipAndReplace(General + "SniperScope", "10%");
                    break;
                case ItemID.WormScarf:
                    tooltips.FuckThisTooltipAndReplace(General + "WormScarf", "17%");
                    break;
                case ItemID.EmpressFlightBooster:
                    tooltips.FuckThisTooltipAndReplace(General + "EmpressFlightBooster");
                    break;
                //下面这些都是盔甲部件了。
                case ItemID.VortexHelmet:
                    tooltips.FuckThisTooltipAndReplace(General + "VortexHelmet", "16%", "7%");
                    break;
                case ItemID.SolarFlareHelmet:
                    tooltips.FuckThisTooltipAndReplace(General + "SolarFlareHelmet", "26%");
                    break;
                case ItemID.MeteorHelmet:
                case ItemID.MeteorSuit:
                case ItemID.MeteorLeggings:
                    tooltips.FuckThisTooltipAndReplace(General + "MeteorPiece", "9%");
                    break;
                case ItemID.JungleHat:
                    tooltips.FuckThisTooltipAndReplace(General + "JungleHat", "40", "6%");
                    break;
                case ItemID.JunglePants:
                    tooltips.FuckThisTooltipAndReplace(General + "JunglePants", "6%");
                    break;
                case ItemID.AdamantiteHelmet:
                    TooltipLine findSetBonus = tooltips.Find(line => line.Name == "SetBouns");
                    if (findSetBonus != null)
                    {
                        string baseText = General + "AdamantiteHelmet";
                        string formateText = baseText.ToLangValue().ToFormatValue("20%");
                        findSetBonus.Text = "\n" + formateText;
                    }
                    break;
                case ItemID.MagicHat:
                    tooltips.FuckThisTooltipAndReplace(General + "MagicHat.Hat", "5%");
                    TooltipLine findSetBonusMagicHat = tooltips.Find(line => line.Name == "SetBouns");
                    if (findSetBonusMagicHat != null)
                    {
                        string baseText = General + "MagicHat.Set";
                        string formateText = baseText.ToLangValue().ToFormatValue("20");
                        findSetBonusMagicHat.Text = "\n" + formateText;
                    }
                    break;
                case ItemID.Gi:
                    tooltips.FuckThisTooltipAndReplace(General + "Gi", "10%");
                    break;
                case ItemID.AmethystRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "AmethystRobe", "20", "5%");
                    break;
                case ItemID.TopazRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "TopazRobe", "40", "7%");
                    break;
                case ItemID.SapphireRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "SapphireRobe", "40", "9%");
                    break;
                case ItemID.EmeraldRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "EmeraldRobe", "60", "11%");
                    break;
                case ItemID.RubyRobe:
                case ItemID.AmberRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "RubyRobe", "60", "13%");
                    break;
                case ItemID.DiamondRobe:
                    tooltips.FuckThisTooltipAndReplace(General + "DiamondRobe", "80", "15%");
                    break;
            }
            //机械手套：可堆叠，10伤害和12速
            
        }
    }
}