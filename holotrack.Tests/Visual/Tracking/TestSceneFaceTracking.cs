using System.Collections.Generic;
using System.Linq;
using FaceRecognitionDotNet;
using holotrack.Tracking;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Camera;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace holotrack.Tests.Visual.Tracking
{
    public class TestSceneFaceTracking : TestScene
    {
        private readonly SpriteText status;
        private readonly FaceTracker tracker;
        private readonly Container faceLocationsContainer;
        private readonly CameraSprite camera;

        public TestSceneFaceTracking()
        {
            Children = new Drawable[]
            {
                tracker = new FaceTracker(),
                camera = new CameraSprite
                {
                    CameraID = 0,
                },
                faceLocationsContainer = new Container
                {
                    Name = @"face locations",
                    Size = new Vector2(640, 480),
                },
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Colour4.Black,
                            RelativeSizeAxes = Axes.Both,
                        },
                        status = new SpriteText
                        {
                            AlwaysPresent = true,
                            Margin = new MarginPadding(5)
                        },
                    }
                }
            };

            tracker.StartTracking(camera);
        }

        protected override void Update()
        {
            if (!status.IsLoaded && !tracker.IsLoaded)
                return;
            
            status.Text = $"Faces: {tracker.Tracked} | IsTracking: {tracker.IsTracking}";

            faceLocationsContainer.Clear();

            if (tracker.Faces != null)
            {
                foreach (var face in tracker.Faces.ToList())
                {
                    faceLocationsContainer.Add(new OutlinedBox
                    {
                        X = face.BoundingBox.X,
                        Y = face.BoundingBox.Y,
                        Width = face.BoundingBox.Width,
                        Height = face.BoundingBox.Height,
                        Landmarks = face.Landmarks
                    });
                }
            }
        }

        private class OutlinedBox : Container
        {
            public IDictionary<FacePart, IEnumerable<FacePoint>> Landmarks;

            public OutlinedBox()
            {
                Masking = true;
                BorderColour = Colour4.Red;
                BorderThickness = 3;
                Child = new Box
                {
                    Colour = Colour4.Transparent,
                    RelativeSizeAxes = Axes.Both,
                };

                if (Landmarks != null)
                {
                    foreach (var part in Landmarks.Values)
                    {
                        foreach (var point in part)
                        {
                            Add(new Circle
                            {
                                X = (float)point.Point.X,
                                Y = (float)point.Point.Y,
                                Size = new Vector2(5),
                                Colour = Colour4.Blue,
                            });
                        }
                    }
                }
            }
        }
    }
}