using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LearnOpenTKWinForm
{
    class Texture
    {
        public readonly int Handle;

        public Texture(string path) 
        {
            // Generate blank texture
            Handle = GL.GenTexture();

            // Bind the handle
            Use();

            // Load the image
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            // Flip image correct way
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            // Get an array of pixels
            Rgba32[] tempPixels = image.GetPixelSpan().ToArray();

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
            List<byte> pixels = new List<byte>();

            foreach (Rgba32 p in tempPixels)
            {
                pixels.Add(p.R);
                pixels.Add(p.G);
                pixels.Add(p.B);
                pixels.Add(p.A);
            }

            // Generate texture
            GL.TexImage2D(
                TextureTarget.Texture2D, 
                0, 
                PixelInternalFormat.Rgba, 
                image.Width, 
                image.Height, 
                0, 
                PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                pixels.ToArray());
        }

        private void Use ()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

    }
}
