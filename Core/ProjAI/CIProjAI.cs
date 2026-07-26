using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Core.ProjAI
{
    public static class CIProjAI
    {
        /// <summary>
        /// Call this function in ModifyHitNPC to make your projectiles stick to enemies, needs StickyProjAI to be called in the AI of the projectile
        /// </summary>
        /// <param name="projectile">The projectile you're giving sticky behaviour to</param>
        /// <param name="maxStick">How many projectiles of this type can stick to one enemy</param>
        public static void ModifyHitNPCSticky(this Projectile projectile, int maxStick)
        {
            Player player = Main.player[projectile.owner];
            Rectangle myRect = projectile.Hitbox;

            if (projectile.owner == Main.myPlayer)
            {
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    NPC npc = Main.npc[npcIndex];
                    //covers most edge cases like voodoo dolls
                    if (npc.active && !npc.dontTakeDamage &&
                        ((projectile.friendly && (!npc.friendly || (npc.type == NPCID.Guide && projectile.owner < Main.maxPlayers && player.killGuide) || (npc.type == NPCID.Clothier && projectile.owner < Main.maxPlayers && player.killClothier))) ||
                        (projectile.hostile && npc.friendly && !npc.dontTakeDamageFromHostiles)) && (projectile.owner < 0 || npc.immune[projectile.owner] == 0 || projectile.maxPenetrate == 1))
                    {
                        if (npc.noTileCollide || !projectile.ownerHitCheck)
                        {
                            bool stickingToNPC;
                            //Solar Crawltipede tail has special collision
                            if (npc.type == NPCID.SolarCrawltipedeTail)
                            {
                                Rectangle rect = npc.Hitbox;
                                int crawltipedeHitboxMod = 8;
                                rect.X -= crawltipedeHitboxMod;
                                rect.Y -= crawltipedeHitboxMod;
                                rect.Width += crawltipedeHitboxMod * 2;
                                rect.Height += crawltipedeHitboxMod * 2;
                                stickingToNPC = projectile.Colliding(myRect, rect);
                            }
                            else
                            {
                                stickingToNPC = projectile.Colliding(myRect, npc.Hitbox);
                            }
                            if (stickingToNPC)
                            {
                                //reflect projectile if the npc can reflect it (like Selenians)
                                if (npc.reflectsProjectiles && projectile.CanBeReflected())
                                {
                                    npc.ReflectProjectile(projectile);
                                    return;
                                }

                                //let the projectile know it is sticking and the npc it is sticking too
                                projectile.ai[0] = 1f;
                                projectile.ai[1] = (float)npcIndex;

                                //follow the NPC
                                projectile.velocity = (npc.Center - projectile.Center);

                                projectile.netUpdate = true;

                                //Count how many projectiles are attached, delete as necessary
                                Point[] array2 = new Point[maxStick];
                                int projCount = 0;
                                for (int projIndex = 0; projIndex < Main.maxProjectiles; projIndex++)
                                {
                                    Projectile proj = Main.projectile[projIndex];
                                    if (projIndex != projectile.whoAmI && proj.active && proj.owner == Main.myPlayer && proj.type == projectile.type && proj.ai[0] == 1f && proj.ai[1] == (float)npcIndex)
                                    {
                                        array2[projCount++] = new Point(projIndex, proj.timeLeft);
                                        if (projCount >= array2.Length)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (projCount >= array2.Length)
                                {
                                    int stuckProjAmt = 0;
                                    for (int m = 1; m < array2.Length; m++)
                                    {
                                        if (array2[m].Y < array2[stuckProjAmt].Y)
                                        {
                                            stuckProjAmt = m;
                                        }
                                    }
                                    Main.projectile[array2[stuckProjAmt].X].Kill();
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Call this function in the ai of your projectile so it can stick to enemies, also requires ModifyHitNPCSticky to be called in ModifyHitNPC
        /// </summary>
        /// <param name="projectile">The projectile you're adding sticky behaviour to</param>
        /// <param name="timeLeft">Number of seconds you want a projectile to cling to an NPC</param>
        public static void StickyProjAI(this Projectile projectile, int timeLeft, bool findNewNPC = false)
        {
            if (projectile.ai[0] == 1f)
            {
                int seconds = timeLeft;
                bool killProj = false;
                bool spawnDust = false;

                //the projectile follows the NPC, even if it goes into blocks
                projectile.tileCollide = false;

                //timer for triggering hit effects
                projectile.localAI[0]++;
                if (projectile.localAI[0] % 30f == 0f)
                {
                    spawnDust = true;
                }

                //So AI knows what NPC it is sticking to
                int npcIndex = (int)projectile.ai[1];
                NPC npc = Main.npc[npcIndex];

                //Kill projectile after so many seconds or if the NPC it is stuck to no longer exists
                if (projectile.localAI[0] >= (float)(60 * seconds))
                {
                    killProj = true;
                }
                else if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
                {
                    killProj = true;
                }

                else if (npc.active && !npc.dontTakeDamage)
                {
                    //follow the NPC
                    projectile.Center = npc.Center - projectile.velocity * 2f;
                    projectile.gfxOffY = npc.gfxOffY;

                    //if attached to npc, trigger npc hit effects every half a second
                    if (spawnDust)
                    {
                        npc.HitEffect(0, 1.0);
                    }
                }
                else
                {
                    killProj = true;
                }

                //Kill the projectile or reset stats if needed
                if (killProj)
                {
                    if (findNewNPC)
                        projectile.ai[0] = 0f;
                    else
                        projectile.Kill();
                }
            }
        }
    }
}
