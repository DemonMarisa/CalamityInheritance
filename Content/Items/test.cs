using CalamityInheritance.CIPlayer;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
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
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<NanoFlare>();
            Item.shootSpeed = 5f;
        }
        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            player.RemoveCooldown(GodSlayerCooldown.ID);
            modPlayer1.CIDashDelay = -100;
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
