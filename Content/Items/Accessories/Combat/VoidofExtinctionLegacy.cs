using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class VoidofExtinctionLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120;
        public int FireCountdown = 0;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 12;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            player.buffImmune[BuffID.OnFire] = true;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.BrimstoneFlames] = true;
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
                            int damage = player.CalcIntDamage<GenericDamageClass>(100);
                            Projectile.NewProjectile(source, spawn, velocity, ProjectileType<BrimstonefireballFriendly>(), damage, 5f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient<CoreofChaos>(3)
                    .AddIngredient(CalamityMaterials.ScoriaBar, 3)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient<CoreofChaos>(3)
                    .AddIngredient(ItemID.BeetleHusk, 3)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
        }
    }
}
