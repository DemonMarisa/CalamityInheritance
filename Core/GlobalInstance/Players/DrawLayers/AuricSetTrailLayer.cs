using CalamityInheritance.Core.Graphics.Insertlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players.DrawLayers
{
    public class AuricSetTrailLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            if (drawInfo.shadow != 0f || drawPlayer.dead || drawPlayer.mount.Active)
                return false;

            return drawPlayer.CI().AuricSet && InsertDraw.DrawAuricTrail;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            var modPlayer = drawPlayer.CI();
            List<DrawData> existingDrawData = drawInfo.DrawDataCache;
            Vector2[] validOldPositions = modPlayer.oldPositions.ToArray();
            int maxAfterimages = validOldPositions.Length;
            List<DrawData> afterimages = new List<DrawData>();
            for (int i = 0; i < maxAfterimages; i++)
            {
                Vector2 oldPos = validOldPositions[i];
                float completionRatio = (float)i / maxAfterimages;
                float scale = MathHelper.Lerp(0.8f, 1f, completionRatio);
                float opacity = MathHelper.Lerp(0f, 0.2f, completionRatio);
                foreach (DrawData original in existingDrawData)
                {
                    // 主残影
                    DrawData drawData = original;
                    drawData.position = original.position - drawPlayer.position + oldPos;
                    drawData.color = Color.Goldenrod with { A = 0 } * opacity; // 金色
                    drawData.scale *= scale;
                    drawData.rotation = original.rotation;

                    // 深色描边
                    DrawData outlineData = original;
                    outlineData.position = drawData.position + new Vector2(2f * scale);
                    outlineData.color = Color.DarkGoldenrod * (opacity * 0.3f);
                    outlineData.scale *= scale;
                    outlineData.rotation = original.rotation;

                    afterimages.Add(outlineData);
                    afterimages.Add(drawData);
                }
            }

            drawInfo.DrawDataCache.AddRange(afterimages);
        }
    }
}
