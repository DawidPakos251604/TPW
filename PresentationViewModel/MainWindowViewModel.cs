using Presentation.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ModelIBall = Presentation.Model.IBall;

namespace Presentation.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region Fields

        private IDisposable Observer = null;
        private ModelAbstractApi ModelLayer;
        private bool Disposed = false;

        #endregion Fields

        #region Constructor

        public MainWindowViewModel() : this(null) { }

        internal MainWindowViewModel(ModelAbstractApi modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractApi.CreateModel() : modelLayerAPI;
            Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x));
            StartCommand = new RelayCommand(StartFromCommand);
        }

        #endregion Constructor

        #region Public API

        public ObservableCollection<ModelIBall> Balls { get; } = new ObservableCollection<ModelIBall>();

        private int _ballCount = 5;
        public int BallCount
        {
            get => _ballCount;
            set
            {
                _ballCount = value;
                RaisePropertyChanged(nameof(BallCount));
            }
        }

        public ICommand StartCommand { get; }

        public void Start(int numberOfBalls)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            ModelLayer.Start(numberOfBalls);
            Observer.Dispose(); // zakończ poprzednie obserwowanie
            Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x)); // rozpocznij nowe
        }

        private void StartFromCommand()
        {
            Balls.Clear(); // wyczyść poprzednie kulki
            Start(BallCount);
        }

        #endregion Public API

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Balls.Clear();
                    Observer?.Dispose();
                    ModelLayer.Dispose();
                }

                Disposed = true;
            }
        }

        public void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
