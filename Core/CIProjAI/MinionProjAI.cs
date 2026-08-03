using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Core.CIProjAI
{
    public static class MinionProjAI
    {
        public static void MinionAntiClump(this Projectile projectile, float pushForce = 0.05f)
        {
            for (int k = 0; k < Main.maxProjectiles; k++)
            {
                Projectile otherProj = Main.projectile[k];
                // Short circuits to make the loop as fast as possible
                if (!otherProj.active || otherProj.owner != projectile.owner || !otherProj.minion || k == projectile.whoAmI)
                    continue;

                // If the other projectile is indeed the same owned by the same player and they're too close, nudge them away.
                bool sameProjType = otherProj.type == projectile.type;
                float taxicabDist = Math.Abs(projectile.position.X - otherProj.position.X) + Math.Abs(projectile.position.Y - otherProj.position.Y);
                if (sameProjType && taxicabDist < projectile.width)
                {
                    if (projectile.position.X < otherProj.position.X)
                        projectile.velocity.X -= pushForce;
                    else
                        projectile.velocity.X += pushForce;

                    if (projectile.position.Y < otherProj.position.Y)
                        projectile.velocity.Y -= pushForce;
                    else
                        projectile.velocity.Y += pushForce;
                }
            }
        }
        public static void ChargingMinionAI(this Projectile projectile, float range, float maxPlayerDist, float extraMaxPlayerDist, float safeDist, int initialUpdates, float chargeDelayTime, float goToSpeed, float goBackSpeed, Vector2 returnOffset, float chargeCounterMax, float chargeSpeed, bool tileVision, bool ignoreTilesWhenCharging, int updateDifference = 1)
        {
            Player player = Main.player[projectile.owner];
            projectile.MinionAntiClump();

            //Breather time between charges as like a reset
            bool chargeDelay = false;
            if (projectile.ai[0] == 2f)
            {
                projectile.ai[1] += 1f;
                projectile.extraUpdates = initialUpdates + updateDifference;
                if (projectile.ai[1] > chargeDelayTime)
                {
                    projectile.ai[1] = 1f;
                    projectile.ai[0] = 0f;
                    projectile.extraUpdates = initialUpdates;
                    // CIT 2OCT2024: This line was breaking how the game counted minions,
                    // which resulted in minions being counted extra times and despawning other minions, most notoriously with Resurrection Butterfly.
                    // As such, I have commented it out.
                    // projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                }
                else
                {
                    chargeDelay = true;
                }
            }
            if (chargeDelay)
            {
                return;
            }

            //Find a target
            float maxDist = range;
            Vector2 targetVec = projectile.position;
            bool foundTarget = false;
            //Prioritize the targeted enemy if possible
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(projectile, false))
                {
                    //Check the size of the target to make it easier to hit fat targets like Levi
                    float extraDist = (npc.width / 2) + (npc.height / 2);

                    float targetDist = Vector2.Distance(npc.Center, projectile.Center);
                    //Some minions will ignore tiles when choosing a target like Ice Claspers, others will not
                    bool canHit = true;
                    if (extraDist < maxDist && !tileVision)
                        canHit = Collision.CanHit(projectile.Center, 1, 1, npc.Center, 1, 1);
                    if (!foundTarget && targetDist < (maxDist + extraDist) && canHit)
                    {
                        maxDist = targetDist;
                        targetVec = npc.Center;
                        foundTarget = true;
                    }
                }
            }
            //If no npc is specifically targetted or the selected enemy can't be found, check through the entire array
            if (!foundTarget)
            {
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    NPC npc = Main.npc[npcIndex];
                    if (npc.CanBeChasedBy(projectile, false))
                    {
                        float extraDist = (npc.width / 2) + (npc.height / 2);
                        float targetDist = Vector2.Distance(npc.Center, projectile.Center);
                        bool canHit = true;
                        if (extraDist < maxDist && !tileVision)
                            canHit = Collision.CanHit(projectile.Center, 1, 1, npc.Center, 1, 1);
                        if (!foundTarget && targetDist < (maxDist + extraDist) && canHit)
                        {
                            maxDist = targetDist;
                            targetVec = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            //If the player is too far, return to the player. Range is increased while attacking something.
            float distBeforeForcedReturn = maxPlayerDist;
            if (foundTarget)
            {
                distBeforeForcedReturn = extraMaxPlayerDist;
            }
            if (Vector2.Distance(player.Center, projectile.Center) > distBeforeForcedReturn)
            {
                projectile.ai[0] = 1f;
                projectile.netUpdate = true;
            }

            //Go to the target if you found one
            if (foundTarget && projectile.ai[0] == 0f)
            {
                //Some minions don't ignore tiles while charging like brittle stars
                projectile.tileCollide = !ignoreTilesWhenCharging;
                Vector2 targetSpot = targetVec - projectile.Center;
                float targetDist = targetSpot.Length();
                targetSpot.Normalize();
                //Tries to get the minion in the sweet spot of 200 pixels away but the minion also charges so idk what good it does
                if (targetDist > 200f)
                {
                    float speed = goToSpeed; //8
                    targetSpot *= speed;
                    projectile.velocity = (projectile.velocity * 40f + targetSpot) / 41f;
                }
                else
                {
                    float speed = -goBackSpeed; //-4
                    targetSpot *= speed;
                    projectile.velocity = (projectile.velocity * 40f + targetSpot) / 41f; //41
                }
            }

            //Movement for idle or returning to the player
            else
            {
                //Ignore tiles so they don't get stuck everywhere like Optic Staff
                projectile.tileCollide = false;

                bool returningToPlayer = false;
                if (!returningToPlayer)
                {
                    returningToPlayer = projectile.ai[0] == 1f;
                }

                //Player distance calculations
                Vector2 playerVec = player.Center - projectile.Center + returnOffset;
                float playerDist = playerVec.Length();

                //If the minion is actively returning, move faster
                float playerHomeSpeed = 6f;
                if (returningToPlayer)
                {
                    playerHomeSpeed = 15f;
                }
                //Move somewhat faster if the player is kinda far~ish
                if (playerDist > 200f && playerHomeSpeed < 8f)
                {
                    playerHomeSpeed = 8f;
                }
                //Return to normal if close enough to the player
                if (playerDist < safeDist && returningToPlayer && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                }
                //Teleport to the player if abnormally far
                if (playerDist > 2000f)
                {
                    projectile.position.X = player.Center.X - (float)(projectile.width / 2);
                    projectile.position.Y = player.Center.Y - (float)(projectile.height / 2);
                    projectile.netUpdate = true;
                }
                //If more than 70 pixels away, move toward the player
                if (playerDist > 70f)
                {
                    playerVec.Normalize();
                    playerVec *= playerHomeSpeed;
                    projectile.velocity = (projectile.velocity * 40f + playerVec) / 41f;
                }
                //Minions never stay still
                else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                {
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
            }

            //Increment attack counter randomly
            if (projectile.ai[1] > 0f)
            {
                projectile.ai[1] += (float)Main.rand.Next(1, 4);
            }
            //If high enough, prepare to attack
            if (projectile.ai[1] > chargeCounterMax)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }

            //Charge at an enemy if not on cooldown
            if (projectile.ai[0] == 0f)
            {
                if (projectile.ai[1] == 0f && foundTarget && maxDist < 500f)
                {
                    projectile.ai[1] += 1f;
                    if (Main.myPlayer == projectile.owner)
                    {
                        projectile.ai[0] = 2f;
                        Vector2 targetPos = targetVec - projectile.Center;
                        targetPos.Normalize();
                        projectile.velocity = targetPos * chargeSpeed; //8
                        projectile.netUpdate = true;
                    }
                }
            }
        }

    }
}
