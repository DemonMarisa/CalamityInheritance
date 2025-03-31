namespace CalamityInheritance.Content.Projectiles
{
    public class GenericProjRoute 
    {
        //总体射弹路径
        public static string ProjRoute => "CalamityInheritance/Content/Projectile";
        //隐形贴图
        public static string InvisProjRoute => $"{ProjRoute}/InvisibleProj"; 
        //激光贴图
        public static string LaserProjRoute => $"{ProjRoute}/LaserProj"; 
        //闪电贴图
        public static string LightingProjRoute => $"{ProjRoute}/LightingProj"; 
        //星星贴图
        public static string StarProjRoute => $"{ProjRoute}/StarProj";
    }
}