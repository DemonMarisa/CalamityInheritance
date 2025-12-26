using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using System;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.EntitySources;
using CalamityMod.Enums;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Balancing;
using CalamityInheritance.CIPlayer.Dash;
using CalamityMod.Items.Mounts;
using System.Collections.Generic;

/* 
 * 这一段是从灾厄里面复制过来的盾冲实现机制
 * 但是直接复制过来后会出现bug，只有开始时的特效，没有后续的击中和过程效果
 * 后续排查中发现为ModDashMovement中的Player.dashDelay与DoADash中的Player.dashDelay没有联动
 * 导致MDM中的DD始终为0，无法触发效果
 * 于是新注册了CItestDashDelay用作交换
 * CIDashDelay可以设置任意值，并且会以1帧1点的速度归零
 * 如果复制请复制全，不然你会遇到莫名其妙的bug
 * 而且这一段没有残影绘制，请自己添加
 * 
 *盾冲朝向排查2025
 .1.5
 *与冲刺速度，冲刺mideffect无关
 *与75行无关
 *找到了，见180行
*/
namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public int VerticalGodslayerDashTimer;
        public int VerticalSpeedBlasterDashTimer;

        public string CIDashID;

        public string DeferredDashID;

        public string LastUsedDashID;
        //"Asgard's Valor old"
        public PlayerDashEffect UsedDash
        {
            get
            {
                CIPlayerDashManager.FindByID(CIDashID, out PlayerDashEffect dashEffect);
                return dashEffect;
            }
        }

        public bool HasCustomDash => !string.IsNullOrEmpty(CIDashID);

        public void ModDashMovement()
        {
            if (Player.whoAmI != Main.myPlayer)
                return;

            var source = new ProjectileSource_PlayerDashHit(Player);

            // Handle collision slam-through effects.
            //和这一块也无关
            //if (HasCustomDash && Player.dashDelay < 1)
            if (HasCustomDash && CIDashDelay < 0)
            {
                //Main.NewText($"CItestDashDelay moddashmovement : {CItestDashDelay}");
                Rectangle hitArea = new Rectangle((int)(Player.position.X + Player.velocity.X * 0.5 - 4f), (int)(Player.position.Y + Player.velocity.Y * 0.5 - 4), Player.width + 8, Player.height + 8);
                foreach (NPC n in Main.ActiveNPCs)
                {
                    // Ignore critters with the Guide to Critter Companionship
                    if (Player.dontHurtCritters && NPCID.Sets.CountsAsCritter[n.type])
                        continue;

                    if (!n.dontTakeDamage && !n.friendly && n.Calamity().dashImmunityTime[Player.whoAmI] <= 0)
                    {
                        if (hitArea.Intersects(n.getRect()) && (n.noTileCollide || Player.CanHit(n)))
                        {
                            //Main.NewText("正在执行 dash 的碰撞");
                            DashHitContext hitContext = default;
                            UsedDash.OnHitEffects(Player, n, source, ref hitContext);

                            // Don't bother doing anything if no damage is done.
                            if (hitContext.damageClass is null || hitContext.BaseDamage <= 0)
                                continue;

                            // Duplicated from the way TML edits vanilla ram dash damage (and Shield of Cthulhu)
                            int dashDamage = (int)Player.GetTotalDamage(hitContext.damageClass).ApplyTo(hitContext.BaseDamage);
                            float dashKB = Player.GetTotalKnockback(hitContext.damageClass).ApplyTo(hitContext.BaseKnockback);
                            bool rollCrit = Main.rand.Next(100) < Player.GetTotalCritChance(hitContext.damageClass);

                            Player.ApplyDamageToNPC(n, dashDamage, dashKB, hitContext.HitDirection, rollCrit, hitContext.damageClass, true);
                            if (n.Calamity().dashImmunityTime[Player.whoAmI] < 12)
                                n.Calamity().dashImmunityTime[Player.whoAmI] = 12;

                            Player.GiveImmuneTimeForCollisionAttack(hitContext.PlayerImmunityFrames);
                        }
                    }
                }
            }

            //Speed Blaster
            //if (Player.dashDelay > 0)
            if (CIDashDelay > 0)
            {
                VerticalSpeedBlasterDashTimer = 0;
                LastUsedDashID = CIDashID;
                return;
            }

            //if (CIDashDelay > 0)
            //if (Player.dashDelay > 0)
            if (CIDashDelay > 0)
            {
                VerticalGodslayerDashTimer = 0;
                LastUsedDashID = CIDashID;
                return;
            }

            //if (Player.dashDelay < 1)
            if (CIDashDelay < 0)
            {
                
                //Main.NewText("正在执行 dash 的冷却添加");
                int dashDelayToApply = CIBalancingConstants.UniversalDashCooldown;
                if (UsedDash.CollisionType == DashCollisionType.ShieldSlam)
                    dashDelayToApply = CIBalancingConstants.UniversalShieldSlamCooldown;
                else if (UsedDash.CollisionType == DashCollisionType.ShieldBonk)
                    dashDelayToApply = CIBalancingConstants.UniversalShieldBonkCooldown;
                
                float dashSpeed = 12f;
                float dashSpeedDecelerationFactor = 0.985f;
                float runSpeed = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
                float runSpeedDecelerationFactor = 0.94f;

                LastUsedDashID = CIDashID;

                // Handle mid-dash effects.
                //盾冲朝向排查2025.1.5
                UsedDash.MidDashEffects(Player, ref dashSpeed, ref dashSpeedDecelerationFactor, ref runSpeedDecelerationFactor);
                
                if (UsedDash.IsOmnidirectional && VerticalGodslayerDashTimer < 25)
                {
                    VerticalGodslayerDashTimer++;
                    if (VerticalGodslayerDashTimer >= 25)
                    {
                        Player.dashDelay = dashDelayToApply;//CD
                        // Stop the player from going flying
                        Player.velocity *= 0.2f;
                    }
                }

                if (UsedDash.IsOmnidirectional && VerticalSpeedBlasterDashTimer < 25)
                {
                    VerticalSpeedBlasterDashTimer++;
                    if (VerticalSpeedBlasterDashTimer >= 25)
                    {
                        Player.dashDelay = dashDelayToApply;//CD
                        // Stop the player from going flying
                        Player.velocity *= 0.2f;
                    }
                }
                
                if (HasCustomDash)
                //if (CIDashDelay < 0)
                {
                    Player.vortexStealthActive = false;
                    // Dash delay depends on the type of dash used.
                    //2024.12.25这里应该添加成功了
                    //2024.12.26确实添加成功了
                    //2024.12.26.1.18飞行期间无法正常添加
                    //2024.12.26.1.46飞行期间可以正常添加

                    if (CIDashDelay > -2)
                    {
                        CIDashDelay = 20;//CD帧
                    }
                    
                    // Decide the player's facing direction.

                    //疑似是这一段
                    //就是这一段
                    //if (Player.velocity.X != 0f)
                        //Player.ChangeDir(Math.Sign(Player.velocity.X));
                    

                    
                    // Handle mid-dash movement.
                    if (UsedDash.IsOmnidirectional)
                    {
                        if (Player.velocity.Length() > dashSpeed)
                        {
                            Player.velocity *= dashSpeedDecelerationFactor;
                            return;
                        }
                        if (Player.velocity.Length() > runSpeed)
                        {
                            Player.velocity *= runSpeedDecelerationFactor;
                            return;
                        }
                    }
                    
                    
                    else
                    {
                        if (Player.velocity.X > dashSpeed || Player.velocity.X < -dashSpeed)
                        {
                            Player.velocity.X *= dashSpeedDecelerationFactor;
                            return;
                        }
                        if (Player.velocity.X > runSpeed || Player.velocity.X < -runSpeed)
                        {
                            Player.velocity.X *= runSpeedDecelerationFactor;
                            return;
                        }
                    }
                    
                    Player.dashDelay = dashDelayToApply;
                    
                    if (UsedDash.IsOmnidirectional)
                    {
                        if (Player.velocity.Length() < 0f)
                        {
                            Player.velocity.Normalize();
                            Player.velocity *= -runSpeed;
                            return;
                        }
                        if (Player.velocity.Length() > 0f)
                        {
                            Player.velocity.Normalize();
                            Player.velocity *= runSpeed;
                            return;
                        }
                    }
                    
                    
                    else
                    {//阿斯加德英勇执行的下列代码
                        if (Player.velocity.X < 0f)
                        {
                            Player.velocity.X = -runSpeed;
                            return;
                        }
                        if (Player.velocity.X > 0f)
                        {
                            Player.velocity.X = runSpeed;
                            return;
                        }
                    }
                    
                }
            }
            
            // Handle first-frame effects.
            else if (HasCustomDash && !Player.mount.Active)
            {
                if (DoADash(UsedDash.CalculateDashSpeed(Player)))
                {
                    UsedDash.OnDashEffects(Player);
                    if (CIDashDelay > -2)
                    {
                        CIDashDelay = 20; // 冷却时间
                        //Main.NewText("初始冲刺冷却时间已设置！");
                    }
                }
            }
            
        }
        
        public bool HandleHorizontalDash(out DashDirection direction)
        {
            direction = DashDirection.Directionless;
            bool dashWasExecuted = false;

            // If the manual hotkey is bound, standard Terraria dashes cannot be triggered by double tapping.
            List<string> manualDashHotkeys = CalamityKeybinds.DashHotkey.GetAssignedKeys();
            bool manualHotkeyBound = (manualDashHotkeys?.Count ?? 0) > 0;
            bool pressedManualHotkey = manualHotkeyBound && CalamityKeybinds.DashHotkey.JustPressed;

            int dashDirectionToUse = 0;

            // The manual hotkey is bound. Dashing is controlled solely by this hotkey. Vanilla inputs will not function.
            if (pressedManualHotkey)
            {
                // If you are holding D but not A, then always dash right.
                if (Player.controlRight && !Player.controlLeft)
                    dashDirectionToUse = 1;
                // If you are holding A but not D, then always dash left.
                else if (Player.controlLeft && !Player.controlRight)
                    dashDirectionToUse = -1;

                // If you are holding neither A nor D, or holding both, then dash in the direction the player is moving.
                // If the player is not moving at all, then dash the direction the player is facing.
                else
                {
                    if (MathF.Abs(Player.velocity.X) <= 0.01f)
                        dashDirectionToUse = Player.direction;
                    else
                        dashDirectionToUse = Player.velocity.X > 0f ? 1 : -1;
                }
            }

            // The manual hotkey is not bound. Dashing is controlled via vanilla inputs.
            else if (!manualHotkeyBound)
            {
                // Check whether or not a horizontal dash was declared via vanilla methods this frame.
                bool vanillaLeftDashInput = !manualHotkeyBound && Player.controlLeft && Player.releaseLeft;
                bool vanillaRightDashInput = !manualHotkeyBound && Player.controlRight && Player.releaseRight;
                dashDirectionToUse = vanillaRightDashInput ? 1 : vanillaLeftDashInput ? -1 : 0;
            }


            if (dashDirectionToUse == 1)
            {
                if (dashTimeMod > 0 || pressedManualHotkey)
                {
                    direction = DashDirection.Right;
                    dashWasExecuted = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = 15;
            }
            else if (dashDirectionToUse == -1)
            {
                if (dashTimeMod < 0 || pressedManualHotkey)
                {
                    direction = DashDirection.Left;
                    dashWasExecuted = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = -15;
            }
            return dashWasExecuted;
        }
        public bool HandleOmnidirectionalDash(out DashDirection direction)
        {
            direction = DashDirection.Directionless;
            bool justDashed = false;

            if (Player.controlUp && Player.controlLeft)
            {
                if (dashTimeMod < 0)
                {
                    direction = DashDirection.UpLeft;
                    justDashed = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = -15;
            }
            else if (Player.controlUp && Player.controlRight)
            {
                if (dashTimeMod > 0)
                {
                    direction = DashDirection.UpRight;
                    justDashed = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = 15;
            }
            else if (Player.controlDown && Player.controlLeft)
            {
                if (dashTimeMod < 0)
                {
                    direction = DashDirection.DownLeft;
                    justDashed = true;
                    dashTimeMod = 0;
                    Player.maxFallSpeed = 50f;
                }
                else
                    dashTimeMod = -15;
            }
            else if (Player.controlDown && Player.controlRight)
            {
                if (dashTimeMod > 0)
                {
                    direction = DashDirection.DownRight;
                    justDashed = true;
                    dashTimeMod = 0;
                    Player.maxFallSpeed = 50f;
                }
                else
                    dashTimeMod = 15;
            }
            else if (Player.controlUp)
            {
                if (dashTimeMod < 0)
                {
                    direction = DashDirection.Up;
                    justDashed = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = -15;
            }
            else if (Player.controlDown)
            {
                if (dashTimeMod > 0)
                {
                    direction = DashDirection.Down;
                    justDashed = true;
                    dashTimeMod = 0;
                    Player.maxFallSpeed = 50f;
                }
                else
                    dashTimeMod = 15;
            }
            else if (Player.controlLeft)
            {
                if (dashTimeMod < 0)
                {
                    direction = DashDirection.Left;
                    justDashed = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = -15;
            }
            else if (Player.controlRight)
            {
                if (dashTimeMod > 0)
                {
                    direction = DashDirection.Right;
                    justDashed = true;
                    dashTimeMod = 0;
                }
                else
                    dashTimeMod = 15;
            }
            return justDashed;
        }
        
        
        public bool DoADash(float dashSpeed)
        {
            //Main.NewText("正在执行 doAdash ");
            bool justDashed;
            bool omnidirectionalDash = UsedDash?.IsOmnidirectional ?? false;
            DashDirection direction;

            // Have the dash time incrementally move towards its default state of zero.
            if (dashTimeMod != 0)
                dashTimeMod -= (dashTimeMod > 0).ToDirectionInt();

            // Determine dash times.
            if (omnidirectionalDash)
                justDashed = HandleOmnidirectionalDash(out direction);
            else
                justDashed = HandleHorizontalDash(out direction);

            // Make dash movements happen if ready.
            if (justDashed)
            {
                /*
                Main.NewText("正在执行 doAdash ");
                */
                int totalDirections = 8;
                Vector2[] possibleVelocities = new Vector2[totalDirections];
                for (int i = 0; i < totalDirections; i++)
                    possibleVelocities[i] = -Vector2.UnitY.RotatedBy(MathHelper.TwoPi * i / totalDirections) * dashSpeed;

                switch (direction)
                {
                    // Up Left
                    case DashDirection.UpLeft:
                        Player.velocity = possibleVelocities[7];
                        break;

                    // Down Left
                    case DashDirection.DownLeft:
                        Player.velocity = possibleVelocities[5];
                        break;

                    // Up
                    case DashDirection.Up:
                        Player.velocity = possibleVelocities[0];
                        break;

                    // Left
                    case DashDirection.Left:
                        Player.velocity = omnidirectionalDash ? possibleVelocities[6] : new Vector2(possibleVelocities[6].X, Player.velocity.Y);
                        break;

                    // Nothing
                    case DashDirection.Directionless:
                        break;

                    // Right
                    case DashDirection.Right:
                        Player.velocity = omnidirectionalDash ? possibleVelocities[2] : new Vector2(possibleVelocities[2].X, Player.velocity.Y);
                        break;

                    // Down
                    case DashDirection.Down:
                        Player.velocity = possibleVelocities[4];
                        break;

                    // Down Right
                    case DashDirection.DownRight:
                        Player.velocity = possibleVelocities[3];
                        break;

                    // Up Right
                    case DashDirection.UpRight:
                        Player.velocity = possibleVelocities[1];
                        break;
                }
                // Make any dash movements move to a rapid halt if there are any tiles in the way.
                Point upwardTilePoint = (Player.Center + new Vector2(MathHelper.Clamp((int)direction, -1f, 1f) * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                Point aheadTilePoint = (Player.Center + new Vector2(MathHelper.Clamp((int)direction, -1f, 1f) * Player.width / 2 + 2, 0f)).ToTileCoordinates();
                if (WorldGen.SolidOrSlopedTile(upwardTilePoint.X, upwardTilePoint.Y) || WorldGen.SolidOrSlopedTile(aheadTilePoint.X, aheadTilePoint.Y))
                    Player.velocity.X /= 2f;
                if (CIDashID == "Statis' Void Sash Old")
                {
                    CIDashDelay = -30;
                }
                else
                {
                    CIDashDelay = -18;
                }
            }
            return justDashed;
        }
        public void ModHorizontalMovement()
        {
            if (Player.mount.Active && Player.mount.Type == ModContent.MountType<RimehoundMount>() && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
            {
                Rectangle damageHitbox = Player.getRect();

                if (Player.direction == 1)
                    damageHitbox.Offset(Player.width - 1, 0);

                damageHitbox.Width = 2;
                damageHitbox.Inflate(6, 12);
                float damage = Player.GetTotalDamage<SummonDamageClass>().ApplyTo(50f);
                float knockback2 = 8f;
                int NPCImmuneTime = 30;
                int playerImmuneTime = 6;
                DoMountDashDamage(damageHitbox, damage, knockback2, NPCImmuneTime, playerImmuneTime);
            }

            if (Player.mount.Active && Player.mount.Type == ModContent.MountType<OnyxExcavator>() && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed / 2f)
            {
                Rectangle damageHitbox = Player.getRect();

                if (Player.direction == 1)
                    damageHitbox.Offset(Player.width - 1, 0);

                damageHitbox.Width = 2;
                damageHitbox.Inflate(6, 12);
                float damage = Player.GetTotalDamage<SummonDamageClass>().ApplyTo(25f);
                float knockback2 = 5f;
                int NPCImmuneTime = 30;
                int playerImmuneTime = 6;
                DoMountDashDamage(damageHitbox, damage, knockback2, NPCImmuneTime, playerImmuneTime);
            }
        }

        public int DoMountDashDamage(Rectangle myRect, float Damage, float Knockback, int NPCImmuneTime, int PlayerImmuneTime)
        {
            int totalHurtNPCs = 0;
            foreach (NPC n in Main.ActiveNPCs)
            {
                // Ignore critters with the Guide to Critter Companionship
                if (Player.dontHurtCritters && NPCID.Sets.CountsAsCritter[n.type])
                    continue;

                if (!n.dontTakeDamage && !n.friendly && n.Calamity().dashImmunityTime[Player.whoAmI] <= 0)
                {
                    Rectangle npcHitbox = n.getRect();
                    if (myRect.Intersects(npcHitbox) && (n.noTileCollide || Collision.CanHit(Player.position, Player.width, Player.height, n.position, n.width, n.height)))
                    {
                        int hitDirection = Math.Sign(Player.velocity.X);

                        // Use the player's facing direction as a fallback if they are not making any horizontal movement.
                        if (hitDirection == 0)
                            hitDirection = Player.direction;

                        // TODO -- This should probably use DirectStrike?
                        if (Player.whoAmI == Main.myPlayer)
                            Player.ApplyDamageToNPC(n, (int)Damage, Knockback, hitDirection, false);

                        // 17APR2024: Ozzatron: Dash iframes are not boosted by Cross Necklace at all and are fixed.
                        n.Calamity().dashImmunityTime[Player.whoAmI] = NPCImmuneTime;
                        Player.GiveUniversalIFrames(PlayerImmuneTime, false);

                        totalHurtNPCs++;
                        break;
                    }
                }
            }
            return totalHurtNPCs;
        }
    }
}
