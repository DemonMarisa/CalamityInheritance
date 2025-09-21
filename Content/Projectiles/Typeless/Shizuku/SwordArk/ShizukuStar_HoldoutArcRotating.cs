
// using Microsoft.Xna.Framework;
// using Terraria;
// using Terraria.ModLoader;

// namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
// {
//     public class ShizukuStarHoldoutAI : ModProjectile, ILocalizedModType
//     {
//         #region 一个比较顺滑的，圆弧运动示例
//         /// <summary>
//         /// 射弹锚点“前一次更新”的位置，初始设定为Vector.Zero，在下一帧会被瞬间更新
//         /// </summary>
//         private Vector2 _lastAnchorPosition = Vector2.Zero;
//         /// <summary>
//         /// 标记射弹是否开始“绕圆弧旋转”
//         /// </summary>
//         private bool _isArcRotating = false;
//         /// <summary>
//         /// 存储射弹进入圆弧运动前的转角
//         /// </summary>
//         private float _arcStartRotation;
//         private Vector2 _rotCenter;
//         /// <summary>
//         /// 总圆弧的运动角
//         /// </summary>
//         private readonly float TotalArcAngle = MathHelper.ToRadians(360f);
//         /// <summary>
//         /// 存储射弹开始绕圆弧旋转前的原始速度
//         /// </summary>
//         private float _originalSpeed;

//         /// <summary>
//         /// 绕圆弧的旋转次数
//         /// </summary>
//         private int _drawArcTime = 0;
//         /// <summary>
//         /// 总圆弧绘制时间
//         /// </summary>
//         private const float ArcDuration = 50f;
//         /// <summary>
//         /// 圆弧半径
//         /// </summary>
//         private const float ArcRadius = 12 * 16;
//         #endregion
//         #region ArcRotation
//         private void DrawDynamicArc()
//         {
//             //平滑玩家自身位置，因为玩家自身是一个不断位移的单位
//             _rotCenter = Vector2.Lerp(_lastAnchorPosition, Owner.Center, 0.2f);
//             //刷新记录位置
//             _lastAnchorPosition = _rotCenter;
//             //初始化一次圆弧情况
//             if (!_isArcRotating)
//             {
//                 //存入初始转角
//                 _arcStartRotation = Projectile.velocity.ToRotation();
//                 //存入原始速度
//                 _originalSpeed = Projectile.velocity.Length();
//                 _isArcRotating = true;
//             }
            
//             if (_isArcRotating)
//             {
//                 AttackTimer += 1;
//                 float progress = AttackTimer / ArcDuration;
//                 float curAngle = TotalArcAngle * progress;
//                 float orbitAngle = _arcStartRotation + curAngle;
//                 //跟随转角
//                 Projectile.rotation = orbitAngle;

//                 //切线方向
//                 Vector2 radiusDir = (Projectile.Center - _rotCenter).SafeNormalize(Vector2.Zero);
//                 Vector2 wantedTanDir = radiusDir.RotatedBy(MathHelper.PiOver2);
//                 //实际调整速度
//                 Vector2 targetVelocity = wantedTanDir * _originalSpeed * 0.8f;
//                 //接近圆弧终点时开始向最终朝向过渡
//                 Vector2 finalDir = orbitAngle.ToRotationVector2();
//                 float transitionStart = 0.8f;
//                 if (progress >= transitionStart && progress <= 1.0f)
//                 {
//                     //过渡因子
//                     float transitionFactor = (progress - transitionStart) / (1.0f - transitionStart);
//                     //从切线方向平滑过渡到最终朝向
//                     targetVelocity = Vector2.Lerp(targetVelocity, finalDir * _originalSpeed * 0.8f, transitionFactor);
//                 }
//                 Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.2f);
//                 //修正半径
//                 float curRadius = (Projectile.Center - _rotCenter).Length();
//                 float radiusError = curRadius - ArcRadius;
//                 Projectile.velocity -= radiusDir * radiusError * 0.1f;
//                 //将1的角度抽象化100%进程，并让他每当进程33%就发射一次特殊射弹

//                 if (progress > 1)
//                 {
//                     AttackTimer = 0;
//                     //自增
//                     _drawArcTime++;
//                     _isArcRotating = false;
//                 }
//             }
//         }
//         #endregion
//     }
// }