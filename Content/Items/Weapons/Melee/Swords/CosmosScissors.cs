using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Particles;
using CalamityInheritance.Rarity.Special.RarityShiny;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    /// <summary>
    /// 绑定玩家类是必要的，因为我需要的是让玩家对着物品右键启用的切换
    /// </summary>
    public class ScissorsToCalamity : ModPlayer
    {
        public override void PostUpdate()
        {
            bool isLegal = Main.mouseMiddle && Main.playerInventory;
            if (!isLegal)
                return;
            if (!Main.mouseMiddleRelease)
                return;
            if(Player.HeldItem.type == ItemType<ArkoftheCosmos>())
                SwitchWeapons(ItemType<CosmosScissors>());
            else if (Player.HeldItem.type == ItemType<CosmosScissors>())
                SwitchWeapons(ItemType<ArkoftheCosmos>());
        }
        public void SwitchWeapons(int nextType)
        {
            //过一个物品是否为moditem的判定，不过应该也不需要。
            if (Player.HeldItem.ModItem == null)
                return;
            //实例化，
            int prefixID = Player.HeldItem.prefix;
            Item scissor = new Item();
            scissor.SetDefaults(nextType);
            //过一个判断，因为原灾的方舟并不存在传奇词缀，但我们移植的方舟是有的
            //因此如果这里词缀为恶魔/神级，转为传奇。
            //???
            int prefix = prefixID;
            if (nextType == ItemType<CosmosScissors>())
            {
                if (prefixID == PrefixID.Godly || prefixID == PrefixID.Demonic)
                    prefix = PrefixID.Legendary;
            }
            else
            {
                if (prefixID == PrefixID.Legendary)
                    prefix = PrefixID.Godly;

            }
            scissor.Prefix(prefix);
            //直接做掉玩家当前手持的方舟。
            Player.inventory[Player.selectedItem] = scissor;
            //粒子。
            for (int i =0;i< 30;i++)
            {
                Vector2 spawnPos = Player.MountedCenter + Main.rand.NextVector2Circular(30f, 30f);
                Vector2 speed = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(0.12f, 3.24f) * 1.5f;
                new ShinyOrb(spawnPos, speed, Color.White, 40, Main.rand.NextFloat(0.40f, 0.70f)).Spawn();
            }
            SoundEngine.PlaySound(SoundID.ResearchComplete, Player.Center);
        }
    }
    public class ModifySomeTooltip : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemType<CosmosScissors>() || item.type == ItemType<ArkoftheCosmos>())
            {
                string textValue = Mod.GetLocalizationKey($"Content.Items.Weapons.Melee.{nameof(CosmosScissors)}.SwitchTooltip").ToLangValue();
                var line = new TooltipLine(Mod, "SwitchItemText", textValue);
                if (tooltips.Count is 0)
                    tooltips.Add(line);
                else
                    tooltips.Insert(tooltips.Count, line);
            }
        }
    }
    /// <summary>
    /// 剪刀的代码基本上没有变动，特效粒子也是
    /// 主要问题是这里的粒子处理太过于神经病了，而且我也不想继续在这个方面深入
    /// 反正不改了。
    /// </summary>
    public class CosmosScissors : ModItem,ILocalizedModType
    {
        public override string Texture => GetInstance<ArkoftheCosmos>().Texture;
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        /// <summary>
        /// 下面这堆全是与方舟弹幕没有有关的参数
        /// 说实话除了一些必要保留的，大部分都没有必要，而且反而让可读性变得更那啥了
        /// </summary>
        public float Charge = 0f;
        public float Combo = 0f;
        public static float NeedleDamageMultiplier = 0.7f; //Damage on the non-homing needle projectile
        public static float MaxThrowReach = 650;
        public static float snapDamageMultiplier = 1.4f; //Extra damage from making the scissors snap

        public static float chargeDamageMultiplier = 1.4f; //Extra damage from charge
        public static float chainDamageMultiplier = 0.1f;

        public static int DashIframes = 10;
        public static float SlashBoltsDamageMultiplier = 0.2f;
        public static float SnapBoltsDamageMultiplier = 0.1f;

        public static float blastDamageMultiplier = 0.5f; //Damage multiplier applied ontop of the charge damage multiplier mutliplied by the amount of charges consumed. So if you consume 5 charges, the blast will get multiplied by 5 times the damage multiplier
        public static float blastFalloffSpeed = 0.1f; //How much the blast damage falls off as you hit more and more targets
        public static float blastFalloffStrenght = 0.75f; //Value between 0 and 1 that determines how much falloff increases affect the damage : Closer to 0 = damage falls off less intensely, closer to 1 : damage falls off way harder

        public static float SwirlBoltAmount = 6f; //The amount of cosmic bolts produced during hte swirl attack
        public static float SwirlBoltDamageMultiplier = 0.7f; //This is the damage multiplier for ALL THE BOLTS: Aka, said damage multiplier is divided by the amount of bolts in a swirl and the full damage multiplier is gotten if you hit all the bolts

        public override void SetDefaults()
        {
            Item.width = Item.height = 136;
            Item.damage = 2100;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9.5f;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 28f;
            Item.rare = RarityType<ArkRarity>();
        }
        public override bool MeleePrefix() => true;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (tooltips == null)
                return;

            Player player = Main.LocalPlayer;
            if (player is null)
                return;
            //不开玩笑的说我觉得这里的tooltip写的非常丑陋。
            var comboTooltip = tooltips.FirstOrDefault(x => x.Text.Contains("[COMBO]") && x.Mod == "Terraria");
            if (comboTooltip != null)
            {
                comboTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("ComboInfo"));
                comboTooltip.OverrideColor = Color.Lerp(Color.Gold, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }

            var parryTooltip = tooltips.FirstOrDefault(x => x.Text.Contains("[PARRY]") && x.Mod == "Terraria");
            if (parryTooltip != null)
            {
                parryTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("ParryInfo"));
                parryTooltip.OverrideColor = Color.Lerp(Color.Cyan, Color.DeepSkyBlue, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.75f);
            }

            var blastTooltip = tooltips.FirstOrDefault(x => x.Text.Contains("[BLAST]") && x.Mod == "Terraria");
            if (blastTooltip != null)
            {
                blastTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("BlastInfo"));
                blastTooltip.OverrideColor = Color.Lerp(Color.HotPink, Color.Crimson, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.625f);
            }
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if(line.Name == "ItemName" && line.Mod == "Terraria")
            {
                ArkRarity.DrawRarity(line);
                return false;
            }
            //这一条是为了绘制专门的物理顶点。
            //一定程度上是硬编码，不过我也不打算继续浪费时间去想办法做适配实现
            if(line.Name == "Tooltip3" && line.Mod == "Terraria")
            {
                ArkRarity.DrawPhysicalLine(line);
                return false;
            }
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 15;

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;

            if (CanUseItem(player) && Combo != 4)
                Item.channel = false;

            if (Combo == 4)
                Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any(n => n.active && n.owner == player.whoAmI && n.type == ProjectileType<CosmosScissorsSwingBlade>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (Charge > 0 && player.controlUp)
                {
                    float angle = velocity.ToRotation();
                    Projectile.NewProjectile(source, player.Center + angle.ToRotationVector2() * 90f, velocity, ProjectileType<CosmosScissorsBlast>(), (int)(damage * Charge * chargeDamageMultiplier * blastDamageMultiplier), 0, player.whoAmI, Charge);

                    if (Main.LocalPlayer.Calamity().GeneralScreenShakePower < 3)
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 3;

                    Charge = 0;
                }
                else if (!Main.projectile.Any(n => n.active && n.owner == player.whoAmI && (n.type == ProjectileType<CosmosScissorsParry>() || n.type == ProjectileType<ArkoftheCosmosParryHoldout>())))
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<CosmosScissorsParry>(), damage, 0, player.whoAmI, 0, 0);

                return false;
            }

            if (Charge > 0)
                damage = (int)(chargeDamageMultiplier * damage);

            float scissorState = Combo == 4 ? 2 : Combo % 2;

            Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<CosmosScissorsSwingBlade>(), damage, knockback, player.whoAmI, scissorState, Charge);


            //Shoot projectiles
            if (scissorState != 2)
            {
                Projectile.NewProjectile(source, player.Center + velocity.SafeNormalize(Vector2.Zero) * 20, velocity * 1.4f, ProjectileType<RendingNeedle>(), (int)(damage * NeedleDamageMultiplier), knockback, player.whoAmI);
            }

            Combo += 1;
            if (Combo > 4)
                Combo = 0;

            Charge--;
            if (Charge < 0)
                Charge = 0;

            return false;
        }

        public override ModItem Clone(Item item)
        {
            var clone = base.Clone(item);

            if (clone is CosmosScissors a && item.ModItem is CosmosScissors a2)
                a.Charge = a2.Charge;

            return clone;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Charge);
        }

        public override void NetReceive(BinaryReader reader)
        {
            Charge = reader.ReadSingle();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D handleTexture = Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ArkoftheCosmosHandle").Value;
            Texture2D bladeTexture = Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ArkoftheCosmosGlow").Value;

            float bladeOpacity = Charge > 0 ? 1f : MathHelper.Clamp((float)Math.Sin(Main.GlobalTimeWrappedHourly % MathHelper.Pi) * 2f, 0, 1) * 0.7f + 0.3f;

            spriteBatch.Draw(handleTexture, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f); //Make the back scissor slightly transparent if the ark isnt charged
            spriteBatch.Draw(bladeTexture, position, null, drawColor * bladeOpacity, 0f, origin, scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Charge <= 0)
                return;

            var barBG = Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarBack").Value;
            var barFG = Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarFront").Value;

            float barScale = 3f;
            Vector2 barOrigin = barBG.Size() * 0.5f;
            float xOffset = frame.Width * 0.25f - barBG.Width * 0.5f;
            float yOffset = frame.Height * 0.7f - barBG.Height;
            Vector2 drawPos = position + Vector2.UnitX * scale * xOffset + Vector2.UnitY * scale * yOffset;
            Rectangle frameCrop = new Rectangle(0, 0, (int)(Charge / 10f * barFG.Width), barFG.Height);
            Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1, 1, 0.75f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f);

            spriteBatch.Draw(barBG, drawPos, null, color, 0f, barOrigin, scale * barScale, 0f, 0f);
            spriteBatch.Draw(barFG, drawPos, frameCrop, color * 0.8f, 0f, barOrigin, scale * barScale, 0f, 0f);
        }
        //移除了合成表。
        //这里的获取方式改为了对着方舟物品的右键
        //原灾的那个。
        //public override void AddRecipes()
        //{
        //    CreateRecipe().
        //        AddIngredient<FourSeasonsGalaxia>().
        //        AddIngredient<ArkoftheElements>().
        //        AddIngredient<AuricBar>(5).
        //        AddTile<CosmicAnvil>().
        //        Register();
        //}
    }
}
