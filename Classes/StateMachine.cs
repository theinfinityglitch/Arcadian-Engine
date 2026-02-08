namespace ArcadianEngine
{
    public class StateMachine
    {
        protected string _stateMachineName = "";
        protected Dictionary<string, State> _states = [];
        protected string _defaultStateName = "";
        protected string _currentStateName = "";

        public StateMachine(string stateMachineName)
        {
            this._stateMachineName = stateMachineName;
        }

        public void AddState(string stateName, State state)
        {
            if (_states.Count == 0)
            {
                _defaultStateName = stateName;
                _currentStateName = stateName;
            }

            _states.Add(stateName, state);
            state.SetOwnerStateMachine(this);
        }

        public int HandleStateMachine(float deltaTime)
        {
            if (_states.Count == 0)
            {
                Console.WriteLine($"No states found for {_stateMachineName}.");
                return 1;
            }

            _states[_currentStateName].HandleInput();
            _states[_currentStateName].Update(deltaTime);
            _states[_currentStateName].Draw();

            return 0;
        }

        public void ChangeState(string stateName)
        {
            if (stateName == _currentStateName || stateName == "" || !_states.ContainsKey(stateName))
            {
                return;
            }

            Console.WriteLine($"Set state {_stateMachineName}::{stateName}");

            _states[_currentStateName].Exit();
            _currentStateName = stateName;
            _states[_currentStateName].Enter();
        }
    }
}
