using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("RogueTypeTriactisTruePaladinianMageHammerofMight")]
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMight : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public static readonly int HammerDamage = 1750;
        public override void SetDefaults()
        {
            Item.width = 160;
            Item.height = 160;
            Item.damage = HammerDamage;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.knockBack = 50f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>();
            Item.shootSpeed = 27f;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;
        }
        public override float StealthDamageMultiplier => 1.35f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.Calamity().StealthStrikeAvailable() || player.CalamityInheritance().ForceHammerStealth)//如果允许潜伏攻击, 或者准备强行进行潜伏攻击
            {
                int stealth = Projectile.NewProjectile(source, position, velocity ,type, (int)(damage*1.14f), knockback, player.whoAmI);
                if(stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueTypeHammerGalaxySmasher>().
                AddIngredient(ItemID.SoulofMight, 30).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
