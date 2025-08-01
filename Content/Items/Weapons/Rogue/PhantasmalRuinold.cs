using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class PhantasmalRuinold : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 655;
            Item.knockBack = 8f;

            Item.width = 102;
            Item.height = 98;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.autoReuse = true;
            Item.shootSpeed = 14.5f;
            Item.shoot = ModContent.ProjectileType<PhantasmalRuinProjold>();
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }

        public override float StealthDamageMultiplier => 0.9f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = player.CheckStealth();
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.LumiStriker).
                AddIngredient<PhantomLance>().
                AddIngredient<RuinousSoul>(4).
                AddIngredient<Necroplasm> (20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
