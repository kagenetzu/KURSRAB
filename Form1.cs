﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KURSRAB
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter; // добавим поле для эмиттера
        GravityPoint point1 = new GravityPoint(); // добавил поле под первую точку

        Color clFrom = Color.Gold;
        Color clTo = Color.FromArgb(0, Color.Red);
        public Form1()
        {
           
            InitializeComponent();
            picDisplay.MouseWheel += picDisplay_MouseWheel;
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            colorDialog1.FullOpen = true;
            colorDialog2.FullOpen = true;
            colorDialog3.FullOpen = true;

           

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 90,
                Spreading = 100,
                SpeedMin = 10,
                SpeedMax = 20,
                ColorFrom = clFrom,
                ColorTo =clTo,
                ParticlesPerTick = 20,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся

            // привязываем гравитоны к полям
            point1 = new GravityPoint
            {
                X = picDisplay.Width / 2 + 100,
                Y = picDisplay.Height / 2,
            };
            
            // привязываем поля к эмиттеру
            emitter.impactPoints.Add(point1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // каждый тик обновляем систему
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black); // добавил очистку
                emitter.Render(g); // рендерим систему
            }
            // обновить picDisplay
            picDisplay.Invalidate();
        }
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // а тут в эмиттер передаем положение мыфки
            foreach (var emitter in emitters)
            {
                emitter.MousePositionX = e.X;
                emitter.MousePositionY = e.Y;
            }
            // а тут передаем положение мыши, в положение гравитона
            point1.X = e.X;
            point1.Y = e.Y;
        }
        private void picDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta < 0 && point1.pointRadius > 0)
            {
                point1.pointRadius -=5;
            }
            if(e.Delta > 0 && point1.pointRadius < 500)
            {
                point1.pointRadius +=5;
            }
        }

        private void picDisplay_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = trackBar1.Value;
            label6.Text = $"{trackBar1.Value}";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            emitter.Spreading = trackBar2.Value;
            label3.Text = $"{trackBar2.Value}";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = trackBar5.Value;
            label8.Text = $"{trackBar5.Value}";
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            emitter.SpeedMax = trackBar4.Value;
            emitter.SpeedMin = trackBar4.Value - 10;
            label7.Text = $"{trackBar4.Value}";
        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            point1.colorRadar = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            Particle.colorF = colorDialog2.Color ;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog3.ShowDialog() == DialogResult.Cancel)
                return;
            Particle.colorT = Color.FromArgb(0,colorDialog3.Color);
        }
    }
}
