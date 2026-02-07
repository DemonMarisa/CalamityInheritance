using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class StellarStrikerLegacy : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 100;
            Item.damage = 450;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.knockBack = 7.75f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 12f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Vortex);
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            SoundEngine.PlaySound(SoundID.Item88 with {MaxInstances = 0}, player.Center);
            int i = Main.myPlayer;
            float cometSpeed = Item.shootSpeed;
            int damage = (int)player.GetTotalDamage<MeleeDamageClass>().ApplyTo(Item.damage);
            for (int j = 0; j < 2; j++)
            {
                Vector2 realPlayerPos = new Vector2(player.Center.X + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.MouseWorld.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                realPlayerPos.Y -= 100 * j;

                Vector2 flareVelocity = LAPUtilities.GetVector2(realPlayerPos, player.LocalMouseWorld()) *  Item.shootSpeed;

                int proj = Projectile.NewProjectile(source, realPlayerPos, flareVelocity, ProjectileID.LunarFlare, damage, Item.knockBack, i, 0f, Main.rand.Next(3));
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].DamageType = DamageClass.Melee;
            }
        }
    }
}