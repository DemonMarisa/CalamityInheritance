using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        #region 路径
        public static string ExtraTexturesPath => "CalamityInheritance/ExtraTextures";
        #endregion

        #region 材质
        #region ShizukuSword
        public static Asset<Texture2D> ShizukuSwordGlow { get; private set; }
        public static Asset<Texture2D> ShizukuSwordTrail { get; private set; }
        public static Asset<Texture2D> ShizukuArkTrail { get; private set; }
        public static Asset<Texture2D> BaseTrail{ get; private set; }
        public static Asset<Texture2D> ShizukuBG { get; private set; }
        public static Asset<Texture2D> ShizukuStar { get; private set; }
        #endregion
        #endregion

        #region 加载卸载
        public override void Load()
        {
            ShizukuSwordTrail = Request<Texture2D>($"{ExtraTexturesPath}/Trails/ShizukuSword_Trail");
            ShizukuSwordGlow = Request<Texture2D>($"{ExtraTexturesPath}/Trails/ShizukuSword_Glow");
            ShizukuArkTrail = Request<Texture2D>($"{ExtraTexturesPath}/Trails/ShizukuArk_Trail");
            BaseTrail = Request<Texture2D>($"{ExtraTexturesPath}/Trails/BasicTrail");
            ShizukuBG = Request<Texture2D>($"CalamityInheritance/ExtraTextures/Metaballs/ShizukuStarMetaball" + "_Layer");
            ShizukuStar = Request<Texture2D>($"CalamityInheritance/ExtraTextures/Metaballs/ShizukuStarMetaball" + "_Texture");
            LoadProjTex();
            LoadItemTextures();
        }

        public override void Unload()
        {
            ShizukuSwordTrail = null;
            ShizukuSwordGlow = null;
            ShizukuArkTrail = null;
            BaseTrail = null;
            ShizukuBG = null;
            ShizukuStar = null;
            UnLoadProjTex();
            UnLoadItemTextures();
        }
        #endregion

    }
}
