using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Assault
{
    public class SomaPrimeOld : CIRanged
    {

        private static readonly float XYInaccuracy = 0.32f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 34;
            Item.damage = 255;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 30f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CIShopValue.RarityPriceDonatorPink;

            Item.SetCalStatInflation(AllWeaponTier.DemonShadow, 2f);
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;
        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-25, -5);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity.X += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            velocity.Y += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            Vector2 vel = velocity;
            Projectile.NewProjectileDirect(source, position + new Vector2(0, -6), vel, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
