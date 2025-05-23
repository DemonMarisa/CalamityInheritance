﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ElementalExcalibur : CIMelee, ILocalizedModType
    {
        
        private static int BaseDamage = 4000;
        private int BeamType = 0;
        private const int alpha = 50;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 8f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.width = 112;
            Item.height = 112;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.shoot = ModContent.ProjectileType<ElementalExcaliburBeam>();
            Item.shootSpeed = 6f;
            Item.rare = ModContent.RarityType<DonatorPink>();
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 10;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, BeamType, 0f);

            BeamType++;
            if (BeamType > 11)
                BeamType = 0;

            return false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0f;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<ElementalExcaliburBeam>();
                Item.shootSpeed = 12f;
            }

            return base.CanUseItem(player);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                Color color = new Color(255, 0, 0, alpha);
                switch (BeamType)
                {
                    case 0: // Red
                        break;
                    case 1: // Orange
                        color = new Color(255, 128, 0, alpha);
                        break;
                    case 2: // Yellow
                        color = new Color(255, 255, 0, alpha);
                        break;
                    case 3: // Lime
                        color = new Color(128, 255, 0, alpha);
                        break;
                    case 4: // Green
                        color = new Color(0, 255, 0, alpha);
                        break;
                    case 5: // Turquoise
                        color = new Color(0, 255, 128, alpha);
                        break;
                    case 6: // Cyan
                        color = new Color(0, 255, 255, alpha);
                        break;
                    case 7: // Light Blue
                        color = new Color(0, 128, 255, alpha);
                        break;
                    case 8: // Blue
                        color = new Color(0, 0, 255, alpha);
                        break;
                    case 9: // Purple
                        color = new Color(128, 0, 255, alpha);
                        break;
                    case 10: // Fuschia
                        color = new Color(255, 0, 255, alpha);
                        break;
                    case 11: // Hot Pink
                        color = new Color(255, 0, 128, alpha);
                        break;
                    default:
                        break;
                }

                Dust dust24 = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RainbowMk2, 0f, 0f, alpha, color, 1.2f)];
                dust24.noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 600);

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 10;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 600);

            if (player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 10;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Orderbringer>().
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient<ShadowspecBar>(5).
                AddIngredient<AscendantSpiritEssence>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
