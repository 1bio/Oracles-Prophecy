using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class MonsterBehaviourMovement : MonsterBehaviour
{
    private CharacterController _characterController;

    private Monster _monster;
    private TargetDetector _targetDetector;

    private PointGrid _pointGrid;
    private List<PointNode> _neighborNodes;

    private List<PointNode> _path;
    private int _pathIndex = 0;

    public override void OnBehaviourStart(Monster monster)
    {
        _monster = monster;
        _targetDetector = monster.GetComponent<TargetDetector>();

        _pointGrid = monster.MovementController.PointGrid;
        _characterController = monster.MovementController.CharacterController;

        monster.AnimationController.PlayWalkAnimation();

        monster.CombatController.Health.ImpactEvent += OnImpact;
    }

    public override void OnBehaviourUpdate(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (monster.MovementController.Astar.OnHasTargetMoved() && !monster.MovementController.Astar.IsCalculating)
        {
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
        }

        if (monster == null || monster.MovementController.Path == null)
        {
            monster.StateMachineController.OnIdle();
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
            return;
        }

        if (_path != monster.MovementController.Path && monster.MovementController.Path.Count >= 1)
        {
            monster.AnimationController.PlayWalkAnimation();
            _path = monster.MovementController.Path;
            _pathIndex = 1;
        }

        if (_pathIndex < _path.Count)
        {
            monster.MovementController.StepToNode(_path[_pathIndex], monster, _pathIndex);

            if (_pointGrid.GetPointNodeFromGridByPosition(monster.transform.position) == _path[_pathIndex])
            {
                _pathIndex++;
            }
        }
    }

    public override void OnBehaviourEnd(Monster monster)
    {
        monster.AnimationController.PlayIdleAnimation();
        monster.CombatController.Health.ImpactEvent -= OnImpact;
    }

    private void OnImpact()
    {
        _monster.StateMachineController.OnGotHit();
    }

    
}