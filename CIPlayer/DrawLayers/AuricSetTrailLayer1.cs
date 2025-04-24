using CalamityMod.Items.VanillaArmorChanges;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using System.Linq;
using System;
using CalamityInheritance.Utilities;
using CalamityMod.Graphics.Renderers;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.CIPlayer.DrawLayers
{
    public class AuricSetTrailLayer1 : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead || drawPlayer.Calamity().AdrenalineTrail || drawPlayer.Calamity().ascendantTrail)
                return false;

            return drawPlayer.CIMod().AuricSilvaFakeDeath;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            var modPlayer = drawPlayer.GetModPlayer<CalamityInheritancePlayer>();
            List<DrawData> existingDrawData = drawInfo.DrawDataCache;
            // 获取历史位置
            Vector2[] validOldPositions = modPlayer.oldPositions.ToArray();
            // 残影数量
            int maxAfterimages = validOldPositions.Length;
            // 移动速度乘数
            float movementSpeedInterpolant = CobaltArmorSetChange.CalculateMovementSpeedInterpolant(drawPlayer);

            List<DrawData> afterimages = new List<DrawData>();

            // 生成残影数据
            for (int i = 0; i < maxAfterimages; i++)
            {
                Vector2 oldPos = validOldPositions[i];
                float completionRatio = (float)i / maxAfterimages;
                float scale = MathHelper.Lerp(0.8f, 1f, completionRatio);
                float opacity = MathHelper.Lerp(0f, 0.2f, completionRatio) * movementSpeedInterpolant;
                foreach (DrawData original in existingDrawData)
                {
                    // 主残影
                    DrawData drawData = original;
                    drawData.position = original.position - drawPlayer.position + oldPos;
                    drawData.color = Color.Goldenrod with { A = 0 } * opacity; // 金色
                    drawData.scale *= scale;

                    // 深色描边
                    DrawData outlineData = original;
                    outlineData.position = drawData.position + new Vector2(2f * scale);
                    outlineData.color = Color.DarkGoldenrod * (opacity * 0.3f);
                    outlineData.scale *= scale;

                    afterimages.Add(outlineData);
                    afterimages.Add(drawData);
                }
            }
            drawInfo.DrawDataCache.AddRange(afterimages);
        }
    }
}
