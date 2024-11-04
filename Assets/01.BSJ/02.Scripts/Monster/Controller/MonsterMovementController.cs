using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterMovementController
{
    public MonsterMovementController(TargetDetector targetDetector, Astar astar, PointGrid pointGrid, CharacterController characterController)
    {
        TargetDetector = targetDetector;
        Astar = astar;
        PointGrid = pointGrid;
        CharacterController = characterController;
    }

    public TargetDetector TargetDetector { get; private set; }
    public Astar Astar { get; private set; }
    public PointGrid PointGrid { get; private set; }
    public List<PointNode> Path { get => Astar.Path; }
    public Vector3 Direction { get; private set; }
    public CharacterController CharacterController { get; private set; }

    public void StepToNode(PointNode nextNode, Monster monster, int pathIndex)
    {
        Vector3 startNode = monster.transform.position;
        Vector3 targetNode = Path[pathIndex].Position;
        Vector3 direction = (targetNode - startNode).normalized;
        float speed = monster.CombatController.MonsterCombatAbility.MoveSpeed * monster.AnimationController.LocomotionBlendValue;

        LookAtNode(targetNode, monster.CombatController.MonsterCombatAbility.TurnSpeed);

        Vector3 newPosition = direction * speed * Time.deltaTime;
        CharacterController.Move(newPosition);
    }

    public void LookAtTarget(float rotationSpeed)
    {
        Vector3 targetPos = Astar.TargetTransform.position;
        targetPos.y = CharacterController.transform.position.y;

        Direction = (targetPos - CharacterController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(Direction);

        CharacterController.transform.rotation = Quaternion.RotateTowards(CharacterController.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public void LookAtNode(Vector3 targetNodePosition, float rotationSpeed)
    {
        targetNodePosition.y = CharacterController.transform.position.y;

        Vector3 forward = CharacterController.transform.forward.normalized;
        Vector3 direction = (targetNodePosition - CharacterController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction); 

        float angle = Mathf.Acos(Vector3.Dot(forward, direction)) * Mathf.Rad2Deg;
        //Debug.Log($"angle: {angle}, Direction: {Direction}");

        float currentRotationSpeed = (angle > 45) ? rotationSpeed : rotationSpeed * 0.25f;

        CharacterController.transform.rotation = Quaternion.RotateTowards(CharacterController.transform.rotation, lookRotation, currentRotationSpeed * Time.deltaTime);

        if (angle < 1f)
        {
            CharacterController.transform.rotation = lookRotation;
        }
    }
}
