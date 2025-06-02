using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity.Special;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku;

namespace CalamityInheritance.Content.Items.Weapons.Typeless
{
    public class ShizukuEdge : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";
        public static readonly SoundStyle ProjectileDeathSound = SoundID.NPCDeath39 with { Volume = 0.5f};
        public static readonly int BaseDamage = 15000;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 108;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Generic;
            Item.useStyle = ItemUseStyleID.Shoot;
            //it dosen't matter.
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.knockBack = 5.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;

            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<ShizukuAqua>();

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetBestClassDamage().ApplyTo(BaseDamage);
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ShizukuEdgeProjectile>()] < 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //发射逻辑：往后方发射三个鬼魂， 往前方发射两个镰刀
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShizukuEdgeProjectile>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
        }
    }
}