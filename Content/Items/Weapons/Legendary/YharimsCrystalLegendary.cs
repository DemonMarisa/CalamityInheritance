using System.Collections.Generic;
using System.Drawing;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class YharimsCrystalLegendary : CIMagic, ILocalizedModType
    {
        #region 颜色
        //默认颜色
        private static Color DefualtColor => Color.White;
        //开发者颜色: TrueScarlet, 近似深红
        private static Color TrueScarletColor => new(228, 1, 10);  
        //开发者颜色: DemonMarisa, 近似金黄
        private static Color DemonMarisaColor => new(255, 165, 0);
        //Tester颜色：Shizuku, 银白
        private static Color ShizukuColorSilver => new(248, 248, 255);
        //Tester颜色：KunojiIchika，近似纯黑
        private static Color IchikaColorBlack => new (79, 79, 79);
        //Supporter颜色: Plantare, 粉红
        private static Color PlantareColorPink => Color.HotPink;
        //彩蛋颜色: Tristan, 皇家蓝
        private static Color TristanColorRoyalBlue => Color.RoyalBlue;
        #endregion
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Magic.YharimsCrystalLegendary";
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item13;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 0f;
            Item.shoot = ModContent.ProjectileType<YharimsCrystalPrismLegendary>();
            Item.shootSpeed = 30f;

            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override bool CanReforge()
        {
            return base.CanReforge();
        }

        public override bool CanUseItem(Player player)
        {
            bool checkPrism = player.ownedProjectileCounts[Item.shoot] <= 0;
            if (player.CIMod().YharimsKilledScal)
            {
                Item.mana = 200;
                return checkPrism;
            }
            else
            {
                Item.mana = 10;
                return checkPrism;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var up = p.CIMod();
            string tExo = up.YharimsKilledExo ? Language.GetTextValue($"{TextRoute}.ExoTint") : Language.GetTextValue($"{TextRoute}.NoExoTint");
            string tScal = up.YharimsKilledScal ? Language.GetTextValue($"{TextRoute}.ScalTint") : Language.GetTextValue($"{TextRoute}.NoScalTint");
            tooltips.FindAndReplace("[EXO]",tExo);
            tooltips.FindAndReplace("[SCAL]",tScal);
            
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            Player p = Main.LocalPlayer;
            var up = p.CIMod();
            //击破终灾时会尽可能地把手感……或者说别的。接近971。
            if (up.YharimsKilledScal)
                damage.Base = 50;
            else damage.Base = 300;
        }
    }
}
