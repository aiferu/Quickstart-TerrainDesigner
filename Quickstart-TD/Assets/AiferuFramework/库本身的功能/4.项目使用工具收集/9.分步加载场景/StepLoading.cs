using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiferuFramework
{
    /// <summary>
    /// �ֲ���ʾ
    /// </summary>
    public class StepLoading : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> LoadProfabList = new List<GameObject>();

        void Start()
        {
            StartCoroutine(LoadingProfab());
        }
        IEnumerator LoadingProfab()
        {
            yield return new WaitForSeconds(1.0f);
            foreach (var Profab in LoadProfabList)
            {
                Profab.gameObject.SetActive(true);
                yield return new WaitForSeconds(3.0f);
                //go.transform.position = Profab.transform.position;
                //��������
            }
            yield return null;
        }


        /// <summary>
        /// ��ʾ���ж���
        /// </summary>
        /// <param name="objects"></param>
        public void ShowAllProfab()
        {

            foreach (var obj in LoadProfabList)
            {
                if (obj == null)
                    return;
                obj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// �������ж���
        /// </summary>
        /// <param name="objects"></param>
        public void HideAllProfab()
        {
            foreach(var obj in LoadProfabList)
            {
                if (obj == null)
                    return;
                obj.gameObject.SetActive(false);
            }
        }
    }
}

