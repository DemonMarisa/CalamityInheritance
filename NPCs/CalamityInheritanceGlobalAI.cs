using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs
{
    public class CIGlobalAI : GlobalNPC
    {
        #region Queen Bee Lore AI Changes
        public static void LoreQueenBeeEffect(NPC npc)
        {
            npc.damage = 0;

            if (npc.target < 0 || npc.target == Main.maxPlayers || Main.player[npc.target].dead)
                npc.TargetClosest(true);

            float num = 5f;
            float num2 = 0.1f;
            npc.localAI[0] += 1f;

            float num3 = (npc.localAI[0] - 60f) / 60f;
            if (num3 > 1f)
                num3 = 1f;
            else
            {
                if (npc.velocity.X > 6f)
                    npc.velocity.X = 6f;
                if (npc.velocity.X < -6f)
                    npc.velocity.X = -6f;
                if (npc.velocity.Y > 6f)
                    npc.velocity.Y = 6f;
                if (npc.velocity.Y < -6f)
                    npc.velocity.Y = -6f;
            }

            num2 *= num3;
            Vector2 vector = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num4 = npc.direction * num / 2f;
            float num5 = -num / 2f;

            if (npc.velocity.X < num4)
            {
                npc.velocity.X += num2;
                if (npc.velocity.X < 0f && num4 > 0f)
                    npc.velocity.X += num2;
            }
            else if (npc.velocity.X > num4)
            {
                npc.velocity.X -= num2;
                if (npc.velocity.X > 0f && num4 < 0f)
                    npc.velocity.X -= num2;
            }
            if (npc.velocity.Y < num5)
            {
                npc.velocity.Y += num2;
                if (npc.velocity.Y < 0f && num5 > 0f)
                    npc.velocity.Y += num2;
            }
            else if (npc.velocity.Y > num5)
            {
                npc.velocity.Y -= num2;
                if (npc.velocity.Y > 0f && num5 < 0f)
                    npc.velocity.Y -= num2;
            }

            if (npc.type != NPCID.Bee && npc.type != NPCID.BeeSmall)
            {
                if (npc.velocity.X > 0f)
                    npc.spriteDirection = 1;
                if (npc.velocity.X < 0f)
                    npc.spriteDirection = -1;

                npc.rotation = npc.velocity.X * 0.1f;
            }
            else
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;

            float num11 = 0.7f;
            if (npc.collideX)
            {
                npc.netUpdate = true;
                npc.velocity.X = npc.oldVelocity.X * -num11;

                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                    npc.velocity.X = 2f;
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                    npc.velocity.X = -2f;
            }
            if (npc.collideY)
            {
                npc.netUpdate = true;
                npc.velocity.Y = npc.oldVelocity.Y * -num11;

                if (npc.velocity.Y > 0f && npc.velocity.Y < 1.5)
                    npc.velocity.Y = 2f;
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1.5)
                    npc.velocity.Y = -2f;
            }

            if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                npc.netUpdate = true;
        }
        #endregion
    }
}
