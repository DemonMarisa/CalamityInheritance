﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class VoidofExtinctionLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120;
        public int FireCountdown = 0;

        public override void SetStaticDefaults()
        {
            if(CIConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                
            }
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 12;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().voidOfCalamity;

    
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.voidOfCalamity = true;
            modPlayer.voidOfExtinction = true;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.fireWalk = true;
            player.lavaImmune = true;
            player.GetDamage<GenericDamageClass>() += 0.15f;
            if (player.lavaWet)
            {
                player.GetDamage<GenericDamageClass>() += 0.25f;
            }
            if (player.immune)
            {
                if (player.miscCounter % 10 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(30);
                        Projectile fire = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<StandingFire>(), damage, 5f, player.whoAmI);
                        if (fire.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            fire.usesLocalNPCImmunity = true;
                            fire.localNPCHitCooldown = 60;
                        }
                    }
                }
            }
            if (FireCountdown == 0)
            {
                FireCountdown = 600;
            }
            if (FireCountdown > 0)
            {
                FireCountdown--;
                if (FireCountdown == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int projSpeed = 25;
                        float spawnX = Main.rand.Next(1000) - 500 + player.Center.X;
                        float spawnY = -1000 + player.Center.Y;
                        Vector2 baseSpawn = new Vector2(spawnX, spawnY);
                        Vector2 baseVelocity = player.Center - baseSpawn;
                        baseVelocity.Normalize();
                        baseVelocity *= projSpeed;
                        for (int i = 0; i < FireProjectiles; i++)
                        {
                            Vector2 spawn = baseSpawn;
                            spawn.X += i * 30 - (FireProjectiles * 15);
                            Vector2 velocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-FireAngleSpread / 2 + (FireAngleSpread * i / FireProjectiles)));
                            velocity.X = velocity.X + 3 * Main.rand.NextFloat() - 1.5f;
                            int damage = (int)player.GetBestClassDamage().ApplyTo(100);
                            Projectile.NewProjectile(source, spawn, velocity, ModContent.ProjectileType<BrimstoneHellfireballFriendly2>(), damage, 5f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
        }
        public override void AddRecipes()
        {
                CreateRecipe()
                    .AddIngredient<CoreofCalamity>()
                    .AddIngredient<CoreofHavoc>()
                    .AddIngredient<ScoriaBar>(3)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
        }
    }
}
