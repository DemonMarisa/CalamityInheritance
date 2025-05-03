using System;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ArkoftheAncients : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.damage = 92;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.25f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 50;
            Item.value = CIShopValue.RarityPriceLightPurple;
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.EonBeam>();
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Utils.SelectRandom(Main.rand, new int[]
            {
                ModContent.ProjectileType<Projectiles.Melee.EonBeam>(),
                ProjectileID.EnchantedBeam
            });

            int beam = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, Main.myPlayer);
            if (Main.projectile[beam].type == ModContent.ProjectileType<Projectiles.Melee.EonBeam>())
                Main.projectile[beam].penetrate = 2;

            if (Main.projectile[beam].type == ProjectileID.EnchantedBeam)
                Main.projectile[beam].extraUpdates = 1;

            float num72 = Main.rand.Next(18, 25);
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;

            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
            }

            float num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
            if (float.IsNaN(num78) && float.IsNaN(num79) || num78 == 0f && num79 == 0f)
            {
                num78 = player.direction;
                num79 = 0f;
                num80 = num72;
            }
            else
            {
                num80 = num72 / num80;
            }

            int num107 = 2;
            for (int num108 = 0; num108 < num107; num108++)
            {
                vector2 = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(201) * -player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                vector2.Y -= 100 * num108;

                num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
                num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (num79 < 0f)
                {
                    num79 *= -1f;
                }
                if (num79 < 20f)
                {
                    num79 = 20f;
                }

                num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
                num80 = num72 / num80;
                num78 *= num80;
                num79 *= num80;

                float speedX4 = num78 + Main.rand.Next(-120, 121) * 0.02f;
                float speedY5 = num79 + Main.rand.Next(-120, 121) * 0.02f;

                int proj = Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ProjectileID.HallowStar, damage / 3, knockback, player.whoAmI, 0f, Main.rand.Next(5));
            }

            return false;
        }
        
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dustType = 15;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        dustType = 15;
                        break;
                    case 1:
                        dustType = 57;
                        break;
                    case 2:
                        dustType = 58;
                        break;
                    default:
                        break;
                }
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, player.direction * 2, 0f, 150, default, 1.3f);
                Main.dust[dust].velocity *= 0.2f;
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EssenceofSunlight>(3).
                AddIngredient<EssenceofEleum>(3).
                AddIngredient(ItemID.Starfury).
                AddIngredient(ItemID.EnchantedSword).
                AddIngredient(ItemID.Excalibur).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient<EssenceofSunlight>(3).
                AddIngredient<EssenceofEleum>(3).
                AddIngredient(ItemID.Starfury).
                AddRecipeGroup(CIRecipeGroup.Arkhalis).
                AddIngredient(ItemID.Excalibur).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}