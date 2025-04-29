using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq.Expressions;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    /*操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    *操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
    */
    public class CryogenPtr : ModProjectile, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public int AttackTimer = -1;
        //冰刺是否在玩家身上旋转
        public bool Idle = true;
        //冰刺是否在敌怪周围旋转
        public bool Rounding = true;
        //右键冰刺距离敌怪的距离
        public float FloatyDist = 90f;
        public bool PingWhip = false;
        public NPC tar = null;
        const float RegulaPtr = 1f;
        const float IfRightClickPtr = 2f;
        const float BuffColdPointer = 1f;
        const int T1CD = 75;
        const int NOT1CD = 180;
        #region 别名
        public ref float AttackAngle => ref Projectile.ai[0];
        public ref float AttackType => ref Projectile.ai[1];
        public ref float AttackBuffer => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        public bool OnTier1 => Owner.CIMod().ColdDivityTier1;
        #endregion
        //这几个攻击方法为什么不写在ai数组里面而是外置？
        //为什么攻击方法跟计时器用的是同一个数组位？
        //这个攻击目标为什么也是外置而不是写在ai数组里面？
        //为什么左键射弹逻辑和右键射弹逻辑必须得写在一个文件里面？
        //为什么必须往AI封一堆超大缩进的ifesle也不愿意封装？
        //你们要几把干嘛？
        //操你妈灾厄
        //操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄操你妈灾厄
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 60;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        //写AI
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AttackTimer);
            writer.Write(Rounding);
            writer.Write(Idle);
            writer.Write(FloatyDist);
            writer.Write(PingWhip);
            writer.Write(tar is null ? -1 : tar.whoAmI);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AttackTimer = reader.ReadInt32();
            Rounding = reader.ReadBoolean();
            Idle = reader.ReadBoolean();
            PingWhip = reader.ReadBoolean();    
            int realTar = reader.ReadInt32();
            tar = realTar == -1 ? null : Main.npc[realTar];
        }
        /*
        *我要去杀了所有写代码不写注释的人的亲妈
        *这串代码中，ai[1]用于表示是否为右键功能的射弹标记，如果ai[1] == 2f, 则右键射弹， ai[1] == 1f, 则是左键常规的射弹逻辑, 需注意
        *操你妈灾厄
        *挂机射弹（转在玩家上），常规追踪射弹（转在玩家上一段时间后发射）
        */
        public override bool PreAI()
        {
            int fireCD = OnTier1 ? T1CD : NOT1CD; 
            //常规射弹：如果是常规的射弹，则发射cd重置为10
            //计时器初始为-1.
            if (AttackTimer == -1)
            {
                //如果仅仅刚刚生成，则重设锭攻击CD。
                AttackTimer = AttackType == 0f? fireCD : 0;
                // NewDust(30);
            }
            if (AttackType == RegulaPtr && Projectile.timeLeft > 1000)
            {
                AttackType = 0f;
                Projectile.timeLeft = 200;
                Rounding = Idle = false;
                Projectile.netUpdate = true;
            }
            else if (AttackType >= IfRightClickPtr && Projectile.timeLeft > 900)
            {
                tar = CalamityUtils.MinionHoming(Projectile.Center, 1000f, Main.player[Projectile.owner]);
                if (tar != null)
                {
                    //?
                    Projectile.timeLeft = 669;
                    AttackType++;
                    Idle = false;
                    float height = tar.getRect().Height;
                    float width = tar.getRect().Width;
                    FloatyDist = MathHelper.Min((height > width ? height : width) * 3f, Main.LogicCheckScreenWidth * Main.LogicCheckScreenHeight / 2);
                    if (FloatyDist > Main.LogicCheckScreenWidth /3)
                        FloatyDist = Main.LogicCheckScreenWidth;
                    Projectile.penetrate = -1;
                    Projectile.usesIDStaticNPCImmunity = true;
                    Projectile.idStaticNPCHitCooldown = 4;
                    Projectile.netUpdate = true;
                }
            }
            if (Idle)
            {
                ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
                if (Projectile.penetrate == 1)
                    Projectile.penetrate++;
            }
            //执行AI
            return true;
        }
        public override void AI()
        {
            var mplr = Owner.CIMod();
            var src = Projectile.GetSource_FromThis();
            int fireCD = OnTier1 ? T1CD : NOT1CD;
            //标记是否正在启用召唤物用
            if (Owner.dead)
                mplr.IsColdDivityActiving = false;
            if (!mplr.IsColdDivityActiving)
            {
                Projectile.active = false;
                return;
            }
            
            //如果在玩家周围待机，占用召唤栏
            if (Idle)
            {
                Projectile.minionSlots = 1f;
                Projectile.timeLeft = 2;
                if (!mplr.IsColdDivityActiving && AttackTimer > 0)
                    Projectile.Kill();
            }
            //如果在敌怪周围，视情况而定干掉射弹
            if (!Idle && Rounding)
                if (tar != null && (!tar.active || tar.life <= 0))
                    Projectile.Kill();
            //重新部署的时间
            if (AttackTimer > 0)
            {
                AttackTimer--;
                if (AttackTimer == 0)
                {
                    //释放一些粒子
                    SoundEngine.PlaySound(SoundID.Item30 with {Pitch = 0.2f}, Projectile.position);
                    Projectile.netUpdate = true;
                }
            }
            //实际执行的AI
            //常规射弹因为Rounding=false， idle=false，所以下方的AI都不会执行
            if (Rounding)
            {
                //如果没在玩家周围转圈, 去寻找一个目标
                if (Rounding && !Idle && Projectile.timeLeft < 120)
                {
                    AttackTimer = 0;
                    Projectile.usesIDStaticNPCImmunity = false;
                    Projectile.penetrate = 1;
                    float appDist = tar.getRect().Width > tar.getRect().Height ? tar.getRect().Width : tar.getRect().Height;
                    if (Projectile.timeLeft > 60)
                        FloatyDist += 5;
                    else
                        FloatyDist -= 10;
                }
                //在玩家周围转圈
                if (Idle)
                {
                    //?
                    float num = AttackTimer == 0 ? 60f : (300 - AttackTimer) /3;
                    float stdDist = num > 60f ? 60f : num;
                    //取玩家中心点，绕着玩家转
                    Projectile.Center = Owner.Center + AttackAngle.ToRotationVector2() * stdDist;
                    //转角，别忘了
                    Projectile.rotation = AttackAngle + (float)Math.Atan(90);
                    AttackAngle -= MathHelper.ToRadians(4f);
                    //寻找附近的单位，如果找到了则发射弹幕
                    NPC aliveTar = AttackTimer > 0 ? null : CalamityUtils.MinionHoming(Projectile.Center, 800f, Owner);
                    //如果单位不为空，则发射弹幕
                    if (aliveTar != null && Projectile.owner == Main.myPlayer)
                    {
                        AttackTimer = fireCD;
                        Vector2 vel = AttackAngle.ToRotationVector2().RotatedBy(Math.Atan(0));
                        vel.Normalize();
                        vel *= 30f;
                        if (Projectile.CalamityInheritance().PingPointerT3)
                            AttackBuffer = BuffColdPointer;
                        int s = Projectile.NewProjectile(src, Projectile.position, vel, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, AttackAngle, RegulaPtr, AttackBuffer);
                        //动态变化其伤害
                        if (Main.projectile.IndexInRange(s))
                            Main.projectile[s].originalDamage = (int)(Projectile.originalDamage * 1.01f);
                    }
                    Projectile.netUpdate = Projectile.owner == Main.myPlayer;
                }
                //用于处理右键时针对敌怪的射弹逻辑
                else
                {
                    Projectile.Center = tar.Center + AttackAngle.ToRotationVector2() * FloatyDist;
                    Projectile.rotation = AttackAngle + (float)Math.Atan(90);
                    Vector2 vel = Projectile.rotation.ToRotationVector2() - tar.Center;
                    vel.Normalize();
                    if (Projectile.timeLeft <= 120)
                        Projectile.rotation = Projectile.timeLeft <= 60 ? AttackAngle - (float)Math.Atan(90) : Projectile.rotation - (MathHelper.Distance(Projectile.rotation, -Projectile.rotation) / (120 - 60));
                    AttackAngle -= MathHelper.ToRadians(4f);
                }
            }
            else
            {
                //常规射弹只会执行这一段
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.Atan(90);
                Homing();
            }
        }
        public override bool? CanDamage()
        {
            return AttackTimer <= 0 && (Idle || (Rounding && (Projectile.timeLeft >= 120 || Projectile.timeLeft <= T1CD)) || !Rounding) && !Projectile.hide ? null : false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //T3时提供自定义的减益: 失温虹吸, 作为月后等级的霜冻
            if (Owner.CIMod().ColdDivityTier3)
            {
                target.AddBuff(ModContent.BuffType<CryoDrain>(), 300);
                Owner.AddBuff(ModContent.BuffType<CryoDrain>(), 60);
            }
            int cir = 0;
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    CryogenPtr ptr = (CryogenPtr)proj.ModProjectile;
                    if (proj.ai[1] > 2f)
                        cir += Main.rand.Next(1, 4);
                }
            }
            //记住：常规射弹没有Rounding等AI
            //而挂载射弹拥有idleAI，
            int fireCD = OnTier1 ? T1CD : NOT1CD;
            cir = (int)MathHelper.Min(Main.rand.Next(15, 21), cir);
            if (AttackType > 2f)
                AttackType++;
            if (AttackType >= (30f - cir) && Projectile.timeLeft >= 120)
                AttackTimer = 15;
            
            if (Rounding && target == tar && Projectile.timeLeft < 60)
            {
                if (Projectile.timeLeft < 60)
                    Projectile.Kill();
            }
            else if (Idle)
            {
                AttackTimer = fireCD;
                NewDust(20); 
            } 
            
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (AttackBuffer != 0f)
                Owner.Heal(1);
            
            if (Rounding && target == tar && Projectile.timeLeft < 60)
            {
                NewDust(30);
                SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.position);
                modifiers.SourceDamage *= 1.0f;
            }
            else if (Rounding && target == tar && Projectile.timeLeft > 60)
            {
                NewDust(5);
                //操你妈灾厄
                modifiers.SourceDamage *= 0.8f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CryoDrain>(), 300);
            if (Idle)
            {
                AttackTimer = 300;
                NewDust(20);
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.position);
            NewDust(20);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(AttackTimer > 0 ? lightColor.R : 53, AttackTimer > 0 ? lightColor.G : Main.DiscoG, AttackTimer > 0 ? lightColor.B : 255, AttackTimer > 200 ? 255 : 255 - AttackTimer);
        }

        private void NewDust(int dAmt)
        {
            for (int i = 0; i < dAmt; i++)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(1, 3), Main.rand.NextFloat(1, 3), 0, Color.Cyan, Main.rand.NextFloat(0.5f, 1.5f));
        }
        //手动接管绘制
        public override bool PreDraw(ref Color lightColor)
        {
            if (Rounding || Idle)
                return true;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 ori = texture.Size() * 0.5f;
            Vector2 baseDrawPos = Projectile.Center - Main.screenPosition;
            //渐进
            float fade = Utils.GetLerpValue(0f, 12f, Projectile.timeLeft, true);
            Color main = Color.White * fade * 1.5f;
            //轨迹
            //绘制残影
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition = baseDrawPos - Projectile.velocity * i * 0.3f;
                Color afterimage = main * (1f - i / 8f);
                Main.EntitySpriteDraw(texture, drawPosition, null, afterimage, Projectile.rotation, ori, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
        private void Homing()
        {
             if (Projectile.timeLeft <= 240)
            {
                if (tar != null)
                {
                    tar.checkDead();
                    if (tar.life <= 0 || !tar.active || !tar.CanBeChasedBy(this, false))
                        tar = null;
                }
                tar ??= CalamityUtils.MinionHoming(Projectile.Center, 1000f, Main.player[Projectile.owner]);
                //开始发起追踪的AI 
                if (tar != null)
                {
                    float projVel = 40f;
                    Vector2 targetDirection = Projectile.Center;
                    float targetX = tar.Center.X - targetDirection.X;
                    float targetY = tar.Center.Y - targetDirection.Y;
                    float targetDist = (float)Math.Sqrt((double)(targetX * targetX + targetY * targetY));
                    if (targetDist < 100f)
                    {
                        projVel = 28f; //14
                    }
                    targetDist = projVel / targetDist;
                    targetX *= targetDist;
                    targetY *= targetDist;
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + targetX) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + targetY) / 21f;
                }
            }
        }
    }
}