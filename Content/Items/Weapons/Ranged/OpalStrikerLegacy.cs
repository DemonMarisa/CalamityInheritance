using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class OpalStrikerLegacy : CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 30;
            Item.useTime = 5;
            Item.reuseDelay = 25;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = CISoundMenu.OpalStriker;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OpalStrikeLegacy>();
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<OpalStrikeLegacy>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool();

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Marble, 20).
                AddIngredient(ItemID.Amber, 5).
                AddIngredient(ItemID.Diamond, 3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
