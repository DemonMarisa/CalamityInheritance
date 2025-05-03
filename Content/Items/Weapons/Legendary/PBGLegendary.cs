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
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 58;
            Item.damage = 70;
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
            int boostPercent = (int)(getdmg * 100);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= LegendaryDamage() + Generic.GenericLegendBuff();
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
                int dmg =  player.CIMod().PBGTier1 ? (int)(damage * 0.8f) : (int)(damage * 0.7f);
                for (int j = 1; j <= pNum; j++)
                {
                    Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedBy(j / 11f + 0.2f) * j / 5f;
                    int p = Projectile.NewProjectile(source, position + velocity * 0.1f, velocity + spread, ModContent.ProjectileType<PBGLegendaryBeam>(), dmg, knockback, player.whoAmI, 0f, 0f, -1f);
                    Main.projectile[p].Calamity().stealthStrike = true;
                }
            }
            return false;
        }
        //总共8个tier, 终灾与exo本身属于同一个tier
        public static float LegendaryDamage()
        {
            float damageBuff = 0f;
            damageBuff += NPC.downedAncientCultist ? 0.2f : 0f;
            damageBuff += NPC.downedMoonlord ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedProvidence ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedPolterghast ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedBoomerDuke ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedDoG ? 0.6f : 0f;
            damageBuff += DownedBossSystem.downedYharon ? 0.8f : 0f;
            damageBuff += DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas? 0.8f : 0f;
            //恭喜击败至尊灾厄眼，所以。500%?
            damageBuff += CIDownedBossSystem.DownedLegacyScal ? 5f : 0f;
            return 1f + damageBuff;
        }
    }
}