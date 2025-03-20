using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ElementalEruptionLegacy : CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 77;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 34;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item34;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraFireGreenLegacy2>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numFirestreams = Main.rand.Next(3, 5);
            for (int index = 0; index < numFirestreams; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.9f;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TerraFlamebursterLegacy>().
                AddIngredient<BlightSpewerLegacy>().
                AddIngredient<HavocsBreathLegacy>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
