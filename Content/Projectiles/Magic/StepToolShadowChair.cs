using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityInheritance.NPCs.Boss.SCAL;


namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class StepToolShadowChair: ModProjectile, ILocalizedModType
    {
        private static readonly int TrueDamage = 25000;
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = 900;
            Projectile.height = 900;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            /*凳子的旋转时间*/
            float spinTime = 50f;

            /*玩家如果死了就摧毁凳子(kill)*/
            if(player.dead || !player.channel)
            {
                Projectile.Kill();
                player.reuseDelay = 2;
                return;
            }
            int spinDirection = Math.Sign(Projectile.velocity.X);
            Projectile.velocity = new Vector2(spinDirection, 0f);
            /*开始转凳子*/
            if(Projectile.ai[0] == 0f)
            {
                Projectile.rotation = new Vector2(spinDirection, -player.gravDir).ToRotation() + MathHelper.ToRadians(135f);
                if(Projectile.velocity.X <0f)
                {
                    Projectile.rotation -= MathHelper.PiOver2;
                }
            }

            Projectile.ai[0] += 1f;
            Projectile.rotation += MathHelper.TwoPi * 2f/spinTime * spinDirection;
            int wantedDirection = (player.SafeDirectionTo(Main.MouseWorld).X>0f).ToDirectionInt();
            if(Projectile.ai[0] % spinTime > spinTime*0.5f && wantedDirection != Projectile.velocity.X)
            {
                player.ChangeDir(wantedDirection);
                Projectile.velocity = Vector2.UnitX * wantedDirection;
                Projectile.rotation -= MathHelper.Pi;
                Projectile.netUpdate = true;
            }
            PositionAndRotation(player);
        }
        private void PositionAndRotation(Player player)
        {
            Vector2 plrCtr = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 offset = Vector2.Zero;
            Projectile.Center = plrCtr + offset;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            var modPlayer = Main.player[Projectile.owner].CIMod();
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(CISoundMenu.StepBonk, Projectile.Center);
            }

            if(modPlayer.StepToolShadowChairSmallCD <= 0)
            SpawnChair();//无视CD生成椅子

            //掠夺鲨、神长、地守和终灾会被直接秒杀.别问我为什么.
            if(target.type == ModContent.NPCType<ReaperShark>() || 
               target.type == ModContent.NPCType<DevourerofGodsHead>() ||
               target.type == ModContent.NPCType<DevourerofGodsBody>() ||
               target.type == ModContent.NPCType<DevourerofGodsTail>() ||
               target.type == ModContent.NPCType<SupremeCalamitas>() ||
               target.type == NPCID.DungeonGuardian)
            target.life -= target.lifeMax;

            //无视敌怪的防御数据，每次真“近战”a击中时必定扣除其固定点血量, 代码是这么写的.
            
            target.AddBuff(ModContent.BuffType<StepToolDebuff>(), 1145); //梯凳驾到
        }
        private void SpawnChair()
        {
            //每次攻击时都会生成2~4张椅子
            int chairCounts = Main.rand.Next(2,5);
            for(int i = 0; i < chairCounts; i++)
            {
                float angle = MathHelper.TwoPi / chairCounts*2;
                float speed = 24f;
                Vector2 velocity = new Vector2(0f, speed);
                velocity = velocity.RotatedBy(angle * i * Main.rand.NextFloat(0.9f,1.1f));
                //生成小椅子
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                         Projectile.Center,
                                         velocity,
                                         ModContent.ProjectileType<StepToolShadowChairSmall>(),
                                         Projectile.damage,
                                         Projectile.knockBack / 4f,
                                         Projectile.owner);
            }
            //给小凳子的生成加了等同于无敌帧的cd，不然打蠕虫怪的时候电脑会爆炸
            Main.player[Projectile.owner].CIMod().StepToolShadowChairSmallCD = 9;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            Rectangle myRect = Projectile.Hitbox;
            if (Projectile.owner == Main.myPlayer)
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    bool voodooDolls = Projectile.owner < Main.maxPlayers && (npc.type == NPCID.Guide && player.killGuide || npc.type == NPCID.Clothier && player.killClothier);
                    bool friendlyProjs = Projectile.friendly && (!npc.friendly || voodooDolls);
                    bool hostileProjs = Projectile.hostile && npc.friendly && !npc.dontTakeDamageFromHostiles;
                    if (!npc.dontTakeDamage && (friendlyProjs || hostileProjs) && (Projectile.owner < 0 || npc.immune[Projectile.owner] == 0 || Projectile.maxPenetrate == 1))
                    {
                        if (npc.noTileCollide || !Projectile.ownerHitCheck || (ProjectileLoader.CanHitNPC(Projectile, npc) ?? false))
                        {
                            bool canHit;
                            Rectangle rect = npc.Hitbox;
                            if (npc.type == NPCID.SolarCrawltipedeTail)
                            {
                                int offset = 8;
                                rect.X -= offset;
                                rect.Y -= offset;
                                rect.Width += offset * 2;
                                rect.Height += offset * 2;
                                canHit = Projectile.Colliding(myRect, rect);
                            }
                            else
                            {
                                canHit = Projectile.Colliding(myRect, rect);
                            }
                            if (canHit)
                            {
                                modifiers.HitDirectionOverride = (player.Center.X < npc.Center.X) ? 1 : -1;
                            }
                        }
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float spinning = Projectile.rotation - MathHelper.PiOver4 * (float)Math.Sign(Projectile.velocity.X);
            float staffRadiusHit = 110f;
            float useless = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + spinning.ToRotationVector2() * -staffRadiusHit, Projectile.Center + spinning.ToRotationVector2() * staffRadiusHit, 23f * Projectile.scale, ref useless))
            {
                return true;
            }
            return false;
        }

        /*出于某些原因，只有把Predraw函数加上才能显示出玩家转凳子的动画。*/
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 origin = tex.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(tex, drawPos, new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}