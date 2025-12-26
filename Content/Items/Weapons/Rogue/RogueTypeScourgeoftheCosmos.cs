using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeScourgeoftheCosmos: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 64;
            Item.damage = 298;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<RogueTypeScourgeoftheCosmosProj>();
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if(stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = player.CheckStealth();
                
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ScourgeoftheCorruptor).
                AddIngredient<Bonebreaker>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
