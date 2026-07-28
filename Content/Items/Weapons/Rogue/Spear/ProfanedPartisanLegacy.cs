using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class ProfanedPartisanLegacy : CIRogue
    {
        public override void ExSD()
        {
            Item.damage = 322;
            Item.knockBack = 8f;

            Item.width = 56;
            Item.height = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.DamageType = RogueDamage.Instance;

            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<ProfanedPartisanProj>();

            Item.LAP().SkillShoot = ProjectileType<ProfanedPartisanProj>();
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 15;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            p.SetStealthAttack();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UelibloomBar, 6).
                    AddIngredient(CalamityMaterials.DivineGeode, 4).
                    AddIngredient(CalamityMaterials.UnholyEssence, 25).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
