using CalamityInheritance.Common.CalamityModCross;
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

namespace CalamityInheritance.Content.Items.Weapons.RogueMelee
{
    public class ElementalDisk : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 38;
            Item.height = 38;
            Item.damage = 180;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<ElementalDiskProj>();
            Item.shootSpeed = 13f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<ElementalDiskProj_Rogue>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(Item.UseSound);
            int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 1f);
            Main.projectile[p].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateCalRecipe(Type).
                AddCalIngredient<MangroveChakram>().
                AddCalIngredient<SubductionSlicer>().
                AddCalIngredient<TerraDisk>().
                AddCalIngredient<GalacticaSingularity>(5).
                AddCalIngredient(CalamityMaterials.LifeAlloy, 5).
                AddCalIngredient(ItemID.LunarBar, 5).
                AddCalTile(TileID.LunarCraftingStation).
                RegisterCal();
            if (HasCalamity())
                return;
            CreateRecipe().
                AddIngredient<MangroveChakram>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<TerraDisk>().
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient(CalamityMaterials.LifeAlloy, 5).
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
