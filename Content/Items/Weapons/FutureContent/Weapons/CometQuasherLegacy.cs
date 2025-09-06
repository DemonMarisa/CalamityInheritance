using CalamityInheritance.Content.Projectiles.FutureContent.CometQuasher;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.FutureContent.Weapons
{
    public class CometQuasherLegacy : CIMelee, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 62;
            Item.damage = 134;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 7.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 9f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 destination = target.Center;
            Vector2 position = destination - (Vector2.UnitY * (destination.Y - Main.screenPosition.Y + 80f));
            Vector2 cachedPosition = position;

            int meteorDamage = (int)player.GetTotalDamage<MeleeDamageClass>().ApplyTo(Item.damage / 2f);
            float meteorKnockback = Item.knockBack * 0.5f;
            for (int i = 0; i < 3; i++)
            {
                position += Vector2.UnitX * Main.rand.Next(-320, 321);
                Vector2 velocity = (destination - position).SafeNormalize(Vector2.UnitY) * Item.shootSpeed * Main.rand.NextFloat(0.9f, 1.1f);
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, ModContent.ProjectileType<CQuasherMeteor>(), meteorDamage, meteorKnockback, player.whoAmI, 0f, 0.5f + Main.rand.NextFloat() * 0.3f, target.Center.Y);
                Main.projectile[proj].Calamity().lineColor = Main.rand.Next(3);
                position = cachedPosition;
            }
        }
    }
}