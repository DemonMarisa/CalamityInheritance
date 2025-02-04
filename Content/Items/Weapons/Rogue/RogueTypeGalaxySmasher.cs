using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeGalaxySmasher : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = BaseDamage;
            //星云射线现在一次生成9个
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;

            Item.shoot = ModContent.ProjectileType<GalaxySmasherHammerRogue>();
            Item.shootSpeed = Speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueTypeStellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();

            if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            {
                CreateRecipe().
                    AddIngredient<StellarContempt>().
                    AddIngredient<CosmiliteBar>(10).
                    AddTile<CosmicAnvil>().
                    Register();
            }
        }
    }
}
