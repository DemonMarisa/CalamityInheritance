using CalamityInheritance.Content.BaseClass;
using Terraria.ModLoader;

using LAP.Assets.TextureRegister;
namespace CalamityInheritance.Content.Projectiles.HeldProj.Typeless
{
    // 用于复杂手持射弹的改变玩家朝向
    public class ChangeDirProj : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override float OffsetX => 0;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        public override float AimResponsiveness => 1f;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
    }
}
