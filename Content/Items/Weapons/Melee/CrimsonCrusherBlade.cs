using CalamityMod.CalPlayer;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    internal class CrimsonCrusherBlade : CIMelee, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 70;
            Item.height = 80;
            Item.scale = 1.25f;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(7))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
            if (target.damage > 0 && hit.Crit && !CalamityPlayer.areThereAnyDamnBosses)
            {
                target.damage = target.defDamage - 5;
                if (target.damage < 1)
                    target.damage = 1;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Ichor, 120);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BlightedGel>(15).
                AddIngredient(ItemID.CrimstoneBlock, 50).
                AddIngredient(ItemID.TissueSample, 5).
                AddRecipeGroup(RecipeGroupID.IronBar, 4).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
