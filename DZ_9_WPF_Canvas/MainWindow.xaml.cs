using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _17._01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    class BallController
    {
        public Ellipse Ball;
        public Rectangle pc, user;
        public Canvas canvas;
        public int userscore = 0;
        public int pcscore = 0;
        short vx = 0, vy = 0;
        public BallController(Ellipse Ball, Rectangle pc, Rectangle user, Canvas canvas)
        {
            this.Ball = Ball;
            this.pc = pc;
            this.user = user;
            this.canvas = canvas;
            Random random = new Random();
            while (this.vx == 0)
            {
                this.vx = (short)random.Next(-3, 4);
            }
            while (this.vy == 0)
            {
                this.vy = (short)random.Next(-3, 4);
            }
        }
        public void UpdateScore()
        {
            //TextChange.Text = Canvas.GetLeft(Ball).ToString();
            if (Canvas.GetLeft(Ball) == 0 /*&& Canvas.GetTop(Ball) != Canvas.GetTop(pc)*/)
            {
                userscore +=1;
                // UserSсore.Content = userscore.ToString();
            }
            if (Canvas.GetLeft(Ball) == canvas.Width-Ball.Width && Canvas.GetTop(Ball) != Canvas.GetTop(user))
            {
                pcscore +=1;
                //BotSсore.Content = userscore.ToString();
            }
        }
        public void Update()
        {
            double left = Canvas.GetLeft(Ball);
            if (left + vx < 0)
            {
                Canvas.SetLeft(Ball, 0);
                vx *= -1;
            }
            else if (left + vx + Ball.Width > canvas.Width)
            {
                Canvas.SetLeft(Ball, canvas.Width - Ball.Width);
                vx *= -1;
            }
            else
            {
                Canvas.SetLeft(Ball, left + vx);
            }


            double top = Canvas.GetTop(Ball);
            if (top + vy < 0)
            {
                Canvas.SetTop(Ball, 0);
                vy *= -1;
            }
            else if (top + vy + Ball.Height > canvas.Height)
            {
                Canvas.SetTop(Ball, canvas.Height - Ball.Height);
                vy *= -1;
            }
            else
            {
                Canvas.SetTop(Ball, top + vy);
            }
        }
    }

    class UserRectengleConntroller
    {
        Rectangle user;
        Canvas canvas;

        public UserRectengleConntroller(Rectangle user, Canvas canvas)
        {
            this.user = user;
            this.canvas = canvas;
        }

        public void Update(MouseEventArgs e)
        {
            Point f = e.GetPosition(this.canvas);

            if (f.Y < 0)
            {
                Canvas.SetTop(user, 0);
            }
            else if (f.Y + user.Height > canvas.Height)
            {
                Canvas.SetTop(user, canvas.Height - user.Height);
            }
            else
            {
                Canvas.SetTop(user, f.Y);
            }
        }
    }
    class PCRectangleController//РС контроллер
    {
        Rectangle PC;
        Canvas canvas;
        Ellipse ellipse;
        double vy = 0;
        public PCRectangleController(Rectangle PC, Canvas canvas, Ellipse ellipse)
        {
            this.PC = PC;
            this.canvas = canvas;
            this.ellipse = ellipse;
        }
        public void Update()
        {
            double top = Canvas.GetTop(ellipse);

            if (top + vy < 0)
            {
                Canvas.SetTop(PC, 0);
                vy *= -1;
            }
            else if (top + vy + PC.Height > canvas.Height)
            {
                Canvas.SetTop(PC, canvas.Height - PC.Height);
                vy *= -1;
            }
            else
            {
                Canvas.SetTop(PC, top + vy);
            }
        }

    }
    public partial class MainWindow : Window
    {
        BallController ballController;
        UserRectengleConntroller userRectengleConntroller;
        PCRectangleController pcRectangleController;
        
        public MainWindow()
        {
            InitializeComponent();
            ballController = new BallController(Ball, PC, User, canvas);
            userRectengleConntroller = new UserRectengleConntroller(User, canvas);
            pcRectangleController = new PCRectangleController(PC, canvas, Ball);

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2);
            timer.Tick += BallUpdate;
            timer.Tick += PCUpdate;
            timer.Tick += SetScore;
            timer.Start();
       
        }

        void BallUpdate(object sender, EventArgs e)
        {
            ballController.Update();
        }
        void PCUpdate(object sender, EventArgs e)//непобедимый комп)))
        {
            pcRectangleController.Update();
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)//при зажатой кнопке двигаем
        {
            if (e.LeftButton == MouseButtonState.Pressed) { userRectengleConntroller.Update(e); }
        }
        void SetScore(object sender, EventArgs e)
        {
            ballController.UpdateScore();
            UserSсore.Content = ballController.userscore.ToString();
            BotSсore.Content = ballController.pcscore.ToString();
        }
    }
}
