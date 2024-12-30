using CalamityMod.Buffs.Summon;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Summon;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadSummon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 12; //132
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isAuricSetNEW = body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
            bool isAuricSetOLD = body.type == ModContent.ItemType<AuricTeslaBodyArmorold>() && legs.type == ModContent.ItemType<AuricTeslaCuissesold>();
            return isAuricSetNEW || isAuricSetOLD;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            CalamityInheritancePlayer modplayer1 = player.GetModPlayer<CalamityInheritancePlayer>();
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraSummon = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareSummon = true;
            modPlayer.silvaSet = true;
            modPlayer.silvaSummon = true;
            modPlayer.godSlayer = true;

            modplayer1.godSlayerSummonold = true;
            modplayer1.GodSlayerReborn = true;
            modplayer1.silvaSummonEx = true;

            modPlayer.auricSet = true;
            modPlayer.WearingPostMLSummonerSet = true;

            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetDamage<SummonDamageClass>() += 1.2f;
            player.maxMinions += 1;

            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(ModContent.BuffType<SilvaCrystalBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<SilvaCrystalBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SilvaCrystal>()] < 1)
                {
                    var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(3000);
                    var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 3000;
                }

                if (player.FindBuffIndex(ModContent.BuffType<Mechworm>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<Mechworm>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<MechwormHead>()] < 1)
                {
                    int owner = player.whoAmI;
                    int typeHead = ModContent.ProjectileType<MechwormHead>();
                    int typeBody = ModContent.ProjectileType<MechwormBody>();
                    int typeBody2 = ModContent.ProjectileType<MechwormBody>();
                    int typeTail = ModContent.ProjectileType<MechwormTail>();
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (Main.projectile[i].type == typeHead || Main.projectile[i].type == typeTail || Main.projectile[i].type == typeBody ||
                                Main.projectile[i].type == typeBody2)
                            {
                                Main.projectile[i].Kill();
                            }
                        }
                    }
                    int maxMinionScale = player.maxMinions;
                    if (maxMinionScale > 10)
                    {
                        maxMinionScale = 10;
                    }
                    float summonDamage = player.GetDamage(DamageClass.Summon).Additive + player.GetDamage(DamageClass.Summon).Multiplicative;
                    int damage = (int)(35 * ((summonDamage * 5 / 3) + (summonDamage * 0.46f * (maxMinionScale - 1))));
                    Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                    Vector2 value = Vector2.UnitX.RotatedBy((double)player.fullRotation, default);
                    Vector2 vector3 = Main.MouseWorld - vector2;
                    float velX = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                    float velY = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (player.gravDir == -1f)
                    {
                        velY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                    }
                    float dist = (float)Math.Sqrt((double)(velX * velX + velY * velY));
                    if ((float.IsNaN(velX) && float.IsNaN(velY)) || (velX == 0f && velY == 0f))
                    {
                        velX = (float)player.direction;
                        velY = 0f;
                        dist = 10f;
                    }
                    else
                    {
                        dist = 10f / dist;
                    }
                    velX *= dist;
                    velY *= dist;
                    int head = -1;
                    int tail = -1;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (head == -1 && Main.projectile[i].type == typeHead)
                            {
                                head = i;
                            }
                            else if (tail == -1 && Main.projectile[i].type == typeTail)
                            {
                                tail = i;
                            }
                            if (head != -1 && tail != -1)
                            {
                                break;
                            }
                        }
                    }
                    if (head == -1 && tail == -1)
                    {
                        float num77 = Vector2.Dot(value, vector3);
                        if (num77 > 0f)
                        {
                            player.ChangeDir(1);
                        }
                        else
                        {
                            player.ChangeDir(-1);
                        }
                        velX = 0f;
                        velY = 0f;
                        vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                        vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                        int curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormHead>(), damage, 1, owner);

                        int prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), damage, 1, owner, (float)prev);

                        prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), damage, 1, owner, (float)prev);
                        Main.projectile[prev].localAI[1] = (float)curr;
                        Main.projectile[prev].netUpdate = true;

                        prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormTail>(), damage, 1, owner, (float)prev);
                        Main.projectile[prev].localAI[1] = (float)curr;
                        Main.projectile[prev].netUpdate = true;
                    }
                    else if (head != -1 && tail != -1)
                    {
                        int body = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), damage, 1, owner, Main.projectile[tail].ai[0]);
                        int back = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), damage, 1, owner, (float)body);

                        Main.projectile[body].localAI[1] = (float)back;
                        Main.projectile[body].ai[1] = 1f;
                        Main.projectile[body].minionSlots = 0f;
                        Main.projectile[body].netUpdate = true;

                        Main.projectile[back].localAI[1] = (float)tail;
                        Main.projectile[back].netUpdate = true;
                        Main.projectile[back].minionSlots = 0f;
                        Main.projectile[back].ai[1] = 1f;

                        Main.projectile[tail].ai[0] = (float)back;
                        Main.projectile[tail].netUpdate = true;
                        Main.projectile[tail].ai[1] = 1f;
                    }
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            modPlayer.auricBoost = true;
            player.maxMinions += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadSummon>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
