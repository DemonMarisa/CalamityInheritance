using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Rarity;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    internal class HolyColliderLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 80;
            Item.damage = 270;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 7.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            SoundEngine.PlaySound(SoundID.Item14, target.Center);
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(Item.shootSpeed, Item.shootSpeed) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int i;
            for (i = 0; i < 4; i++)
            {
                offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                int p = Projectile.NewProjectile(source, target.Center.X, target.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ProjectileType<HolyFireLegacy>(), damageDone, hit.Knockback, Main.myPlayer);
                int p2 = Projectile.NewProjectile(source, target.Center.X, target.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ProjectileType<HolyFireLegacy>(), damageDone, hit.Knockback, Main.myPlayer);
                Main.projectile[p].DamageType = DamageClass.Melee;
                Main.projectile[p2].DamageType = DamageClass.Melee;
            }
        }
    }
}
