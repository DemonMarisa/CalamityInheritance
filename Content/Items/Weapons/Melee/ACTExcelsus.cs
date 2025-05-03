using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee;

public class ACTExcelsus : CIMelee, ILocalizedModType
{
    #region 主射弹属性管理
    //射弹追踪速度
    public const float HomingSpeed = 52f;
    //射弹发起追踪的Timer
    public const int HomingTimer = 18;
    //射弹的旋转,非追踪下的
    public const float NonHomingRotation = 0.74f;
    //搜索到敌怪单位后，发起追踪前的Timer
    public const int IdleTimer = 30;
    //射弹旋转速度
    public const float LerpAngle = 0.2f;
    //射弹最大检索距离
    public const float MaxSearchDist = 1600f;
    #endregion
    #region 附属射弹属性管理 
    //用于击中敌怪后的射弹减速
    public const float SideHitSlowSpeed = 0.965f;
    public const float SideIdleSlowSpeed = 0.9f;
    public const short SideFadeInTime = 50;
    #endregion
    public override void SetDefaults()
    {
        Item.width = 78;
        Item.height = 94;
        Item.damage = 250;
        Item.DamageType = DamageClass.Melee;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 14;
        Item.useTurn = true;
        Item.knockBack = 8f;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        Item.rare = ModContent.RarityType<DarkBlue>();
        Item.shoot = ModContent.ProjectileType<ACTExcelsusMain>();
        Item.shootSpeed = 18f;
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/ACTExcelsusGlow").Value);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int pType;
        float spreading = 3.8f;
        // -1 0 1, 同时处理转角和弹幕类型。 -1: 蓝刀 -> -30°, 0: 主刀 -> 指向鼠标指针, 1: 粉刀 -> 30°
        for (int i = -1; i <= 1; i ++)
        {
            pType = i switch
            {
                0 => ModContent.ProjectileType<ACTExcelsusMain>(), 
                1 => ModContent.ProjectileType<ACTExcelsusPink>(), 
                _ => ModContent.ProjectileType<ACTExcelsusBlue>(),
            };
            //处理转角即可
            float speedX = velocity.X;
            float speedY = velocity.Y + spreading * i;
            Vector2 newSpeed = new (speedX, speedY);
            Vector2 boostSpeed = i == 0 ? newSpeed / 4f : Vector2.Zero;
            Projectile.NewProjectile(source, position, newSpeed + boostSpeed, pType, damage, knockback, player.whoAmI);
        }
        return false;
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        var source = player.GetSource_ItemUse(Item);
        Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountain>(), 0, 0, player.whoAmI);
    }

    public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
    {
        var source = player.GetSource_ItemUse(Item);
        Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountain>(), 0, 0, player.whoAmI);
    }
    //总控射弹属性刷新
    public static void GlobalResetProj(Projectile projectile)
    {
        projectile.timeLeft = 300;
        projectile.penetrate = -1;
        projectile.localNPCHitCooldown = -1;
    }
}
