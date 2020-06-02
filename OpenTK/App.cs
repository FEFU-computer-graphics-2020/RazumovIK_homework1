using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using ImGuiNET;
using OpenGL;

namespace OpenTK
{
    class App : GameWindow
    {
        public App(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }
        //float[] data =
        //{
        //    -0.8f, -0.8f, 0.0f,
        //    0.8f, -0.8f, 0.0f,
        //    0.8f, 0.8f, 0.0f,
        //    -0.8f, 0.8f, 0.0f
        //};

        //float[] colors =
        //{
        //    0.03f, 0.03f, 0.03f,
        //    1.0f, 0.0f, 0.0f,
        //    0.03f, 0.03f, 0.03f,
        //    0.0f, 0.0f, 1.0f
        //};

        //int[] index =
        //{
        //    0, 1, 2,
        //    0, 2, 3
        //};

        //Vertex[] vertices =
        //{
        //    new Vertex(new Vector3 (-0.8f, -0.8f, 0.0f), new Vector3 (0.03f, 0.03f, 0.03f)),
        //    new Vertex(new Vector3 (0.8f, -0.8f, 0.0f), new Vector3 (1.0f, 0.0f, 0.0f)),
        //    new Vertex(new Vector3 (0.8f, 0.8f, 0.0f), new Vector3 (0.03f, 0.03f, 0.03f)),
        //    new Vertex(new Vector3 (-0.8f, 0.8f, 0.0f), new Vector3 (0.0f, 0.0f, 1.0f))
        //};

        //int ColorBufferObject;
        int VertexBufferObject; // VBO
        int VertexArrayObject; // VAO
        int IndexBufferObject; // IBO

        private Mesh mesh;
        
        private Shader shader;

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            controller.SetWindowSize(Width, Height);

            base.OnResize(e);
        }

        private ImGuiController controller;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.03f, 0.03f, 0.03f, 1.0f);

            shader = new Shader("shaders/shader.v", "shaders/shader.f");

            controller = new ImGuiController();

            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            //ColorBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            mesh = MeshLoader.LoadMesh("mesh/Tower.obj");

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Unsafe.SizeOf<Vertex>(), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.Vertices.Length * Unsafe.SizeOf<Vertex>(), mesh.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shader.GetAttributeLocation("aPosition"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), 0);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(shader.GetAttributeLocation("aColor"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), Unsafe.SizeOf<Vector3>());
            GL.VertexAttribPointer(shader.GetAttributeLocation("aColor"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), Unsafe.SizeOf<Vector3>());


            GL.EnableVertexAttribArray(shader.GetAttributeLocation("aPosition"));
            GL.EnableVertexAttribArray(shader.GetAttributeLocation("aColor"));

            IndexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.Indeces.Length * sizeof(int), mesh.Indeces, BufferUsageHint.StaticDraw);
            
            base.OnLoad(e);
        }

        private float scale = 1.0f;
        private float angle = 0.0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();

            controller.NewFrame(this);

            ImGui.SliderFloat("Scale", ref scale, 0, 10);
            ImGui.SliderFloat("Angle", ref angle, 0, 3.14f * 2);

            shader.SetUniform("scaleFactor", scale);

            var model = Matrix4.CreateRotationY(angle);

            shader.SetUniform("model", model);

            //scale += 0.001f;

            //GL.BindVertexArray(VertexArrayObject);
            //GL.PointSize(50);
            //GL.DrawArrays(PrimitiveType.Points, 0, 4);
            
            GL.DrawElements(PrimitiveType.Lines, mesh.Indeces.Length, DrawElementsType.UnsignedInt, 0);

            controller.Render();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
