using System.Data;
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

public class ACTExcelsus : ModItem, ILocalizedModType
{
    public new string LocalizationCategory => "Content.Items.Weapons.Melee";
    #region 射弹属性管理，三个射弹除了贴图以外都是没区别的
    //射弹追踪速度
    public const float HomingSpeed = 52f;
    //射弹发起追踪的Timer
    public const float HomingTimer = 12f;
    //射弹的旋转,非追踪下的
    public const float NonHomingRotation = 0.54f;
    //搜索到敌怪单位后，发起追踪前的Timer
    public const float IdleTimer = 30;
    #endregion
    public override void SetDefaults()
    {
        Item.width = 78;
        Item.height = 94;
        Item.damage = 250;
        Item.DamageType = DamageClass.Melee;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 12;
        Item.useTurn = true;
        Item.knockBack = 8f;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        Item.rare = ModContent.RarityType<DarkBlue>();
        Item.shoot = ModContent.ProjectileType<ACTExcelsusMain>();
        Item.shootSpeed = 24f;
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/ACTExcelsusGlow").Value);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int pType = Main.rand.Next(3);
        pType = pType switch
        {
            0 => ModContent.ProjectileType<ACTExcelsusBlue>(),
            1 => ModContent.ProjectileType<ACTExcelsusPink>(),
            _ => ModContent.ProjectileType<ACTExcelsusMain>(),
        };
            Projectile.NewProjectile(source, position, velocity, pType, damage, knockback, player.whoAmI, 0f, 0f);
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
}
