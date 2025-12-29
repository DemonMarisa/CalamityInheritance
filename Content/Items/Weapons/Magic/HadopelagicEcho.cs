using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class HadopelagicEcho : CIMagic, ILocalizedModType
    {
        
        private int counter = 0;
        public static readonly SoundStyle UseSound = new("CalamityInheritance/Sounds/Custom/WyrmScream") { Volume = 1f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 2554;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 8;
            Item.reuseDelay = 17;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileType<HadopelagicEchoSoundwave>();
            Item.rare = RarityType<CatalystViolet>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float damageMult = 1f;
            if (counter == 1)
                damageMult = 1.1f;
            if (counter == 2)
                damageMult = 1.2f;
            if (counter == 3)
                damageMult = 1.35f;
            if (counter == 4)
                damageMult = 1.5f;
            Projectile.NewProjectile(source,position.X, position.Y + 2, velocity.X, velocity.Y, type, (int)(damage * damageMult), knockback, player.whoAmI, counter, 0f);
            counter++;
            if (counter >= 5)
                counter = 0;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EidolicWail>()
                .AddIngredient<ReaperTooth>(20)
                .AddIngredient<DepthCells>(20)
                .AddIngredient<Lumenyl>(20)
                .AddIngredient<AuricBarold>()
                .AddTile(TileType<CosmicAnvil>())
                .Register();

            CreateRecipe()
                .AddIngredient<EidolicWail>()
                .AddIngredient<ReaperTooth>(20)
                .AddIngredient<DepthCells>(20)
                .AddIngredient<Lumenyl>(20)
                .AddIngredient<AuricBar>(5)
                .AddTile(TileType<CosmicAnvil>())
                .Register();
        }
    }
}
