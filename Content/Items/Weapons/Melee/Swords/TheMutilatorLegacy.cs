using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    internal class TheMutilatorLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 90;
            Item.damage = 483;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.SpawnLifeStealProj(target, player.GetSource_ItemUse(Item), ProjectileType<StandardHealProj>(), target.Center, Vector2.Zero, 20);
            SoundEngine.PlaySound(SoundID.Item14, target.Center);
            target.position.X += target.width / 2;
            target.position.Y += target.height / 2;
            target.position.X -= target.width / 2;
            target.position.Y -= target.height / 2;
            for (int i = 0; i < 30; i++)
            {
                int bloodDust = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[bloodDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[bloodDust].scale = 0.5f;
                    Main.dust[bloodDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 50; j++)
            {
                int bloodDust2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 3f);
                Main.dust[bloodDust2].noGravity = true;
                Main.dust[bloodDust2].velocity *= 5f;
                bloodDust2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[bloodDust2].velocity *= 2f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
