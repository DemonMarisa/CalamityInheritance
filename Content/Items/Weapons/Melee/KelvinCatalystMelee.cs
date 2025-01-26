using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class KelvinCatalystMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.damage = 50;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<KelvinCatalystBoomerangMelee>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        // public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        // {
        //     int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        //     if (proj.WithinBounds(Main.maxProjectiles))
        //         Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
        //     return false;
        // }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<IceStar>(200).
                AddIngredient<Avalanche>(2).
                AddIngredient<HoarfrostBow>(2).
                AddIngredient<Icebreaker>(2).
                AddIngredient<SnowstormStaff>(2).
                AddIngredient<SoulofCryogen>(2).
                AddIngredient<FrostFlare>(2).
                AddIngredient<CryoStone>(2).
                AddIngredient(ItemID.FrostStaff, 2).
                AddIngredient(ItemID.FrostCore, 2).
                AddIngredient(ItemID.FrozenKey, 2).
                AddIngredient<EssenceofEleum>(20).
                AddIngredient<CryoBar>(20).
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}
