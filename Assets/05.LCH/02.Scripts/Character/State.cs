using UnityEngine;

public abstract class State 
{
    public abstract void Enter(); // 상태 진입 시
    public abstract void Tick(float deltaTime); // 매 프레임마다 수행
    public abstract void Exit(); // 상태 종료 시
}
