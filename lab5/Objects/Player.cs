using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5.Objects
{
    class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap; //при пересечении игрока с маркером
        public Action<Circle> OnCollectibleCircleOverlap; //при пересечении игрока с кругом
        public float vX, vY; //скорость игрока
        public Player(float x, float y, float angle) : base(x, y, angle)
        {
        }


        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.DeepSkyBlue), -15, -15, 30, 30);
            g.DrawEllipse(new Pen(Color.Black, 2), -15, -15, 30, 30);
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0); //линия, показывающая направление движения игрока

        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Marker)
            {
                OnMarkerOverlap?.Invoke(obj as Marker);//вызываем все методы подписанные на OnMarkerOverlap
            }

            if (obj is Circle)
            {
                OnCollectibleCircleOverlap?.Invoke(obj as Circle); //вызываем все методы подписанные на  OnCollectibleCircleOverlap
            }
        }
    }
}
