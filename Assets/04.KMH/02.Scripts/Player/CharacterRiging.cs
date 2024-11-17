using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRiging : MonoBehaviour
{
    public Transform boneRoot; // ĳ���� boneRoot
    public SkinnedMeshRenderer equipmentRenderer; // ���� ���� ��Ų �޽� ������

    private void Start()
    {
        AttachEquipmentToCharacter(boneRoot, equipmentRenderer);
    }

    void AttachEquipmentToCharacter(Transform characterBoneRoot, SkinnedMeshRenderer equipmentRenderer)
    {
        Transform[] characterBones = characterBoneRoot.GetComponentsInChildren<Transform>(); // ĳ���� boneRoot ���� ��ü ��������
        Transform[] updatedBones = new Transform[equipmentRenderer.bones.Length]; // ����� ��Ų �Ž� �������� ���� ��ŭ

        for (int i = 0; i < equipmentRenderer.bones.Length; i++)
        {
            string boneName = equipmentRenderer.bones[i].name;

            foreach (Transform characterBone in characterBones)
            {
                if (characterBone.name == boneName)
                {
                    updatedBones[i] = characterBone;
                    break;
                }
            }
        }

        equipmentRenderer.bones = updatedBones;

        equipmentRenderer.rootBone = characterBoneRoot;
    }
}
