using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Sounds.Custom.Shizuku;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem
{
    public class ShizukuSword : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Typeless";
        private const int ManaUsage = 200;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 200;
            //仅标记作用。
            Item.DamageType = ShizukuDamageClass.Instance;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.height = 112;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = RarityType<ShizukuAqua>();
            Item.shoot = ProjectileType<ShizukuEnergy>();
            Item.shootSpeed = 12f;

            Item.SetCalStatInflation(AllWeaponTier.DemonShadow);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            // 问题出在这里，二模式的情况下必须要按住右键射弹才不会凭空消失
            // 但是就算按住右键，channel也不会是true

            /*这里的问题非常逆天：
            Item.channel仅表示武器支持长按模式，j即允许player.channel状态生效，但不会主动设置player.channel为true。
            其次：若未设置item.useStyle = ItemUseStyleID.HoldUp（长按使用的动画样式），会导致长按输入无法触发player.channel
            最后：CanUseItem在每次玩家点击时触发，若返回true的时机与长按输入不同步，会导致player.channel无法被激活。
            武器设置了item.autoReuse = true，导致短按后自动重复使用，覆盖了长按的channel逻辑。
            综合上述才导致了射弹自杀现象
            */
            int OwnerAttackType = player.altFunctionUse;
            switch (OwnerAttackType)
            {
                default:
                    Item.channel = false;
                    Item.shootsEveryUse = true;
                    Item.useStyle = ItemUseStyleID.Swing;
                    Item.autoReuse = true;
                    Item.noMelee = false;
                    return true;
                case 2:
                    Item.channel = true;
                    Item.shootsEveryUse = false;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.autoReuse = false;
                    Item.noMelee = true;
                    return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player owner = Main.LocalPlayer;
            //->change it to holding shift
            bool isHoldingShift = Main.keyState.IsKeyDown(Keys.LeftAlt);
            string defaultPath = $"{Generic.WeaponTextPath}Typeless.{GetType().Name}.";
            //创建列表映射，老样子。
            var handlers = new List<(Func<bool> Condition, Action<List<TooltipLine>, string> Handler)>
            {
                ( () => isHoldingShift, TooltipHallowedBiome),
                ( () => true, TooltipDefault)
            };
            handlers.First(t => t.Condition()).Handler(tooltips, defaultPath);
            string path = defaultPath + "Pillar";
            string dayPath = defaultPath + "Evil";
            string nightPath = defaultPath + "TripEvent";
            Color shizukuColor = new(152, 245, 255);
            if (Main.dayTime || Main.eclipse)
                tooltips.InsertNewLineToFinalLine(Mod, dayPath);
            if (!Main.dayTime)
                tooltips.InsertNewLineToFinalLine(Mod, nightPath);
            tooltips.InsertNewLineToFinalLine(Mod, path);
        }
        #region 特殊情况下的效果
        private void TooltipDefault(List<TooltipLine> tooltips, string defaultPath)
        {
            string path = defaultPath + "General";
            tooltips.FuckThisTooltipAndReplace(path, "Alt");
        }
        private void TooltipHallowedBiome(List<TooltipLine> tooltips, string defaultPath)
        {
            string path = defaultPath + "Hallowed";
            tooltips.FuckThisTooltipAndReplace(path, GetManaUsage());
        }
        #endregion
        private static void ShootAOTC(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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

                int projectileFire = Projectile.NewProjectile(source, finalPos, newVelocity, ProjectileType<ShizukuStar>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
                Main.projectile[projectileFire].timeLeft = 160;
                Main.projectile[projectileFire].DamageType = DamageClass.Melee;
            }
            SoundStyle starToss = Utils.SelectRandom(Main.rand, ShizukuSounds.StarToss.ToArray());
            SoundEngine.PlaySound(starToss with { MaxInstances = 0, Volume = 0.6f }, position);
            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, 1.0f);
        }
        private static void ShootTarget(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int target = ProjectileType<ShizukuSwordHoldout>();
            if  (player.ownedProjectileCounts[target] < 1 && player.statMana > GetManaUsage())
                Projectile.NewProjectileDirect(source, position, velocity, target, damage, knockback, player.whoAmI);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse is 2)
                ShootTarget(player, source, position, velocity, type, damage, knockback);
            else
                ShootAOTC(player, source, position, velocity, type, damage, knockback);
            
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, Request<Texture2D>($"{Generic.WeaponPath}/Typeless/ShizukuItem/{GetType().Name}"+"_GlowMask").Value);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                    AddIngredient<BlessingOftheMoon>().
                    AddIngredient<Valediction>().
                    AddIngredient<SoulEdge>().
                    AddIngredient(ItemID.Chest).
                    AddIngredient(ItemID.LunarBar, 30).
                    AddIngredient<Lumenyl>(30).
                    AddIngredient<AshesofAnnihilation>(15).
                    AddIngredient<AncientMiracleMatter>().
                    AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                    AddCondition(Condition.NearShimmer).
                    Register();

        }
        public static int GetManaUsage() => Main.dayTime ? ManaUsage : ManaUsage / 2;
    }
}