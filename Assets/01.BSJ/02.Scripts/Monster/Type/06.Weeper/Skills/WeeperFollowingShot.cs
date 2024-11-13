using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowingShot", menuName = "Data/MonsterSKillData/Weeper/FollowingShot")]
public class WeeperFollowingShot : MonsterSkillData
{
    private Weeper _weeper;
    private Transform _vfxTransform;

    private PointGrid _grid;
    private PointNode _targetNode = null;

    private List<PointNode> _path = new List<PointNode>();
    private int _pathIndex = 0;

    private bool _hasAttacked = false;
    private bool _hasMove = false;
    private bool _isMoving = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _weeper = (Weeper) monster;

        monster.ParticleController.VFXTransform["Weeper Shot"] = _weeper.FirePositionTransform;

        _vfxTransform = monster.ParticleController.GetAvailableParticle("Weeper Shot").transform;
        _vfxTransform.position = _weeper.FirePositionTransform.position;
        _vfxTransform.rotation = _weeper.FirePositionTransform.rotation;

        _grid = monster.MovementController.PointGrid;
        _targetNode = null;

        _path = new List<PointNode>();
        _pathIndex = 0;
        _hasAttacked = false;
        _hasMove = false;
        _isMoving = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        if (_targetNode == null)
        {
            FindPathDFS(monster, monster.transform.position, 5);
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, _targetNode.Position);
        }

        if (_hasMove)
        {
            if (!monster.AnimationController.AnimatorStateInfo.IsName("Weeper Shot"))
            {
                monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

                if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
                {
                    _vfxTransform = monster.ParticleController.GetAvailableParticle("Weeper Shot").transform;
                    _vfxTransform.position = _weeper.FirePositionTransform.position;
                    _vfxTransform.rotation = _weeper.FirePositionTransform.rotation;

                    monster.AnimationController.PlaySkillAnimation("Weeper Shot");

                    _hasAttacked = true;
                }
            }
        }
        else
        {
            if (monster.SkillController.CurrentSkillData.Range / 2 >=
            Vector3.Distance(monster.MovementController.Astar.TargetTransform.position, monster.transform.position))
            {
                _isMoving = (_targetNode != _grid.GetPointNodeFromGridByPosition(monster.transform.position));
            }
            else
            {
                _hasMove = true;
            }

            if (_isMoving)
            {
                if (monster.MovementController.Path == null)
                {
                    monster.MovementController.Astar.StartPathCalculation(monster.transform.position, _targetNode.Position);
                }

                if (_path != monster.MovementController.Path && monster.MovementController.Path?.Count >= 1)
                {
                    monster.AnimationController.PlayWalkAnimation();
                    _path = monster.MovementController.Path;
                    _pathIndex = 1;
                }

                if (_pathIndex < _path?.Count)
                {
                    monster.MovementController.StepToNode(_path[_pathIndex], monster, _pathIndex);

                    if (_grid.GetPointNodeFromGridByPosition(monster.transform.position) == _path[_pathIndex])
                    {
                        _isMoving = (_targetNode != _grid.GetPointNodeFromGridByPosition(monster.transform.position));
                        _pathIndex++;
                    }
                }
            }

            _hasMove = monster.MovementController.Path != null && _isMoving ? false : true;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
        monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
    }


    private void FindPathDFS(Monster monster, Vector3 monsterPosition, int max)
    {
        BackDFS(monster, monster.transform.position, 5);

        if (_targetNode == _grid.GetPointNodeFromGridByPosition(monster.transform.position))
        {
            LeftDFS(monster, monster.transform.position, 5);
        }

        if (_targetNode == _grid.GetPointNodeFromGridByPosition(monster.transform.position))
        {
            RightDFS(monster, monster.transform.position, 5);
        }
    }

    private void BackDFS(Monster monster, Vector3 monsterPosition, int max)
    {
        PointNode node = _grid.GetPointNodeFromGridByPosition(monsterPosition);

        if (node == null || !node.IsGround || node.IsObstacle || max <= 0)
            return;

        _targetNode = node;

        Vector3 backwardPosition = _targetNode.Position - monster.transform.forward;

        BackDFS(monster, backwardPosition, max - 1);
    }

    private void LeftDFS(Monster monster, Vector3 monsterPosition, int max)
    {
        PointNode node = _grid.GetPointNodeFromGridByPosition(monsterPosition);

        if (node == null || !node.IsGround || node.IsObstacle || max <= 0)
            return;

        _targetNode = node;

        Vector3 backwardPosition = _targetNode.Position - monster.transform.right;

        LeftDFS(monster, backwardPosition, max - 1);
    }

    private void RightDFS(Monster monster, Vector3 monsterPosition, int max)
    {
        PointNode node = _grid.GetPointNodeFromGridByPosition(monsterPosition);

        if (node == null || !node.IsGround || node.IsObstacle || max <= 0)
            return;

        _targetNode = node;

        Vector3 backwardPosition = _targetNode.Position + monster.transform.right;

        RightDFS(monster, backwardPosition, max - 1);
    }
}
