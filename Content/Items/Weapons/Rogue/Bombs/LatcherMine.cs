using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Bombs;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Bombs
{
    internal class LatcherMine : CIRogue
    {
        public const int BaseDamage = 80;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        //让粘性地雷无法消耗从而让他正常获得词缀。
        public override void ExSD()
        {
            Item.height = 32;
            Item.width = 26;
            Item.damage = BaseDamage;
            Item.noMelee = true;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<LatcherMineProjectile>();
            Item.shootSpeed = 10f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<LatcherMineProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile stealth = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            stealth.SetStealthAttack();
        }
    }
}
