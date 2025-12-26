using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class PBGLegendary: LegendaryWeaponClass
    {
        public static readonly SoundStyle StealthSound = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowSingle") { PitchVariance = 0.4f };
        public override ClassType WeaponDamageClass => ClassType.Rogue;
        public override Color DrawColor => new(0, 255, 127);
        public override int SetRarityColor => ModContent.RarityType<PBGLime>();
        public int BaseDamage = 30;
        public int baseDamage = 30;
        public override void ExSD()
        {
            Item.width = 26;
            Item.height = 58;
            Item.damage = baseDamage;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.25f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PBGLegendaryProj>();
            Item.shootSpeed = 10f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation = 8;
                Item.UseSound = SoundID.Item109;
            }
            else
            {
                Item.useTime = Item.useAnimation = 4;
                Item.UseSound = CISoundID.SoundWeaponSwing;
            }
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.PBGTier1 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierOne") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.PBGTier2 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierTwo") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.PBGTier3 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierThree") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.WeaponTextPath}EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            float getdmg = LegendaryDamage() + Generic.GenericLegendBuff();
            int boostPercent = (int)(getdmg);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(baseDamage + LegendaryDamage() + Generic.GenericLegendBuffInt(0)) / (float)baseDamage);
            damage *= Buff;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.Calamity().StealthStrikeAvailable())
            {
                if (player.altFunctionUse == 2)
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PBGLegendaryBolt>(), (int)(damage * 1.75f), knockback, player.whoAmI);
                else
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PBGLegendaryProj>(), (int)(damage * 1.75f), knockback, player.whoAmI);
            }
            else
            {
                int pNum = player.CIMod().PBGTier1 ? 6 : 5;
                int dmg =  (int)(damage * 0.7f);
                for (int j = 1; j <= pNum; j++)
                {
                    Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedBy(j / 11f + 0.2f) * j / 5f;
                    int p = Projectile.NewProjectile(source, position + velocity * 0.1f, velocity + spread, ModContent.ProjectileType<PBGLegendaryBeam>(), dmg, knockback, player.whoAmI, 0f, 0f, -1f);
                    Main.projectile[p].Calamity().stealthStrike = true;
                }
            }
            return false;
        }
        public static int LegendaryDamage()
        {
            int dmgBuff = 0;
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 5 : 0;    // 35
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 5 : 0; // 40
            dmgBuff += DownedBossSystem.downedRavager ? 5 : 0;         // 45
            dmgBuff += Condition.DownedCultist.IsMet() ? 10 : 0;       // 55
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 5 : 0;      // 60
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 10 : 0;      // 70
            dmgBuff += DownedBossSystem.downedGuardians ? 10 : 0;      // 80
            dmgBuff += DownedBossSystem.downedDragonfolly ? 10 : 0;    // 90
            dmgBuff += DownedBossSystem.downedProvidence ? 50 : 0;     // 140
            dmgBuff += DownedBossSystem.downedSignus ? 5 : 0;          // 145
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 5 : 0;   // 150
            dmgBuff += DownedBossSystem.downedStormWeaver ? 5 : 0;     // 155
            dmgBuff += DownedBossSystem.downedPolterghast ? 50 : 0;    // 205
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 25 : 0;     // 230
            dmgBuff += DownedBossSystem.downedDoG ? 80 : 0;            // 310
            dmgBuff += DownedBossSystem.downedYharon ? 600 : 0;        // 920
            dmgBuff += DownedBossSystem.downedCalamitas ? 100 : 0;     // 1020
            dmgBuff += DownedBossSystem.downedExoMechs ? 80 : 0;       // 1100
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 3100 : 0;

            return dmgBuff;
        }
    }
}