using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Content.Items.Armor.Xeroc;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class AncientXerocWings: CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Wings";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:22,
            itemHeight:20,
            itemRare:ItemRarityID.Red,
            itemValue:CIShopValue.RarityPriceRed
        );
        public override void ExSSD()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
            Type.ShimmerEach<ExodusWings>(false);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bool activeXeroc = player.statLife <= player.statLifeMax2 * 0.15f;
            if(player.armor[0].type == ModContent.ItemType<AncientXerocMask>() && 
               player.armor[1].type == ModContent.ItemType<AncientXerocPlateMail>() &&
               player.armor[2].type == ModContent.ItemType<AncientXerocCuisses>() &&
               activeXeroc
               )
            {
                //Scarlet: 这次真的抵消掉伤害惩罚了
                player.buffImmune[ModContent.BuffType<AncientXerocShame>()] = true;
                player.GetDamage<GenericDamageClass>() += 0.5f;
                player.GetCritChance<GenericDamageClass>() += 50;
            }

            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                float xOffset = 4f;
                if (player.direction == 1)
                {
                    xOffset = -40f;
                }
                int index = Dust.NewDust(new Vector2(player.Center.X + xOffset, player.Center.Y - 15f), 30, 30, DustID.PurpleTorch, 0f, 0f, 100, default, 2.4f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.3f;
                if (Main.rand.NextBool(10))
                {
                    Main.dust[index].fadeIn = 2f;
                }
                Main.dust[index].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
            }
            player.wingTimeMax = 180;
            player.noFallDmg = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient<NebulaBar>(9).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
