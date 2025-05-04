using CalamityMod.Items.Weapons.Rogue;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Quasar : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = 80; //50
            Item.crit += 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 0f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 48;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.shoot = ModContent.ProjectileType<QuasarKnife>();
            Item.shootSpeed = 20f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = player.CheckStealth();
            return false;
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                    AddRecipeGroup(CIRecipeGroup.LoreAD).
                    DisableDecraft().
                    AddTile(TileID.LunarCraftingStation).
                    Register();

            }
        }
    }
}
