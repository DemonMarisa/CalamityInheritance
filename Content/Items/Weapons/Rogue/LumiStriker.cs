using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class LumiStriker: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static readonly SoundStyle ThrowSound = new("CalamityMod/Sounds/Item/WulfrumKnifeTileHit2") { Volume = 0.3f, PitchVariance = 0.3f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 86;
            Item.width = 102;
            Item.damage = 180;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red; 
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 16f;
            Item.shoot = ModContent.ProjectileType<LumiStrikerProj>();
            Item.shootSpeed = 22f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable())
            {
                SoundEngine.PlaySound(ThrowSound);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].velocity *= 1.4f;
                Main.projectile[stealth].damage = (int)(damage * 0.70f);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpearofPaleolith>().
                AddIngredient<Turbulance>().
                AddIngredient(ItemID.FragmentStardust, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}