using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiferuFramework
{
    /// <summary>
    /// 分步显示
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
                //生成物体
            }
            yield return null;
        }


        /// <summary>
        /// 显示所有对象
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
        /// 隐藏所有对象
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

