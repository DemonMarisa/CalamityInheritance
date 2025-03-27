using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("RogueTypeGalaxySmasher")]
    public class RogueTypeHammerGalaxySmasher : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public static int BaseDamage = 325;
        public static float Speed = 20f;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProj>();
            Item.shootSpeed = Speed;
        }
        public override float StealthDamageMultiplier => 1.20f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.Calamity().StealthStrikeAvailable())//如果允许潜伏攻击
            {
                int stealth = Projectile.NewProjectile(source, position, velocity ,type, (int)(damage*1.15f), knockback, player.whoAmI, 0f, 0f, -3f);
                if(stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueTypeHammerStellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
