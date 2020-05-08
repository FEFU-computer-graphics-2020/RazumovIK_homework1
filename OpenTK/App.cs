using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTK
{
    class App : GameWindow
    {
        public App(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }
        float[] data =
        {
            -0.8f, -0.8f, 0.0f,
            0.8f, -0.8f, 0.0f,
            0.8f, 0.8f, 0.0f,
            -0.8f, 0.8f, 0.0f
        };

        float[] colors =
{
            0.03f, 0.03f, 0.03f,
            1.0f, 0.0f, 0.0f,
            0.03f, 0.03f, 0.03f,
            0.0f, 0.0f, 1.0f
        };
        int ColorBufferObject;
        int VertexBufferObject; // VBO
        int VertexArrayObject; // VAO

        private Shader shader;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.03f, 0.03f, 0.03f, 1.0f);

            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ColorBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayObject);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            shader = new Shader("shader.v", "shader.f");

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
