using UnityEngine;

public class AIController : ControllerBase
{
    private const float DecisionDelay = 1f;
    private float _decisionDelayCounter;
    private new void Update()
    {
        base.Update();

        if (_decisionDelayCounter < 0)
        {
            Decide();
            _decisionDelayCounter = DecisionDelay;
        }
        else
        {
            _decisionDelayCounter -= Time.deltaTime;
        }
        
    }

    private void Decide()
    {
        if (GetDistanceToPlayer() > 5)
        {
            Vertical = 1f;
            BasicAttackInput = false;
        }
        else
        {
            Vertical = 0;
            if (enemy.MovementState == MovementState.Died)
            {
                BasicAttackInput = false;
            }
            else
            {
                BasicAttackInput = true;
            }
        }
    }

    private float GetDistanceToPlayer()
    {
        Vector3 pos = transform.position;
        Vector3 enemyPos = enemy.transform.position;
        return (pos - enemyPos).magnitude;
    }
    
}
