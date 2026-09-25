using CalamityInheritance.Core.GlobalInstance.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static void SpawnBossUsingItem<BossType>(Player player, in SoundStyle? spawnSound = null) where BossType : ModNPC
        {
            SpawnBossUsingItem(player, NPCType<BossType>(), spawnSound);
        }
        public static void SpawnBossUsingItem(Player player, int npcType, in SoundStyle? spawnSound = null)
        {
            SoundEngine.PlaySound(spawnSound, player.Center);

            if (player.whoAmI != Main.myPlayer)
                return;

            // NOTE: MP netcode can be simplified by directly spawn npc like SpawnBossOnPosUsingItem does
            // but leaving this as vanilla's standard now
            switch (Main.netMode)
            {
                // SP: Spawn Boss Immediately
                case NetmodeID.SinglePlayer:
                    NPC.SpawnOnPlayer(player.whoAmI, npcType);
                    break;

                // MP: Ask server to spawn one
                case NetmodeID.MultiplayerClient:
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, npcType);
                    break;
            }
        }
        public static bool Organic(this NPC target)
        {
            if ((target.HitSound != SoundID.NPCHit4 && target.HitSound != SoundID.NPCHit41 && target.HitSound != SoundID.NPCHit2 && target.HitSound != SoundID.NPCHit5 && target.HitSound != SoundID.NPCHit11 && target.HitSound != SoundID.NPCHit30 && target.HitSound != SoundID.NPCHit34 && target.HitSound != SoundID.NPCHit36 && target.HitSound != SoundID.NPCHit42 && target.HitSound != SoundID.NPCHit49 && target.HitSound != SoundID.NPCHit52 && target.HitSound != SoundID.NPCHit53 && target.HitSound != SoundID.NPCHit54 && target.HitSound.HasValue))
            {
                return true;
            }

            return false;
        }
        public static void AddDebuffDamage(this NPC target, int damage)
        {
            target.CI().DeBuffDamage += damage;
        }
        public static void SmoothMovement(NPC npc, float movementDistanceGateValue, Vector2 distanceFromDestination, float baseVelocity, float acceleration, bool useSimpleFlyMovement)
        {
            // Inverse lerp returns the percentage of progress between A and B
            float lerpValue = Terraria.Utils.GetLerpValue(movementDistanceGateValue, 2400f, distanceFromDestination.Length(), true);

            // Min velocity
            float minVelocity = distanceFromDestination.Length();
            float minVelocityCap = baseVelocity;
            if (minVelocity > minVelocityCap)
                minVelocity = minVelocityCap;

            // Max velocity
            Vector2 maxVelocity = distanceFromDestination / 24f;
            float maxVelocityCap = minVelocityCap * 3f;
            if (maxVelocity.Length() > maxVelocityCap)
                maxVelocity = distanceFromDestination.SafeNormalize(Vector2.Zero) * maxVelocityCap;

            // Set the velocity
            Vector2 desiredVelocity = Vector2.Lerp(distanceFromDestination.SafeNormalize(Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
            if (useSimpleFlyMovement)
                npc.SimpleFlyMovement(desiredVelocity, acceleration);
            else
                npc.velocity = desiredVelocity;
        }
    }
}
