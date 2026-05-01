
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class MeleeTriactisHammer : CIMelee, ILocalizedModType
    {
        public override string Texture => GetInstance<RogueTriactisHammer>().Texture;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 160;
            Item.height = 160;
            Item.damage = 6000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.knockBack = 50f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<RogueTriactisHammerProj>();
            Item.shootSpeed = 27f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();
            Item.Calamity().devItem = true;
            Item.LAP().UseCustomStatInflationMult = true;
            Item.LAP().StatInflationMult = 1.3f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].DamageType = DamageClass.Melee;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeleeGalaxySmasher>().
                AddIngredient(ItemID.SoulofMight, 30).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
