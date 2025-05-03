using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Graphics.Metaballs;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    /*星流传颂之物重做计划:
    右键: 射弹将完全穿墙， 
    */

    public class PhotovisceratorCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<Photoviscerator>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();

            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                damage.Base = 810;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.PhotovisceratorChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<OmicronWingman>()] < 1)
            {
                Projectile.NewProjectile(source, position, (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<OmicronWingman>(), damage, knockback, player.whoAmI, 0, 0, 1);
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
    public class PhotovisceratorProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoFlareCluster>();
        //干掉原本的AI，我们重写一个
        public Color sparkColor;
        public int Time = 0;
        public override void AI(Projectile projectile)
        {
            //在AI内更新射弹非穿墙的属性
            Player player = Main.player[projectile.owner];
            var usPlayer = player.CIMod();
            Time++;
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                projectile.tileCollide = false;
                //在行程路径上生成若干星流碎片
                //不好孩子们，星流碎片被这个b白面团挡完了
                if (Time % 10 == 0)
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity * 0.1f, ModContent.ProjectileType<PhotovisceratorCrystal>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
            }
            sparkColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };

            PhotoMetaball.SpawnParticle(projectile.Center, 90);
            PhotoMetaball2.SpawnParticle(projectile.Center, 85);
            CIFunction.HomeInOnNPC(projectile, true, 1200f, 12f, 20f);
        }
    }
    public class OmicronWingman_Photoviscerator : OmicronWingman
    {
        public override void AI()
        {
            Owner ??= Main.player[base.Projectile.owner];
            if (time > 1 && Owner.ownedProjectileCounts[ModContent.ProjectileType<PhotovisceratorHoldout>()] < 1 && PostFireCooldown <= 0f)
            {
                base.Projectile.Kill();
            }
            Lighting.AddLight(base.Projectile.Center, StaticEffectsColor.ToVector3() * 0.2f);
            if (time == 0)
            {
                MovingUp = base.Projectile.ai[2] == 1f;
            }
            firingDelay--;
            Item heldItem = Owner.ActiveItem();
            base.Projectile.damage = ((heldItem != null) ? Owner.GetWeaponDamage(heldItem) : 0);
            if (((PostFireCooldown == 0f && launchDelay == 0 && Owner.CantUseHoldout()) || heldItem.type != ModContent.ItemType<Photoviscerator>()) && PostFireCooldown <= 0f)
            {
                base.Projectile.Kill();
            }
            if (PostFireCooldown > 0f)
            {
                PostFiringCooldown();
            }
            if (launchDelay > 0 || (PostFireCooldown <= 0f && (Owner.Calamity().mouseRight || (firingDelay <= 0 && base.Projectile.ai[2] == 1f) || base.Projectile.ai[2] == -1f)))
            {
                if (launchDelay > 0 || Owner.Calamity().mouseRight)
                {
                    if (launchDelay < 50)
                    {
                        launchDelay++;
                    }
                    if (launchDelay >= 50 && Owner.CheckMana(Owner.ActiveItem(), (int)(heldItem.mana * Owner.manaCost) * 2, pay: true))
                    {
                        Shoot(isGrenade: true);
                        PostFireCooldown = 50f;
                        ShootingTimer = 0f;
                        launchDelay = 0;
                    }
                }
                else if (ShootingTimer >= FiringTime)
                {
                    if (Owner.CheckMana(Owner.ActiveItem()))
                    {
                        Shoot(isGrenade: false);
                        ShootingTimer = 0f;
                    }
                    else if (PostFireCooldown <= 0f)
                    {
                        base.Projectile.Kill();
                    }
                }
            }
            Vector2 ownerPosition = Owner.MountedCenter;
            Vector2 ownerToMouse = Owner.Calamity().mouseWorld - ownerPosition;
            ManagePlayerProjectileMembers(ownerToMouse);
            if (OffsetLength != MaxOffsetLength)
            {
                OffsetLength = MathHelper.Lerp(OffsetLength, MaxOffsetLength, 0.1f);
            }
            ShootingTimer += 1f;
            time++;
            base.Projectile.soundDelay--;
            base.Projectile.netSpam = 0;
            base.Projectile.netUpdate = true;
        }
        protected new void Shoot(bool isGrenade)
        {
            Vector2 shootDirection = (Owner.Calamity().mouseWorld - base.Projectile.Center).SafeNormalize(Vector2.UnitX);
            Vector2 tipPosition = base.Projectile.Center + base.Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(-0.05f * Projectile.direction) * 12f;
            Vector2 firingVelocity = shootDirection * 10f;
            if (isGrenade)
            {
                SoundStyle style = new SoundStyle("CalamityMod/Sounds/Item/DeadSunExplosion");
                style.Volume = 0.2f;
                style.Pitch = -0.4f;
                style.PitchVariance = 0.2f;
                SoundEngine.PlaySound(in style, base.Projectile.Center);
                if (Main.myPlayer == base.Projectile.owner)
                {
                    Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), tipPosition, firingVelocity, ModContent.ProjectileType<WingmanGrenade>(), base.Projectile.damage * 14, base.Projectile.knockBack * 5f, base.Projectile.owner, 0f, 2f).timeLeft = 530;
                    Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), tipPosition, firingVelocity * 1.2f, ModContent.ProjectileType<WingmanGrenade>(), base.Projectile.damage * 14, base.Projectile.knockBack * 5f, base.Projectile.owner, 0f, 2f);
                }
            }
            else
            {
                SoundStyle style = new SoundStyle("CalamityMod/Sounds/Item/MagnaCannonShot");
                style.Volume = 0.25f;
                style.Pitch = 1f;
                style.PitchVariance = 0.35f;
                SoundEngine.PlaySound(in style, base.Projectile.Center);
                if (Main.myPlayer == base.Projectile.owner)
                {
                    Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), tipPosition, firingVelocity, ModContent.ProjectileType<WingmanShot>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 2f);
                    Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), tipPosition, firingVelocity.RotatedBy(-0.05) * 0.85f, ModContent.ProjectileType<WingmanShot>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 2f);
                    Projectile.NewProjectileDirect(base.Projectile.GetSource_FromThis(), tipPosition, firingVelocity.RotatedBy(0.05) * 0.85f, ModContent.ProjectileType<WingmanShot>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 2f);
                }
            }
            if (!Main.dedServ)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 shootVel = (shootDirection * 10f).RotatedByRandom(0.5) * Main.rand.NextFloat(0.1f, 1.8f);
                    Dust dust = Dust.NewDustPerfect(tipPosition, Main.rand.NextBool(4) ? 264 : 66, shootVel);
                    dust.scale = Main.rand.NextFloat(1.15f, 1.45f);
                    dust.noGravity = true;
                    dust.color = (Main.rand.NextBool() ? Color.Lerp(StaticEffectsColor, Color.White, 0.5f) : StaticEffectsColor);
                }
                GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(tipPosition - shootDirection * 14f, shootDirection * 20f, affectedByGravity: false, Main.rand.Next(7, 12), 0.035f, StaticEffectsColor, new Vector2(1.5f, 0.9f), quickShrink: true));
                if (isGrenade)
                {
                    OffsetLength -= 27f;
                }
                else
                {
                    OffsetLength -= 5f;
                }
            }
        }
    }
}
