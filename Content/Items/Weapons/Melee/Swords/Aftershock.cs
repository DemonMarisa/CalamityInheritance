using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class Aftershock : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public const float ShootSpeed = 12f;
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 58;
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = ShootSpeed;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<ArmorCrunch>(), 300);
            Vector2 destination = target.Center;
            Vector2 position = destination - Vector2.UnitY * (destination.Y - Main.screenPosition.Y + 80f) + Vector2.UnitX * Main.rand.Next(-160, 161);
            Vector2 velocity = (destination - position).SafeNormalize(Vector2.UnitY) * ShootSpeed * Main.rand.NextFloat(0.9f, 1.1f);
            int rockDamage = player.GetWeaponDamage(Item);
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, ProjectileType<AftershockRock>(), rockDamage, hit.Knockback, player.whoAmI, 0f, Main.rand.Next(10), target.Center.Y);
        }
    }
}
