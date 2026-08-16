using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (FungalSymbiote && Player.whoAmI == Main.myPlayer)
            {
                if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1) ||
                    Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3) ||
                    Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5) ||
                    Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7) ||
                    Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                {
                    float yVel = 0f;
                    float xVel = 0f;
                    float yOffset = 0f;
                    float xOffset = 0f;
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                    {
                        yVel = -7f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                    {
                        yVel = -6f;
                        xVel = 2f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5))
                    {
                        yVel = -4f;
                        xVel = 4f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                    {
                        yVel = -2f;
                        xVel = 6f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                    {
                        xVel = 7f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                    {
                        xOffset = 26f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                    {
                        xOffset -= 4f;
                        yOffset -= 20f;
                    }
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                    {
                        yOffset += 6f;
                    }
                    if (Player.direction == -1)
                    {
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                        {
                            xOffset -= 8f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                        {
                            xOffset -= 6f;
                        }
                    }
                    yVel *= 1.5f;
                    xVel *= 1.5f;
                    xOffset *= Player.direction;
                    yOffset *= Player.gravDir;
                    int damage = (int)(Player.GetWeaponDamage(Player.ActiveItem()) * 0.25f);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), hitbox.X + hitbox.Width / 2 + xOffset, hitbox.Y + hitbox.Height / 2 + yOffset, Player.direction * xVel, yVel * Player.gravDir, ProjectileID.Mushroom, damage, 0f, Player.whoAmI);
                }
            }
        }
    }
}
