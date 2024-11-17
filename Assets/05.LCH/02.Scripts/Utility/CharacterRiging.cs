using UnityEngine;

public class CharacterRiging : MonoBehaviour
{
    public Transform boneRoot; // ĳ���� boneRoot
    public SkinnedMeshRenderer equipmentRenderer; // ���� ���� ��Ų �޽� ������

    public Transform handTransfor;
    public GameObject sword;

    private void Start()
    {
        AttachEquipmentToCharacter(boneRoot, equipmentRenderer);

        AttachSwordToCharacter(handTransfor, sword);
    }

    void AttachEquipmentToCharacter(Transform characterBoneRoot, SkinnedMeshRenderer equipmentRenderer)
    {
        Transform[] characterBones = characterBoneRoot.GetComponentsInChildren<Transform>(); // ĳ���� boneRoot ���� ��ü ��������
        Transform[] updatedBones = new Transform[equipmentRenderer.bones.Length]; // ����� ��Ų �Ž� �������� ���� ��ŭ 

        Debug.Log(equipmentRenderer.bones.Length);

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

    void AttachSwordToCharacter(Transform handTransform, GameObject weapon)
    {
        weapon.transform.SetParent(handTransform);
        weapon.transform.localPosition = handTransform.localPosition;
        weapon.transform.rotation = handTransform.localRotation;
    }
}
