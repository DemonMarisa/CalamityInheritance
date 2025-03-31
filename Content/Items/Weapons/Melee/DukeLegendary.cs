using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class DukeLegendary: CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 1.33f;
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 102;
            Item.damage = 75;
            Item.scale *= 1.5f;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 15;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shootSpeed = 9f;

            Item.shoot = ModContent.ProjectileType<Razorwind>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
            }
            else
            {
                Item.noMelee = false;
            }

            return base.UseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            if (mp.DukeTier1)
            {
                string t1 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.DukeLegendary.TierOne");
                tooltips.Add(new TooltipLine(Mod, "TIERONE", t1));
            }
            if (mp.DukeTier2)
            {
                string t2 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.DukeLegendary.TierTwo");
                tooltips.Add(new TooltipLine(Mod, "TIERTWO", t2));
            }
            if (mp.DukeTier1)
            {
                string t3 = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.DukeLegendary.TierThree");
                tooltips.Add(new TooltipLine(Mod, "TIERTHREE", t3));
            }
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

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                damage = player.CIMod().DukeTier2? (int)(damage * 0.9) : (int)(damage * 0.6);
                type = ModContent.ProjectileType<DukeLegendaryRazor>();
            }

            else
                type = ProjectileID.None;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255));
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<DukeLegendaryBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<DukeLegendaryBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void UseAnimation(Player player)
        {
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;

            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item84;
            }
        }
        //8个Boss
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
            damageBuff += (DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas)? 1f : 0f;
            return damageBuff;
        }
    }
}
