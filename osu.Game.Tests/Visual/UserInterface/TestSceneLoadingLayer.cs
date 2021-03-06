// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tests.Visual.UserInterface
{
    public class TestSceneLoadingLayer : OsuTestScene
    {
        private Drawable dimContent;
        private LoadingLayer overlay;

        private Container content;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Children = new[]
            {
                content = new Container
                {
                    Size = new Vector2(300),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new[]
                    {
                        new Box
                        {
                            Colour = Color4.SlateGray,
                            RelativeSizeAxes = Axes.Both,
                        },
                        dimContent = new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(10),
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.9f),
                            Children = new Drawable[]
                            {
                                new OsuSpriteText { Text = "Sample content" },
                                new TriangleButton { Text = "can't puush me", Width = 200, },
                                new TriangleButton { Text = "puush me", Width = 200, Action = () => { } },
                            }
                        },
                        overlay = new LoadingLayer(dimContent),
                    }
                },
            };
        });

        [Test]
        public void TestShowHide()
        {
            AddAssert("not visible", () => !overlay.IsPresent);

            AddStep("show", () => overlay.Show());

            AddUntilStep("wait for content dim", () => dimContent.Colour != Color4.White);

            AddStep("hide", () => overlay.Hide());

            AddUntilStep("wait for content restore", () => dimContent.Colour == Color4.White);
        }

        [Test]
        public void TestContentRestoreOnDispose()
        {
            AddAssert("not visible", () => !overlay.IsPresent);

            AddStep("show", () => overlay.Show());

            AddUntilStep("wait for content dim", () => dimContent.Colour != Color4.White);

            AddStep("expire", () => overlay.Expire());

            AddUntilStep("wait for content restore", () => dimContent.Colour == Color4.White);
        }

        [Test]
        public void TestLargeArea()
        {
            AddStep("show", () =>
            {
                content.RelativeSizeAxes = Axes.Both;
                content.Size = new Vector2(1);

                overlay.Show();
            });

            AddStep("hide", () => overlay.Hide());
        }
    }
}
