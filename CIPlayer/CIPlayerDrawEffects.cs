using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod.Dusts;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityMod.Graphics.Renderers;
using CalamityMod.CalPlayer;
using CalamityMod.Particles;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public Vector2 RandomDebuffVisualSpot => Player.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-20f, 20f));
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            Player player = drawInfo.drawPlayer;
            CalamityInheritancePlayer cIplayer = player.CIMod();
            SetArmorEffectVisuals(player, drawInfo);
            if (abyssalFlames && drawInfo.shadow == 0f)
                AbyssalFlames.DrawEffects(drawInfo);
            if (vulnerabilityHexLegacy && drawInfo.shadow == 0f)
                VulnerabilityHexLegacy.DrawEffects(drawInfo);
        }
        #region Tanks/Backpacks
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            Item item = drawPlayer.ActiveItem();

            if (!drawPlayer.frozen &&
                (item.IsAir || item.type > ItemID.None) &&
                !drawPlayer.dead &&
                (!drawPlayer.wet || !item.noWet) &&
                (drawPlayer.wings == 0 || drawPlayer.velocity.Y == 0f))
            {
                //Make sure the lists are in the same order
                List<int> tankItems = new List<int>()
                {
                    ModContent.ItemType<Photovisceratorold>(),
                };
                List<Texture2D> tankTextures = new List<Texture2D>()
                {
                    ModContent.Request<Texture2D>("CalamityInheritance/CIPlayer/DrawLayers/Backpack_Photoviscerator").Value,
                };
                if (tankItems.Contains(item.type) || drawPlayer.Calamity().plaguebringerCarapace)
                {
                    Texture2D thingToDraw = null;
                    if (tankItems.Contains(item.type))
                    {
                        for (int i = 0; i < tankItems.Count; i++)
                        {
                            if (item.type == tankItems[i])
                            {
                                thingToDraw = tankTextures[i];
                                break;
                            }
                        }
                    }
                    if (thingToDraw is null)
                        return;

                    SpriteEffects spriteEffects = Player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                    DrawData howDoIDrawThings = new DrawData(thingToDraw,
                        new Vector2((int)(drawPlayer.position.X - Main.screenPosition.X + (drawPlayer.width / 2) - (9 * drawPlayer.direction)) - 4f * drawPlayer.direction, (int)(drawPlayer.position.Y - Main.screenPosition.Y + (drawPlayer.height / 2) + 2f * drawPlayer.gravDir - 8f * drawPlayer.gravDir)),
                        new Rectangle(0, 0, thingToDraw.Width, thingToDraw.Height),
                        drawInfo.colorArmorBody,
                        drawPlayer.bodyRotation,
                        new Vector2(thingToDraw.Width / 2, thingToDraw.Height / 2),
                        1f,
                        spriteEffects,
                        0);
                    howDoIDrawThings.shader = 0;
                    drawInfo.DrawDataCache.Add(howDoIDrawThings);
                }
            }
        }
        #endregion
        public void SetArmorEffectVisuals(Player player, PlayerDrawSet drawInfo)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (modPlayer.CIDashDelay < 0 && !modPlayer.AuricSilvaSet)
                player.armorEffectDrawShadow = true;

            if (modPlayer.AuricSilvaSet && drawInfo.shadow == 0f)
            {
                if (Player != null && !Player.dead)
                    return;

                Lighting.AddLight(Player.Center, Color.Lerp(Color.Gold, Color.DarkGoldenrod, 0.7f).ToVector3());
                if (!Player.StandingStill() && !Player.mount.Active)
                {
                    if (Main.rand.NextBool())
                    {
                        Vector2 velocity = -Player.velocity.SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(2, 5);
                        Particle nanoDust = new NanoParticle(drawInfo.Position + new Vector2(Main.rand.Next(Player.width + 1), Main.rand.Next(Player.height + 1)), velocity, (Main.rand.NextBool(3) ? Color.DarkGoldenrod : Color.Gold) * 0.9f, Main.rand.NextFloat(0.2f, 0.7f), 9, false, true);
                        GeneralParticleHandler.SpawnParticle(nanoDust);
                    }
                }
            }
        }
    }
}
