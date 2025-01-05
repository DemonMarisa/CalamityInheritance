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

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 900;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 114;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<AncientStarCI>();
            Item.shootSpeed = 19f;
            Item.rare = ModContent.RarityType<Violet>();
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
