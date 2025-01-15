using CalamityMod;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class IchorSpearLegacy : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = 84;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 52;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<IchorSpearProjLegacy>();
            Item.shootSpeed = 20f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                    Main.projectile[stealth].usesLocalNPCImmunity = true;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ichor, 20).
                AddIngredient(ItemID.Vertebrae, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
