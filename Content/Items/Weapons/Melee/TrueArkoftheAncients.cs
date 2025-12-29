using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TrueArkoftheAncients : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.damage = 113;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 60;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<EonBeam>();
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*
            type = Utils.SelectRandom(Main.rand, new int[]
            {
                ProjectileType<EonBeam>(),
                ProjectileType<EonBeamV2>()
            });
            */


            Projectile beam = Projectile.NewProjectileDirect(source, position, velocity * 0.75f, type, (int)(damage * 0.75), knockback, Main.myPlayer);
            if (beam.active)
            {
                beam.localNPCHitCooldown = 14;
                beam.penetrate = 2;
                beam.ai[1] = Main.rand.Next(1, 3);
            }

            int i = Main.myPlayer;
            float num72 = Main.rand.Next(18, 27);
            float adjustedKnockback = player.GetWeaponKnockback(Item, knockback);
            player.itemTime = Item.useTime;

            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 value = Vector2.UnitX.RotatedBy(player.fullRotation);
            Vector2 vector3 = Main.MouseWorld - vector2;

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


            for (int num108 = 0; num108 < Main.rand.Next(2, 4); num108++)
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

                float speedX2 = num78 + Main.rand.Next(-160, 161) * 0.02f;
                float speedY2 = num79 + Main.rand.Next(-160, 161) * 0.02f;


                int proj = Projectile.NewProjectile(source, vector2, new Vector2(speedX2, speedY2), ProjectileID.HallowStar, damage / 2, adjustedKnockback, i, 0f, Main.rand.Next(10));
                Main.projectile[proj].DamageType = DamageClass.Melee;

                speedX2 = num78 + Main.rand.Next(-80, 81) * 0.02f;
                speedY2 = num79 + Main.rand.Next(-80, 81) * 0.02f;
                Projectile.NewProjectile(source, vector2, new Vector2(speedX2, speedY2), ProjectileType<TerraBall>(), damage, adjustedKnockback, i, 0f, Main.rand.Next(5));
            }

            return false;
        }

        

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int num249 = Main.rand.Next(3);
                if (num249 == 0)
                {
                    num249 = 15;
                }
                else if (num249 == 1)
                {
                    num249 = 57;
                }
                else
                {
                    num249 = 58;
                }
                int num250 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, num249, player.direction * 2, 0f, 150, default, 1.3f);
                Main.dust[num250].velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffType<HolyFlames>(), 300);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffType<HolyFlames>(), 300);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArkoftheAncients>().
                AddIngredient<CoreofCalamity>().
                AddIngredient(ItemID.BrokenHeroSword).
                AddIngredient<LivingShard>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
