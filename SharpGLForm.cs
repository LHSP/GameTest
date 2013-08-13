using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

using SharpGL;
using SharpGL.SceneGraph.Assets;
using System.Threading;
using System.IO;

namespace GameTest
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        Texture[] textures;
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();

            string[] texFilePaths = Directory.GetFiles("../../textures", "*.png");

            textures = new Texture[texFilePaths.Length];

            //  Get the OpenGL object.
            OpenGL gl = this.openGLControl.OpenGL;

            //  A bit of extra initialisation here, we have to enable textures.
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            //  Create our texture object from a file. This creates the texture for OpenGL.
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture();
                textures[i].Create(gl, texFilePaths[i]);
            }
            if (textures.Length <= 0)
            {
                MessageBox.Show("No textures found! Exiting.");
                Application.Exit();
            }
        }

        int[,] level = { { 1, 0, 2 }, { 1, 1, 1 }, { 2, 1, 2 }, { 1, 0, 2 }, { 1, 1, 1 }, { 2, 1, 2 } };


        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            //  Get the OpenGL object, for quick access.
            OpenGL gl = this.openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(42, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(0 + tX, 0 + tY, 10 + tZ, 0 + tX, 0 + tY, 0 + tZ, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.LoadIdentity();

            int width = level.GetLength(0), height = level.GetLength(1);
            gl.Color(1f, 1f, 1f);

            for (int i = 0; i < width; i++)
            {
                gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
                for (int j = 0; j < height; j++)
                {
                    if (textures.Length > 0)
                    {
                        //  Bind the texture.
                        textures[level[i,j]].Bind(gl);
                    }
                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(i, j, 0.0f);
                    gl.TexCoord(1.0f, 0.0f); gl.Vertex(i + 1.0f, j, 0.0f);
                    gl.TexCoord(0.0f, 1.0f); gl.Vertex(i, j + 1.0f, 0.0f);
                    gl.TexCoord(1.0f, 1.0f); gl.Vertex(i + 1.0f, j + 1.0f, 0.0f);
                }
                gl.End();
            }

            gl.Flush();

            //test += 1;
            Console.WriteLine(test);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = this.openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = this.openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(42, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(0 + tX, 0 + tY, 10 + tZ, 0 + tX, 0 + tY, 0 + tZ, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        int test = 7;
        int tX = 3;
        int tY = 3;
        int tZ = 0;

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Add:
                    test += 1;
                    return true;
                case Keys.Subtract:
                    test -= 1;
                    return true;
                case Keys.Left:
                    tX -= 1;
                    return true;
                case Keys.Right:
                    tX += 1;
                    return true;
                case Keys.Up:
                    tY += 1;
                    return true;
                case Keys.Down:
                    tY -= 1;
                    return true;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
