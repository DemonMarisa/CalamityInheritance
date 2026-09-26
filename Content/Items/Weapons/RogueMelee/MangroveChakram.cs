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
    public class MangroveChakram : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            EffectColor = Color.SandyBrown;

            Item.width = 38;
            Item.height = 38;
            Item.damage = 84;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 7.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileType<MangroveChakramProj>();
            Item.shootSpeed = 16f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<MangroveChakramProj>();
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
                AddCalIngredient(CalamityMaterials.PerennialBar, 7).
                AddCalTile(TileID.MythrilAnvil).
                RegisterCal();

            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 3).
                AddIngredient(ItemID.JungleSpores, 3).
                AddIngredient<CoreofEleum>(2).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
