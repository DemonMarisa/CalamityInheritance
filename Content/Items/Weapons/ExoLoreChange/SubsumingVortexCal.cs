using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.Audio;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class SubsumingVortexCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<SubsumingVortex>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 1165;
                item.mana = 20;
                item.useTime = 18;
                item.useAnimation = 18;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.SubsumingVortexChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    //大型追踪漩涡
    public class ExoVortexRework: GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoVortex2>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                projectile.velocity *= 1.02f;
            }
            base.OnSpawn(projectile, source);
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            int j = Main.rand.Next(1,4);
            int pCounts = Main.rand.Next(1,4);
            //较大的这些漩涡在死后会生成小的斩切
            if (projectile.DamageType == DamageClass.Magic && (usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                float hue = (j / (float)(pCounts- 1f) + Main.rand.NextFloat(0.3f)) % 1f;
                Vector2 vel = new Vector2(6f, 0f).RotatedByRandom(MathHelper.TwoPi);
                int magic = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, vel, ModContent.ProjectileType<ExobeamSlash>(), projectile.damage, projectile.knockBack, projectile.owner, hue);
                //标记这个射弹为魔法伤害
                Main.projectile[magic].DamageType = DamageClass.Magic;
                //2判，我们需要2判
                Main.projectile[magic].penetrate = 2;
                SoundEngine.PlaySound(Exoblade.BeamHitSound, projectile.Center);
                //斩击的音效
            }
        }
    }
    //小型追踪漩涡
    public class ExoVortexReworkSmall: GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoVortex>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                projectile.velocity *= 1.02f;
            }
        }
        //射弹被干掉的时候斩击
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            //限定只有法术伤害才会生成额外射弹
            if (projectile.DamageType == DamageClass.Magic && (usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                int magic = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.oldVelocity, ModContent.ProjectileType<ExobeamSlash>(), projectile.damage /2, projectile.knockBack, projectile.owner);
                //标记这个射弹为魔法伤害
                Main.projectile[magic].DamageType = DamageClass.Magic;
                Main.projectile[magic].extraUpdates = 2;
                //斩击的音效
                SoundEngine.PlaySound(Exoblade.BeamHitSound, projectile.Center);
            }
        }
    }

    //大漩涡
    public class ExoVortexReworkGiant: GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ProjectileType<EnormousConsumingVortex>();
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            var usPlayer = Main.player[projectile.owner].CIMod();
            //原灾暂时没有复写大漩涡的Onhit,因此这里如果判定到没开星流传颂，直接干掉AI就行了
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer && player.ActiveItem().type == ModContent.ItemType<SubsumingVortex>())
            {
                //这里最主要是为了确定生成的位置
                int offset = Main.rand.Next(200, 1080);
                //尽可能让射弹在屏幕外生成
                float xPos = player.position.X + offset * Main.rand.NextBool(2).ToDirectionInt();
                float yPos = player.position.Y + (Main.rand.NextBool() ? Main.rand.NextFloat(-600, -801): Main.rand.NextFloat(600, 801));
                Vector2 startPos = new(xPos, yPos);
                //指定好速度和方向
                Vector2 velocity = target.position - startPos;
                float dir = 10 / startPos.X;
                velocity.X *= dir * 150;
                velocity.Y *= dir * 150;
                velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
                velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);
                //固定三个，因为这个玩意右键手持的时候是有判定的
                int pCounts = 3;
                //击杀的时候往多个方向生成大量的……台风弹幕.
                for (int j = 0; j < pCounts; j++) 
                {
                    //改色，或者说改饱和度
                    float hue = (j / (float)(pCounts- 1f) + Main.rand.NextFloat(0.3f)) % 1f;
                    int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), startPos, velocity, ModContent.ProjectileType<ExoVortex2>(), projectile.damage / 2, projectile.knockBack, projectile.owner, hue); 
                    Main.projectile[p].DamageType = DamageClass.Magic;
                    Main.projectile[p].scale *= 0.85f;
                    //2穿, 即2判
                    Main.projectile[p].penetrate = 2;
                }
            }
        }
    }
}
