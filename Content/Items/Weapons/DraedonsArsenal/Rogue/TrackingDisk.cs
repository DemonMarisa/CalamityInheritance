using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Rogue;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Rogue
{
    public class TrackingDisk : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 30;
            Item.height = 34;
            Item.damage = 19;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.knockBack = 3f;

            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;

            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<TrackingDiskProj>();
            Item.shootSpeed = 8f;

            Item.LAP().SkillShoot = ProjectileType<TrackingDiskProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[proj].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 7).
                    AddIngredient(CalamityMaterials.DubiousPlating, 7).
                    AddIngredient(CalamityMaterials.AerialiteBar, 4).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
