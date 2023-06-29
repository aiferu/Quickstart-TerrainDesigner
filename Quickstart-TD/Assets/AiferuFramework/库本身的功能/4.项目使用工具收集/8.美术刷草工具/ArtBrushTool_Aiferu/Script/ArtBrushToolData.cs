//
//  InputMaterialTextureConfig.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/2/7.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace AiferuFramework.ArtBrushTool
{
    /// <summary>
    /// �洢����ˢ�ݹ��ߵĳ־û�����
    /// </summary>
    [CreateAssetMenu(fileName = "ArtBrushToolData", menuName = "cs",order = 7)]
    public class ArtBrushToolData : ScriptableObject
    {
        /// <summary>
        /// ����������Ӷ�������
        /// </summary>
        public static int PlantCount = 12;
        /// <summary>
        /// ��ˢ��С
        /// </summary>
        public float BrushSize;
        /// <summary>
        /// �ݶ��������С��Χ���ֵ
        /// </summary>
        public float ScaleRandomMax;
        /// <summary>
        /// �ݶ��������С��Χ��Сֵ
        /// </summary>
        public float ScaleRandomMin;
        /// <summary>
        /// ���ܶ�
        /// </summary>
        public float Density;
        /// <summary>
        /// Plant��������
        /// </summary>
        public GameObject[] Plants = new GameObject[PlantCount];
        /// <summary>
        /// SelectionGrid�����ѡ�ж�������,0��ʼ
        /// </summary>
        public int PlantSelect;
        /// <summary>
        /// ��ˢ�Ƿ��ǵ���ģʽ(�������ȥ�л�)
        /// </summary>
        public bool BrushIsAddMode = true;
        /// <summary>
        /// �Ƿ���Ҫ��ײ��
        /// </summary>
        public bool NeedCollider = true;
        /// <summary>
        /// �������常���������
        /// </summary>
        public GameObject MapDataObject;

        public Vector3 RotationMax;

        public Vector3 RotationMin;
    }
}
#endif