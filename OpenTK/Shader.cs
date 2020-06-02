using System.IO;
using System.Text;
using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenTK
{
    public class Shader
    {
        int _handle;

        private Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();
        private Dictionary<string, int> _attributeLocations = new Dictionary<string, int>();
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

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle); // линковка

            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);

            GL.DeleteShader(vertexShader); // удаляем для сбережения ресурсов
            GL.DeleteShader(fragmentShader);

        }

        public int GetUniformLocation(string name)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                _uniformLocations[name] = GL.GetUniformLocation(_handle, name);
            }
            return _uniformLocations[name];
        }

        public int GetAttributeLocation(string name)
        {
            if (!_attributeLocations.ContainsKey(name))
            {
                _attributeLocations[name] = GL.GetAttribLocation(_handle, name);
            }
            return _attributeLocations[name];
        }


        public void SetUniform(string name, float val)
        {
            GL.Uniform1(GetUniformLocation(name), val);
        }

        public void SetUniform(string name, Matrix4 val)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref val);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }
    }
}
