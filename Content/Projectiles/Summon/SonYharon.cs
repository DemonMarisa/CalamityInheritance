using System;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class SonYharon: ModProjectile,ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.extraUpdates = 1;
            Projectile.minion = true;
            Projectile.minionSlots = 4f;
            Projectile.timeLeft = 18000;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            var usPlayer = player.CIMod();
            //召唤时生成粒子
            if (Projectile.localAI[0] == 0f)
            {
                int dCounts = 100;
                for (int i = 0; i < dCounts; i++)
                {
                    int dType = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, CIDustID.DustCopperCoin);
                    Main.dust[dType].velocity *= 2f;
                    Main.dust[dType].scale *= 1.15f;
                }
                Projectile.localAI[0] = 1f;
            }
            //改贴图朝向
            if (Math.Abs(Projectile.velocity.X) > 0.2f)
                Projectile.spriteDirection = -Projectile.direction;
            float num633 = 800f;
            float num634 = 1200f;
            float num635 = 3000f;
            float num636 = 500f;
            float colorValue = Main.rand.Next(90, 111) * 0.01f;
            colorValue *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 1.2f * colorValue, 0.8f * colorValue, 0f);
            bool ifHasMinion = Projectile.type == ProjectileType<SonYharon>();
            //给buff
            player.AddBuff(BuffType<SonYharonBuff>(), 1200);
            if (ifHasMinion)
            {
                if (player.dead) usPlayer.OwnSonYharon = false;
                if (usPlayer.OwnSonYharon) Projectile.timeLeft = 2;
            }
            //?
            float accele = 0.15f;
            for (int i = 0; i < 1000; i++)
            {
                if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && ifHasMinion && Math.Abs(Projectile.position.X - Main.projectile[i].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[i].position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < Main.projectile[i].position.X)
                        Projectile.velocity.X = Projectile.velocity.X - accele;
                    else
                        Projectile.velocity.X = Projectile.velocity.X + accele;

                    if (Projectile.position.Y < Main.projectile[i].position.Y)
                        Projectile.velocity.Y = Projectile.velocity.Y - accele;
                    else
                        Projectile.velocity.Y = Projectile.velocity.Y + accele;
                }
            }
            //?
            bool canUpdate = false;
            if (Projectile.ai[0] == 2f)
            {
                Projectile.ai[1] += 1f;
                Projectile.extraUpdates = 2;
                //帧图
                Projectile.frame = CIFunction.FramesChanger(Projectile, 3, 3);
                if (Projectile.ai[1] > 30f)
                {
                    Projectile.ai[1] = 1f;
                    Projectile.ai[0] = 0f;
                    Projectile.extraUpdates = 1;
                    Projectile.netUpdate = true;
                }
                else canUpdate = true;
            }
            if (canUpdate) return;

            Vector2 getMinionPos = Projectile.position;
            bool canChase = false;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float getDistance = Vector2.Distance(npc.Center, Projectile.Center);
                    if ((Vector2.Distance(Projectile.Center, getMinionPos) > getDistance && getDistance < num633) || !canChase)
                    {
                        num633 = getDistance;
                        getMinionPos = npc.Center;
                        canChase = true;
                    }
                }
            }
            else
            {
                for (int j = 0 ; j < 200 ; j++)
                {
                    NPC npc2 = Main.npc[j];
                    if (npc2.CanBeChasedBy(Projectile, false))
                    {
                        float npc2Dist = Vector2.Distance(npc2.Center,Projectile.Center);
                        if ((Vector2.Distance(Projectile.Center, getMinionPos) > npc2Dist && npc2Dist < num633) || !canChase)
                        {
                            num633 = npc2Dist;
                            getMinionPos = npc2.Center;
                            canChase = true;
                        }
                    }
                }
            }
            float num647 = num634;
            if (canChase) num647 = num635;
            if (Vector2.Distance(player.Center, Projectile.Center) > num647)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (canChase && Projectile.ai[0] == 0f)
            {
                Vector2 newPos = getMinionPos - Projectile.Center;
                float num648 = newPos.Length();
                newPos.Normalize();
                float scaleFac = num648 > 200f ? 8f : -4f;
                newPos *= scaleFac;
                Projectile.velocity = (Projectile.velocity * 40f + newPos) / 41f;
            }
            else
            {
                bool whatTheFuck = false;
                if (!whatTheFuck) whatTheFuck = Projectile.ai[0] == 1f;
                float num650 = 6f;
                if (whatTheFuck) num650 = 15f;
                Vector2 newVec = player.Center - Projectile.Center + new Vector2(0f, -60f);
                float num651 = newVec.Length();
                if (num651 > 200f && num650 < 8f)
                {
                    num650 = 8f;
                }
                if (num651 < num636 && whatTheFuck && !Collision.SolidCollision(Projectile.Center, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (num651 > 2000f)    
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width /2;
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - Projectile.height/2;
                    Projectile.netUpdate = true;
                }
                if (num651 > 70f)
                {
                    newVec.Normalize();
                    newVec *= num650;
                    Projectile.velocity = (Projectile.velocity * 40f + newVec) / 41f;
                }
                else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
            Projectile.frame = CIFunction.FramesChanger(Projectile, 12, 4);
            if (Projectile.ai[1] > 0f) Projectile.ai[1] += Main.rand.Next(1,4);
            if (Projectile.ai[1] > 40f)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.ai[1] == 0f && canChase && num633 < 500f)
                {
                    Projectile.ai[0] = 2f;
                    Vector2 getNewVec = getMinionPos - Projectile.Center;
                    getNewVec.Normalize();
                    Projectile.velocity = getNewVec * 8f;
                    Projectile.netUpdate = true;
                }
            }
        }
    }
}