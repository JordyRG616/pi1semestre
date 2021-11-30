//. ESQUEMAS DE CONTROLE
public enum MovementControlScheme {None, WASD, Arrows}
public enum RotationControlScheme {None, QE, Mouse}

//.WAVE/FORMATION/ENEMY RELATED
public enum WaveType {Normal, Elite, Boss}
public enum EnemyType {Shooter}
public enum WigglePattern 
{
    HorizontalSine, 
    VerticalSine, 
    HorizontalCosine, 
    VerticalCosine, 
    ClockwiseCircle, 
    CounterClockwiseCircle
}

//. TURRET/REWARD RELATED
public enum RewardLevel {Common, Uncommon, Rare, Unique, Error}
public enum ActionStat {Damage, Rest, BulletSpeed, BurstSize, Projectiles, BulletSize, Duration, Capacity, DroneLevel}
public enum BaseEffectTrigger {Immediate, StartOfWave, EndOfWave, OnDestruction}

//. GAME STATE
public enum GameState {OnWave, OnReward}