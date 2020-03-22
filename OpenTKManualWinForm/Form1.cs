using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application; // Used for application version
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Reflection;

namespace OpenTKManualWinForm
{
    
    public partial class Form1 : Form
    {
        // Variables
        //------------------------------------------------------
        bool glLoaded = false; // Blocks OpenGL calls until loaded
        
        // Position values for triangle
        int x = 0;
        int y = 0;
        float rotation = 0;

        // Buffer objects
        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        // Vertices
        float[] verticesTriangle =
        {
           -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
           -0.5f,  0.5f, 0.0f  //Top vertex
        };

        float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,        // first triangle
            1, 2, 3         // second triangle
        };          

        // Instantiate objects
        //------------------------------------------------------
        private Shader shader;

        // Constructor
        //------------------------------------------------------
        public Form1()
        {
            InitializeComponent();

            // Set label text to current app version    
            label1.Text = "v" + CurrentAppVersion;
        }

        // WinForm Functions
        //------------------------------------------------------
        public string CurrentAppVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        // glControl Handlers
        //------------------------------------------------------
        private void glControl1_Load(object sender, EventArgs e)
        {
            VertexBufferObject  = GL.GenBuffer();
            VertexArrayObject   = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();
            shader = new Shader("C:/Users/Mariu/source/repos/OpenTKManualWinForm/OpenTKManualWinForm/Shaders/shader.vert", "C:/Users/Mariu/source/repos/OpenTKManualWinForm/OpenTKManualWinForm/Shaders/shader.frag");
            shader.Use();

            GL.ClearColor(Color.DarkCyan);
            SetupViewport();

            /*ElementBuffer test*/
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            
            /********************/

            // Initialization code
            // 1. Bind vertex array object       
            //GL.BindVertexArray(VertexArrayObject);

            // 2. copy our vertices array in a buffer for OpenGL to use
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            // 3. then set our vertex attributes pointers
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);
            
            glLoaded = true;
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!glLoaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Bind the shader
            //shader.Use();

            // Bind the VAO
            //GL.BindVertexArray(VertexArrayObject);

            // Call draw function
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            /*
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(x, y, 0); // position triangle according x and y

            if (glControl1.Focused)
                GL.Color3(Color.Yellow);
            else
                GL.Color3(Color.DarkGray);

            GL.Rotate(rotation, Vector3.UnitZ);

            GL.Begin(PrimitiveType.Triangles);

            GL.Vertex2(10, 20);
            GL.Vertex2(100, 20);
            GL.Vertex2(100, 50);

            GL.End();
            */

            glControl1.SwapBuffers();
            glControl1.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!glLoaded)
                return;

            SetupViewport();
            glControl1.Invalidate();
        }
        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!glLoaded)
                return;
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!glLoaded)
                return;

            if (e.KeyCode == Keys.D)      x += 5;
            else if (e.KeyCode == Keys.A) x -= 5;
            else if (e.KeyCode == Keys.W) y += 5;
            else if (e.KeyCode == Keys.S) y -= 5;

            if (e.KeyCode == Keys.E)
                rotation += 5;
            else if (e.KeyCode == Keys.R)
                rotation -= 5;

            glControl1.Invalidate();
        }

        private void glControl1_Leave(object sender, EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteProgram(shader.Handle);
        }

        // OpenTK Related Functions
        //------------------------------------------------------
        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h);     // Use all of the glControl painting area
        }

    }
}
