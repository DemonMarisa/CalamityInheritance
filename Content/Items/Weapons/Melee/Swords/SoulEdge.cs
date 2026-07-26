using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class SoulEdge : CIMelee
    {

        public static readonly SoundStyle ProjectileDeathSound = SoundID.NPCDeath39 with { Volume = 0.5f };

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 88;
            Item.height = 88;
            Item.damage = 420;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.knockBack = 5.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<SoulEdgeSoulLegacyLarge>();
            Item.shootSpeed = 15f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numShots = 2;
            int soulDamage = (int)(damage * 0.8f);
            int[] projectileTypes = { ProjectileType<SoulEdgeSoulLegacyLarge>(), ProjectileType<SoulEdgeSoulLegacyMedium>(), ProjectileType<SoulEdgeSoulLegacySmall>() };
            for (int i = 0; i < numShots; ++i)
            {
                float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                float ai1 = Main.rand.NextFloat() + 0.5f;
                int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, randomProjectileType, soulDamage, knockback, player.whoAmI, 0.0f, ai1);
            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CICrushDepth>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<CICrushDepth>(), 300);
        }
    }
}