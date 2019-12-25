public struct MoveState {
    public Move Move;
    public LevelState State;
    public MoveState(Move move, LevelState state) {
        Move = move;
        State = state;
    }
}