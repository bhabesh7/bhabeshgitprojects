using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace TowersOfHanoi
{
    public class MainViewModel : NotificationObject
    {


        private SourcePegState _pegState;

        public SourcePegState PegState
        {
            get { return _pegState; }
            set { _pegState = value; RaisePropertyChanged(() => PegState); }
        }


        LinkedList<SourcePegState> PegStateTransition { get; set; }


        private Stack<int> _beginStack;

        public Stack<int> BeginStack
        {
            get { return _beginStack; }
            set { _beginStack = value; RaisePropertyChanged(() => BeginStack); }
        }


        private Stack<int> _auxStack;

        public Stack<int> AuxStack
        {
            get { return _auxStack; }
            set { _auxStack = value; RaisePropertyChanged(() => AuxStack); }
        }


        private Stack<int> _endStack;

        public Stack<int> EndStack
        {
            get { return _endStack; }
            set { _endStack = value; RaisePropertyChanged(() => EndStack); }
        }




        public ICommand StartCommand
        {
            get;
            set;
        }

        public ObservableCollection<int> BeginCollection { get; set; }
        public ObservableCollection<int> AuxCollection { get; set; }
        public ObservableCollection<int> EndCollection { get; set; }


        readonly int INIT_DISK_LEN = 0;
        private bool isSolved;

        public bool IsSolved
        {
            get { return isSolved; }
            set { isSolved = value; RaisePropertyChanged(() => IsSolved); }
        }



        public MainViewModel()
        {
            BeginCollection = new ObservableCollection<int>();
            AuxCollection = new ObservableCollection<int>();
            EndCollection = new ObservableCollection<int>();

            StartCommand = new DelegateCommand(ExecuteStartCommand);
            InitializeStack();
            INIT_DISK_LEN = BeginStack.Count;
            PegState = SourcePegState.BEGIN;
            UpdateCollectionAsync();

        }

        private void ExecuteStartCommand()
        {
            BeginStack.Clear();
            AuxStack.Clear();
            EndStack.Clear();
            IsSolved = false;

            InitPushIntoBeginStack();
            PegState = SourcePegState.BEGIN;
            UpdateCollectionAsync();
            SolveTowersOfHanoi();
        }


        async Task UpdateCollectionAsync()
        {
            Task t = Task.Delay(3000);
            BeginCollection.Clear();
            AuxCollection.Clear();
            EndCollection.Clear();

            BeginStack.ToList().ForEach((x) => BeginCollection.Add(x));
            AuxStack.ToList().ForEach((x) => AuxCollection.Add(x));
            EndStack.ToList().ForEach((x) => EndCollection.Add(x));
            await t;
        }


        async Task SolveTowersOfHanoi()
        {
            if (IsSolved)
            {
                return;
            }

            await UpdateCollectionAsync();
            
            //Done State
            if (EndStack.Count == INIT_DISK_LEN)
            {
                IsSolved = true;
                return;
            }


            switch (PegState)
            {
                case SourcePegState.BEGIN:

                    // END , AUX
                    if (BeginStack.Count > 0)
                    {
                        if (EndStack.Count == 0 || CanPush(BeginStack.Peek(), EndStack.Peek()))
                        {
                            EndStack.Push(BeginStack.Pop());                            
                            SolveTowersOfHanoi();
                        }
                        else if (AuxStack.Count == 0 || CanPush(BeginStack.Peek(), AuxStack.Peek()))
                        {
                            AuxStack.Push(BeginStack.Pop());
                            //PegState = SourcePegState.BEGIN;
                            SolveTowersOfHanoi();
                        }
                        else
                        {
                            PegState = SourcePegState.END;
                            SolveTowersOfHanoi();
                        }
                    }
                    else
                    {
                        PegState = SourcePegState.END;
                        SolveTowersOfHanoi();
                    }
                    break;

                case SourcePegState.AUXILLARY:

                    // BEGIN, END
                    if (AuxStack.Count > 0)
                    {
                        if (BeginStack.Count == 0 || CanPush(AuxStack.Peek(), BeginStack.Peek()))
                        {
                            BeginStack.Push(AuxStack.Pop());
                            SolveTowersOfHanoi();
                        }
                        else if (EndStack.Count == 0 || CanPush(AuxStack.Peek(), EndStack.Peek()))
                        {
                            EndStack.Push(AuxStack.Pop());
                            SolveTowersOfHanoi();
                        }
                        else
                        {
                            PegState = SourcePegState.BEGIN;
                            SolveTowersOfHanoi();
                        }
                    }
                    else
                    {
                        PegState = SourcePegState.BEGIN;
                        SolveTowersOfHanoi();
                    }


                    break;
                case SourcePegState.END:
                    //Lock State
                    if (EndStack.Count > 0)
                    {
                        //max elem
                        if (EndStack.Peek() == 3 )//&& EndStack.Count == 1)
                        {
                            //state transition to Aux
                            PegState = SourcePegState.AUXILLARY;
                            SolveTowersOfHanoi();
                            return;
                        }
                    }

                    //AUX, BEGIN
                    if (EndStack.Count > 0)
                    {
                        if (AuxStack.Count == 0 || CanPush(EndStack.Peek(), AuxStack.Peek()))
                        {
                            AuxStack.Push(EndStack.Pop());
                            SolveTowersOfHanoi();
                        }
                        else if (BeginStack.Count == 0 || CanPush(EndStack.Peek(), BeginStack.Peek()))
                        {
                            BeginStack.Push(EndStack.Pop());
                            SolveTowersOfHanoi();
                        }
                        else
                        {
                            PegState = SourcePegState.BEGIN;
                            SolveTowersOfHanoi();
                        }
                    }
                    else
                    {
                        PegState = SourcePegState.BEGIN;
                        SolveTowersOfHanoi();
                    }


                    break;
                default:
                    break;
            }



        }

        private void InitializeStack()
        {
            BeginStack = new Stack<int>();
            AuxStack = new Stack<int>();
            EndStack = new Stack<int>();


            InitPushIntoBeginStack();
        }



        private void InitPushIntoBeginStack()
        {            
            BeginStack.Push(3);
            BeginStack.Push(2);
            BeginStack.Push(1);
        }

        bool CanPush(int source, int destination)
        {
            return (destination - source) == 1;
        }







    }
}
