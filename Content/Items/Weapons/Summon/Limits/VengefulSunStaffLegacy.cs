using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod.Items.Materials;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Limits
{
    public class VengefulSunStaffLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Summon;
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 72;
            Item.damage = 60;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 35;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.15f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<SunSpiritMinionLegacy>();
        }
        public override bool CanUseItem(Player player) => !player.HasProj<SunSpiritMinionLegacy>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SunSpiriteStaffLegacy>().
                AddIngredient<EssenceofSunlight>(5).
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
