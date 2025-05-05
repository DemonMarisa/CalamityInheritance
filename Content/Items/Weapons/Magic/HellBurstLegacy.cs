using CalamityInheritance.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class HellBurstLegacy : CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item34;
            Item.shoot = ModContent.ProjectileType<FlameBeamTip>();
            Item.autoReuse = true;
            Item.shootSpeed = 32f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => reduce -= 4;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int getProj = Projectile.NewProjectile(source, position, velocity, ProjectileID.Flamelash, damage, knockback, player.whoAmI);
                Main.projectile[getProj].scale *= 2.5f;
                Main.projectile[getProj].penetrate = 3;
            }
            else
            {
                Projectile.NewProjectile(source,position,velocity,ModContent.ProjectileType<FlameBeamTip>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source,position,-velocity,ModContent.ProjectileType<FlameBeamTip>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Flamelash).
                AddIngredient(ItemID.CrystalVileShard).
                AddIngredient(ItemID.DarkShard, 2).
                AddIngredient(ItemID.SoulofNight, 10).
                AddIngredient(ItemID.SoulofFright, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
