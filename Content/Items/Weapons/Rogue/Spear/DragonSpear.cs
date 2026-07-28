using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class DragonSpear : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 400;
            Item.noMelee = true;
            Item.DamageType = RogueDamage.Instance;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.rare = RarityType<YharonFire>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ProjectileType<DragonSpearProj>();
            Item.shootSpeed = 28f;
        }

        public override void PostSD()
        {
                        Item.LAP().SkillShoot = -1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int steal = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[steal].CI().Stealth = true;
        }
    }
}
