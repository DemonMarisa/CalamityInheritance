using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.RogueMelee;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.RogueMelee
{
    public class StellarContempt : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ExSD()
        {
            EffectColor = Color.DeepSkyBlue;

            Item.width = 74;
            Item.height = 74;
            Item.DamageType = RogueDamage.Instance;
            Item.damage = 300;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ProjectileType<StellarContemptProj>();
            Item.shootSpeed = 12f;

            Item.LAP().SkillShoot = ProjectileType<StellarContemptProj>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(Item.UseSound);
            int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 1f);
            Main.projectile[p].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FallenHammer>().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
