using System.Collections.Generic;
using System.Linq;

namespace ECSModel
{
    [ObjectSystem]
    public class BallComponentAwakeSystem : AwakeSystem<BallComponent>
    {
        public override void Awake(BallComponent self)
        {
            self.Awake();
        }
    }

    public class BallComponent : Component
    {
        public static BallComponent Instance { get; private set; }

        private Ball curball;

        public Ball CurBall
        {
            get
            {
                return this.curball;
            }
            set
            {
                this.curball = value;
                this.curball.Parent = this;
            }
        }

        private readonly Dictionary<long, Ball> balls = new Dictionary<long, Ball>();

        public void Awake()
        {
            Instance = this;
        }

        public void Add(Ball ball)
        {
            balls.Add(ball.Id, ball);
            ball.Parent = this;
        }

        public Ball Get(long id)
        {
            Ball ball;
            this.balls.TryGetValue(id, out ball);
            return ball;
        }
        
        public Ball GetByConfigId(long ConfigId)
        {
            foreach (Ball ball in balls.Values)
            {
                if (ball.ConfigID == ConfigId) return ball;
            }
            return null;
        }
        public void Remove(long id)
        {
            this.balls.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.balls.Count;
            }
        }

        public Ball[] GetAll()
        {
            return this.balls.Values.ToArray();
        }

        public int GetCurBallIndex()
        {
            int index = -1;
            if (curball == null)
                return index;


            Ball[] balls = GetAll();

            for( int i =0; i<balls.Length;i++)
            {
                if ( balls[i].Id == curball.Id)
                {
                    index = i;
                    break;
                }
            }

            return index;

        }


        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (Ball ball in this.balls.Values)
            {
                ball.Dispose();
            }

            Instance = null;
        }
    }
}