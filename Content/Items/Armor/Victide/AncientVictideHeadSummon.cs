using CalamityMod.CalPlayer;
using CalamityMod.Buffs.Summon;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.Victide
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientVictideHeadSummon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.defense = 1; //8
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == 
                   ModContent.ItemType<AncientVictideBreastplate>()&& 
                   legs.type == 
                   ModContent.ItemType<AncientVictideLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.victideSet = true;
            modPlayer.victideSummoner = true;
            player.maxMinions++;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<SeaSnailBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<SeaSnailBuff>(), 3600, true);
                }
                var source = player.GetSource_ItemUse(Item);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<VictideSeaSnail>()] < 1)
                {
                    // 08DEC2023: Ozzatron: Victide Sea Snails spawned with Old Fashioned active will retain their bonus damage indefinitely. Oops. Don't care.
                    var baseDamage = player.ApplyArmorAccDamageBonusesTo(7);
                    var minionDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);

                    var p = Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ModContent.ProjectileType<VictideSeaSnail>(), minionDamage, 0f, Main.myPlayer, 0f, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = baseDamage;
                }
            }
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.GetDamage<SummonDamageClass>() += 0.1f;
                player.lifeRegen += 3;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(4).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
