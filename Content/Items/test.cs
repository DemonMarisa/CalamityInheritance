using CalamityInheritance.CIPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon.SmallAresArms;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<CISporeGas>();
            Item.shootSpeed = 12f;
        }
        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            player.RemoveCooldown(GodSlayerCooldown.ID);

            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade);
            }
        }
    }
}
