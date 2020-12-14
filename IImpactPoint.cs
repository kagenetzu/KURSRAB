using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KURSRAB
{
    public abstract class IImpactPoint
    {
        public float X; // ну точка же, вот и две координаты
        public float Y;

        public abstract void ImpactParticle(Particle particle);

        // базовый класс для отрисовки точечки
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }
    public class RadarPoint : IImpactPoint //точка - радар
    {
        public int pointRadius = 120; // радиус точки
        // счетчики частиц в радаре
        int littleCount = 0;
        int averangeCount = 0;
        int bigCount = 0;

        public Color colorRadar = Color.Red; // цвет радара

        List<Particle> radarParticle = new List<Particle>(); //лист частиц, попавших в радар
       
       
        public override void ImpactParticle(Particle particle)
        {
            
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы
            if (r + particle.Radius < pointRadius / 2) // если частица оказалось внутри радара
            {
                radarParticle.Add(particle); // добавляем частицу в список
            }
            
        }
        public override void Render(Graphics g)
        {
           g.DrawEllipse(
                   new Pen(Color.White,3),
                   X - pointRadius / 2,
                   Y - pointRadius / 2,
                   pointRadius,
                   pointRadius
               );

            SolidBrush b = new SolidBrush(colorRadar); // цвет частиц в радаре
            foreach (var par in radarParticle)
            {
                // повер частиц, которые попали в радар, рисуем эллипсы с такими же координатами
                g.FillEllipse(b, par.X - par.Radius, par.Y - par.Radius, par.Radius * 2, par.Radius * 2); 
                // считаем кол-во маленьких/больших/средних частиц
                if (par.Radius < 5)
                {
                    littleCount++;
                }
                else
                {
                    if (par.Radius > 8)
                    {
                        bigCount++;
                    }
                    else
                    {
                        averangeCount++;
                    }
                }
            }
            radarParticle.Clear();//очищаем список

            var stringFormat = new StringFormat(); // создаем экземпляр класса
            stringFormat.Alignment = StringAlignment.Center; // выравнивание по горизонтали
            stringFormat.LineAlignment = StringAlignment.Center; // выравнивание по вертикали

            g.DrawString(
                // вывод значений счетчика
                $"Маленькие {littleCount}\n Средние {averangeCount}\n Большие {bigCount}",
                new Font("Verdana", 10),
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat // передаем инфу о выравнивании
            );
            // обнуляем счетчики
            littleCount = 0;
            averangeCount = 0;
            bigCount = 0;
        }
    }
}
