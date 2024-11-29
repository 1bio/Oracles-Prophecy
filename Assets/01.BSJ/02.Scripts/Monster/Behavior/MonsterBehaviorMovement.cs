using MasterRealisticFX;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class MonsterBehaviorMovement : MonsterBehavior
{
    private CharacterController _characterController;

    private Monster _monster;
    private TargetDetector _targetDetector;

    private PointGrid _pointGrid;
    private List<PointNode> _neighborNodes;

    private List<PointNode> _path;
    private int _pathIndex = 0;


    public override void OnBehaviorStart(Monster monster)
    {
        _monster = monster;
        _targetDetector = monster.GetComponent<TargetDetector>();

        _pointGrid = monster.MovementController.PointGrid;
        _characterController = monster.MovementController.CharacterController;

        _pathIndex = 0;
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (monster.MovementController.Astar.OnHasTargetMoved())
        {
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
        }

        if (monster.MovementController.Path == null)
        {
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, FindNeighborNode(_pointGrid.GetPointNodeFromGridByPosition(monster.MovementController.Astar.TargetTransform.position)).Position);
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
        else
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        }
    }

    public override void OnBehaviorEnd(Monster monster)
    {
        monster.AnimationController.PlayIdleAnimation();
    }

    private PointNode FindNeighborNode(PointNode node)
    {
        List<PointNode> neighborNodes = _pointGrid.GetNeighborNodes(node);
        PointNode findNode = null;

        foreach (PointNode neighborNode in neighborNodes)
        {
            if (neighborNode.IsGround && !neighborNode.IsObstacle)
            {
                findNode = neighborNode;
                break;
            }
        }
        return findNode;
    }
}