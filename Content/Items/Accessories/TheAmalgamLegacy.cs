using System;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class TheAmalgamLegacy : CIAccessories, ILocalizedModType
    {
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120;
        public int FireCountdown = 0;
        public override string Texture => $"{CIResprite.CalItemsRoute}/Accessories/TheAmalgam";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(9, 6));
            Type.ShimmerEach<TheAmalgam>();
            base.SetStaticDefaults();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().amalgam;
        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<TheAmalgam>());
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            var calPlayer = player.Calamity();
            player.GetDamage<GenericDamageClass>() += 0.20f;
            usPlayer.AmalgamLegacy = true;
            usPlayer.FungalClumpLegacySummon = true;
            SummonMinion(usPlayer, calPlayer, player, hideVisual);
            #region 龙涎香
            var source = player.GetSource_Accessory(Item);
            if (!player.lavaWet && !player.honeyWet)
            {
                if (!Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                {
                    player.endurance += 0.05f;
                    player.GetDamage<GenericDamageClass>() += 0.05f;
                }
                else
                {
                    player.GetDamage<GenericDamageClass>() += 0.15f;
                    player.statDefense += 50;
                    player.moveSpeed += 0.75f;
                }
            }
            if ((double)player.velocity.X > 0 || (double)player.velocity.Y > 0 || player.velocity.X < -0.1 || player.velocity.Y < -0.1)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    int seawaterDamage = (int)player.GetBestClassDamage().ApplyTo(50);
                    Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PoisonousSeawater>(), seawaterDamage, 5f, player.whoAmI, 0f, 0f);
                }
            }
            int seaCounter = 0;
            Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.5f, 1.25f);
            int tryGeyBuff = BuffID.Venom;
            float distance = 200f;
            bool flag = seaCounter % 60 == 0;
            int auraDamage = (int)player.GetBestClassDamage().ApplyTo(15);
            int random = Main.rand.Next(5);
            if (player.whoAmI == Main.myPlayer)
            {
                if (random == 0 && player.immune && Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                {
                    for (int l = 0; l < Main.maxNPCs; l++)
                    {
                        NPC tryGetNPC = Main.npc[l];
                        if (tryGetNPC.active && !tryGetNPC.friendly && tryGetNPC.damage > 0 && !tryGetNPC.dontTakeDamage && !tryGetNPC.buffImmune[tryGeyBuff] && Vector2.Distance(player.Center, tryGetNPC.Center) <= distance)
                        {
                            if (tryGetNPC.FindBuffIndex(tryGeyBuff) == -1)
                            {
                                tryGetNPC.AddBuff(tryGeyBuff, 300, false);
                            }
                            if (flag)
                            {
                                if (player.whoAmI == Main.myPlayer)
                                {
                                    Projectile.NewProjectileDirect(source, tryGetNPC.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), auraDamage, 0f, player.whoAmI, l);
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region 终结虚空
            if (player.lavaWet)
            {
                player.GetDamage<GenericDamageClass>() += 0.30f;
                player.GetCritChance<GenericDamageClass>() += 15;
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
            #endregion
        }

        private void SummonMinion(CalamityInheritancePlayer usPlayer, CalamityPlayer calPlayer, Player player, bool hideVisual)
        {
            int pType = ModContent.ProjectileType<FungalClumpLegacyMinion>();
            int buffType = ModContent.BuffType<FungalClumpLegacyBuff>();
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(buffType) == -1)
                    player.AddBuff(buffType, 3600, true);
                if (player.ownedProjectileCounts[pType] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(150);
                    int p = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, pType, damage, 6f, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = damage;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AmalgamatedBrain>().
                AddIngredient<VoidofExtinctionLegacy>().
                AddIngredient<FungalClump>().
                AddIngredient<LeviathanAmbergrisLegacy>().
                AddIngredient<AscendantSpiritEssence>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}