using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CITextureRegistry : ModSystem
    {
        #region 路径
        public static string ExtraTexturesPath => "CalamityInheritance/ExtraTextures";
        #endregion

        #region 材质
        #region ShizukuSword
        public static Asset<Texture2D> ShizukuSwordGlow { get; private set; }
        public static Asset<Texture2D> ShizukuSwordTrail { get; private set; }
        #endregion
        #endregion

        #region 加载卸载
        public override void Load()
        {
            ShizukuSwordTrail = ModContent.Request<Texture2D>($"{ExtraTexturesPath}/Trails/ShizukuSword_Trail", AssetRequestMode.ImmediateLoad);
            ShizukuSwordGlow = ModContent.Request<Texture2D>($"{ExtraTexturesPath}/Trails/ShizukuSword_Glow", AssetRequestMode.ImmediateLoad);
        }

        public override void Unload()
        {
            ShizukuSwordTrail = null;
            ShizukuSwordGlow = null;
        }
        #endregion

    }
}
