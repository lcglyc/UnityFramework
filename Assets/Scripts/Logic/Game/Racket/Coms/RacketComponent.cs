using System.Collections.Generic;
using System.Linq;

namespace ECSModel
{
    [ObjectSystem]
    public class RacketComponentAwakeSystem : AwakeSystem<RacketComponent>
    {
        public override void Awake(RacketComponent self)
        {
            self.Awake();
        }
    }

    public class RacketComponent : Component
    {
        public static RacketComponent Instance { get; private set; }

        private Racket curRacket;

        public Racket CurRacket
        {
            get
            {
                return this.curRacket;
            }
            set
            {
                this.curRacket = value;
                this.curRacket.Parent = this;
            }
        }

        private readonly Dictionary<long, Racket> rackets = new Dictionary<long, Racket>();

        public void Awake()
        {
            Instance = this;
        }

        public void Add(Racket tmpracket)
        {
            this.rackets.Add(tmpracket.Id, tmpracket);
            tmpracket.Parent = this;
        }

        public Racket Get(long id)
        {
            Racket racket;
            this.rackets.TryGetValue(id, out racket);
            return racket;
        }

        public void Remove(long id)
        {
            this.rackets.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.rackets.Count;
            }
        }

        public Racket[] GetAll()
        {
            return this.rackets.Values.ToArray();
        }

        public int GetCurRacketIndex()
        {
            int index = -1;
            if (curRacket == null)
                return index;


            Racket[] rackets = GetAll();

            for (int i = 0; i < rackets.Length; i++)
            {
                if (rackets[i].Id == curRacket.Id)
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

            foreach (Racket racket in this.rackets.Values)
            {
                racket.Dispose();
            }
            Instance = null;
        }
    }
}