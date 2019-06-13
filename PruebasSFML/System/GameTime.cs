namespace PruebasSFML.System
{
    public class GameTime
    {
        private float _deltaTime = 0f;
        private float _timeScale = 1f;

        public float TimeScale
        {
            get { return _timeScale; }
            set { _timeScale = value; }
        }

        public float DeltaTime
        {
            get { return _deltaTime; }
            set { _deltaTime = value; }
        }

        public float ScaledDeltaTime
        {
            get { return TimeScale * _deltaTime; }
        }

        public float TotalTimeElapsed
        {
            get;
            private set;
        }

        public void Update (float deltaTime, float totalTimeElapsed)
        {
            DeltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
    }
}
