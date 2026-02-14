namespace ArcadianEngine
{
    public class State
    {
        protected StateMachine? _ownerStateMachine = null;

        public virtual void Enter() { }
        
        public virtual void OnOwnerSet() { }

        public virtual void HandleInput() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Draw() { }

        public virtual void Exit() { }

        public void SetOwnerStateMachine(StateMachine owner)
        {
            this._ownerStateMachine = owner;
            OnOwnerSet();
        }
    }
}
