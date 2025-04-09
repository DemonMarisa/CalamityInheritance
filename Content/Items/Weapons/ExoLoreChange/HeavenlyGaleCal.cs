using System.Collections.Generic;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class HeavenlyGaleCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ModContent.ItemType<HeavenlyGale>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            //刚出的版本
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
                damage.Base = 355;
            base.ModifyWeaponDamage(item, player, ref damage);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> o)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.HeavenlyGaleChange"): null;
            if (t != null) o.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class HeavenlyGaleCrystalArrow : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoCrystalArrow>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                //天风的箭矢没有使用（暂时）ai2，因此这里会通过这个进行操作
                if (projectile.ai[2] != 1f)
                {
                    int ex = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1], 1f);
                    //原灾的重做其实已经足够优秀（而且耦合度太高了），这里给点额外更新了
                    Main.projectile[ex].extraUpdates += 1;
                }
                
            }
        }
    }
    public class HeavenlyGaleCrystalStrike : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoLightningBolt>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo && projectile.owner == Main.myPlayer)
            {
                //5->10
                projectile.MaxUpdates = 10;
                //13->10
                projectile.localNPCHitCooldown = projectile.MaxUpdates * 10;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            //因为总体闪电变多了因此这里射弹会降低20%伤害
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                modifiers.SourceDamage *= 0.80f;
        }
    }
}