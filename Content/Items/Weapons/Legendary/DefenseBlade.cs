using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Core;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class DefenseBlade: CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee";
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Melee.DefenseBlade";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = Item.width = 72;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = 135;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 10;
            Item.scale *= 1.25f;
            Item.useTurn = true;
            Item.knockBack = 4.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<GolemPurple>() : ModContent.RarityType<MaliceChallengeDrop>();
            Item.value = CIShopValue.RarityMaliceDrop;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Melee;
                //谁家好人射弹剑刀刃不造成伤害啊?
                Item.noMelee = false;
                Item.UseSound = SoundID.Item73;
                Item.shoot = ModContent.ProjectileType<DefenseBeam>();
            }
            else
            {
                Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
                Item.noMelee = false;
                Item.UseSound = SoundID.Item1;
                Item.shoot = ProjectileID.None;
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
             if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GoldCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 53));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.DefendTier1? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.DefendTier2? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.DefendTier3? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //以下，用于比较复杂的计算
            float getDmg = LegendaryDamage();
            int boostPercent = (int)(getDmg * 100);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= 1f + LegendaryDamage();
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) => Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DefenseBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) => Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DefenseBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
        public static float LegendaryDamage()
        {
            float damageBuff = 0f;
            damageBuff += NPC.downedAncientCultist ? 0.2f : 0f;
            damageBuff += NPC.downedMoonlord ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedProvidence ? 0.4f : 0f;
            damageBuff += DownedBossSystem.downedPolterghast ? 0.8f : 0f;
            damageBuff += DownedBossSystem.downedBoomerDuke ? 0.8f : 0f;
            damageBuff += DownedBossSystem.downedDoG ? 1.0f : 0f;
            damageBuff += DownedBossSystem.downedYharon ? 1.2f : 0f;
            damageBuff += DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas? 1f : 0f;
            return damageBuff;
        }

        
    }
}