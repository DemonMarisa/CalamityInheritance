using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles
{
    public class GenericProjRoute 
    {
        //总体射弹路径
        public static string ProjRoute => "CalamityInheritance/Content/Projectiles";
        //隐形贴图
        public static string InvisProjRoute => $"{ProjRoute}/InvisibleProj"; 
        //激光贴图
        public static string LaserProjRoute => $"{ProjRoute}/LaserProj"; 
        //闪电贴图
        public static string LightingProjRoute => $"{ProjRoute}/LightingProj"; 
        //星星贴图
        public static string StarProjRoute => $"{ProjRoute}/StarProj";
    }
    public abstract class RogueDamageProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public Player Owner => Main.player[Projectile.owner];
        public bool IsStealth => Projectile.Calamity().stealthStrike;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            ExSD();
        }
        public virtual void ExSD() { }
    }
    public abstract class GeneralDamageProj : ModProjectile, ILocalizedModType
    {
        public enum ProjDamageType
        {
            Melee,
            Ranged,
            Magic,
            Summon,
            Typeless
        }
        public virtual ProjDamageType UseDamageClass { get; }
        public new string LocalizationCategory => $"Content.Projectiles.{UseDamageClass}";
        public Player Owner => Main.player[Projectile.owner];
        public CalamityInheritancePlayer UsPlayer => Owner.CIMod();
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = GetDamageClass();
            ExSD();
        }
        public virtual void ExSD() { }
        private DamageClass GetDamageClass()
        {
            return UseDamageClass switch
            {
                ProjDamageType.Melee => DamageClass.Melee,
                ProjDamageType.Ranged => DamageClass.Ranged,
                ProjDamageType.Magic => DamageClass.Magic,
                ProjDamageType.Summon => DamageClass.Summon,
                _ => DamageClass.Generic,
            };
        }
    }
}