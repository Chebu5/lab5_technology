using lab5.Objects;
using System.Drawing;


namespace lab5
{
    public partial class Form1 : Form
    {
        
        List<BaseObject> objects = new();//список всех объектов на форме
        Player player;
        Marker marker;

        private List<Circle> _collectibleCircles = new List<Circle>(); //храню все собираемые круги
        private int score = 0;
        private Random rand = new Random();



        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);


            //событие игрок достал кружок
            player.OnCollectibleCircleOverlap += (circle) =>
            {
                score += 10; // даем 10 очков за кружок
                UpdateScoreLabel();
                RemoveCollectibleCircle(circle); // Убираем кружок
                AddCollectibleCircle(); // Добавляем новый кружок на поле
            };

            //событие игрок пересекся в опр время с кругом
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            //событие игрок пересекся с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);//удаляем маркер из списка объектов
                marker = null; 
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            objects.Add(marker);
            objects.Add(player);
            AddCollectibleCircle();
            AddCollectibleCircle();

            // Выводим начальное количество очков
            UpdateScoreLabel();
        }

        private void AddCollectibleCircle()
        {
            var circle = new Circle(
                rand.Next(50, pbMain.Width - 50),
                rand.Next(50, pbMain.Height - 50)
            ); //новый круг рандомно внутри pbMain

            // событие обнуления размера
            circle.OnSizeZero += () =>
            {
                // Перемещаем кружок в случайное место и задаем новый размер
                circle.X = rand.Next(50, pbMain.Width - 50);
                circle.Y = rand.Next(50, pbMain.Height - 50);
                circle.Size = rand.Next(20, 40); // Задаем случайный начальный размер
            };

            _collectibleCircles.Add(circle);
            objects.Add(circle);
        }

        private void RemoveCollectibleCircle(Circle circle)
        {
            _collectibleCircles.Remove(circle);
            objects.Remove(circle);
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Очки: {score}";
        }

        //когда произойдет событие Paint у pbMain, вызываем метод pbMain_Paint
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics; //получаем объект Graphics для рисования

            g.Clear(Color.White);
            updatePlayer();

            foreach (var obj in objects.ToList()) //копия objects для безопасного удаления объектов
            {
                if (obj is Circle collectibleCircle) //является ли текущий объект obj экземпляром класса Circle
                {
                    collectibleCircle.Size -= 0.1f; //Уменьшаем размер каждого кружка
                }

                if (obj != player && player.Overlaps(obj, g)) //произошло ли столкновение с игроком (но только чтобы объект сам не был игроком)
                {
                    player.Overlap(obj); //если пересеклись, то вызываем метод Overlap 
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate(); //перерисовка 
        }

        private void updatePlayer()
        {
            if (marker != null) //если маркер установлен на поле
            {
                float dx = marker.X - player.X; //разница координат между маркером и игроком
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy); //расстояние между маркером и игроком
                dx /= length; //нормализует вектор направления к маркеру(делает его единичным)
                dy /= length;

                player.vX += dx * 0.5f; //увеличиваю скорость игрока по оси X в направлении маркера
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;//вычислием угол поворота игрока
            }

            player.vX += -player.vX * 0.1f; //замедляем движение игрока
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;//обновляем позицию игрока
            player.Y += player.vY;
        }
        
        
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {

            if (marker == null)
            {
                marker = new Marker(0, 0, 0); //если маркера нет на поле, то новый экземляр
                objects.Add(marker);
            }

            marker.X = e.X; //координата Х по клику мыши
            marker.Y = e.Y;
        }
    }
}
