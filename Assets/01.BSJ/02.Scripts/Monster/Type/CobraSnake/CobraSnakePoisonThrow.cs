using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PoisonThrow", menuName = "Data/MonsterSKillData/CobraSnake/PoisonThrow")]
public class CobraSnakePoisonThrow : MonsterSkillData
{
    private enum PoisonThrowAnimationName
    {
        PoisonThrow
    }
    private CobraSnake _cobraSnake;

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
        _cobraSnake = (CobraSnake) monster;

        monster.ParticleController.VFXTransform["PoisonThrower"] = _cobraSnake.FirePositionTransform;

        _vfxTransform = monster.ParticleController.GetAvailableParticle("PoisonThrower").transform;
        _vfxTransform.position = _cobraSnake.FirePositionTransform.position;
        _vfxTransform.rotation = _cobraSnake.FirePositionTransform.rotation;

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
            BackDFS(monster, monster.transform.position, 5);
            monster.MovementController.Astar.StartPathCalculation(monster.transform.position, _targetNode.Position);
        }

        if (_hasMove)
        {
            if (!monster.AnimationController.AnimatorStateInfo.IsName(PoisonThrowAnimationName.PoisonThrow.ToString()))
            {
                monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

                if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
                {
                    _vfxTransform.position = _cobraSnake.FirePositionTransform.position;
                    _vfxTransform.rotation = _cobraSnake.FirePositionTransform.rotation;

                    monster.AnimationController.PlaySkillAnimation(PoisonThrowAnimationName.PoisonThrow.ToString());

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

    private void BackDFS(Monster monster, Vector3 monsterPosition, int max)
    {
        PointNode node = _grid.GetPointNodeFromGridByPosition(monsterPosition);

        if (node == null || max <= 0)
            return;

        _targetNode = node;

        Vector3 backwardPosition = _targetNode.Position - monster.transform.forward;

        BackDFS(monster, backwardPosition, max - 1);
    }
}
