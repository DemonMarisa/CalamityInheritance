using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.OldLordClaymoreLegacy;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class OldLordOathswordLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public int Filp = 1;
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 70;
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;

            Item.shoot = ProjectileType<OldLordOathswordLegacyHeld>();
            Item.shootSpeed = 6f;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.CIMod().CanUseOldLordDash)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<OldLordOathswordLegacyHeld_C>(), damage, knockback, player.whoAmI, Filp);
                player.CIMod().CanUseOldLordDash = false;
                return false;
            }
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<OldLordOathswordLegacyHeld_R>(), damage, knockback, player.whoAmI, Filp);
                return false;
            }
            if (Filp == 1)
            {
                Filp = -1;
            }
            else
            {
                Filp = 1;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Filp);
            return false;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool? CanHitNPC(Player player, NPC target) => false;

        public override bool CanHitPvp(Player player, Player target) => false;
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<DemonicBoneAsh>(3).
            AddTile(TileID.Anvils).
            Register();
        }
    }
}
