using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Effect;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class LumiStriker : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.height = 86;
            Item.width = 102;
            Item.damage = 90;
            Item.DamageType = RogueDamage.Instance;
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
            Item.shoot = ProjectileType<LumiStrikerProj>();
            Item.shootSpeed = 10f;

            Item.LAP().SkillShoot = ProjectileType<LumiStrikerProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 1.2f, type, damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(CISounds.LumiSpearAttackNor);
            Projectile.NewProjectile(source, position, -velocity * 1.4f, ProjectileType<LumiStrikerBack>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISounds.LanceofDestinyStrong);
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity * 1.2f, type, damage, knockback, player.whoAmI);
            p.SetStealthAttack();
            Projectile.NewProjectile(source, position, -velocity * 1.8f, ProjectileType<LumiStrikerBack>(), damage, knockback, player.whoAmI);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpearofPaleolith>().
                AddIngredient(ItemID.FragmentStardust, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}