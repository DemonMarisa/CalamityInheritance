using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{

	public class ExoSpearProjNor : ModProjectile
	{
		private int increment;

		private int splits;

		private int phase;

		private int phasecounter;

		private NPC teleportTarget;

		private int penetrates = 5;

		private int teleportticks = 32;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 720;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
		}

		public void Explode(float StartAngle, int Streams, float ProjSpeed)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < Streams; i++)
				{
					Vector2 vector = Utils.RotatedBy(Vector2.Normalize(new Vector2(1f, 1f)), (double)MathHelper.ToRadians(360 / Streams * i + StartAngle), default(Vector2));
					vector.X *= ProjSpeed;
					vector.Y *= ProjSpeed;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector.X, vector.Y, ModContent.ProjectileType<ExoSpearTrailNor>(), Projectile.damage / 12, 0, Main.myPlayer, 0f, 0f);
				}
			}
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.rotation = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(135f);
			if (phase == 1)
			{
				teleportticks -= 2;
				if (teleportticks < 1 && Projectile.owner == Main.myPlayer)
				{
					Vector2 vector = new Vector2(400f, 400f);
					vector = Utils.RotatedByRandom(vector, (double)MathHelper.ToRadians(360f));
					Vector2 vector2 = ((Entity)teleportTarget).position + vector;
					Vector2 vector3 = ((Entity)teleportTarget).position - vector2;
					vector3.Normalize();
					vector3.X *= 12f;
					vector3.Y *= 12f;
					for (int i = 0; i < 40; i++)
					{
						Dust.NewDust(vector2, 20, 20, DustID.PlatinumCoin, vector3.X / 2f, vector3.Y / 2f, 0, default(Color), 1f);
					}
					Projectile.position = vector2;
					Projectile.velocity = vector3;
					phase = 0;
					if (player.Calamity().StealthStrikeAvailable())
					{
						Explode(Utils.NextFloat(Main.rand, (float)Math.PI * 2f), 4, 16f);
					}
					Projectile.netUpdate = true;
				}
			}
			else
			{
				increment++;
				teleportticks++;
			}
			if (penetrates >= 5)
			{
				Dust.NewDustPerfect(Projectile.Center, 247, (Vector2?)new Vector2(0f, 0f), 0, default(Color), 1f);
			}
			if (increment >= 6 && penetrates >= 5 && phase == 0 && splits <= 12)
			{
				Vector2 vector4 = Utils.RotatedBy(Projectile.velocity, (double)MathHelper.ToRadians(120f), default(Vector2));
				Vector2 vector5 = Utils.RotatedBy(Projectile.velocity, (double)MathHelper.ToRadians(240f), default(Vector2));
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, vector4.X, vector4.Y, ModContent.ProjectileType<ExoSpearTrailNor>(), (int)((double)((ModProjectile)this).Projectile.damage * 0.075), ((ModProjectile)this).Projectile.knockBack, ((ModProjectile)this).Projectile.owner, 0f, 0f);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, vector5.X, vector5.Y, ModContent.ProjectileType<ExoSpearTrailNor>(), (int)((double)Projectile.damage * 0.075), ((ModProjectile)this).Projectile.knockBack, ((ModProjectile)this).Projectile.owner, 0f, 0f);
					increment = 0;
				}
				splits++;
			}
			if (phase == 0 && Projectile.ai[1] == 1f && penetrates < 5)
			{
				Vector2 velocity = ((Entity)teleportTarget).position - Projectile.Center;
				velocity.Normalize();
				velocity.X *= 12f;
				velocity.Y *= 12f;
				Projectile.velocity = velocity;
				if (!((Entity)teleportTarget).active)
				{
					Projectile.Kill();
				}
			}
			Projectile.alpha = 255 - teleportticks * 8;
			Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			_ = TextureAssets.Projectile[Projectile.type].Value.Height;
			Rectangle rectangle = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
			Vector2 vector = Utils.Size(rectangle) / 2f;
			vector.X = 28f;
			vector.Y = 35f;
			Color alpha = Projectile.GetAlpha(lightColor);
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, ((ModProjectile)this).Projectile.gfxOffY), rectangle, alpha, ((ModProjectile)this).Projectile.rotation, vector, ((ModProjectile)this).Projectile.scale, effects, 0f);
			Vector2 origin = new Vector2((float)TextureAssets.Projectile[((ModProjectile)this).Projectile.type].Value.Width * 0.5f, (float)TextureAssets.Projectile[((ModProjectile)this).Projectile.type].Value.Height * 0.5f);
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 position = Projectile.oldPos[i] - Main.screenPosition + vector;
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, default(Rectangle), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			}
			CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			if (penetrates == 5)
			{
				teleportTarget = target;
			}
			if (target == teleportTarget)
			{
				penetrates--;
				teleportticks = 32;
				phase = 1;
				for (int i = 0; i < 80; i++)
				{
					Dust.NewDust(Projectile.position, 20, 20, DustID.PlatinumCoin, Projectile.velocity.X, Projectile.velocity.Y / 4f, 0, default(Color), 1f);
				}
			}
			if (penetrates == 0)
			{
				Projectile.Kill();
			}
		}
	}
}
