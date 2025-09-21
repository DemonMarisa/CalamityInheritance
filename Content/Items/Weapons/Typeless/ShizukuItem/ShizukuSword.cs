using System;
using System.Collections.Generic;
using System.Linq;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem
{
    public class ShizukuSword : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Typeless";
        public override void SetStaticDefaults()
        {
            // Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 1140;
            Item.width = 100;
            Item.height = 112;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.noMelee = true;
            // Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<ShizukuAqua>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShizukuStar>();
            Item.channel = true;
            Item.shootSpeed = 12f;
        }
        public override bool AltFunctionUse(Player player)
        {
            //右键切换攻击模式
            ref ShizukuSwordType Style = ref player.CIMod().ShizukuSwordStyle;
            Style = Style switch
            {
                ShizukuSwordType.ArkoftheCosmos => ShizukuSwordType.TargetSpawn,
                _ => ShizukuSwordType.ArkoftheCosmos
            };
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            ShizukuSwordType shizukuSwordType = player.CIMod().ShizukuSwordStyle;
            switch (shizukuSwordType)
            {
                case ShizukuSwordType.ArkoftheCosmos:
                    Item.channel = false;
                    Item.noUseGraphic = false;
                    return true;
                case ShizukuSwordType.TargetSpawn:
                    Item.channel = true;
                    Item.noUseGraphic = true;
                    return true;
                default:
                    return false;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player owner = Main.LocalPlayer;
            ShizukuSwordType attackType = owner.CIMod().ShizukuSwordStyle;
            string defaultPath = $"{Generic.WeaponTextPath}Typeless.{GetType().Name}.";
            //创建列表映射，老样子。
            var handlers = new List<(Func<bool> Condition, Action<List<TooltipLine>, string> Handler)>
            {
                ( () => attackType is ShizukuSwordType.TargetSpawn, TooltipHallowedBiome),
                ( () => true, TooltipDefault)
            };
            handlers.First(t => t.Condition()).Handler(tooltips, defaultPath);
            string path = defaultPath + "Pillar";
            Color shizukuColor = new(152, 245, 255);
            tooltips.InsertNewLineToFinalLine(Mod, path, shizukuColor);
        }
        #region 特殊情况下的效果
        private void TooltipDefault(List<TooltipLine> tooltips, string defaultPath)
        {
            string path = defaultPath + "General";
            tooltips.FuckThisTooltipAndReplace(path);
        }
        private void TooltipHallowedBiome(List<TooltipLine> tooltips, string defaultPath)
        {
            string path = defaultPath + "Hallowed";
            tooltips.FuckThisTooltipAndReplace(path);
        }
        #endregion
        private delegate void ShootAction(CalamityInheritancePlayer modPlayer, CalamityPlayer calPlayer, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback);
        private readonly Dictionary<ShizukuSwordType, ShootAction> _shootActionMap = new()
        {
            {ShizukuSwordType.ArkoftheCosmos,ShootAOTC},
            {ShizukuSwordType.TargetSpawn,ShootTarget}
        };
        private static void ShootAOTC(CalamityInheritancePlayer modPlayer, CalamityPlayer calPlayer, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int fireOffset = -65;
            Vector2 mousePos = Main.MouseWorld;
            int totalFire = 4;
            int firePosX = (int)(mousePos.X + player.Center.X) / 2;
            int firePosY = (int)player.Center.Y;

            for (int fireCount = 0; fireCount < totalFire; fireCount++)
            {
                // 垂直偏移计算
                Vector2 finalPos = new(firePosX, firePosY + fireOffset * fireCount);
                // 计算朝向鼠标的方向
                Vector2 direction = mousePos - finalPos;
                direction.Normalize();
                // 随机30度发射
                direction = direction.RotatedByRandom(MathHelper.ToRadians(15));
                // 保持原速度并应用新方向
                Vector2 newVelocity = direction * velocity.Length();

                int projectileFire = Projectile.NewProjectile(source, finalPos, newVelocity, ModContent.ProjectileType<ShizukuStar>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
                Main.projectile[projectileFire].timeLeft = 160;
            }
        }
        private static void ShootTarget(CalamityInheritancePlayer modPlayer, CalamityPlayer calPlayer, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if  (player.ownedProjectileCounts[ModContent.ProjectileType<ShizukuSwordHoldout>()] < 1)
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ShizukuSwordHoldout>(), damage, knockback, player.whoAmI);
            if  (player.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarHoldout>()] < 1)
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ShizukuStarHoldout>(), damage, knockback, player.whoAmI);

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ShizukuSwordType attackType = player.CIMod().ShizukuSwordStyle;
            CalamityPlayer calPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.altFunctionUse is 2)
                return false;
            if (_shootActionMap.TryGetValue(attackType, out var shootAction))
                shootAction(modPlayer, calPlayer, player, source, position, velocity, type, damage, knockback);
            return false;
        }
    }
}