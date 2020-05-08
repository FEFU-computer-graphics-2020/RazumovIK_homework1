using System.IO;
using System.Text;
using System;
using OpenTK.Graphics.OpenGL;

namespace OpenTK
{
    public class Shader
    {
        int Handle;
        int CompileShader(string path, ShaderType shaderType)
        {
            string shaderSource;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);

            GL.CompileShader(shader);

            var infolog = GL.GetShaderInfoLog(shader);
            if (infolog.Length != 0)
            {
                Console.WriteLine($"Failed at shader {path}");
                Console.WriteLine(infolog);
                return 0;
            }
            
            return shader;
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
            var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            GL.LinkProgram(Handle); // линковка

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);

            GL.DeleteShader(vertexShader); // удаляем для сбережения ресурсов
            GL.DeleteShader(fragmentShader);

        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }
    }
}
