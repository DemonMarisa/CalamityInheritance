using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Typeless.LevelFirework;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee";

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10;
            //Item.shoot = ModContent.ProjectileType<SummonLevelFirework_Final>();
        }/*
        public override bool CanUseItem(Player player)
        {
            return true;
        }*/
        /*
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int fireOffset = -100;
            Vector2 mousePos = Main.MouseWorld;
            int totalFire = 4;
            int firePosX = (int)(mousePos.X + player.Center.X) / 2;
            int firePosY = (int)player.Center.Y;

            for (int fireCount = 0; fireCount < totalFire; fireCount++)
            {
                // 垂直偏移计算
                Vector2 finalPos = new Vector2(firePosX, firePosY + fireOffset * fireCount);

                // 计算朝向鼠标的方向
                Vector2 direction = mousePos - finalPos;
                direction.Normalize();

                // 随机30度发射
                direction = direction.RotatedByRandom(MathHelper.ToRadians(15));

                // 保持原速度并应用新方向
                Vector2 newVelocity = direction * velocity.Length();

                int projectileFire = Projectile.NewProjectile(source, finalPos, newVelocity, ModContent.ProjectileType<Galaxia2>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
                Main.projectile[projectileFire].timeLeft = 160;
            }
            
            return false;
        }*/
        
        public override bool? UseItem(Player player)
        {
            var calPlayer = player.CIMod();
            if (CIConfig.Instance.UIX == 1)
                calPlayer.magicPool += CIConfig.Instance.LevelUp;
            if (CIConfig.Instance.UIX == 2)
                calPlayer.summonPool += CIConfig.Instance.LevelUp;
            if (CIConfig.Instance.UIX == 3)
                calPlayer.meleePool += CIConfig.Instance.LevelUp;
            if (CIConfig.Instance.UIX == 4)
                calPlayer.rangePool += CIConfig.Instance.LevelUp;
            if (CIConfig.Instance.UIX == 5)
                calPlayer.roguePool += CIConfig.Instance.LevelUp;
            // player.RemoveCICooldown(GodSlayerDash.ID);

            return base.CanUseItem(player);
        }
        
    }
}
