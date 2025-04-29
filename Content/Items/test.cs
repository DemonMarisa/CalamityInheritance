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
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.System;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items
{
    public class Test : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        

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
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            cIPlayer.meleeLevel = 0;
            cIPlayer.meleePool = 0;
            cIPlayer.rangeLevel = 0;
            cIPlayer.rangePool = 0;
            cIPlayer.magicLevel = 0;
            cIPlayer.magicPool = 0;
            cIPlayer.summonLevel = 0;
            cIPlayer.summonPool = 0;
            cIPlayer.rogueLevel = 0;
            cIPlayer.roguePool = 0;
            /*
            if (CIConfig.Instance.UIX == 3)
            {
                // 空列表检查
                if (CalStatInflationBACK.PostOldDukeWeapons == null ||
                    CalStatInflationBACK.PostOldDukeWeapons.Count == 0)
                {
                    Main.NewText("没有可生成的物品！");
                    return false;
                }

                // 生成所有物品
                foreach (int itemType in CalStatInflationBACK.PostOldDukeWeapons)
                {
                    player.QuickSpawnItem(player.GetSource_GiftOrReward(), itemType, 1);
                }

                // 显示提示信息
                Main.NewText($"生成了 {CalStatInflationBACK.PostOldDukeWeapons.Count} 件物品！");
            }
            */
            return true;
        }

    }
}
