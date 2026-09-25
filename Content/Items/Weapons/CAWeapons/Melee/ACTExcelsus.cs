using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.CAWeapons;
using CalamityInheritance.Content.Projectiles.Melee.CurvedSword;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.CAWeapons.Melee
{
    public class ACTExcelsus : CIMelee
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 94;
            Item.damage = 250;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useTurn = false;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<AlgtPink>();
            Item.shoot = ProjectileType<ACTExcelsusMain>();
            Item.shootSpeed = 18f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int pType;
            float spreading = 3.8f;
            // -1 0 1, 同时处理转角和弹幕类型。 -1: 蓝刀 -> -30°, 0: 主刀 -> 指向鼠标指针, 1: 粉刀 -> 30°
            for (int i = -1; i <= 1; i++)
            {
                pType = i switch
                {
                    0 => ProjectileType<ACTExcelsusMain>(),
                    1 => ProjectileType<ACTExcelsusPink>(),
                    _ => ProjectileType<ACTExcelsusBlue>(),
                };
                //处理转角即可
                float speedX = velocity.X;
                float speedY = velocity.Y + spreading * i;
                Vector2 newSpeed = new(speedX, speedY);
                Vector2 boostSpeed = i == 0 ? newSpeed / 4f : Vector2.Zero;
                Projectile.NewProjectile(source, position, newSpeed + boostSpeed, pType, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<LaserFountain>(), 0, 0, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<LaserFountain>(), 0, 0, player.whoAmI);
        }
        //总控射弹属性刷新
        public static void GlobalResetProj(Projectile projectile)
        {
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
        }
    }

}