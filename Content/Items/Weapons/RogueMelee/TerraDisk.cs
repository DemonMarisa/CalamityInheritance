using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
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
    public class TerraDisk : CIMeleeRogue
    {

        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ExSD()
        {
            EffectColor = Color.ForestGreen;

            Item.width = 60;
            Item.height = 64;
            Item.damage = 100;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 30;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<TerraDiskProj>();
            Item.shootSpeed = Speed;

            Item.LAP().SkillShoot = ProjectileType<TerraDiskProj>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(Item.UseSound);
            int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            Main.projectile[p].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateCalRecipe(Type).
                AddCalIngredient<EquanimityLegacy>().
                AddCalIngredient<VictideBoomerang>().
                AddCalIngredient(ItemID.ThornChakram, 1).
                AddCalIngredient(CalamityMaterials.LivingShard, 8).
                AddCalTile(TileID.MythrilAnvil).
                RegisterCal();

            CreateRecipe().
                AddIngredient<EquanimityLegacy>().
                AddIngredient<VictideBoomerang>().
                AddIngredient(ItemID.ThornChakram).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
