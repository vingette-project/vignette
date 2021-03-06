// Copyright 2020 - 2021 Vignette Project
// Licensed under NPOSLv3. See LICENSE for details.

using osu.Framework.Graphics;
using Vignette.Game.Graphics.UserInterface;

namespace Vignette.Game.Screens.Menu.Settings
{
    public class SettingsHexColourBox : SettingsItem<Colour4>
    {
        protected override Drawable CreateControl() => new FluentHexColourBox { Width = 100 };
    }
}
