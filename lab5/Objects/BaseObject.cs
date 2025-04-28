using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace lab5.Objects
{
    class BaseObject
    {
        public float X;//координаты объекта
        public float Y;
        public float Angle;
        public Action<BaseObject, BaseObject> OnOverlap; //пересечение объекта ссылка на метод

        public BaseObject(float x, float y, float angle)
        {
            X = x;
            Y = y;
            Angle = angle;
        }

        public Matrix GetTransform() //матрица преобразования используется для перемещения, поворота и масштабирования объекта.
        {
            var matrix = new Matrix(); //Matrix - это класс в .NET, который представляет матрицу 3x3, используемую для 2D-трансформаций.
            matrix.Translate(X, Y); //операция перемещения
            matrix.Rotate(Angle); //операция вращения
            return matrix;
        }

        public virtual void Render(Graphics g) //рисуем объект на графическом контексте g
        {
            
        }

        public virtual GraphicsPath GetGraphicsPath()
        {
           
            return new GraphicsPath(); //графический путь объекта для определения формы объекта при проверке на пересечение
        }

        public virtual bool Overlaps(BaseObject obj, Graphics g) //возвращает или определяет было пересечение или нет
        {
           
            var path1 = this.GetGraphicsPath();
            var path2 = obj.GetGraphicsPath();

      
            path1.Transform(this.GetTransform()); //изменяем графический путь объекта в зависимости от параметров в матрице
            path2.Transform(obj.GetTransform());

            var region = new Region(path1); //просто область занимаемая объектом
            region.Intersect(path2); // пересекаем формы
            return !region.IsEmpty(g); // если полученная форма не пуста то значит было пересечение
        }

        public virtual void Overlap(BaseObject obj) //что произошло после столкновения
        {
            if (this.OnOverlap != null) // если к полю есть привязанные функции (подписчики на событие)
            {
                this.OnOverlap(this, obj); // то вызываем их
            }
        }
    }
}
