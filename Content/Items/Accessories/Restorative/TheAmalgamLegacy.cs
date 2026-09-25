using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Buff.SummonBuff.Accessories;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class TheAmalgamLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Restorative;
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120;
        public int FireCountdown = 0;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(9, 6));
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 10;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().NormalDoge = true;
            player.GetDamage<GenericDamageClass>() += 0.20f;
            if (player.lavaWet)
            {
                player.GetDamage<GenericDamageClass>() += 0.30f;
                player.GetCritChance<GenericDamageClass>() += 15;
            }
            if (player.IsUnderwater())
            {
                player.statDefense += 50;
                player.moveSpeed += 0.25f;
            }
            // 终结虚空
            player.lavaImmune = true;
            var source = player.GetSource_Accessory(Item);
            if (player.immune)
            {
                if (player.miscCounter % 10 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = player.CalcIntDamage<GenericDamageClass>(30);
                        Projectile fire = CIUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<StandingFire>(), damage, 5f, player.whoAmI);
                        fire.usesLocalNPCImmunity = true;
                        fire.localNPCHitCooldown = 60;
                    }
                }
            }
            if (FireCountdown == 0)
            {
                FireCountdown = 120;
            }
            if (FireCountdown > 0)
            {
                FireCountdown--;
                if (FireCountdown == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        for (int i = 0; i < FireProjectiles; i++)
                        {
                            int projSpeed = 25;
                            float spawnX = Main.rand.Next(1000) - 500 + player.LocalMouseWorld().X;
                            float spawnY = -1200 + player.LocalMouseWorld().Y;
                            Vector2 baseSpawn = new Vector2(spawnX, spawnY);
                            Vector2 baseVelocity = player.LocalMouseWorld() - baseSpawn;
                            baseVelocity.Normalize();
                            baseVelocity *= projSpeed;
                            Vector2 spawn = baseSpawn;
                            Vector2 velocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-FireAngleSpread / 2 + FireAngleSpread / FireProjectiles));
                            velocity.X = velocity.X + 3 * Main.rand.NextFloat() - 1.5f;
                            int damage = player.CalcIntDamage<GenericDamageClass>(2000);
                            Projectile.NewProjectile(source, spawn, velocity, ProjectileType<BrimstonefireballFriendly>(), damage, 5f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
            // 利维坦龙涎香
            if (player.miscCounter % 4 == 0)
            {
                if ((double)player.velocity.X > 0 || (double)player.velocity.Y > 0 || player.velocity.X < -0.1 || player.velocity.Y < -0.1)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int seawaterDamage = (int)player.GetTotalDamage<GenericDamageClass>().ApplyTo(500);
                        Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ProjectileType<PoisonousSeawater>(), seawaterDamage, 5f, player.whoAmI, 0f, 0f);
                    }
                }
            }
            Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.5f, 1.25f);
            if (player.miscCounter % 25 == 0)
            {
                int damage = player.CalcIntDamage<GenericDamageClass>(1000);
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= 300f)
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            int dir = npc.Center.X > player.Center.X ? 1 : -1;
                            player.ApplyDamageToNPC(npc, damage, 1f, dir, false, DamageClass.Generic, true);
                        }
                        if (npc.buffImmune[BuffID.Venom])
                            return;
                        if (npc.FindBuffIndex(BuffID.Venom) == -1)
                            npc.AddBuff(BuffID.Venom, 120, false);
                    }
                }
            }
            // 真菌块
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(BuffType<FungalClumpLegacyBuff>(), 2);
                if (player.ownedProjectileCounts[ProjectileType<FungalClumpMinion>()] < 1)
                {
                    int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center + new Vector2(600, 300), Vector2.UnitX, ProjectileType<FungalClumpMinion>(), 15, 1, player.whoAmI);
                    Main.projectile[p].originalDamage = 15;
                }
            }
            // Debuff
            if (player.miscCounter % 50 == 0)
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= 300f)
                    {
                        if (npc.buffImmune[BuffID.Venom])
                            return;
                        if (npc.FindBuffIndex(BuffID.Venom) == -1)
                            npc.AddBuff(BuffID.Venom, 120, false);

                        if (npc.buffImmune[BuffType<CIGodSlayerInferno>()])
                            return;
                        if (npc.FindBuffIndex(BuffType<CIGodSlayerInferno>()) == -1)
                            npc.AddBuff(BuffType<CIGodSlayerInferno>(), 120, false);

                        if (npc.buffImmune[BuffType<CIBrimstoneFlames>()])
                            return;
                        if (npc.FindBuffIndex(BuffType<CIBrimstoneFlames>()) == -1)
                            npc.AddBuff(BuffType<CIBrimstoneFlames>(), 120, false);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
        }
    }
}