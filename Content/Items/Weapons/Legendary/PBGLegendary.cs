using CalamityMod.Items.Weapons.Rogue;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Rarity;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class PBGLegendary: RogueWeapon, ILocalizedModType
    {
        public static readonly SoundStyle StealthSound = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowSingle") { PitchVariance = 0.4f };
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Rogue.PBGLegendary";
        public int BaseDamage = 70;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type]= true;
        }
        public int baseDamage = 50;
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 58;
            Item.damage = baseDamage;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
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
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<PBGLime>() : ModContent.RarityType<MaliceChallengeDrop>();
            Item.value = CIShopValue.RarityMaliceDrop;
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
            string t1 = mp.PBGTier1 ? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.PBGTier2 ? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.PBGTier3 ? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.GetWeaponLocal}.EmpoweredTooltip.Generic");
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
            /*
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(baseDamage + LegendaryDamage() + Generic.GenericLegendBuffInt()) / (float)baseDamage);
            damage *= Buff;
            */
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
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 5 : 0;    // 55
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 5 : 0; // 60
            dmgBuff += DownedBossSystem.downedRavager ? 5 : 0;         // 65
            dmgBuff += Condition.DownedCultist.IsMet() ? 10 : 0;       // 75
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 10 : 0;     // 85
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 25 : 0;      // 110
            dmgBuff += DownedBossSystem.downedGuardians ? 10 : 0;      // 120
            dmgBuff += DownedBossSystem.downedDragonfolly ? 10 : 0;    // 130
            dmgBuff += DownedBossSystem.downedProvidence ? 85 : 0;    // 225
            dmgBuff += DownedBossSystem.downedSignus ? 15 : 0;         // 240
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 15 : 0;  // 255
            dmgBuff += DownedBossSystem.downedStormWeaver ? 15 : 0;    // 270
            dmgBuff += DownedBossSystem.downedPolterghast ? 155 : 0;   // 425
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 25 : 0;     // 450
            dmgBuff += DownedBossSystem.downedDoG ? 350 : 0;           // 800

            dmgBuff += DownedBossSystem.downedYharon ? 1000 : 0;       // 1800
            dmgBuff += DownedBossSystem.downedCalamitas ? 100 : 0;     // 1900
            dmgBuff += DownedBossSystem.downedExoMechs ? 100 : 0;      // 2000
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 2000 : 0;

            return dmgBuff;
        }
    }
}