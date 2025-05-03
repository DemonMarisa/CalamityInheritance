using System.Collections.Generic;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class ExoBladeCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<Exoblade>();
        
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var mp = player.CIMod();
            if (player.CheckExoLore())
            {
                damage.Base = 2500;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            string t = mp.PanelsLoreExo || mp.LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Melee.ExoBladeChange") : null;
            if (t != null)
                tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }

    //强化斩击与刀片命中时生成更多的光束
    public class ExtraBeams : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExobladeProj>() || entity.type == ModContent.ProjectileType<Exoboom>();
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            
            Player player = Main.player[projectile.owner];
            //生成的一瞬间会往不同的方向发射一些星流光束
            int pCounts = 2;
            //2 4 8 16
            for (int i = 2; i <= 16; i *=2)
                //最高+4, 总6
                pCounts += Main.rand.NextBool(i) ? 1 : 0;
            
            //大爆炸生成的数量需要缩水至少一半, 因为无敌帧的原因他可以造成双判->三判
            if (projectile.type == ModContent.ProjectileType<Exoboom>())
                pCounts /=2;

            //给CD，这里CD是必要的，不然这个刀片会无限生成多个光束
            //如果是斩击爆炸，直接绕过这个CD
            if ((player.CIMod().PanelsLoreExo || player.CIMod().LoreExo)&& (player.CIMod().GlobalFireDelay == 0 || projectile.type == ModContent.ProjectileType<Exoboom>()) && projectile.owner == Main.myPlayer)
            {
                for (int j = 0; j < pCounts; j++) 
                {
                    float hue = (j / (float)(pCounts- 1f) + Main.rand.NextFloat(0.3f)) % 1f;
                    Vector2 vel = new Vector2(6f, 0f).RotatedByRandom(MathHelper.TwoPi);
                    int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), target.Center, vel, ModContent.ProjectileType<Exobeam>(), projectile.damage / 2, projectile.knockBack, projectile.owner, hue); 
                    Main.projectile[p].DamageType = DamageClass.Melee;
                    Main.projectile[p].scale *= 0.9f;
                    //启用星流传颂的时候，星流束会+1判，但我们这里只想让星流束打三判。因此给二判。
                    Main.projectile[p].penetrate = 2;
                }
                player.CIMod().GlobalFireDelay = 10;
            }
        }
    }
    public class ExoBladeSlasherProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExobeamSlash>() || entity.type == ModContent.ProjectileType<ExobeamSlashCreator>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player p = Main.player[projectile.owner];
            var mp = p.CIMod();

            //手持原版的耀界，生成时将其设定为魔法伤害
            if ((mp.LoreExo || mp.PanelsLoreExo) && p.ActiveItem().type == ModContent.ItemType<VividClarity>() && projectile.owner == Main.myPlayer)
                projectile.DamageType = DamageClass.Magic;
        }
    }
    //星流光束，就是星流光束
    public class ExoBladeBeam : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<Exobeam>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var mp = Main.player[projectile.owner].CIMod();
            //+eu 判定
            if (projectile.owner == Main.myPlayer && (mp.LoreExo || mp.PanelsLoreExo))
            {
                projectile.extraUpdates += 1;
                projectile.penetrate += 1;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            var mp = Main.player[projectile.owner].CIMod();
            //modify这个射弹的伤害，使其恢复为接近刀本体50%而非34%
            //当然这里是只允许近战伤害跑这个的
            if (projectile.owner == Main.myPlayer && (mp.LoreExo || mp.PanelsLoreExo) && (projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed))
                //0.34 * 1.47 ≈ 0.5
                modifiers.SourceDamage *= 1.47f;

            base.ModifyHitNPC(projectile, target, ref modifiers);
        }
    }
}
