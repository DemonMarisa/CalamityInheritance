using CalamityInheritance.Content.Projectiles.Legendary;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class AegisBladeLegendary : LegendaryWeapon
    {
        public override Color DrawColor => new(145, 115, 177);
        public int baseDamage = 75;
        public override void SetDefaults()
        {
            Item.height = Item.width = 72;
            Item.damage = baseDamage;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 10;
            Item.scale *= 1.25f;
            Item.useTurn = true;
            Item.knockBack = 4.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Melee;
                //谁家好人射弹剑刀刃不造成伤害啊?
                Item.noMelee = false;
                Item.useTurn = false;
                Item.UseSound = SoundID.Item73;
                Item.shoot = ProjectileType<AegisBladeLegendBeam>();
            }
            else
            {
                Item.DamageType = GetInstance<TrueMeleeDamageClass>();
                Item.noMelee = false;
                Item.useTurn = true;
                Item.UseSound = SoundID.Item1;
                Item.shoot = ProjectileID.None;
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (TierOneMatcher)
            {
                player.statDefense += (int)(.25f * player.statDefense);
            }
            //免疫防损变成了加dr
            if (TierTwoMatcher)
            {
                player.endurance += .3f;
            }
            if (TierThreeMatcher)
            {
                float ratios = player.statDefense * .001f / 3 * 4;
                player.GetDamage<MeleeDamageClass>() += ratios;
                player.GetAttackSpeed<MeleeDamageClass>() += ratios;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, player.whoAmI);
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GoldCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 53));
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) => 
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ProjectileType<AegisBladeLegendBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) => 
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ProjectileType<AegisBladeLegendBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
    }
}