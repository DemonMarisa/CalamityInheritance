using CalamityMod.NPCs.Providence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class ColdheartIcicle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Coldheart Icicle");
            // Tooltip.SetDefault("Drains a percentage of enemy health on hit\nCannot inflict critical hits");
        }
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Melee;
            Item.width = 26;
            Item.height = 26;
            Item.useAnimation = Item.useTime = 12;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.shoot = ModContent.ProjectileType<ColdheartIcicleProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Pink;
        }
        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SetMaxDamage(target.statLifeMax2 * 2 / 100);
            target.statDefense -= (int)target.statDefense;
            target.endurance = 0f;
        }

        // LATER -- Providence specifically is immune to Coldheart Icicle. There is probably a better way to do this
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
        }
    }
}
