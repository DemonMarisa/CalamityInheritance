using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
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
    public class Pwnagehammer : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            EffectColor = Color.Gold;

            Item.width = 68;
            Item.height = 68;
            Item.damage = 90;
            Item.DamageType = RogueDamage.Instance;
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<PwnagehammerProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;

            Item.LAP().SkillShoot = ProjectileType<PwnagehammerProj_Rogue>();
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
                AddIngredient(ItemID.Pwnhammer).
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
