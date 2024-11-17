using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRiging : MonoBehaviour
{
    public Transform boneRoot; // 캐릭터 boneRoot
    public SkinnedMeshRenderer equipmentRenderer; // 착용 무기 스킨 메쉬 렌더러

    private void Start()
    {
        AttachEquipmentToCharacter(boneRoot, equipmentRenderer);
    }

    void AttachEquipmentToCharacter(Transform characterBoneRoot, SkinnedMeshRenderer equipmentRenderer)
    {
        Transform[] characterBones = characterBoneRoot.GetComponentsInChildren<Transform>(); // 캐릭터 boneRoot 하위 객체 가져오기
        Transform[] updatedBones = new Transform[equipmentRenderer.bones.Length]; // 장비의 스킨 매쉬 렌더러의 개수 만큼

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
