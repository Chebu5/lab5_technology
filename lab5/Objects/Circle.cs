using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lab5.Objects
{

    class Circle : BaseObject
    {
        private Color color = Color.DarkBlue;
        private float size = 40; // Начальный размер круга
        public Action OnSizeZero; // Событие обнуления размера, Action может хранить ссылку на метод
        public float Size //свойство размер
        {
            get { return size; }
            set
            {
                size = value;
                if (size <= 0)
                {
                    size = 0;
                    OnSizeZero?.Invoke(); // Генерируем событие
                }
            }
        }

        public Circle(float x, float y) : base(x, y, 0)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(color), -Size / 2, -Size / 2, Size, Size); // -Size / 2, -Size / 2 - это координаты верхнего левого угла прямоугольника, в который вписан эллипс. Мы смещаем координаты на -Size / 2 по обеим осям, чтобы центр круга совпадал с координатой (X, Y), которая хранится в BaseObject. Size
            g.DrawEllipse(new Pen(Color.Black, 1), -Size / 2, -Size / 2, Size, Size);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Size / 2, -Size / 2, Size, Size);
            return path;
        }
    }
}
