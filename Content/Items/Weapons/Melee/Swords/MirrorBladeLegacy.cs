using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class MirrorBladeLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        private int baseDamage = 100;

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 62;
            Item.damage = baseDamage;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<MirrorBlastLegacy>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int conDamage = target.damage + baseDamage;
            if (conDamage < baseDamage)
            {
                conDamage = baseDamage;
            }
            if (conDamage > 400)
            {
                conDamage = 400;
            }
            Item.damage = conDamage;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DarkPlasma>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
